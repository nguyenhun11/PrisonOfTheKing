using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private TravelTile _travelTile;

    private void Start()
    {
        _travelTile = GetComponent<TravelTile>();
        _travelTile.OnTravelTileFall += Fall;
    }
    
    public bool StandOnDown;
    public bool StandOnLeft;
    public bool StandOnRight;
    public bool StandOnUp;
    
    public void Fall()
    {
        Debug.Log("Fall");
    }

    public void Die()
    {
        Debug.Log("Die");
        Destroy(gameObject);
    }
}
