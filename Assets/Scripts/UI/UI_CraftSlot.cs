using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    private void OnEnable()
    {
        UpdateSlot(item);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        //工艺数据
        ItemData_Equipment craftDate=item.date as ItemData_Equipment;
        Inventory.instance.CanCraft(craftDate,craftDate.craftingMaterials);
    }
}
