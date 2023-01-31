using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionHeal : MonoBehaviour, IPotionEffect
{
    int _healAmount;
    EnemyStat _target;

    public void Init(int pHealAmount)
    {
        _healAmount = pHealAmount;
    }

    private void Start()
    {
        _target = GetComponent<EnemyStat>();
        _target.Heal(_healAmount);
        Destroy(this);
    }
    
    
}
