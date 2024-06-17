using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using UnityEngine.SceneManagement;

public class LineaDeSalida : NetworkBehaviour
{
    public NetworkObject wallObj;
    private int playersInside = 0;
    [Networked(OnChanged = nameof(OnCountdownChanged))]
    private NetworkBool countingDown { get; set; }
    [Networked]
    private float countdownTimer { get; set; } = 3f;
    public TMP_Text countdownText;

    private NetworkBool objDestroyed = false;

    [SerializeField] ParticleSystem destroy;
    [SerializeField] AudioManager audioM;

    private void Update()
    {
        if (countingDown && !objDestroyed)
        {
            countdownTimer -= Time.deltaTime;
            int secondsLeft = Mathf.CeilToInt(countdownTimer);
            countdownText.text = secondsLeft.ToString();

            if (countdownTimer <= 0)
            {
                countdownText.text = "";
                countingDown = false;

                destroy.Play();
                audioM.PlaySFX(5);

                DespawnWalls();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInside++;

            if (playersInside >= 2 && Object.HasStateAuthority)
            {
                countingDown = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInside--;

            if (playersInside < 2 && Object.HasStateAuthority)
            {
                countdownTimer = 3f;
                countingDown = false;
                countdownText.text = "";
            }
        }
    }

    private void DespawnWalls()
    {
        GameObject[] lobbyWalls = GameObject.FindGameObjectsWithTag("LobbyWall");
        foreach (GameObject wall in lobbyWalls)
        {
            if (Object.HasStateAuthority)
            {
                objDestroyed = true;
                Runner.Despawn(wallObj);
            }
        }
    }

    public static void OnCountdownChanged(Changed<LineaDeSalida> changed)
    {
        // Cuando el estado de countingDown cambia, sincronizamos el texto para todos los clientes
        changed.Behaviour.SyncCountdownText();
    }

    private void SyncCountdownText()
    {
        if (!countingDown)
        {
            countdownText.text = "";
            return;
        }

        int secondsLeft = Mathf.CeilToInt(countdownTimer);
        countdownText.text = secondsLeft.ToString();
    }
}