using UnityEngine;

[System.Serializable]
public class SFXItem
{
    public string sfxName;
    public AudioClip audioClip;
    [Range(0.1f, 3f)]
    public float pitchMin = 0.8f;
    [Range(0.1f, 3f)]
    public float pitchMax = 1.2f;
}