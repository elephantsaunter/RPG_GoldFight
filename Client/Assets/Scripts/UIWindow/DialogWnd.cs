using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogWnd : WindowRoot {
	public Text txtDialog;
	protected override void InitWnd () {
		base.InitWnd();
	}
	public void ClickCloseBtn () {
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		MainCitySys.Instance.CloseTaskWnd();
	}
}
