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

    public override bool AddItem(ItemSO pItem, int pQuantity)
    {
        if (!isItemAllow(pItem))
        {
            GameEvent.NotificationEvent("Can't equip this item!");
            return false;
        }

        Debug.Log($"ManualAdd {Amount} / {pQuantity}");
        if (HeldObject == null || HeldObject == pItem)
        {
            SetHeldObject(pItem);
            SetSprite(pItem.sprite);
            SetAmount(Amount + pQuantity);
            return true;
        }

        return false;
    }

    public override bool RemoveItem(int pQuantity)
    {
        Debug.Log($"ManualRemove {Amount} / {pQuantity}");
        if (Amount <= pQuantity)
        {
            SetHeldObject(null);
            SetAmount(0);
            SetSpriteBlank();
            return true;
        }

        SetAmount(Amount - pQuantity);
        return true;
    }

    public override bool isItemAllow(ItemSO pItem)
    {
        return (pItem.canEquip);
    }
}
