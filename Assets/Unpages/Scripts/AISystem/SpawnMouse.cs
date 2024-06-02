using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class SpawnMouse : NetworkBehaviour
{
    public NetworkObject mouseAgent;
    public Transform mouseAgentBase;
    public override void Spawned()
    {
        SpawnAgentMouse();
    }
    public void SpawnAgentMouse()
    {
        if (Runner.IsSharedModeMasterClient)
        {
            NetworkObject mouseNetworkObject = Runner.Spawn(mouseAgent, mouseAgentBase.position,mouseAgent.transform.rotation, Object.StateAuthority);
            mouseNetworkObject.GetComponentInChildren<MouseStateManager>().mouseAgentBase = mouseAgentBase;
            //GameService.Instance.mouseStateManager.mouseAgent = mouseNetworkObject.GetComponentInParent<NavMeshAgent>();
            //GameService.Instance.mouseStateManager.mouseAI = mouseNetworkObject.GetComponent<MouseAI>();
            //GameService.Instance.mouseStateManager.networkMouseAI = mouseNetworkObject.GetComponent<NetworkMouseAI>();
            //GameService.Instance.mouseStateManager.networkMouseAI.MouseAnimatorController = mouseNetworkObject.GetComponent<Animator>();
        }
    }


}
