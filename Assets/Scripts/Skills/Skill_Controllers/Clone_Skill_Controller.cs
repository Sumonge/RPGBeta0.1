using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLoosingSpeed;

    private float cloneTimer;
    private float attackMultiplier;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;
    private Transform closestEnemy;
    private bool canDuplicateClone;
    private int facingDir = 1;
    private float chanceToDuplicate;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));

            if (sr.color.a <= 0)
                Destroy(gameObject);
        }
    }

    public void SetupClone(Transform _newTransforem, float _cloneDuration, bool _canAttack, Vector3 _offset, Transform _closestEnemy, bool _canDuplicate, float _chanceToDuplicate, Player _player, float _attackMultiplier)
    {
        if (_canAttack)
        {
            anim.SetInteger("AttackNumber", UnityEngine.Random.Range(1, 3)); //��������
        }
        attackMultiplier = _attackMultiplier;
        player = _player;
        transform.position = _newTransforem.position + _offset;
        cloneTimer = _cloneDuration;

        canDuplicateClone = _canDuplicate;
        closestEnemy = _closestEnemy;
        chanceToDuplicate = _chanceToDuplicate;

        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //player.stats.DoDamage(hit.GetComponent<CharacterStats>());

                hit.GetComponent<Entity>().SetupKnockDir(transform);


                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

                playerStats.CloneDodamege(enemyStats, attackMultiplier);

                if (player.skill.clone.canApplyOnHitEffect)
                {
                    ItemData_Equipment weaponData = Inventory.instance.GetEquipmentType(EquipmentType.Wepon);
                    if (weaponData != null)
                        weaponData.Effect(hit.transform);
                }


                if (canDuplicateClone)
                {
                    if (UnityEngine.Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(.3f * facingDir, 0));
                    }
                }

            }
        }
    }
    private void FaceClosestTarget()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
        else if (player != null)
        {
            if (player.transform.position.x < transform.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
