using UnityEngine;

public class FireballController : MonoBehaviour
{
    public LayerMask groundLayer;
    public float lifetime = 2f;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer.value) != 0)
        {
            Destroy(gameObject);
        }
    }
}