using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion.Sockets;
using Fusion;
using System;

public class CanvasFinal : MonoBehaviour
{
    public void exitGame(NetworkRunner runner)
    {
        Application.Quit();
        runner.Shutdown();
    }

    public void goToMainMenu(NetworkRunner runner)
    {
        SceneManager.LoadScene("Main Menu");
        runner.Shutdown();
    }
}
