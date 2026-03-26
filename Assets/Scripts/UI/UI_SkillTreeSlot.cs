using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler,ISaveManager
{
    private UI ui;

    [SerializeField]private int skillPrice;
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
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }
    private void Start()
    {
        skillImage=GetComponent<Image>();

        ui = GetComponentInParent<UI>();

        skillImage.color = lockedSkillColor;

        if(unlocked)
            skillImage.color=Color.white;
        
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
        if (PlayerManager.instance.HaveEnoughSkillPoint(skillPrice) == false)
            return;

        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription, skillName);

        Vector2 mousePosition = Input.mousePosition;

        // 尝试基于 Canvas 做本地坐标定位并夹紧到画布内，适配不同分辨率与画布缩放模式
        RectTransform tooltipRT = ui.skillToolTip.GetComponent<RectTransform>();
        Canvas canvas = ui.skillToolTip.GetComponentInParent<Canvas>();
        if (canvas != null && tooltipRT != null)
        {
            RectTransform canvasRT = canvas.GetComponent<RectTransform>();
            Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, mousePosition, cam, out localPoint);

            // 推荐偏移，可根据需求调整或基于 tooltip 大小计算
            Vector2 offset = new Vector2(150f, 150f);

            Vector2 desired = localPoint + offset;

            // 计算可用最小/最大值以防 tooltip 超出画布边界
            Vector2 tooltipSize = tooltipRT.rect.size;
            Vector2 min = canvasRT.rect.min + (tooltipSize * tooltipRT.pivot);
            Vector2 max = canvasRT.rect.max - (tooltipSize * (Vector2.one - tooltipRT.pivot));

            // Vector2 没有 Clamp 静态方法，按分量使用 Mathf.Clamp 进行夹紧
            desired = new Vector2(
                Mathf.Clamp(desired.x, min.x, max.x),
                Mathf.Clamp(desired.y, min.y, max.y)
            );

            tooltipRT.anchoredPosition = desired;
        }
        else
        {
            // 回退到屏幕坐标的简单逻辑（保持原有行为）
            float XOffset = 0;
            float YOffset = 0;

            if (mousePosition.x > 600)
                XOffset = -150;
            else
                XOffset = 150;

            if (mousePosition.y > 320)
                YOffset = -150;
            else
                YOffset = 150;

            ui.skillToolTip.transform.position = new Vector2(mousePosition.x + XOffset, mousePosition.y + YOffset);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       ui.skillToolTip.HideToolTip();
    }

    public void LoadData(GameData _data)
    {
        if(_data.skillTree.TryGetValue(skillName,out bool value))
        {
            unlocked = value;
        }
    }

    public void SaveData(ref GameData _data)
    {
        if(_data.skillTree.TryGetValue(skillName,out bool value))
        {
            _data.skillTree.Remove(skillName);
            _data.skillTree.Add(skillName, unlocked);
        }
        else
        {
            _data.skillTree.Add(skillName,unlocked);
        }
    }
}
