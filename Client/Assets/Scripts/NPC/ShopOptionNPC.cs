using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOptionNPC:WindowRoot {
    public void OnMouseOver() {

        if (Input.GetMouseButtonDown(0)) {
            //OptionWnd.Instantiate
            GameRoot.AddTips("Hallo");
            MissionSys.Instance.OpenOptionWnd();

            //GameObject go = resSvc.LoadPrefab(PathDefine.TaskItemPrefab);
        }
    }
}
