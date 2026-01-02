using System;
using UnityEngine;

// [TỐI ƯU] Nên cân nhắc đổi 'class' thành 'struct' nếu không cần null,
// sẽ nhẹ hơn cho bộ nhớ (Garbage Collector) vì Node chỉ chứa dữ liệu nhỏ.
public class Node : IEquatable<Node> 
{
    public int x;
    public int y;

    // --- SỬA LỖI ---
    // Constructor cũ của bạn quên gán this.y
    public Node(float x, float y)
    {
        this.x = Mathf.FloorToInt(x);
        this.y = Mathf.FloorToInt(y); // Đã thêm dòng này
    }

    public Node(int x, int y) // Thêm constructor int cho tiện
    {
        this.x = x;
        this.y = y;
    }

    public Node(Vector3 position)
    {
        this.x = Mathf.FloorToInt(position.x);
        this.y = Mathf.FloorToInt(position.y);
    }

    public void SetToNode(Tile tile)
    {
        tile.transform.position = Position();
    }

    public Vector3 Position(float z = 0f)
    {
        return new Vector3(this.x + 0.5f, this.y  + 0.5f, z);
    }

    public Node GetNode(Tile.DIR dir)
    {
        switch (dir)
        {
            case Tile.DIR.UP: return new Node(x, y + 1);
            case Tile.DIR.DOWN: return new Node(x, y - 1);
            case Tile.DIR.LEFT: return new Node(x - 1, y);
            case Tile.DIR.RIGHT: return new Node(x + 1, y);
            default: return null;
        }
    }

    public override string ToString()
    {
        return $"[{x}, {y}]";
    }

    // --- PHẦN QUAN TRỌNG ĐỂ SO SÁNH == ---

    // 1. Hàm so sánh logic
    public bool Equals(Node other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return x == other.x && y == other.y;
    }

    // 2. Ghi đè Equals của object (cần thiết cho các collection như List.Contains)
    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Node)obj);
    }

    // 3. Ghi đè GetHashCode (Bắt buộc khi override Equals, dùng cho Dictionary/HashSet)
    public override int GetHashCode()
    {
        // Cách combine hash code đơn giản và hiệu quả
        return HashCode.Combine(x, y); 
    }

    // 4. Overload toán tử ==
    public static bool operator ==(Node a, Node b)
    {
        if (a is null) return b is null;
        return a.Equals(b);
    }

    // 5. Overload toán tử !=
    public static bool operator !=(Node a, Node b)
    {
        return !(a == b);
    }
}