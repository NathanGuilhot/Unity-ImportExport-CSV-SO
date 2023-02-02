using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFXController
{
    void DisplayDamage(int pAmount, Vector3 pPosition);
    void AddSlash(Vector3 pPosition);
}
