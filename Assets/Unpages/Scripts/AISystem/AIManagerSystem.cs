using Fusion;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using Unpages.Network;

public class AIManagerSystem : SerializedMonoBehaviour
{
    [SerializeField] private NetworkObject mouseAgent;
    [SerializeField] private Transform agentBase;
    public Dictionary<string, NetworkObject> mouseList;/*=new Dictionary<string, NetworkObject>();*/

    public NavMeshSurface meshSurface;

    public void AreaBake()
    {
        
        meshSurface.BuildNavMesh();
      //  MouseSpawned();      
    }
    public void MouseSpawned()
    {
        
        if (NetworkManager.Instance.SessionRunner.IsSharedModeMasterClient)
        {          
            //mouseList.Add("Mouse1", NetworkManager.Instance.SessionRunner.Spawn(mouseAgent, agentBase.position, agentBase.rotation));
           // mouseList.Add("Mouse2" ,NetworkManager.Instance.SessionRunner.Spawn(mouseAgent, agentBase.position, agentBase.rotation));
           // 
        }
    }
    public void Init(Vector3 targetPosition)
    {
        mouseList["Mouse1"].gameObject.GetComponent<MouseAI>().MouseAgentDestination(targetPosition);
    }
    public void ReturnBase()
    {
        mouseList["Mouse1"].gameObject.GetComponent<MouseAI>().MouseAgentDestination(agentBase.position);
    }
}
