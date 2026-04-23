using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour,ISaveManager
{
    public static GameManager instance;

    private Transform player;


    [SerializeField]private Checkpoint[] checkpoints;

    [Header("Lost currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
        {
            instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        checkpoints = FindObjectsOfType<Checkpoint>();
        if(PlayerManager.instance != null && PlayerManager.instance.player != null)
            player = PlayerManager.instance.player.transform;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
            RestartGame();
    }

    public void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        try
        {
            LoadLostCurrency(_data);
            LoadCheckPoints(_data);

            // 检查checkpoints是否已初始化
            if (checkpoints == null || checkpoints.Length == 0)
            {
                checkpoints = FindObjectsOfType<Checkpoint>();
            }

            // 查找最近的检查点并设置玩家位置
            if (!string.IsNullOrEmpty(_data.closestCheckpointId))
            {
                foreach (Checkpoint checkpoint in checkpoints)
                {
                    if (checkpoint != null && _data.closestCheckpointId == checkpoint.id)
                    {
                        // 检查PlayerManager和player引用
                        if (PlayerManager.instance != null && PlayerManager.instance.player != null)
                        {
                            Vector3 spawnPosition = new Vector3(checkpoint.transform.position.x, checkpoint.transform.position.y + 1.5f, checkpoint.transform.position.z);
                            PlayerManager.instance.player.transform.position = spawnPosition;
                        }
                        else
                        {
                        }
                        break;
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("加载游戏数据失败: " + e.Message);
        }
    }

    private void LoadCheckPoints(GameData _data)
    {
        try
        {
            // 确保checkpoints已初始化
            if (checkpoints == null || checkpoints.Length == 0)
            {
                checkpoints = FindObjectsOfType<Checkpoint>();
            }

            foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
            {
                foreach (Checkpoint checkpoint in checkpoints)
                {
                    if (checkpoint != null && checkpoint.id == pair.Key && pair.Value == true)
                    {
                        checkpoint.ActivateCheckpoint();
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("加载检查点数据失败: " + e.Message);
        }
    }

    private void LoadLostCurrency(GameData _data)
    {
        try
        {
            lostCurrencyAmount = _data.lostCurrencyAmount;
            lostCurrencyX = _data.lostCurrencyX;
            lostCurrencyY = _data.lostCurrencyY;

            if(lostCurrencyAmount>0)
            {
                if (lostCurrencyPrefab != null)
                {
                    GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY, 0), Quaternion.identity);
                    LostCurrencyController controller = newLostCurrency.GetComponent<LostCurrencyController>();
                    if (controller != null)
                    {
                        controller.currency = lostCurrencyAmount;
                    }
                }
                else
                {
                    Debug.LogWarning("lostCurrencyPrefab为空，无法生成丢失货币");
                }
            }

            lostCurrencyAmount = 0;
        }
        catch (System.Exception e)
        {
            Debug.LogError("加载丢失货币数据失败: " + e.Message);
        }
    }

    public void SaveData(ref GameData _data)
    {
        try
        {
            _data.lostCurrencyAmount = lostCurrencyAmount;

            // 检查player引用
            if (player != null)
            {
                _data.lostCurrencyX = player.position.x;
                _data.lostCurrencyY = player.position.y;
            }
            else
            {
                _data.lostCurrencyX = 0;
                _data.lostCurrencyY = 0;
                Debug.LogWarning("player引用为空，无法保存丢失货币位置");
            }

            Debug.Log("已保存丢失货币: " + lostCurrencyAmount);

            // 确保checkpoints已初始化
            if (checkpoints == null || checkpoints.Length == 0)
            {
                checkpoints = FindObjectsOfType<Checkpoint>();
            }

            Checkpoint closest = FindClosestCheckpoint();
            if(closest != null)
                _data.closestCheckpointId = closest.id;
            else
                _data.closestCheckpointId = string.Empty;

            _data.checkpoints.Clear();

            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint != null)
                {
                    _data.checkpoints.Add(checkpoint.id, checkpoint.activationStats);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("保存游戏数据失败: " + e.Message);
        }
    }

    private Checkpoint FindClosestCheckpoint()
    {
        try
        {
            // 检查必要引用
            if (player == null)
            {
                Debug.LogWarning("player引用为空，无法查找最近检查点");
                return null;
            }

            // 确保checkpoints已初始化
            if (checkpoints == null || checkpoints.Length == 0)
            {
                checkpoints = FindObjectsOfType<Checkpoint>();
                if (checkpoints.Length == 0)
                    return null;
            }

            float closesDistance = Mathf.Infinity;
            Checkpoint closestCheckpoint = null;

            foreach (var checkpoint in checkpoints)
            {
                if (checkpoint != null && checkpoint.activationStats == true)
                {
                    float distanceToCheckpoint = Vector2.Distance(player.position, checkpoint.transform.position);
                    if (distanceToCheckpoint < closesDistance)
                    {
                        closesDistance = distanceToCheckpoint;
                        closestCheckpoint = checkpoint;
                    }
                }
            }
            return closestCheckpoint;
        }
        catch (System.Exception e)
        {
            Debug.LogError("查找最近检查点失败: " + e.Message);
            return null;
        }
    }

    public void PauseGame(bool _pause)
    {
        if (_pause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
