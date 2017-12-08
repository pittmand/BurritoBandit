using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Tutorial : MonoBehaviour, ITriggerable
{

    GameController _gameController;
    public int index;

    void Start()
    {
        _gameController = GameController.s_Instance;
    }

    public void onActivate<T>(GameObject culprit, T metaData)
    {
        if (_gameController != null)
        {
            TutorialMessage message;
            message.index = index;
            message.open = true;
            _gameController.openTutorial(message);
        }
    }

    public void onActive<T>(GameObject culprit, T metaData)
    {
        throw new NotImplementedException();
    }

    public void onDeactivate<T>(GameObject culprit, T metaData)
    {
        if (_gameController != null)
        {
            TutorialMessage message;
            message.index = index;
            message.open = false;
            _gameController.openTutorial(message);
        }
    }
}
