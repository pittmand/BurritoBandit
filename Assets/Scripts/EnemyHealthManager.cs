using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyHealthManager : MonoBehaviour {
    public int MaxHealth = 50;
    public int CurrentHealth;
    public GameObject enemy;
    public int scoreValue = 1;
    public GameController gameController;
    public GameObject prefab_Explosion;
   

    void Start ()
    {
        
        CurrentHealth = MaxHealth;
        gameController = GameController.s_Instance;
        if (gameController == null)
            Debug.Log("GameController was not instantiated");

    }
	
	// Update is called once per frame
	void Update () {
        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
            if (gameController != null)
                gameController.addScore(scoreValue);
            GameObject _explosion_obj = Instantiate(prefab_Explosion, transform.position, Quaternion.identity);
        }
    }

    public void HurtEnemy(int damageTaken)
    {
        CurrentHealth -= damageTaken;
        AudioSource splat = GetComponent<AudioSource>();
        splat.Play();
    }
}
