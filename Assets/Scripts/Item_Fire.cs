using UnityEngine;

public class Item_Fire : MonoBehaviour
{
    public bool isBlocked = false;
    private Collider2D _collider2D;
    private Tile _tile;
    
    private Animator _animator;
    private static  readonly int IsBlockedParam = Animator.StringToHash("Blocked");
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
        _tile = GetComponent<Tile>();
        CheckBlocked();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool(IsBlockedParam, isBlocked);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckBlocked();
    }

    private void CheckBlocked()
    {
        _collider2D.enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 0.5f);
        _collider2D.enabled = true;
        if (hit.collider == null)
        {
            isBlocked = false;
            return;
        }
        else
        {
            if (hit.collider.TryGetComponent(out PlayerState playerState))
            {
                playerState.Die();
                isBlocked = false;
            }
            else if (hit.collider.TryGetComponent(out Enemy enemy))
            {
                enemy.Die();
                isBlocked = false;
            }
            else
            {
                if (hit.collider.TryGetComponent(out Tile tile))
                {
                    if (tile.currNode.x == _tile.currNode.x && tile.currNode.y == _tile.currNode.y)
                    {
                        isBlocked = true;
                    }
                }
            }
        }
    }
}
