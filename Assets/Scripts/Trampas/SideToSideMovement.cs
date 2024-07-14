using UnityEngine;

public class SideToSideAndForwardMovement : MonoBehaviour
{
    public float sideToSideSpeed = 5f;

    public float sideToSideRange = 3f;

    public float forwardSpeed = 2f;

    private Vector3 startPosition;

    private float time;

    [SerializeField] private LineaDeSalida lineaDeSalida;

    void Start()
    {
        startPosition = transform.position;
        lineaDeSalida = FindObjectOfType<LineaDeSalida>();
    }

    void Update()
    {
        if (lineaDeSalida != null && lineaDeSalida.objDestroyed)
        {
            time += Time.deltaTime;

            float newX = startPosition.x + Mathf.Sin(time * sideToSideSpeed) * sideToSideRange;

            float newZ = startPosition.z + time * forwardSpeed;

            transform.position = new Vector3(newX, transform.position.y, newZ);
        }

    }
}