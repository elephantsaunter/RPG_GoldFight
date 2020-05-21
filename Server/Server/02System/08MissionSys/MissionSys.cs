using PEProtocol;

public class MissionSys {
    private static MissionSys instance;
    public static MissionSys Instance {
        get {
            if (instance == null) {
                instance = new MissionSys();
            }
            return instance;
        }
    }
    private CacheSvc cacheSvc = null;
    private CfgSvc cfgSvc = null;
    public void Init () {
        cacheSvc = CacheSvc.Instance;
        cfgSvc = CfgSvc.Instance;
        PECommon.Log("MissionSys Init Done");
    }
    public void ReqMission (MsgPack pack) {
        ReqMission data = pack.msg.reqMission;
        GameMsg msg = new GameMsg {
            cmd = (int)CMD.RspMission
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        int power = cfgSvc.GetMapCfg(data.fbid).power;

        if (pd.mission < data.fbid) {
            // your missionid is lower than the map id required
            // eg. 10001 is your zustand, and you want to go 10002 map, not allowed
            msg.err = (int)ErrorCode.ClientDataError;
        } else if (pd.power < power) {
            msg.err = (int)ErrorCode.LackPower;

        } else {
            if (cacheSvc.UpdatePlayerData(pd.id, pd)) {
                RspMission rspMission = new RspMission {
                    fbid = data.fbid,
                    power = pd.power
                };
                msg.rspMission = rspMission;
            } else {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
        }
        pack.session.SendMsg(msg);
    }
    public void ReqBuyWithCoin (MsgPack pack) {
        ReqBuyWithCoin data = pack.msg.reqBuyWithCoin;
        GameMsg msg = new GameMsg {
            cmd = (int)CMD.RspBuyWithCoin
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        if (pd.coin < data.cost) {
            msg.err = (int)ErrorCode.LackCoin;
        } else {
            pd.coin -= data.cost;
            switch (data.type) {
                case 0:
                    pd.coin -= 200;
                    break;
                case 1:
                    //pd.coin += 1000;
                     break;
                case 2:
                    //pd.power += 100;
                     break;
                case 3:
                    //pd.coin += 1000;
                    break;
            }

            if (!cacheSvc.UpdatePlayerData(pd.id, pd)) {
                msg.err = (int)ErrorCode.UpdateDBError;
            } else {
                msg.rspBuyWithCoin = new RspBuyWithCoin {
                    type = data.type,
                    coin = pd.coin,
                };
            }
        }
        pack.session.SendMsg(msg);
    }
}
