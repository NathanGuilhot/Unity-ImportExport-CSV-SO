using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PotionEffect : MonoBehaviour
{
    public static void Perform(IPotionTarget pTarget, ItemSO pPotion)
    {
        IPotionEffect Potion = null;
        switch (pPotion.potionType)
        {
            case ItemSO.PotionType.heal:
                Potion = (IPotionEffect)pTarget.gameObject.AddComponent<PotionHeal>();
                break;
            case ItemSO.PotionType.love:
                Potion = (IPotionEffect)pTarget.gameObject.AddComponent<PotionLove>();
                break;
            case ItemSO.PotionType.enrage:
                Potion = (IPotionEffect)pTarget.gameObject.AddComponent<PotionEnrage>();
                break;
            case ItemSO.PotionType.pacifier:
                Potion = (IPotionEffect)pTarget.gameObject.AddComponent<PotionPacifier>();
                break;
            default:
                break;
        }
        Potion?.Init(pPotion, pTarget);
    }
}
