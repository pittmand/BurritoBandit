using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Tracker_Mark : MonoBehaviour {

    internal List<Entity_Tracker> trackers = new List<Entity_Tracker>();

    void OnDestroy()
    {
        foreach (Entity_Tracker tracker in trackers)
            tracker.targetDestoryed(gameObject);
    }

    internal void untrack(Entity_Tracker tracker)
    {
        trackers.Remove(tracker);
        if (trackers.Count == 0)
            Destroy(this);
    }
}
