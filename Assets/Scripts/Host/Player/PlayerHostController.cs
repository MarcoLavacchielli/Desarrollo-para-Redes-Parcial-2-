using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(PlayerHostMovement))]
[RequireComponent(typeof(PlayerHostGun))]
[RequireComponent(typeof(LifeHostHandler))]
public class PlayerHostController : NetworkBehaviour
{
    PlayerHostMovement _playerHostMovement;
    PlayerHostGun _playerHostGun;
    NetworkInputData _networkInputData;

    Vector3 _direction;

    void Awake()
    {
        _playerHostMovement = GetComponent<PlayerHostMovement>();
        _playerHostGun = GetComponent<PlayerHostGun>();
        GetComponent<LifeHostHandler>().OnEnableMyController += 
            (controller) => enabled = controller;
    }

    private void OnEnable()
    {
        if (!_playerHostMovement.Controller) return;
        _playerHostMovement.Controller.enabled = true;
    }

    private void OnDisable()
    {
        if (!_playerHostMovement.Controller) return;
        _playerHostMovement.Controller.enabled = false;
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
            _playerHostGun.Shoot();
        }
    }
}
