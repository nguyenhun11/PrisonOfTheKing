using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private TravelTile _travelTile;

    private void Start()
    {
        _travelTile = GetComponent<TravelTile>();
        _travelTile.OnTravelTileStop += Stop;
    }
    
    public bool StandOnDown;
    public bool StandOnLeft;
    public bool StandOnRight;
    public bool StandOnUp;
    
    public void Fall()
    {
        Debug.Log("Fall");
        _travelTile.Move(Tile.DIR.DOWN);
    }

    public void Die()
    {
        Debug.Log("Die");
        Destroy(gameObject);
    }

    public void Stop()
    {
        Controller_Sound.Play("Stop");
    }
    
}
