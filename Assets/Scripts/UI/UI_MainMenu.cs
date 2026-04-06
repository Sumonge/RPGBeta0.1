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
        if(SaveManager.instance.HasNoSaveData()==false)
            continueButton.SetActive (false);
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceneWithFadeEffect(1.5f)); // 1秒的淡出动画时间
    }
    public void NewGame()
    {
        SaveManager.instance.DeletSaveData();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f)); // 1秒的淡出动画时间

    }
    public void ExitGame()
    {
        Debug.Log("退出游戏");
    }

    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay); // 等待淡出动画完成
        SceneManager.LoadScene(sceneName);
    }
}
