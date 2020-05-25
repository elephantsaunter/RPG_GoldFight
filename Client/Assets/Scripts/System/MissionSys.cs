using PEProtocol;

public class MissionSys :SystemRoot{
    public static MissionSys Instance = null;
    public MissionWnd missionWnd;
    public OptionWnd optionWnd;
    public LifeWnd lifeWnd;
    public override void InitSys () {
        base.InitSys();
        Instance = this;
        PECommon.Log("Init MissionSys...");
    }

    public void EnterMission() {
        SetMissionWndState();
    }

    #region Mission Wnd
    public void SetMissionWndState(bool isActive = true) {
        missionWnd.SetWndState(isActive);
    }
    #endregion

    public void RspMission(GameMsg msg) {
        GameRoot.Instance.SetPlayerDataByMission(msg.rspMission);
        MainCitySys.Instance.mainCityWnd.SetWndState(false);
        SetMissionWndState(false);
        BattleSys.Instance.StartBattle(msg.rspMission.fbid);
    }
    #region Option Wnd
    public void OpenOptionWnd () {
        optionWnd.SetWndState(true);
    }

    public void RspBuyWithCoin(GameMsg msg) {
        RspBuyWithCoin data = msg.rspBuyWithCoin;
        GameRoot.Instance.SetPlayerDataByBuyWithCoin(data);
        optionWnd.RefreshUI();
        //optionWnd.SetWndState(false);
        //buyWnd.btnSure.interactable = false;
    }
    #endregion

    #region LifeWnd
    public void OpenLifeWnd () {
        lifeWnd.SetWndState(true);
    }
    #endregion

}
