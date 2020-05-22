using System.Collections.Generic;
using UnityEngine;

public class SkillMgr:MonoBehaviour {
    private ResSvc resSvc;
    private TimerSvc timerSvc;
    public void Init() {
        resSvc = ResSvc.Instance;
        timerSvc = TimerSvc.Instance;
        PECommon.Log("Init SkillMgr Done");
    }
    public void SkillAttack(EntityBase entity,int skillID) {
        AttackDamage(entity, skillID);
        AttackEffect(entity, skillID);

    }
    public void AttackDamage(EntityBase entity,int skillID){
        SkillCfg skillCfg = resSvc.GetSkillCfg(skillID);
        if(!skillCfg.isCollide) {
            // ignore the physical collider layer 9 is player, layer 10 is monster
            Physics.IgnoreLayerCollision(9, 10);
            timerSvc.AddTimerTask((int tid) => {
                Physics.IgnoreLayerCollision(9, 10, false);
            }, skillCfg.skillTime);
        }
        List<int> actionLst = skillCfg.skillActionLst;
        int sum = 0; 
        for(int i = 0; i< actionLst.Count; i++) {
            SkillActionCfg skillActionCfg = resSvc.GetSkillActionCfg(actionLst[i]);
            sum += skillActionCfg.delayTime;
            int index = i;
            if(sum > 0) {
                timerSvc.AddTimerTask((int tid) => {
                    SkillAction(entity,skillCfg,index);
                },sum);
            } else {
                // no need to wait,do the attack immediately
                SkillAction(entity, skillCfg,index);
            }
        }

    }

    public void SkillAction(EntityBase caster,SkillCfg skillCfg, int index) {
        SkillActionCfg skillActionCfg = resSvc.GetSkillActionCfg(skillCfg.skillActionLst[index]);
        int damage = skillCfg.skillDamageLst[index];
        if(caster.entityType == EntityType.Monster) {
            // monster attack the player
            EntityPlayer target = caster.battleMgr.entitySelfPlayer;
            // check distance and angle
            if (InRange(caster.GetPos(), target.GetPos(), skillActionCfg.radius)
                && InAngle(caster.GetTrans(), target.GetPos(), skillActionCfg.angle)) {
                CalcDamage(caster, target, skillCfg, damage);
            }
        } else if (caster.entityType == EntityType.Player) {
            // player attack the monster
            // obtain all monster in this scene, calc the damage
            List<EntityMonster> monsterLst = caster.battleMgr.GetEntityMonsters();
            for(int i = 0; i < monsterLst.Count; i++) {
                EntityMonster em = monsterLst[i];
                // check distance and angle
                if(InRange(caster.GetPos(), em.GetPos(),skillActionCfg.radius)
                    && InAngle(caster.GetTrans(), em.GetPos(),skillActionCfg.angle)) {
                    CalcDamage(caster, em, skillCfg, damage);
                }
            }
        }
    }
    System.Random rd = new System.Random();
    private void CalcDamage(EntityBase caster,EntityBase target,SkillCfg skillCfg, int damage) {
        int dmgSum = damage;
        if(skillCfg.dmgType == DamageType.AD) {
            // calc whether target can avoid
            int dogdeNum = PETools.RDInt(1, 100, rd);
            if(dogdeNum <= target.Props.dodge) {
                // UI shows MISS
                //PECommon.Log("MISS Rate: " + dogdeNum + "/" + target.Props.dodge);
                target.SetDodge();
                return;
            }
            // calc role detail add
            dmgSum += caster.Props.ad;
            // calc critical
            int criticalNum = PETools.RDInt(1, 100, rd);
            if(criticalNum <= caster.Props.critical) {
                // criticalRate =>> 1.0 < cr < 2.0
                float criticalRate = 1 + (PETools.RDInt(1, 100, rd) / 100.0f);
                dmgSum = (int)criticalRate * dmgSum;
                //PECommon.Log("CRITICAL Rate: " + criticalNum + "/" + target.Props.critical);
                target.SetCritical(dmgSum);
            }
            // calc pierce
            int addef = (int)((1 - caster.Props.pierce / 100.0f) * target.Props.addef);
            dmgSum -= addef;
        }
        else if (skillCfg.dmgType == DamageType.AP) {
            // calc role detail add
            dmgSum += caster.Props.ap;
            // calc magical defence
            dmgSum -= target.Props.apdef;
        }
        else {
            // other dmgType
        }

        // final danage
        if(dmgSum < 0) {
            dmgSum = 0;
            return;
        }
        target.SetHurt(dmgSum);
        if(target.HP <= dmgSum) {
            target.HP = 0;
            // target die
            target.Die();
            target.battleMgr.RmvMonster(target.Name);
        }
        else {
            target.HP -= dmgSum;
            if(target.entityState != EntityState.ControlledState) {
                target.Hit();
            }
        }
    }
    private bool InRange(Vector3 from, Vector3 to, float range) {
        // check the distance between the player and monster, if in range
        float dis = Vector3.Distance(from, to);
        if(dis > range) {
            return false;
        }
        return true;
    }
    private bool InAngle(Transform player,Vector3 to, float angle) {
        // check the angle between the player and monster, whether in range
        if(angle==360) {
            return true;
        } else {
            Vector3 start = player.forward;
            Vector3 dir = (to - player.position).normalized; // direction from the player to monster
            float ang = Vector3.Angle(start, dir);
            if(ang <= angle/2) {
                return true;
            }
            return false;
        }
    }
    public void AttackEffect(EntityBase entity, int skillID) {
        SkillCfg skillCfg = resSvc.GetSkillCfg(skillID);
        if(entity.entityType == EntityType.Player) {
            // only player can
            if(entity.GetDirInput()==Vector2.zero) {
                // search to nearest monster, and set the attack direction
                Vector2 dir = entity.CalcTargetDir();
                if(dir != Vector2.zero) {
                    entity.SetAtkRotation(dir);
                }
            } else {
                entity.SetAtkRotation(entity.GetDirInput(),true);
            }
        }
        entity.SetAction(skillCfg.aniAction);
        entity.SetFX(skillCfg.fx, skillCfg.skillTime);
        CalcSkillMove(entity, skillCfg);
        entity.canControl = false;
        entity.SetDir(Vector2.zero);

        if(!skillCfg.isBreak) {
            entity.entityState = EntityState.ControlledState;
        }
        timerSvc.AddTimerTask((int tid) => {
            entity.Idle();
        }, skillCfg.skillTime);
    }

    public void CalcSkillMove(EntityBase entity, SkillCfg skillCfg) {
        List<int> skillMoveLst = skillCfg.skillMoveLst;
        int sumTime = 0;
        for (int i = 0; i < skillMoveLst.Count; i++) {
            SkillMoveCfg skillMoveCfg = resSvc.GetSkillMoveCfg(skillCfg.skillMoveLst[i]);
            float speed = skillMoveCfg.moveDis / (skillMoveCfg.moveTime / 1000f);
            sumTime += skillMoveCfg.delayTime;
            if (sumTime > 0) {
                int moveid = timerSvc.AddTimerTask((int tid) => {
                    entity.SetSkillMoveState(true, speed);
                    entity.RmvMoveCB(tid);
                }, sumTime);
                entity.skMoveCBLst.Add(moveid);
            } else {
                entity.SetSkillMoveState(true, speed);
            }
            sumTime += skillMoveCfg.moveTime;
            int stopid = timerSvc.AddTimerTask((int tid) => {
                entity.SetSkillMoveState(false);
                entity.RmvMoveCB(tid);
            }, sumTime);
            entity.skMoveCBLst.Add(stopid);
        }
    }
}
