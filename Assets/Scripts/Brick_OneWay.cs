using System;
using TreeEditor;
using UnityEngine;

public class Brick_OneWay : MonoBehaviour
{
    [SerializeField] private Tile.DIR dirCanMove;
    private Animator _animator;
    private Tile _tile;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HandleSpriteRotation();
        _animator = GetComponent<Animator>();
        _tile = GetComponent<Tile>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out TravelTile travelTile))
        {
            Debug.Log(travelTile.MoveDir);
            if (travelTile != null && travelTile.IsMoving && travelTile.MoveDir == dirCanMove)
            {
                _animator.SetTrigger("Open");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out TravelTile travelTile))
        {
            if (travelTile != null 
                && travelTile.IsMoving
                && _tile.SameRowOrSameCol(travelTile)
                && travelTile.MoveDir == travelTile.FlipDir(dirCanMove))
            {
                travelTile.Stop();
            }
        }
    }

    private void HandleSpriteRotation()
    {
        if (dirCanMove == Tile.DIR.NONE)
        {
            dirCanMove = Tile.DIR.UP;
        }
        if (dirCanMove == Tile.DIR.UP)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (dirCanMove == Tile.DIR.DOWN)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else if (dirCanMove == Tile.DIR.RIGHT)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
        else if (dirCanMove == Tile.DIR.LEFT)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
    }
}
