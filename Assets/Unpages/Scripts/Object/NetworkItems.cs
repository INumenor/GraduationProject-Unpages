using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkItems : MonoBehaviour
{
    public List<NetworkObject> networkItems;

    public NetworkObject GetNetworkItem(string gameObject)
    {
        foreach (NetworkObject networkObject in networkItems)
        {
            if(networkObject.name == gameObject)
                {
                    return networkObject;
                }
        }
        return null;
    }
}
