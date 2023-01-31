using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeBar : LifeBar
{
    private void OnEnable()
    {
        PlayerStat.onUpdateLife += UpdateLife;
    }
    private void OnDisable()
    {
        PlayerStat.onUpdateLife -= UpdateLife;
    }

    void UpdateLife(int pPV, int pPVMax)
    {
        SetValue((float)pPV / (float)pPVMax);
    }
}
