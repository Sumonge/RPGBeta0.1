using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class UI : MonoBehaviour,ISaveManager
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

    [SerializeField] private UI_VolumeSlider[] volumeSettings;

    private void Awake()
    {
        SwitchTo(skillTreeUI);//��Ҫ�ȴ򿪼��������棬������ȷ���ü����������ϼ���ͼ��� tooltip ������

        fadeScreen.gameObject.SetActive(true);
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
            // �ر����в˵�
            SwitchTo(null);

            // �������п��ܵ�����ʾ�� UI Ԫ�أ�tooltip / craft window��
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
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;//��Ҫ���ֵ��뵭�������ϵ�����UIԪ�ص���ʾ״̬����

            if (fadeScreen == false)
                transform.GetChild(i).gameObject.SetActive(false);
        }
        if (_menu != null)
        {
            // ���޸ġ�ֻ�е� AudioManager �����ҡ��������š�ʱ�Ų�����
            // ������ʹ Awake ������� SwitchTo������ canPlaySFX ���� false�������ᱻ����
            if (AudioManager.instance != null && AudioManager.instance.canPlaySFX)
            {
                AudioManager.instance.PlaySFX(27, null);
            }
            _menu.SetActive(true);
        }

        if(GameManager.instance!=null)
        {
            if(_menu ==inGameUI )
            {
                GameManager.instance.PauseGame(false);
            }
            else
            {
                GameManager.instance.PauseGame(true);
            }
        }


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
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
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

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, float> pair in _data.volumeSettings)
        {
            foreach (UI_VolumeSlider item in volumeSettings)
            {
                if (item.parametr == pair.Key)
                {
                    item.LoadSilder(pair.Value);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();
        foreach(UI_VolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parametr, item.slider.value);
        }
    }
}
