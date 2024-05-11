using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;
using NetworkPlayer = Unpages.Network.NetworkPlayer;

public class Test : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        NetworkManager.Instance.GetPlayer().Ready = true;
    }
    private void OnTriggerExit(Collider other)
    {
        NetworkManager.Instance.GetPlayer().Ready = false;
    }
}
