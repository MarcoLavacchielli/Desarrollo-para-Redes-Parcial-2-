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
    public NetworkObject LineaSelf;
    private int playersInside = 0;
    [Networked(OnChanged = nameof(OnCountdownChanged))]
    private NetworkBool countingDown { get; set; }
    [Networked]
    private float countdownTimer { get; set; } = 3f;
    public TMP_Text countdownText;

    [Networked]
    public NetworkBool objDestroyed { get; set; } = false;

    [SerializeField] ParticleSystem destroy;
    [SerializeField] AudioManager audioM;

    private Vector3 escondido = new Vector3(0f, -10f, 0f);

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
                //Destruyo la linea
                //Runner.Despawn(LineaSelf);
                LineaSelf.transform.position = escondido;
            }
        }
    }

    public static void OnCountdownChanged(Changed<LineaDeSalida> changed)
    {
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
