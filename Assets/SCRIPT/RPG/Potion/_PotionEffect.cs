using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PotionEffect : MonoBehaviour
{
    public static void Perform(IPotionTarget pTarget, ItemSO pPotion)
    {

        Dictionary<ItemSO.PotionType, Type> PotionMap = new Dictionary<ItemSO.PotionType, Type>()
        {
            {ItemSO.PotionType.heal, typeof(PotionHeal)},
            {ItemSO.PotionType.love, typeof(PotionLove)},
            {ItemSO.PotionType.enrage, typeof(PotionEnrage)},
            {ItemSO.PotionType.pacifier, typeof(PotionPacifier)},
        };

        IPotionEffect Potion = (IPotionEffect)pTarget.gameObject.AddComponent(
                PotionMap[pPotion.potionType]
            );
        
        Potion?.Init(pPotion, pTarget);
    }
}
