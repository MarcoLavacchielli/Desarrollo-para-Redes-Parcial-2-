using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Updatea posicion y valor


public class LifeBar : MonoBehaviour
{
    [SerializeField] Image _lifeBarImage;
    [SerializeField] float _offset = 2.5f;

    Transform _target;

    public void UpdatePosition()
    {
        transform.position = _target.position + Vector3.up * _offset;
    }

    public void UpdateLifeBar(float amount)
    {
        _lifeBarImage.fillAmount = amount;
    }

    public LifeBar SetTarget(PlayerModel target)
    {
        _target = target.transform;

        target.OnLifeUpdate += UpdateLifeBar;

        return this;
    }

}
