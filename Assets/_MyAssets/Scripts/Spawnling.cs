using UnityEngine;

public class Spawnling : MonoBehaviour
{
    internal Spawn spawner;

    void OnDestroy()
    {
        spawner.ChildDestoryed(gameObject);
    }
}