using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill, ISaveManager
{
    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked { get; private set; }

    [Header("Parry restore")]
    [SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
    [Range(0f,1f)]
    [SerializeField]private float restoreHealthPercentage;
    public bool restoreUnlocked { get; private set; }

    [Header("Parry with mirage")]
    [SerializeField]private UI_SkillTreeSlot mirageUnlockButton;
    public bool mirageUnlocked { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();

        if (restoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthPercentage);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
          
       
    }
    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockedParryRestore);
        mirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockedParryMirage);
    }
    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockedParryRestore();
        UnlockedParryMirage();
    }

    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
            parryUnlocked = true;
    }
    private void UnlockedParryRestore()
    {
        if(restoreUnlockButton.unlocked)
            restoreUnlocked = true;
    }
    private void UnlockedParryMirage()
    {
        if(mirageUnlockButton.unlocked)
            mirageUnlocked = true;
    }

    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if(mirageUnlocked)
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
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
