using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _sessionName, _playerAmount;
    [SerializeField] Button _joinButton;

    public void SetInfo(SessionInfo session, Action<SessionInfo> onClick)
    {
        _sessionName.text = session.Name;
        _playerAmount.text = $"{session.PlayerCount}/{session.MaxPlayers}";

        _joinButton.enabled = session.PlayerCount < session.MaxPlayers;

        _joinButton.onClick.AddListener(() => onClick(session));
    }
}
