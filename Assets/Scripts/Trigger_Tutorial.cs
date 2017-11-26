using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Tutorial : MonoBehaviour {

    GameController _gameController;


    void Start()
    {
        _gameController = GameController.s_Instance;
    }


    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject culprit = collider.gameObject;
        if (culprit.CompareTag("Player"))
        {
            Debug.Log("OnTriggerEnter T");
            if (_gameController != null)
                _gameController.openTutorial(0, true);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        GameObject culprit = collider.gameObject;
        if (culprit.CompareTag("Player"))
        {
            Debug.Log("OnTriggerExit T");
            if (_gameController != null)
                _gameController.openTutorial(0, false);
        }
    }
}
