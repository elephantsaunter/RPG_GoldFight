public class StateDie: IState {
    public void Enter (EntityBase entity, params object[] args) {
        entity.currentAniState = AniState.Die;
    }

    public void Exit (EntityBase entity, params object[] args) {
    }

    public void Process (EntityBase entity, params object[] args) {
        entity.SetAction(Constants.ActionDie);
        TimerSvc.Instance.AddTimerTask((int tid) => {
            entity.SetActive(false);
        }, Constants.DieAniLength);
    }
}
