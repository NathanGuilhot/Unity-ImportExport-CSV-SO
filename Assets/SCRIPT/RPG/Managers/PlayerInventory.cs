using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    const int INVENTORYSLOTS = 4;
    KeyValuePair<ItemSO, int>[] _InventoryData = new KeyValuePair<ItemSO, int>[INVENTORYSLOTS];

    public bool AddItem(ItemSO pItem)
    {
        Debug.Log($"{pItem.name} has been added to the inventory!");
        return AddItem(pItem, 1);
    }
    public bool AddItem(ItemSO pItem, int pQuantity)
    {
        int firstAvailableSlot = -1;

        for (int i = 0; i < INVENTORYSLOTS; i++)
        {
            if (IsInventorySlotEmpty(i) || _InventoryData[i].Key == pItem)
            {
                firstAvailableSlot = i;
                break;
            }
        }

        if (firstAvailableSlot == -1)
            return false; //No space available

        return AddItem(pItem, firstAvailableSlot, pQuantity);
    }
    public bool AddItem(ItemSO pItem, int pSlot, int pQuantity)
    {
        if (IsInventorySlotEmpty(pSlot))
        {
            _InventoryData[pSlot] = new KeyValuePair<ItemSO, int>(pItem, pQuantity);
            GameEvent.InventoryChangeEvent(_InventoryData); 
            return true;
        }
        else if (_InventoryData[pSlot].Key == pItem)
        {
            KeyValuePair<ItemSO, int>  newSlot = new KeyValuePair<ItemSO, int>(_InventoryData[pSlot].Key, _InventoryData[pSlot].Value + pQuantity);
            _InventoryData[pSlot] = newSlot;
            GameEvent.InventoryChangeEvent(_InventoryData);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void LogInventory()
    {
        Debug.Log("---------\nInventory:");
        for (int i = 0; i < INVENTORYSLOTS; i++)
        {
            if (IsInventorySlotEmpty(i))
            {
                Debug.Log($"{i} :: Empty");
            }
            else { 
                Debug.Log($"{i} :: {_InventoryData[i].Key.name} > {_InventoryData[i].Value}");
            }
        }
        Debug.Log("---------");
    }

    //Should only be used by the manager
    public KeyValuePair<ItemSO, int>[] GetInventory()
    {
        return _InventoryData;
    }

    bool IsInventorySlotEmpty(int pSlot) {
        return _InventoryData[pSlot].Equals(default(KeyValuePair<ItemSO, int>));
    }
}
