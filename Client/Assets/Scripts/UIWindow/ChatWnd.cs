using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatWnd: WindowRoot{
    public InputField iptChat;
    public Text txtChat;
    public Image imgWorld;
    public Image imgGuild;
    public Image imgFriend;

    private int charType;
    private List<string> chatLst = new List<string>();

    protected override void InitWnd () {
        base.InitWnd();
        charType = 0;
        RefreshUI();
    }

    public void AddChatMsg(string name,string chat) {
        chatLst.Add(Constants.Color(name + ": ", TxtColor.Blue) + chat);
        if(chatLst.Count>12) {
            chatLst.RemoveAt(0);
        }
        RefreshUI();
    }
    private void RefreshUI() {
        if(charType == 0) {
            // WorldChat
            string chatMsg = "";
            for(int i = 0;i<chatLst.Count;i++) {
                chatMsg += chatLst[i] + "\n";
            }
            SetText(txtChat,chatMsg);
            SetSprite(imgWorld, "ResImages/btntype1");
            SetSprite(imgGuild, "ResImages/btntype2");
            SetSprite(imgFriend, "ResImages/btntype2");
        }
        else if (charType ==1) {
            // GuildChat
            SetText(txtChat, "You haven't join a Guild yet");
            SetSprite(imgWorld, "ResImages/btntype2");
            SetSprite(imgGuild, "ResImages/btntype1");
            SetSprite(imgFriend, "ResImages/btntype2");
        } else if (charType ==2) {
            // FriendChat
            SetText(txtChat, "You don't have friend yet");
            SetSprite(imgWorld, "ResImages/btntype2");
            SetSprite(imgGuild, "ResImages/btntype2");
            SetSprite(imgFriend, "ResImages/btntype1");
        }
    }
    private bool canSend = true;
    public void ClickSendBtn() {
        if (!canSend) {
            GameRoot.AddTips("Please wait 5s");

            return;
        }
        if(iptChat.text!=null && iptChat.text!="" && iptChat.text != " ") {
            if(iptChat.text.Length > 12) {
                GameRoot.AddTips("Please send infos within 12 chars");

            } else {
                // send internet infos to server
                GameMsg msg = new GameMsg {
                    cmd = (int)CMD.SndChat,
                    sndChat = new SndChat {
                        chat = iptChat.text
                    }
                };
                iptChat.text = "";
                netSvc.SendMsg(msg);
                canSend = false;
                //StartCoroutine(MsgTimer());
                timerSvc.AddTimerTask((int tid) => {
                    canSend = true;
                }, 5, PETimeUnit.Second);
            }
        } else {
            GameRoot.AddTips("Please enter the chat infos");
        }
    }
    //IEnumerator MsgTimer() {
    //    yield return new WaitForSeconds(5.0f);
    //    canSend = true;
    //}
    public void ClickWorldBtn () {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        charType = 0;
        RefreshUI();
    }
    public void ClickGuildBtn () {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        charType = 1;
        RefreshUI();
    }
    public void ClickFriendBtn () {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        charType = 2;
        RefreshUI();
    }
    public void ClickCloseBtn () {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        charType = 0;
        SetWndState(false);
    }
}
