using Fusion;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InfoScriptableObject", menuName = "Unpages/ScriptableObject/ItemInfo")]
public class FoodInfo : SerializedScriptableObject
{
    public NetworkObject item;
    public FoodType foodtype;
    public bool isSpawnable;
    [Range(0,100)]public int minRandomGameSpawn;
    [Range(0,100)]public int maxRandomGameSpawn;
}
