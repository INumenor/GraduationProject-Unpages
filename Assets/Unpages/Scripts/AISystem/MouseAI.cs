using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseAI : MonoBehaviour
{
    public NavMeshAgent mouseAgent;   
    public void MouseAgentDestination(Vector3 targetPosition)
    {
        mouseAgent.SetDestination(targetPosition);
    }
}
