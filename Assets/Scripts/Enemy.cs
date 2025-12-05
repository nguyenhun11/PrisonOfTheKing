using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// Đảm bảo có Rigidbody2D để xử lý vật lý rơi
[RequireComponent(typeof(Rigidbody2D))] 
public class Enemy : MonoBehaviour
{
    [SerializeField] private TravelTile _travelTile;
    [SerializeField] private Effect _killEffect;
    
    [Header("Death Settings")]
    [SerializeField] private float _jumpForce = 8f;   // Lực nảy lên cao
    [SerializeField] private float _throwForce = 3f;  // Lực bay ngang
    [SerializeField] private float _destroyDelay = 2f;// Thời gian chờ rơi khỏi màn hình rồi mới xóa

    public bool IsDead { get; private set; } // Cờ đánh dấu để tránh hàm Die gọi nhiều lần
    private Rigidbody2D _rb;
    private Collider2D _col;
    
    //On wall state
    public LayerMask wallLayer;
    public LayerMask tileLayer;
    public bool onRight;
    public bool onLeft;
    public bool onUp;
    public bool onDown;

    private Animator _animator;
    private static readonly int IdleStateParam = Animator.StringToHash("IdleState");
    private static readonly int MovingStateParam = Animator.StringToHash("MovingState");
    private static readonly int DieParam = Animator.StringToHash("Die");
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        HandleOnState();
        HandleIdleState();
    }

    private void OnEnable()
    {
        if (_travelTile != null)
        {
            _travelTile.OnTravelTileStop += Die;
            _travelTile.OnTravelTileMove += HandleMovingState;
        }
    }

    private void OnDisable()
    { 
        if (_travelTile != null)
        {
            _travelTile.OnTravelTileStop -= Die;
            _travelTile.OnTravelTileMove -= HandleMovingState;
        }
    }

    public void Die()
    {
        if (IsDead) return;
        IsDead = true;
        _animator.SetTrigger(DieParam);
        Controller_Sound.Play("EnemyDie");
        
        if (_travelTile != null)
        {
            _travelTile.OnTravelTileStop -= Die;
            _travelTile.enabled = false;
        }

        if (_killEffect != null)
        {
            Instantiate(_killEffect, transform.position, transform.rotation);
        }
        
        if (_col != null)
        {
            _col.enabled = false;
        }

        // 3. Xử lý Vật lý: Nhảy lên và rơi xuống (Parabol)
        if (_rb != null)
        {
            _rb.bodyType = RigidbodyType2D.Dynamic; 
            _rb.gravityScale = 2.5f; // Tăng trọng lực xíu cho rơi nhanh, dứt khoát
            float randomDirection = UnityEngine.Random.Range(-1f, 1f) > 0 ? 1 : -1;
            Vector2 velocity = new Vector2(randomDirection * _throwForce, _jumpForce);
            _rb.linearVelocity = velocity;
            _rb.angularVelocity = randomDirection * -360f; 
        }
        Destroy(gameObject, _destroyDelay);
    }
    
    private void HandleOnState()
    {
        LayerMask targetLayer = wallLayer | tileLayer;
        
        onUp = Physics2D.Raycast(transform.position, Vector2.up, 0.6f, targetLayer);
        onDown = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, targetLayer);
        onLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.6f, targetLayer);
        onRight =  Physics2D.Raycast(transform.position, Vector2.right, 0.6f, targetLayer);
    }

    private void HandleIdleState()
    {
        if (onDown)
        {
            _animator.SetFloat(IdleStateParam, 0f);
        }
        else if (onLeft)
        {
            _animator.SetFloat(IdleStateParam, 0.25f);
        }
        else if (onRight)
        {
            _animator.SetFloat(IdleStateParam, 0.5f);
        }
        else if (onUp)
        {
            _animator.SetFloat(IdleStateParam, 0.75f);
        }
        else 
        {
            _animator.SetFloat(IdleStateParam, 1f); //Fly
        }
    }

    private void HandleMovingState(Tile.DIR direction)
    {
        _animator.SetTrigger("Moving");
        if (direction ==  Tile.DIR.DOWN)
        {
            _animator.SetFloat(MovingStateParam, 0f);
        }
        else if (direction ==  Tile.DIR.LEFT)
        {
            _animator.SetFloat(MovingStateParam, 0.33f);
        }
        else if (direction ==  Tile.DIR.RIGHT)
        {
            _animator.SetFloat(MovingStateParam, 0.67f);
        }
        else if (direction == Tile.DIR.UP)
        {
            _animator.SetFloat(MovingStateParam, 1f);
        }
        else return;
    }
}