using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    private Animator _animator;

    private bool doorOpen;

    private bool openedOnce = false;

    // Use this for initialization
    void Start()
    {
        doorOpen = false;
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            openedOnce = doorOpen = true;
            Doors("Open");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (doorOpen)
        {
            doorOpen = false;
            Doors("Close");
        }
    }

    void Doors(string direction)
    {
        _animator.SetTrigger(direction);
    }
}
