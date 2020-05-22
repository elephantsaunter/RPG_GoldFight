using UnityEngine;

class StateIdle: IState {
    public void Enter (EntityBase entity, params object[] args) {
        //throw new System.NotImplementedException();
        entity.currentAniState = AniState.Idle;
        entity.SetDir(Vector2.zero);
        //PECommon.Log("Enter StateIdle");
    }

    public void Exit (EntityBase entity, params object[] args) {
        //PECommon.Log("Exit StateIdle");
    }

    public void Process (EntityBase entity, params object[] args) {
        if(entity.nextSkillID != 0) {
            // continously attack
            entity.Attack(entity.nextSkillID);
        } else {
            if(entity.entityType == EntityType.Player) {
                entity.canRisSkill = true;
            }
            if (entity.GetDirInput() != Vector2.zero) {
                // Youre player, and you have controlled the direction
                entity.Move();
                entity.SetDir(entity.GetDirInput());
            } else {
                // You re monster
                entity.SetBlend(Constants.BlendIdle);
            }
            //PECommon.Log("Process StateIdle");
        }
    }
}
