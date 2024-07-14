using UnityEngine;

public class RotatingLog : MonoBehaviour
{
    public float rotationSpeed = 10f;
    [SerializeField] private LineaDeSalida lineaDeSalida;

    private void Start()
    {
        lineaDeSalida = FindObjectOfType<LineaDeSalida>();
    }

    private void Update()
    {

        if(lineaDeSalida != null && lineaDeSalida.objDestroyed)
        {
            transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
        }
    }
}
