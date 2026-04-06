using System.Collections;
using UnityEngine;

public class UI : MonoBehaviour
{
    [Header("End screen")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endScreenText;
    [SerializeField] private GameObject restartButton;
    [Space]

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject inGameUI;


    public UI_SkillToolTip skillToolTip;
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;

    private void Awake()
    {
        SwitchTo(skillTreeUI);//需要先打开技能树界面，才能正确设置技能树界面上技能图标的 tooltip 的引用
    }
    void Start()
    {
       
        SwitchTo(inGameUI);

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
            bool fadeScreen=transform.GetChild(i).GetComponent<UI_FadeScreen>()!=null;//需要保持淡入淡出界面上的其他UI元素的显示状态不变

            if(fadeScreen==false)
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
            CheckForInGameUI();

            return;
        }
        SwitchTo(_menu);
    
    }

    private void CheckForInGameUI()
    {
        for(int i=0;i<transform.childCount;i++)
        {
             if (transform.GetChild(i).gameObject.activeSelf)
                return;
        }
        SwitchTo(inGameUI);
    }

    public void SwitchOnEndScreen()
    {
       
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCorutione());

    }
    IEnumerator EndScreenCorutione()
    {
        yield return new WaitForSeconds(1.5f);
        endScreenText.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
        //SwitchTo(null);
    }
    public void RestartGameButton()
    {
        GameManager.instance.RestartGame();
    }

}
