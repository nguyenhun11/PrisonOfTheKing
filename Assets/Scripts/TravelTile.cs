using UnityEngine;
using UnityEngine.Serialization;

public class TravelTile : Tile
{
    [Header("Moving settings")]
    public Node targetNode;//move from curr to target
    public LayerMask wallLayer;
    public bool IsMoving {get; private set;}
    public float moveSpeed = 5f;
    public Tile.DIR MoveDir {get; private set;}
    
    [Header("Move by others")]
    public bool canMoveByOthers = true;
    public bool sameSpeedWithOther = true;
    
    
    private Collider2D _collider2D;
    
    public delegate void StopMoveDelegate();
    public event StopMoveDelegate OnTravelTileStop;
    void Start()
    {
        SnapToNode();
        _collider2D = GetComponent<Collider2D>();
    }
    
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (IsMoving && targetNode != null)
        {
            // Lấy vị trí đích thực tế từ Node
            Vector3 targetPos = targetNode.Position();
            
            // Di chuyển frame này
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            // Kiểm tra nếu khoảng cách cực nhỏ (coi như đã đến nơi)
            if (Vector3.Distance(transform.position, targetPos) < 0.001f)
            {
                Stop();
            }
        }
    }

    public void Move(Tile.DIR dir)// Sử dụng từ bên ngoài để tile di chuyển
    {
        if (dir == Tile.DIR.NONE) return;
        SetDestination(DirToVector2[dir]);
        IsMoving = true;
        MoveDir = dir;
        Debug.Log(this.name + currNode);
    }

    void SetDestination(Vector2 direction)//Xác định target
    {
        // Bắn Raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 100, wallLayer);

        if (hit.collider != null)
        {
            // Lấy điểm va chạm
            Vector3 hitPoint = hit.point;

            // Lùi lại một chút từ điểm va chạm theo hướng ngược lại của ray.
            Vector3 fixedPos = hitPoint - (Vector3)(direction * 0.5f); // Lấy tâm của ô ngay trước tường
            
            targetNode = new Node(fixedPos);
        }
    }

    public void Stop()
    {
        SnapToNode(); // Cập nhật dữ liệu Node hiện tại
        IsMoving = false;
        MoveDir = DIR.NONE;
        OnTravelTileStop?.Invoke();
    }

    public void Fall()
    {
        Stop();
        Move(DIR.DOWN);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canMoveByOthers && other.TryGetComponent(out TravelTile otherTravelTile))//Di chuyển khi các TravelTile chạm nhau
        {
            if (otherTravelTile.IsMoving && !IsMoving && SameRowOrSameCol(otherTravelTile))//
            {
                if (sameSpeedWithOther) moveSpeed = otherTravelTile.moveSpeed;
                Move(otherTravelTile.MoveDir);
            }
        }
    }
}