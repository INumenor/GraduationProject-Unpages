using Cysharp.Threading.Tasks;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;

public class Plate : NetworkBehaviour//tabak yerinden tabak alýnabilecek. Tabak boþ alanlara konulabilecek. Tabaðýn içerisine doðranmýþ sebzeler konulabilecek diðerleri konulamayacak.tabaðýn olduðu yerdeki bir yere yemekler konulabilecek ve tabaðýn içerisinde spawn edilecek ya da hazýr spawn edilmiþ halleri açýlacak.
{
    public NetworkObject platePrefab;

    public void GrabAndDropPlate(GameObject gameObject)//tabak yerine gidið bastýðýnda tabak almasý ancak tabak olan bir yere bastýðýnda eli boþsa önündeki tabaðý almasý lazým ve eðer elinde varsa da tabaðý koymasý gerekiyor.
    {
        if (!gameObject)
        {
            GetPlate();
        }
        else
        {
            DropPlate();
            Debug.Log("tabaðý býrak");
        }
    }
    public void DropPlate()
    {
        DespawnPlate(platePrefab);
        SpawnPlate(platePrefab);
        Debug.Log("tabak yok olup oluþtu");
    }
    public void GetPlate()
    {
            GameService.Instance.playerAction.playerInteraction.PlayerPlateGrab(platePrefab);               
    }
    public void SpawnPlate(NetworkObject networkObject)
    {
        platePrefab = NetworkManager.Instance.SessionRunner.Spawn(networkObject, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), this.transform.rotation, Object.InputAuthority);
        platePrefab.transform.SetParent(transform);
        platePrefab.gameObject.GetComponent<Rigidbody>().useGravity = false;
        platePrefab.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        platePrefab.name = networkObject.name;
    }
    public void DespawnPlate(NetworkObject networkObject)
    {
        GameService.Instance.playerAction.RPC_Trigger(networkObject);
    }
    public void InteractPlateArea()
    {
        GameService.Instance.playerAction.playerInteractionKitchenObject = this.gameObject;
    }
}
