using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private TravelTile _travelTile;
    private Collider2D _collider2D;
    private Rigidbody2D _rigidbody2D;
    

    public bool IsMoving
    {
        get { return _travelTile.IsMoving; }
    }

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
        Tile.DIR inputThisFrame = GetDirInput();

        if (IsMoving)
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
                _bufferDirWhenMoving = Tile.DIR.NONE; // Reset buffer sau khi dùng
            }
            else if (inputThisFrame != Tile.DIR.NONE)
            {
                MoveCharacter(inputThisFrame);
            }
        }
    }

    private void MoveCharacter(Tile.DIR direction)
    {
        _travelTile.Move(direction);
        //Debug.Log("PM: " + direction.ToString());
        OnPlayerMoved?.Invoke(direction);
    }

    private Tile.DIR GetDirInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            return Tile.DIR.UP;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            return  Tile.DIR.DOWN;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            return Tile.DIR.LEFT;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            return  Tile.DIR.RIGHT;
        }
        else 
        {
            return Tile.DIR.NONE;
        }
    }
}
