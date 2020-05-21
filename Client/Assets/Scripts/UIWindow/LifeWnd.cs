using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeWnd : WindowRoot {
    public Text txtCoin;
    private PlayerData pd;
    protected override void InitWnd () {
        base.InitWnd();
    }

    public void ClickCloseWnd () {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        SetWndState(false);
    }
}
