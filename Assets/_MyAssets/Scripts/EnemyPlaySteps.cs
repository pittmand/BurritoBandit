using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyPlaySteps : MonoBehaviour {
    public AudioSource _stepSound;
    private float starttime;
    private float newtime;
    private float curtime;

    // Use this for initialization
    void Start () {
        curtime = starttime = Time.time;
        gennewtime();
    }
	
	// Update is called once per frame
	void Update () {
        curtime = Time.time;
        if (curtime >= newtime)
        {
            play();
        }
	}

    void play()
    {
        _stepSound.Play();
        gennewtime();
    }

    void gennewtime() {
        newtime = curtime + Random.Range(1, 1.2f);
    }
}
