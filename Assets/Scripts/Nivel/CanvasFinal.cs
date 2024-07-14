using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;

public class CanvasFinal : MonoBehaviour
{
    private NetworkRunner networkRunner;

    private void Awake()
    {
        networkRunner = FindObjectOfType<NetworkRunner>();
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void goToMainMenu()
    {
        if (networkRunner != null)
        {
            networkRunner.Shutdown();
        }
        SceneManager.LoadScene("Main Menu");
    }
}
