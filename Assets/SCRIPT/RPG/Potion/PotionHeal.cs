using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionHeal : MonoBehaviour, IPotionEffect
{
    int _healAmount;
    GameObject _FX;
    IPotionTarget _target;

    public void Init(ItemSO pPotion)
    {
        _healAmount = pPotion.PotionValue;
        _FX = Instantiate(pPotion.effectParticle, transform);
        _FX.transform.position += new Vector3(0f, 0f, -1f);
    }

    private void Start()
    {
        _target = GetComponent<IPotionTarget>();
        _target.Heal(_healAmount);
        Destroy(this, 1f);
    }

    private void OnDestroy()
    {
        Destroy(_FX);   
    }


}
