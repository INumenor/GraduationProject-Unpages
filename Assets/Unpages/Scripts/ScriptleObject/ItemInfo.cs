using Fusion;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[CreateAssetMenu(fileName = "InfoScriptableObject", menuName = "Unpages/ScriptableObject/TestInfo")]
public class ItemInfo : SerializedScriptableObject
{
    public NetworkObject item;
    public ItemType Type;
    public bool isSpawnable;
    public Sprite itemImage;
    
    [Range(0, 100)] public int minRandomGameSpawn;
    [Range(0, 100)] public int maxRandomGameSpawn;
}
