using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour {


    public Transform player;
    private UnityEngine.AI.NavMeshAgent navComponent;
    private float moveSpeed;

	void Start () {
        player = GameObject.FindWithTag("Player").transform;
        moveSpeed = Random.Range(5f, 8f);
    }

    void Update () {
        float move = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.position, move);
	}
}
