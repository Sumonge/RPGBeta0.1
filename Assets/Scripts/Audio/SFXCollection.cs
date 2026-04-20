using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SFX Collection", menuName = "Audio/SFX Collection")]
public class SFXCollection : ScriptableObject
{
    public List<SFXData> sfxList = new List<SFXData>();

    public SFXData GetSFX(string sfxName)
    {
        foreach (var sfx in sfxList)
        {
            if (sfx.sfxName == sfxName)
                return sfx;
        }
        Debug.LogWarning($"SFX not found: {sfxName}");
        return null;
    }

    public int GetSFXIndex(string sfxName)
    {
        for (int i = 0; i < sfxList.Count; i++)
        {
            if (sfxList[i].sfxName == sfxName)
                return i;
        }
        return -1;
    }
}