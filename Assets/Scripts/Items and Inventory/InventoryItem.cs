using System;

[Serializable]
public class InventoryItem
{
    public ItemData date;
    public int stackSize;

    public InventoryItem(ItemData _newItemDate)
    {
        date = _newItemDate;
        AddStack();
    }

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
