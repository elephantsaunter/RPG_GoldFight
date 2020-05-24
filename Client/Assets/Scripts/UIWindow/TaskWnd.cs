using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskWnd : WindowRoot{
	public Transform scrollTrans;
	private PlayerData pd = null;
	private List<TaskRewardData> trdLst = new List<TaskRewardData>();
	protected override void InitWnd () {
		base.InitWnd();
		pd = GameRoot.Instance.PlayerData;
		RefreshUI();
	}

	public void RefreshUI() {
		trdLst.Clear();

		List<TaskRewardData> todoLst = new List<TaskRewardData>();
		List<TaskRewardData> doneLst = new List<TaskRewardData>();

		// 1|0|0
		for(int i = 0;i < pd.taskArr.Length;i++) {
			string[] taskInfo = pd.taskArr[i].Split('|');
			TaskRewardData trd = new TaskRewardData {
				ID = int.Parse(taskInfo[0]),
				prgs = int.Parse(taskInfo[1]),
				taked = taskInfo[2].Equals("1")
			};

			if (trd.taked) {
				doneLst.Add(trd);
			} else {
				todoLst.Add(trd);
			}
		}
		// make sure the sequence that the to-do task is always before the done task
		trdLst.AddRange(todoLst);
		trdLst.AddRange(doneLst);

		for(int i = 0;i < scrollTrans.childCount;i++) {
			Destroy(scrollTrans.GetChild(i).gameObject);
		}

		for(int i = 0;i < trdLst.Count;i++) {
			GameObject go = resSvc.LoadPrefab(PathDefine.TaskItemPrefab);
			go.transform.SetParent(scrollTrans);
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one;
			go.name = "taskItem_" + i;

			TaskRewardData trd = trdLst[i];
			TaskRewardCfg trc = resSvc.GetTaskRewardCfg(trd.ID);

			SetText(GetTrans(go.transform, "txtName"),trc.taskName);
			SetText(GetTrans(go.transform, "txtPrg"),trd.prgs + "/" + trc.count);
			SetText(GetTrans(go.transform, "txtExp"), "Reward :               " + trc.exp + " exp");
			SetText(GetTrans(go.transform, "txtCoin"),trc.coin + " coin");
			Image imgPrg = GetTrans(go.transform, "prgBar/prgVal").GetComponent<Image>();
			float prgVal = trd.prgs * 1.0f / trc.count;
			imgPrg.fillAmount = prgVal;

			Button btnTake = GetTrans(go.transform, "btnTake").GetComponent<Button>();
			btnTake.onClick.AddListener(() => {
				ClickTakeBtn(go.name);
			});

			Transform transComp = GetTrans(go.transform, "imgComp");
			if(trd.taked) {
				btnTake.interactable = false;
				SetActive(transComp);
			} else {
				SetActive(transComp, false);
				if(trd.prgs == trc.count) {
					// task is done
					btnTake.interactable = true;
				} else {
					btnTake.interactable = false;
				}
			}
		}
	}

	private void ClickTakeBtn(string name) {
		Debug.Log("name" + name);
		string[] nameArr = name.Split('_');
		int index = int.Parse(nameArr[1]);
		GameMsg msg = new GameMsg {
			cmd = (int)CMD.ReqTakeTaskReward,
			reqTakeTaskReward = new ReqTakeTaskReward {
				rid = trdLst[index].ID
			}
		};
		netSvc.SendMsg(msg);
		TaskRewardCfg trc = resSvc.GetTaskRewardCfg(trdLst[index].ID);
		int coin = trc.coin;
		int exp = trc.exp;
		GameRoot.AddTips(Constants.Color("Rewards£º", TxtColor.Blue) + Constants.Color(" Coin +" + coin + " Exo +" + exp, TxtColor.Green));

	}
	public void ClickCloseBtn () {
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		MainCitySys.Instance.CloseTaskWnd();
	}
}
