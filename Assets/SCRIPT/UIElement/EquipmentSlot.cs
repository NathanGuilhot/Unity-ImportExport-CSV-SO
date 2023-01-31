using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : InventorySlot
{
    public delegate void UpdateEquipment(ItemSO pItem);
    public static event UpdateEquipment OnUpdateEquipment;

    public override void SetHeldObject(ItemSO pItem)
    {
        base.SetHeldObject(pItem);
        OnUpdateEquipment?.Invoke(HeldObject);
    }

    public override bool ManualAddItem(ItemSO pItem, int pQuantity)
    {
        if (!isItemAllow(pItem))
        {
            GameEvent.NotificationEvent("Can't equip this item!");
            return false;
        }
        return base.ManualAddItem(pItem, pQuantity);
    }

    public override bool isItemAllow(ItemSO pItem)
    {
        return (pItem.canEquip);
    }
}
