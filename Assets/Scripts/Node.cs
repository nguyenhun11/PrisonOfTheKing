using System;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Node //Vị trí trong game
{
    public int x;
    public int y;

    private Node _upNode;
    private Node _downNode;
    private Node _leftNode;
    private Node _rightNode;

    public Node(float x, float y) //Snap
    {
        this.x = (int)Mathf.FloorToInt(x);
        this.y = (int)Mathf.FloorToInt(y);
    }

    public Node(Vector3 position)
    {
        this.x = (int)Mathf.FloorToInt(position.x);
        this.y = (int)Mathf.FloorToInt(position.y);
    }

    public void SetToNode(Tile tile)
    {
        tile.transform.position = Position();
    }

    public Vector3 Position(float z = 0f)
    {
        return new Vector3(this.x + 0.5f, this.y  + 0.5f, z);
    }

    public Node GetNode(Tile.DIR dir)
    {
        switch (dir)
        {
            case Tile.DIR.UP: return _upNode;
            case Tile.DIR.DOWN: return  _downNode;
            case Tile.DIR.LEFT: return  _leftNode;
            case Tile.DIR.RIGHT: return  _rightNode;
            default: return null;
        }
    }

    public override string ToString()
    {
        return string.Format("[{0}, {1}]", x, y);
    }
}
