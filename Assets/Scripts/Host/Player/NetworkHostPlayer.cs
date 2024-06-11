using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

public class NetworkHostPlayer : NetworkBehaviour
{
    public static NetworkHostPlayer Local;

    NickNameItem _myNickName;

    [Networked(OnChanged = nameof(OnNickNameChanged))]
    private string NickName { get; set; }

    public event Action OnPlayerDespawn = delegate { };


    public override void Spawned()
    {
        //_myNickName = NickNameHandler.Instance.CreateNewNickName(this); //Comento esto porque si no tira error

        if (Object.HasInputAuthority)
        {
            Local = this;

            RPC_SetNewNickName(PlayerPrefs.GetString("NickName"));

            GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.green;
        }
        else
            GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.red;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_SetNewNickName(string newNick)
    {
        NickName = newNick;
    }

    static void OnNickNameChanged(Changed<NetworkHostPlayer> changed)
    {
        var behaviour = changed.Behaviour;
        behaviour._myNickName.UpdateNickName(behaviour.NickName);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnPlayerDespawn();
    }
}
