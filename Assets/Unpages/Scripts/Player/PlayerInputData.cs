using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerInputData : INetworkInput
{
    public Vector3 Direction;
    public float isJumped;
    public float isBombDrop;
    public float isPlayerGrapandDrop;
    public float isCameraChange;
}
