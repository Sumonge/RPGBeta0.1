using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField]private string sceneName="MainScene";
    [SerializeField]private GameObject continueButton;
    [SerializeField] UI_FadeScreen fadeScreen;

    private void Start()
    {
        // 如果有存档数据，显示继续按钮
        if(SaveManager.instance.HasNoSaveData() == false)
            continueButton.SetActive(true);
        else
            continueButton.SetActive(false);

    }

    public void ContinueGame()
    {
        // 确保加载保存的游戏数据
        SaveManager.instance.LoadGame();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f)); // 1��ĵ�������ʱ��
    }
    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f)); // 1��ĵ�������ʱ��

    }
    public void ExitGame()
    {
        Debug.Log("�˳���Ϸ");
    }

    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay); // �ȴ������������
        SceneManager.LoadScene(sceneName);
    }
}
