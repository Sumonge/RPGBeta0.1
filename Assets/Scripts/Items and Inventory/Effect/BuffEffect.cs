using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    strength,
        agility,
        intelligence,
        vitality,
        damage,
        critChance,
        critPower,
        health,
        armor,
        evasion,
        magicRes,
        fireDamage,
        iceDamage,
        lightDamage

}

[CreateAssetMenu(fileName = "Buff effect", menuName = "Date/Item effect/Buff effect")]

public class BuffEffect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField]private StatType buffType;
    [SerializeField]private int buffAmount;
    [SerializeField]private int buffDuration;//in seconds

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        stats=PlayerManager.instance.player.GetComponent<PlayerStats>();
        stats.IncreaseStatBy(buffAmount, buffDuration, StartToModify());
       // PlayerManager.instance.player.StartCoroutine(RemoveBuffAfterDuration());
    }   
    private Stat StartToModify()
    {
        switch (buffType)
        {
            case StatType.strength:
                return stats.strength;
            case StatType.agility:
                return stats.agility;
            case StatType.intelligence:
                return stats.intelligence;
            case StatType.vitality:
                return stats.vitality;
            case StatType.damage:
                return stats.damage;
            case StatType.critChance:
                return stats.critChance;
            case StatType.critPower:
                return stats.critPower;
            case StatType.health:
                return stats.maxHealth;
            case StatType.armor:
                return stats.armor;
            case StatType.evasion:
                return stats.evasion;
            case StatType.magicRes:
                return stats.magicResistance;
            case StatType.fireDamage:
                return stats.fireDamage;
            case StatType.iceDamage:
                return stats.iceDamage;
            case StatType.lightDamage:
                return stats.lightingDamage;
            default:
                return null;
        }
    }
}
