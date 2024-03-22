using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Unpages.Network;

public class PlayerInput : MonoBehaviour , INetworkRunnerCallbacks
{
    [SerializeField]
    private InputActionReference _moveInput;
    [SerializeField]
    private InputActionReference _jumpInput;
    [SerializeField]
    private InputActionReference _bombInput; 
    [SerializeField]
    private InputActionReference _dropAndGrapInput;

    private void OnEnable()
    {
        _moveInput.action.Enable();
        _jumpInput.action.Enable();
        _bombInput.action.Enable();
        _dropAndGrapInput.action.Enable();
    }
    private void OnDisable()
    {
        _moveInput.action.Disable();
        _jumpInput.action.Disable();
        _bombInput.action.Disable();
        _dropAndGrapInput.action.Disable();
    }

    void Start()
    {
        NetworkManager.Instance.SessionRunner.AddCallbacks(this);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        PlayerInputData inputData = new PlayerInputData();

        Vector2 direction2D = _moveInput.action.ReadValue<Vector2>();
        Vector3 direction3D = new Vector3(direction2D.x, 0, direction2D.y);

        inputData.Direction = direction3D;
        inputData.isJumped = _jumpInput.action.ReadValue<float>();
        inputData.isBombDrop = _bombInput.action.ReadValue<float>();
        inputData.isPlayerGrapandDrop = _dropAndGrapInput.action.ReadValue<float>();

        input.Set(inputData);
    }
    #region RunnerCallBacks
    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }
#endregion
}
