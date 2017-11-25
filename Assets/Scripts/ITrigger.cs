using UnityEngine;


public interface ITrigger{

    void onActivate(GameObject culprit);
    void onActive(GameObject culprit);
    void onDeactivate(GameObject culprit);

}
