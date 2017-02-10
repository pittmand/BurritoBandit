using UnityEngine;

public class Spawnling : MonoBehaviour
{
    internal Spawn spawner;
    private GameController gameController;

    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            Debug.Log("'GameController' instance found");
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        else
            Debug.Log("Unable to find 'GameController' asset");
    }

    void OnDestroy()
    {
        Debug.Log("Calling AddScore");
        gameController.addScore(10);
        spawner.ChildDestoryed(gameObject);
    }
}