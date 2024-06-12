using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerInputs : MonoBehaviour
{
    private NetworkInputData _inputData;

    private bool _isJumpPressed;
    private bool _isFirePressed;
    private bool _isCrouchPressed;

    private void Awake()
    {
        _inputData = new NetworkInputData();
    }

    void Update()
    {
        _inputData.xMovement = Input.GetAxis("Horizontal");
        _inputData.yMovement = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space)) _isJumpPressed = true;
        if (Input.GetKeyDown(KeyCode.R)) _isFirePressed = true;
        if (Input.GetKeyDown(KeyCode.LeftControl)) _isCrouchPressed = true;
        if (Input.GetKeyUp(KeyCode.LeftControl)) _isCrouchPressed = false; // Agregado para detectar cuando se suelta la tecla
    }

    public NetworkInputData GetLocalInputs()
    {
        _inputData.isJumpPressed = _isJumpPressed;
        _inputData.isFirePressed = _isFirePressed;
        _inputData.isCrouchPressed = _isCrouchPressed;

        _isJumpPressed = _isFirePressed = false;
        // No restablecer _isCrouchPressed aquí, ya que su estado depende de la tecla presionada.

        return _inputData;
    }
}