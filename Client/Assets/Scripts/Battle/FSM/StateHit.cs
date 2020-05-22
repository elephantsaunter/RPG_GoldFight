using UnityEngine;

public class StateHit: IState {
    public void Enter (EntityBase entity, params object[] args) {
        entity.currentAniState = AniState.Hit;
        for(int i = 0;i<entity.skMoveCBLst.Count;i++) {
            int tid = entity.skMoveCBLst[i];
            TimerSvc.Instance.DelTask(tid);
        }
        for (int i = 0; i < entity.skActionCBLst.Count; i++) {
            int tid = entity.skActionCBLst[i];
            TimerSvc.Instance.DelTask(tid);
        }
    }

    public void Exit (EntityBase entity, params object[] args) {
    }

    public void Process (EntityBase entity, params object[] args) {
        entity.SetDir(Vector2.zero);
        entity.SetAction(Constants.ActionHit);

        if(entity.entityType == EntityType.Player) {
            AudioSource charAudio = entity.GetAudio();
            AudioSvc.Instance.PlayCharAudio(Constants.AssassinHit,charAudio);
        }
        TimerSvc.Instance.AddTimerTask((int tid) => {
            entity.SetAction(Constants.ActionDefault);
            entity.Idle();
        },(int)(GetHitAniLen(entity) * 1000));
    }

    private float GetHitAniLen(EntityBase entity) {
        AnimationClip[] clips = entity.GetAniClips();
        for(int i = 0; i< clips.Length; i++) {
            string clipName = clips[i].name;
            if(clipName.Contains("hit") || clipName.Contains("Hit") || clipName.Contains("HIT")) {
                return clips[i].length;
            }
        }
        // ensure that if the ani is not found, default return 1s
        return 1;
    }
}