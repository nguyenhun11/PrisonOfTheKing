using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public enum DIR
    {
        None,
        Left,
        Right,
        Up,
        Down
    };
    
    private Rigidbody2D _rb;
    
    private float _speed = 5f;
    private bool _isMoving = false;
    

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(DIR.Down);
        }
    }


    private void Move(DIR dir)
    {
        if (dir == DIR.None) return;
        else
        {
            _isMoving = true;
            if (dir == DIR.Up)
            {
                _rb.linearVelocity = Vector2.up * _speed;
            }
            else if (dir == DIR.Down)
            {
                _rb.linearVelocity = Vector2.down * _speed;
            }
            else if (dir == DIR.Left)
            {
                _rb.linearVelocity = Vector2.left * _speed;
            }
            else if (dir == DIR.Right)
            {
                _rb.linearVelocity = Vector2.right * _speed;
            }
        }
    }
}
