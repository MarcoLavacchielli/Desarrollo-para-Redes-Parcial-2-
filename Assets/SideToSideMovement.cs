using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideToSideMovement : MonoBehaviour
{
    public float movementSpeed = 5f;

    public float movementRange = 3f;

    private Vector3 startPosition;

    private float time;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        time += Time.deltaTime;

        float newX = startPosition.x + Mathf.Sin(time * movementSpeed) * movementRange;

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}