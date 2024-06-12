using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHostMovement : NetworkCharacterControllerPrototype
{
    [SerializeField] private NetworkMecanimAnimator _mecanimAnim;

    private NetworkInputData _inputs;

    public float crouchSpeed = 1.0f;
    public bool IsCrouching;
    private Vector3 _originalScale;
    private float _originalSpeed;

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out _inputs))
        {
            if (_inputs.isCrouchPressed)
            {
                Crouch();
            }
            if (_inputs.isStandPressed)
            {
                Stand();
            }
        }
    }

    public override void Spawned()
    {
        base.Spawned();

        GetComponent<LifeHostHandler>().OnRespawn += () => TeleportToPosition(transform.position);
    }

    protected override void Awake()
    {
        base.Awake();
        _originalScale = transform.localScale;
        _originalSpeed = maxSpeed;  // Aquí asignamos el valor de maxSpeed a _originalSpeed
    }

    // Agregar las mecanicas aca

    public void Crouch()
    {
        transform.localScale = new Vector3(_originalScale.x, 0.5f, _originalScale.z);
        Velocity += Vector3.down * 5f;
        maxSpeed = crouchSpeed;
        IsCrouching = true;
    }

    public void Stand()
    {
        transform.localScale = _originalScale; // Restablece la escala original
        maxSpeed = _originalSpeed; // Restablece la velocidad original
        IsCrouching = false;
    }

}
