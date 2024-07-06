using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampolin : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f; // Ajusta la fuerza del salto según sea necesario

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que colisiona es un jugador
        if (other.CompareTag("Player"))
        {

            // Obtener el componente CharacterController o NetworkCharacterControllerPrototype del jugador
            PlayerHostMovement player = other.GetComponent<PlayerHostMovement>();

            if (player != null)
            {
                // Aplicar fuerza de salto al jugador
                player.ApplyJumpForce(jumpForce);
            }
        }
    }
}
