using UnityEngine;

public class Effect : MonoBehaviour
{
    public void OnFinishAnimation()
    {
        Destroy(gameObject);
    }
}
