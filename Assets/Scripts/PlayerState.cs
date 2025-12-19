using System.Collections;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private TravelTile _travelTile;
    private PlayerMovement _playerMovement;
    private PlayerAnimator _playerAnimator;

    [Header("Death Settings")] [SerializeField]
    private float _jumpForce = 8f; // Lực nảy lên cao

    [SerializeField] private float _throwForce = 3f; // Lực bay ngang
    [SerializeField] private float _destroyDelay = 2f; // Thời gian chờ rơi khỏi màn hình rồi mới xóa
    private bool _isDead = false;
    private Rigidbody2D _rb;
    private Collider2D _col;

    public delegate void Infor();

    public event Infor OnPlayerDeath;


    private void Start()
    {
        _travelTile = GetComponent<TravelTile>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        _travelTile.OnTravelTileStop += Stop;
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
    }

    public void Fall()
    {
        _playerMovement.canMove = false;
        StartCoroutine(HandleFallAnimation());
    }

    private IEnumerator HandleFallAnimation()
    {
        _playerAnimator.Fall();
        yield return new WaitForSeconds(0.5f);
        _playerMovement.MoveCharacter(Tile.DIR.DOWN);
        _playerMovement.canMove = true;
    }

    public void Stop()
    {
        Controller_Sound.Play("Stop");
       
    }

    public void Die()
    {
        if (_isDead) return;
        _isDead = true;

        _playerAnimator.Die();
        Controller_Sound.Play("PlayerDie");

        if (_travelTile != null) _travelTile.enabled = false;
        if (_col != null) _col.enabled = false;

        if (_rb != null)
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.gravityScale = 2.5f; // Tăng trọng lực xíu cho rơi nhanh, dứt khoát
            float randomDirection = UnityEngine.Random.Range(-1f, 1f) > 0 ? 1 : -1;
            Vector2 velocity = new Vector2(randomDirection * _throwForce, _jumpForce);
            _rb.linearVelocity = velocity;
            _rb.angularVelocity = randomDirection * -360f;
        }

        StartCoroutine(HandleDestroyAndDeath());
    }

    private IEnumerator HandleDestroyAndDeath()
    {
        yield return new WaitForSeconds(_destroyDelay);
        OnPlayerDeath?.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Stop();
        }
    }
}
