using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseStateManager : NetworkBehaviour
{
    public IMouseState currentState;

    public MouseAI mouseAI;
    public NetworkMouseAI networkMouseAI;

    public NavMeshAgent mouseAgent;
    public Transform mouseAgentBase;

    public List<NetworkObject> expiredFood = new List<NetworkObject>();

    public NetworkObject mouseGrabbleObject;
    public NetworkObject targetFood;

    public bool isCatch;

    public NetworkObject mouseAgentPrefab;

    public void StartStation()
    {
        //if (!HasStateAuthority) return;
        currentState = new MouseIdleState();
        currentState.mouseStateManager = this;
        currentState.EnterState();
    }

    public void ChangeState(IMouseState newState)
    {
        //if (!HasStateAuthority) return;
        currentState.ExitState();
        currentState = newState;
        currentState.mouseStateManager = this;
        currentState.EnterState();
    }

    private void Update()
    {
        //if (!HasStateAuthority) return;
        if(mouseAgent) currentState.UpdateState();
    }
}
