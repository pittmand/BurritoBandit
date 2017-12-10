using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyPlaySteps : MonoBehaviour {
    private AudioSource[] soundHandler;
    private AudioSource _stepSound;
    private int starttime;
    private int newtime;
    private int curtime;

    // Use this for initialization
    void Start () {
        soundHandler = GetComponents<AudioSource>();
        _stepSound = soundHandler[1];
        starttime = (int)Time.time;
        newtime = starttime + (int)Random.Range(3, 20);
    }
	
	// Update is called once per frame
	void Update () {
        curtime = (int)Time.time;
        if (curtime == newtime)
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
        newtime = curtime + (int)Random.Range(3, 10);

    }
}
