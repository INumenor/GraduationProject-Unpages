using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerMovement : NetworkBehaviour
{
    private Vector3 _velocity;
    private bool _jumpPressed;

    public CharacterController _controller;

    public float MoveSpeed = 6f;

    public float JumpForce = 10f;
    public float GravityValue = -9.81f;

    

    //private void Awake()
    //{
    //    _controller = GetComponent<CharacterController>();
    //}

    public void PlayerMove()
    {
        if (GetInput<PlayerInputData>(out var inputData))
        {
            Vector3 move = (inputData.Direction * Runner.DeltaTime * MoveSpeed);

            _controller.Move((inputData.Direction * Runner.DeltaTime * MoveSpeed) + _velocity * Runner.DeltaTime);


            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
                if (GameService.Instance.playerAction.keepObject)
                {
                    GameService.Instance.playerAction.stateManager.isGrabbing = true;
                    //GameService.Instance.playerAnimationControl.RPC_CharacterGrabbing();
                    GameService.Instance.playerAction.playerMovement.MoveSpeed = 4;

                }
                else
                {
                    GameService.Instance.playerAction.stateManager.isRunning = true;
                    GameService.Instance.playerAction.stateManager.isGrabbing = false;
                    //GameService.Instance.playerAnimationControl.RPC_CharacterRunning();
                    GameService.Instance.playerAction.playerMovement.MoveSpeed = 7.5f;

                }
            }
            else
            {
                GameService.Instance.playerAction.stateManager.isRunning = false;
                GameService.Instance.playerAction.stateManager.isGrabbing = false;
                //GameService.Instance.playerAnimationControl.RPC_CharacterIdle();
                GameService.Instance.playerAction.playerMovement.MoveSpeed = 6;
            }
        }
    }
    public void PlayerJump()
    {
        if (GetInput<PlayerInputData>(out var inputData))
        {
            if (_controller.isGrounded)
            {
                _velocity = new Vector3(0, -1, 0);
                //GameService.Instance.playerAnimationControl.RPC_CharacterDontJumping();
                GameService.Instance.playerAction.stateManager.isJumping = false;
            }

            _velocity.y += GravityValue * Runner.DeltaTime;

            if (inputData.isJumped == 1 && _controller.isGrounded)
            {
                _velocity.y += JumpForce;
                //GameService.Instance.playerAnimationControl.RPC_CharacterJumping();
                GameService.Instance.playerAction.stateManager.isJumping = true;

            }
            _jumpPressed = false;
        }
        if(gameObject.transform.localPosition.y < -1)
        {
            GameService.Instance.playerAction.isOpen = false;
        }
        else
        {
            GameService.Instance.playerAction.isOpen = true;
        }
    }

   


    //public override void FixedUpdateNetwork()
    //{


    //    base.FixedUpdateNetwork();


    //        //transform.Translate(inputData.Direction * Runner.DeltaTime * MoveSpeed);

    //        //if(inputData.isBombDrop == 1)
    //        //{
    //        //    Debug.Log("Girdim");
    //        //    BombManager.BombInstance.DropBomb();
    //        //}
    //    }
    //}
}
