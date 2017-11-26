using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Dialog : MonoBehaviour {

    GameController _gameController;

        
    void Start ()
    {
        _gameController = GameController.s_Instance;
    }
	
	
	void Update () {
		
	}

    void OnTriggerEnter(Collider collider)
    {
        GameObject culprit = collider.gameObject;
        if (culprit.CompareTag("Player"))
        {
            Debug.Log("OnTriggerEnter D");
            if (_gameController != null)
                _gameController.openDialog(SpeakerIconID.PLAYER, "Testing 1 2 3!", 1, 3);
        }
    }
}
