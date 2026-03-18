using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;

    public int currency;

    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public bool HaveEnoughSkillPoint(int _sp)
    {
        if (_sp > currency)
        {
            Debug.Log("技能点不足");
            return false;
        }
       currency -= _sp;

        return true;
        
    }
    public int GetCurrency()=> currency;
}
