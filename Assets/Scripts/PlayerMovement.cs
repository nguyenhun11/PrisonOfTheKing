using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private TravelTile _travelTile;
    private Collider2D _collider2D;
    private Rigidbody2D _rigidbody2D;

    // --- CẤU HÌNH INPUT ---
    [Header("Input Settings")]
    [SerializeField] private float minSwipeDistance = 20f; // Ngưỡng tối thiểu để nhận diện là vuốt (pixel)
    private Vector2 _fingerDownPos;
    private Vector2 _fingerUpPos;
    private bool _isDetectingSwipe = false;
    // ----------------------

    public bool IsMoving
    {
        get { return _travelTile.IsMoving; }
    }

    public bool canMove = true;

    private Tile.DIR _bufferDirWhenMoving = Tile.DIR.NONE;
    public delegate void MoveAction(Tile.DIR direction);

    public event MoveAction OnPlayerMoved;

    void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _travelTile = GetComponent<TravelTile>();
    }

    void Update()
    {
        // Lấy Input từ hàm mới đã hỗ trợ cả Swipe và Keyboard
        Tile.DIR inputThisFrame = GetDirInput(); 

        if (!canMove || IsMoving)
        {
            if (_bufferDirWhenMoving == Tile.DIR.NONE)
            {
                _bufferDirWhenMoving = inputThisFrame;
            }
        }
        else
        {
            if (_bufferDirWhenMoving != Tile.DIR.NONE)
            {
                MoveCharacter(_bufferDirWhenMoving);
                _bufferDirWhenMoving = Tile.DIR.NONE;
            }
            else if (inputThisFrame != Tile.DIR.NONE)
            {
                MoveCharacter(inputThisFrame);
            }
        }
    }

    public void MoveCharacter(Tile.DIR direction)
    {
        _travelTile.Move(direction);
        OnPlayerMoved?.Invoke(direction);
    }

    // Hàm xử lý Input chính
    private Tile.DIR GetDirInput()
    {
        // 1. Ưu tiên kiểm tra Keyboard trước (để test trên Unity Editor dễ dàng)
        Tile.DIR keyboardDir = GetKeyboardInput();
        if (keyboardDir != Tile.DIR.NONE) return keyboardDir;

        // 2. Kiểm tra Swipe Input trên điện thoại (hoặc giả lập chuột)
        return GetSwipeInput();
    }

    private Tile.DIR GetSwipeInput()
    {
        // Logic cho chuột (Editor) và Cảm ứng (Mobile)
        // Unity cũ dùng Input.GetMouseButton cho cả touch đơn điểm
        
        if (Input.GetMouseButtonDown(0))
        {
            _fingerDownPos = Input.mousePosition;
            _fingerUpPos = Input.mousePosition;
            _isDetectingSwipe = true;
        }

        if (_isDetectingSwipe && Input.GetMouseButtonUp(0))
        {
            _fingerUpPos = Input.mousePosition;
            _isDetectingSwipe = false;
            return DetectDirection();
        }
        
        // Nếu muốn vuốt mà không cần nhấc tay (continuous swipe), 
        // có thể dùng Input.GetMouseButton(0) và reset _fingerDownPos sau khi nhận diện xong.

        return Tile.DIR.NONE;
    }

    private Tile.DIR DetectDirection()
    {
        // Tính vector khoảng cách
        Vector2 swipeVector = _fingerUpPos - _fingerDownPos;

        // Nếu khoảng cách vuốt quá ngắn -> coi như là chạm (tap), không di chuyển
        if (swipeVector.magnitude < minSwipeDistance) return Tile.DIR.NONE;

        // So sánh độ lớn của X và Y để biết vuốt ngang hay dọc
        if (Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y))
        {
            // Vuốt Ngang
            return swipeVector.x > 0 ? Tile.DIR.RIGHT : Tile.DIR.LEFT;
        }
        else
        {
            // Vuốt Dọc
            return swipeVector.y > 0 ? Tile.DIR.UP : Tile.DIR.DOWN;
        }
    }

    private Tile.DIR GetKeyboardInput()
    {
        if (Input.GetKey(KeyCode.W)) return Tile.DIR.UP;
        if (Input.GetKey(KeyCode.S)) return Tile.DIR.DOWN;
        if (Input.GetKey(KeyCode.A)) return Tile.DIR.LEFT;
        if (Input.GetKey(KeyCode.D)) return Tile.DIR.RIGHT;
        return Tile.DIR.NONE;
    }
}