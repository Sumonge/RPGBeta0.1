
using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")]
    public Stat strength;//�����˺�
    public Stat agility;//������
    public Stat intelligence;//����
    public Stat vitality;//ѩ������

    [Header("Offsive stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;//Ĭ��ֵΪ1.5

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;


    public bool isIgnited;//��ʱ����
    public bool isChilled;//����20�˺�
    public bool isShocked;//20�Ĺ���ʧ��

    [SerializeField] private float ailmentsDuration = 4;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;



    private float igniteCooldown = .3f;
    private float igniteDamageTimer;
    private int ignitedDamage;
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;


    public int currentHealth;

    public System.Action onHealthChanged;

    public bool isDead { get; private set; }
    public bool isInvincible   { get; private set; }
    private bool isVulnerable;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();


    }
    protected virtual void Update()
    {

        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;

        if (chilledTimer < 0)
            isChilled = false;
        if (shockedTimer < 0)
            isShocked = false;
        if (isIgnited)
            ApplyIgniteDamage();
    }
    public void MakeVulnerableFor(float _duration)
    {
        StartCoroutine(VulnerableCorutine(_duration));
    }

    private IEnumerator VulnerableCorutine(float _duration)
    {
        isVulnerable = true;
        yield return new WaitForSeconds(_duration);
        isVulnerable = false;
    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }
    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);


    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        bool criticalStrike = false;

        if (TargetCanAvoidAttack(_targetStats))
            return;

        _targetStats.GetComponent<Entity>().SetupKnockDir(transform);

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            criticalStrike = true;
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        fx.CreateHix(_targetStats.transform,criticalStrike);


        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
        //DoMagicDamage(_targetStats);
        //���������ħ�����ħ���˺�
        DoMagicDamage(_targetStats);//�Ƴ���ϵ�˺������Ļ�Ҫɾ����һ��
    }
    #region Magic and ailemnts
    public virtual void DoMagicDamage(CharacterStats _targetStates)
    {

        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightDamage = lightingDamage.GetValue();

        int totalMagicakDamage = _fireDamage + _iceDamage + _lightDamage + intelligence.GetValue();

        totalMagicakDamage = CheckTargetResistance(_targetStates, totalMagicakDamage);
        _targetStates.TakeDamage(totalMagicakDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightDamage) <= 0)
            return;


        bool flowControl = AttemptyToApplyAilements(_targetStates, _fireDamage, _iceDamage, _lightDamage);
        if (!flowControl)
        {
            return;
        }

    }

    private bool AttemptyToApplyAilements(CharacterStats _targetStates, int _fireDamage, int _iceDamage, int _lightDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightDamage;
        bool canApplyShock = _lightDamage > _iceDamage && _lightDamage > _fireDamage;

        while (!canApplyChill && !canApplyIgnite && !canApplyShock)
        {
            if (Random.value < .3f && _fireDamage > 0)
            {

                canApplyIgnite = true;
                _targetStates.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return false;
            }
            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStates.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return false;
            }
            if (Random.value < 1f && _lightDamage > 0)
            {
                canApplyShock = true;
                _targetStates.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return false;
            }
        }

        if (canApplyIgnite)
            _targetStates.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));

        if (canApplyShock)
            _targetStates.SetupShockStrikeDamage(Mathf.RoundToInt(_lightDamage * .1f));



        _targetStates.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
        return true;
    }



    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChilled = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;

            fx.IgniteFxFor(ailmentsDuration);
        }
        if (_chill && canApplyChilled)
        {
            isChilled = _chill;
            chilledTimer = ailmentsDuration;
            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);


            fx.ChillFxFor(ailmentsDuration);
        }
        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                    return;
                HitNearestTargetShockStrike();

            }

        }

    }

    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;

        isShocked = _shock;
        shockedTimer = ailmentsDuration;

        fx.SkockFxFor(ailmentsDuration);
    }

    private void HitNearestTargetShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }

            }
            if (closestEnemy == null)//�����㲻����Ŀ��
                closestEnemy = transform;
        }
        if (closestEnemy != null)
        {
            GameObject newShickStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

            newShickStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
        //�ҵ�����Ĵ�������ֻ�ڵ���֮��
        //��Ҫ��ʱ��
        //�������
    }
    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {
            DecreaseHealthBy(ignitedDamage);

            if (currentHealth < 0 && !isDead)
                Die();
            igniteDamageTimer = igniteCooldown;
        }
    }

    public void SetupIgniteDamage(int _damage) => ignitedDamage = _damage;

    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;

    #endregion


    public virtual void TakeDamage(int _damage)
    {
        if (isInvincible || isDead)
            return;
        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;

        if (currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();

        if (onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        if (isVulnerable)
            _damage = Mathf.RoundToInt(_damage * 1.1f);//����buff����10%���˺�

        currentHealth -= _damage;

        if(_damage>0)
            fx.CreatePopUpText(_damage.ToString());

        if (onHealthChanged != null)
            onHealthChanged();
    }
    protected virtual void Die()
    {
        isDead = true;
    }

    public void KillEntity()
    {
        if(!isDead)
            Die();
    }

    public void MakeInvincible(bool _invincible)
    {
        isInvincible = _invincible;
    }
    
    #region Stat calculations
    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else
            totalDamage -= _targetStats.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        //_targetStats.TakeDamage(totalDamage);
        return totalDamage;
    }
    private int CheckTargetResistance(CharacterStats _targetStates, int totalMagicakDamage)
    {
        totalMagicakDamage -= _targetStates.magicResistance.GetValue() + (_targetStates.intelligence.GetValue() * 3);
        totalMagicakDamage = Mathf.Clamp(totalMagicakDamage, 0, int.MaxValue);
        return totalMagicakDamage;
    }
    public virtual void OnEvasion()
    {

    }
    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();


        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }
        return false;
    }

    protected bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    protected int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        float critDamage = _damage * totalCritPower;
        return Mathf.RoundToInt(critDamage);
    }
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
    #endregion
    public Stat GetStat(StatType _statType)
    {
        switch (_statType)
        {
            case StatType.strength:
                return strength;
            case StatType.agility:
                return agility;
            case StatType.intelligence:
                return intelligence;
            case StatType.vitality:
                return vitality;
            case StatType.damage:
                return damage;
            case StatType.critChance:
                return critChance;
            case StatType.critPower:
                return critPower;
            case StatType.health:
                return maxHealth;
            case StatType.armor:
                return armor;
            case StatType.evasion:
                return evasion;
            case StatType.magicRes:
                return magicResistance;
            case StatType.fireDamage:
                return fireDamage;
            case StatType.iceDamage:
                return iceDamage;
            case StatType.lightDamage:
                return lightingDamage;
            default:
                return null;
        }
    }
}


