using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUP : MonoBehaviour {

    private float timestamp_Spawn;
    public float lifespan = 20;
    
    void Start () {
        //set timestamp to record age
        timestamp_Spawn = Time.time;
    }
	
	void Update () {
        if (Time.time > lifespan + timestamp_Spawn)//test if lifespan has elapsed
            //remove artifact from play
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject collector = collider.gameObject;
        if (collector.tag.Equals("Player"))//test if player
        {
            GameController gameController = GameController.s_Instance;
            if (gameController == null)
                Debug.Log("GameController was not instantiated");
            else
                gameController.applyPowerUP();

            Destroy(gameObject);
        }
    }
}
