using PEProtocol;

public class GuideSys {
    private static GuideSys instance = null;
    public static GuideSys Instance {
        get {
            if(instance == null) {
                instance = new GuideSys();
            }
            return instance;
        }
    }
    private CacheSvc cacheSvc = null;
    private CfgSvc cfgSvc = null;

    public void Init() {
        cacheSvc = CacheSvc.Instance;
        cfgSvc = CfgSvc.Instance;
        PECommon.Log("GuideSystem Init Done");
    }

    public void ReqGuide(MsgPack pack) {
        ReqGuide data = pack.msg.reqGuide;
        GameMsg msg = new GameMsg {
            cmd = (int)CMD.RspGuide
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        AutoGuideCfg agc = cfgSvc.GetAutoGuideCfg(data.guideid);
        // update guide ID
        if(pd.guideid == data.guideid) {
            // check whether is wiser task
            if(pd.guideid == 1001) {
                TaskSys.Instance.CalcTaskPrgs(pd,1);
            }
            pd.guideid += 1;
            // update player data
            pd.coin += agc.coin;
            PECommon.CalcExp(pd,agc.exp);
            if (!cacheSvc.UpdatePlayerData(pd.id, pd)) {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else {
                msg.rspGuide = new RspGuide {
                    guideid = pd.guideid,
                    coin = pd.coin,
                    lv = pd.lv,
                    exp = pd.exp
                };
            }
        } else {
            //client and server are asyn
            msg.err = (int)ErrorCode.ServerClientDataAsynError;
        }
        pack.session.SendMsg(msg);
    }

}
