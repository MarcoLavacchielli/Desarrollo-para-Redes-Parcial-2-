using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Crear barras de vida
//Actualizar cada una de las barras de vida
//Destruir la barra de vida que necesitemos destruir


public class LifeHandler : MonoBehaviour
{
    public static LifeHandler Instance { get; private set; }

    [SerializeField] LifeBar _lifeBarPrefab;

    List<LifeBar> _lifeBarList = new List<LifeBar>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CreateLifeBar(PlayerModel target)
    {
        //Instancear barra de vida y setearlo en el lugar del player

        LifeBar lifeBar = Instantiate(_lifeBarPrefab, transform).SetTarget(target);

        _lifeBarList.Add(lifeBar);

        target.OnPlayerDespawn += () =>   //lambda
        {
            _lifeBarList.Remove(lifeBar);
            Destroy(lifeBar.gameObject);
        };

       // target.OnPlayerDespawn += DestroyLifeBar;
    }

   /* private void DestroyLifeBar(LifeBar lifeBar)
    {
        _lifeBarList.Remove(lifeBar);
        Destroy(lifeBar.gameObject);
    }*/

    private void LateUpdate()
    {
        foreach (var bar in _lifeBarList)
        {
            bar.UpdatePosition();
        }
    }
}
