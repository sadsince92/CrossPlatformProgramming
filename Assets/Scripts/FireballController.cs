using UnityEngine;

public class FireballController : MonoBehaviour
{
    private bool hasBeenVisible = false;

    void OnBecameVisible()
    {
        hasBeenVisible = true;
    }

    void OnBecameInvisible()
    {
        if (hasBeenVisible)
        {
            Destroy(gameObject);
        }
    }
}