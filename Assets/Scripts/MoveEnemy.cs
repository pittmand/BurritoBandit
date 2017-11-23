using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour {


    public Transform player;
    private UnityEngine.AI.NavMeshAgent navComponent;
    private float moveSpeed;
    private Animator spriteAnimator;

	void Start () {
        player = GameObject.FindWithTag("Player").transform;
        moveSpeed = Random.Range(5f, 8f);
        spriteAnimator = GetComponent<Animator>();
    }

    void Update () {
        float move = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.position, move);
        spriteAnimator.SetBool("Looking_Left", transform.position.x > player.position.x);
        spriteAnimator.SetBool("Looking_Forward", transform.position.z < player.position.z);

    }
}
