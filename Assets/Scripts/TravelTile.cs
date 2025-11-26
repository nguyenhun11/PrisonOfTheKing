using System;
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

    public delegate void MoveDelegate(DIR dir);
    public event StateDelegate OnTravelTileStop;
    //public event MoveDelegate OnTravelTileMove;
    public Func<Tile.DIR, Node> OnCalculateTargetNode;

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
        if (IsMoving && _targetNode != null && (currNode.x != _targetNode.x || currNode.y != _targetNode.y))
        {
            Vector3 targetPos = _targetNode.Position();
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.001f)
            {
                Stop(); 
            }
        }
    }

    public bool Move(Tile.DIR dir)
    {
        // Debug.Log(name + ": SO want me to go to " + dir);
        if (dir == Tile.DIR.NONE || IsMoving) return false;

        // 1. CHECK VẬT CẢN NGAY TRƯỚC MẶT
        if (_collider2D != null) _collider2D.enabled = false;
        Vector2 dirVec = DirToVector2[dir];

        // Lưu ý: Nên dùng độ dài < 1 (ví dụ 0.8f) để tránh bắn xuyên sang ô kế tiếp quá sâu
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirVec, 1f, wallLayer);

        if (_collider2D != null) _collider2D.enabled = true;

        if (hit.collider != null)
        {
            // TH1: Gặp vật có thể đẩy được (TravelTile)
            if (hit.collider.TryGetComponent(out TravelTile neighborTile))
            {
                if (!neighborTile.IsMoving && neighborTile.canMoveByOthers)
                {
                    if (neighborTile.sameSpeedWithOther) neighborTile.moveSpeed = this.moveSpeed;

                    // Đệ quy: Ra lệnh cho thằng kia đi trước
                    if (!neighborTile.Move(dir))
                    {
                        Debug.Log(name + $"goi {neighborTile.name} di chuyen khong thanh cong");
                        if (neighborTile.isStopTile) return false;
                        neighborTile.Stop();//Enemy chet
                        return true;
                    }
                    Debug.Log(name + $"goi {neighborTile.name} di chuyen THANH CONG");
                    
                    // nếu di chuyển được, nhưng mình bi chan, mình dung
                    if (neighborTile.stopOther) return false;
                    //if (!neighborTile.IsMoving) return false; // Kiem lai cho chac

                    neighborTile.SetFollower(this);
                }
                else
                {
                    return false; // Nó đang đi hoặc không cho đẩy -> Mình dừng
                }
            }
            else if (hit.collider.TryGetComponent(out Tile wall))
            {
                if (wall.isStopTile) return false;
            }
            else
            {
                return false; // Gặp tường cứng -> Return ngay lập tức, KHÔNG set IsMoving = true
            }
        }
        
        Node customTarget = null;
        if (OnCalculateTargetNode != null)
        {
            customTarget = OnCalculateTargetNode.Invoke(dir);
        }

        if (customTarget != null)
        {
            if (customTarget.x == currNode.x && customTarget.y == currNode.y) return false;
            return Move(customTarget);
        }
        else
        {
            SetDestination(dirVec);
            IsMoving = true;
            MoveDir = dir;
            return true;
        }
    }

    public bool Move(Node target)
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
            return true;
            //OnTravelTileMove?.Invoke(MoveDir);
        }

        return false;
    }
    
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
    
    // Trong TravelTile.cs

    public bool CheckOnGround()
    {
        // 1. Xác định vị trí tâm của ô vuông bên dưới
        Vector2 checkPos = (Vector2)transform.position + Vector2.down;

        // 2. Kiểm tra xem tại điểm đó có ai thuộc wallLayer đang đứng không
        // Bán kính 0.2f là đủ nhỏ để nằm gọn trong ô, không liếm sang ô bên cạnh
        bool hasGround = Physics2D.OverlapCircle(checkPos, 0.2f, wallLayer) != null;

        return hasGround;
    }


    public void SetFollower(TravelTile follower)
    {
        _followerTile = follower;
    }

    private void SetDestination(Vector2 direction)
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
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsMoving) return; // Chỉ người đang đi mới xử lý

        if (other.TryGetComponent(out TravelTile staticTravelTile))
        {
            // Chỉ tương tác nếu thẳng hàng và vật kia đứng yên
            if (SameRowOrSameCol(staticTravelTile) && !staticTravelTile.IsMoving)
            {
                Controller_Sound.Play("Hit", true);
                if (staticTravelTile.canMoveByOthers)//Day
                {
                    if (staticTravelTile.sameSpeedWithOther)
                    {
                        staticTravelTile.moveSpeed = this.moveSpeed;
                    }

                    if (staticTravelTile.Move(this.MoveDir))
                    {
                        if (staticTravelTile.stopOther) //Dung lai hoac di tiep
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
                    else
                    {
                        staticTravelTile.Stop();
                        if (staticTravelTile.isStopTile) Stop();
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