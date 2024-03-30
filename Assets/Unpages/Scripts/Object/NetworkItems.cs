using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkItems : MonoBehaviour
{
    public List<ItemInfo> networkItems;

    public NetworkObject GetNetworkItem(string gameObject)
    {
        foreach (ItemInfo networkObject in networkItems)
        {
            if(networkObject.item.name == gameObject)
                {
                    return networkObject.item;
                }
        }
        return null;
    }
}
