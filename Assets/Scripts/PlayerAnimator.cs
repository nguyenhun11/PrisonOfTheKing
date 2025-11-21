using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TravelTile _travelTile; // Kéo TravelTile vào đây
    [SerializeField] private Animator _animator;
    
    [Header("Settings")]
    public LayerMask wallLayer;
    
    // Animator Parameters ID
    private static readonly int IsMovingParam = Animator.StringToHash("IsMoving");
    private static readonly int MoveStateParam = Animator.StringToHash("MoveState"); // 0:Up, 1:Down, 0.5:Run

    // State Variables
    private bool _wasMoving;
    private Tile.DIR _lastMoveDir;
    private float _currentAnimState; // Lưu trạng thái animation hiện tại (Run/Up/Down)

    void Update()
    {
        bool isMoving = _travelTile.isMoving;
        
        // Lấy hướng di chuyển thực tế (Vector) để tính toán logic
        Vector2 moveVector = Vector2.zero;
        if (_travelTile.currNode != null && isMoving)
        {
            // Tính vector hướng đi dựa trên đích đến
             moveVector = (_travelTile.GetTargetPosition() - transform.position).normalized;
        }
        
        // Cập nhật Animator
        _animator.SetBool(IsMovingParam, isMoving);

        if (isMoving)
        {
            HandleMovingState(moveVector);
            _wasMoving = true;
        }
        else
        {
            if (_wasMoving)
            {
                HandleStopState();
                _wasMoving = false;
            }
        }
    }

    // XỬ LÝ TRẠNG THÁI KHI ĐANG DI CHUYỂN (Rule 3, 4, 5)
    private void HandleMovingState(Vector2 moveVec)
    {
        Tile.DIR dir = VectorToDir(moveVec);
        _lastMoveDir = dir; // Lưu hướng để dùng cho logic khi dừng lại (Rule 6)

        float stateToSet = 0; // Mặc định là Moving Up (0)

        // Rule 5: Moving Down -> Luôn là State 1
        if (dir == Tile.DIR.DOWN)
        {
            stateToSet = 1;
        }
        // Rule 4 & 3: Running vs Moving Up
        else if (dir == Tile.DIR.LEFT || dir == Tile.DIR.RIGHT)
        {
            // Kiểm tra xem có tường ở dưới chân (so với hướng trọng lực thế giới) hoặc trên trần không
            // Bắn raycast ngắn lên trên và xuống dưới
            bool hasGround = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, wallLayer);
            bool hasCeiling = Physics2D.Raycast(transform.position, Vector2.up, 0.6f, wallLayer);

            if (hasGround || hasCeiling) 
            {
                stateToSet = 0.5f; // Running
            }
            else
            {
                stateToSet = 0; // Moving Up (Bay ngang qua khoảng không)
            }
        }
        else // Tile.DIR.UP
        {
            stateToSet = 0; // Moving Up
        }

        _currentAnimState = stateToSet;
        _animator.SetFloat(MoveStateParam, stateToSet);
        
        // Xử lý quay mặt (Facing) khi đang di chuyển
        HandleRotationWhileMoving(dir);
    }

    // XỬ LÝ TRẠNG THÁI KHI VỪA DỪNG LẠI (Rule 1, 2, 6)
    private void HandleStopState()
    {
        // Rule 6a: Nếu đang Running, Idle hướng ngược lại hướng đã chạy
        if (_currentAnimState == 0.5f) 
        {
            FlipFacing(); // Đảo ngược Scale X
        }
        // Rule 6b: Nếu đang Moving Up, giữ nguyên hoặc reset về mặc định (Code hiện tại giữ nguyên hướng cũ)
        else if (_currentAnimState == 0)
        {
            // "Idle hướng nào cũng được" -> Giữ nguyên hướng hiện tại
            AlignToGravity(Vector2.down); // Mặc định chân hướng xuống đất
        }
        // Rule 6c: Nếu đang Moving Down
        else if (_currentAnimState == 1)
        {
            CheckSideWallsAndOrient();
        }
    }

    // HÀM PHỤ TRỢ: Xoay trục Z để chân chạm tường (Rule 1)
    // SurfaceNormal: Vector pháp tuyến của bề mặt (Ví dụ tường bên trái thì Normal là Right)
    private void AlignToSurface(Vector2 surfaceNormal)
    {
        float rotZ = 0;
        if (surfaceNormal == Vector2.up) rotZ = 0;         // Đất
        else if (surfaceNormal == Vector2.down) rotZ = 180; // Trần
        else if (surfaceNormal == Vector2.right) rotZ = -90; // Tường Trái
        else if (surfaceNormal == Vector2.left) rotZ = 90;   // Tường Phải

        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
    
    // Helper: Reset về trọng lực bình thường (chân chạm đất)
    private void AlignToGravity(Vector2 gravityDir)
    {
        if(gravityDir == Vector2.down) transform.rotation = Quaternion.Euler(0,0,0);
    }

    // Xử lý quay mặt và xoay trục Z khi đang di chuyển
    private void HandleRotationWhileMoving(Tile.DIR dir)
    {
        // Mặc định xoay Z về 0 (đứng thẳng) khi di chuyển ngang/dọc trừ khi bám trần
        // Logic đơn giản hóa: Luôn hướng chân xuống dưới khi bay, trừ khi chạy trên trần
        
        bool isOnCeiling = Physics2D.Raycast(transform.position, Vector2.up, 0.6f, wallLayer);
        
        if (isOnCeiling && (dir == Tile.DIR.LEFT || dir == Tile.DIR.RIGHT))
        {
            transform.rotation = Quaternion.Euler(0, 0, 180); // Chạy trên trần
        }
        else
        {
            transform.rotation = Quaternion.identity; // Reset Z
        }

        // Xử lý Flip X (Mặt hướng theo hướng đi)
        Vector3 scale = transform.localScale;
        if (dir == Tile.DIR.LEFT) scale.x = -Mathf.Abs(scale.x);
        else if (dir == Tile.DIR.RIGHT) scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private void FlipFacing()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void CheckSideWallsAndOrient()
    {
        // Rule 6c: Moving Down -> Check 2 bên
        bool leftWall = Physics2D.Raycast(transform.position, Vector2.left, 0.6f, wallLayer);
        bool rightWall = Physics2D.Raycast(transform.position, Vector2.right, 0.6f, wallLayer);

        Vector3 scale = transform.localScale;

        if (leftWall && !rightWall)
        {
            // Có tường trái, không có tường phải -> Quay mặt sang Phải (xa tường)
            scale.x = Mathf.Abs(scale.x);
            // Chỉnh chân bám vào tường trái? Hoặc đứng dưới đất? 
            // Theo yêu cầu "Idle về bên không có tường", tức là nhìn về bên trống
            transform.rotation = Quaternion.identity; 
        }
        else if (!leftWall && rightWall)
        {
            // Có tường phải, không tường trái -> Quay mặt sang Trái
            scale.x = -Mathf.Abs(scale.x);
            transform.rotation = Quaternion.identity;
        }
        else
        {
            // Cả 2 hoặc không có gì -> Giữ nguyên hoặc random. 
            // Ở đây giữ nguyên rotation, chỉ đảm bảo Z thẳng
            transform.rotation = Quaternion.identity;
        }
        transform.localScale = scale;
    }

    // Helper convert Vector -> Enum
    private Tile.DIR VectorToDir(Vector2 vec)
    {
        if (Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
            return vec.x > 0 ? Tile.DIR.RIGHT : Tile.DIR.LEFT;
        return vec.y > 0 ? Tile.DIR.UP : Tile.DIR.DOWN;
    }
}