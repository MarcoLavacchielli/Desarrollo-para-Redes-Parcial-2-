using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public Transform[] waypoints;

    public float speed = 2f;

    private int currentWaypointIndex = 0;
    public float waitTime = 1f;
    private float waitCounter = 0f;
    private bool waiting = false;

    private bool isRotating = false;
    private int playersInScene = 0;

    void Update()
    {

        CountPlayers();

        if (isRotating)
        {
            if (waypoints.Length == 0)
                return;

            if (waiting)
            {
                waitCounter += Time.deltaTime;
                if (waitCounter >= waitTime)
                {
                    waiting = false;
                    waitCounter = 0f;
                }
                return;
            }

            Transform targetWaypoint = waypoints[currentWaypointIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                waiting = true;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Gizmos.color = Color.red;
        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.DrawSphere(waypoints[i].position, 0.5f);
            if (i < waypoints.Length - 1)
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
        if (waypoints.Length > 1)
            Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
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
