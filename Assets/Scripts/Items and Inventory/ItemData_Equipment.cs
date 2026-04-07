using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Wepon,
    Armor,
    Amulet,
    Flask
}



[CreateAssetMenu(fileName = "New Item Date", menuName = "Date/Equipment")]

public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
    [Header("Unique effect")]


    public float itemCooldown;
    public ItemEffect[] itemEffects;
    [TextArea]
    public string itemEffectDescription;

    [Header("Major stats")]
    public int strength;//±©»÷ÉËº¦
    public int agility;//±©»÷ÂÊ
    public int intellgence;//·¨¿¹
    public int vitality;//Ñ©Á«Ôö¼Ó

    [Header("Offsive stats")]
    public int damage;
    public int critChance;
    public int critPower;//Ä¬ÈÏÖµÎª1.5

    [Header("Defensive stats")]
    public int maxHealth;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    [Header("Craft requirementsx")]
    public List<InventoryItem> craftingMaterials;

    private int descriptionLength;
    public void Effect(Transform _enemyPosition)
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);

        }
    }
    public void AddModfiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intellgence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(maxHealth);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);

    }
    public void RemoveModfiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intellgence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(maxHealth);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);
    }

    public override string GetDescription()
    {

        sr.Length = 0;
        descriptionLength = 0;

        AddItemDescription(strength, "Á¦Á¿");
        AddItemDescription(agility, "Ãô½Ý");
        AddItemDescription(intellgence, "ÖÇÁ¦");
        AddItemDescription(vitality, "ÌåÁ¦");

        AddItemDescription(damage, "ÉËº¦");
        AddItemDescription(critChance, "±©»÷ÂÊ");
        AddItemDescription(critPower, "±©»÷ÉËº¦");

        AddItemDescription(maxHealth, "ÉúÃüÖµ");
        AddItemDescription(armor, "»¤¼×");
        AddItemDescription(evasion, "ÉÁ±Ü");
        AddItemDescription(magicResistance, "Ä§¿¹");

        AddItemDescription(fireDamage, "»ðÑæÉËº¦");
        AddItemDescription(iceDamage, "±ùËªÉËº¦");
        AddItemDescription(lightingDamage, "ÉÁµçÉËº¦");

        if (descriptionLength < 5)
        {
            for (int i = 0; i < 5 - descriptionLength; i++)
            {
                sr.AppendLine();
                sr.Append("");
            }
        }

        if (itemEffectDescription.Length > 0)
        {
            sr.AppendLine();
            sr.Append(itemEffectDescription);
        }

        return sr.ToString();
    }

    private void AddItemDescription(int _value, string _name)
    {
        if (_value != 0)
        {
            if (sr.Length > 0)
                sr.AppendLine();

            if (_value > 0)
                sr.Append("+" + _value + "" + _name);

            descriptionLength++;
        }
    }
}
