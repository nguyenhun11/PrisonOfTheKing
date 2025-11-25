using System;
using UnityEngine;

public class Key : MonoBehaviour
{
    public delegate void KeyEvent();
    public event KeyEvent OnKeyClaimed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnKeyClaimed?.Invoke();
            Destroy(this.gameObject);
        }
    }
}
