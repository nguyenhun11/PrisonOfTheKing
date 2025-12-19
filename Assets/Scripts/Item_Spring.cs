using System.Collections;
using UnityEngine;

public class Item_Spring : MonoBehaviour
{
    [Header("Settings")]
    public Tile.DIR moveDir;
    
    private Tile _tile;
    private Animator _animator;
    private static readonly int Trigger = Animator.StringToHash("Trigger");
    
    // --- KHÓA LOGIC ---
    // Biến này để đánh dấu lò xo đang bận xử lý, không nhận thêm lệnh Trigger
    [SerializeField] private bool _isBouncing = false; 

    [SerializeField] private TravelTile _pendingTravelTile;

    private void Start()
    {
        if (moveDir == Tile.DIR.NONE) moveDir = Tile.DIR.UP;
        
        _animator = GetComponent<Animator>();
        _tile = GetComponent<Tile>();
        
        if (_tile.currNode == null) _tile.SnapToNode();
        HandleSpriteRotation();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isBouncing) return;
        if (other.GetComponent<Enemy>() != null) return;

        if (other.TryGetComponent(out TravelTile travelTile))
        {
            if (travelTile.IsMoving)
            {
                _isBouncing = true;
                
                travelTile.Move(_tile.currNode.GetNode(moveDir)); 
                
                _pendingTravelTile = travelTile;
                _pendingTravelTile.OnTravelTileStop += OnObjectArrivedAtCenter;
            }
        }
    }
    
    private void OnObjectArrivedAtCenter()
    {
        if (_pendingTravelTile == null) return;
        _pendingTravelTile.OnTravelTileStop -= OnObjectArrivedAtCenter;

        if (_animator != null) _animator.SetTrigger(Trigger);
        Controller_Sound.Play("Spring");
        
        if (_pendingTravelTile.TryGetComponent(out PlayerMovement player))
        {
            player.MoveCharacter(moveDir);
        }
        else
        {
            _pendingTravelTile.Move(moveDir);
        }

        _pendingTravelTile = null;

        StartCoroutine(ResetSpringCooldown());
    }

    private IEnumerator ResetSpringCooldown()
    {
        // Chờ khoảng 0.5 giây (hoặc đủ thời gian để bóng rời khỏi ô này)
        yield return new WaitForSeconds(0.3f); 
        _isBouncing = false; // Mở khóa để đón lượt khách tiếp theo
    }
    
    private void OnDisable()
    {
        if (_pendingTravelTile != null)
        {
            _pendingTravelTile.OnTravelTileStop -= OnObjectArrivedAtCenter;
        }
    }
    
    private void HandleSpriteRotation()
    {
        if (moveDir == Tile.DIR.UP)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (moveDir == Tile.DIR.DOWN)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else if (moveDir == Tile.DIR.RIGHT)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
        else if (moveDir == Tile.DIR.LEFT)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
    }
}