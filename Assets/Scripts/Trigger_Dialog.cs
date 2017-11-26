using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Dialog : MonoBehaviour, ITriggerable
{

    GameController _gameController;
    public DialogMessage message;

    void Start ()
    {
        _gameController = GameController.s_Instance;
    }

    public void onActivate<T>(GameObject culprit, T metaData)
    {
        if (_gameController != null)
        {
            _gameController.openDialog(message);
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
            _gameController.closeDialog();
        }
    }
}
