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
    private static readonly int MoveStateParam = Animator.StringToHash("MoveState"); // 0:Up, 1:Down, 0.5:Run

    // State Variables
    // Moving State
    private bool _isMoving {get {return _travelTile.IsMoving;}}
    private Tile.DIR _currMoveDir;
    private Tile.DIR _lastMoveDir;
    private Tile.DIR _lastIdleDir;//Xử lý khi bắt đầu di chuyển
    private float _currentAnimState; // Lưu trạng thái animation hiện tại (Run/Up/Down)
    
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

        if (!_isMoving)
        {
            HandleStopState();
        }
    }

    private void StartMoving(Tile.DIR direction) //Khi nhân vật bắt đầu di chuyển
    {
        //Debug.Log("PA: "+ direction.ToString());
        _currMoveDir = direction;
        
        if (direction == Tile.DIR.DOWN)
        {
            _animator.SetFloat(MoveStateParam, 1);
        }
        else if (direction == Tile.DIR.UP)
        {
            if (_lastIdleDir == Tile.DIR.DOWN)
            {
                _animator.SetFloat(MoveStateParam, 0);
            }
            else
            {
                _animator.SetFloat(MoveStateParam, 0.5f);
                FlipRight(onRight);
            }
        }
        else
        {
            if (onDown)
            {
                _animator.SetFloat(MoveStateParam, 0.5f);
                FlipRight(direction == Tile.DIR.RIGHT);
            }
            else
            {
                _animator.SetFloat(MoveStateParam, 1);
            }
        }
    }
    

    private void HandleStopState()
    {
        _lastMoveDir = _currMoveDir;
        _currMoveDir = Tile.DIR.NONE;
        
        if (onDown)
        {
            SetSpriteDirection(Tile.DIR.DOWN);
            if (onRight || onLeft) FlipRight(!onRight);
        }
        else if (onUp)
        {
            SetSpriteDirection(Tile.DIR.UP);
            if (onRight || onLeft) FlipRight(onRight);
        }
        else if (onLeft)
        {
            SetSpriteDirection(Tile.DIR.LEFT);
            if (onUp || onDown) FlipRight(onUp);
        }
        else if (onRight)
        {
            SetSpriteDirection(Tile.DIR.RIGHT);
            if (onUp || onDown) FlipRight(onDown);
        }
        else
        {
            SetSpriteDirection(Tile.DIR.DOWN);
        }
    }

    private void HandleOnState()
    {
        onUp = Physics2D.Raycast(transform.position, Vector2.up, 0.6f, wallLayer);
        onDown = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, wallLayer);
        onLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.6f, wallLayer);
        onRight =  Physics2D.Raycast(transform.position, Vector2.right, 0.6f, wallLayer);
    }

    private void SetSpriteDirection(Tile.DIR dir)
    {
        float rotation = 0;
        if (dir == Tile.DIR.DOWN)
        {
            rotation = 0f;
        }
        else if (dir == Tile.DIR.UP)
        {
            rotation = 180f;
        }
        else if (dir == Tile.DIR.LEFT)
        {
            rotation = -90f;
        }
        else if (dir == Tile.DIR.RIGHT)
        {
            rotation = 90f;
        }
        else return;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
    }

    private void FlipRight(bool right = true)
    {
        Vector3 scale = transform.localScale;
        scale.x = (right ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x));
        transform.localScale = scale;
    }
}