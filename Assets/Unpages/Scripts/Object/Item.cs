using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Food , Other ,Plate , Attributes , Null}
public class Item : NetworkBehaviour
{
    public ItemType itemType;
    public override void Spawned()
    {
        RPCTrigger();
    }

    [Networked, OnChangedRender(nameof(RPCTrigger))] public NetworkBool isInteractable { get; set; } = false;

    public void AddComponentInteract()
    {
        isInteractable = true;
    }

    public virtual void RPCTrigger()
    {
        
    }

}
