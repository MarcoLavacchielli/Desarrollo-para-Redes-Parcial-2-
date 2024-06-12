using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerInputs : MonoBehaviour
{
    private NetworkInputData _inputData;

    private bool _isJumpPressed;
    private bool _isFirePressed;
    private bool _isCrouchPressed;
    private bool _isStandPressed;
    private bool _isSprintPressed;

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

        if (Input.GetKeyDown(KeyCode.C))
        {
            _isCrouchPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            _isStandPressed = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isSprintPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isStandPressed = true;
        }

    }

    public NetworkInputData GetLocalInputs()
    {
        _inputData.isJumpPressed = _isJumpPressed;
        _inputData.isFirePressed = _isFirePressed;
        _inputData.isCrouchPressed = _isCrouchPressed;
        _inputData.isStandPressed = _isStandPressed;
        _inputData.isSprintPressed = _isSprintPressed;

        _isJumpPressed = _isFirePressed = _isCrouchPressed = _isStandPressed = _isSprintPressed = false;

        return _inputData;
    }
}
