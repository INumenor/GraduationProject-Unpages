using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliderUnFollow : ISoliderState
{
    public SoliderStateManager soliderStateManager { get; set; }

    public void EnterState()
    {
        soliderStateManager.isLocked = false;
        soliderStateManager.targetPlayer = null;
        RandomWayPoint();
    }

    public void ExitState()
    {

    }

    public void UpdateState()
    {
        if (Vector3.Distance(soliderStateManager.agent.transform.position, soliderStateManager.target) < 1)
        {
            soliderStateManager.ChangeState(new SoliderPatrolState());
        }
    }
    public void RandomWayPoint()
    {
        soliderStateManager.target = soliderStateManager.wayPoints[Random.Range(0, soliderStateManager.wayPoints.Count)].transform.position;
        soliderStateManager.agent.SetDestination(soliderStateManager.target);
        soliderStateManager.agent.gameObject.transform.LookAt(new Vector3(soliderStateManager.target.x, soliderStateManager.agent.gameObject.transform.position.y, soliderStateManager.target.z));
        AgentSpeedDefault();
    }
    public void AgentSpeedDefault()
    {
        soliderStateManager.agent.speed = 5;
    }
}
