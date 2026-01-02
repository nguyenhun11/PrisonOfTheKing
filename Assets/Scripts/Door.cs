using System;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator _animator;
    private Tile _tile;
    private bool _canOpenDoor = false;
    
    public delegate void DoorOpenDelegate();
    public event DoorOpenDelegate OnDoorOpen;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _tile = GetComponent<Tile>();
        CloseDoor();
    }

    public void CloseDoor()
    {
        _animator.ResetTrigger("Open");
        _animator.SetTrigger("Close");
        CanOpenDoor(false);
        Controller_Sound.Play("CloseDoor");
    }

    public void CanOpenDoor(bool on)
    {
        _tile.isStopTile = on;
        _canOpenDoor = on;
    }

    public void OpenDoor()
    {
        _animator.ResetTrigger("Close");
        _animator.SetTrigger("Open");
        Controller_Sound.Play("OpenDoor");
        OnDoorOpen?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_canOpenDoor && other.CompareTag("Player"))
        {
            OpenDoor();
            TravelTile playerTravelTile = other.GetComponent<TravelTile>();
            PlayerAnimator playerAnimator = other.GetComponent<PlayerAnimator>();
            CanOpenDoor(false);
            if (playerTravelTile != null) playerTravelTile.Move(_tile.currNode);
            if (playerAnimator != null) playerAnimator.JumpIn();
            if (playerAnimator.TryGetComponent(out PlayerMovement playerMovement))
            {
                playerMovement.canMove = false;
            }
            OnDoorOpen?.Invoke();
        }
    }
}
