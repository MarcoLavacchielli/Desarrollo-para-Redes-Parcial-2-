using UnityEngine;

public class RotatingLog : MonoBehaviour
{
    public float rotationSpeed = 10f;

    void Update()
    {
        transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
    }
}