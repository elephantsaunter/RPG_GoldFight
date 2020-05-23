using PEProtocol;
using System.Collections.Generic;
using UnityEngine;

public class BattleMgr: MonoBehaviour {
    private ResSvc resSvc;
    private AudioSvc audioSvc;

    private StateMgr stateMgr;
    private SkillMgr skillMgr;
    private MapMgr mapMgr;

    public EntityPlayer entitySelfPlayer;
    private MapCfg mapCfg;

    private Dictionary<string, EntityMonster> monsterDic = new Dictionary<string, EntityMonster>();
    public void Init (int mapid) {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
        // Initialize all manager
        stateMgr = gameObject.AddComponent<StateMgr>();
        stateMgr.Init();
        skillMgr = gameObject.AddComponent<SkillMgr>();
        skillMgr.Init();
        // load combat map
        mapCfg = resSvc.GetMapCfg(mapid);
        resSvc.AsyncLoadScene(mapCfg.sceneName, () => {
            // initialize map data
            GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
            mapMgr = map.GetComponent<MapMgr>();
            mapMgr.Init(this);
            map.transform.localPosition = Vector3.zero;
            map.transform.localScale = Vector3.one;

            Camera.main.transform.position = mapCfg.mainCamPos;
            Camera.main.transform.localEulerAngles = mapCfg.mainCamRote;

            LoadPlayer(mapCfg);
            entitySelfPlayer.Idle();
            // activalization first batch monster
            ActiveCurrrentBatchMonster();
            audioSvc.PlayBGMusic(Constants.BGLeipzig);
        });
    }
    public bool triggerCheck = true;
    public void Update() {
        foreach(var item in monsterDic) {
            EntityMonster em = item.Value;
            em.TickAILogic();
        }
        // check this wave whether monster all dead
        if(mapCfg != null && mapMgr != null) {
            if(triggerCheck && monsterDic.Count == 0) {
                bool isExist = mapMgr.SetNextTriggerOn();
                triggerCheck = false;
                if(!isExist) {
                    // all waves were completed, mission sucess
                    EndBattle(entitySelfPlayer.HP);
                    // TODO reward calc
                }
            }
        }
    }
    public void EndBattle(int restHP) {
        AudioSvc.Instance.StopBGMusic();
        AudioSvc.Instance.StopBGMusic();
        BattleSys.Instance.EndBattle(restHP);
    }
    private void LoadPlayer(MapCfg mapCfg) {
        GameObject player = resSvc.LoadPrefab(PathDefine.AssissinBattlePlayerPrefab);
        player.transform.position = mapCfg.playerBornPos;
        player.transform.localEulerAngles = mapCfg.playerBornRote;
        player.transform.localScale = Vector3.one;

        PlayerData pd = GameRoot.Instance.PlayerData;
        BattleProps props = new BattleProps {
            hp = pd.hp,
            ad = pd.ad,
            ap = pd.ap,
            addef = pd.addef,
            apdef = pd.apdef,
            dodge = pd.dodge,
            pierce = pd.pierce,
            critical = pd.critical,
        };

        entitySelfPlayer = new EntityPlayer {
            battleMgr = this,
            stateMgr = stateMgr,
            skillMgr = skillMgr
        };
        entitySelfPlayer.Name = "AssassinBattle";
        entitySelfPlayer.SetBattleProps(props);
        PlayerController playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();
        entitySelfPlayer.SetCtrl(playerCtrl);
    }
    public void LoadMonsterByWaveID(int wave) {
        for(int i = 0; i < mapCfg.monsterLst.Count; i++) {
            MonsterData md = mapCfg.monsterLst[i];
            if(md.mWave==wave) {
                // this wave is this monster's wave,this monster should be released
                GameObject m = resSvc.LoadPrefab(md.mCfg.resPath,true);
                m.transform.localPosition = md.mBornPos;
                m.transform.localEulerAngles = md.mBornRote;
                m.transform.localScale = Vector3.one;
                m.name = "m" + md.mWave + "_" + md.mIndex;
                EntityMonster em = new EntityMonster {
                    battleMgr = this,
                    stateMgr = stateMgr,
                    skillMgr = skillMgr,
                };
                // give it initialized 
                em.md = md;
                em.SetBattleProps(md.mCfg.bps);
                em.Name = m.name;
                //em.SetBattleProps();
                MonsterController mc = m.GetComponent<MonsterController>();
                mc.Init();
                em.SetCtrl(mc);

                m.SetActive(false);
                monsterDic.Add(m.name, em);
                if(md.mCfg.mType == MonsterType.Normal) {
                    GameRoot.Instance.dynamicWnd.AddHpItemInfo(m.name,mc.hpRoot, em.HP);
                } else if (md.mCfg.mType == MonsterType.Boss) {
                    BattleSys.Instance.playerCtrlWnd.SetBossHPBarState(true);
                }
            }
        }
    }
    public void ActiveCurrrentBatchMonster() {
        TimerSvc.Instance.AddTimerTask((int tid) => {
            foreach(var item in monsterDic) {
                item.Value.SetActive(true);
                item.Value.Born();
                TimerSvc.Instance.AddTimerTask((int ttid) => {
                    // After 1 second born, switch to idle state
                    item.Value.Idle();
                }, 1000);
            }
        }, 500);
    }
    public List<EntityMonster> GetEntityMonsters() {
        List<EntityMonster> monsterLst = new List<EntityMonster>();
        foreach(var item in monsterDic) {
            monsterLst.Add(item.Value);
        }
        return monsterLst;
    }
    public void RmvMonster(string key) {
        EntityMonster entityMonster;
        if(monsterDic.TryGetValue(key,out entityMonster)) {
            monsterDic.Remove(key);
            GameRoot.Instance.dynamicWnd.RmvHpItemInfo(key);
        }
    }
    #region skill release and role kontrolliert
    public void SetSelfPlayerMoveDir(Vector2 dir) {
        // set player mov
        // PECommon.Log(dir.ToString());
        if(entitySelfPlayer.canControl == false) {
            return;
        }
        if(entitySelfPlayer.currentAniState == AniState.Idle || entitySelfPlayer.currentAniState == AniState.Move) {
            if(dir ==  Vector2.zero) {
                entitySelfPlayer.Idle();
            } else {
                entitySelfPlayer.Move();
                entitySelfPlayer.SetDir(dir);
            }
        }
    }
    public void ReqReleaseSkill(int index) {
        switch(index) {
            case 0:
                ReleaseNormalAtk();
                break;
            case 1:
                ReleaseSkill1();
                break;
            case 2:
                ReleaseSkill2();
                break;
            case 3:
                ReleaseSkill3();
                break;
        }
    }
    public double lastAtkTime = 0;
    private int[] comboArr = new int[] { 111, 112, 113, 114, 115 };
    public int comboIndex = 0;
    private void ReleaseNormalAtk() {
        //PECommon.Log("Click Normal Atk");
        if(entitySelfPlayer.currentAniState == AniState.Attack) {
            // attack state in 500ms do second continous attack
            double nowAtkTime = TimerSvc.Instance.GetNowTime();
            if(nowAtkTime-lastAtkTime< Constants.ComboSpace && lastAtkTime != 0) {
                if(comboArr[comboIndex] != comboArr[comboArr.Length-1]) {
                    comboIndex += 1;
                    entitySelfPlayer.comboQue.Enqueue(comboArr[comboIndex]);
                    lastAtkTime = nowAtkTime;
                } else {
                    lastAtkTime = 0;
                    comboIndex = 0;
                }
            }
        } 
        else if(entitySelfPlayer.currentAniState == AniState.Idle 
            || entitySelfPlayer.currentAniState == AniState.Move) {
            comboIndex = 0;
            lastAtkTime = TimerSvc.Instance.GetNowTime();
            entitySelfPlayer.Attack(comboArr[comboIndex]);
        }
    }
    private void ReleaseSkill1 () {
        //PECommon.Log("Click Normal Atk");
        entitySelfPlayer.Attack(101);
    }
    private void ReleaseSkill2 () {
        //PECommon.Log("Click Normal Atk");
        entitySelfPlayer.Attack(102);
    }
    private void ReleaseSkill3() {
        //PECommon.Log("Click Normal Atk");
        entitySelfPlayer.Attack(103);
    }
    public Vector2 GetDirInput() {
        return BattleSys.Instance.GetDirInput();
    }
    public bool CanRisSkill() {
        return entitySelfPlayer.canRisSkill;
    }
    #endregion
}
