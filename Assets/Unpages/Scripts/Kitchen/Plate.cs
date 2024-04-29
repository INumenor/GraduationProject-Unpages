using Cysharp.Threading.Tasks;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;

public class Plate : NetworkBehaviour//tabak yerinden tabak al�nabilecek. Tabak bo� alanlara konulabilecek. Taba��n i�erisine do�ranm�� sebzeler konulabilecek di�erleri konulamayacak.taba��n oldu�u yerdeki bir yere yemekler konulabilecek ve taba��n i�erisinde spawn edilecek ya da haz�r spawn edilmi� halleri a��lacak.
{
    public NetworkObject platePrefab;

    public void GrabAndDropPlate(GameObject gameObject)//tabak yerine gidi� bast���nda tabak almas� ancak tabak olan bir yere bast���nda eli bo�sa �n�ndeki taba�� almas� laz�m ve e�er elinde varsa da taba�� koymas� gerekiyor.
    {
        if (!gameObject)
        {
            GetPlate();
        }
        else
        {
            DropPlate();
            Debug.Log("taba�� b�rak");
        }
    }
    public void DropPlate()
    {
        DespawnPlate(platePrefab);
        SpawnPlate(platePrefab);
        Debug.Log("tabak yok olup olu�tu");
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
