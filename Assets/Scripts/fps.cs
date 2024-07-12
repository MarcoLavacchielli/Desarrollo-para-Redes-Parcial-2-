using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps : MonoBehaviour
{
    void Start()
    {
        // Establece la tasa de cuadros objetivo a 60 fps
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        // Asegura que VSync esté desactivado
        if (QualitySettings.vSyncCount != 0)
        {
            QualitySettings.vSyncCount = 0;
        }
    }
}
