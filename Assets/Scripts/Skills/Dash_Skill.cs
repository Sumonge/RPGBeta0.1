using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill, ISaveManager
{
    [Header("Dash")]
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;
    public bool dashUnlocked { get; private set; }

    [Header("Clone On Dash")]
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockButton;
    public bool cloneOnDashUnlocked { get; private set; }

    [Header("Clone On arrival")]
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockButton;
    public bool cloneOnArrivalUnlocked { get; private set; }
    public override void UseSkill()
    {
        base.UseSkill();


    }

    protected override void Start()
    {
        base.Start();

        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(CloneOnDashUnlocked);
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(CloneOnArrivalUnlocked);//�պ��޸��¼�����
    }
    protected override void CheckUnlock()
    {
        UnlockDash();
        CloneOnDashUnlocked();
        CloneOnArrivalUnlocked();
    }

    private void UnlockDash()
    {


        if (dashUnlockButton.unlocked)
        {

            dashUnlocked = true;

        }
    }
    private void CloneOnDashUnlocked()
    {
        if (cloneOnDashUnlockButton.unlocked)
            cloneOnDashUnlocked = true;
    }
    private void CloneOnArrivalUnlocked()
    {
        if (cloneOnArrivalUnlockButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }
    public void CloneOnDash()
    {
        if (cloneOnDashUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }
    public void CloneOnArrival()
    {
        if (cloneOnArrivalUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
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
