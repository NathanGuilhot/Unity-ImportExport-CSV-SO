using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerEvent
{
    public static event Action<int, int> onUpdateLife;
    public static void UpdateLife(int PV, int PV_Max)
    {
        onUpdateLife?.Invoke(PV, PV_Max);
    }
}
