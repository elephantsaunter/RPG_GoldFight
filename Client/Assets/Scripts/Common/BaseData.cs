// Zeilenzaehler: ^b*[^:b#/]+.*$
// Zeilenzaehler(pure): ^(?!(\s*\*))(?!(\s*\-\-\>))(?!(\s*\<\!\-\-))(?!(\s*\n))(?!(\s*\*\/))(?!(\s*\/\*))(?!(\s*\/\/\/))(?!(\s*\/\/))(?!(\s*\}))(?!(\s*\{))(?!(\s (using))).*$

using System.Collections.Generic;
using UnityEngine;

public class MonsterData: BaseData<MonsterData> {
    public int mWave; // which wave
    public int mIndex; // nth of this wave
    public MonsterCfg mCfg;
    public Vector3 mBornPos;
    public Vector3 mBornRote;
    public int mLevel;
}
public class MonsterCfg: BaseData<MonsterCfg> {
    public string mName;
    public MonsterType mType; // 1,normal; 2,boss
    public bool isStop; // whther it can be interrupt by being beaten
    public string resPath;
    public int skillID;
    public float atkDis;
    public BattleProps bps;
}
public class SkillMoveCfg: BaseData<SkillMoveCfg> {
    public int delayTime;
    public int moveTime;
    public float moveDis;
}
public class SkillActionCfg: BaseData<SkillActionCfg> {
    public int delayTime;
    public float radius;
    public int angle;
}
public class SkillCfg: BaseData<SkillCfg> {
    public string skillName;
    public int cdTime;
    public int skillTime;
    public int aniAction;
    public string fx;
    public bool isCombo;
    public bool isCollide;
    public bool isBreak;
    public DamageType dmgType;
    public List<int> skillMoveLst;
    public List<int> skillActionLst;
    public List<int> skillDamageLst;
}
public class StrongCfg: BaseData<StrongCfg> {
    public int pos;
    public int starlv;
    public int addhp;
    public int addhurt;
    public int adddef;
    public int minlv;
    public int coin;
    public int crystal;
}
public class AutoGuideCfg :BaseData<AutoGuideCfg>{
    public int npcID;
    public string dialogArr;
    public int actID;
    public int coin;
    public int exp;
}
public class MapCfg: BaseData<MapCfg> {
    public string mapName;
    public string sceneName;
    public int power;
    public Vector3 mainCamPos;
    public Vector3 mainCamRote;
    public Vector3 playerBornPos;
    public Vector3 playerBornRote;
    public List<MonsterData> monsterLst;

    public int coin;
    public int exp;
    public int crystal;
}

public class TaskRewardCfg: BaseData<TaskRewardCfg> {
    public string taskName;
    public int count;
    public int exp;
    public int coin;
}

public class TaskRewardData: BaseData<TaskRewardData> {
    public int prgs;
    public bool taked;
}
public class BaseData<T> {
    public int ID;
}

public class BattleProps {
    public int hp;
    public int ad;
    public int ap;
    public int addef;
    public int apdef;
    public int dodge;
    public int pierce;
    public int critical;
}
