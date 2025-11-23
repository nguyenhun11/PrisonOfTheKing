using UnityEngine;

public class Item_Ball : MonoBehaviour
{
    [Header("Settings")]
    public bool breakWhenStop = false; // Nếu true: Bóng sẽ vỡ khi dừng lại
    
    private TravelTile _travelTile;
    private Animator _animator;
    private Collider2D _collider;

    void Start()
    {
        _travelTile = GetComponent<TravelTile>();
        _animator = GetComponent<Animator>(); // Có thể null nếu bóng không có animation
        _collider = GetComponent<Collider2D>();

        // Đăng ký sự kiện dừng
        if (_travelTile != null)
        {
            _travelTile.OnTravelTileStop += OnBallStop;
        }
    }
    
    private void OnDisable()
    {
        if (_travelTile != null)
        {
            _travelTile.OnTravelTileStop -= OnBallStop;
        }
    }

    private void OnBallStop()
    {
        if (breakWhenStop)
        {
            HandleBreak();
            return;
        }
        _travelTile.Fall(false);
    }

    private void HandleBreak()
    {
        if (_collider != null) _collider.enabled = false;

        if (_animator != null)
        {
            _animator.SetTrigger("Break");
        }
        else
        {
            DestroySelf();
        }
    }

    // --- HÀM NÀY ĐƯỢC GỌI BỞI ANIMATION EVENT ---
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}