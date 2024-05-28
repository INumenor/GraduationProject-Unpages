using Cysharp.Threading.Tasks;
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

    [Networked] public NetworkBool isIdle { get; set; }
    [Networked] public NetworkBool isRunning { get; set; }
    [Networked] public NetworkBool isJumping { get; set; }
    public  void StartStation()
    {
        //if (!Object.HasStateAuthority) return;
        currentState = new MouseIdleState();
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

        // if (!Object.HasStateAuthority) return;
        if (mouseAgent) currentState.UpdateState();
    }

    public void AreaBake()
    {
        meshSurface.BuildNavMesh();
        //  MouseSpawned();      
    }
}
