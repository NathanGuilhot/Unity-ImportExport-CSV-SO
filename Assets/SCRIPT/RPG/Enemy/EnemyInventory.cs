using System.Collections;
using UnityEngine;
using NightenUtils;
using System.Collections.Generic;

public class EnemyInventory : MonoBehaviour, IInventory
{
    [SerializeField] List<ItemSO> _InventoryData;
    public void Init(ItemSO[] pInventory)
    {
        _InventoryData = new List<ItemSO>(pInventory);
    }
    public bool AddItem(ItemSO pItem)
    {
        _InventoryData.Add(pItem);
        return true;
    }

    public bool AddItem(ItemSO pItem, int pQuantity)
    {
        for (int i = 0; i < pQuantity; i++)
        {
            AddItem(pItem);
        }
        return true;
    }

    public bool AddItem(ItemSO pItem, int pSlot, int pQuantity)
    {
        return AddItem(pItem, pQuantity);
    }

    public (bool, NightenUtils.KeyValuePair<ItemSO, int>) GetRandomElement()
    {
        if (_InventoryData.Count == 0)
            return (false, new NightenUtils.KeyValuePair<ItemSO, int>());

        int RandomNumber = Random.Range(0, _InventoryData.Count);
        return (true, new NightenUtils.KeyValuePair<ItemSO, int>(_InventoryData[RandomNumber], 1));
    }

    public bool RemoveItem(int pSlot, int pQuantity) => RemoveItem(_InventoryData[pSlot]);

    public bool RemoveItem(ItemSO pItem)
    {
        _InventoryData.Remove(pItem);
        return true;
    }

    public void SwapInventory(int pSlot1, int pSlot2)
    {
        return;
    }

    public int GetInventoryCount()
    {
        return _InventoryData.Count;
    }
}
