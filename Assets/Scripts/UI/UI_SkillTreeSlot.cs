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
        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();

        // 初始化颜色
        UpdateSkillColor();
    }

    public void UnlockSkillSlot()
    {
        
        //�𼶽���
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                Debug.Log("�޷���������");
                return;
            }
               
        }
        //��֧����
        //�����������ü������Ĺ��ܣ����ú����м��ܶ�δ��������ҿ�������ѡ�����·��
        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                Debug.Log("�޷���������");
                return;
            }
        }
        if (PlayerManager.instance.HaveEnoughSkillPoint(skillPrice) == false)
            return;

        unlocked = true;
        UpdateSkillColor();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //ui.skillToolTip.ShowToolTip(skillDescription, skillName);

        Vector2 mousePosition = Input.mousePosition;

        // ���Ի��� Canvas ���������궨λ���н��������ڣ����䲻ͬ�ֱ����뻭������ģʽ
        RectTransform tooltipRT = ui.skillToolTip.GetComponent<RectTransform>();
        Canvas canvas = ui.skillToolTip.GetComponentInParent<Canvas>();
        if (canvas != null && tooltipRT != null)
        {
            RectTransform canvasRT = canvas.GetComponent<RectTransform>();
            Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, mousePosition, cam, out localPoint);

            // �Ƽ�ƫ�ƣ��ɸ��������������� tooltip ��С����
            Vector2 offset = new Vector2(150f, 150f);

            Vector2 desired = localPoint + offset;

            // ���������С/���ֵ�Է� tooltip ���������߽�
            Vector2 tooltipSize = tooltipRT.rect.size;
            Vector2 min = canvasRT.rect.min + (tooltipSize * tooltipRT.pivot);
            Vector2 max = canvasRT.rect.max - (tooltipSize * (Vector2.one - tooltipRT.pivot));

            // Vector2 û�� Clamp ��̬������������ʹ�� Mathf.Clamp ���мн�
            desired = new Vector2(
                Mathf.Clamp(desired.x, min.x, max.x),
                Mathf.Clamp(desired.y, min.y, max.y)
            );

            tooltipRT.anchoredPosition = desired;
        }
        else
        {
            // ���˵���Ļ����ļ��߼�������ԭ����Ϊ��
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

    private void UpdateSkillColor()
    {
        // 确保skillImage已初始化
        if (skillImage == null)
            skillImage = GetComponent<Image>();

        if (skillImage != null)
        {
            skillImage.color = unlocked ? Color.white : lockedSkillColor;
        }
    }

    public void LoadData(GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
            UpdateSkillColor();
        }
        else
        {
        }
    }

    public void SaveData(ref GameData _data)
    {
        if (string.IsNullOrEmpty(skillName))
        {
            Debug.LogError("技能名称为空，无法保存技能数据");
            return;
        }

        // 直接设置或更新技能状态
        if (_data.skillTree.ContainsKey(skillName))
        {
            _data.skillTree[skillName] = unlocked;
        }
        else
        {
            _data.skillTree.Add(skillName, unlocked);
        }

    }
}
