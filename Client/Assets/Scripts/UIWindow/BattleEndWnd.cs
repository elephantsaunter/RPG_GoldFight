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
    public Animation ani;
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
                SetActive(rewardTrans, false);
                SetActive(btnExit.gameObject,false);
                SetActive(btnClose.gameObject,false);
                MapCfg cfg = resSvc.GetMapCfg(bid);
                int min = time / 60;
                int sec = time % 60;
                int coin = cfg.coin;
                int exp = cfg.exp;
                int crystal = cfg.crystal;

                SetText(txtTime, "Time: " + min + " min: " + sec + "secs");
                SetText(txtRestHP, "RestHP: " + resthp);
                SetText(txtReward, "Reward: " + Constants.Color(coin+"coin",TxtColor.Green) + " exp: " + exp + "crystal: " + crystal);
                timerSvc.AddTimerTask((int tid) => {
                    SetActive(rewardTrans);
                    ani.Play();
                    timerSvc.AddTimerTask((int tid1) => {
                        audioSvc.PlayUIAudio(Constants.dungeonItemEnter);
                        timerSvc.AddTimerTask((int tid2) => {
                            audioSvc.PlayUIAudio(Constants.dungeonItemEnter);
                            timerSvc.AddTimerTask((int tid3) => {
                                audioSvc.PlayUIAudio(Constants.dungeonItemEnter);
                                timerSvc.AddTimerTask((int tid4) => {
                                    audioSvc.PlayUIAudio(Constants.LogonEnter);
                                }, 650);
                            }, 650);
                        }, 650);
                    }, 650);
                }, 1000);
                break;
            case BattleEndType.Lose:
                SetActive(rewardTrans, false);
                SetActive(btnExit.gameObject);
                SetActive(btnClose.gameObject,false);
                audioSvc.PlayUIAudio(Constants.missionFail);
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
        MainCitySys.Instance.EnterMainCity();
        BattleSys.Instance.DestroyBattle();
        MissionSys.Instance.EnterMission();
    }
    public void SetWndType(BattleEndType endType) {
        this.endType = endType;
    }
    private int bid;
    private int time;
    private int resthp;

    public void SetBattleEndData(int bid, int time, int resthp) {
        this.bid = bid;
        this.time = time;
        this.resthp = resthp;
    }
}

public enum BattleEndType {
    None,
    Pause,
    Win,
    Lose
}
