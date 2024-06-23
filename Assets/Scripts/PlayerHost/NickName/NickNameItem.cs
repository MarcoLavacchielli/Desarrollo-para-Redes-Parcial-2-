using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NickNameItem : MonoBehaviour
{
    float _offSet = 2f;

    Transform _target;

    TextMeshProUGUI _nameText;

    public void SetOwner(Transform target)
    {
        _target = target;
        _nameText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateNickName(string newName) => _nameText.text = newName;

    public void UpdatePosition() => transform.position = _target.position +  Vector3.up * _offSet;
}
