using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [Networked] public int TotalScore { get; set; }

    [Networked] public NetworkBool Ready { get; set; }

    public override void Spawned()
    {
        base.Spawned();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
