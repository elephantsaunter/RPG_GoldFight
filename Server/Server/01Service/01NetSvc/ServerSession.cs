
using PENet;
using PEProtocol;

public class ServerSession : PESession<GameMsg> {
    public int sessionID = 0;
    protected override void OnConnected() {
        sessionID = ServerRoot.Instance.GetSessionID();
        PECommon.Log("Session ID: " + sessionID +" Client Connect");
    }

    protected override void OnReciveMsg(GameMsg msg) {
        PECommon.Log("Session ID:" + "  RcvPack CMD:" + ((CMD)msg.cmd).ToString());
        NetSvc.Instance.AddMsgQue(this, msg);
    }

    protected override void OnDisConnected() {
        LoginSys.Instance.ClintOfflineData(this);
        PECommon.Log("Session ID:" + sessionID + "Client Disconnect");
    }
}
