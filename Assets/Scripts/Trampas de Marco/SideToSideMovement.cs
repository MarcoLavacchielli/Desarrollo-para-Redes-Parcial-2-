using UnityEngine;

public class SideToSideAndForwardMovement : MonoBehaviour
{
    public float sideToSideSpeed = 5f;

    public float sideToSideRange = 3f;

    public float forwardSpeed = 2f;

    private Vector3 startPosition;

    private float time;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        time += Time.deltaTime;

        float newX = startPosition.x + Mathf.Sin(time * sideToSideSpeed) * sideToSideRange;

        float newZ = startPosition.z + time * forwardSpeed;

        transform.position = new Vector3(newX, transform.position.y, newZ);
    }
}