using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KitchenObjectType { Null,Cupboard,Storage,PlateHolder,Trash,ChoppingBoard}

public class KitchenObject : MonoBehaviour
{
    public Transform anchorPoints;
    public KitchenObjectType kitchenObjectType;
    public NetworkObject onTheCupboardObject;

    public virtual void DropItem(NetworkObject networkObject) 
    {
    
    }

    public virtual void GrabItem(NetworkObject networkObject,Transform anchorPoint)
    {

    }
}
