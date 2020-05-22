using UnityEngine;

public class MonsterController: Controller {
    private void Update () {
        // AI logic
        if (isMove) {
            SetDir();
            SetMove();
        }
    }
    private void SetDir () {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1));
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }
    private void SetMove () {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.MonsterMoveSpeed);
        // this 3d monster modell has some bug, cannot drop, to fix it
        ctrl.Move(Vector3.down * Time.deltaTime * Constants.MonsterMoveSpeed);
    }
}
