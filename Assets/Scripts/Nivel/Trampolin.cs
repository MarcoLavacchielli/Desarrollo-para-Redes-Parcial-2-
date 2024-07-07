using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampolin : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHostMovement player = other.GetComponent<PlayerHostMovement>();

            if (player != null)
            {
                player.ApplyJumpForce(jumpForce);
            }
        }
    }
}
