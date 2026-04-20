using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CreateSFXCollection : Editor
{
    [MenuItem("Audio/Create SFX Collection")]
    public static void Create()
    {
        SFXCollection collection = ScriptableObject.CreateInstance<SFXCollection>();

        string path = "Assets/Audio/SFX/SFXCollection.asset";
        AssetDatabase.CreateAsset(collection, path);
        AssetDatabase.SaveAssets();

        Debug.Log("已创建 SFX Collection: " + path);
    }
}