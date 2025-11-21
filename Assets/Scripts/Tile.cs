using UnityEngine;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour
{
    public Vector2 currPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    protected void SpapToGrid()
    {
        transform.position = new Vector3(Mathf.Round(currPosition.x)-.5f, Mathf.Round(currPosition.y)-.5f, 0);
    }
    
    
}
