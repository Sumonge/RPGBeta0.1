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
            instance = this;
    }
    private void Start()
    {
        checkpoints = FindObjectsOfType<Checkpoint>();
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
        LoadLostCurrency(_data);

        LoadCheckPoints(_data);

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (_data.closestCheckpointId == checkpoint.id)
            {

                // 向上偏移 1.5 个单位，防止卡进地里
                Vector3 spawnPosition = new Vector3(checkpoint.transform.position.x, checkpoint.transform.position.y + 1.5f, checkpoint.transform.position.z);
                PlayerManager.instance.player.transform.position = spawnPosition;
                //修改前代码，出现问题回溯PlayerManager.instance.player.transform.position = checkpoint.transform.position;

            }
        }

    }

    // 3. 协程保持不带参数


    private void LoadCheckPoints(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.id == pair.Key && pair.Value == true)
                {
                    checkpoint.ActivateCheckpoint();
                }
            }
        }
    }

    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;

        if(lostCurrencyAmount>0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY, 0), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().currency=lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }

    public void SaveData(ref GameData _data)
    {
        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyX=player.position.x;
        _data.lostCurrencyY=player.position.y;
        Debug.Log("已保存点数" + lostCurrencyAmount);

        if(FindClosestCheckpoint()!=null)
            _data.closestCheckpointId = FindClosestCheckpoint().id;

        _data.checkpoints.Clear();

        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.id, checkpoint.activationStats);
        }
    }

    private Checkpoint FindClosestCheckpoint()
    {
        float closesDistance=Mathf.Infinity;
        Checkpoint closestCheckpoint=null;
        foreach(var checkpoint in checkpoints)
        {
            float distanceToCheckpoint=Vector2.Distance(player.position,checkpoint.transform.position);
            if(distanceToCheckpoint<closesDistance&&checkpoint.activationStats==true)
            {
                closesDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }
        return closestCheckpoint;
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
