using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Entity_Tracker {

    private List<GameObject> marks = new List<GameObject>();

    internal int count()
    {
        return marks.Count;
    }

    internal bool isTracking(GameObject target)
    {
        return marks.Contains(target);
    }

    internal bool trackEntity(GameObject target)
    {
        Entity_Tracker_Mark mark;

        mark = target.GetComponent<Entity_Tracker_Mark>();
        if (mark == null)
        {
            mark = target.AddComponent<Entity_Tracker_Mark>();
            if (mark == null)
                return false;
        }

        mark.trackers.Add(this);
        marks.Add(target);

        return true;
    }

    internal void unTrackEntity(GameObject target)
    {
        marks.Remove(target);
        Entity_Tracker_Mark mark = target.GetComponent<Entity_Tracker_Mark>();
        if (mark != null)
            mark.untrack(this);
    }

    internal void OnDestroy()
    {
        foreach (GameObject target in marks)
        {
            Entity_Tracker_Mark mark = target.GetComponent<Entity_Tracker_Mark>();
            if (mark != null)
                mark.untrack(this);
        }
    }

    internal void targetDestoryed(GameObject target)
    {
        marks.Remove(target);
    }

    
}
