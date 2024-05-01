using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;

public class PlateKitchen : NetworkBehaviour
{
    public NetworkObject plateNetworkObject;

    public int plateLimit;
    
    [SerializeField] List<NetworkObject> platesObjects = new List<NetworkObject>();
    // Start is called before the first frame update

    public void GrabAndDropPlate(GameObject gameObject)//tabak yerine gidi� bast���nda tabak almas� ancak tabak olan bir yere bast���nda eli bo�sa �n�ndeki taba�� almas� laz�m ve e�er elinde varsa da taba�� koymas� gerekiyor.
    {
        if (!gameObject && plateLimit > 0)
        {
            GetPlate();
        }
        else if(gameObject && gameObject.GetComponent<ItemInfo>().itemType == ItemType.Plate)
        {
            DropPlate();
            Debug.Log("taba�� b�rak");
        }
    }

    public void DropPlate()
    {
        DespawnPlate(plateNetworkObject);
        plateLimit++;
        Debug.Log("tabak yok olup olu�tu");
    }
    public void GetPlate()
    {
        GameService.Instance.playerAction.playerInteraction.PlayerPlateGrab(plateNetworkObject);
        plateLimit --;
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
