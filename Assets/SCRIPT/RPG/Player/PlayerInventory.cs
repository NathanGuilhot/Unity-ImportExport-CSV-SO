using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using NightenUtils;
using System.Linq;

public class PlayerInventory : MonoBehaviour, IInventory
{
    public const int INVENTORYSLOTSNUMBER = 6;
    [SerializeField] KeyValuePair<ItemSO, int>[] _InventoryData = new KeyValuePair<ItemSO, int>[INVENTORYSLOTSNUMBER];

    [SerializeField] ItemSO[] StartingInventory;

    private void Start()
    {
        GameEvent.InventoryChangeEvent(_InventoryData);

        for (int i = 0; i < StartingInventory.Length; i++)
        {
            ItemSO item = StartingInventory[i];
            AddItem(item);
        }
    }

    public bool AddItem(ItemSO pItem)
    {
        Debug.Log($"{pItem.name} has been added to the inventory!");
        return AddItem(pItem, 1);
    }
    public bool AddItem(ItemSO pItem, int pQuantity)
    {
        int firstAvailableSlot = -1;

        for (int i = INVENTORYSLOTSNUMBER - 1; i >= 0; i--)
        {
            if (_InventoryData[i].Key == pItem)
            {
                firstAvailableSlot = i;
                break;
            }
            else if (IsInventorySlotEmpty(i))
            {
                firstAvailableSlot = i;
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

    public bool RemoveItem(int pSlot, int pQuantity)
    {
        Debug.Log($"Trying to remove {pQuantity} items from slot #{pSlot}");
        if (IsInventorySlotEmpty(pSlot))
        {
            return false;
        }
        else if (_InventoryData[pSlot].Value >= pQuantity)
        {
            KeyValuePair<ItemSO, int> newSlot = _InventoryData[pSlot].Value == pQuantity ? new KeyValuePair<ItemSO, int>() : new KeyValuePair<ItemSO, int>(_InventoryData[pSlot].Key, _InventoryData[pSlot].Value - pQuantity);
            _InventoryData[pSlot] = newSlot;
            GameEvent.InventoryChangeEvent(_InventoryData);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SwapInventory(int pSlot1, int pSlot2)
    {
        (_InventoryData[pSlot1], _InventoryData[pSlot2]) = (_InventoryData[pSlot2], _InventoryData[pSlot1]);
        GameEvent.InventoryChangeEvent(_InventoryData);
    }

    bool IsInventorySlotEmpty(int pSlot) {
        return (_InventoryData[pSlot].Key == null);
        //return _InventoryData[pSlot].Equals(default(KeyValuePair<ItemSO, int>));
    }

    public (bool, KeyValuePair<ItemSO, int>) GetRandomElement()
    {
        int[] RandomRange = _Random.Shuffle<int>(Enumerable.Range(0, _InventoryData.Length).ToArray());
        for (int i=0; i < RandomRange.Length; i++)
        {
            if (!IsInventorySlotEmpty(i))
            {
                return (true, _InventoryData[i]); //Success
            }
        }
        return (false, new KeyValuePair<ItemSO, int>()); //Fail :( 
    }
}
