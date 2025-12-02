using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Date", menuName = "Date/Item effect")]

public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffect(Transform _enemyPosition)
    {
        Debug.Log("¹¦ÄÜ´¥·¢");
    }
}
