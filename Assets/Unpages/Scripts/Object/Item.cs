using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Food , Other ,Plate , Attributes , Null}
public class Item : MonoBehaviour
{
    public ItemType itemType;


    public virtual void AddComponentInteract()
    {

    }
}
