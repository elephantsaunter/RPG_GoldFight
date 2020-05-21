using PEProtocol;
using System.Collections.Generic;

public class ChatSys {
    private static ChatSys instance = null;
    public static ChatSys Instance {
        get {
            if(instance == null) {
                instance = new ChatSys();
            }
            return instance;
        }
    }
    private CacheSvc cacheSvc;
    public void Init() {
        cacheSvc = CacheSvc.Instance;
        PECommon.Log("ChatSys Init Done");
    }

   public void SndChat(MsgPack pack) {
        SndChat data = pack.msg.sndChat;
        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);

        // task progress update
        TaskSys.Instance.CalcTaskPrgs(pd, 6);
        
        GameMsg msg = new GameMsg {
            cmd = (int)CMD.PshChat,
            pshChat = new PshChat {
                name = pd.name,
                chat = data.chat
            }
        };

        // broadcasting to all clients
        List<ServerSession> lst = cacheSvc.GetOnlineServerSession();
        // Serializable it one, then it dont need to do many times in the internet push this msg to many clients
        byte[] bytes = PENet.PETool.PackNetMsg(msg);
        for(int i = 0;i<lst.Count;i++) {
            lst[i].SendMsg(msg);
        }

    }
}
