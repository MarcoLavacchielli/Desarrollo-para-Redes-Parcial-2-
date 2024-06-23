using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NickNameHandler : MonoBehaviour
{
    public static NickNameHandler Instance;

    [SerializeField] NickNameItem _nickNamePrefab;

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

        newNickName.SetOwner(owner.transform);

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
