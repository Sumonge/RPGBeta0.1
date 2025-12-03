using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Health effect", menuName = "Date/Item effect/Health effect")]

public class HealthEffect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healthPercent;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats=PlayerManager.instance.player.GetComponent<PlayerStats>();

        int healAmount=Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healthPercent);

        playerStats.IncreaseHealthBy(healAmount);
    }
}
