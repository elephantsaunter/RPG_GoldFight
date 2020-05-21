using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class StrongWnd : WindowRoot {
    #region UI Define
    public Image imgCurtPos;
    public Text txtStarLv;
    public Transform starTransGrp;
    public Text propHp1;
    public Text propHurt1;
    public Text propDef1;
    public Text propHP2;
    public Text propHurt2;
    public Text propDef2;
    public Image propArr1;
    public Image propArr2;
    public Image propArr3;

    public Text txtNeedLv;
    public Text txtCostCoin;
    public Text txtCostCrystal;

    public Transform costTransRoot;
    public Text txtCoin;
    #endregion
    #region DataArea
    public Transform posBtnTrans;
    private Image[] imgs = new Image[6];
    private int currentIndex;
    private PlayerData pd;
    StrongCfg nextSc;
    //StrongCfg nextSc = resSvc.GetStrongCfg(currentIndex, nextStarLv);
    #endregion
    protected override void InitWnd () {
        base.InitWnd();
        pd = GameRoot.Instance.PlayerData;
        RegClickEvts();
        ClickPosItem(0); // default display is the first block(head)
    }

    private void RegClickEvts() {
        for(int i = 0;i<posBtnTrans.childCount;i++) {
            Image img = posBtnTrans.GetChild(i).GetComponent<Image>();
            OnClick(img.gameObject, (object args) => {
                ClickPosItem((int)args);
                audioSvc.PlayUIAudio(Constants.UIClickBtn);
            },i);
            imgs[i] = img;
        }
    }

    private void ClickPosItem(int index) {
        currentIndex = index;
        for(int i = 0;i<imgs.Length;i++) {
            Transform trans = imgs[i].transform;
            if(i == currentIndex) {
                //this block is current block
                SetSprite(imgs[i], PathDefine.ItemArrorBG);
                trans.localPosition = new Vector3(10, trans.localPosition.y, 0);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 95);
            }
            else {
                SetSprite(imgs[i], PathDefine.ItemPlatBG);
                trans.localPosition = new Vector3(0, trans.localPosition.y, 0);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(220, 85);
            }
        }

        RefreshItem();
    }

    // every time ,click left block(head or body...) then refresh right detail display
    private void RefreshItem() {
        // Coin
        SetText(txtCoin, pd.coin);
        // Select the position
        switch(currentIndex) {
            case 0:
                SetSprite(imgCurtPos, PathDefine.ItemHead);
                break;
            case 1:
                SetSprite(imgCurtPos, PathDefine.ItemBody);
                break;
            case 2:
                SetSprite(imgCurtPos, PathDefine.ItemBelt);
                break;
            case 3:
                SetSprite(imgCurtPos, PathDefine.ItemHand);
                break;
            case 4:
                SetSprite(imgCurtPos, PathDefine.ItemLeg);
                break;
            case 5:
                SetSprite(imgCurtPos, PathDefine.ItemFoot);
                break;
        }
        SetText(txtStarLv, pd.strongArr[currentIndex] + "  STARS");

        int curtStarLv = pd.strongArr[currentIndex];
        for(int i = 0;i<starTransGrp.childCount;i++) {
            Image img = starTransGrp.GetChild(i).GetComponent<Image>();
            if(i<curtStarLv) {
                // show star
                SetSprite(img, PathDefine.SpStar2);
            } else {
                // not show star
                SetSprite(img, PathDefine.SpStar1);
            }
        }
        int nextStarLv = curtStarLv + 1;

        int sumAddHp = resSvc.GetPropAddValPreLv(currentIndex, nextStarLv, 1);
        int sumAddHurt = resSvc.GetPropAddValPreLv(currentIndex, nextStarLv, 2);
        int sumAddDef =  resSvc.GetPropAddValPreLv(currentIndex, nextStarLv, 3);

        SetText(propHp1, "HP + " + sumAddHp);
        SetText(propHurt1, "Hurt + " + sumAddHurt);
        SetText(propDef1, "Def + " + sumAddDef);

        nextSc = resSvc.GetStrongCfg(currentIndex, nextStarLv);
        if (nextSc != null) {
            SetActive(propHP2);
            SetActive(propHurt2);
            SetActive(propDef2);

            SetActive(costTransRoot);
            SetActive(propArr1);
            SetActive(propArr2);
            SetActive(propArr3);

            SetText(propHP2, "Aft Enhance + " + nextSc.addhp);
            SetText(propHurt2, "         + " + nextSc.addhurt);
            SetText(propDef2, "         + " + nextSc.adddef);

            SetText(txtNeedLv, "LV req. :                      " + nextSc.minlv);
            SetText(txtCostCoin, "Res. req.            " + nextSc.coin);
            SetText(txtCostCrystal, nextSc.crystal + "/" + pd.crystal);
        }
        else {
            SetActive(propHP2, false);
            SetActive(propHurt2, false);
            SetActive(propDef2, false);

            SetActive(costTransRoot, false);
            SetActive(propArr1, false);
            SetActive(propArr2, false);
            SetActive(propArr3, false);

        }
    }
    public void ClickCloseBtn() {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }

    public void ClickStrongBtn() {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        // before send msg to server, should check it in client firstly
        if(pd.strongArr[currentIndex] < 10) {
            if(pd.lv < nextSc.minlv) {
                GameRoot.AddTips("Level does not meet the requirements");
                return;
            }
            if (pd.coin < nextSc.coin) {
                GameRoot.AddTips("Coin does not meet the requirements");
                return;
            }
            if (pd.crystal < nextSc.crystal) {
                GameRoot.AddTips("Crystal does not meet the requirements");
                return;
            }

            netSvc.SendMsg(new GameMsg {
                cmd = (int)CMD.ReqStrong,
                reqStrong = new ReqStrong {
                    pos = currentIndex
                }
            });
        } else {
            GameRoot.AddTips("Maximal Level");
        }
    }

    public void UpdateUI() {
        audioSvc.PlayUIAudio(Constants.dungeonItemEnter);
        // click current block is eqaul to refresh the UI
        ClickPosItem(currentIndex);
    }

}
