using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour {

    private Transform camTrans;
    private Vector3 camOffset;
    public CharacterController ctrl;
    public GameObject daggerskill1fx;

    private float targetBlend;
    private float currentBlend;

    public Animator ani;
    // test current whether its move to decide whether use Set___()
    protected bool isMove = false;
    // player direction
    private Vector2 dir = Vector2.zero;
    public Vector2 Dir {
        get {
            return dir;
        }

        set {
            if (value == Vector2.zero) {
                isMove = false;
            } else {
                isMove = true;
            }
            dir = value;
        }
    }
    public void Start () {
        camTrans = Camera.main.transform;
        camOffset = transform.position - camTrans.position;
    }
    private void Update () {
        #region Input
        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector2 _dir = new Vector2(h, v).normalized;
        if (_dir != Vector2.zero) {
            Dir = _dir;
            SetBlend(Constants.BlendMove);// 1
        } else {
            Dir = Vector2.zero;
            SetBlend(Constants.BlendIdle);// 0
        }
        
        #endregion
        if (currentBlend != targetBlend) {
            UpdateMixBlend();
        }

        if (isMove) {
            SetDir();// set direction
            SetMove(); // move
            SetCam();// camera follow
        }
    }

    private void SetDir () {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1)) + camTrans.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    private void SetMove () {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.PlayerMoveSpeed);
    }

    public void SetCam () {
        if (camTrans != null) {
            camTrans.position = transform.position - camOffset;
        }
    }

    public void SetBlend (float blend) {
        targetBlend = blend;
    }

    private void UpdateMixBlend () {
        // let the action nict so ploetzlich
        if (Mathf.Abs(currentBlend - targetBlend) < Constants.AccelerSpeed * Time.deltaTime) {
            currentBlend = targetBlend;
        } else if (currentBlend > targetBlend) {
            currentBlend -= Constants.AccelerSpeed * Time.deltaTime;
        } else {
            currentBlend += Constants.AccelerSpeed * Time.deltaTime;

        }
        ani.SetFloat("Blend", currentBlend);
    }

    public void ClickSkill1Btn() {
        ani.SetInteger("Action", 1);
        daggerskill1fx.gameObject.SetActive(true);
        StartCoroutine(Delay());
    }

    IEnumerator Delay() {
        yield return new WaitForSeconds(1f);
        ani.SetInteger("Action", -1);
        daggerskill1fx.gameObject.SetActive(false);
    }
}
