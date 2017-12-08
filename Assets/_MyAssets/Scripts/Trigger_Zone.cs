using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Zone : MonoBehaviour {

    public List<TriggerSet> EventTargets_onEnterOnce;
    public List<TriggerSet> EventTargets_onEnter;
    public List<TriggerSet> EventTargets_onResiding;
    public List<TriggerSet> EventTargets_onExitOnce;
    public List<TriggerSet> EventTargets_onExit;

    public List<string> culprits_tags;

    public Collider _trigger;
    public Color gizmoColor = Color.green;


    // Use this for initialization
    void Start () {
        // remove invalids
        for (int i=0; i< EventTargets_onEnterOnce.Count;)
        {
            if (!StaticCaller_Trigger.isTriggerable(EventTargets_onEnterOnce[i]))
                EventTargets_onEnterOnce.RemoveAt(i);
            else
                ++i;
        }
        for (int i = 0; i < EventTargets_onEnter.Count;)
        {
            if (!StaticCaller_Trigger.isTriggerable(EventTargets_onEnter[i]))
                EventTargets_onEnter.RemoveAt(i);
            else
                ++i;
        }
        for (int i = 0; i < EventTargets_onResiding.Count;)
        {
            if (!StaticCaller_Trigger.isTriggerable(EventTargets_onResiding[i]))
                EventTargets_onResiding.RemoveAt(i);
            else
                ++i;
        }
        for (int i = 0; i < EventTargets_onExitOnce.Count;)
        {
            if (!StaticCaller_Trigger.isTriggerable(EventTargets_onExitOnce[i]))
                EventTargets_onExitOnce.RemoveAt(i);
            else
                ++i;
        }
        for (int i = 0; i < EventTargets_onExit.Count;)
        {
            if (!StaticCaller_Trigger.isTriggerable(EventTargets_onExit[i]))
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
            foreach (TriggerSet ts in EventTargets_onEnterOnce)
                StaticCaller_Trigger.applyTrigger(ts, culprit);
            EventTargets_onEnterOnce.Clear();

            foreach (TriggerSet ts in EventTargets_onEnter)
                StaticCaller_Trigger.applyTrigger(ts, culprit);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        GameObject culprit = collider.gameObject;

        if (validCulprit(culprit))
        {
            foreach (TriggerSet ts in EventTargets_onResiding)
                StaticCaller_Trigger.applyTrigger(ts, culprit);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        GameObject culprit = collider.gameObject;

        if (validCulprit(culprit))
        {
            foreach (TriggerSet ts in EventTargets_onExitOnce)
                StaticCaller_Trigger.applyTrigger(ts, culprit);
            EventTargets_onExitOnce.Clear();

            foreach (TriggerSet ts in EventTargets_onExit)
                StaticCaller_Trigger.applyTrigger(ts, culprit);
        }
    }

    private bool validCulprit(GameObject culprit)
    {
        foreach (string s in culprits_tags)
        {
            if (culprit.CompareTag(s))
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        if (_trigger != null)
        {
            Bounds bounds = _trigger.bounds;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}
