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
    void onActivate<T>(GameObject culprit, T metaData);
    void onActive<T>(GameObject culprit, T metaData);
    void onDeactivate<T>(GameObject culprit, T metaData);
}

public class StaticCaller_Trigger
{
    internal static void applyTrigger(TriggerSet ts, GameObject culprit)
    {
        applyTrigger(ts, culprit, (int)0);
    }

    internal static void applyTrigger<T>(TriggerSet ts, GameObject culprit, T metaData)
    {
        ITriggerable triggerable = getTriggerable(ts);

        Debug.Log("Trigger Call "+ triggerable.ToString()+ " " + ts.action.ToString() + " " + culprit.ToString() );
        switch (ts.action)
        {
            case TriggerAction.ACTIVATE:
                triggerable.onActivate(culprit, metaData);
                break;
            case TriggerAction.DEACTIVATE:
                triggerable.onDeactivate(culprit, metaData);
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