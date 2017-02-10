using UnityEngine;
public class Spawn : MonoBehaviour
{
    public int max_active = 10;
    public int max_spawned = 50;
    public float delay_activation = 1;
    public float delay_multipuleSpawn = 3;
    public GameObject[] prefab_enemies;
    public int scoreValue;

    private GameController gameController;
    internal System.Collections.Generic.List<GameObject> enemies;
    private Vector3 spawnPoint;
    private bool invoked = false;
    private int count = 0;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            Debug.Log("'GameController' instance found");
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        else
            Debug.Log("Unable to find 'GameController' asset");
        enemies = new System.Collections.Generic.List<GameObject>();
    }

    void Update()
    {
        if (!invoked && enemies.Count < max_active)
        {
            InvokeRepeating("SpawnEnemy", delay_activation, delay_multipuleSpawn);
            invoked = true;
        }
    }

    void SpawnEnemy()
    {
        spawnPoint.x = Random.Range(-20, 20);
        spawnPoint.y = 0.5f;
        spawnPoint.z = Random.Range(-20, 20);

        GameObject enemy = Instantiate(prefab_enemies[UnityEngine.Random.Range(0, prefab_enemies.Length - 1)], spawnPoint, Quaternion.identity);
        enemies.Add(enemy);
        enemy.GetComponent<Spawnling>().spawner = this;
        ++count;

        if (enemies.Count >= max_active)
        {
            CancelInvoke();
            invoked = false;
        }
    }

    internal void ChildDestoryed(GameObject child)
    {
        Debug.Log("Calling AddScore");
        gameController.addScore(scoreValue);
        enemies.Remove(child);
    }
}
