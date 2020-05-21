using UnityEngine;

public class LoopDragonAni: MonoBehaviour {
    private Animation ani;

    private void Awake () {
        ani = transform.GetComponent<Animation>();
    }

    private void Start () {
        if (ani != null) {
            InvokeRepeating("PlayDragonAni", 0, 20);
        }
    }

    private void PlayDragonAni () {
        if (ani != null) {
            ani.Play();
        }
    }
}