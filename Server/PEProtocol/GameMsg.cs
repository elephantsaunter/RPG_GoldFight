using System;
using PENet;

namespace PEProtocol {
    [Serializable]
    public class GameMsg: PEMsg {
        public ReqLogin reqLogin;
        public RspLogin rspLogin;
        public ReqRename reqRename;
        public RspRename rspRename;
        public ReqGuide reqGuide;
        public RspGuide rspGuide;
        public ReqStrong reqStrong;
        public RspStrong rspStrong;
        public SndChat sndChat;
        public PshChat pshChat;
        public ReqBuy reqBuy;
        public RspBuy rspBuy;

        public PshPower pshPower;
        public ReqTakeTaskReward reqTakeTaskReward;
        public RspTakeTaskReward rspTakeTaskReward;
        public PshTaskPrgs pshTaskPrgs;
        public ReqMission reqMission;
        public RspMission rspMission;
        public ReqBuyWithCoin reqBuyWithCoin;
        public RspBuyWithCoin rspBuyWithCoin;
        public ReqMissionEnd reqMissionEnd;
        public RspMissionEnd rspMissionEnd;
    }

    #region login
    [Serializable]
    public class ReqLogin {
        public string acct;
        public string pass;
    }

    [Serializable]
    public class RspLogin {
        public PlayerData playerData;
    }

    [Serializable]
    public class PlayerData {
        public int id;
        public string name;
        public int lv;
        public int exp;
        public int power;
        public int coin;
        public int diamond;
        public int crystal;

        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int dodge;
        public int pierce;
        public int critical;

        public int guideid;
        public int[] strongArr; // index is pos,value is star
        public long time;
        public string[] taskArr;
        public int mission;
        //public int[] optionArr;
        //TOADD
    }

    [Serializable]
    public class ReqRename {
        public string name;
    }

    [Serializable]
    public class RspRename {
        public string name;
    }
    #endregion

    #region Guide System
    [Serializable]
    public class ReqGuide {
        public int guideid;
    }

    [Serializable]
    public class RspGuide {
        public int guideid;
        public int coin;
        public int lv;
        public int exp;
    }

    #endregion

    #region Enhancement
    [Serializable]
    public class ReqStrong {
        public int pos;
    }

    [Serializable]
    public class RspStrong {
        public int coin;
        public int crystal;
        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int[] strongArr;
    }
    #endregion

    #region Chat 
    [Serializable]
    public class SndChat {
        public string chat;
    }

    [Serializable]
    public class PshChat {
        public string name;
        public string chat;
    }
    #endregion

    #region Buy
    [Serializable]
    public class ReqBuy {
        public int type;
        public int cost;
    }

    [Serializable]
    public class RspBuy {
        public int type;
        public int diamond;
        public int coin;
        public int power;
    }

    [Serializable]
    public class PshPower {
        public int power;
    }

    #endregion

    #region MissionCombat
    [Serializable]
    public class ReqMission {
        public int fbid;
    }
    [Serializable]
    public class RspMission {
        public int fbid;
        public int power;
    }
    [Serializable]
    public class ReqMissionEnd {
        public bool isWin;
        public int bid; // battle id
        public int resthp;
        public int costtime;
    }
    [Serializable]
    public class RspMissionEnd {
        public bool isWin;
        public int bid; // battle id
        public int resthp;
        public int costtime;
        // reward
        public int coin;
        public int lv;
        public int exp;
        public int crystal;
        public int afterBid;

    }

    [Serializable]
    public class ReqBuyWithCoin {
        public int type;
        public int cost;
    }

    [Serializable]
    public class RspBuyWithCoin {
        public int type;
        public int coin;
    }

    #endregion
    #region TaskReward
    [Serializable]
    public class ReqTakeTaskReward {
        public int rid;
    }
    [Serializable]
    public class RspTakeTaskReward {
        public int coin;
        public int lv;
        public int exp;
        public string[] taskArr;
    }
    [Serializable]
    public class PshTaskPrgs {
        public string[] taskArr;
    }
    #endregion
    public enum ErrorCode {
        None = 0,// no mistake
        ServerClientDataAsynError,// data from client and server are not matched ,may cheating app is used
        UpdateDBError, // database error
        ClientDataError,

        AcctIsOnline,// account is online
        WrongPass,// password incorrect
        NameIsExist,

        LackCoin,
        LackLv,
        LackCrystal,
        LackDiamond,
        LackPower
    }

    public enum CMD {
        None = 0,
        // login 100
        ReqLogin = 101,
        RspLogin = 102,
        ReqRename = 103,
        RspRename = 104,
        // MainCity 200
        ReqGuide = 201,
        RspGuide = 202,
        ReqStrong = 203,
        RspStrong = 204,
        SndChat = 205,
        PshChat = 206,
        ReqBuy = 207,
        RspBuy = 208,
        PshPower = 210,
        ReqTakeTaskReward = 211,
        RspTakeTaskReward = 212,
        PshTaskPrgs = 214,
        ReqMission = 301,
        RspMission = 302,
        ReqBuyWithCoin = 303,
        RspBuyWithCoin = 304,
        ReqMissionEnd = 305,
        RspMissionEnd = 306,
    }

    public class SrvCfg {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 17666;
    }
}
