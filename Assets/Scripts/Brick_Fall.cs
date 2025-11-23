using UnityEngine;

public class Brick_Fall : MonoBehaviour
{
    private Tile _tile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tile = GetComponent<Tile>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out TravelTile travelTile))
        {
            if (travelTile.IsMoving && _tile.SameRowOrSameCol(travelTile))
            { 
                if (travelTile.MoveDir == Tile.DIR.DOWN)
                {
                    travelTile.Stop();
                }
                else
                {
                    travelTile.Stop();
                    if (collision.TryGetComponent(out PlayerState playerState)) playerState.Fall();
                }
            }
        }
    }
}
