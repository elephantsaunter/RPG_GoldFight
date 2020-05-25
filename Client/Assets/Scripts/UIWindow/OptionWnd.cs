using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionWnd : WindowRoot {
    //public static OptionWnd Instance = null;
    public Text txtCoin;
    private PlayerData pd;

    #region skill1
    public Image imgSk1CD;
    private bool isSk1CD = false; // flag whether you should wait
    private float sk1CDTime; // CD Time
    private int sk1Num; // the second number ist shown in txtSk1CD
    private float sk1FillCount = 0;
    private float sk1NumCount = 0;
    #endregion

    #region skill2
    public Image imgSk2CD;
    private bool isSk2CD = false; // flag whether you should wait
    private float sk2CDTime; // CD Time
    private int sk2Num; // the second number ist shown in txtSk1CD
    private float sk2FillCount = 0;
    private float sk2NumCount = 0;
    #endregion

    #region skill3
    public Image imgSk3CD;
    private bool isSk3CD = false; // flag whether you should wait
    private float sk3CDTime; // CD Time
    private int sk3Num; // the second number ist shown in txtSk1CD
    private float sk3FillCount = 0;
    private float sk3NumCount = 0;
    #endregion

    #region skill4
    public Image imgSk4CD;
    private bool isSk4CD = false; // flag whether you should wait
    private float sk4CDTime; // CD Time
    private int sk4Num; // the second number ist shown in txtSk1CD
    private float sk4FillCount = 0;
    private float sk4NumCount = 0;
    #endregion
    protected override void InitWnd () {
        //Instance = this;
        base.InitWnd();
        pd = GameRoot.Instance.PlayerData;
        sk1CDTime = 2000 / 1000.0f;
        sk2CDTime = 10000 / 1000.0f;
        sk3CDTime = 30000 / 1000.0f;
        sk4CDTime = 60000 / 1000.0f;
        RefreshUI();
    }
    private void Update() {
        float delta = Time.deltaTime;
        if (isSk1CD) {
            // cannot release skill
            sk1FillCount += delta;
            if (sk1FillCount >= sk1CDTime) {
                // wait time is enough
                isSk1CD = false;
                SetActive(imgSk1CD, false);
                sk1FillCount = 0;
            } else {
                // wait time is not enough
                imgSk1CD.fillAmount = 1 - sk1FillCount / sk1CDTime;
            }
            // every second do a refresh in txtSk1CD
            sk1NumCount += delta;
        }
        if (isSk2CD) {
            // cannot release skill
            sk2FillCount += delta;
            if (sk2FillCount >= sk2CDTime) {
                // wait time is enough
                isSk2CD = false;
                SetActive(imgSk2CD, false);
                sk2FillCount = 0;
            } else {
                // wait time is not enough
                imgSk2CD.fillAmount = 1 - sk2FillCount / sk2CDTime;
            }
            // every second do a refresh in txtSk1CD
            sk2NumCount += delta;
        }
        if (isSk3CD) {
            // cannot release skill
            sk3FillCount += delta;
            if (sk3FillCount >= sk3CDTime) {
                // wait time is enough
                isSk3CD = false;
                SetActive(imgSk3CD, false);
                sk3FillCount = 0;
            } else {
                // wait time is not enough
                imgSk3CD.fillAmount = 1 - sk3FillCount / sk3CDTime;
            }
            // every second do a refresh in txtSk1CD
            sk3NumCount += delta;
        }
        if (isSk4CD) {
            // cannot release skill
            sk4FillCount += delta;
            if (sk4FillCount >= sk4CDTime) {
                // wait time is enough
                isSk4CD = false;
                SetActive(imgSk4CD, false);
                sk4FillCount = 0;
            } else {
                // wait time is not enough
                imgSk4CD.fillAmount = 1 - sk4FillCount / sk4CDTime;
            }
            // every second do a refresh in txtSk1CD
            sk4NumCount += delta;
        }
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
        if(isSk1CD == false) {
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            int result = PETools.RDInt(-50, 50);
            if(result >= 0) {
                GameRoot.AddTips("Gluckwunsch! You get " + result + " coin");
            } else {
                GameRoot.AddTips("Shade! You lose " + result + " coin");
            }
            // send internet msgpur
            GameMsg msg = new GameMsg {
                cmd = (int)CMD.ReqBuyWithCoin,
                reqBuyWithCoin = new ReqBuyWithCoin {
                    type = 1,
                    cost = -result,
                }
            };
            netSvc.SendMsg(msg);
            isSk1CD = true;
            SetActive(imgSk1CD);
            imgSk1CD.fillAmount = 1;
            //btnSure.interactable = false;
            RefreshUI();
        }
    }
    public void ClickBuyPutBtn () {
        if (isSk2CD == false) {
            GameRoot.AddTips("2");
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            // send internet msg
            GameMsg msg = new GameMsg {
                cmd = (int)CMD.ReqBuyWithCoin,
                reqBuyWithCoin = new ReqBuyWithCoin {
                    type = 1,
                    cost = -100,
                }
            };
            netSvc.SendMsg(msg);
            isSk2CD = true;
            SetActive(imgSk2CD);
            imgSk2CD.fillAmount = 1;
            //btnSure.interactable = false;
            RefreshUI();
        }
    }
    
    public void ClickSellCallBtn () {
        if (isSk3CD == false) {
            GameRoot.AddTips("3");
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            // send internet msg
            GameMsg msg = new GameMsg {
                cmd = (int)CMD.ReqBuyWithCoin,
                reqBuyWithCoin = new ReqBuyWithCoin {
                    type = 1,
                    cost = -300,
                }
            };
            netSvc.SendMsg(msg);
            isSk3CD = true;
            SetActive(imgSk3CD);
            imgSk3CD.fillAmount = 1;
            //btnSure.interactable = false;
            RefreshUI();
        }
    }
    public void ClickSellPutBtn () {
        if (isSk4CD == false) {
            GameRoot.AddTips("4");
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            // send internet msg
            GameMsg msg = new GameMsg {
                cmd = (int)CMD.ReqBuyWithCoin,
                reqBuyWithCoin = new ReqBuyWithCoin {
                    type = 1,
                    cost = 600,
                }
            };
            netSvc.SendMsg(msg);
            isSk4CD = true;
            SetActive(imgSk4CD);
            imgSk4CD.fillAmount = 1;
            //btnSure.interactable = false;
            RefreshUI();
        }
    }


}