using UnityEngine;

public class Spawn : MonoBehaviour {

    public GameObject[] enemies;
    public int count;
    private Vector3 spawnPoint;

	void Update () {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        count = enemies.Length;
        if (count != 20)
        {
            InvokeRepeating("SpawnEnemy", 5, 10);
        }
	}

    void SpawnEnemy()
    {
        spawnPoint.x = Random.Range(-20, 20);
        spawnPoint.y = 0.5f;
        spawnPoint.z = Random.Range(-20,20);

        Instantiate(enemies[UnityEngine.Random.Range(0, enemies.Length - 1)], spawnPoint, Quaternion.identity);
        CancelInvoke();
    }
}
