using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour, ITrigger{

    public bool active = true;
    public float fullAlpha = 1.0f;
    public float voidAlpha = 0.0f;
    public float fadeRate = 0.5f;

    private float currentState = 1.0f;
    private Renderer renderer;

    
    void Start () {
        renderer = gameObject.GetComponent<Renderer>();
	}
	
	
	void Update () {
        float targetState = active ? fullAlpha : voidAlpha;
        if (currentState != targetState)
        {
            float delta = Time.deltaTime * Time.timeScale;
            float diff = Mathf.Abs(targetState - currentState);
            float change = Mathf.Min(fadeRate * delta , diff);
            if (targetState < currentState)
                change = -change;

            currentState += change;

            // set alpha
            Color c = renderer.material.color;
            c.a = currentState;
            renderer.material.color = c;
        }

    }

    public void onActivate(GameObject culprit)
    {
        active = false;
    }

    public void onActive(GameObject culprit)
    {
        throw new NotImplementedException();
    }

    public void onDeactivate(GameObject culprit)
    {
        active = true;
    }
}
