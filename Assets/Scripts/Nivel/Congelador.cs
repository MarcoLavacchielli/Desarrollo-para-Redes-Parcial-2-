using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Congelador : MonoBehaviour
{
    private PlayerHostMovement playerMovement;

    public float speedMultiplier = 0.5f; 

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement = other.GetComponent<PlayerHostMovement>();

            if (playerMovement != null)
            {
                playerMovement.SetSpeedMultiplier(speedMultiplier);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerMovement != null)
            {
                playerMovement.ResetSpeedMultiplier();
            }
        }
    }
}
