using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public struct NetworkInputData : INetworkInput
{
    public float xMovement;
    public float yMovement;
    public NetworkBool isJumpPressed;
    public NetworkBool isFirePressed;
}
