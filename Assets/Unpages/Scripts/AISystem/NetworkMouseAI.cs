using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;

public class NetworkMouseAI : NetworkBehaviour
{
    public void DropItem(NetworkObject grabbleNetworkObject)
    {
        if (NetworkManager.Instance.SessionRunner.IsSharedModeMasterClient && grabbleNetworkObject)
        {
            Vector3 spawnPosition = transform.position + transform.forward * -1f;
            NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(grabbleNetworkObject, spawnPosition, this.transform.rotation, Object.StateAuthority);
            item.name = grabbleNetworkObject.name;
            item.gameObject.GetComponent<Rigidbody>().useGravity = true;
            grabbleNetworkObject = null;
            item.GetComponent<Item>().AddComponentInteract();
           
        }
    }

    //[Rpc(RpcSources.StateAuthority , RpcTargets.All)]
    //public void RPC_Despawn(NetworkObject)
    //{
    //    Runner.Despawn(NetworkObject );
    //}
}
