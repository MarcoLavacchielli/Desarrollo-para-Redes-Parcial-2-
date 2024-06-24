using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NickNameHandler : MonoBehaviour
{
    public static NickNameHandler Instance;

    [SerializeField] NickNameItem _nickNamePrefab;
    [SerializeField] Camera _camera; // Añadido para referenciar la cámara

    List<NickNameItem> _allNickNames = new List<NickNameItem>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public NickNameItem CreateNewNickName(NetworkHostPlayer owner)
    {
        var newNickName = Instantiate(_nickNamePrefab, transform);
        _allNickNames.Add(newNickName);

        newNickName.SetOwner(owner.transform, _camera); // Pasar la cámara al NickNameItem

        owner.OnPlayerDespawn += () =>
        {
            _allNickNames.Remove(newNickName);
            Destroy(newNickName.gameObject);
        };
        return newNickName;
    }

    void LateUpdate()
    {
        foreach (var item in _allNickNames) item.UpdatePosition();
    }
}
