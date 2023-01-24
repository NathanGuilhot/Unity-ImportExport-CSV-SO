using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static IFXController FX;

    private void Start()
    {
        FX = GetComponent<IFXController>();
    }

}
