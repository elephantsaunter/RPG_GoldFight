using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEndWnd : WindowRoot {
    #region UI Define
    public Transform rewardTrans;
    public Button btnClose;
    public Button btnExit;
    public Button btnSure;
    public Text txtTime;
    public Text txtRestHP;
    public Text txtReward;
    #endregion
    private BattleEndType endType = BattleEndType.None;
    protected override void InitWnd () {
        base.InitWnd();
        RefreshUI();
    }
    private void RefreshUI() {
        switch(endType) {
            case BattleEndType.Pause:
                SetActive(rewardTrans, false);
                SetActive(btnExit.gameObject);
                SetActive(btnClose.gameObject);
                break;
            case BattleEndType.Win:
                break;
            case BattleEndType.Lose:
                SetActive(rewardTrans, false);
                SetActive(btnExit.gameObject);
                SetActive(btnClose.gameObject,false);
                break;
        }
    }
    public void ClickClose() {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        BattleSys.Instance.battleMgr.isPauseGame = false;
        SetWndState(false);
    }
    public void ClickExitBtn() {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        // enter city and fail
        MainCitySys.Instance.EnterMainCity();
        BattleSys.Instance.DestroyBattle();
    }
    public void ClickSureBtn() {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        // enter city and win and open battle display to select next battle
    }
    public void SetWndType(BattleEndType endType) {
        this.endType = endType;
    }
}

public enum BattleEndType {
    None,
    Pause,
    Win,
    Lose
}
