using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance;
    public Player player;

    public int currency;

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

    public bool HaveEnoughSkillPoint(int _sp)
    {
        if (_sp > currency)
        {
            Debug.Log("���ܵ㲻��");
            return false;
        }
        currency -= _sp;

        return true;

    }
    public int GetCurrency() => currency;

    public void LoadData(GameData _data)
    {
        this.currency = _data.currency;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = this.currency;
    }
}
