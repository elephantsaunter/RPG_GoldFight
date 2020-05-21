using PEProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoWnd : WindowRoot {
	#region UI define
	public RawImage imgChar;

	public Text txtInfo;
	public Text txtExp;
	public Image imgExpPrg;
	public Text txtPower;
	public Image imgPowerPrg;

	public Text txtJob;
	public Text txtFight;
	public Text txtHP;
	public Text txtHurt;
	public Text txtDef;

	public Button btnClose;
	public Button btnDetail;
	public Button btnCloseDetail;
	public Transform transDetail;

	public Text dtxhp;
	public Text dtxad;
	public Text dtxap;
	public Text dtxaddef;
	public Text dtxapdef;
	public Text dtxdodge;
	public Text dtxpierce;
	public Text dtxcritical;

	#endregion
	private Vector2 startPos;
	protected override void InitWnd () {
		base.InitWnd();
		RegTouchEvts();
		SetActive(transDetail, false);
		RefreshUI();
	}

	private void RegTouchEvts() {
		OnClickDown(imgChar.gameObject, (PointerEventData evt) => {
			startPos = evt.position;
			MainCitySys.Instance.SetStartRotate();
		});
		OnDrag(imgChar.gameObject, (PointerEventData evt) => {
			float rotate = -(evt.position.x - startPos.x) * 0.3f;
			MainCitySys.Instance.SetPlayerRotate(rotate);
		});
	}

	private void PointEventData (PointerEventData obj) {
		throw new NotImplementedException();
	}

	private void RefreshUI() {
		PlayerData pd = GameRoot.Instance.PlayerData;
		SetText(txtInfo, pd.name + " LV." + pd.lv);
		SetText(txtExp, pd.exp +"/" + PECommon.GetExpUpValByLv(pd.lv));
		imgExpPrg.fillAmount = pd.exp * 1.0F / PECommon.GetExpUpValByLv(pd.lv);
		SetText(txtPower, pd.power + "/" + PECommon.GetPowerLimit(pd.lv));
		imgPowerPrg.fillAmount = pd.power * 1.0F / PECommon.GetPowerLimit(pd.lv);

		SetText(txtJob, " Charc.         Spiter");
		SetText(txtHP, pd.hp);
		SetText(txtHurt, pd.ad + pd.ap);
		SetText(txtFight, pd.addef + pd.apdef);
		// detail
		SetText(dtxhp, pd.hp);
		SetText(dtxad, pd.ad);
		SetText(dtxap, pd.ap);
		SetText(dtxaddef, pd.addef);
		SetText(dtxapdef, pd.apdef);
		SetText(dtxdodge, pd.dodge + "%");
		SetText(dtxpierce, pd.pierce + "%");
		SetText(dtxcritical, pd.critical + "%");

	}

	public void ClickCloseBtn() {
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		MainCitySys.Instance.CloseInfoWnd();
	}

	public void ClickDetailBtn() {
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		SetActive(transDetail);
	}
	public void ClickCloseDetailBtn () {
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		SetActive(transDetail,false);
	}
}
