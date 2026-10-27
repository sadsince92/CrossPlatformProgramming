using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public float minXPos;

    public float maxXPos;

    public Transform target;

    public float cameraSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Player object not found. Please assign the target manually.");
                return;
            }
            target = player.transform;
        }
    }

   void LateUpdate()
    {
        if (target == null)
        {
            return;
        }
        Vector3 currentPos = transform.position;
        currentPos.x = Mathf.Clamp(target.position.x, minXPos, maxXPos);
        transform.position = Vector3.MoveTowards(transform.position, currentPos, 5 * Time.deltaTime);
    }
}
