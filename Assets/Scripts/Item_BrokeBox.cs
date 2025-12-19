using UnityEngine;

public class Item_BrokeBox : MonoBehaviour
{
    private Animator _animator;
    private Collider2D _collider;
    private TravelTile _travelTile;
    public bool _canBreak =  false;
    public Tile.DIR breakBySide = Tile.DIR.NONE;
    
    [Header("Break handle")]
    public LayerMask wallLayer;
    public LayerMask tileLayer;
    public bool onRight;
    public bool onLeft;
    public bool onUp;
    public bool onDown;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>(); // Lấy cả BoxCollider2D
        _travelTile = GetComponent<TravelTile>();
        _travelTile.OnTravelTileMove += HandleCanBreak;
        
    }
    
    private void OnDisable()
    {
        _travelTile.OnTravelTileStop -= Break;
        _travelTile.OnTravelTileMove -= HandleCanBreak;
    }

    private void HandleCanBreak(Tile.DIR dir)
    {
        _canBreak = true;
        _travelTile.OnTravelTileStop += Break;
        breakBySide = dir;
    }
    
    // Hàm này được gọi khi Box bị đập (VD: Player va chạm hoặc Boom nổ)
    private void Break()
    {
        if (!CanBreak(breakBySide)) return;
        _animator.SetTrigger("Break");
        Controller_Sound.Play("Boom");
        if (_collider != null) _collider.enabled = false;
    }
    
    private bool CanBreak(Tile.DIR dir)
    {
        if (!_canBreak) return false;
        LayerMask targetLayer = wallLayer | tileLayer;
        
        onUp = Physics2D.Raycast(transform.position, Vector2.up, 0.6f, targetLayer);
        onDown = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, targetLayer);
        onLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.6f, targetLayer);
        onRight =  Physics2D.Raycast(transform.position, Vector2.right, 0.6f, targetLayer);

        if (dir == Tile.DIR.UP && onUp) return true;
        if (dir == Tile.DIR.DOWN && onDown) return true;
        if (dir == Tile.DIR.RIGHT && onRight) return true;
        if (dir == Tile.DIR.LEFT && onLeft) return true;
        return false;
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