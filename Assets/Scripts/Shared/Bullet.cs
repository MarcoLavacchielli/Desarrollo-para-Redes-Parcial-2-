using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private NetworkRigidbody _networkRgbd;
    void Start()
    {
        _networkRgbd.Rigidbody.AddForce(transform.forward * 10f, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object || !Object.HasStateAuthority) return;

        if (other.TryGetComponent(out PlayerModel enemy)) enemy.TakeDamage(25f);

        Runner.Despawn(Object);
    }
}
