using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeBar : LifeBar
{
    private void OnEnable()
    {
        PlayerEvent.onUpdateLife += UpdateLife;
    }
    private void OnDisable()
    {
        PlayerEvent.onUpdateLife -= UpdateLife;
    }

    void UpdateLife(int pPV, int pPVMax)
    {
        SetValue((float)pPV / (float)pPVMax);
    }
}
