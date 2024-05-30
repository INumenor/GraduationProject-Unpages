using Fusion;
using UnityEngine;

public class Cupboard : KitchenObject
{
    public AudioSource audioSource;
    public override void DropItem(NetworkObject networkObject)
    {
        if (onTheCupboardObject == null)
        {
            onTheCupboardObject = GameService.Instance.spawnObject.PlayerDropCupboardItem(networkObject, anchorPoints, false);
            audioSource.Play();
        }
    }

    public override void GrabItem(NetworkObject networkObject, Transform anchorPoint)
    {
        if (onTheCupboardObject != null)
        {
            GameService.Instance.spawnObject.PlayerGrabCupboardItem(onTheCupboardObject, anchorPoint, false);
            onTheCupboardObject = null;
        }
    }

    public override void PlateDrop(NetworkObject networkObject)
    {
        base.PlateDrop(networkObject);
    }
}
