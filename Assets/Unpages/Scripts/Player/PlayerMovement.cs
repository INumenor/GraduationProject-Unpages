using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerMovement : NetworkBehaviour
{
    private Vector3 _velocity;
    private bool _jumpPressed;

    private CharacterController _controller;

    public float MoveSpeed = 6f;

    public float JumpForce = 10f;
    public float GravityValue = -9.81f;


    [SerializeField] private GameObject bombPrefab;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        //if (Input.GetButtonDown("Jump"))
        //{
        //    _jumpPressed = true;
        //}
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority == false)  return;

        base.FixedUpdateNetwork();

        if (GetInput<PlayerInputData>(out var inputData))
        {
            //transform.Translate(inputData.Direction * Runner.DeltaTime * MoveSpeed);

            if (_controller.isGrounded)
            {
                _velocity = new Vector3(0, -1, 0);
            }

            Vector3 move = (inputData.Direction * Runner.DeltaTime * MoveSpeed);

            _velocity.y += GravityValue * Runner.DeltaTime;

            if (inputData.isJumped == 1 && _controller.isGrounded)
            {
                _velocity.y += JumpForce;
            }
            _controller.Move((inputData.Direction * Runner.DeltaTime * MoveSpeed) + _velocity * Runner.DeltaTime);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }

            _jumpPressed = false;

            if(inputData.isBombDrop == 1)
            {
                Debug.Log("Girdim");
                //BombManager.BombInstance.DropBomb();
            }
        }
        
        
    }
}
