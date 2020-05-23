using NUnit.Framework;
using UnityEngine;

public class BattleSys: SystemRoot {
    public static BattleSys Instance = null;
    public PlayerCtrlWnd playerCtrlWnd;
    public BattleEndWnd battleEndWnd;
    public BattleMgr battleMgr;


    public override void InitSys () {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init BattleSys...");
    }

    public void StartBattle(int mapid) {
        GameObject go = new GameObject {
            name = "BattleRoot"
        };

        go.transform.SetParent(GameRoot.Instance.transform);
        battleMgr = go.AddComponent<BattleMgr>();
        battleMgr.Init(mapid);
        SetPlayerCtrlWndState();
    }
    public void EndBattle(int restHP) {
        playerCtrlWnd.SetWndState(false);
        GameRoot.Instance.dynamicWnd.RmvAllHPInfo();
        if (restHP > 0) {
            // win, send end fight request
            // TODO
        } else {
            SetBattleEndWndState(BattleEndType.Lose);
        }
    }
    public void DestroyBattle() {
        SetPlayerCtrlWndState(false);
        SetBattleEndWndState(BattleEndType.None, false);
        GameRoot.Instance.dynamicWnd.RmvAllHPInfo();
        Destroy(battleMgr.gameObject);
    }
    public void SetPlayerCtrlWndState(bool isActive = true) {
        playerCtrlWnd.SetWndState(isActive);
    }
    public void SetBattleEndWndState(BattleEndType endType, bool isActive = true) {
        battleEndWnd.SetWndType(endType);
        battleEndWnd.SetWndState(isActive);
    }
    public void SetMoveDir (Vector2 dir) {
        battleMgr.SetSelfPlayerMoveDir(dir);
    }

    public void ReqReleaseSkill (int index) {
        battleMgr.ReqReleaseSkill(index);
    }

    public Vector2 GetDirInput() {
        return playerCtrlWnd.currentDir;
    }
}
