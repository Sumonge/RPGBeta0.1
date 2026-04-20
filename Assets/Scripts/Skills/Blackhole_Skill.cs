using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackhole_Skill : Skill, ISaveManager
{
    [SerializeField] private UI_SkillTreeSlot blackholeUnlockButton;
    public bool blackholeUnlocked { get; private set; }
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float blackholeDuration;
    [Space]

    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shinkSpeed;

    Blackhole_Skill_Controller currentBlackhole;
    private void UnlockBlackhole()
    {
        if (blackholeUnlockButton.unlocked)
            blackholeUnlocked = true;
    }
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);

        currentBlackhole = newBlackHole.GetComponent<Blackhole_Skill_Controller>();

        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shinkSpeed, amountOfAttacks, cloneCooldown, blackholeDuration);

        AudioManager.instance.PlaySFX(5, null);
    }

    protected override void Start()
    {
        base.Start();

        blackholeUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackhole);
    }

    protected override void Update()
    {
        base.Update();
    }
    public bool SkillCompleted()
    {
        if (!currentBlackhole)
            return false;

        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }
        return false;
    }

    public float GetBlackholeRadius()
    {
        return maxSize / 2;


    }
    protected override void CheckUnlock()
    {
        base.CheckUnlock();

        UnlockBlackhole();
    }

    public void LoadData(GameData _data)
    {
        // 数据加载后重新检查解锁状态，延迟一帧确保UI_SkillTreeSlot已加载
        StartCoroutine(DelayedCheckUnlock());
    }

    private IEnumerator DelayedCheckUnlock()
    {
        yield return null; // 等待一帧
        CheckUnlock();
    }

    public void SaveData(ref GameData _data)
    {
        // 技能数据通过UI_SkillTreeSlot保存，此处无需操作
    }
}
