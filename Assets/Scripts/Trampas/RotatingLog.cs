using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingLog : NetworkBehaviour
{
    public float rotationSpeed = 10f;
    private LineaDeSalida lineaDeSalida;

    private void Start()
    {
        lineaDeSalida = FindObjectOfType<LineaDeSalida>();
    }

    private void Update()
    {
        if (lineaDeSalida != null && lineaDeSalida.objDestroyed)
        {
            transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
        }
    }
}
