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
            NetworkObject mouseNetworkObject = Runner.Spawn(mouseAgent, mouseAgentBase.position, mouseAgentBase.rotation, Object.StateAuthority);
            GameService.Instance.mouseStateManager.mouseAgent = mouseNetworkObject.GetComponent<NavMeshAgent>();
            GameService.Instance.mouseStateManager.mouseAI = mouseNetworkObject.GetComponent<MouseAI>();
            GameService.Instance.mouseStateManager.networkMouseAI = mouseNetworkObject.GetComponent<NetworkMouseAI>();
            GameService.Instance.mouseStateManager.StartStation();
        }
    }
}
