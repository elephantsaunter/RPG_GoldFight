using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocol;

public class BattleEndWnd : WindowRoot {
    #region UI Define
    public Transform rewardTrans;
    public Transform sweobject;
    public Button btnClose;
    public Button btnExit;
    public Button btnSure;
    public Animation ani;

    // This part for SWE Project show
    public Text txtFrage;
    public Text txtContent1;
    public Text txtContent2;
    public Text txtContent3;
    public Text txtDiamond;
    public Text txtPower;
    private PlayerData pd;

    #endregion
    private BattleEndType endType = BattleEndType.None;
    protected override void InitWnd () {
        base.InitWnd();
        RefreshUI();
    }
    private void RefreshUI() {
        switch(endType) {
            case BattleEndType.Pause:
                SetActive(sweobject, false);
                SetActive(rewardTrans, false);
                SetActive(btnExit.gameObject);
                SetActive(btnClose.gameObject);
                break;
            case BattleEndType.Win:
                SetActive(sweobject, true);
                SetActive(rewardTrans, true);
                SetActive(btnExit.gameObject,false);
                SetActive(btnClose.gameObject,false);
                MapCfg cfg = resSvc.GetMapCfg(bid);
                int min = time / 60;
                int sec = time % 60;
                int coin = cfg.coin;
                int exp = cfg.exp;
                int crystal = cfg.crystal;

                pd = GameRoot.Instance.PlayerData;
                if (pd.mission == 10002) {
                    SetText(txtFrage, "Level 1: Voruntersuchung");
                    SetText(txtContent1, "Was sind drei Aufgaben in Level 1");
                    SetText(txtContent2, "A.Persona, Rahmenbedingungen und User Needs");
                    SetText(txtContent3, "B.V-Model, Wasserfall und XP");
                } else if (pd.mission == 10003) {
                    SetText(txtFrage, "Level 2: Analyse");
                    SetText(txtContent1, "Welche Aussage ist richtig über Wasserfallmodell");
                    SetText(txtContent2, "A. Das Wasserfallmodell bietet Kontrollpunkte, die nach Phasen für das Projekt unterteilt sind.");
                    SetText(txtContent3, "B. Das Wasserfallmodell kann nicht auf das iterative Modell angewendet werden.");
                } else if (pd.mission == 10004) {
                    SetText(txtFrage, "Level 3: Design");
                    SetText(txtContent1, "Welche Aussage ist richtig?");
                    SetText(txtContent2, "A. MVC steht für Model View Controller");
                    SetText(txtContent3, "B. MVVM  Micro Version View Model");

                } else if (pd.mission == 10005) {
                    SetText(txtFrage, "Level 4: Implementierung");
                    SetText(txtContent1, "Welche gehört zu Clean Code nicht?");
                    SetText(txtContent2, "A. Code-Verdopplungen");
                    SetText(txtContent3, "B. Formatierung und Kommentare");

                } else if (pd.mission == 10006) {
                    SetText(txtFrage, "Level 5: Test");
                    SetText(txtContent1, "Welche Testmethode ist wichtig?");
                    SetText(txtContent2, "A. Smoketest und Sicherheitstest");
                    SetText(txtContent3, "B. Formatierung und Kommentare");

                } else if (pd.mission == 10007) {
                    SetText(txtFrage, "Level 6: Inbetriebnahme");
                    SetText(txtContent1, "Was ist die richtige Reihenfolge der CI/CD");
                    SetText(txtContent2, "A. continuous integration, continuous delivery und continuous deployment");
                    SetText(txtContent3, "B. continuous integration, continuous deployment und continuous delivery");

                } else if (pd.mission == 10008) {
                    SetText(txtFrage, "Level 7: Wartung");
                    SetText(txtContent1, "Welche Aussage über Erweiterung ist richtig?");
                    SetText(txtContent2, "A. Gewichtskontrolle für Fitness App, neues Bezahlungssystem für Online Shop");
                    SetText(txtContent3, "B. neues Bezahlungssystem für Fitness App, Gewichtskontrolle für Online Shop");

                } else if (pd.mission == 10009) {
                    SetText(txtFrage, "Level 8: Migration ");
                    SetText(txtContent1, "Level 1: Voruntersuchung");

                } else {
                    SetText(txtFrage, "xxx ");
                    SetText(txtContent1, "xxx");
                }

                /** This part is battle end part
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
                **/
                break;
            case BattleEndType.Lose:
                SetActive(rewardTrans, false);
                SetActive(sweobject, false);
                SetActive(btnExit.gameObject);
                SetActive(btnClose.gameObject,false);
                audioSvc.PlayUIAudio(Constants.missionFail);
                break;
            case BattleEndType.Info:
                SetActive(sweobject, true);
                SetActive(rewardTrans, false);
                // SetActive(btnExit.gameObject);
                SetActive(btnClose.gameObject);
                break;
            case BattleEndType.First:
                break;
        }
    }
    public void ClickClose() {
        SetActive(sweobject, false);
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        BattleSys.Instance.battleMgr.isPauseGame = false;
        SetWndState(false);
    }
    public void ClickWeiter() {
        SetActive(sweobject, false);
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
    public void ClickSureBtn () {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        // enter city and win and open battle display to select next battle
        GameRoot.AddTips("Prima, Korrekte Antwort");
        MainCitySys.Instance.EnterMainCity();
        BattleSys.Instance.DestroyBattle();
        MissionSys.Instance.EnterMission();
    }

    public void ClickWrongBtn () {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        // enter city and win and open battle display to select next battle
        GameRoot.AddTips("Sie haben falsch beantwortet");
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
    Lose,
    // for SWE Prasentation
    Info,
    First,
    Secend,
    Third
}
