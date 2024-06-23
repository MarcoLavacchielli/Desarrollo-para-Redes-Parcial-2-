using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;

public class NetworkHostHandler : MonoBehaviour, INetworkRunnerCallbacks
{

    [SerializeField] NetworkRunner _runnerPrefab;
    NetworkRunner _actualRunner;

    public event Action OnLobbyJoined = delegate { };
    public event Action<List<Fusion.SessionInfo>> OnSessionListUpdate = delegate { };

    #region Lobby
    public void JoinLobby()
    {
        if (_actualRunner) Destroy(_actualRunner.gameObject);

        _actualRunner = Instantiate(_runnerPrefab);

        _actualRunner.AddCallbacks(this);

        var clientTask = JoinLobbyTask();
    }

    async Task JoinLobbyTask()
    {
        var result = await _actualRunner.JoinSessionLobby(SessionLobby.Custom, "Normal Lobby");

        if(result.Ok)
        {
            OnLobbyJoined();
        }
        else
        {
            //ERROR
        }
    }
    #endregion

    #region Create/Join Game
    public void CreateGame(string sessionName, string sceneName)
    {
        var clienTask = InitializeGame(GameMode.Host, sessionName, SceneUtility.GetBuildIndexByScenePath($"Scenes/{sceneName}"));
    }

    public void JoinGame(Fusion.SessionInfo session)
    {
        var clienTask = InitializeGame(GameMode.Client, session.Name);
    }

    async Task InitializeGame(GameMode gameMode, string sessionName, SceneRef? sceneToLoad = null)
    {
        var sceneManager = _actualRunner.GetComponent<NetworkSceneManagerDefault>();

        _actualRunner.ProvideInput = true;

        var result = await _actualRunner.StartGame(new StartGameArgs()
        {
            GameMode = gameMode,    
            SessionName = sessionName,
            Scene = sceneToLoad,
            CustomLobbyName = "Normal Lobby",
            SceneManager = sceneManager
        });

        if(result.Ok) 
        {
            Debug.Log("Game Created/Joined");
        }
        else
        {
            Debug.Log("Error");
        }
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<Fusion.SessionInfo> sessionList)
    {
        OnSessionListUpdate(sessionList);
    }
    #endregion

    #region callbacks sin usar
    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnDisconnectedFromServer(NetworkRunner runner) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    #endregion

}
