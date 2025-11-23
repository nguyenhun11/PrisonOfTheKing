using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private TravelTile _travelTile;

    private void OnEnable()
    {
        if (_travelTile != null)
        {
            _travelTile.OnTravelTileStop += Die;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}