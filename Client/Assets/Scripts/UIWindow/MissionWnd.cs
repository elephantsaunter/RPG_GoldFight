using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionWnd : WindowRoot{
    public Button[] missionBtnArr;
    public Transform pointerTrans;
    private PlayerData pd;
    protected override void InitWnd () {
        base.InitWnd();
        pd = GameRoot.Instance.PlayerData;
        RefreshUI();
    }

    public void RefreshUI() {
        int fbid = pd.mission;
        for(int i = 0;i < missionBtnArr.Length;i++) {
            if(i < fbid % 10000) {
                SetActive(missionBtnArr[i].gameObject);
                if (i == fbid % 10000 - 1) {
                    pointerTrans.SetParent(missionBtnArr[i].transform);
                    pointerTrans.transform.localPosition = new Vector3(25, 100, 0);
                }
            } else {
                SetActive(missionBtnArr[i].gameObject,false);
            }
        }
    }
    
    public void  ClickOptionBtn() {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        MissionSys.Instance.OpenOptionWnd();
    }

    public void ClickLifeBtn () {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        MissionSys.Instance.OpenLifeWnd();
    }
    public void ClickTaskBtn(int fbid) {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        // check power if satisfied
        int power = resSvc.GetMapCfg(fbid).power;
        if(pd.mission == 10006) {
            GameRoot.AddTips("Congratulatons, you have done all missions");
        }else if (power > pd.power) {
            GameRoot.AddTips("Power is not enough to start a battle");
        } else {
            // send msg
            netSvc.SendMsg(new GameMsg {
                cmd = (int)CMD.ReqMission,
                reqMission = new ReqMission{
                    fbid = fbid
                }
            });
        }
    }
    public void ClickCloseBtn() {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }
}
