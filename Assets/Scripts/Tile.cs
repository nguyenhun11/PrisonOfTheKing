using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour
{
    public Node currNode;
    
    public bool isStopTile = true;
    
    public enum DIR
    {
        NONE,
        UP,
        DOWN,
        LEFT,
        RIGHT
    };

    public DIR FlipDir(DIR dir)
    {
        if (dir == DIR.UP) return DIR.DOWN;
        if (dir == DIR.DOWN) return DIR.UP;
        if (dir == DIR.LEFT) return DIR.RIGHT;
        if (dir == DIR.RIGHT) return DIR.LEFT;
        else return DIR.NONE;
    }


    void Start()
    {
        SnapToNode();
    }

    public Dictionary<DIR, Vector2> DirToVector2 = new Dictionary<DIR, Vector2>()
    {
        {
            DIR.NONE, Vector2.zero
        },
        {
            DIR.UP, Vector2.up
        },
        {
            DIR.DOWN, Vector2.down
        },
        {
            DIR.LEFT, Vector2.left
        },
        {
            DIR.RIGHT, Vector2.right
        }
    };
    
    public static Tile.DIR VectorToDir(Vector2 vec)
    {
        if (Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
            return vec.x > 0 ? Tile.DIR.RIGHT : Tile.DIR.LEFT;
        return vec.y > 0 ? Tile.DIR.UP : Tile.DIR.DOWN;
    }
    
    public Node SnapToNode()
    {
        Node snapNode = new Node(transform.position);
        snapNode.SetToNode(this);
        currNode = snapNode;
        return snapNode;
    }
    
    public bool SameRowOrSameCol(Tile otherTile)
    {
        Node otherNode = otherTile.currNode;
        return (currNode.x == otherNode.x || currNode.y == otherNode.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out TravelTile travelTile))
        {
            if (isStopTile)
            {
                travelTile.Stop();
            }
        }
    }

}