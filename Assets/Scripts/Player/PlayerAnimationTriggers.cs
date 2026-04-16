using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player=>GetComponentInParent<Player>();
    
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }
    private void AttackTrigger()
    {
        AudioManager.instance.PlaySFX(1,null);//攻击音效

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target=hit.GetComponent<EnemyStats>();

                if (_target != null) 
                    player.stats.DoDamage(_target);

                //调用物品效果可以
                ItemData_Equipment weaponData = Inventory.instance.GetEquipmentType(EquipmentType.Wepon);
                if (weaponData != null)
                    weaponData.Effect(_target.transform);

            }
        }
    }



    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
