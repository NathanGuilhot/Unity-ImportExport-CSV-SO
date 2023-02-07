using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerEquip : MonoBehaviour
{
    ItemSO _equipedItem;
    [SerializeField] int DEFAULTATTACK = 10;

    private void OnEnable()
    {
        EquipmentSlot.OnUpdateEquipment += UpdateEquipment;
    }
    private void OnDisable()
    {
        EquipmentSlot.OnUpdateEquipment -= UpdateEquipment;
    }

    void UpdateEquipment(ItemSO pItem)
    {
        if (pItem != null)
            Debug.Log($"The equiped item is now {pItem?.name} !");
        else
            Debug.Log($"No more equiped items !");

        _equipedItem = pItem;
    }

    public int getBaseAttack()
    {
        return (_equipedItem != null) ? _equipedItem.damage : DEFAULTATTACK;
    }
}
