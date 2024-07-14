using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CollisionToggle : NetworkBehaviour
{
    public NetworkObject[] pair1 = new NetworkObject[2];
    public NetworkObject[] pair2 = new NetworkObject[2];
    public NetworkObject[] pair3 = new NetworkObject[2];
    public NetworkObject[] pair4 = new NetworkObject[2];
    public NetworkObject[] pair5 = new NetworkObject[2];

    void Start()
    {
        if (Object.HasStateAuthority)
        {
            ToggleCollision(pair1);
            ToggleCollision(pair2);
            ToggleCollision(pair3);
            ToggleCollision(pair4);
            ToggleCollision(pair5);
        }
    }

    void ToggleCollision(NetworkObject[] pair)
    {
        if (pair.Length != 2) return;

        Collider collider1 = pair[0].GetComponent<Collider>();
        Collider collider2 = pair[1].GetComponent<Collider>();

        if (collider1 == null || collider2 == null)
        {
            Debug.LogError("Uno o ambos NetworkObjects no tienen Collider.");
            return;
        }

        collider1.isTrigger = false;
        collider2.isTrigger = false;

        int randomIndex = Random.Range(0, 2);

        if (randomIndex == 0)
        {
            collider1.isTrigger = true;
        }
        else
        {
            collider2.isTrigger = true;
        }
    }
}
