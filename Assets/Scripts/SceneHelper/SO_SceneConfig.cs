using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Config", menuName = "Custom/Scene Config")]
public class SO_SceneConfig : ScriptableObject
{
    public string ConfigTitle;
    public int SceneID;
    public AssetReferenceGameObject[] Environment;
}