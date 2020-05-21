
using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class LoginWnd : WindowRoot {
    public InputField iptAcct;
    public InputField iptPass;
    public Button btnEnter;
    public Button btnNotice;

    protected override void InitWnd() {
        base.InitWnd();

        //获取本地存储的账号密码
        if (PlayerPrefs.HasKey("Acct") && PlayerPrefs.HasKey("Pass")) {
            iptAcct.text = PlayerPrefs.GetString("Acct");
            iptPass.text = PlayerPrefs.GetString("Pass");
        }
        else {
            iptAcct.text = "";
            iptPass.text = "";
        }
    }


    /// <summary>
    /// click to enter game
    /// </summary>
    public void ClickEnterBtn() {
        audioSvc.PlayUIAudio(Constants.UILoginBtn);

        string _acct = iptAcct.text;
        string _pass = iptPass.text;
        if (_acct != "" && _pass != "") {
            //refresh acct pass which in local
            PlayerPrefs.SetString("Acct", _acct);
            PlayerPrefs.SetString("Pass", _pass);

            //send msg ,login request
            GameMsg msg = new GameMsg {
                cmd = (int)CMD.ReqLogin,
                reqLogin = new ReqLogin {
                    acct = _acct,
                    pass = _pass
                }
            };
            netSvc.SendMsg(msg);
        }
        else {
            GameRoot.AddTips("Plearse enter account or password");
        }
    }

    public void ClicKNoticeBtn() {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        GameRoot.AddTips("To be continue....");
    }
}