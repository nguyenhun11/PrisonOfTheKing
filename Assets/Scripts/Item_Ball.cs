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
        CheckOnGround();
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

        // KHÔNG check ngay lập tức, hãy chờ vật lý ổn định
        StartCoroutine(CheckGroundAndDecide());
    }

    private IEnumerator CheckGroundAndDecide()
    {
        // Chờ hết frame hiện tại (để đảm bảo bóng đã snap vào đúng ô Grid)
        yield return new WaitForEndOfFrame();

        // 1. Kiểm tra đất bằng BoxCast
        CheckOnGround();

        // 2. Quyết định
        if (!onGround)
        {
            // Nếu không có đất -> Rơi xuống
            if (gameObject.activeSelf && !_travelTile.IsMoving)
            {
                _travelTile.Move(Tile.DIR.DOWN);
            }
        }
        else
        {
            // Có đất -> Đứng yên (Không làm gì cả)
            // Lúc này logic TravelTile đã gọi Stop() rồi, nên bóng sẽ nằm im.
        }
    }

    private void CheckOnGround()
    {
        // Tắt Collider của chính mình để không tự quét trúng mình
        if (_collider2D != null) _collider2D.enabled = false;

        // --- SỬ DỤNG BOXCAST (QUAN TRỌNG) ---
        // Origin: Vị trí bóng
        // Size: 0.9f x 0.9f (Nhỏ hơn 1 chút so với ô 1x1 để không bị vướng tường bên cạnh)
        // Direction: Xuống dưới
        // Distance: 1.1f (Dài hơn 1 ô một chút để chắc chắn chạm vật bên dưới)
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(0.9f, 0.9f), 0f, Vector2.down, 1.1f, wallLayer);

        if (_collider2D != null) _collider2D.enabled = true;

        // Reset trạng thái
        onGround = false;

        if (hit.collider != null)
        {
            // Bất kể đụng trúng cái gì (Tile tĩnh hay TravelTile động)
            // Miễn là nó nằm ở layer WallLayer -> Coi là đất
            onGround = true;
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

    // --- VẼ DEBUG ĐỂ DỄ NHÌN TRONG SCENE ---
    private void OnDrawGizmos()
    {
        Gizmos.color = onGround ? Color.green : Color.red;
        // Vẽ hình hộp mô phỏng BoxCast
        Gizmos.DrawWireCube(transform.position + Vector3.down * 1.1f, new Vector3(0.9f, 0.9f, 0));
    }
}