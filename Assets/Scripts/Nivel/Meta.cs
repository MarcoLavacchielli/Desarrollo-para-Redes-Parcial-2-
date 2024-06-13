using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meta : NetworkBehaviour
{
    [SerializeField]
    private List<PlayerHostMovement> playerScreensList = new List<PlayerHostMovement>();
    [SerializeField]
    private PlayerHostMovement firstPlayerModel;

    [SerializeField] private NetworkBool UnoGano;
    [SerializeField] private NetworkBool DosGano;

    //[SerializeField] GameObject victoryScreen, defeatScreen;

    [SerializeField] private NetworkBool termino = false;


    public void AddPlayerModel(PlayerHostMovement playerModel)
    {
        playerScreensList.Add(playerModel);
    }

    private void Update()
    {

        if (termino == false)
        {
            listManagement();
        }
        if (termino == true)
        {
            playerScreensList = null;
        }

    }

    void listManagement()
    {
        PlayerHostMovement[] playerModelsInScene = FindObjectsOfType<PlayerHostMovement>();
        foreach (PlayerHostMovement playerModel in playerModelsInScene)
        {
            if (!playerScreensList.Contains(playerModel))
            {
                AddPlayerModel(playerModel);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //firstPlayerModel = playerScreensList[0];

        if (other.GetComponent<PlayerHostMovement>() == playerScreensList[0])
        {
            UnoGano = true;
            DosGano = false;
            RpcDeclareVictory();
        }
        else if (other.GetComponent<PlayerHostMovement>() == playerScreensList[1])
        {
            UnoGano = false;
            DosGano = true;
            RpcDeclareVictory();
        }
    }

    void RpcDeclareVictory()
    {
        if (UnoGano)
        {
            playerScreensList[0].Gano();
            playerScreensList[1].Perdio();
            termino = true;
        }
        else if (DosGano)
        {
            playerScreensList[1].Gano();
            playerScreensList[0].Perdio();
            termino = true;
        }
    }
}