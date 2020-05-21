using PEProtocol;

public class StrongSys {
    private static StrongSys instance = null;
    public static StrongSys Instance {
        get {
            if (instance == null) {
                instance = new StrongSys();
            }
            return instance;
        }
    }
    private CacheSvc cacheSvc = null;

    public void Init () {
        cacheSvc = CacheSvc.Instance;
        PECommon.Log("EnhancementSys Init Done.");
    }

    public void ReqStrong(MsgPack pack) {
        ReqStrong data = pack.msg.reqStrong;
        GameMsg msg = new GameMsg {
            cmd = (int)CMD.RspStrong
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        int curtStarLv = pd.strongArr[data.pos];
        StrongCfg nextSc = CfgSvc.Instance.GetStrongCfg(data.pos, curtStarLv + 1);
        // check consitions--coin, lv and crystal
        if(pd.lv < nextSc.minlv) {
            msg.err = (int)ErrorCode.LackLv;
        }
        else if (pd.coin < nextSc.coin) {
            msg.err = (int)ErrorCode.LackCoin;
        }
        else if (pd.crystal < nextSc.crystal) {
            msg.err = (int)ErrorCode.LackCrystal;
        }
        else {
            // task progress update
            TaskSys.Instance.CalcTaskPrgs(pd, 3);
            // minus coin and crystal
            pd.coin -= nextSc.coin;
            pd.crystal -= nextSc.crystal;
            pd.strongArr[data.pos] += 1; // star + 1
            // enhancement applied, bonus add HP, Hurt and Defence
            pd.hp += nextSc.addhp;
            pd.ad += nextSc.addhurt;
            pd.ap += nextSc.addhurt;
            pd.addef += nextSc.adddef;
            pd.apdef += nextSc.adddef;
        }
        // update Database
        if(!cacheSvc.UpdatePlayerData(pd.id,pd)) {
            msg.err = (int)ErrorCode.UpdateDBError;
        }
        else {
            msg.rspStrong = new RspStrong {
                coin = pd.coin,
                crystal = pd.crystal,
                hp = pd.hp,
                ad = pd.ad,
                ap = pd.ap,
                addef = pd.addef,
                apdef = pd.apdef,
                strongArr = pd.strongArr
            };
        }

        pack.session.SendMsg(msg);
    }
}
