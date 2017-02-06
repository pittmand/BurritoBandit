using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour {
    public int MaxHealth = 50;
    public int CurrentHealth;
    public GameObject enemy;

    void Start ()
    {
        CurrentHealth = MaxHealth;
    }
	
	// Update is called once per frame
	void Update () {
        if (CurrentHealth <= 0)
        {
            Debug.Log("Health Manager Update Called");
            Destroy(gameObject);
        }
    }

    public void HurtEnemy(int damageTaken)
    {
        Debug.Log("hurt enemy called");
        CurrentHealth -= damageTaken;
    }
}
