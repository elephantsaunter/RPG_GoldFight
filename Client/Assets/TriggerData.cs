using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerData : MonoBehaviour {
    public int triggerWave;
    public MapMgr mapMgr;
    public void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player") {
            if(mapMgr != null) {
                mapMgr.TriggerMonsterBorn(this,triggerWave);
            }
        }
    }
}
