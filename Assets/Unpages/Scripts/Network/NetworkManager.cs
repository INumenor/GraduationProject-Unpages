using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour , INetworkRunnerCallbacks
{
    public static NetworkManager Instance {get; private set;}

    public NetworkRunner runner;

    [SerializeField] private GameObject networkRunnerPrefab;
    [SerializeField] private NetworkObject playerPreafab;
    [SerializeField] private NetworkObject networkCharacterPrefab;

    //public delegate void OnPlayerSpawn(NetworkRunner runner, PlayerRef playerRef);
    //public event OnPlayerSpawn onPlayerSpawn;
    private void Awake()
    {
        if (true)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
    
        }
        CreateNetworkRunner();
    }

    private void Start()
    {
        ConnectGame();
    }

    private void CreateNetworkRunner() 
    {
        if(!runner) runner = Instantiate(networkRunnerPrefab,transform).GetComponent<NetworkRunner>();
        runner.AddCallbacks(this);
    }

    public async void ConnectGame()
    {
        var args = new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = "Test",
            Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        };

        var connectionResult = await runner.StartGame(args);

        if (connectionResult.Ok)
        {
            Debug.Log("Scene Successful");
        }
        else
        {
            Debug.LogError(connectionResult.ErrorMessage);
        }
    }

    public void SpawnPlayer(NetworkRunner runner, PlayerRef player)
    {
        if (player == runner.LocalPlayer)
        {
            runner.Spawn(playerPreafab, transform.position, transform.rotation, player);
            //runner.Spawn(networkCharacterPrefab,Vector3.zero,Quaternion.identity,player);
            Debug.Log(runner + player.ToString());
            //onPlayerSpawn.Invoke(runner, player);
        }
    }


    #region NetworkRunnerCallBacks
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("New Player Joined" + player);
        SpawnPlayer(runner, player);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Player Out" + player);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }
    #endregion
}
