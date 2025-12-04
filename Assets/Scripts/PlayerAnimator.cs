using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TravelTile _travelTile; // Kéo TravelTile vào đây
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerMovement _playerMovement;
    
    [Header("Settings")]
    public LayerMask wallLayer;
    
    // Animator Parameters ID
    private static readonly int IsMovingParam = Animator.StringToHash("IsMoving");
    private static readonly int MoveStateParam = Animator.StringToHash("MoveState");

    // State Variables
    // Moving State
    private bool _isMoving {get {return _travelTile.IsMoving;}}
    private Tile.DIR _lastMoveDir;
    private Tile.DIR _lastIdleDir;
    
    //On wall state
    public bool onRight;
    public bool onLeft;
    public bool onUp;
    public bool onDown;

    
    
    
    private void OnEnable()
    {
        if (_playerMovement != null)
        {
            _playerMovement.OnPlayerMoved += StartMoving;
        }
    }

    private void OnDisable()
    {
        if (_playerMovement != null)
        {
            _playerMovement.OnPlayerMoved -= StartMoving;
        }
    }


    void Update() //Move and Idle
    {
        HandleOnState();
        _animator.SetBool(IsMovingParam, _isMoving);
    }

    private void StartMoving(Tile.DIR direction) //Khi nhân vật bắt đầu di chuyển
    {
        if (direction == Tile.DIR.DOWN)
        {
            _animator.SetFloat(MoveStateParam, 0);
        }
        else if (direction == Tile.DIR.UP)
        {
            _animator.SetFloat(MoveStateParam, 1);
        }
        else if (direction == Tile.DIR.LEFT)
        {
            _animator.SetFloat(MoveStateParam, 0.33f);
        }
        else if (direction == Tile.DIR.RIGHT)
        {
            _animator.SetFloat(MoveStateParam, 0.67f);
        }
    }

    private void HandleOnState()
    {
        onUp = Physics2D.Raycast(transform.position, Vector2.up, 0.6f, wallLayer);
        onDown = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, wallLayer);
        onLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.6f, wallLayer);
        onRight =  Physics2D.Raycast(transform.position, Vector2.right, 0.6f, wallLayer);
    }

    public void JumpIn()
    {
        _animator.SetTrigger("JumpIn");
    }
}