using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class CreateWnd : WindowRoot {
    public InputField iptName;

    protected override void InitWnd() {
        base.InitWnd();

        //show a random name
        iptName.text = resSvc.GetRDNameData(false);
    }

    public void ClickRandBtn() {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        string rdName = resSvc.GetRDNameData(false);// im woman
        iptName.text = rdName;
    }

    public void ClickEnterBtn() {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        if (iptName.text != "" ) {
            //send name data to server, and enter main city
            GameMsg msg = new GameMsg {
                cmd = (int)CMD.ReqRename,
                reqRename = new ReqRename {
                    name = iptName.text
                }
            };
            netSvc.SendMsg(msg);
        }
        else {
            GameRoot.AddTips("This name is illegal");
        }
    }
}