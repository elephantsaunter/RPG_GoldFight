﻿using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public abstract class EntityBase {
    public AniState currentAniState = AniState.None;

    public BattleMgr battleMgr = null;
    public StateMgr stateMgr = null;
    public SkillMgr skillMgr = null;
    protected Controller controller = null;
    private string name;
    public string Name {
        get {
            return name;
        }

        set {
            name = value;
        }
    }
    public bool canControl = true;
    public bool canRisSkill = true;
    public EntityType entityType = EntityType.None;
    public EntityState entityState = EntityState.None;
    private BattleProps props;
    public BattleProps Props {
        get {
            return props;
        }

        protected set {
            props = value;
        }
    }
    private int hp;
    public int HP {
        get {
            return hp;
        }

        set {
            // send msg to UI-layer
            PECommon.Log("hp change: " + hp + " to " + value);
            SetHPVal(hp, value);
            hp = value;
        }
    }
    public Queue<int> comboQue = new Queue<int>();
    public int nextSkillID = 0;
    public SkillCfg curtSkillCfg;
    // skill move CallBack ID
    public List<int> skMoveCBLst = new List<int>();
    // skill hurt calc callback ID
    public List<int> skActionCBLst = new List<int>();
    public int skEndCB = -1;
    public void Born () {
        stateMgr.ChangeStatus(this, AniState.Born, null);
    }
    public void Move () {
        stateMgr.ChangeStatus(this, AniState.Move,null);
    }
    public void Idle() {
        stateMgr.ChangeStatus(this, AniState.Idle,null);
    }
    public void Attack(int skillID) {
        stateMgr.ChangeStatus(this, AniState.Attack, skillID);
    }
    public void Hit() {
        stateMgr.ChangeStatus(this, AniState.Hit, null);
    }
    public void Die() {
        stateMgr.ChangeStatus(this, AniState.Die,null);
    }
    public virtual void TickAILogic() {

    }
    public void SetCtrl(Controller ctrl) {
        controller = ctrl;
    }
    public void SetActive(bool active=true) {
        if(controller != null) {
            controller.gameObject.SetActive(active);
        }
    }
    public virtual void TickAILogik() {

    }
    public virtual void SetBattleProps(BattleProps props) {
        this.HP = props.hp;
        this.Props = props;
    }
    public virtual void SetBlend(float blend) {
        if(controller != null) {
            controller.SetBlend(blend);
        }
    }
    public virtual void SetDir(Vector2 dir) {
        if (controller != null) {
            controller.Dir = dir;
        }
    }
    public virtual void SetAction(int act) {
        if (controller != null) {
            controller.SetAction(act);
        }
    }
    public virtual void SetFX (string name, float destroy) {
        if (controller != null) {
            controller.SetFX(name, destroy);
        }
    }
    public virtual void SetSkillMoveState(bool move, float speed=0f) {
        if (controller != null) {
            controller.SetSkillMoveState(move,speed);
        }
    }
    public virtual void SetAtkRotation(Vector2 dir,bool offset=false) {
        if(controller != null) {
            if(offset) {
                controller.SetatkRotationCam(dir);
            }
            else {
                controller.SetatkRotationLocal(dir);
            }
        }
    }
    #region FightInfoDisplay
    public virtual void SetDodge () {
        if(controller != null) {
            GameRoot.Instance.dynamicWnd.SetDodge(Name);
        }
    }
    public virtual void SetCritical (int critical) {
        if (controller != null) {
            GameRoot.Instance.dynamicWnd.SetCritical(Name, critical);
        }
    }
    public virtual void SetHurt (int hurt) {
        if(controller != null) {
            GameRoot.Instance.dynamicWnd.SetHurt(Name,hurt);
        }
    }
    public virtual void SetHPVal (int oldVal,int newVal) {
        if(controller != null) {
            GameRoot.Instance.dynamicWnd.SetHPVal(Name, oldVal,newVal);
        }
    }
    #endregion
    public virtual void SkillAttack(int skillID) {
        skillMgr.SkillAttack(this, skillID);
    }
    public virtual Vector2 GetDirInput() {
        return Vector2.zero;
    }
    public virtual Vector3 GetPos() {
        return controller.transform.position;
    }
    public virtual Transform GetTrans() {
        return controller.transform;
    }
    public AnimationClip[] GetAniClips() {
        if(controller!=null) {
            return controller.ani.runtimeAnimatorController.animationClips;
        }
        return null;
    }
    public AudioSource GetAudio() {
        return controller.GetComponent<AudioSource>();
    }
    public CharacterController GetCC() {
        return controller.GetComponent<CharacterController>();
    }
    public virtual bool GetBreakState() {
        return true;
    }
    public virtual Vector2 CalcTargetDir() {
        return Vector2.zero;
    }
    public void ExitCurtSkill () {
        canControl = true;
        if(curtSkillCfg != null) {
            if(!curtSkillCfg.isBreak) {
                entityState = EntityState.None;
            }
            if(curtSkillCfg.isCombo) {
                // check whether continous
                if (comboQue.Count > 0) {
                    nextSkillID = comboQue.Dequeue();
                } else {
                    nextSkillID = 0;
                }
            }
            curtSkillCfg = null;
        }
        SetAction(Constants.ActionDefault);
    }
    public void RmvActionCB (int tid) {
        int index = -1;
        for (int i = 0; i < skActionCBLst.Count; i++) {
            if (skActionCBLst[i] == tid) {
                // taget is found
                index = i;
                break;
            }
        }
        if (index != -1) {
            skActionCBLst.RemoveAt(index);
        }
    }
    public void RmvMoveCB(int tid) {
        int index = -1;
        for(int i = 0;i <skMoveCBLst.Count; i++) {
            if (skMoveCBLst[i] == tid) {
                // taget is found
                index = i;
                break;
            }
        }
        if(index!=1) {
            skMoveCBLst.RemoveAt(index);
        }
    }
    public void RmvSkillCB() {
        SetDir(Vector2.zero);
        SetSkillMoveState(false);
        for (int i = 0; i < skMoveCBLst.Count; i++) {
            int tid = skMoveCBLst[i];
            TimerSvc.Instance.DelTask(tid);
        }
        for (int i = 0; i < skActionCBLst.Count; i++) {
            int tid = skActionCBLst[i];
            TimerSvc.Instance.DelTask(tid);
        }
        // if attack is interrupted, delete the callback
        if (skEndCB != -1) {
            TimerSvc.Instance.DelTask(skEndCB);
            skEndCB = -1;
        }
        skMoveCBLst.Clear();
        skActionCBLst.Clear();

        // clear continously attack(combo)
        if (nextSkillID != 0 || comboQue.Count > 0) {
            nextSkillID = 0;
            comboQue.Clear();
            battleMgr.lastAtkTime = 0;
            battleMgr.comboIndex = 0;
        }
    }
}
