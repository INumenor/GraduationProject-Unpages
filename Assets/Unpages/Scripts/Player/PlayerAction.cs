using Cysharp.Threading.Tasks;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;
using NetworkPlayer = Unpages.Network.NetworkPlayer;

public class PlayerAction : NetworkBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] public BombManager bombManager;
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private PlayerCamera playerCamera;

    public  bool isGrabbable = false;

    [Header("KeepObject")]
    public GameObject keepObject;
    //public ItemType keepObjectType;

    [Header("GrabbleObject")]
    public GameObject grabbableObject;
    public ItemType grabbableObjectType;
    //public bool isTriggered = false;

    public GameObject playerInteractionKitchenObject;

    //----->
    private async void Start()
    {
        //bombManager = FindAnyObjectByType<BombManager>();
        if (Object.HasStateAuthority) 
        {
        GameService.Instance.playerAction = this;
        await TryGetPlayer();
        }
    }
    private async UniTask<NetworkPlayer> TryGetPlayer()
    {
        NetworkPlayer networkPlayer = null;
        while (networkPlayer == null)
        {
            networkPlayer = NetworkManager.Instance.GetPlayer(Object.StateAuthority);
            Debug.LogWarning("TryToGet");
            await UniTask.WaitForSeconds(1, cancellationToken: destroyCancellationToken);

        }
        networkPlayer.networkCharacter = GetComponent<NetworkObject>();
        return networkPlayer;

    }

    public void RPC_Trigger(NetworkObject networkObject)
    {
        playerInteraction.RPC_Despawn(networkObject);
    }

    public void TriggerObjcet(Collider Object)
    {
        if(Object.CompareTag("Item")) grabbableObject = Object.gameObject;
        else if(Object.CompareTag("Cupboard"))
        {
            playerInteractionKitchenObject = Object.gameObject;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority == false) return;

        if (GetInput<PlayerInputData>(out var inputData))
        {
            playerMovement.PlayerMove();

            playerMovement.PlayerJump();

            //if (inputData.isJumped == 1)
            //{
            //    playerMovement.PlayerJump();
            //}
            //else
            //{
            //    playerMovement.PlayerJump();
            //}

            if (inputData.isBombDrop == 1)
            {
                bombManager.DropBomb(this.gameObject , Runner ,Object.StateAuthority);
            }

            if (inputData.isPlayerGrapandDrop == 1)
            {
                if (playerInteractionKitchenObject && keepObject)
                {
                    
                }
                else
                {
                    if (!isGrabbable)
                    {
                        playerInteraction.PlayerGrabAndDropItem(grabbableObjectType, grabbableObject);
                    }
                    else if (isGrabbable)
                    {
                        playerInteraction.PlayerGrabAndDropItem(ItemType.Null, null);
                    }
                }
            }
            if(inputData.isCameraChange == 1)
            {
                playerCamera.Camera();
            }
        }
        //base.FixedUpdateNetwork();
    }


    
}
