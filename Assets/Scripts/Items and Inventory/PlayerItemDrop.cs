using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player`s drop")]
    [SerializeField] private float chanceToLooseItems;
    [SerializeField] private float chanceToLooseMaterials;

    public override void GenerateDrop()
    {
        Inventory inventory=Inventory.instance;

        List<InventoryItem> itemToUnequip=new List<InventoryItem> ();
        List<InventoryItem> materulsToLoose = new List<InventoryItem>();

        foreach (InventoryItem item in inventory.GetEquipmentList())
        {
            if(Random.Range(0,100)<=chanceToLooseItems)
            {
                DropItem(item.data);
                inventory.UnequipItem(item.data as ItemData_Equipment);//Υ��foreachֱ���޸����飬����tolistѭ������ʹ��forѭ���������
            }
            
        }
        for (int i = 0; i < itemToUnequip.Count; i++)
        {

            inventory.UnequipItem(itemToUnequip[i].data as ItemData_Equipment);
        }

        foreach(InventoryItem item in inventory.GetStashList())
        {
            if(Random.Range(0,100)<=chanceToLooseMaterials)
            {
                DropItem(item.data);
                materulsToLoose.Add(item);
            }
        }
        for (int i = 0; i < materulsToLoose.Count; i++)
        {
            inventory.RemoveItem(materulsToLoose[i].data);
        }
    }
}
