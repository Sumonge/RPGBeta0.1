using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ItemToolTip : MonoBehaviour
{
    [SerializeField]private TMPro.TextMeshProUGUI itemNameText;
    [SerializeField]private TMPro.TextMeshProUGUI itemDescription;
    [SerializeField]private TMPro.TextMeshProUGUI itemTypeText;

    [SerializeField] private int defaultFontSize = 32;
    void Start()
    {
        
    }

    public void ShowToolTip(ItemData_Equipment item)
    {
        if (item == null)
            return;

        itemNameText.text = item.itemName;
        
        itemTypeText.text = item.itemType.ToString();

        itemDescription.text = item.GetDescription();

        if (itemNameText.text.Length > 12)
            itemNameText.fontSize = itemNameText.fontSize * .7f;
        else
            itemNameText.fontSize = defaultFontSize;

        gameObject.SetActive(true);

    }
    public void HideToolTip()
    {
        itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }
}
