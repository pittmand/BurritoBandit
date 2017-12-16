using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credit_Scroller : MonoBehaviour {

    public GameObject continueButton;

    private GameController _gameController;

    void Start()
    {
        continueButton.SetActive(false);
        _gameController = GameController.s_Instance;
    }

    public void enableButton(int state)
    {
        continueButton.SetActive(state != 0);
    }

    public void endScene()
    {
        _gameController.ReturnToMain();
    }
}

