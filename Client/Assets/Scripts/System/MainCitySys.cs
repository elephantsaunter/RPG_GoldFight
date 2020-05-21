using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainCitySys : SystemRoot {

	public static MainCitySys Instance = null;
	public MainCityWnd mainCityWnd;
	public InfoWnd InfoWnd;
	public GuideWnd guideWnd;
	public StrongWnd strongWnd;
	public ChatWnd chatWnd;
	public BuyWnd buyWnd;
	public TaskWnd taskWnd;

	private PlayerController playerCtrl;
	private Transform charCamTrans;
	private AutoGuideCfg curtTaskData;
	private Transform[] npcPosTrans;
	private NavMeshAgent nav;


	public override void InitSys()
	{
		base.InitSys();

		Instance = this;
		PECommon.Log("Init MainSys...");
	}
	public void EnterMainCity()
	{
		MapCfg mapData = resSvc.GetMapCfg(Constants.MainCityMapID);
		resSvc.AsyncLoadScene(mapData.sceneName, () =>
		{
			PECommon.Log("Entre MainCity...");

			// TODO load main role
			LoadPlayer(mapData);
			//open main city UI
			mainCityWnd.SetWndState();
			// play background music
			audioSvc.PlayBGMusic(Constants.BGMainCity);

			GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
			MainCityMap mcm = map.GetComponent<MainCityMap>();
			npcPosTrans = mcm.NpcPosTrans;
			// set role display camare
			if(charCamTrans!=null) {
				charCamTrans.gameObject.SetActive(false);
			}
		});
	}
	private void LoadPlayer(MapCfg mapData) {
		GameObject player = resSvc.LoadPrefab(PathDefine.AssissinCityPlayerPrefab, true);
		player.transform.position = mapData.playerBornPos;
		player.transform.localEulerAngles = mapData.playerBornRote;
		player.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

		// camara initialization
		Camera.main.transform.position = mapData.mainCamPos;
		Camera.main.transform.localEulerAngles = mapData.mainCamRote;

		playerCtrl = player.GetComponent<PlayerController>();
		playerCtrl.Init();
		nav = player.GetComponent<NavMeshAgent>();
	}
	public void SetMoveDir(Vector2 dir) {
		StopNavTask();
		// Animation
		if(dir == Vector2.zero) {
			playerCtrl.SetBlend(Constants.BlendIdle);
		} else {
			playerCtrl.SetBlend(Constants.BlendMove);
		}

		// Vector Direction
		playerCtrl.Dir = dir;
	}
    #region MissionWnd
    public void EnterMission() {
		StopNavTask();
		MissionSys.Instance.EnterMission();
	}
    #endregion
    #region TaskWnd
    public void OpenTaskRewardWnd() {
		StopNavTask();
		taskWnd.SetWndState();
	}
	public void RspTakeTaskReward(GameMsg msg) {
		RspTakeTaskReward data = msg.rspTakeTaskReward;
		GameRoot.Instance.SetPlayerDataByTask(data);
		taskWnd.RefreshUI();
		mainCityWnd.RefreshUI();
	}
	public void PshTaskPrgs (GameMsg msg) {
		PshTaskPrgs data = msg.pshTaskPrgs;
		GameRoot.Instance.SetPlayerDataByTaskPsh(data);

		if (taskWnd.GetWndState()) {
			taskWnd.RefreshUI();
		}
	}
	public void CloseTaskWnd() {
		taskWnd.SetWndState(false);
	}
    #endregion
    #region Buy Wnd
    public void OpenBuyWnd(int type) {
		StopNavTask();
		buyWnd.SetBuyType(type);
		buyWnd.SetWndState();
	}
	public void RspBuy(GameMsg msg) {
		RspBuy rspBuy = msg.rspBuy;
		GameRoot.Instance.SetPlayerDataByBuy(rspBuy);
		GameRoot.AddTips("Purchased sucessfully");
		mainCityWnd.RefreshUI();
		buyWnd.SetWndState(false);
		if(msg.pshTaskPrgs != null) {
			GameRoot.Instance.SetPlayerDataByTaskPsh(msg.pshTaskPrgs);

			if (taskWnd.GetWndState()) {
				taskWnd.RefreshUI();
			}

		}
		buyWnd.btnSure.interactable = false;
	}
	public void PshPower(GameMsg msg) {
		PshPower data = msg.pshPower;
		GameRoot.Instance.SetPlayerDataByPower(data);
		if (mainCityWnd.GetWndState()) {
			mainCityWnd.RefreshUI();
		}

	}
	#endregion
	#region Chat Wnd
	public void OpenChatWnd() {
		StopNavTask();
		chatWnd.SetWndState(true);
	}

	public void PshChat(GameMsg msg) {
		chatWnd.AddChatMsg(msg.pshChat.name, msg.pshChat.chat);
	}
    #endregion
    #region Enhancement Wnd
    public void OpenStrongWnd() {
		StopNavTask();
		strongWnd.SetWndState(true);
	}
	public void RspStrong(GameMsg msg) {
		int fightPre = PECommon.GetFightByProps(GameRoot.Instance.PlayerData);
		GameRoot.Instance.SetPlayerDataByStrong(msg.rspStrong);
		int fightNow = PECommon.GetFightByProps(GameRoot.Instance.PlayerData);
		GameRoot.AddTips(Constants.Color("Update sucessfully! Your Fight incresed by " + (fightNow-fightPre),TxtColor.Blue));
		strongWnd.UpdateUI();

	}


	#endregion
	#region Info Wnd
	public void OpenInfoWnd() {
		StopNavTask();
		if(charCamTrans == null) {
			charCamTrans = GameObject.FindGameObjectWithTag("CharShowCam").transform;
		}
		// set role and camera relative distance/position
		charCamTrans.localPosition = playerCtrl.transform.position + playerCtrl.transform.forward * 2.7f + new Vector3(0,1.2f,0);
		charCamTrans.localEulerAngles = new Vector3(0, 180 + playerCtrl.transform.localEulerAngles.y, 0);
		charCamTrans.localScale = Vector3.one;
		charCamTrans.gameObject.SetActive(true);
		InfoWnd.SetWndState(true);
	}
	public void CloseInfoWnd() {
		if(charCamTrans != null) {
			charCamTrans.gameObject.SetActive(false);
			InfoWnd.SetWndState(false);
		}
	}

	private float startRotate = 0;
	public void SetStartRotate() {
		startRotate = playerCtrl.transform.localEulerAngles.y;
	}
    public void SetPlayerRotate(float rotate) {
		playerCtrl.transform.localEulerAngles = new Vector3(0, startRotate + rotate, 0);
	}
    #endregion
    #region Guide Wnd
    private bool isNavGuide = false;
	public void RunTask(AutoGuideCfg agc) {
		if(agc!=null) {
			curtTaskData = agc;
		}

		//load task data, and do some relative opration
		nav.enabled = true;
		if (curtTaskData.npcID != -1) {
			float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[agc.npcID].position);
			if(dis<0.5) {
				// if the distance between npc and player is less than 0.5, that means the player has already encountered the npc, then stop nav-system
				StopNavTask();
				OpenGuideWnd();
			} else {
				// isnt reach, go to the npc
				isNavGuide = true;
				nav.enabled = true;
				nav.speed = Constants.PlayerMoveSpeed;
				nav.SetDestination(npcPosTrans[agc.npcID].position);
				playerCtrl.SetBlend(Constants.BlendMove);
			}
		}else {
			OpenGuideWnd();
		}
	}

	private void Update() {
		if(isNavGuide) {
			isArriveNavPos();
			playerCtrl.SetCam();
		}
	}
	private void isArriveNavPos() {
		float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[curtTaskData.npcID].position);
		if (dis < 0.5) {
			// if the distance between npc and player is less than 0.5, that means the player has already encountered the npc, then stop nav-system
			StopNavTask();
			OpenGuideWnd();
		}
	}
	private void StopNavTask() {
		if(isNavGuide) {
			isNavGuide = false;
			nav.isStopped = true;
			nav.enabled = false;
			playerCtrl.SetBlend(Constants.BlendIdle);
		}
	}
	private void OpenGuideWnd() {
		guideWnd.SetWndState();
	}
	public AutoGuideCfg GetCurtTaskData() {
		return curtTaskData;
	}
	public void RspGuide(GameMsg msg) {
		RspGuide data = msg.rspGuide;
		GameRoot.AddTips(Constants.Color("You get coin " + curtTaskData.coin + ", exp " + curtTaskData.exp,TxtColor.Blue));
		switch(curtTaskData.actID) {
			case 0:
				// first Task ,einfach dialog with wiser
				break;
			case 1:
				EnterMission();
				// second Task, combat 
				break;
			case 2:
				// Enhancement
				OpenStrongWnd();
				break;
			case 3:
				// buy power
				OpenBuyWnd(0);
				break;
			case 4:
				// buy coin
				OpenBuyWnd(1);
				break;
			case 5:
				// chat with world
				OpenChatWnd();
				break;
		}
		GameRoot.Instance.SetPlayerDataByGuide(data);
		mainCityWnd.RefreshUI();
	}
	#endregion
	
}
