using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerStats : CharacterStats
{
    private Player player;
    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();

        player.Die();
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }
    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        ItemData_Equipment currentArmor=Inventory.instance.GetEquipmentType(EquipmentType.Armor);

        if (currentArmor != null)
            currentArmor.Effect(player.transform);
    }
    public override void OnEvasion()
    {
       player.skill.dodge.CreatMirageOnDodge();
    }

    public void CloneDodamege(CharacterStats _targetStats,float _multiplier)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();
        
        if(_multiplier>0)
            totalDamage=Mathf.RoundToInt(totalDamage*_multiplier);

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
        //DoMagicDamage(_targetStats);
        //如果武器附魔则造成魔法伤害
        DoMagicDamage(_targetStats);//移除火系伤害如果想的话要删掉这一行
    }
}
