using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeHostHandler : NetworkBehaviour
{
    private const byte _fullLife = 100;

    [Networked(OnChanged = nameof(OnLifeChanged))]
    private byte CurrentLife { get; set; }

    [SerializeField] byte _lifeAmount = 3;
    [SerializeField] GameObject _visualObject;

    [Networked(OnChanged = nameof(OnDeadChanged))]
    private bool IsDead { get; set; }

    public event Action OnRespawn = delegate { };
    public event Action<bool> OnEnableMyController = delegate { };

    public override void Spawned()
    {
        CurrentLife = _fullLife;
    }

    public void TakeDamage(byte damage)
    {
        if (damage > CurrentLife) damage = CurrentLife;

        CurrentLife -= damage;

        if (CurrentLife != 0) return;

        _lifeAmount--;

        if(_lifeAmount == 0)
        {
            DisconnectInputAuthority();
            return;
        }

        StartCoroutine(RespawningPlayer());
    }

    private void DisconnectInputAuthority()
    {
        if(!Object.HasInputAuthority)
        {
            Runner.Disconnect(Object.InputAuthority);
        }
        else
        {
            //Activarian el canvas de derrota al jugador 1
        }

        Runner.Despawn(Object);
    }

    IEnumerator RespawningPlayer()
    {
        IsDead = true;

        yield return new WaitForSeconds(3f);

        IsDead = false;

        ApplyRespawn();
    }

    private void ApplyRespawn()
    {
        CurrentLife = _fullLife;

        OnRespawn();
    }

    static void OnLifeChanged(Changed<LifeHostHandler> changed)
    {
        //Floating bars
    }

    static void OnDeadChanged(Changed <LifeHostHandler> changed)
    {
        bool currentDead = changed.Behaviour.IsDead;

        changed.LoadOld();

        bool oldDead = changed.Behaviour.IsDead;

        if(currentDead)
        {
            changed.Behaviour.RemoteDeadRespawn(false);
        }
        else if (oldDead)
        {
            changed.Behaviour.RemoteDeadRespawn(true);
        }
    }

    void RemoteDeadRespawn(bool value)
    {
        _visualObject.SetActive(value);

        OnEnableMyController(value);
    }
}
