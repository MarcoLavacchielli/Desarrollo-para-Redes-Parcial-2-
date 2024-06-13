using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using UnityEngine.SceneManagement;

public class DespawnObject : NetworkBehaviour
{
    public NetworkObject wallObj;
    private int playersInside = 0;
    private NetworkBool countingDown = false;
    private float countdownTimer = 3f;
    public TMP_Text countdownText;

    private NetworkBool objDestroyed = false;

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
                DespawnWalls();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("A");

            playersInside++;

            if (playersInside >= 2)
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

            if (playersInside < 2)
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
            objDestroyed = true;
            Runner.Despawn(wallObj);
        }
    }
}