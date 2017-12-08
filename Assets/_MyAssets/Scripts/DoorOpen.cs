using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public enum DoorMode
{
    LOCKED,
    PLAYER_ONLY,
    JAMMED,
    FREE_USE
};

public class DoorOpen : MonoBehaviour
{
    public DoorMode mode;
    public Collider _trigger;
    public Color gizmoColor = Color.green;
    

    private Animator _animator;
    private NavMeshObstacle _lock;
    private Entity_Tracker tracker;

    private bool doorOpen;
    private bool openedOnce = false;

    // Use this for initialization
    void Start()
    {
        doorOpen = false;
        _animator = transform.Find("Hinge").GetComponent<Animator>();
        _lock = transform.Find("Hinge/Door").GetComponent<NavMeshObstacle>();
        _lock.enabled = mode != DoorMode.FREE_USE;
        tracker = new Entity_Tracker();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(mode != DoorMode.LOCKED)
        {
            bool isPlayer = other.CompareTag("Player");
            if (mode != DoorMode.FREE_USE)
            {
                if (mode == DoorMode.PLAYER_ONLY || !openedOnce)
                {
                    if (!isPlayer)
                        goto skip;
                }

                if(mode == DoorMode.JAMMED)
                    _lock.enabled = false;
            }

            if (isPlayer || other.CompareTag("Enemy"))
            {
                Debug.Log("open"+gameObject);
                openedOnce = doorOpen = true;
                _animator.SetBool("Mirrored", Vector3.Dot((other.transform.position - transform.position), transform.forward) > 0);
                Doors("Open");
                tracker.trackEntity(other.gameObject);
            }
        }

    skip:
        return;
    }

    private void OnTriggerExit(Collider other)
    {
        tracker.unTrackEntity(other.gameObject);

        if (doorOpen && tracker.count() <= 0)
        {
            Debug.Log("close"+gameObject);
            doorOpen = false;
            Doors("Close");
        }
    }

    void Doors(string direction)
    {
        _animator.SetTrigger(direction);
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
