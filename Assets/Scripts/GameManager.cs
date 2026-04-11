using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour,ISaveManager
{
    public static GameManager instance;
    [SerializeField]private Checkpoint[] checkpoints;
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
       foreach(KeyValuePair<string,bool>pair in _data.checkpoints)
        {
            foreach(Checkpoint checkpoint in checkpoints)
            {
                if(checkpoint.id==pair.Key&&pair.Value==true)
                {
                     checkpoint.ActivateCheckpoint();
                }
            }
        }

       foreach(Checkpoint checkpoint in checkpoints)
        {
            if(_data.closestCheckpointId==checkpoint.id)
            {
                // 向上偏移 1.5 个单位，防止卡进地里
                Vector3 spawnPosition = new Vector3(checkpoint.transform.position.x, checkpoint.transform.position.y + 1.5f, checkpoint.transform.position.z);
                PlayerManager.instance.player.transform.position = spawnPosition;
                //修改前代码，出现问题回溯PlayerManager.instance.player.transform.position = checkpoint.transform.position;

            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.closestCheckpointId = FindClosesCheckpoubt().id;
        _data.checkpoints.Clear();

        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.id, checkpoint.activationStats);
        }
    }

    private Checkpoint FindClosesCheckpoubt()
    {
        float closesDistance=Mathf.Infinity;
        Checkpoint closestCheckpoint=null;
        foreach(var checkpoint in checkpoints)
        {
            float distanceToCheckpoint=Vector2.Distance(PlayerManager.instance.player.transform.position,checkpoint.transform.position);
            if(distanceToCheckpoint<closesDistance&&checkpoint.activationStats==true)
            {
                closesDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }
        return closestCheckpoint;
    }
}
