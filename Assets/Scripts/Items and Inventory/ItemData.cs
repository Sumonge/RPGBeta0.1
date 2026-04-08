using System.Text;
using UnityEditor;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}



[CreateAssetMenu(fileName = "New Item Date", menuName = "Date/Item")]

public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public string itemId;

    [Range(0, 100)]
    public float dropChance;

    protected StringBuilder sr = new StringBuilder();

    public void OnValidate()
    {
#if UNITY_EDITOR   
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);
#endif
    }
    public virtual string GetDescription()
    {
        return "";
    }
}
