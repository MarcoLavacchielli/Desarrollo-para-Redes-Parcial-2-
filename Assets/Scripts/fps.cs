using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (QualitySettings.vSyncCount != 0)
        {
            QualitySettings.vSyncCount = 0;
        }
    }
}
