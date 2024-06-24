using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasFinal : MonoBehaviour
{
    public void exitGame()
    {
        Application.Quit();
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
