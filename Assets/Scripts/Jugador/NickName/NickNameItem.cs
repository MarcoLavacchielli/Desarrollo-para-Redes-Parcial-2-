using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NickNameItem : MonoBehaviour
{
    float _offSet = 2f;
    Transform _target;
    TextMeshProUGUI _nameText;
    Camera _camera;

    public void SetOwner(Transform target, Camera camera)
    {
        _target = target;
        _camera = camera;
        _nameText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateNickName(string newName) => _nameText.text = newName;

    public void UpdatePosition()
    {
        if (_target != null)
        {
            transform.position = _target.position + Vector3.up * _offSet;
            if (_camera != null)
            {
                transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward, _camera.transform.rotation * Vector3.up);
            }
        }
    }

    void LateUpdate()
    {
        UpdatePosition();
    }
}
