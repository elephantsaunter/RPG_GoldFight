
using PEProtocol;

public class LoginSys : SystemRoot {
    public static LoginSys Instance = null;

    public LoginWnd loginWnd;
    public CreateWnd createWnd;

    public override void InitSys() {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init LoginSys...");
    }

    /// <summary>
    /// enter login screen
    /// </summary>
    public void EnterLogin() {
        //asy-load login screen
        //show load progress procent
        resSvc.AsyncLoadScene(Constants.SceneLogin, () => {
            //after load progress, open register login display
            loginWnd.SetWndState();
            audioSvc.PlayBGMusic(Constants.BGLogin);
        });
    }

    public void RspLogin(GameMsg msg) {
        GameRoot.AddTips("login success");
        GameRoot.Instance.SetPlayerData(msg.rspLogin);

        if (msg.rspLogin.playerData.name == "") {
            //open create role display
            createWnd.SetWndState();
        }
        else {
            //enter main city
            MainCitySys.Instance.EnterMainCity();
        }
        //close login display
        loginWnd.SetWndState(false);
    }

    public void RspRename(GameMsg msg) {
        GameRoot.Instance.SetPlayerName(msg.rspRename.name);

        //TODO
        //change screen to main city
        //open main city display
        MainCitySys.Instance.EnterMainCity();

        //close create display
        createWnd.SetWndState(false);
    }
}