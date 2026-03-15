using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionUI;


    public UI_SkillToolTip skillToolTip;
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;
    void Start()
    {
        SwitchTo(null);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SwitchWithKeyTo(characterUI);

        if (Input.GetKeyDown(KeyCode.B))
            SwitchWithKeyTo(craftUI);

        if (Input.GetKeyDown(KeyCode.K))
            SwitchWithKeyTo(skillTreeUI);

        if (Input.GetKeyDown(KeyCode.O))
            SwitchWithKeyTo(optionUI);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 关闭所有菜单
            SwitchTo(null);

            // 隐藏所有可能单独显示的 UI 元素（tooltip / craft window）
            if (itemToolTip != null)
                itemToolTip.gameObject.SetActive(false);

            if (statToolTip != null)
                statToolTip.gameObject.SetActive(false);

            if (skillToolTip != null)
                skillToolTip.gameObject.SetActive(false);

            if (craftWindow != null)
                craftWindow.gameObject.SetActive(false);
        }
    }

    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        if (_menu != null)
            _menu.SetActive(true);


    }
    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            return;
        }
        SwitchTo(_menu);
    }

}
