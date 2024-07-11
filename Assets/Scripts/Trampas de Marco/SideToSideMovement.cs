using UnityEngine;

public class SideToSideAndForwardMovement : MonoBehaviour
{
    public float sideToSideSpeed = 5f;

    public float sideToSideRange = 3f;

    public float forwardSpeed = 2f;

    private Vector3 startPosition;

    private float time;

    private bool isRotating = false;
    private int playersInScene = 0;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {

        CountPlayers();

        if (isRotating)
        {
            time += Time.deltaTime;

            float newX = startPosition.x + Mathf.Sin(time * sideToSideSpeed) * sideToSideRange;

            float newZ = startPosition.z + time * forwardSpeed;

            transform.position = new Vector3(newX, transform.position.y, newZ);
        }

    }

    private void CountPlayers()
    {
        var players = FindObjectsOfType<PlayerHostMovement>();
        playersInScene = players.Length;

        if (playersInScene >= 2)
        {
            StartRotating();
        }
        else
        {
            StopRotating();
        }
    }

    public void OnPlayerCountChanged()
    {
        CountPlayers();
    }

    private void StartRotating()
    {
        isRotating = true;
    }

    private void StopRotating()
    {
        isRotating = false;
    }
}