using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;

    [SerializeField]private string skillName;
    [TextArea]
    [SerializeField]private string skillDescription;
    [SerializeField] private Color lockedSkillColor;


    public bool unlocked;

    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;

    private Image skillImage;

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }

    private void Start()
    {
        skillImage=GetComponent<Image>();

        ui = GetComponentInParent<UI>();

        skillImage.color = lockedSkillColor;

        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    public void UnlockSkillSlot()
    {
        //逐级解锁
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                Debug.Log("无法解锁技能");
                return;
            }
               
        }
        //分支解锁
        //或者添加重置技能树的功能，重置后所有技能都未解锁，玩家可以重新选择解锁路径
        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                Debug.Log("无法解锁技能");
                return;
            }
        }

        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       ui.skillToolTip.ShowToolTip(skillDescription,skillName);

        Vector2 mousePosition=Input.mousePosition;

        float XOffset = 0;
        float YOffset = 0;

        if (mousePosition.x>600)
            XOffset=-150;
        else
            XOffset=150;

        if (mousePosition.y>320)
            YOffset=-150;
        else
            YOffset=150;

        ui.skillToolTip.transform.position = new Vector2(mousePosition.x+XOffset,mousePosition.y+ YOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       ui.skillToolTip.HideToolTip();
    }
}
