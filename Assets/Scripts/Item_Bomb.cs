using System;
using UnityEngine;

public class Item_Bomb : MonoBehaviour
{
    private TravelTile _travelTile;
    
    [Header("Moving on road")]
    public bool canMoveOnRoad = false; // Nên check cái này trong Start để tránh null
    public Road_Full roadFull;
    
    private bool isHeadingToEnd = true; 

    // Item_Bomb.cs

    void Awake()
    {
        _travelTile = GetComponent<TravelTile>();
    
        // Đăng ký sự kiện thì OK trong Awake
        _travelTile.OnTravelTileStop += ChangeDirection;
    }

    void Start()
    {
        // Kiểm tra node của chính mình
        if (_travelTile.currNode == null) _travelTile.SnapToNode();

        // Kiểm tra tham chiếu
        if (roadFull == null || roadFull.startPoint == null || roadFull.endPoint == null)
        {
            Debug.LogError("Boom: Thiếu Road hoặc Start/End points!");
            return;
        }

        // --- KHẮC PHỤC LỖI NULL ---
        // Đảm bảo các điểm đích đã có Node trước khi gọi lệnh đi
        if (roadFull.startPoint.currNode == null) roadFull.startPoint.SnapToNode();
        if (roadFull.endPoint.currNode == null) roadFull.endPoint.SnapToNode();

        // Bắt đầu di chuyển lần đầu tiên TẠI ĐÂY
        ChangeDirection(); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out TravelTile otherTravelTile))
        {
            if ((otherTravelTile.IsMoving && !_travelTile.IsMoving && _travelTile.SameRowOrSameCol(otherTravelTile))
                || _travelTile.IsMoving)
            {
                HandleItemType(other);
            }
        }
    }

    private void HandleItemType(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerState otherPlayerState))
        {
            otherPlayerState.Die();
            Boom();
        }

        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.Die();
            Boom();
        }
    }

    private void Boom()
    {
        Debug.Log("Boom nổ!");
        Destroy(gameObject);
    }
    
    private void ChangeDirection()
    {
        if (!canMoveOnRoad) return;

        // --- SỬA LOGIC ĐẢO CHIỀU ---
        if (isHeadingToEnd)
        {
            // Đang muốn đến End -> Đi tới EndPoint
            _travelTile.Move(roadFull.endPoint.currNode);
            
            // Thiết lập cho lần tới: Khi đến nơi rồi, thì lần sau sẽ quay về Start
            isHeadingToEnd = false; 
        }
        else
        {
            // Đang muốn về Start -> Đi tới StartPoint
            _travelTile.Move(roadFull.startPoint.currNode);
            
            // Thiết lập cho lần tới: Khi về đến Start rồi, thì lần sau sẽ đi End
            isHeadingToEnd = true;
        }
    }

    private void OnDestroy()
    {
        if (_travelTile != null)
        {
            _travelTile.OnTravelTileStop -= ChangeDirection;
        }
    }
}