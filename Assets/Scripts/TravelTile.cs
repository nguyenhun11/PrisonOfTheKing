using UnityEngine;

public class TravelTile : Tile
{
    [Header("Moving settings")]
    private Node _targetNode;
    public LayerMask wallLayer;
    public bool IsMoving { get; private set; }
    public bool DebugIsMoving;
    public float moveSpeed = 5f;
    public Tile.DIR MoveDir { get; private set; }
    
    [Header("Interaction Settings")]
    public bool canMoveByOthers = true;     
    public bool sameSpeedWithOther = true;  
    public bool stopOther = false;           

    private TravelTile _followerTile = null;
    private Collider2D _collider2D;

    public delegate void StateDelegate();
    public event StateDelegate OnTravelTileStop;

    void Awake() // Dùng Awake an toàn hơn Start
    {
        SnapToNode();
        _collider2D = GetComponent<Collider2D>(); // Đảm bảo dòng này có
    }
    
    void Update()
    {
        MoveToTarget();
        DebugIsMoving = IsMoving;
    }

    private void MoveToTarget()
    {
        if (IsMoving && _targetNode != null)
        {
            Vector3 targetPos = _targetNode.Position();
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.001f)
            {
                Stop(); 
            }
        }
    }

    // --- CÁC HÀM DI CHUYỂN

    public void Move(Tile.DIR dir)
    {
        Debug.Log(name + ": SO want me to go to " + dir);
        if (dir == Tile.DIR.NONE || IsMoving) return; // Nếu đang đi thì không nhận lệnh mới

        Vector2 dirVec = DirToVector2[dir];
        
        // Ban raycast
        if (_collider2D != null) _collider2D.enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirVec, 1.0f, wallLayer);
        if (_collider2D != null) _collider2D.enabled = true;
        
        if (hit.collider != null)
        {
            // A. Gặp TravelTile (Hộp, Bóng, Người khác)
            if (hit.collider.TryGetComponent(out TravelTile neighborTile))
            {
                // Nếu thằng kia đứng yên và cho phép đẩy
                if (!neighborTile.IsMoving && neighborTile.canMoveByOthers)
                {
                    // --- ĐỆ QUY: RA LỆNH CHO NÓ ĐI TRƯỚC ---
                    if (neighborTile.sameSpeedWithOther) neighborTile.moveSpeed = this.moveSpeed;
                    
                    // Gọi nó di chuyển
                    neighborTile.Move(dir);

                    // --- KIỂM TRA LẠI SAU KHI RA LỆNH ---
                    // TH1: Đá Banh (stopOther = true) -> Nó đi, mình đứng
                    if (neighborTile.stopOther) return;

                    // TH2: Đẩy Hộp -> Nếu đẩy xong mà nó vẫn đứng yên (do nó bị kẹt tường) -> Mình cũng đứng
                    if (!neighborTile.IsMoving) return;

                    // Nếu nó đi thành công -> Mình đi theo (Gán follower)
                    neighborTile.SetFollower(this);
                }
                // Nếu gặp vật không đẩy được hoặc đang di chuyển ngược chiều -> Đứng yên
                else 
                {
                    return; 
                }
            }
            // B. Gặp Tile thường (Tường, Đá cố định)
            else if (hit.collider.TryGetComponent(out Tile wall))
            {
                if (wall.isStopTile) return; // Bị chặn -> Đứng yên
            }
        }

        // --- BƯỚC 3: NẾU KHÔNG CÒN VẬT CẢN -> TÌM ĐÍCH ĐẾN ---
        // (Lúc này đường đã thoáng vì thằng hàng xóm đã bắt đầu di chuyển rồi)
        
        // Tắt collider lần nữa để tìm đích xa (SetDestination dùng Raycast xa)
        
        SetDestination(dirVec);
        
        // Kích hoạt di chuyển
        IsMoving = true; 
        MoveDir = dir;
    }
    
    public void Move(Node target)
    {
        //if (IsMoving) return;
        Debug.Log(name + ": SO want me to go to " + target);
        if (currNode.x == target.x || currNode.y == target.y)
        {
            this._targetNode = target;
            if (currNode.x == target.x)
                MoveDir = (currNode.y > target.y) ? DIR.DOWN : DIR.UP;
            else
                MoveDir = (currNode.x > target.x) ? DIR.LEFT : DIR.RIGHT;
            IsMoving = true; 
        }
    }

    public void SetFollower(TravelTile follower)
    {
        _followerTile = follower;
    }

    void SetDestination(Vector2 direction)
    {
        // Raycast này tìm điểm đích xa (Tường)
        if (_collider2D != null) _collider2D.enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 100, wallLayer);
        if (_collider2D != null) _collider2D.enabled = true;
        
        if (hit.collider != null)
        {
            Vector3 hitPoint = hit.point;
            Vector3 fixedPos = hitPoint - (Vector3)(direction * 0.5f);
            _targetNode = new Node(fixedPos);
        }
    }

    // --- HÀM STOP (GỌN HƠN) ---
    public void Stop(bool invokeEvent = true)
    {
        IsMoving = false; 
        MoveDir = DIR.NONE;
        currNode = SnapToNode(); 
        _targetNode = currNode;
        
        if (_followerTile != null)
        {
            if (isStopTile) 
            {
                if (_followerTile.IsMoving) _followerTile.Stop();
            }
            _followerTile = null; 
        }

        if(invokeEvent) OnTravelTileStop?.Invoke();
    }
    
    // --- TRIGGER ENTER: Xử lý va chạm ĐỘNG (từ xa lao tới) ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsMoving) return; // Chỉ người đang đi mới xử lý

        if (other.TryGetComponent(out TravelTile staticTravelTile))
        {
            // Chỉ tương tác nếu thẳng hàng và vật kia đứng yên
            if (SameRowOrSameCol(staticTravelTile) && !staticTravelTile.IsMoving)
            {
                if (staticTravelTile.canMoveByOthers)//Day
                {
                    if (staticTravelTile.sameSpeedWithOther)
                    {
                        staticTravelTile.moveSpeed = this.moveSpeed;
                    }
                    staticTravelTile.Move(this.MoveDir);
                    
                    if (staticTravelTile.stopOther)//Dung lai hoac di tiep
                    {
                        this.Stop();
                    }
                    else
                    {
                        // Nếu đẩy hộp mà hộp bị kẹt tường (không di chuyển được) -> Mình dừng
                        if (!staticTravelTile.IsMoving) 
                        {
                            this.Stop();
                        }
                        else 
                        {
                            staticTravelTile.SetFollower(this); // Đẩy hộp -> Đi theo
                        }
                    }
                }
                else if (staticTravelTile.isStopTile)
                {
                    this.Stop(); // Gặp đá tảng -> Dừng
                }
            }
        }
        else if (other.TryGetComponent(out Tile staticTile))
        {
            if (SameRowOrSameCol(staticTile) && staticTile.isStopTile)
            {
                this.Stop(); // Gặp tường -> Dừng
            }
        }
    }
}