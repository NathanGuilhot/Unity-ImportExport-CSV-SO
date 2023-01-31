using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    [SerializeField] GameObject _InventoryLayout;
    [SerializeField] InventorySlot _InventoryLayoutSlot;

    private void OnEnable()
    {
        GameEvent.OnInventoryChanged += UpdateInventory;
    }
    private void OnDisable()
    {
        GameEvent.OnInventoryChanged -= UpdateInventory;
    }

    void UpdateInventory(KeyValuePair<ItemSO, int>[] pInventory)
    {
        //Destroy all children for the layout
        foreach (Transform child in _InventoryLayout.transform)
        {
            Destroy(child.gameObject);
        }

        //Spawn all items in the layout
        for (int i = 0; i < pInventory.Length; i++)
        {
            InventorySlot InventoryItem = Instantiate<InventorySlot>(_InventoryLayoutSlot, _InventoryLayout.transform);
            
            if (!pInventory[i].Equals(default(KeyValuePair<ItemSO, int>))) //Check if a value has been initialized
            {
                InventoryItem.SetHeldObject(pInventory[i].Key);
                InventoryItem.SetSprite(pInventory[i].Key.sprite);
                InventoryItem.SetAmount(pInventory[i].Value);
            } else
            {
                InventoryItem.SetSpriteBlank();
                InventoryItem.SetAmount(0);
            }
            InventoryItem.SetSlot(i);
        }
    }
}
