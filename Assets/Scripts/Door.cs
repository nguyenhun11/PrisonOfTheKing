using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator _animator;
    private Tile _tile;
    private bool canOpenDoor = false;
    
    public delegate void DoorOpenDelegate();
    public event DoorOpenDelegate OnDoorOpen;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _tile = GetComponent<Tile>();
    }

    public void CloseDoor()
    {
        _animator.ResetTrigger("Open");
        _animator.SetTrigger("Close");
        CanOpenDoor(false);
    }

    public void CanOpenDoor(bool on)
    {
        _tile.isStopTile = on;
        canOpenDoor = on;
    }

    public void OpenDoor()
    {
        _animator.ResetTrigger("Close");
        _animator.SetTrigger("Open");
        OnDoorOpen?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canOpenDoor && other.CompareTag("Player"))
        {
            OpenDoor();
            OnDoorOpen?.Invoke();
        }
    }
    
    
}
