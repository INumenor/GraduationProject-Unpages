using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : NetworkBehaviour
{
    //This Facade pattern used metod in code 

    //[SerializeField] GameObject PlayerPrefab;

    //
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private BombManager bombManager;
    [SerializeField] private PlayerInteraction playerInteraction;

    public  bool isGrabbable = false;

    public GameObject grabbableObject;
    //public bool isTriggered = false;

    //----->
    private void Start()
    {
        //bombManager = FindAnyObjectByType<BombManager>();
        GameService.Instance.playerAction = this;
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
                bombManager.DropBomb(this.gameObject);
            }

            if (!isGrabbable && inputData.isPlayerGrapandDrop == 1)
            {
                playerInteraction.PlayerGrabAndDropItem(grabbableObject);
            }
            else if (isGrabbable && inputData.isPlayerGrapandDrop == 1)
            {
                playerInteraction.PlayerGrabAndDropItem();
            }
        }

        base.FixedUpdateNetwork();
    }


    
}
