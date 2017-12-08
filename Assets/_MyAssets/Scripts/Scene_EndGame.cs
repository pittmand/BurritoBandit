using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_EndGame : MonoBehaviour {

    public float duration_FadeIn = 3;
    public float duration_ExitDelay = 5;
    private float timestamp_loaded;
    private bool flag = true;
    private CanvasGroup group; 
    
    void Start () {
        timestamp_loaded = Time.unscaledTime;
        group = gameObject.GetComponent<CanvasGroup>();
    }
	
    // Update is called once per frame
    void Update () {

        float prog = Time.unscaledTime - timestamp_loaded;
        
        group.alpha = Mathf.Min(prog / duration_FadeIn, 1.0f);

        if (flag && prog >= duration_ExitDelay)
        {
            if(Input.GetButton("Submit") || Input.GetButton("Cancel"))
            {
                GameController.s_Instance.ReturnToMain();
                flag = false;
            }
        }
    }
}
