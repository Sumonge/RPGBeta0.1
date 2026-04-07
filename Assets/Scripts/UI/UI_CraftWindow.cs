using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button craftButton;

    [SerializeField] private Image[] materialImage;

    public void SetupCraftWindow(ItemData_Equipment _data)
    {
        for (int i = 0; i < materialImage.Length; i++)
        {
            craftButton.onClick.RemoveAllListeners();

            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }
        for (int i = 0; i < _data.craftingMaterials.Count; i++)
        {
            if (_data.craftingMaterials.Count > materialImage.Length)
                Debug.LogWarning("³¬³öÉÏÏÞ");

            materialImage[i].sprite = _data.craftingMaterials[i].data.itemIcon;
            materialImage[i].color = Color.white;
            TextMeshProUGUI materialSlotText=materialImage[i].GetComponentInChildren<TextMeshProUGUI>();


            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().text = _data.craftingMaterials[i].stackSize.ToString();
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

        }

        itemIcon.sprite=_data.itemIcon;
        itemName.text=_data.itemName;
        itemDescription.text=_data.GetDescription();

        craftButton.onClick.AddListener(() =>
        {
            Inventory.instance.CanCraft(_data, _data.craftingMaterials);
        });
    }
}
