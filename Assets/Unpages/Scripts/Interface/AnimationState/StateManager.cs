using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : NetworkBehaviour
{
    public IState currentState;
    public Animator characterAnimator;
    public PlayerAction playerAction;

    public bool isGrabbing;
    public bool isRunning;
    public bool isJumping;
    public bool isKitchenAction;

    void Start()
    {
        if (!HasInputAuthority) return;
        currentState = new IdleState();
        currentState.stateManager = this;
        currentState.EnterState();
    }

    public void ChangeState(IState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.stateManager = this;
        currentState.EnterState();
    }

    private void Update()
    {
        currentState.UpdateState();
    }

}
