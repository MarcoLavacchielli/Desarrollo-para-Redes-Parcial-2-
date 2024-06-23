using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHostGun : NetworkBehaviour
{
    [SerializeField] HostBullet _bulletPrefab;
    [SerializeField] Transform _bulletSpawner;
    [SerializeField] ParticleSystem _shootParticles;

    [SerializeField] float _shootCooldown;

    float _lastShootTime;

    [Networked (OnChanged = nameof(OnFiringChanged))]
    private bool IsFiring { get; set;}

    public void Shoot()
    {
        if (Time.time - _lastShootTime < _shootCooldown) return;

        _lastShootTime = Time.time;

        StartCoroutine(ShootCooldown());

        Runner.Spawn(_bulletPrefab, _bulletSpawner.position, transform.rotation);

        #region RAYCAST OPTION
        /*var raycast = Runner.LagCompensation.Raycast(origin: _bulletSpawner.position,
                                                    direction: _bulletSpawner.forward, 
                                                    length: 100, 
                                                    player: Object.InputAuthority, 
                                                    hit: out var hitInfo);

        if (!raycast) return;

        Debug.Log("Collision info: " + hitInfo.Hitbox.Root.gameObject.name);
        hitInfo.GameObject.GetComponentInParent<LifeHostHandler>()?.TakeDamage(25);*/
        #endregion
        //HelloWorld(number: 1, name: "Carlos", secondNumber: 6f);
    }

    /*void HelloWorld(int number, float secondNumber, string name = "Pepito")
    {

    }*/

    IEnumerator ShootCooldown()
    {
        IsFiring = true;

        yield return new WaitForSeconds(_shootCooldown);

        IsFiring = false;
    }

    static void OnFiringChanged(Changed<PlayerHostGun> changed)
    {
        bool currentFiring = changed.Behaviour.IsFiring;
        changed.LoadOld();
        bool oldFiring = changed.Behaviour.IsFiring;

        if (!oldFiring && currentFiring) changed.Behaviour.TurnOnParticleSystem();
    }

    void TurnOnParticleSystem()
    {
        _shootParticles.Play();
    }
}
