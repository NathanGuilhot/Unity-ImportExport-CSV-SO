using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PotionEffect : MonoBehaviour
{
    public static void Perform(GameObject pTarget, ItemSO pPotion)
    {
        IPotionEffect Potion = null;
        switch (pPotion.potionType)
        {
            case ItemSO.PotionType.heal:
                Potion = (IPotionEffect)pTarget.AddComponent<PotionHeal>();
                break;
            case ItemSO.PotionType.love:
                Potion = (IPotionEffect)pTarget.AddComponent<PotionLove>();
                break;
            case ItemSO.PotionType.enrage:
                Potion = (IPotionEffect)pTarget.AddComponent<PotionEnrage>();
                break;
            case ItemSO.PotionType.pacifier:
                Potion = (IPotionEffect)pTarget.AddComponent<PotionPacifier>();
                break;
            default:
                break;
        }
        Potion?.Init(pPotion);
    }
}
