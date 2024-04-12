using Cysharp.Threading.Tasks;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;
using NetworkPlayer = Unpages.Network.NetworkPlayer;

public class PlayerAction : NetworkBehaviour
{
    //This Facade pattern used metod in code 

    //[SerializeField] GameObject PlayerPrefab;

    //
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] public BombManager bombManager;
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private PlayerCamera playerCamera;

    public  bool isGrabbable = false;

    public GameObject grabbableObject;
    public ItemType grabbableObjectType;
    //public bool isTriggered = false;

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

            if (!isGrabbable && inputData.isPlayerGrapandDrop == 1)
            {
                playerInteraction.PlayerGrabAndDropItem(grabbableObjectType,grabbableObject);
            }
            else if (isGrabbable && inputData.isPlayerGrapandDrop == 1)
            {
                playerInteraction.PlayerGrabAndDropItem(ItemType.Null, null);
            }
            Debug.Log(inputData.isCameraChange);
            if(inputData.isCameraChange == 1)
            {
                playerCamera.Camera();
            }
        }
        //base.FixedUpdateNetwork();
    }


    
}
