using Fusion;
using UnityEngine;

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
        if (soliderStateManager.targetPlayer != null)
        {
            ChasePlayer();
        }
        else
        {
            soliderStateManager.ChangeState(new SoliderPatrolState());
            //if (soliderStateManager.isCatch)
            //{
            //    soliderStateManager.isCatch = false;
            //    soliderStateManager.ChangeState(new SoliderUnFollow());
            //}
            //else
            //{
            //    soliderStateManager.ChangeState(new SoliderPatrolState());
            //}
        }

    }
    public void ChasePlayer()
    {
        if (soliderStateManager.targetPlayer != null)
        {
            soliderStateManager.agent.SetDestination(soliderStateManager.targetPlayer.transform.position);
            soliderStateManager.agent.gameObject.transform.LookAt(new Vector3(soliderStateManager.targetPlayer.transform.position.x, soliderStateManager.agent.gameObject.transform.position.y, soliderStateManager.targetPlayer.transform.position.z));
            AgentSpeedUp();


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
                soliderStateManager.ChangeState(new SoliderUnFollow());
            }

            if (soliderStateManager.targetPlayer != null && Vector3.Distance(soliderStateManager.agent.destination, soliderStateManager.targetPlayer.transform.position) > 2f)
            {
                soliderStateManager.ChangeState(new SoliderUnFollow());
            }
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
