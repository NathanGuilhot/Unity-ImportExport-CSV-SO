using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IDropHandler
{
    [SerializeField] int _slotNumber;
    [SerializeField] public int Amount = 0;
    [SerializeField] TextMeshProUGUI _AmountDisplay;
    [SerializeField] Image _ImageRenderer;
    [SerializeField] Sprite _blankImage;

    public ItemSO HeldObject { get; set; }

    [SerializeField] TooltipHandler _ToolTipPrefab;
    TooltipHandler _ToolTipInstance;

    public void Start()
    {
        SetAmount(Amount);
    }

    public virtual void SetHeldObject(ItemSO pItem)
        => HeldObject = pItem;
    public void SetSprite(Sprite pSprite)
        => _ImageRenderer.sprite = pSprite;
    public void SetSpriteBlank()
        => _ImageRenderer.sprite = _blankImage;

    private void OnDestroy()
    {
        if (_ToolTipInstance != null) //Delete the tooltip if there isn't any object in that slot
            Destroy(_ToolTipInstance.gameObject);
        
    }
    public void SetAmount(int pAmount)
    {
        _AmountDisplay.text = pAmount.ToString();
        _AmountDisplay.gameObject.SetActive(pAmount > 1);
        Amount = pAmount;
    }
    public void SetSlot(int pSlot) => _slotNumber = pSlot;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (HeldObject != null && !PlayerInventoryDrag.isDragging) {
            _ToolTipInstance = Instantiate<TooltipHandler>(_ToolTipPrefab, UIController.MainCanvas.transform);
            _ToolTipInstance.SetItem(HeldObject);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_ToolTipInstance != null)
            Destroy(_ToolTipInstance.gameObject);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!PlayerInventoryDrag.isDragging && HeldObject != null && HeldObject.canUse)
        {
            _PotionEffect.Perform(HeldObject.targetPlayer ? GameManager.Player : EnemySpawner.ActiveEnemy,
                                HeldObject);
            RemoveItem(1);
        }
        PlayerInventoryDrag.isDragging = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        PlayerInventoryDrag.slotDragged = this;
        PlayerInventoryDrag.itemDragged = HeldObject;
        PlayerInventoryDrag.itemQuantity = Amount;
    }

    public void OnDrag(PointerEventData eventData)
        => PlayerInventoryDrag.isDragging = true;

    public void OnDrop(PointerEventData eventData)
    {
        PlayerInventoryDrag.isDragging = false;
        if (PlayerInventoryDrag.slotDragged != this)
        {
            Debug.Log("Drag event ended");

            bool AddSuccess = AddItem(PlayerInventoryDrag.itemDragged, PlayerInventoryDrag.itemQuantity);

            if (AddSuccess)
            {
                bool RemoveSuccess = PlayerInventoryDrag.slotDragged.RemoveItem(PlayerInventoryDrag.itemQuantity);
                return;
            }
            
            if (!isItemAllow(PlayerInventoryDrag.itemDragged))
                return;

            if (PlayerInventoryDrag.slotDragged.isThisSlotInInventory() && this. isThisSlotInInventory())
            {
                GameManager.Inventory.SwapInventory(PlayerInventoryDrag.slotDragged._slotNumber, _slotNumber);
                return;
            }
            //Manual swap
            PlayerInventoryDrag.slotDragged.RemoveItem(PlayerInventoryDrag.itemQuantity);
            PlayerInventoryDrag.slotDragged.AddItem(HeldObject, Amount);
            this.RemoveItem(Amount);
            this.AddItem(PlayerInventoryDrag.itemDragged, PlayerInventoryDrag.itemQuantity);
            
        }
    }

    public virtual bool AddItem(ItemSO pItem, int pQuantity)
        => GameManager.Inventory.AddItem(pItem, _slotNumber, pQuantity);
    public virtual bool RemoveItem(int pQuantity) 
        => GameManager.Inventory.RemoveItem(_slotNumber, pQuantity);

    bool isThisSlotInInventory()
        => (0 <= _slotNumber && _slotNumber <= PlayerInventory.INVENTORYSLOTSNUMBER);

    public virtual bool isItemAllow(ItemSO pItem) => true;
}