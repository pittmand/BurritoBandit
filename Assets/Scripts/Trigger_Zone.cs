using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Zone : MonoBehaviour {

    public List<GameObject> EventTargets_onEnterOnce;
    public List<GameObject> EventTargets_onEnter;
    public List<GameObject> EventTargets_onResiding;
    public List<GameObject> EventTargets_onExitOnce;
    public List<GameObject> EventTargets_onExit;

    public List<string> triggerables_tags;


    // Use this for initialization
    void Start () {
        // remove invalids
        for (int i=0; i< EventTargets_onEnterOnce.Count;)
        {
            if (getTrigger(EventTargets_onEnterOnce[i]) == null)
                EventTargets_onEnterOnce.RemoveAt(i);
            else
                ++i;
        }
        for (int i = 0; i < EventTargets_onEnter.Count;)
        {
            if (getTrigger(EventTargets_onEnter[i]) == null)
                EventTargets_onEnter.RemoveAt(i);
            else
                ++i;
        }
        for (int i = 0; i < EventTargets_onResiding.Count;)
        {
            if (getTrigger(EventTargets_onResiding[i]) == null)
                EventTargets_onResiding.RemoveAt(i);
            else
                ++i;
        }
        for (int i = 0; i < EventTargets_onExitOnce.Count;)
        {
            if (getTrigger(EventTargets_onExitOnce[i]) == null)
                EventTargets_onExitOnce.RemoveAt(i);
            else
                ++i;
        }
        for (int i = 0; i < EventTargets_onExit.Count;)
        {
            if (getTrigger(EventTargets_onExit[i]) == null)
                EventTargets_onExit.RemoveAt(i);
            else
                ++i;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collider)
    {
        GameObject culprit = collider.gameObject;

        if (validCulprit(culprit))
        {
            foreach (GameObject g in EventTargets_onEnterOnce)
            {
                ITrigger trigger = getTrigger(g);
                trigger.onActivate(culprit);
            }
            EventTargets_onEnterOnce.Clear();

            foreach (GameObject g in EventTargets_onEnter)
            {
                ITrigger trigger = getTrigger(g);
                trigger.onActivate(culprit);
            }
        }
    }

    void OnTriggerStay(Collider collider)
    {
        GameObject culprit = collider.gameObject;

        if (validCulprit(culprit))
        {
            foreach (GameObject g in EventTargets_onResiding)
            {
                ITrigger trigger = getTrigger(g);
                trigger.onActive(culprit);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        GameObject culprit = collider.gameObject;

        if (validCulprit(culprit))
        {
            foreach (GameObject g in EventTargets_onExitOnce)
            {
                ITrigger trigger = getTrigger(g);
                trigger.onDeactivate(culprit);
            }
            EventTargets_onExitOnce.Clear();

            foreach (GameObject g in EventTargets_onExit)
            {
                ITrigger trigger = getTrigger(g);
                trigger.onDeactivate(culprit);
            }
        }
    }

    private ITrigger getTrigger(GameObject g)
    {
        ITrigger trigger = g.GetComponent<ITrigger>();
        if (trigger != null)
            return trigger;

        trigger = g.GetComponentInChildren<ITrigger>();
        if (trigger != null)
            return trigger;

        return null;
    }

    private bool validCulprit(GameObject culprit)
    {
        foreach (string s in triggerables_tags)
        {
            if (culprit.CompareTag(s))
            {
                return true;
            }
        }
        return false;
    }
}
