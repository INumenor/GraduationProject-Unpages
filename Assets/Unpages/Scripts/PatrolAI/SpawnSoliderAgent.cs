using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSoliderAgent : NetworkBehaviour
{
    public List<Agent> Agents = new List<Agent>();
    public GameObject nawMash;
    public override void Spawned()
    {
        SpawnAgentHorror();
    }
    public void SpawnAgentHorror()
    {
        if (Runner.IsSharedModeMasterClient)
        {
            foreach (Agent agent in Agents)
            {
                Runner.Spawn(agent.agentHorror, agent.agentSpawnPoint.position, agent.agentSpawnPoint.rotation, Object.StateAuthority);
            }
            if (nawMash != null)
            {
                nawMash.SetActive(true);
            }
        }
    }

}
[Serializable]
public struct Agent
{
    public NetworkObject agentHorror;
    public Transform agentSpawnPoint;
}
