using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking_LookAt : MonoBehaviour {

    public Transform target;
    public string targetName;
    public bool ignoreYAxis;
    public bool lookAway;
    public Quaternion rotation;

    // Use this for initialization
    void Start () {
		if (target == null)
            target = GameObject.Find(targetName).transform;

    }
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            if (ignoreYAxis)
            {
                Vector3 origin = new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 targ = new Vector3(target.position.x, 0, target.position.z);
                Quaternion targetRotation;
                if (lookAway)
                {
                    targetRotation = Quaternion.LookRotation(origin - targ);                    
                }
                else
                    targetRotation = Quaternion.LookRotation(targ - origin);      
                          
                transform.eulerAngles = Vector3.up * targetRotation.eulerAngles.y;
                this.transform.localRotation = targetRotation;
                this.transform.Rotate(90.0f, 0.0f, 0.0f);
            }
            else
            {
                if (lookAway)
                {
                    transform.rotation = Quaternion.LookRotation(transform.position - target.position);
                }
                else
                {
                    transform.rotation = Quaternion.LookRotation(target.position - transform.position);

                }
            }
        }
        else if(rotation != Quaternion.identity)
        {
            transform.rotation = rotation;
        }
    }
}
