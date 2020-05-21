﻿

public class ServerRoot {
    private static ServerRoot instance = null;
    public static ServerRoot Instance {
        get {
            if (instance == null) {
                instance = new ServerRoot();
            }
            return instance;
        }
    }

    public void Init() {
        //Data layer
        DBMgr.Instance.Init();

        //Service layer
        CfgSvc.Instance.Init();
        CacheSvc.Instance.Init();
        NetSvc.Instance.Init();
        TimerSvc.Instance.Init();


        //Oprational Risk Service Layer
        LoginSys.Instance.Init();
        GuideSys.Instance.Init();
        StrongSys.Instance.Init();
        ChatSys.Instance.Init();
        BuySys.Instance.Init();
        PowerSys.Instance.Init();
        TaskSys.Instance.Init();
        MissionSys.Instance.Init();
    }

    public void Update() {
        NetSvc.Instance.Update();
        TimerSvc.Instance.Update();
    }

    private int SessionID = 0;
    public int GetSessionID()
    {   if(SessionID==int.MaxValue)
        {
            SessionID = 0;
        }
        return SessionID += 1;
    }
}
