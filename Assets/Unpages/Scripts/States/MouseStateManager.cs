using Fusion;
using System.Collections.Generic;
using Unity.AI.Navigation;
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
    public Animator MouseAnimatorController;
    public NavMeshSurface meshSurface;

    [Networked] public NetworkBool isIdle { get; set; } = true;
    [Networked] public NetworkBool isRunning { get; set; } = false;
    [Networked] public NetworkBool isJumping { get; set; } = false;
    public void StartStation()
    {
        //if (!Object.HasStateAuthority) return;
        currentState = new MouseReturnBaseState();
        currentState.mouseStateManager = this;
        currentState.EnterState();
    }

    public void ChangeState(IMouseState newState)
    {
        //if (!Object.HasStateAuthority) return;
        currentState.ExitState();
        currentState = newState;
        currentState.mouseStateManager = this;
        currentState.EnterState();
    }

    private void Update()
    {
        Debug.Log(currentState);
        if (mouseAgent) currentState.UpdateState();
        //if (isIdle)
        //{
        //    MouseAnimatorController.SetBool("isIdle", true);
        //}
        //else if (isRunning)
        //{
        //    MouseAnimatorController.SetBool("isRunning", true);
        //}
        //else if (isJumping)
        //{
        //    MouseAnimatorController.SetBool("isJumping", false);
        //}
        //else
        //{
        //    MouseAnimatorController.SetBool("isIdle", true);
        //    MouseAnimatorController.SetBool("isRunning", false);
        //    MouseAnimatorController.SetBool("isJumping", false);
        //}
    }

    public void AreaBake()
    {
        meshSurface.BuildNavMesh();
        //  MouseSpawned();      
    }
}
