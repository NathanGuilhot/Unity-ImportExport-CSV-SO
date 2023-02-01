using NightenUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour, IPotionTarget
{
    public static event Action<int, int> onUpdateLife;

    [SerializeField] int PV;
    [SerializeField] int PV_Max;

    public List<_ProcessInt> CheckDamage { get; set; } = new List<_ProcessInt>();
    public List<_ProcessInt> CheckAttack { get; set; } = new List<_ProcessInt>();
    public _CheckCondition CanAttack { get; set; } = () => true;
    public Action PerformOnAttack { get; set; }

    public void GetDamage(uint pAmount)
    {
        GameManager.FX.DisplayDamage((int)pAmount, Camera.main.transform.position + new Vector3(10f, 0f, 4f));

        PV = Mathf.Max(PV - (int)pAmount, 0);
        onUpdateLife?.Invoke(PV, PV_Max);

        if (PV <= 0)
        {
            OnDead();
        }

    }

    void OnDead()
    {
        Debug.Log("You're dead :(");
    }

    public void Heal(int pAmount)
    {
        PV = Mathf.Min(PV + pAmount, PV_Max);
        onUpdateLife?.Invoke(PV, PV_Max);

        GameEvent.NotificationEvent("You're healing!");
    }
}
