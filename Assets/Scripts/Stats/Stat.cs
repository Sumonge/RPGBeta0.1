using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Stat 
{
    [SerializeField]private int baseValue;



    public List<int> modifiers;
    public int GetValue()
    {
        int fineValue=baseValue;

        foreach(int modifier in modifiers)
        {
            fineValue += modifier;
        }
        return fineValue;
    }

    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }
    public void AddModifier(int _modifier)//便于buff伤害添加
    {
        modifiers.Add(_modifier);
    }

    public void RemoveModifier(int _modifier)
    {
        modifiers.Remove(_modifier);
    }
    //可以设置成Map<物品id，List<数值>>格式，加的时候直接数值相加，删除的时候直接用物品id删除，干净利索
}
