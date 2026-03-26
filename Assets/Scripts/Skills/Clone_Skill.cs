using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Clone_Skill : Skill
{



    [Header("Clone info")]
    [SerializeField]private float attackMultiplier;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]


    [Header("Clone attack")]
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMutiplier;
    [SerializeField] private bool canAttack;

    [Header("Aggresive clone")]
    [SerializeField]private UI_SkillTreeSlot aggresiveCloneUnlockButton;
    [SerializeField]private float aggresiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect { get; private set; }


    [Header("Multiple clone")]
    [SerializeField]private UI_SkillTreeSlot multipleCloneUnlockButton;
    [SerializeField] private float multiCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Crystal instead of clone")]
    [SerializeField]private UI_SkillTreeSlot crystalInsteadUnlockButton;
    public bool crystalInseadOfClone;

    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggresiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveClone);
        multipleCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultipleClone);
        crystalInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInsteadOfClone);
    }

    #region Unlock region
    protected override void CheckUnlock()
    {
        UnlockCloneAttack();
        UnlockAggresiveClone();
        UnlockMultipleClone();
        UnlockCrystalInsteadOfClone();

    }

    private void UnlockCloneAttack()
    {
        if(cloneAttackUnlockButton.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMutiplier;
        }
    }
    private void UnlockAggresiveClone()
    {
        if(aggresiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggresiveCloneAttackMultiplier;
        }
    }
    private void UnlockMultipleClone()
    {
        if(multipleCloneUnlockButton.unlocked)
        {
            canDuplicateClone = true;
            attackMultiplier = multiCloneAttackMultiplier;
        }
    }

    private void UnlockCrystalInsteadOfClone()
    {
        if(crystalInsteadUnlockButton.unlocked)
        {
            crystalInseadOfClone = true;
        }
    }

    #endregion
    public void CreateClone(Transform _clonePosition,Vector3 _offset)
    {
        if(crystalInseadOfClone)
        {
            SkillManager.instance.crystal.CreatCrystal();
            //SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            return;
        }
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().
            SetupClone(_clonePosition,cloneDuration,canAttack,_offset,FindCloseEnemy(newClone.transform),canDuplicateClone,chanceToDuplicate,player,attackMultiplier);
    }


    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
     
         StartCoroutine(CloneDelayCorotine(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }
    private IEnumerator CloneDelayCorotine(Transform _transform,Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f); //—”≥Ÿ÷µ
            CreateClone(_transform,_offset);
    }
}
