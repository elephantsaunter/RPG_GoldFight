using UnityEngine;

public class MapMgr: MonoBehaviour {
    private int waveIndex = 1; // default to generate first wave
    private BattleMgr battleMgr;
    public TriggerData[] triggerArr;
    public void Init(BattleMgr battle) {
        this.battleMgr = battle;
        // init first wave monster
        battleMgr.LoadMonsterByWaveID(waveIndex);
        PECommon.Log("Init MapMgr Done");
    }
    public void TriggerMonsterBorn(TriggerData trigger, int waveIndex) {
        if(battleMgr != null) {
            BoxCollider co = trigger.gameObject.GetComponent<BoxCollider>();
            co.isTrigger = false;
            battleMgr.LoadMonsterByWaveID(waveIndex);
            battleMgr.ActiveCurrrentBatchMonster();
            battleMgr.triggerCheck = true;
        }
    }
    public bool SetNextTriggerOn() {
        waveIndex += 1;
        if(waveIndex < 4) {
            GameRoot.AddTips("You can enter the Nr." + waveIndex + " wave now!");
        }
        for(int i = 0; i < triggerArr.Length; i++) {
            if(triggerArr[i].triggerWave == waveIndex) {
                BoxCollider co = triggerArr[i].GetComponent<BoxCollider>();
                co.isTrigger = true;
                return true;
            }
        }
        return false;
    }
}
