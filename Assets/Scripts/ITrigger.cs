using UnityEngine;

[System.Serializable]
public enum TriggerAction
{
    ACTIVATE,
    DEACTIVATE
};

[System.Serializable]
public struct TriggerSet
{
    public GameObject triggerable;
    public TriggerAction action;
}

public interface ITrigger
{
    void onActivate(GameObject culprit);
    void onActive(GameObject culprit);
    void onDeactivate(GameObject culprit);
}

public class StaticCaller_Trigger
{
    internal static void applyTrigger(TriggerSet ts, GameObject culprit)
    {
        ITrigger triggerable = getTriggerable(ts);

        Debug.Log("Trigger Call "+ triggerable.ToString()+ " " + ts.action.ToString() + " " + culprit.ToString() );
        switch (ts.action)
        {
            case TriggerAction.ACTIVATE:
                triggerable.onActivate(culprit);
                break;
            case TriggerAction.DEACTIVATE:
                triggerable.onDeactivate(culprit);
                break;
        }
    }

    private static ITrigger getTriggerable(TriggerSet ts)
    {
        GameObject g = ts.triggerable;
        ITrigger triggerable = g.GetComponent<ITrigger>();
        if (triggerable != null)
            return triggerable;

        return null;
    }

    internal static bool isTriggerable(TriggerSet ts)
    {
        GameObject g = ts.triggerable;
        ITrigger triggerable = g.GetComponent<ITrigger>();

        return triggerable != null;
    }
}