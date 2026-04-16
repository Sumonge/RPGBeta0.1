using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;



    private void SetipVisuals()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.itemIcon;//icon改成itemIcon了
        gameObject.name = "Item object-" + itemData.itemName;
    }


    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;

        rb.velocity = _velocity;
        SetipVisuals();
    }

    public void PickupItem()
    {

        if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0, 7);
            return;
        }

        AudioManager.instance.PlaySFX(26, transform);//拾取音效

        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
