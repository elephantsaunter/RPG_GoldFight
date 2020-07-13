using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SWEWnd: WindowRoot {
    //public static OptionWnd Instance = null;
    public Text txtFrage;
    public Text txtContent;
    public Text txtDiamond;
    public Text txtPower;
    private PlayerData pd;

    protected override void InitWnd () {
        //Instance = this;
        base.InitWnd();
        
        RefreshUI();
    }
    private void Update () {
       
    }

    public void RefreshUI () {
        pd = GameRoot.Instance.PlayerData;
        // SetText(txtCoin, pd.coin);
        SetText(txtDiamond, pd.diamond);
        SetText(txtPower, "Power " + pd.power + "/150");
    }

    

}

