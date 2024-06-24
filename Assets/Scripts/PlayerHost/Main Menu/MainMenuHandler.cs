using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using TMPro.EditorUtilities;

public class MainMenuHandler : MonoBehaviour
{
    [Header("NetworkHostHandler"), SerializeField] NetworkHostHandler _networkHostHandler;

    [Space(25),Header("Panels"), SerializeField] GameObject _joinLobbyPanel;
    [SerializeField] GameObject _statusPanel, _sessionBrowserPanel, _hostGamePanel;

    [Space(25), Header("Buttons"), SerializeField] Button _joinLobbyButton;
    [SerializeField] Button  _hostGamePanelButton, _hostGameButton;

    [Space(25), Header("Input Field"),SerializeField] TMP_InputField _sessionNameField;
    [SerializeField] TMP_InputField _nickNameField;

    [Space(25), Header("Text"), SerializeField] TextMeshProUGUI _statusText;
    [SerializeField] string _sceneName;

    public AudioManager audioM;

    private void Awake()
    {
        _joinLobbyPanel.SetActive(true);
        _statusPanel.SetActive(false);
        _sessionBrowserPanel.SetActive(false);
        _hostGamePanel.SetActive(false);
    }

    void Start()
    {
        _joinLobbyButton.onClick.AddListener(JoinLobby);
        _hostGamePanelButton.onClick.AddListener(ShowHostPanel);
        _hostGameButton.onClick.AddListener(HostGame);

        _networkHostHandler.OnLobbyJoined += () =>
        {
            _statusPanel.SetActive(false);
            _sessionBrowserPanel.SetActive(true);
        };
    }

    void JoinLobby()
    {
        _networkHostHandler.JoinLobby();

        PlayerPrefs.SetString("NickName", _nickNameField.text);

        _joinLobbyPanel.SetActive(false);
        _statusPanel.SetActive(true);

        StartCoroutine(WaitingToJoinLobby());
    }

    IEnumerator WaitingToJoinLobby()
    {
        while(_statusPanel.activeInHierarchy)
        {
            _statusText.text = "Cargando Epicidad.";
            yield return new WaitForSeconds(0.2f);
            _statusText.text = "Cargando Epicidad..";
            yield return new WaitForSeconds(0.2f);
            _statusText.text = "Cargando Epicidad...";
            yield return new WaitForSeconds(0.2f);
        }
    }

    void ShowHostPanel()
    {
        _sessionBrowserPanel.SetActive(false);
        _hostGamePanel.SetActive(true);
    }

    void HostGame()
    {
        _networkHostHandler.CreateGame(_sessionNameField.text, _sceneName);
    }

    public void ButtonClicked()
    {
        audioM.PlaySFX(0);
    }
}
