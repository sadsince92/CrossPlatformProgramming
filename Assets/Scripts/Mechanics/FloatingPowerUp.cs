using UnityEngine;

public class FloatingPowerUp : MonoBehaviour
{
    public float amplitude = 0.25f;
    public float frequency = 2f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;

        transform.position = new Vector3(
            startPosition.x,
            newY,
            startPosition.z
        );
    }
}