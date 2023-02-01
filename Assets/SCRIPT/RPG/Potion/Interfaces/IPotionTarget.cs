using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NightenUtils;
using System;

public interface IPotionTarget
{
    _CheckCondition CanAttack { get; set; }
    List<_ProcessInt> CheckAttack { get; set; }
    List<_ProcessInt> CheckDamage { get; set; }
    Action PerformOnAttack { get; set; }

    void Heal(int pAmount);
    void GetDamage(uint pAmount);
}
