using Cysharp.Threading.Tasks;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;
using NetworkPlayer = Unpages.Network.NetworkPlayer;

public class PlayerAction : NetworkBehaviour
{
    public PlayerMovement playerMovement;
    public BombManager bombManager;
    public PlayerInteraction playerInteraction;
    public PlayerCamera playerCamera;
    public StateManager stateManager;

    //public IState currentState;

    //Player Anchor Point
    public Transform playerAnchorPoint;

    public bool isGrabbable = false;


    public GameObject interactionCollable;

    [Header("KeepObject")]
    public NetworkObject keepObject;
    //public ItemType keepObjectType;

    [Header("GrabbleObject")]
    public NetworkObject interactionObjcet;
    public ItemType interactionObjcetType;
    //public bool isTriggered = false;

    [Header("KitchenObject")]
    public NetworkObject playerInteractionKitchenObject;

    
    private async void Start()
    {
        if (Object.HasStateAuthority) 
        {
        GameService.Instance.playerAction = this;
        this.enabled = false;
        await TryGetPlayer();
        }
        else
        {
            interactionCollable.SetActive(false);
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

    public void RPC_Despawn(NetworkObject networkObject)
    {
        playerInteraction.RPC_Despawn(networkObject);
    }

    public void TriggerObject(Collider Object)
    {
        if(Object.CompareTag("Item")) interactionObjcet = Object.GetComponent<NetworkObject>();
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority == false) return;

        //currentState.UpdateState();
        //GameService.Instance.playerAnimationControl.RPC_CharacterDontKichenAction();
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
                if (playerInteractionKitchenObject)
                {
                    GameService.Instance.kitchenMechanics.SelectKitchenGD(
                        playerInteractionKitchenObject,
                        keepObject,
                        playerInteractionKitchenObject.GetComponent<KitchenObject>().kitchenObjectType,
                        playerAnchorPoint);
                }
                else
                {
                    if (!isGrabbable)
                    {
                        playerInteraction.PlayerGrabObject(interactionObjcetType, interactionObjcet);
                    }
                    else if (isGrabbable)
                    {
                        playerInteraction.PlayerDrobObject(keepObject,interactionObjcetType,interactionObjcet);
                    }
                }
            }
            if (inputData.isFeaturesUse == 1)
            {
                if (playerInteractionKitchenObject)
                {
                    GameService.Instance.kitchenMechanics.SelectKitchenAction(playerInteractionKitchenObject,
                        keepObject,
                        playerInteractionKitchenObject.GetComponent<KitchenObject>().kitchenObjectType);
                    //GameService.Instance.playerAnimationControl.RPC_CharacterKichenAction();
                }
                
            }
            else
            {
                stateManager.isKitchenAction = false;

            }
            if (inputData.isCameraChange == 1)
            {
                playerCamera.Camera();
            }
        }
        //base.FixedUpdateNetwork();
    }


    
}
