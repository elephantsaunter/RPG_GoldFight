using System;
using System.Collections.Generic;
using PENet;
using PEProtocol;
using UnityEngine;

public class NetSvc: MonoBehaviour {
    public static NetSvc Instance = null;

    private static readonly string obj = "lock";
    PESocket<ClientSession, GameMsg> client = null;
    private Queue<GameMsg> msgQue = new Queue<GameMsg>();


    public void InitSvc () {
        Instance = this;

        client = new PESocket<ClientSession, GameMsg>();
        client.SetLog(true, (string msg, int lv) => {
            switch (lv) {
                case 0:
                    msg = "Log:" + msg;
                    Debug.Log(msg);
                    break;
                case 1:
                    msg = "Warn:" + msg;
                    Debug.LogWarning(msg);
                    break;
                case 2:
                    msg = "Error:" + msg;
                    Debug.LogError(msg);
                    break;
                case 3:
                    msg = "Info:" + msg;
                    Debug.Log(msg);
                    break;
            }
        });
        client.StartAsClient(SrvCfg.srvIP, SrvCfg.srvPort);
        PECommon.Log("Init NetSvc...");
    }

    public void SendMsg (GameMsg msg) {
        if (client.session != null) {
            client.session.SendMsg(msg);
        } else {
            GameRoot.AddTips("Server is unconnected");
            InitSvc();
        }
    }

    public void AddNetPkg (GameMsg msg) {
        lock (obj) {
            msgQue.Enqueue(msg);
        }
    }

    private void Update () {
        if (msgQue.Count > 0) {
            lock (obj) {
                GameMsg msg = msgQue.Dequeue();
                ProcessMsg(msg);
            }
        }
    }

    private void ProcessMsg (GameMsg msg) {
        if (msg.err != (int)ErrorCode.None) {
            switch ((ErrorCode)msg.err) {
                case ErrorCode.ServerClientDataAsynError:
                    PECommon.Log("Server Data error", LogType.Error);
                    GameRoot.AddTips("Client Server error");
                    break;
                case ErrorCode.UpdateDBError:
                    PECommon.Log("Database update error", LogType.Error);
                    GameRoot.AddTips("Internet Verbindung error");
                    break;
                case ErrorCode.ClientDataError:
                    PECommon.Log("Client Data error", LogType.Error);
                    break;
                case ErrorCode.AcctIsOnline:
                    GameRoot.AddTips("This account is already online");
                    break;
                case ErrorCode.WrongPass:
                    GameRoot.AddTips("Password incorrect");
                    break;
                case ErrorCode.LackCoin:
                    GameRoot.AddTips("You do not have enough coin");
                    break;
                case ErrorCode.LackLv:
                    GameRoot.AddTips("Your Level don't meet the requirement");
                    break;
                case ErrorCode.LackCrystal:
                    GameRoot.AddTips("You do not have enough crystal");
                    break;
                case ErrorCode.LackDiamond:
                    GameRoot.AddTips("You do not have enough diamond");
                    break;
                case ErrorCode.LackPower:
                    GameRoot.AddTips("You do not have enough power");
                    break;
            }
            return;
        }
        switch ((CMD)msg.cmd) {
            case CMD.RspLogin:
                LoginSys.Instance.RspLogin(msg);
                break;
            case CMD.RspRename:
                LoginSys.Instance.RspRename(msg);
                break;
            case CMD.RspGuide:
                MainCitySys.Instance.RspGuide(msg);
                break;
            case CMD.RspStrong:
                MainCitySys.Instance.RspStrong(msg);
                break;
            case CMD.PshChat:
                MainCitySys.Instance.PshChat(msg);
                break;
            case CMD.RspBuy:
                MainCitySys.Instance.RspBuy(msg);
                break;
            case CMD.PshPower:
                MainCitySys.Instance.PshPower(msg);
                break;
            case CMD.RspTakeTaskReward:
                MainCitySys.Instance.RspTakeTaskReward(msg);
                break;
            case CMD.PshTaskPrgs:
                MainCitySys.Instance.PshTaskPrgs(msg);
                break;
            case CMD.RspMission:
                MissionSys.Instance.RspMission(msg);
                break;
            case CMD.RspBuyWithCoin:
                MissionSys.Instance.RspBuyWithCoin(msg);
                break;
            case CMD.RspMissionEnd:
                BattleSys.Instance.RspMissionEnd(msg);
                break;
        }
    }

    internal void SendMessage (GameMsg msg) {
        throw new NotImplementedException();
    }
}