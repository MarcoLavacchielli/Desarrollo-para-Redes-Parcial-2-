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

    [SerializeField] private bool UnoGano;
    [SerializeField] private bool DosGano;

    //[SerializeField] GameObject victoryScreen, defeatScreen;

    [SerializeField] private bool termino = false;

    public void AddPlayerModel(PlayerHostMovement playerModel)
    {
        if (playerModel.Object.HasStateAuthority)
        {
            playerScreensList.Insert(0, playerModel);
        }
        else
        {
            if (playerScreensList.Count > 0 && playerScreensList[0].Object.HasStateAuthority)
            {
                playerScreensList.Insert(1, playerModel);
            }
            else
            {
                playerScreensList.Insert(0, playerModel);
            }
        }
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
        playerScreensList.Clear();
        foreach (PlayerHostMovement playerModel in playerModelsInScene)
        {
            AddPlayerModel(playerModel);
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
            if (playerScreensList[0].Object.HasInputAuthority)
            {
                playerScreensList[0].Gano();
            }
            else if (playerScreensList[1].Object.HasInputAuthority)
            {
                playerScreensList[1].Perdio();
            }
        }
        else if (DosGano)
        {
            if (playerScreensList[1].Object.HasInputAuthority)
            {
                playerScreensList[1].Gano();
            }
            else if (playerScreensList[0].Object.HasInputAuthority)
            {
                playerScreensList[0].Perdio();
            }
        }
        termino = true;
    }
}
