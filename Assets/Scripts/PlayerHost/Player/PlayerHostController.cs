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

    void Awake()
    {
        _playerHostMovement = GetComponent<PlayerHostMovement>();
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

        _direction = new Vector3(_networkInputData.xMovement, 0, _networkInputData.yMovement);
        _playerHostMovement.Move(_direction);

        if(_networkInputData.isJumpPressed)
        {
            _playerHostMovement.Jump();
        }

        if (_networkInputData.isCrouchPressed)
        {
            _playerHostMovement.Crouch();
        }

        if (_networkInputData.isStandPressed)
        {
            _playerHostMovement.Stand();
        }

        if (_networkInputData.isSprintPressed)
        {
            _playerHostMovement.Sprint();
        }

        if (_networkInputData.isSlidePressed)
        {
            _playerHostMovement.StartSliding();
        }

        if (_networkInputData.isAttackPressed)
        {
            _playerHostMovement.Attack();
        }

    }
}
