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
        stats.IncreaseStatBy(buffAmount, buffDuration, stats.GetStat(buffType));
       // PlayerManager.instance.player.StartCoroutine(RemoveBuffAfterDuration());
    }   

}
