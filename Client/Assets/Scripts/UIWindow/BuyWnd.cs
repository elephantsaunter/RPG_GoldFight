using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyWnd : WindowRoot {
    public Text txtInfo;
    public Button btnSure;
    private int buyType; // 0 power,1 coin
    public void SetBuyType(int type) {
        this.buyType = type;
    }
    protected override void InitWnd () {
        base.InitWnd();
        btnSure.interactable = true;
        RefreshUI();
    }

    private void RefreshUI() {
         switch(buyType) {
            case 0:
                // power
                txtInfo.text = "cost" + Constants.Color(" 10 diamond ", TxtColor.Red) + "buy" + Constants.Color(" 100 Power ", TxtColor.Red) + "?";
                break;
            case 1:
                // coin
                txtInfo.text = "cost" + Constants.Color(" 10 diamond ", TxtColor.Red) + "buy" + Constants.Color(" 1000 coin ", TxtColor.Red) + "?";
                break;
        }
    }

    public void ClickSureBtn() {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        // send internet msg
        GameMsg msg = new GameMsg {
            cmd = (int)CMD.ReqBuy,
            reqBuy = new ReqBuy {
                type = buyType,
                cost = 10
            }
        };

        netSvc.SendMsg(msg);
        //btnSure.interactable = false;
    }

    public void ClickCloseBtn () {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }
}
