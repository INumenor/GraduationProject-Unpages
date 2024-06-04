using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : NetworkBehaviour
{
    public IState currentState;
    public Animator characterAnimator;
    public PlayerAction playerAction;

    [Networked] public NetworkBool isGrabbing { get; set; }
    [Networked] public NetworkBool isRunning { get; set; }
    [Networked] public NetworkBool isJumping { get; set; }
    [Networked] public NetworkBool isKitchenAction { get; set; }
    [Networked] public NetworkBool isWin { get; set; }
    [Networked] public NetworkBool isLose { get; set; }
    //public bool isGrabbing;
    //public bool isRunning;
    //public bool isJumping;
    //public bool isKitchenAction;

    void Start()
    {
        //if (!HasStateAuthority) return;
        currentState = new IdleState();
        currentState.stateManager = this;
        currentState.EnterState();
    }

    public void ChangeState(IState newState)
    {
        //if (!HasStateAuthority) return;
        currentState.ExitState();
        currentState = newState;
        currentState.stateManager = this;
        currentState.EnterState();
    }

    private void Update()
    {
        //if (!HasStateAuthority) return;
        currentState.UpdateState();
    }

}
