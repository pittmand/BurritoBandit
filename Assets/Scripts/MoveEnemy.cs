using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveEnemy : MonoBehaviour {


    public Transform player;
    public float moveSpeed;
    private float maxSpeed;
    private Rigidbody physics;
    private NavMeshAgent navAgent;
    private Animator spriteAnimator;

	void Start () {
        player = GameObject.FindWithTag("Player").transform;
        moveSpeed *= Random.Range(0.8f, 1.0f);
        physics = GetComponent<Rigidbody>();
        spriteAnimator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.angularSpeed = 0.0f;//freeze rotation
        maxSpeed = moveSpeed * 1.5f;

    }

    void Update () {
        navAgent.speed = moveSpeed;
        if ((player.position - navAgent.destination).sqrMagnitude > 1.0f)
            //player moved
            navAgent.SetDestination(player.position);
        spriteAnimator.SetBool("Looking_Left", transform.position.x > player.position.x);
        spriteAnimator.SetBool("Looking_Forward", transform.position.z < player.position.z);

    }

    void FixedUpdate()
    {
        // clamp max land speed (helps prevent weird collision behavior)
        Vector3 velocity = physics.velocity;
        float y_velocity = velocity.y;
        velocity.y = 0;
        if (velocity.magnitude > maxSpeed)
            physics.velocity = velocity.normalized * maxSpeed;
    }
}
