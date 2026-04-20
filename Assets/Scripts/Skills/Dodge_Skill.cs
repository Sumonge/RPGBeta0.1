using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill, ISaveManager
{
    [Header("Dodge")]
    [SerializeField] UI_SkillTreeSlot unlockeDodgeButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked;

    [Header("Mirage Dodge")]
    [SerializeField]UI_SkillTreeSlot unlockMirageDodgeButton;
    public bool dodgeMirageUnlocked;

    protected override void Start()
    {
        base.Start();

        unlockeDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }

    protected override void CheckUnlock()
    {
        UnlockDodge();
        UnlockMirageDodge();
    }
    private void UnlockDodge()
    {
        if (dodgeUnlocked) return; // 已经解锁，避免重复添加修饰符

        if(unlockeDodgeButton.unlocked)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }

    private void UnlockMirageDodge()
    {
        if (unlockMirageDodgeButton.unlocked)
            dodgeMirageUnlocked = true;
    }
    public void CreatMirageOnDodge()
    {
        if (dodgeMirageUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform,new Vector3(2*player.facingDir,0));
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
