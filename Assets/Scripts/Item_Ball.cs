using UnityEngine;

public class Item_Ball : MonoBehaviour
{
    private TravelTile _travelTile;
    public bool breakWhenStop = false;
    

    void Start()
    {
        _travelTile = GetComponent<TravelTile>();
        _travelTile.OnTravelTileStop += Stop;
    }

    private void Stop()
    {
        if(breakWhenStop) Break();
    }
    private void Break()
    {
        Destroy(gameObject);
    }
    
}
