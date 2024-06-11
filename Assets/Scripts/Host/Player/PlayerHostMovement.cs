using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHostMovement : NetworkCharacterControllerPrototype
{
    [SerializeField] private NetworkMecanimAnimator _mecanimAnim;

    private NetworkInputData _inputs;

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out _inputs))
        {
            if (_inputs.isCrouchPressed)
            {
                Crouch();
            }
        }
    }

    public override void Spawned()
    {
        base.Spawned();

        GetComponent<LifeHostHandler>().OnRespawn += () => TeleportToPosition(transform.position);
    }

    // Agregar las mecanicas aca

    public void Crouch()
    {
        if (_inputs.isCrouchPressed)
        {
            transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
            //_networkRgbd.Rigidbody.AddForce(Vector3.down * 5f, ForceMode.Impulse);

            //_speed = crouchSpeed;
        }
    }

}
