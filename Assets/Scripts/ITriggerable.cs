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

public interface ITriggerable
{
    void onActivate(GameObject culprit);
    void onActive(GameObject culprit);
    void onDeactivate(GameObject culprit);
}

public class StaticCaller_Trigger
{
    internal static void applyTrigger(TriggerSet ts, GameObject culprit)
    {
        ITriggerable triggerable = getTriggerable(ts);

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

    private static ITriggerable getTriggerable(TriggerSet ts)
    {
        GameObject g = ts.triggerable;
        ITriggerable triggerable = g.GetComponent<ITriggerable>();
        if (triggerable != null)
            return triggerable;

        return null;
    }

    internal static bool isTriggerable(TriggerSet ts)
    {
        GameObject g = ts.triggerable;
        ITriggerable triggerable = g.GetComponent<ITriggerable>();

        return triggerable != null;
    }
}