using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;

public class GameManager : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI _timerText, _authoriryText;

    [Networked] private float Timer { get; set; }

    public override void Spawned()
    {
        Debug.Log(Object.HasStateAuthority);
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority) Timer += Runner.DeltaTime;

        _timerText.text = $"Timer: {Timer}";
        _authoriryText.text = $"Authority: {Object.HasStateAuthority}";
    }
}
