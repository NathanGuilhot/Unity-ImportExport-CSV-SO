using UnityEngine;

public class PlayerInventoryDrag: MonoBehaviour
{
    public static bool isDragging = false;
    public static ItemSO itemDragged;
    public static InventorySlot slotDragged;
    public static int itemQuantity;


    /// JUST USED TO MAKE VARIABLE DISPLAY IN THE EDITOR///
    [SerializeField] bool _isDragging;
    [SerializeField] ItemSO _itemDragged;
    [SerializeField] InventorySlot _slotDragged;
    [SerializeField] int _itemQuantity;

    [SerializeField] DragObjectVisual DragVisualPrefab;
    [SerializeField] static DragObjectVisual DragVisualInstance;

    private void Update()
    {
        //Debug.Log($"Update: {!_isDragging} && {isDragging}");
        if (!_isDragging && isDragging) //If we wasn't dragging on the last frame
        {
            //Spawn
            DragVisualInstance = Instantiate<DragObjectVisual>(DragVisualPrefab, UIController.MainCanvas.transform);
            DragVisualInstance.SetSprite(itemDragged.sprite);
        } else if (_isDragging && !isDragging) //Drag released
        {
            //Despawn
            Destroy(DragVisualInstance.gameObject);
        }

        /////////////////////////////////////////////////////////
        _isDragging = isDragging;
        _itemDragged = itemDragged;
        _slotDragged = slotDragged;
        _itemQuantity = itemQuantity;
        /////////////////////////////////////////////////////////
    

    }

}
