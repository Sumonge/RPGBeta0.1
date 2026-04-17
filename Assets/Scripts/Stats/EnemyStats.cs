using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;
    public Stat spDropAmount;

    [Header("Level details")]
    [SerializeField] private int level;

    [Range(0f, 1f)]
    [SerializeField] private float percentageaModifer=.4f;

    protected override void Start()
    {
        spDropAmount.SetDefaultValue(10);


        ApplyLevelModfiers();

        base.Start();

        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();

    }

    private void ApplyLevelModfiers()
    {
        //适当修改，暴击率等出现问题需要后期修改
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);

        Modify(spDropAmount);//修改技能点掉落数量
    }

    private void Modify(Stat _stat)
    {
        for (int i = 0; i < level; i++)
        {
            float modfier = _stat.GetValue() * percentageaModifer;

            _stat.AddModifier(Mathf.RoundToInt(modfier));
        }
    }
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }
    protected override void Die()
    {
        base.Die();
        enemy.Die();

        PlayerManager.instance.currency += spDropAmount.GetValue();

        myDropSystem.GenerateDrop();

        Destroy(gameObject, 5f);
    }
}
