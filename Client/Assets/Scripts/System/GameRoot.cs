using PEProtocol;
using System;
using UnityEngine;

public class GameRoot : MonoBehaviour {
    public static GameRoot Instance = null;

    public LoadingWnd loadingWnd;
    public DynamicWnd dynamicWnd;

    private void Start() {
        Instance = this;
        DontDestroyOnLoad(this);
        PECommon.Log("Game Start...");

        ClearUIRoot();

        Init();
    }

    private void ClearUIRoot() {
        Transform canvas = transform.Find("Canvas");
        for (int i = 0; i < canvas.childCount; i++) {
            canvas.GetChild(i).gameObject.SetActive(false);
        }

    }

    private void Init() {
        //Service init...
        NetSvc net = GetComponent<NetSvc>();
        net.InitSvc();
        ResSvc res = GetComponent<ResSvc>();
        res.InitSvc();
        AudioSvc audio = GetComponent<AudioSvc>();
        audio.InitSvc();
        TimerSvc time = GetComponent<TimerSvc>();
        time.InitSvc();


        //Oprational System init...
        LoginSys login = GetComponent<LoginSys>();
        login.InitSys();
        MainCitySys mainCitySys = GetComponent<MainCitySys>();
        mainCitySys.InitSys();
        MissionSys missionSys = GetComponent<MissionSys>();
        missionSys.InitSys();
        BattleSys battleSys = GetComponent<BattleSys>();
        battleSys.InitSys();


        dynamicWnd.SetWndState();
        //enter login display and load UI
        login.EnterLogin();
    }

    public static void AddTips(string tips) {
        Instance.dynamicWnd.AddTips(tips);
    }

    private PlayerData playerData = null;
    public PlayerData PlayerData {
        get {
            return playerData;
        }
    }
    public void SetPlayerData(RspLogin data) {
        playerData = data.playerData;
    }

    public void SetPlayerName(string name) {
        PlayerData.name = name;
    }

    public void SetPlayerDataByGuide(RspGuide data) {
        PlayerData.coin = data.coin;
        PlayerData.lv = data.lv;
        PlayerData.exp = data.exp;
        PlayerData.guideid = data.guideid;
    }

    public void SetPlayerDataByStrong(RspStrong data) {
        PlayerData.coin = data.coin;
        PlayerData.crystal = data.crystal;
        PlayerData.hp = data.hp;
        PlayerData.ad = data.ad;
        PlayerData.ap = data.ap;
        PlayerData.addef = data.addef;
        PlayerData.apdef = data.apdef;

        PlayerData.strongArr = data.strongArr;
    }

    public void SetPlayerDataByBuy(RspBuy data) {
        PlayerData.coin = data.coin;
        PlayerData.diamond = data.diamond;
        PlayerData.power = data.power;
    }

    public void SetPlayerDataByBuyWithCoin (RspBuyWithCoin data) {
        PlayerData.coin = data.coin;
    }

    public void SetPlayerDataByPower(PshPower data) {
        PlayerData.power = data.power;
    }
    public void SetPlayerDataByTask (RspTakeTaskReward data) {
        PlayerData.coin = data.coin;
        PlayerData.exp = data.exp;
        PlayerData.lv = data.lv;
        PlayerData.taskArr = data.taskArr;
    }

    public void SetPlayerDataByTaskPsh (PshTaskPrgs data) {
        PlayerData.taskArr = data.taskArr;
    }
    public void SetPlayerDataByMission (RspMission data) {
        PlayerData.power= data.power;
    }

}