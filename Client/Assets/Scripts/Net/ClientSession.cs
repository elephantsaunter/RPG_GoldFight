
using PENet;
using PEProtocol;

public class ClientSession : PESession<GameMsg> {
    protected override void OnConnected() {
        GameRoot.AddTips("Connect To Server Succ");
        PECommon.Log("Connect To Server Succ");
    }

    protected override void OnReciveMsg(GameMsg msg) {
        PECommon.Log("RcvPack CMD:" + ((CMD)msg.cmd).ToString());
        NetSvc.Instance.AddNetPkg(msg);
    }

    protected override void OnDisConnected() {
        GameRoot.AddTips("DisConnect To Server");
        PECommon.Log("DisConnect To Server");
    }
}