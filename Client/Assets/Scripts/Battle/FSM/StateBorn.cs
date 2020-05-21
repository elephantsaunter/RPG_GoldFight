﻿public class StateBorn: IState {
    public void Enter (EntityBase entity, params object[] args) {
        entity.currentAniState = AniState.Born;
    }

    public void Exit (EntityBase entity, params object[] args) {
    }

    public void Process (EntityBase entity, params object[] args) {
        // play born animation
        entity.SetAction(Constants.ActionBorn);
        TimerSvc.Instance.AddTimerTask((int tid) => {
            entity.SetAction(Constants.ActionDefault);
        },500);
    }
}
