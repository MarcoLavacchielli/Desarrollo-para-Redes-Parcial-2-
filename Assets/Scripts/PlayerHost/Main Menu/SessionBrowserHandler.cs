using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionBrowserHandler : MonoBehaviour
{
    [SerializeField] NetworkHostHandler _networkHostHandler;
    [SerializeField] GameObject _statusTextObject;
    [SerializeField] SessionItem _itemPrefab;
    [SerializeField] VerticalLayoutGroup _parent;

    void OnEnable()
    {
        _networkHostHandler.OnSessionListUpdate += ReceiveSessionList;
    }


    void OnDisable()
    {
        _networkHostHandler.OnSessionListUpdate -= ReceiveSessionList;
    }

    void ReceiveSessionList(List<SessionInfo> allSessions)
    {
        //Limpiar todas las sesiones
        ClearPreviousChildren();

        //Checkeo de lista nula
        if(allSessions.Count == 0)
        {
            NoSessionFound();
            return;
        }
        //Por cada sesion, instancear un nuevo SessionItem
        foreach(var session in allSessions) 
        {
            AddNewSessionItem(session);
        }
    }

    void ClearPreviousChildren()
    {
        foreach (GameObject child in _parent.transform) Destroy(child);

        _statusTextObject.SetActive(false);
    }

    void NoSessionFound() => _statusTextObject.SetActive(true);

    void AddNewSessionItem(SessionInfo session)
    {
        var newItem = Instantiate(_itemPrefab, _parent.transform);
        newItem.SetInfo(session, JoinSelectedSession);
    }

    void JoinSelectedSession(SessionInfo session)
    {
        _networkHostHandler.JoinGame(session);
    }
}
