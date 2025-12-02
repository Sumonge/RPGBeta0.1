using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemDate;



    private void SetipVisuals()
    {
        if (itemDate == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemDate.icon;
        gameObject.name = "Item object-" + itemDate.itemName;
    }


    public void SetupItem(ItemData _itemData,Vector2 _velocity)
    {
        itemDate = _itemData;
       
        rb.velocity = _velocity;
        SetipVisuals();
    }

    public void PickupItem()
    {
        Inventory.instance.AddItem(itemDate);
        Destroy(gameObject);
    }
}
