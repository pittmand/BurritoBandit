using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    private float timestamp_Spawn;
    internal float lifespan = 5;
    internal float speed = 10;
    internal Vector3 direction;

    void Start ()
    {
        //set timestamp to record age
        timestamp_Spawn = Time.time;

        //set orientation of projectile
        transform.rotation = Quaternion.LookRotation(Vector3.up, direction);
    }
	
	void Update ()
    {
        if (Time.time > lifespan + timestamp_Spawn)//test if lifespan has elapsed
            //remove artifact projectile from play
            Destroy(gameObject);
        else
            //apply "physics"
            transform.position += direction * (Time.deltaTime * speed);
    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject victem = collider.gameObject;
        if (victem.tag.Equals("Enemy"))//test if projectile hit an enemy
        {
            //damage enemy
        }

        if (!victem.tag.Equals("Player"))//test if projectile did not hit the player
            Destroy(gameObject);
    }
}
