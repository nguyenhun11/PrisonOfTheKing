using UnityEngine;

public class Brick_MoveOneNode : MonoBehaviour
{
    public Road_Node roadMap;
    private TravelTile _travelTile;

    void Start()
    {
        _travelTile = GetComponent<TravelTile>();
        _travelTile.OnCalculateTargetNode = GetOneNodeTarget;
    }

    private Node GetOneNodeTarget(Tile.DIR dir)
    {
        Node nextNode = _travelTile.currNode.GetNode(dir);
        
        if (roadMap != null && roadMap.CheckNode(nextNode))
        {
            return nextNode;
        }
        
        // Trả về currNode để báo hiệu: "Tôi muốn đứng yên tại chỗ"
        // TravelTile sẽ nhận diện điều này và hủy lệnh di chuyển.
        return _travelTile.currNode; 
    }
    
    void OnDestroy()
    {
        if (_travelTile != null) _travelTile.OnCalculateTargetNode = null;
    }
}