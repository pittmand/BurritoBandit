using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    internal static LevelController s_Instance;

    public List<GameObject> objectives;

    private GameController _gameController;


    void Start()
    {
        s_Instance = this;

        _gameController = GameController.s_Instance;
    }


    void Update()
    {

    }

    internal void objective_complete(GameObject g)
    {
        Debug.Log("Objective Complete Called "+g.ToString());
        objectives.Remove(g);
        if (objectives.Count <= 0)
            if (_gameController != null)
                _gameController.Victory();
    }
}
