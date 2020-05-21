using UnityEngine;

public class MapMgr: MonoBehaviour {
    private int waveIndex = 1; // default to generate first wave
    private BattleMgr battleMgr;
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
        }
    }
}
