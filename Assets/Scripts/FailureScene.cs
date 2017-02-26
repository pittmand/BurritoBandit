using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailureScene : MonoBehaviour {

    public float duration_FadeIn = 3;
    public float duration_ExitDelay = 5;
    private float timestamp_loaded;
    private bool flag = true;
    
    void Start () {
        timestamp_loaded = Time.unscaledTime;
    }
	
	// Update is called once per frame
	void Update () {
        if (flag && Time.unscaledTime >= timestamp_loaded + duration_ExitDelay)
        {
            if(Input.GetButton("Submit") || Input.GetButton("Cancel"))
            {
                GameController.s_Instance.ReturnToMain();
                flag = false;
            }
        }
	}
}
