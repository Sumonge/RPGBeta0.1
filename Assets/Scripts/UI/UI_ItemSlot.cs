using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    protected UI ui;
    public InventoryItem item;

    protected virtual void Start()
    {
        ui = FindObjectOfType<UI>();
    }

    public void UpdateSlot(InventoryItem _item)
    {
        item = _item;

        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.itemIcon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }

        }
    }

    public void CleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;

        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        if (Input.GetKey(KeyCode.T))
        {
            Inventory.instance.RemoveItem(item.data);
            return;


        }

        if (item.data.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.data);

        ui.itemToolTip.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
            return;


        Vector2 mousePosition = Input.mousePosition;

        float XOffset = 0;
        float YOffset = 0;

        if (mousePosition.x > 600)
            XOffset = -150;
        else
            XOffset = 150;

        if (mousePosition.y > 320)
            YOffset = -150;
        else
            YOffset = 150;

        ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
        ui.itemToolTip.transform.position = new Vector2(mousePosition.x + XOffset, mousePosition.y + YOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
            return;
        ui.itemToolTip.HideToolTip();
    }
}

