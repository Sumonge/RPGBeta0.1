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
        if(SaveManager.instance != null && SaveManager.instance.HasNoSaveData() == false)
            continueButton.SetActive(true);
        else
            continueButton.SetActive(false);

    }

    public void ContinueGame()
    {
        // 确保加载保存的游戏数据
        SaveManager.instance.LoadGame();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }
    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));

    }
    public void ExitGame()
    {
        Debug.Log("退出游戏");
    }

    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();

        // 使用 scaled time 确保计时器正常工作
        float timer = _delay;
        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}
