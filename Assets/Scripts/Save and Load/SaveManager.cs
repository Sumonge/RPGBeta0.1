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
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName,encryptData);

        saveManagers = FindAllSaveManagers();

        LoadGame();

        //*********************************************************************************
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
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

        if(this.gameData==null)
        {
            Debug.Log("无保存数据");
            NewGame();
        }
        
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }

       
    }
    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);

        
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
        if(dataHandler.Load()!=null)
        {
            return true;
        }
        return false;
    }
}

