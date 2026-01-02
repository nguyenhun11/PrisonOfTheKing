using System.Collections.Generic;
using UnityEngine;

public class Road_Node : MonoBehaviour
{
    // SỬA: Dùng Vector2Int thay vì string
    private HashSet<Vector2Int> roadCoordinates = new HashSet<Vector2Int>();
    
    void Awake()
    {
        var tiles = GetComponentsInChildren<Tile>();
        foreach (Tile tile in tiles)
        {
            if(tile.currNode == null) tile.SnapToNode();
            
            // Lưu tọa độ số
            roadCoordinates.Add(new Vector2Int(tile.currNode.x, tile.currNode.y));
        }
    }

    public bool CheckNode(Node node)
    {
        if (node == null) return false;
        // Check bằng số -> Siêu nhanh, không gây lỗi bộ nhớ
        return roadCoordinates.Contains(new Vector2Int(node.x, node.y));
    }
}