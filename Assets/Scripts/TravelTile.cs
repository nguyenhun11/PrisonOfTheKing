using UnityEngine;
using UnityEngine.Serialization;

public class TravelTile : Tile
{
    [SerializeField] private Node _targetNode;//move from curr to target
    public float moveSpeed = 5f;
    private Collider2D _collider2D;
    public bool isMoving;
    
    public LayerMask wallLayer;

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
        if (isMoving && _targetNode != null)
        {
            // Lấy vị trí đích thực tế từ Node
            Vector3 targetPos = _targetNode.Position();

            // Di chuyển frame này
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            // Kiểm tra nếu khoảng cách cực nhỏ (coi như đã đến nơi)
            if (Vector3.Distance(transform.position, targetPos) < 0.001f)
            {
                // Bước quan trọng: Gán cứng vị trí bằng đích đến để triệt tiêu sai số float
                transform.position = targetPos;
            
                SnapToNode(); // Cập nhật dữ liệu Node hiện tại
                currNode = _targetNode;
                isMoving = false;
            }
        }
    }

    public void Move(Tile.DIR dir)// Sử dụng từ bên ngoài để tile di chuyển
    {
        if (dir == Tile.DIR.NONE) return;
        SetDestination(DirToVector2[dir]);
        isMoving = true;
    }

    void SetDestination(Vector2 direction)
    {
        // Bắn Raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 100, wallLayer);

        if (hit.collider != null)
        {
            // Lấy điểm va chạm
            Vector3 hitPoint = hit.point;

            // Lùi lại một chút từ điểm va chạm theo hướng ngược lại của ray.
            Vector3 fixedPos = hitPoint - (Vector3)(direction * 0.5f); // Lấy tâm của ô ngay trước tường
            
            _targetNode = new Node(fixedPos);
        }
    }
    
    // Trong TravelTile.cs
    public Vector3 GetTargetPosition()
    {
        return _targetNode != null ? _targetNode.Position() : transform.position;
    }
    
    
}
