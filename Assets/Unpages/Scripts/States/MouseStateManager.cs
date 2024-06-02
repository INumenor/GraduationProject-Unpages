using Cysharp.Threading.Tasks;
using Fusion;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Unpages.Network;

public class MouseStateManager : SerializedMonoBehaviour
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
    //public NavMeshSurface meshSurface;

    async void Start()
    {
        await UniTask.WaitForSeconds(5f);
        StartStation();
    }

    public void StartStation()
    {
        if (!NetworkManager.Instance.SessionRunner.IsSharedModeMasterClient) return;
        GameService.Instance.mouseStateManager = this;
        currentState = new MouseIdleState();
        currentState.mouseStateManager = this;
        currentState.EnterState();
    }

    public void ChangeState(IMouseState newState)
    {
        if (!NetworkManager.Instance.SessionRunner.IsSharedModeMasterClient) return;
        currentState.ExitState();
        currentState = newState;
        currentState.mouseStateManager = this;
        currentState.EnterState();
    }

    private void Update()
    {
        if (!NetworkManager.Instance.SessionRunner.IsSharedModeMasterClient) return;
        if(currentState != null) currentState.UpdateState();
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

    public void AreaBake(NavMeshData navMeshData)
    {
        //meshSurface.UpdateNavMesh(navMeshData);
        //  MouseSpawned();      
    }
}
