using UnityEngine;

public class TravelTile : Tile
{
    public Vector2 targetPosition;
    public float moveSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Kiểm tra đã đến nơi chưa
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            SpapToGrid(); // Snap thẳng vào vị tri
            currPosition = targetPosition;
        }
    }

    private void SetTargetPosition()
    {
        
    }
    
}
