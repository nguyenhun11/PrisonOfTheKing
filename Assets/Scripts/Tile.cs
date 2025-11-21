using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour
{
    public Node currNode;
    
    public enum DIR
    {
        NONE,
        UP,
        DOWN,
        LEFT,
        RIGHT
    };

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
    
    protected void SnapToNode()
    {
        Node snapNode = new Node(transform.position);
        snapNode.SetToNode(this);
    }
}
