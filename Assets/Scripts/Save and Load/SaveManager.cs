using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;

    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;

    [ContextMenu("Delet save file")]
    public void DeletSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName,encryptData);
        dataHandler.Delete();

    }

    private void Awake()
    {

                //脚本运行顺序问题解决尝试，或者在inventory添加协程解决，保证此段早于add初始装备生成
        //哦我的老天，如果这个东西再早checkpoint就加载不出来了，晚了inventory就加载不出来了
        //直接在unity项目gamemanager先于这两个大麻烦启动
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);

        saveManagers = FindAllSaveManagers();

        LoadGame();

        //*********************************************************************************


        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
        if (dataHandler == null)
            dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
    }

    private void Start()
    {

    }

    public void NewGame()
    {
        gameData=new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("无保存数据，初始化新游戏数据");
            NewGame();
        }

        // 之前这里的 foreach 可能会因为 saveManagers 列表不全而失效
        // 我们保留它用于更新当前场景已存在的物体
        RefreshAllSaveManagers();
    }
    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);

        
    }
    // 核心：新增一个公开方法，让单个物体可以随时加入并立刻加载数据
    public void RegisterSaveManager(ISaveManager saveManager)
    {
        if (!saveManagers.Contains(saveManager))
        {
            saveManagers.Add(saveManager);
        }

        // 如果数据已经准备好了，立刻传给它！
        if (gameData != null)
        {
            saveManager.LoadData(gameData);
        }
    }

    private void RefreshAllSaveManagers()
    {
        saveManagers = FindAllSaveManagers();
        foreach (ISaveManager sm in saveManagers)
        {
            sm.LoadData(gameData);
        }
    }

    private void OnApplicationQuit()
    {
       

        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }
    public bool HasNoSaveData()
    {
        // 双重保险：即使 Awake 还没跑完（极端情况），这里也能自愈
        if (dataHandler == null) return true;

        GameData temp = dataHandler.Load();
        return temp == null;
    }
}

