using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IPotionTarget
{
    string name { get; }
    GAMESTATE ActiveState { get; }
    Func<bool> CanAttack { get; set; }
    List<Func<int,int>> CheckAttack { get; set; }
    List<Func<int, int>> CheckDamage { get; set; }
    Action PerformOnAttack { get; set; }
    GameObject gameObject {get;}

    void Heal(int pAmount);
    void GetDamage(int pAmount);
}
