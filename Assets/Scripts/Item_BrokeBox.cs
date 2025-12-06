using UnityEngine;

public class Item_BrokeBox : MonoBehaviour
{
    private Animator _animator;
    private Collider2D _collider;
    private TravelTile _travelTile;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>(); // Lấy cả BoxCollider2D
        _travelTile = GetComponent<TravelTile>();
        _travelTile.OnTravelTileStop += Break;
    }
    
    private void OnDisable()
    {
        _travelTile.OnTravelTileStop -= Break;
    }

    // Hàm này được gọi khi Box bị đập (VD: Player va chạm hoặc Boom nổ)
    private void Break()
    {
        // 1. Chạy Animation vỡ
        _animator.SetTrigger("Break");
        
        if (_collider != null) _collider.enabled = false;
        
        // Lưu ý: KHÔNG gọi Destroy ở đây
    }

    // Hàm này sẽ được Animation Event gọi ở frame cuối cùng
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerAnimator player))
        {
            player.StandOnGround();
        }
    }
}