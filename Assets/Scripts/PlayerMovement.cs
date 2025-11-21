using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 400f; // Tốc độ di chuyển (cao vì map 24 unit rất rộng)
    public float tileSize = 24f;   // Kích thước 1 ô (quan trọng)
    public LayerMask wallLayer;    // Layer của tường để Raycast nhận diện

    private Vector3 _targetPosition;
    private bool _isMoving = false;
    private Collider2D _myCollider;

    void Start()
    {
        // Snap vị trí ban đầu vào giữa ô lưới để tránh lỗi lẻ số
        SnapToGrid();
        _targetPosition = transform.position;
        _myCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        // 1. Nếu đang di chuyển, tiếp tục lướt tới đích
        if (_isMoving)
        {
            MovePlayer();
        }
        // 2. Nếu đang đứng yên, lắng nghe phím bấm
        else
        {
            CheckInput();
        }
    }

    void CheckInput()
    {
        Vector2 inputDir = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.UpArrow)) inputDir = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) inputDir = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) inputDir = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) inputDir = Vector2.right;

        if (inputDir != Vector2.zero)
        {
            // Tính toán điểm đích bằng Raycast
            SetDestination(inputDir);
        }
    }

    void SetDestination(Vector2 direction)
    {
        // Bắn Raycast từ tâm nhân vật theo hướng bấm
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, wallLayer);

        if (hit.collider)
        {
            // CÔNG THỨC QUAN TRỌNG:
            // Vị trí va chạm (hit.point) là bề mặt bức tường.
            // Ta cần lùi lại một nửa kích thước ô (tileSize / 2) để nhân vật dừng ở tâm ô trống kế bên tường.
            
            float distanceToStop = hit.distance - (tileSize / 2f);
            
            // Tính toạ độ đích
            Vector3 calculatedPos = transform.position + (Vector3)direction * distanceToStop;

            // Làm tròn toạ độ theo Grid để đảm bảo "Pixel Perfect" (tránh sai số 0.0001)
            _targetPosition = RoundToGrid(calculatedPos);

            // Chỉ di chuyển nếu khoảng cách đủ xa (tránh kẹt tường)
            if (Vector3.Distance(transform.position, _targetPosition) > 0.1f)
            {
                _isMoving = true;
            }
        }
    }

    void MovePlayer()
    {
        // Di chuyển mượt mà tới đích
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, moveSpeed * Time.deltaTime);

        // Kiểm tra đã đến nơi chưa
        if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
        {
            transform.position = _targetPosition; // Snap thẳng vào vị trí
            _isMoving = false; // Cho phép nhập lệnh tiếp theo
        }
    }

    // Hàm hỗ trợ làm tròn toạ độ về bội số của TileSize (24)
    Vector3 RoundToGrid(Vector3 pos)
    {
        float x = Mathf.Round((pos.x))+0.5f; // +12 vì tâm ô lệch 0.5
        float y = Mathf.Round((pos.y))+0.5f;
        return new Vector3(x, y, pos.z);
    }
    
    void SnapToGrid()
    {
        transform.position = RoundToGrid(transform.position);
    }
}