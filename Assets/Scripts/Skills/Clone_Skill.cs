using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Clone_Skill : Skill
{



    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [SerializeField] private bool canAttack;




    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Crystal instead of clone")]
    public bool crystalInseadofClone;

    public void CreateClone(Transform _clonePosition,Vector3 _offset)
    {
        if(crystalInseadofClone)
        {
            SkillManager.instance.crystal.CreatCrystal();
            SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            return;
        }
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().
            SetupClone(_clonePosition,cloneDuration,canAttack,_offset,FindCloseEnemy(newClone.transform),canDuplicateClone,chanceToDuplicate,player);
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
