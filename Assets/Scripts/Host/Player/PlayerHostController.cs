using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(PlayerHostMovement))]
public class PlayerHostController : NetworkBehaviour
{
    PlayerHostMovement _playerHostMovement;
    NetworkInputData _networkInputData;

    Vector3 _direction;

    public override void Spawned()
    {
        _playerHostMovement = GetComponent<PlayerHostMovement>();
    }

    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out _networkInputData)) return;

        _direction = Vector3.forward * _networkInputData.xMovement;
        _playerHostMovement.Move(_direction);

        if(_networkInputData.isJumpPressed)
        {
            _playerHostMovement.Jump();
        }

        if (_networkInputData.isFirePressed)
        {
            
        }
    }
}
