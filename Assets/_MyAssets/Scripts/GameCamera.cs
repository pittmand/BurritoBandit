using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour {

    private Transform _target;
    private Vector3 _cameraTarget;

	void Start () {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _cameraTarget = _target.position;
        transform.position = _cameraTarget;
    }
	
	// Update is called once per frame
	void Update () {
        _cameraTarget = _target.position;
        transform.position = Vector3.Lerp(transform.position, _cameraTarget, Time.deltaTime * 8);
    }
}
