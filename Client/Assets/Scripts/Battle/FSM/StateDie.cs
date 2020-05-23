﻿public class StateDie: IState {
    public void Enter (EntityBase entity, params object[] args) {
        entity.currentAniState = AniState.Die;
        entity.RmvSkillCB();
    }

    public void Exit (EntityBase entity, params object[] args) {
    }

    public void Process (EntityBase entity, params object[] args) {
        entity.SetAction(Constants.ActionDie);
        if(entity.entityType == EntityType.Monster) {
            TimerSvc.Instance.AddTimerTask((int tid) => {
                    entity.SetActive(false);
            }, Constants.DieAniLength);
        }
    }
}
