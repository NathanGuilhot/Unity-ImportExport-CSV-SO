using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Dependencies:
/// 

public class PotionHeal : MonoBehaviour, IPotionEffect
{
    int _healAmount;
    GameObject _FX;
    IPotionTarget _target;

    public void Init(ItemSO pPotion, IPotionTarget pTarget)
    {
        _healAmount = pPotion.PotionValue;
        
        _target = pTarget;
        _target.Heal(_healAmount);
        
        _FX = Instantiate(pPotion.effectParticle, transform);
        _FX.transform.position += new Vector3(0f, 0f, -1f);
    }

    private void Start()
    {
        Destroy(this, 1f);
    }

    private void OnDestroy()
    {
        Destroy(_FX);   
    }


}
