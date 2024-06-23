using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostBullet : NetworkRigidbody
{
    TickTimer _expireTimer = TickTimer.None;

    public override void Spawned()
    {
        base.Spawned();

        Rigidbody.AddForce(transform.forward * 10f, ForceMode.VelocityChange);

        if (Object.HasStateAuthority)
            _expireTimer = TickTimer.CreateFromSeconds(Runner, 3f);
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority) return;

        if (_expireTimer.Expired(Runner)) DespawnObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object || !Object.HasStateAuthority) return;

        if (other.TryGetComponent(out LifeHostHandler enemy)) enemy.TakeDamage(25);

        DespawnObject();
    }

    void DespawnObject()
    {
        _expireTimer = TickTimer.None;
        Runner.Despawn(Object);
    }

}
