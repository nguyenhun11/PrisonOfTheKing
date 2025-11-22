using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private TravelTile _travelTile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (_travelTile != null)
        {
            _travelTile.OnTravelTileStop += Die;
        }
    }

 

    private void Die()
    {
        Destroy(gameObject);
    }
}