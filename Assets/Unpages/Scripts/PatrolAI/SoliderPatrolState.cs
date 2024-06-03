using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliderPatrolState : ISoliderState
{
    public SoliderStateManager soliderStateManager { get; set; }

    public void EnterState()
    {
        RandomWayPoint();
    }

    public void ExitState()
    {

    }

    public void UpdateState()
    {

        //soliderStateManager.closestPlayer = ClosestPlayer();
        //NavMeshHit hit;
        if (/*soliderStateManager.targetPlayer == null &&*/ Vector3.Distance(soliderStateManager.agent.transform.position, soliderStateManager.target) < 1)
        {
            RandomWayPoint();
        }
        //else if (soliderStateManager.targetPlayer != null /*&& NavMesh.SamplePosition(stateManager.targetPlayer.GetComponent<NetworkRig>().Destinetion.position, out hit, 1.5f, NavMesh.AllAreas)*/)
        //{
        //    //soliderStateManager.ChangeState(new ChaseState());
        //}

    }
    public void RandomWayPoint()
    {
        soliderStateManager.target = soliderStateManager.wayPoints[Random.Range(0, soliderStateManager.wayPoints.Count)].transform.position;
        //stateManager.aiNetworkUpdate.RPC_SetDestination(stateManager.target);
        //stateManager.aiNetworkUpdate.RPC_AgentSpeedUp();

        soliderStateManager.agent.SetDestination(soliderStateManager.target);
        soliderStateManager.agent.gameObject.transform.LookAt(new Vector3(soliderStateManager.target.x, soliderStateManager.agent.gameObject.transform.position.y, soliderStateManager.target.z));
        AgentSpeedDefault();
    }

    //NetworkObject ClosestPlayer()
    //{
    //    NetworkObject closestHere = null;
    //    float leastDistance = Mathf.Infinity;
    //    foreach (var player in soliderStateManager.allPlayer.Values)
    //    {
    //        if (player.networkCharacter)
    //        {
    //            float distanceHere = Vector3.Distance(soliderStateManager.transform.position, player.networkCharacter.GetComponent<NetworkRig>().Destinetion.position);

    //            if (distanceHere < soliderStateManager.circleRadius && distanceHere < leastDistance)
    //            {
    //                leastDistance = distanceHere;
    //                closestHere = player.networkCharacter;
    //                soliderStateManager.targetPlayer = closestHere;
    //                soliderStateManager.isLocked = true;
    //                //break;
    //            }
    //        }

    //    }
    //    return closestHere;
    //}

    public void AgentSpeedDefault()
    {
        soliderStateManager.agent.speed = soliderStateManager.patrolSpeed;
    }
}
