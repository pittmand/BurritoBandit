using UnityEngine;
public class Spawn : MonoBehaviour
{

    public GameObject[] prefab_enemies;
    internal System.Collections.Generic.List<GameObject> enemies;
    private Vector3 spawnPoint;

    void Start()
    {
        enemies = new System.Collections.Generic.List<GameObject>();
    }

    void Update()
    {
        if (enemies.Count < 20)
        {
            InvokeRepeating("SpawnEnemy", 5, 10);
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

        CancelInvoke();
    }

    internal void ChildDestoryed(GameObject child)
    {
        enemies.Remove(child);
    }
}

//using UnityEngine;

//public class Spawn : MonoBehaviour
//{

//    public GameObject[] prefab_enemies;
//    internal System.Collections.Generic.List<GameObject> enemies;
//    private Vector3 spawnPoint;

//    void Start()
//    {
//        enemies = new System.Collections.Generic.List<GameObject>();
//    }

//    void Update()
//    {
//        if (enemies.Count < 20)
//        {
//            InvokeRepeating("SpawnEnemy", 5, 10);
//        }
//    }

//    void SpawnEnemy()
//    {
//        Debug.Log("spawn");
//        spawnPoint.x = Random.Range(-20, 20);
//        spawnPoint.y = 0.5f;
//        spawnPoint.z = Random.Range(-20, 20);

//        GameObject enemy = Instantiate(prefab_enemies[UnityEngine.Random.Range(0, prefab_enemies.Length - 1)], spawnPoint, Quaternion.identity);
//        Debug.Log(enemy);
//        enemies.Add(enemy);
//        //TODO: tell enemy to remeber me

//        CancelInvoke();
//    }

//    internal void ChildDestoryed(GameObject child)
//    {
//        Debug.Log("destroying " + child.name);
//        enemies.Remove(child);
//    }
//}
