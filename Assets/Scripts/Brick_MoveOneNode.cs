using UnityEngine;

public class Brick_MoveOneNode : MonoBehaviour
{
    public Road_Node roadMap; // Class quản lý bản đồ (Wall, Floor)
    private TravelTile _travelTile;

    void Start()
    {
        _travelTile = GetComponent<TravelTile>();
        // Gán hàm tính toán đích cho TravelTile
        _travelTile.OnCalculateTargetNode = GetOneNodeTarget;
    }

    private Node GetOneNodeTarget(Tile.DIR dir)
    {
        // Lấy node kế tiếp theo hướng
        Node nextNode = _travelTile.currNode.GetNode(dir);
        
        // Kiểm tra logic Game: Node đó có phải là đường đi (Floor) và không có Wall?
        // Lưu ý: Raycast của TravelTile đã check vật cản động (Brick khác), 
        // nhưng hàm này check vật cản tĩnh (RoadMap/Floor type)
        if (roadMap != null && roadMap.CheckNode(nextNode))
        {
            return nextNode;
        }
        
        // Trả về chính nó báo hiệu "Tao không đi được đâu"
        return _travelTile.currNode; 
    }

    void OnDestroy()
    {
        if (_travelTile != null) _travelTile.OnCalculateTargetNode = null;
    }
}