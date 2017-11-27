using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveEnemy : MonoBehaviour {


    public Transform player;
    public float moveSpeed;
    private NavMeshAgent navAgent;
    private Animator spriteAnimator;

	void Start () {
        player = GameObject.FindWithTag("Player").transform;
        moveSpeed *= Random.Range(0.8f, 1.0f);
        spriteAnimator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.angularSpeed = 0.0f;//freeze rotation
    }

    void Update () {
        navAgent.speed = moveSpeed;
        if ((player.position - navAgent.destination).sqrMagnitude > 1.0f)
            //player moved
            navAgent.SetDestination(player.position);
        spriteAnimator.SetBool("Looking_Left", transform.position.x > player.position.x);
        spriteAnimator.SetBool("Looking_Forward", transform.position.z < player.position.z);

    }
}
