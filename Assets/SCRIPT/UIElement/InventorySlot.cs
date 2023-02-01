using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IDropHandler
{
    [SerializeField] int _slotNumber;
    [SerializeField] int _amount = 0;
    [SerializeField] TextMeshProUGUI _AmountDisplay;
    [SerializeField] Image _ImageRenderer;
    [SerializeField] Sprite _blankImage;

    public ItemSO HeldObject { get; set; }

    bool _mouseHover = false;
    [SerializeField] TooltipHandler _ToolTipPrefab;
    TooltipHandler _ToolTipInstance;

    public void Start()
    {
        SetAmount(_amount);
    }

    public virtual void SetHeldObject(ItemSO pItem)
    {
        HeldObject = pItem;
    }
    public void SetSprite(Sprite pSprite)
    {
        _ImageRenderer.sprite = pSprite;
    }
    public void SetSpriteBlank()
    {
        _ImageRenderer.sprite = _blankImage;
    }

    private void OnDestroy()
    {
        if (_ToolTipInstance != null) //Delete the tooltip if there isn't any object in that slot
            Destroy(_ToolTipInstance.gameObject);
        
    }
    public void SetAmount(int pAmount)
    {
        _AmountDisplay.text = pAmount.ToString();
        _AmountDisplay.gameObject.SetActive(pAmount > 1);
        _amount = pAmount;
    }
    public void SetSlot(int pSlot)
    {
        _slotNumber = pSlot;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _mouseHover = true;
        if (HeldObject != null && !PlayerInventoryDrag.isDragging) {
            _ToolTipInstance = Instantiate<TooltipHandler>(_ToolTipPrefab, UIController.MainCanvas.transform);
            _ToolTipInstance.SetItem(HeldObject);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _mouseHover = false;
        if (_ToolTipInstance != null)
            Destroy(_ToolTipInstance.gameObject);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!PlayerInventoryDrag.isDragging && HeldObject != null && HeldObject.canUse)
        {
            _PotionEffect.Perform(EnemySpawner.ActiveEnemy.gameObject, HeldObject);

            GameManager.Inventory.RemoveItem(_slotNumber, 1);
        }
        
        PlayerInventoryDrag.isDragging = false;
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        PlayerInventoryDrag.slotDragged = this;
        PlayerInventoryDrag.itemDragged = HeldObject;
        PlayerInventoryDrag.itemQuantity = _amount;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!PlayerInventoryDrag.isDragging)
        {
            PlayerInventoryDrag.isDragging = true;
            PlayerInventoryDrag.slotDragged = this;
            PlayerInventoryDrag.itemDragged = HeldObject;
            PlayerInventoryDrag.itemQuantity = _amount;
        }
    }


    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("DROP");
        if (PlayerInventoryDrag.slotDragged != this)
        {
            Debug.Log("Drag event ended");

            bool AddSuccess = (isThisSlotInInventory()) ?
                    GameManager.Inventory.AddItem(PlayerInventoryDrag.itemDragged, _slotNumber, PlayerInventoryDrag.itemQuantity) :
                    ManualAddItem(PlayerInventoryDrag.itemDragged, PlayerInventoryDrag.itemQuantity);
            //bool AddSuccess = GameManager.Inventory.AddItem(PlayerInventoryDrag.itemDragged, _slotNumber, PlayerInventoryDrag.itemQuantity);

            if (AddSuccess)
            {
                bool RemoveSuccess = (PlayerInventoryDrag.slotDragged.isThisSlotInInventory()) ?
                    GameManager.Inventory.RemoveItem(PlayerInventoryDrag.slotDragged._slotNumber, PlayerInventoryDrag.itemQuantity) :
                    PlayerInventoryDrag.slotDragged.ManualRemoveItem(PlayerInventoryDrag.itemQuantity);
                //GameManager.Inventory.RemoveItem(PlayerInventoryDrag.slotDragged, PlayerInventoryDrag.itemQuantity);
            }
            else
            {
                if (!isItemAllow(PlayerInventoryDrag.itemDragged))
                {
                    return;
                }

                if (PlayerInventoryDrag.slotDragged.isThisSlotInInventory() &&
                    isThisSlotInInventory())
                {
                    GameManager.Inventory.SwapInventory(PlayerInventoryDrag.slotDragged._slotNumber, _slotNumber);
                }
                else {
                    //One of the slot isn't part of the inventory
                    if (PlayerInventoryDrag.slotDragged.isThisSlotInInventory())
                    {
                        //Remove and replace
                        GameManager.Inventory.RemoveItem(PlayerInventoryDrag.slotDragged._slotNumber, PlayerInventoryDrag.itemQuantity);
                        GameManager.Inventory.AddItem(HeldObject, PlayerInventoryDrag.slotDragged._slotNumber, _amount);
                    }
                    else
                    {
                        PlayerInventoryDrag.slotDragged.ManualRemoveItem(PlayerInventoryDrag.itemQuantity);
                        PlayerInventoryDrag.slotDragged.ManualAddItem(HeldObject, _amount);
                    }

                    if (isThisSlotInInventory())
                    {
                        //Remove and replace 
                        GameManager.Inventory.RemoveItem(_slotNumber, _amount);
                        GameManager.Inventory.AddItem(PlayerInventoryDrag.itemDragged, _slotNumber, PlayerInventoryDrag.itemQuantity);
                    }
                    else
                    {
                        ManualRemoveItem(_amount);
                        ManualAddItem(PlayerInventoryDrag.itemDragged, PlayerInventoryDrag.itemQuantity);
                    }
                }
            }
        }
        PlayerInventoryDrag.isDragging = false;
    }

    //used only by slot not in the inventory array

    public virtual bool ManualAddItem(ItemSO pItem, int pQuantity)
    {
        Debug.Log($"ManualAdd {_amount} / {pQuantity}");
        if (HeldObject == null || HeldObject == pItem)
        {
            SetHeldObject(pItem);
            SetSprite(pItem.sprite);
            SetAmount(_amount + pQuantity);
            return true;
        }
        
        return false;
    }
    bool ManualRemoveItem(int pQuantity)
    {
        Debug.Log($"ManualRemove {_amount} / {pQuantity}");
        if (_amount <= pQuantity)
        {
            SetHeldObject(null);
            SetAmount(0);
            SetSpriteBlank();
            return true;
        }

        SetAmount(_amount - pQuantity);
        return true;
    }

    bool isThisSlotInInventory() {
        return (0 <= _slotNumber && _slotNumber <= PlayerInventory.INVENTORYSLOTSNUMBER);
    }

    public virtual bool isItemAllow(ItemSO pItem)
    {
        return true;
    }
}
