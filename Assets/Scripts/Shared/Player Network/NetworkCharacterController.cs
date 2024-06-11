using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SharedMode
{
    public class NetworkCharacterController : MonoBehaviour
    {
        NetworkInputData _networkInputs;

        private bool _isJumpPressed;
        private bool _isFirePressed;


        void Start()
        {
            _networkInputs = new NetworkInputData();
        }

        void Update()
        {
            _networkInputs.xMovement = Input.GetAxis("Horizontal");

            if (Input.GetKeyDown(KeyCode.W))
            {
                _isJumpPressed = true;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                _isFirePressed = true;
            }
        }

        public NetworkInputData GetLocalInputs()
        {
            _networkInputs.isJumpPressed = _isJumpPressed;
            _isJumpPressed = false;

            _networkInputs.isFirePressed = _isFirePressed;
            _isFirePressed = false;

            return _networkInputs;  
        }
    }
}

