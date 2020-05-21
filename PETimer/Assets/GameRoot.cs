using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour {
    private void Awake() {
        Debug.Log("Awake:" + gameObject.name);
    }
    //private void Start() {
    //    Debug.Log("Start:" + gameObject.name);
    //}
}
