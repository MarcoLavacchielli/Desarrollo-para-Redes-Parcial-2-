using UnityEngine;

public class FixedCameraPositionAndRotation : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float mouseSensitivity = 100f; // Sensibilidad del ratón
    public float pitchMin = -30f; // Mínimo ángulo de rotación vertical
    public float pitchMax = 60f; // Máximo ángulo de rotación vertical

    private Vector3 initialOffset; // Desplazamiento inicial de la cámara respecto al jugador
    private float pitch = 0f; // Ángulo de rotación vertical
    private float yaw = 0f; // Ángulo de rotación horizontal

    void Start()
    {
        // Guardar el desplazamiento inicial de la cámara respecto al jugador
        initialOffset = transform.position - player.position;

        // Esconder y bloquear el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        // Obtener el input del ratón
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Actualizar los ángulos de rotación basados en el input del ratón
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax); // Limitar el ángulo de rotación vertical

        // Calcular la nueva rotación de la cámara
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Mantener la posición de la cámara sumando el desplazamiento inicial al jugador
        transform.position = player.position + rotation * initialOffset;
        // Aplicar la nueva rotación a la cámara
        transform.LookAt(player.position);
    }
}
