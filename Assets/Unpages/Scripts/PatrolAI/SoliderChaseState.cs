using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class SoliderChaseState : ISoliderState
{
    public SoliderStateManager soliderStateManager { get; set; }

    public void EnterState()
    {

        if (soliderStateManager.closestPlayer)
        {
            ChasePlayer();
        }
    }
    public void ExitState()
    {

    }

    public void UpdateState()
    {

        soliderStateManager.closestPlayer = ClosestPlayer();
        if (soliderStateManager.targetPlayer)
        {
            ChasePlayer();
        }
        else
        {
            if (soliderStateManager.isCache)
            {
                soliderStateManager.isCache = false;
                soliderStateManager.ChangeState(new SoliderUnFollow());
            }
            else
            {
                soliderStateManager.ChangeState(new SoliderPatrolState());
            }
        }
        
    }
    public void ChasePlayer()
    {
        if (soliderStateManager.targetPlayer != null)
        {
            soliderStateManager.agent.SetDestination(soliderStateManager.targetPlayer.transform.position);
            soliderStateManager.agent.gameObject.transform.LookAt(new Vector3(soliderStateManager.targetPlayer.transform.position.x, soliderStateManager.agent.gameObject.transform.position.y, soliderStateManager.targetPlayer.transform.position.z));
            AgentSpeedUp();
        }
        NavMeshHit hit;
        if ((NavMesh.SamplePosition(soliderStateManager.targetPlayer.transform.position, out hit, 1.5f,4)))
        {
            float distanceHere = Vector3.Distance(soliderStateManager.transform.position, soliderStateManager.targetPlayer.transform.position);
            Debug.Log("Distance Here : " + distanceHere);
        if (distanceHere > soliderStateManager.circleFollowRadius)
        {
            soliderStateManager.targetPlayer = null;
            soliderStateManager.isLocked = false;
        }
        else if (distanceHere < 1f)
        {
            soliderStateManager.targetPlayer = null;
            soliderStateManager.isLocked = false;
            soliderStateManager.isCache = true;
        }
        }
        else
        {
            soliderStateManager.targetPlayer = null;
        }
    }


    NetworkObject ClosestPlayer()
    {
        NetworkObject closestHere = null;
        float leastDistance = Mathf.Infinity;
        foreach (var player in soliderStateManager.allPlayer.Values)
        {
            if (player.networkCharacter)
            {
                float distanceHere = Vector3.Distance(soliderStateManager.transform.position, player.networkCharacter.transform.position);

                if (distanceHere < soliderStateManager.circleRadius && distanceHere < leastDistance)
                {
                    leastDistance = distanceHere;
                    closestHere = player.networkCharacter;
                    soliderStateManager.targetPlayer = closestHere;
                    soliderStateManager.isLocked = true;
                }
            }
        }
        return closestHere;
    }

    public void AgentSpeedUp()
    {
        soliderStateManager.agent.speed = Mathf.Clamp(soliderStateManager.agent.speed + 0.2f, soliderStateManager.patrolSpeed, soliderStateManager.chaseSpeed);
    }


}
