using PEProtocol;

public class LoginSys {
    private static LoginSys instance = null;
    public static LoginSys Instance {
        get {
            if (instance == null) {
                instance = new LoginSys();
            }
            return instance;
        }
    }
    private CacheSvc cacheSvc = null;
    private TimerSvc timerSvc = null;

    public void Init() {
        cacheSvc = CacheSvc.Instance;
        timerSvc = TimerSvc.Instance;
        PECommon.Log("LoginSys Init Done.");
    }

    public void ReqLogin(MsgPack pack) {
        ReqLogin data = pack.msg.reqLogin;
        //whether this account online
        GameMsg msg = new GameMsg {
            cmd = (int)CMD.RspLogin
        };
        if (cacheSvc.IsAcctOnLine(data.acct)) {
            //already online, return problem infos
            msg.err = (int)ErrorCode.AcctIsOnline;
        }
        else {
            //not online
            //whether account exists 
            PlayerData pd = cacheSvc.GetPlayerData(data.acct, data.pass);
            if (pd == null) {
                //acccount exists, password incorrect
                msg.err = (int)ErrorCode.WrongPass;
            }
            else {
                // calculate power increase offline
                int power = pd.power;
                long now = timerSvc.GetNowTime();
                long milliseconds = now - pd.time;
                int addPower = (int)(milliseconds) / (1000 * 60 * PECommon.PowerAddSpace) * PECommon.PowerAddCount;
                if(addPower > 0) {
                    int powerMax = PECommon.GetPowerLimit(pd.lv);
                    if(power<powerMax) {
                        power += addPower;
                        if(power > powerMax) {
                            power = powerMax;
                        }
                    }
                }

                if(power!=pd.power) {
                    cacheSvc.UpdatePlayerData(pd.id, pd);
                }
                msg.rspLogin = new RspLogin {
                    playerData = pd
                };
                //session account data
                cacheSvc.AcctOnline(data.acct, pack.session, pd);
            }
        }
        //feedback to server
        pack.session.SendMsg(msg);
    }
    
    public void ReqRename(MsgPack pack)
    {
        ReqRename data = pack.msg.reqRename;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspRename
        };

        if(cacheSvc.IsNameExist(data.name))
        {
            //whether name exists
            //name exists: return error code
            msg.err = (int)ErrorCode.NameIsExist;

        }else
        {
            //name not exists, refresh session and datenbase, then return to client
            PlayerData playerData = cacheSvc.GetPlayerDataBySession(pack.session);
            playerData.name = data.name;

            if (!cacheSvc.UpdatePlayerData(playerData.id, playerData))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }else
            {
                msg.rspRename = new RspRename
                {
                    name = data.name
                };
            }
        }
        pack.session.SendMsg(msg);
        
    }

    public void ClintOfflineData(ServerSession session)
    {
        // offline time
        PlayerData pd = cacheSvc.GetPlayerDataBySession(session);
        if(pd!=null) {
            pd.time = timerSvc.GetNowTime();
            if(!cacheSvc.UpdatePlayerData(pd.id,pd)) {
                PECommon.Log("Update Offline Time Error",LogType.Error);
            }
            cacheSvc.AcctOffLine(session);
        }
    }
}
