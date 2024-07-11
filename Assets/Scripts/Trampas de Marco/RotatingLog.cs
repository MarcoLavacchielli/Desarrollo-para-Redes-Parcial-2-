using UnityEngine;

public class RotatingLog : MonoBehaviour
{
    public float rotationSpeed = 10f;
    private int playersInScene = 0;
    private bool isRotating = false;

    private void Update()
    {
        CountPlayers();

        if (isRotating)
        {
            transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
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
