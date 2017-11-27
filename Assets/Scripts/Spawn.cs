using UnityEngine;
using UnityEngine.AI;

public class Spawn : MonoBehaviour
{
    public int max_active = 10;
    public int max_spawned = 50;
    public float delay_activation = 1;
    public float delay_multipuleSpawn = 3;
    public LayerMask spawn_layer = -1;
    public float Local_bounds_radius_min = 20;
    public float Local_bounds_radius_max = 100;
    public Bounds bounds;
    public GameObject[] prefab_enemies;
    internal System.Collections.Generic.List<GameObject> enemies;
    private LevelController _levelController;
    private Vector3 spawnPoint;
    private bool invoked = false;
    private int count = 0;

    void Start()
    {
        enemies = new System.Collections.Generic.List<GameObject>();
        _levelController = LevelController.s_Instance;
    }

    void Update()
    {
        if (!invoked && enemies.Count < max_active)
        {
            InvokeRepeating("SpawnEnemy", delay_activation, delay_multipuleSpawn);
            invoked = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + bounds.center, bounds.size);

        //find/comfirm player instance
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 pos_player = player.transform.position;
            Vector3 pos = transform.position;
            Vector3 min_b = bounds.min + pos;
            Vector3 max_b = bounds.max + pos;
            Vector3 min;
            Vector3 max;

            //localize bounds towards play position
            min.x = Mathf.Min(Mathf.Max(pos_player.x - Local_bounds_radius_max, min_b.x), max_b.x);
            min.y = Mathf.Min(Mathf.Max(pos_player.y - Local_bounds_radius_max, min_b.y), max_b.y);
            min.z = Mathf.Min(Mathf.Max(pos_player.z - Local_bounds_radius_max, min_b.z), max_b.z);
            max.x = Mathf.Max(Mathf.Min(pos_player.x + Local_bounds_radius_max, max_b.x), min_b.x);
            max.y = Mathf.Max(Mathf.Min(pos_player.y + Local_bounds_radius_max, max_b.y), min_b.y);
            max.z = Mathf.Max(Mathf.Min(pos_player.z + Local_bounds_radius_max, max_b.z), min_b.z);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube((min + max)*0.5f, max - min);

            float size = Local_bounds_radius_min * 2;
            Vector3 safe = new Vector3(size, size, size);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(pos_player, safe);
        }
    }

    void SpawnEnemy()
    {
        //find/comfirm player instance
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            if (enemies.Count >= max_active)
            {
                CancelInvoke();
                invoked = false;
            }
            else if (count < max_spawned)
            {
                Vector3 pos_player = player.transform.position;

                //find bounds
                Vector3 pos = transform.position;
                Vector3 min_b = bounds.min + pos;
                Vector3 max_b = bounds.max + pos;

                //localize bounds towards player position
                Vector3 min;
                Vector3 max;
                min.x = Mathf.Min(Mathf.Max(pos_player.x - Local_bounds_radius_max, min_b.x), max_b.x);
                min.y = Mathf.Min(Mathf.Max(pos_player.y - Local_bounds_radius_max, min_b.y), max_b.y);
                min.z = Mathf.Min(Mathf.Max(pos_player.z - Local_bounds_radius_max, min_b.z), max_b.z);
                max.x = Mathf.Max(Mathf.Min(pos_player.x + Local_bounds_radius_max, max_b.x), min_b.x);
                max.y = Mathf.Max(Mathf.Min(pos_player.y + Local_bounds_radius_max, max_b.y), min_b.y);
                max.z = Mathf.Max(Mathf.Min(pos_player.z + Local_bounds_radius_max, max_b.z), min_b.z);

                //randomize position
                spawnPoint.y = pos_player.y;
                int attempt = 0;
                float distance = 0;
                do
                {
                    // stop spawning if failed to select valid spot of 5 tries
                    if (attempt >= 5)
                        return;

                    spawnPoint.x = Random.Range(min.x, max.x);
                    spawnPoint.z = Random.Range(min.z, max.z);
                    ++attempt;
                    distance = Vector3.Distance(spawnPoint, pos_player);
                }
                while (distance < Local_bounds_radius_min || Local_bounds_radius_max > 100);
                spawnPoint.y = max.y;

                //test terrain
                RaycastHit hit;
                Ray ray = new Ray(spawnPoint, Vector3.down);
                if (Physics.Raycast(ray, out hit, max.y - min.y, spawn_layer) && hit.collider != null)
                {
                    spawnPoint.y = hit.point.y + 1.0f;
                }

                GameObject enemy = Instantiate(prefab_enemies[UnityEngine.Random.Range(0, prefab_enemies.Length - 1)], spawnPoint, Quaternion.identity);
                NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
                if (agent != null)
                    agent.Warp(spawnPoint);
                enemy.AddComponent<Spawnling>().spawner = this;
                ++count;

                enemies.Add(enemy);

                if (count >= max_spawned)
                {
                    // stop inkove
                }
            }
        }
    }

    internal void ChildDestoryed(GameObject child)
    {
        enemies.Remove(child);
        if (count == max_spawned && enemies.Count <= 0)
            _levelController.objective_complete(gameObject);
    }
}
