using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHostMovement : NetworkCharacterControllerPrototype
{
    [SerializeField] private NetworkMecanimAnimator _mecanimAnim;

    public override void Spawned()
    {
        base.Spawned();

        GetComponent<LifeHostHandler>().OnRespawn += () => TeleportToPosition(transform.position);
    }
}
