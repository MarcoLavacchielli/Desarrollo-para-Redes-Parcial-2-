using Fusion.Sockets;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{

    //runner.SessionInfo.PlayerCount contador propio de fusion para contar cantidad de jugadores. Si lo van a agregar a una coleccion de elementos, asegurense de restar 1 valor al count.

    [SerializeField] NetworkHostPlayer _playerPrefab;
    LocalPlayerInputs _playerInputs;

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, player);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        //darle inputs al jugador local

        if (!NetworkHostPlayer.Local) return;

        if (!_playerInputs) _playerInputs = NetworkHostPlayer.Local.GetComponent<LocalPlayerInputs>();
        else input.Set(_playerInputs.GetLocalInputs());
    }

    public void OnDisconnectedFromServer(NetworkRunner runner) 
    {
        //Activarian el canvas de derrota al jugador 2
        runner.Shutdown();
    }


    #region Callbacks sin usar
    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }


    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    #endregion
}

