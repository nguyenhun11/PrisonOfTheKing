using System;
using UnityEngine;

// Đảm bảo có Rigidbody2D để xử lý vật lý rơi
[RequireComponent(typeof(Rigidbody2D))] 
public class Enemy : MonoBehaviour
{
    [SerializeField] private TravelTile _travelTile;
    [SerializeField] private Effect _killEffect;
    
    [Header("Death Settings")]
    [SerializeField] private float _jumpForce = 8f;   // Lực nảy lên cao
    [SerializeField] private float _throwForce = 3f;  // Lực bay ngang
    [SerializeField] private float _destroyDelay = 2f;// Thời gian chờ rơi khỏi màn hình rồi mới xóa

    public bool IsDead { get; private set; } // Cờ đánh dấu để tránh hàm Die gọi nhiều lần
    private Rigidbody2D _rb;
    private Collider2D _col;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        if (_travelTile != null)
        {
            _travelTile.OnTravelTileStop += Die;
        }
    }

    private void OnDisable()
    {
        // Luôn nhớ hủy đăng ký sự kiện khi object bị tắt để tránh lỗi
        if (_travelTile != null)
        {
            _travelTile.OnTravelTileStop -= Die;
        }
    }

    public void Die()
    {
        // Nếu đã chết rồi thì không làm gì nữa (tránh spam hiệu ứng)
        if (IsDead) return;
        IsDead = true;

        // 1. Xử lý logic cũ (sự kiện & hiệu ứng nổ)
        if (_travelTile != null)
        {
            _travelTile.OnTravelTileStop -= Die;
            _travelTile.enabled = false; // QUAN TRỌNG: Tắt script di chuyển để Physics hoạt động
        }

        if (_killEffect != null)
        {
            Instantiate(_killEffect, transform.position, transform.rotation);
        }

        // 2. Tắt va chạm để Enemy rơi xuyên qua sàn/người chơi
        if (_col != null)
        {
            _col.enabled = false;
        }

        // 3. Xử lý Vật lý: Nhảy lên và rơi xuống (Parabol)
        if (_rb != null)
        {
            // Đảm bảo Rigidbody hoạt động (phòng trường hợp enemy để Kinematic)
            _rb.bodyType = RigidbodyType2D.Dynamic; 
            _rb.gravityScale = 2.5f; // Tăng trọng lực xíu cho rơi nhanh, dứt khoát

            // Random hướng bay ngang (trái hoặc phải)
            float randomDirection = UnityEngine.Random.Range(-1f, 1f) > 0 ? 1 : -1;
            
            // Tạo vector lực: Lên trên + Sang ngang
            Vector2 velocity = new Vector2(randomDirection * _throwForce, _jumpForce);
            _rb.linearVelocity = velocity;
            
            // Thêm chút xoay vòng cho sinh động (Optional)
            _rb.angularVelocity = randomDirection * -360f; 
        }

        // 4. Hủy object sau khi đã rơi khỏi màn hình (ví dụ 2 giây)
        Destroy(gameObject, _destroyDelay);
    }
}