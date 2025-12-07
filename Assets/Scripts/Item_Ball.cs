using System.Collections;
using UnityEngine;

public class Item_Ball : MonoBehaviour
{
    [Header("Settings")]
    public bool breakWhenStop = false;
    
    [Tooltip("Chọn layer của: Tường, Đất, Hộp, Bóng khác, Enemy... (Tất cả những gì có thể đứng lên được)")]
    public LayerMask wallLayer; 

    [Header("Debug")]
    public bool onGround; // Để xem trong Inspector

    private TravelTile _travelTile;
    private Animator _animator;
    private Collider2D _collider2D;

    void Start()
    {
        _travelTile = GetComponent<TravelTile>();
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();

        if (_travelTile != null)
        {
            _travelTile.OnTravelTileStop += OnBallStop;
        }

        // Check ngay khi sinh ra để tránh lơ lửng
        //CheckOnGround();
    }
    
    private void OnDisable()
    {
        if (_travelTile != null) _travelTile.OnTravelTileStop -= OnBallStop;
    }

    private void OnBallStop()
    {
        if (breakWhenStop)
        {
            HandleBreak();
            return;
        }
        Debug.Log("Ball stop and not break");
        // KHÔNG check ngay lập tức, hãy chờ vật lý ổn định
        StartCoroutine(CheckGroundAndDecide());
    }

    // Trong Item_Ball.cs

    private IEnumerator CheckGroundAndDecide()
    {
        yield return new WaitForEndOfFrame();
        
        onGround = _travelTile.CheckOnGround();
        Debug.Log("Check on Ground: " + onGround);
        
        if (!onGround)
        {
            if (gameObject.activeSelf && !_travelTile.IsMoving)
            {
                bool moveSuccess = _travelTile.Move(Tile.DIR.DOWN);
                if (!moveSuccess)
                {
                    onGround = true;
                    //gameObject.layer = LayerMask.NameToLayer("Wall");
                }
            }
        }
        else
        {
            //gameObject.layer = LayerMask.NameToLayer("Wall");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerAnimator player))
        {
            player.StandOnGround();
        }
    }

    private void HandleBreak()
    {
        if (_collider2D != null) _collider2D.enabled = false;
        if (_animator != null) _animator.SetTrigger("Break");
        else DestroySelf();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}