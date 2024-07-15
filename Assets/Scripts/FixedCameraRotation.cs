using UnityEngine;

public class FixedCameraPositionAndRotation : MonoBehaviour
{
    public Transform player; 
    public float mouseSensitivity = 100f; 
    public float limtVertMin = -30f; 
    public float limtVertMax = 60f; 

    private Vector3 initialOffset; 
    private float verticalAngle = 0f;
    private float horizontalAngle = 0f; 

    void Start()
    {
        initialOffset = transform.position - player.position;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        horizontalAngle += mouseX;
        verticalAngle -= mouseY;
        verticalAngle = Mathf.Clamp(verticalAngle, limtVertMin, limtVertMax); 

        Quaternion rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0f);

        transform.position = player.position + rotation * initialOffset;
        transform.LookAt(player.position);
    }
}
