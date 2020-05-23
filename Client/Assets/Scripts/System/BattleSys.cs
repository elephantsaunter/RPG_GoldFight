using UnityEngine;

public class BattleSys: SystemRoot {
    public static BattleSys Instance = null;
    public PlayerCtrlWnd playerCtrlWnd;
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
    }
    public void SetPlayerCtrlWndState(bool isActive = true) {
        playerCtrlWnd.SetWndState(isActive);
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
