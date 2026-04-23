using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;

    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;

    [ContextMenu("Delete save file")]
    public void DeleteSaveData()
    {
        try
        {
            // 确保dataHandler已初始化
            if (dataHandler == null)
            {
                dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
            }

            dataHandler.Delete();

            // 重置内存中的游戏数据
            gameData = new GameData();

            // 重新加载空数据到所有管理器
            if (saveManagers != null && saveManagers.Count > 0)
            {
                foreach (ISaveManager saveManager in saveManagers)
                {
                    try
                    {
                        saveManager.LoadData(gameData);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("重置管理器数据失败: " + e.Message);
                    }
                }
            }

            Debug.Log("保存文件已删除，游戏数据已重置");
        }
        catch (System.Exception e)
        {
            Debug.LogError("删除保存文件失败: " + e.Message);
        }
    }

    private void Awake()
    {
        // 单例模式：先检查并设置实例，避免重复
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        // 初始化数据处理器
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);

        // 订阅场景加载事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // 取消订阅场景加载事件
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"场景加载完成: {scene.name}, 刷新保存管理器列表");

        // 延迟一帧，确保所有组件已完成初始化
        StartCoroutine(RefreshAfterSceneLoad());
    }

    private IEnumerator RefreshAfterSceneLoad()
    {
        // 等待一帧，确保所有组件的Awake和Start方法已完成
        yield return null;

        // 重新查找所有保存管理器
        saveManagers = FindAllSaveManagers();
        Debug.Log($"场景加载后找到 {saveManagers.Count} 个保存管理器");

        // 如果已经有游戏数据，重新加载到新场景的管理器中
        if (gameData != null)
        {
            Debug.Log("重新加载游戏数据到新场景的管理器");
            foreach (ISaveManager saveManager in saveManagers)
            {
                try
                {
                    saveManager.LoadData(gameData);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"重新加载数据到管理器失败: {e.Message}");
                }
            }
        }
        else
        {
            // 只有在非主菜单场景且确实需要加载存档时才加载
            string currentScene = SceneManager.GetActiveScene().name;
            if (!currentScene.Contains("Menu") && !currentScene.Contains("Main"))
            {
                LoadGame();
            }
            // 主菜单场景下 gameData 为空是正常行为，不输出警告
        }
    }

    private void Start()
    {
        // 在Start中初始化，但延迟一帧确保所有组件已准备就绪
        StartCoroutine(InitializeAfterDelay());
    }

    private IEnumerator InitializeAfterDelay()
    {
        // 等待一帧，确保所有组件的Awake和Start方法已完成
        yield return null;

        try
        {
            saveManagers = FindAllSaveManagers();
            Debug.Log("存档管理器初始化，找到 " + saveManagers.Count + " 个保存组件");

            // 加载游戏数据（如果是第一次启动）
            if (gameData == null)
            {
                LoadGame();
            }
            else
            {
                // 如果已经有游戏数据（比如从主菜单继续游戏），重新加载到管理器中
                Debug.Log("已有游戏数据，重新加载到管理器中");
                foreach (ISaveManager saveManager in saveManagers)
                {
                    try
                    {
                        saveManager.LoadData(gameData);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"加载数据到管理器失败: {e.Message}");
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("存档管理器初始化失败: " + e.Message);
            // 即使初始化失败，也创建空的保存列表
            if (saveManagers == null)
                saveManagers = new List<ISaveManager>();
        }
    }

    public void NewGame()
    {
        gameData=new GameData();
    }

    public void LoadGame()
    {
        try
        {
            GameData loadedData = dataHandler.Load();

            if (loadedData == null)
            {
                // 文件加载失败或无保存数据
                if (gameData == null)
                {
                    Debug.Log("无保存数据，开始新游戏");
                    NewGame();
                }
                else
                {
                    Debug.LogWarning("文件加载失败，但内存中已有游戏数据，保留现有数据");
                    // 保留现有的gameData
                }
            }
            else
            {
                gameData = loadedData;
                Debug.Log("游戏数据加载成功");
            }

            // 确保saveManagers列表已初始化
            if (saveManagers == null || saveManagers.Count == 0)
            {
                saveManagers = FindAllSaveManagers();
            }

            // 加载数据到所有管理器
            foreach (ISaveManager saveManager in saveManagers)
            {
                try
                {
                    saveManager.LoadData(gameData);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("加载数据到管理器失败: " + e.Message);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("加载游戏失败: " + e.Message);
            // 加载失败时，如果内存中没有数据，创建新游戏
            if (gameData == null)
                gameData = new GameData();
        }
    }
    public void SaveGame()
    {
        try
        {
            // 确保有游戏数据
            if (gameData == null)
            {
                gameData = new GameData();
            }

            // 确保saveManagers列表已初始化
            if (saveManagers == null || saveManagers.Count == 0)
            {
                saveManagers = FindAllSaveManagers();
            }

            // 收集所有管理器的数据
            foreach (ISaveManager saveManager in saveManagers)
            {
                try
                {
                    saveManager.SaveData(ref gameData);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("保存管理器数据失败: " + e.Message);
                }
            }

            // 保存到文件
            dataHandler.Save(gameData);
            Debug.Log("游戏数据保存成功");
        }
        catch (System.Exception e)
        {
            Debug.LogError("保存游戏失败: " + e.Message);
        }
    }
    public void RegisterSaveManager(ISaveManager saveManager)
    {
        if (saveManager == null)
        {
            Debug.LogError("尝试注册空的ISaveManager");
            return;
        }

        // 确保列表已初始化
        if (saveManagers == null)
        {
            saveManagers = new List<ISaveManager>();
        }

        if (!saveManagers.Contains(saveManager))
        {
            saveManagers.Add(saveManager);
            Debug.Log("注册新的保存管理器: " + saveManager.GetType().Name);
        }

        // 如果数据已经加载，立即初始化新注册的管理器
        if (gameData != null)
        {
            try
            {
                saveManager.LoadData(gameData);
            }
            catch (System.Exception e)
            {
                Debug.LogError("初始化新注册管理器失败: " + e.Message);
            }
        }
    }

    private void RefreshAllSaveManagers()
    {
        try
        {
            // 重新查找所有管理器（用于场景加载后）
            saveManagers = FindAllSaveManagers();
            Debug.Log("刷新保存管理器列表，找到 " + saveManagers.Count + " 个组件");

            if (gameData != null)
            {
                foreach (ISaveManager sm in saveManagers)
                {
                    try
                    {
                        sm.LoadData(gameData);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("刷新管理器数据失败: " + e.Message);
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("刷新保存管理器失败: " + e.Message);
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("应用程序退出，保存游戏数据...");
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        try
        {
            // 使用 FindObjectsOfType 并包含非活动对象，确保能找到所有 ISaveManager
            MonoBehaviour[] allBehaviours = FindObjectsOfType<MonoBehaviour>(true);
            List<ISaveManager> foundManagers = new List<ISaveManager>();

            foreach (MonoBehaviour behaviour in allBehaviours)
            {
                if (behaviour is ISaveManager saveManager)
                {
                    foundManagers.Add(saveManager);
                }
            }

            Debug.Log($"找到 {foundManagers.Count} 个保存管理器（包含非活动对象）");
            return foundManagers;
        }
        catch (System.Exception e)
        {
            Debug.LogError("查找保存管理器失败: " + e.Message);
            return new List<ISaveManager>();
        }
    }
    public bool HasNoSaveData()
    {
        try
        {
            // 双重保险，即使Awake没执行（比如编辑器模式下直接调用）也能工作
            if (dataHandler == null)
            {
                // 尝试初始化dataHandler
                dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
            }

            GameData temp = dataHandler.Load();
            return temp == null;
        }
        catch (System.Exception e)
        {
            Debug.LogError("检查保存数据失败: " + e.Message);
            // 出现异常时假定无保存数据
            return true;
        }
    }
}

