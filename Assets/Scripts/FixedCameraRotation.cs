using UnityEngine;

public class FixedCameraPositionAndRotation : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float mouseSensitivity = 100f; // Sensibilidad del rat�n
    public float pitchMin = -30f; // M�nimo �ngulo de rotaci�n vertical
    public float pitchMax = 60f; // M�ximo �ngulo de rotaci�n vertical

    private Vector3 initialOffset; // Desplazamiento inicial de la c�mara respecto al jugador
    private float pitch = 0f; // �ngulo de rotaci�n vertical
    private float yaw = 0f; // �ngulo de rotaci�n horizontal

    void Start()
    {
        // Guardar el desplazamiento inicial de la c�mara respecto al jugador
        initialOffset = transform.position - player.position;

        // Esconder y bloquear el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        // Obtener el input del rat�n
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Actualizar los �ngulos de rotaci�n basados en el input del rat�n
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax); // Limitar el �ngulo de rotaci�n vertical

        // Calcular la nueva rotaci�n de la c�mara
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Mantener la posici�n de la c�mara sumando el desplazamiento inicial al jugador
        transform.position = player.position + rotation * initialOffset;
        // Aplicar la nueva rotaci�n a la c�mara
        transform.LookAt(player.position);
    }
}
