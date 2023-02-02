
public interface IInventory
{
    bool AddItem(ItemSO pItem);
    bool AddItem(ItemSO pItem, int pSlot, int pQuantity);
    bool RemoveItem(int pSlot, int pQuantity);
    void SwapInventory(int pSlot1, int pSlot2);
}
