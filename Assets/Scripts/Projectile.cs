using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    float timestamp_Spawn;
    public float lifespan;
    public Vector3 direction;
    public float speed;

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

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals("Enemy"))//test if projectile hit an enemy
        {
            //damage enemy
        }

        //projectile is destroyed irregardless on collide
        Destroy(gameObject);
    }
}
