using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCtrlWnd : WindowRoot {
    public Image imgTouch;
    public Image imgDirBg;
    public Image imgDirPoint;
    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;
    public Transform expPrgTrans;

    private float pointDis;
    private Vector2 startPos = Vector2.zero;
    private Vector2 defaultPos = Vector2.zero;

    public Vector2 currentDir;

    #region skill1
    public Image imgSk1CD;
    public Text txtSk1CD;
    private bool isSk1CD = false; // flag whether you should wait
    private float sk1CDTime; // CD Time
    private int sk1Num; // the second number ist shown in txtSk1CD
    private float sk1FillCount = 0;
    private float sk1NumCount = 0;
    #endregion

    #region skill2
    public Image imgSk2CD;
    public Text txtSk2CD;
    private bool isSk2CD = false; // flag whether you should wait
    private float sk2CDTime; // CD Time
    private int sk2Num; // the second number ist shown in txtSk1CD
    private float sk2FillCount = 0;
    private float sk2NumCount = 0;
    #endregion

    #region skill3
    public Image imgSk3CD;
    public Text txtSk3CD;
    private bool isSk3CD = false; // flag whether you should wait
    private float sk3CDTime; // CD Time
    private int sk3Num; // the second number ist shown in txtSk1CD
    private float sk3FillCount = 0;
    private float sk3NumCount = 0;
    #endregion

    public Text txtSelfHP;
    public Image imgSelfHP;

    private int HPSum;
    protected override void InitWnd () {
        base.InitWnd();
        pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis;
        defaultPos = imgDirBg.transform.position;
        SetActive(imgDirPoint, false);
        HPSum = GameRoot.Instance.PlayerData.hp;
        SetText(txtSelfHP, HPSum + "/" + HPSum);
        imgSelfHP.fillAmount = 1;
        SetBossHPBarState(false);
        RegisterTouchEvts();
        sk1CDTime = resSvc.GetSkillCfg(101).cdTime / 1000.0f;
        sk2CDTime = resSvc.GetSkillCfg(102).cdTime / 1000.0f;
        sk3CDTime = resSvc.GetSkillCfg(103).cdTime / 1000.0f;
        RefreshUI();
    }
    public void RefreshUI () {
        // Set fight and power and level and name
        PlayerData pd = GameRoot.Instance.PlayerData;
        SetText(txtLevel, pd.lv);
        SetText(txtName, pd.name);

        #region ExpPrg
        // exp-text
        int expPrgVal = (int)(pd.exp * 1.0f / PECommon.GetExpUpValByLv(pd.lv) * 100);
        SetText(txtExpPrg, expPrgVal + "%");


        //Screen anpassen
        //itemLst 1264 bgExp_left=72 itemLST_left=8 itemLST_right=10 space=20*9
        // 1334-180=1154    1154/10=115.4
        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();

        float globalRate = 1.0f * Constants.ScreenStandardHeight / Screen.height;
        float screenWidth = Screen.width * globalRate;
        // 10 blocks have 9 space, every space is 20px, all space is 180px
        float width = (screenWidth - 180) / 10;

        grid.cellSize = new Vector2(width, 7);

        //exp-image-prozent
        int index = expPrgVal / 10; // your exp reached the number of exp-block
        for (int i = 0; i < expPrgTrans.childCount; i++) {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            if (i < index) {
                img.fillAmount = 1;
            } else if (i == index) {
                img.fillAmount = expPrgVal % 10 * 1.0f / 10;
            } else {
                img.fillAmount = 0;
            }
        }
        #endregion

    }
    private void Update() {
        // TEST
        if (Input.GetKeyDown(KeyCode.A)) {
            ClickNormalAtk();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            ClickSkill1();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            ClickSkill2();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            ClickSkill3();
        }
        float delta = Time.deltaTime;
        #region SkillCD
        if (isSk1CD) {
            // cannot release skill
            sk1FillCount += delta;
            if(sk1FillCount>=sk1CDTime) {
                // wait time is enough
                isSk1CD = false;
                SetActive(imgSk1CD, false);
                sk1FillCount = 0;
            }
            else {
                // wait time is not enough
                imgSk1CD.fillAmount = 1 - sk1FillCount / sk1CDTime;
            }
            // every second do a refresh in txtSk1CD
            sk1NumCount += delta;
            if(sk1NumCount >= 1) {
                // wait state, update the text and, every second do a judge
                sk1NumCount -= 1;
                sk1Num -= 1;
                SetText(txtSk1CD, sk1Num);
            }
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
            // every second do a refresh in txtSk2CD
            sk2NumCount += delta;
            if (sk2NumCount >= 1) {
                // wait state, update the text and, every second do a judge
                sk2NumCount -= 1;
                sk2Num -= 1;
                SetText(txtSk2CD, sk2Num);
            }
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
            if (sk3NumCount >= 1) {
                // wait state, update the text and, every second do a judge
                sk3NumCount -= 1;
                sk3Num -= 1;
                SetText(txtSk3CD, sk3Num);
            }
        }
        #endregion
        if(transBossHPBar.gameObject.activeSelf) {
            BlendBossHP();
            imageYellow.fillAmount = currentPrg;
        }
    }
    public void RegisterTouchEvts () {
        OnClickDown(imgTouch.gameObject, (PointerEventData evt) => {
            startPos = evt.position;
            SetActive(imgDirPoint);
            imgDirBg.transform.position = evt.position;
        });

        OnClickUp(imgTouch.gameObject, (PointerEventData evt) => {
            imgDirBg.transform.position = defaultPos;
            SetActive(imgDirPoint, false);
            imgDirPoint.transform.localPosition = Vector2.zero;
            currentDir = Vector2.zero;
            BattleSys.Instance.SetMoveDir(currentDir);

        });

        OnDrag(imgTouch.gameObject, (PointerEventData evt) => {
            Vector2 dir = evt.position - startPos;
            float len = dir.magnitude;
            if (len > pointDis) {
                //already outside of controllor background
                Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
                imgDirPoint.transform.position = startPos + clampDir;
            } else {
                imgDirPoint.transform.position = evt.position;
            }
            currentDir = dir.normalized;
            // Direction infos
            BattleSys.Instance.SetMoveDir(currentDir);
        });
    }
   
    
    public void ClickNormalAtk () {
        BattleSys.Instance.ReqReleaseSkill(0);
    }
    public void ClickSkill1 () {
        if(isSk1CD==false && GetCanRisSkill()) {
            BattleSys.Instance.ReqReleaseSkill(1);
            isSk1CD = true;
            SetActive(imgSk1CD);
            imgSk1CD.fillAmount = 1;
            sk1Num = (int)sk1CDTime;
            SetText(txtSk1CD, sk1Num);
        }
    }

    public void ClickSkill2 () {
        if (isSk2CD == false && GetCanRisSkill()) {
            BattleSys.Instance.ReqReleaseSkill(2);
            isSk2CD = true;
            SetActive(imgSk2CD);
            imgSk2CD.fillAmount = 1;
            sk2Num = (int)sk2CDTime;
            SetText(txtSk2CD, sk2Num);
        }
    }

    public void ClickSkill3 () {
        if (isSk3CD == false && GetCanRisSkill()) {
            BattleSys.Instance.ReqReleaseSkill(3);
            isSk3CD = true;
            SetActive(imgSk3CD);
            imgSk3CD.fillAmount = 1;
            sk3Num = (int)sk3CDTime;
            SetText(txtSk3CD, sk3Num);
        }
    }
    // Test Reset Data
    public void ClickResetCfgs() {
        resSvc.ResetSkillCfgs();
    }
    public void SetSelfHPVal(int val) {
        SetText(txtSelfHP, val + "/" + HPSum);
        imgSelfHP.fillAmount = val * 1.0f / HPSum;
    }

    public bool GetCanRisSkill() {
        return BattleSys.Instance.battleMgr.CanRisSkill();
    }

    public Transform transBossHPBar;
    public Image imageRed;
    public Image imageYellow;
    private float currentPrg = 1f;
    private float targetPrg = 1f;
    public void SetBossHPBarVal(int oldVal, int newVal, int sumVal) {
        currentPrg = oldVal * 1.0f / sumVal;
        targetPrg = newVal * 1.0f / sumVal;
        imageRed.fillAmount = targetPrg;
    }
    private void BlendBossHP() {
        if(Mathf.Abs(currentPrg - targetPrg) < Constants.AccelerHPSpeed * Time.deltaTime) {
            currentPrg = targetPrg;
        }
        else if(currentPrg > targetPrg) {
            currentPrg -= Constants.AccelerHPSpeed * Time.deltaTime;
        } else {
            currentPrg += Constants.AccelerHPSpeed * Time.deltaTime;
        }
    }
    public void SetBossHPBarState(bool state, float prg = 1) {
        SetActive(transBossHPBar, state);
        imageRed.fillAmount = prg;
        imageYellow.fillAmount = prg;
    }
}
