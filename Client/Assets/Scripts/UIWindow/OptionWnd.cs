using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionWnd : WindowRoot {
    //public static OptionWnd Instance = null;
    public Text txtCoin;
    private PlayerData pd;

    protected override void InitWnd () {
        //Instance = this;
        base.InitWnd();
        pd = GameRoot.Instance.PlayerData;
        RefreshUI();
    }

    public void ClickCloseWnd() {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        SetWndState(false);
    }

    public void RefreshUI() {
        pd = GameRoot.Instance.PlayerData;
        SetText(txtCoin, pd.coin);
    }

    public void ClickBuyCallBtn() {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        // send internet msg
        GameMsg msg = new GameMsg {
            cmd = (int)CMD.ReqBuyWithCoin,
            reqBuyWithCoin = new ReqBuyWithCoin {
                type = 1,
                cost = 100,
            }
        };

        netSvc.SendMsg(msg);
        //btnSure.interactable = false;
        RefreshUI();
    }
    public void ClickBuyPutBtn () {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        // send internet msg
        GameMsg msg = new GameMsg {
            cmd = (int)CMD.ReqBuyWithCoin,
            reqBuyWithCoin = new ReqBuyWithCoin {
                type = 1,
                cost = 100,
            }
        };

        netSvc.SendMsg(msg);
        //btnSure.interactable = false;
        RefreshUI();
    }
    public void ClickSellCallBtn () {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        // send internet msg
        GameMsg msg = new GameMsg {
            cmd = (int)CMD.ReqBuyWithCoin,
            reqBuyWithCoin = new ReqBuyWithCoin {
                type = 2,
                cost = -100,
            }
        };

        netSvc.SendMsg(msg);
        //btnSure.interactable = false;
        RefreshUI();
    }
    public void ClickSellPutBtn () {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        // send internet msg
        GameMsg msg = new GameMsg {
            cmd = (int)CMD.ReqBuyWithCoin,
            reqBuyWithCoin = new ReqBuyWithCoin {
                type = 3,
                cost = -100,
            }
        };

        netSvc.SendMsg(msg);
        //btnSure.interactable = false;
        RefreshUI();
    }


}
