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
        return _travelTile.currNode; 
    }
    
    void OnDestroy()
    {
        if (_travelTile != null) _travelTile.OnCalculateTargetNode = null;
    }
}