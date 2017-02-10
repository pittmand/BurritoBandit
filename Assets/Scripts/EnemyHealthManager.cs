using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour {
    public int MaxHealth = 50;
    public int CurrentHealth;
    public GameObject enemy;
    public int scoreValue = 1;
    public GameController gameController;

    void Start ()
    {
        CurrentHealth = MaxHealth;
        gameController = GameController.s_Instance;
        if(gameController == null)
            Debug.Log("GameController was not instantiated");
    }
	
	// Update is called once per frame
	void Update () {
        if (CurrentHealth <= 0)
        {
            Debug.Log("Health Manager Update Called");
            Destroy(gameObject);
            if (gameController != null)
                gameController.addScore(scoreValue);
        }
    }

    public void HurtEnemy(int damageTaken)
    {
        Debug.Log("hurt enemy called");
        CurrentHealth -= damageTaken;
    }
}
