using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideWnd: WindowRoot {
    public Text txtName;
    public Text txtTalk;
    public Image imgIcon;

    private PlayerData pd;
    private AutoGuideCfg curtTaskData;
    private string[] dialogArr;
    private int index; // dialog index 
    protected override void InitWnd () {
        base.InitWnd();

        pd = GameRoot.Instance.PlayerData;
        curtTaskData = MainCitySys.Instance.GetCurtTaskData();
        dialogArr = curtTaskData.dialogArr.Split('#');
        index = 1;

        SetTalk();
    }

    private void SetTalk () {
        string[] talkArr = dialogArr[index].Split('|');
        if (talkArr[0] == "0") {
            // player talks
            SetSprite(imgIcon, PathDefine.SelfIcon);
            SetText(txtName, pd.name);
        } else {
            // npc talks
            switch (curtTaskData.npcID) {
                case 0:
                    SetSprite(imgIcon, PathDefine.WiseManIcon);
                    SetText(txtName, "Wiser");
                    break;
                case 1:
                    SetSprite(imgIcon, PathDefine.GeneralIcon);
                    SetText(txtName, "General");
                    break;
                case 2:
                    SetSprite(imgIcon, PathDefine.ArtisanIcon);
                    SetText(txtName, "Artisan");
                    break;
                case 3:
                    SetSprite(imgIcon, PathDefine.TraderIcon);
                    SetText(txtName, "Trader");
                    break;
                default:
                    SetSprite(imgIcon, PathDefine.GuideIcon);
                    SetText(txtName, "Guider");
                    break;
            }
        }
        imgIcon.SetNativeSize();
        SetText(txtTalk, talkArr[1].Replace("$name",pd.name));
    }
    public void ClickNextBtn() {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        index += 1;
        if (index == dialogArr.Length) {
            // the dialog is finished, and send a meg to complete
            GameMsg msg = new GameMsg {
                cmd = (int)CMD.ReqGuide,
                reqGuide = new ReqGuide {
                    guideid = curtTaskData.ID
                }
            };
            netSvc.SendMsg(msg);
            SetWndState(false);
        } else {
            SetTalk();
        }
    }
}
