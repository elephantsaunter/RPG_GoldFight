using UnityEngine;

public class StateHit: IState {
    public void Enter (EntityBase entity, params object[] args) {
        entity.currentAniState = AniState.Hit;
    }

    public void Exit (EntityBase entity, params object[] args) {
    }

    public void Process (EntityBase entity, params object[] args) {
        entity.SetDir(Vector2.zero);
        entity.SetAction(Constants.ActionHit);
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
        // ensure that the ani is not found, default return 1s
        return 1;
    }
}