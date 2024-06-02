namespace Unpages.Network
{
    using Cysharp.Threading.Tasks;
    using Fusion;
    using Fusion.Sockets;
    using Sirenix.OdinInspector;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [RequireComponent(typeof(NetworkSceneManagerDefault))]
    public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        public static NetworkManager Instance { get; private set; }

        public NetworkRunner SessionRunner;

        [SerializeField] private GameObject networkRunnerPrefab;
        [SerializeField] private NetworkObject playerPreafab;
        //[SerializeField] private NetworkObject networkCharacterPrefab;
        public INetworkSceneManager sceneManager;


        //[SerializeField] private Transform Player1SpawnPoint;
        //[SerializeField] private Transform Player2SpawnPoint;

        public static Dictionary<PlayerRef,NetworkPlayer> PlayerList { get; private set; }
        //[SerializeField] private NetworkObject networkCharacterPrefab;

        public Action<PlayerRef, NetworkPlayer> OnNetworkPlayerCreated ; /*{ get; private set; }*/
        public Action OnNetworkPlayerDisconneted ;


        public MapDefault GameMap
        {
            get
            {
                if (map == null) { map = FindObjectOfType<MapDefault>(); }
                return map;

            }
            set
            {
                map = value;
            }
        }
        private MapDefault map;


        //public delegate void OnPlayerSpawn(NetworkRunner runner, PlayerRef playerRef);
        //public event OnPlayerSpawn onPlayerSpawn;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);

            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            sceneManager = gameObject.GetComponent<NetworkSceneManagerDefault>();
            CreateNetworkRunner();
        }

        private void Start()
        {
            PlayerList = new();
            //ConnectGame();
        }

        private void CreateNetworkRunner()
        {
            if (!SessionRunner) SessionRunner = Instantiate(networkRunnerPrefab, transform).GetComponent<NetworkRunner>();
            SessionRunner.AddCallbacks(this);
        }

        public enum SesssionType : int
        {
            Public,
            Private
        }

        public async void ConnectLobby(string sessionName = "")
        {
            var customProps = new Dictionary<string, SessionProperty>();

            customProps["SessionType"] = sessionName == "" ? (int)SesssionType.Public : (int)SesssionType.Private;

            var args = new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                Scene = SceneRef.FromIndex(1),
                SessionProperties = customProps,
                SessionName = sessionName,
                SceneManager = sceneManager
            };

            var connectionResult = await SessionRunner.StartGame(args);

            if (connectionResult.Ok)
            {
                Debug.Log("Scene Successful");
            }
            else
            {
                Debug.LogError(connectionResult.ErrorMessage);
            }
        }

        public async void ConnectGame(string sessionName = "")
        {

            var customProps = new Dictionary<string, SessionProperty>();

            customProps["SessionType"] = sessionName == "" ? (int)SesssionType.Public : (int)SesssionType.Private;

            var args = new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                Scene = SceneRef.FromIndex(2),
                SessionProperties = customProps,
                SessionName = sessionName,
                SceneManager = sceneManager
            };

            var connectionResult = await SessionRunner.StartGame(args);

            if (connectionResult.Ok)
            {
                Debug.Log("Scene Successful");
            }
            else
            {
                Debug.LogError(connectionResult.ErrorMessage);
            }
        }
        public async void Disconnect()
        {
            Debug.Log("Disconnecting");

            await SessionRunner.Shutdown();

            Destroy(SessionRunner);
            SessionRunner = null;

            // await SceneLoadManager.UnloadAllScenes();
            SceneManager.LoadScene(0);

        }

        private static readonly int maxAttempt = 30;
        public static async UniTask<NetworkPlayer> GetPlayerAsync(CancellationToken token, Action<NetworkPlayer> action = default, PlayerRef ply = default)
        {
            int attempt = 0;
            NetworkPlayer player = null;

            while (player == null && attempt <= maxAttempt)
            {
                if (Instance != null)
                {
                    if (Instance.SessionRunner.State == NetworkRunner.States.Running)
                    {
                        player = Instance.GetPlayer(ply);
                        if (player != null)
                        {
                            action?.Invoke(player);
                            return player;
                        }
                    }

                }

                await UniTask.WaitForSeconds(1, token.IsCancellationRequested);
                attempt++;
            }
            if (player == null) Debug.LogError("player not found. Attempt: " + attempt);
            return player;
        }

        public void SpawnPlayer(NetworkRunner runner, PlayerRef player)
        {
            if (player == runner.LocalPlayer)
            {
                runner.Spawn(playerPreafab, position: transform.position, rotation: transform.rotation, player, flags: NetworkSpawnFlags.DontDestroyOnLoad);
                //runner.Spawn(networkCharacterPrefab,Vector3.zero,Quaternion.identity,player);
                Debug.Log(runner + player.ToString());
                //onPlayerSpawn.Invoke(runner, player);
            }
        }

        public void SetPlayer(PlayerRef playerRef, NetworkPlayer player)
        {
            if (player == null)
            {
                return;
            }
            Debug.Log("PlayerRef : "+ playerRef + "Player :" + player);
            PlayerList[playerRef] = player;

            player.transform.SetParent(transform);

            if (player.networkCharacter == null)
            {
                GameMap.CharacterSpawn(SessionRunner,playerRef);
            }

            OnNetworkPlayerCreated?.Invoke(playerRef, player);
        }

        public NetworkPlayer GetPlayer(PlayerRef ply = default)
        {
            if (!SessionRunner)
                return null;
            if (ply == default)
                ply = SessionRunner.LocalPlayer;
            if (PlayerList.TryGetValue(ply, out NetworkPlayer player))
            {
                return player;
            }
            else
            {
                Debug.LogWarning("Player: " + ply.PlayerId + " Network Player not found.");
                return null;
            }
        }
        public static void DoToAllPlayers(Action<NetworkPlayer> action)
        {
            foreach (NetworkPlayer player in PlayerList.Values)
            {
                action(player);
            }
        }

        //public async void CahngeGameScene()
        //{

        //    Debug.Log("Disconnecting");

        //    await SessionRunner.Shutdown();

        //    Destroy(SessionRunner);
        //    SessionRunner = null;

        //    // await SceneLoadManager.UnloadAllScenes();
        //    ConnectGame();

        //}

        //public void CharacterSpawn(PlayerRef playerRef)
        //{
        //    if (playerRef == SessionRunner.LocalPlayer && networkCharacterPrefab != null)
        //    {               
        //        NetworkObject networkPlayerObject;
        //        if (playerRef.PlayerId == 1)
        //        {
        //            networkPlayerObject = SessionRunner.Spawn(networkCharacterPrefab, new Vector3(Player1SpawnPoint.position.x, Player1SpawnPoint.position.y + 1f, Player1SpawnPoint.position.z), transform.rotation, playerRef, (runner, obj) => { });
        //            networkPlayerObject.gameObject.transform.GetChild(1).gameObject.layer= LayerMask.NameToLayer("Player1Kitchen");                  
        //            PlayerList[playerRef].networkCharacter = networkPlayerObject;

        //        }
        //        else
        //        {
        //            networkPlayerObject = SessionRunner.Spawn(networkCharacterPrefab, new Vector3(Player2SpawnPoint.position.x, Player2SpawnPoint.position.y + 1f, Player2SpawnPoint.position.z), transform.rotation, playerRef, (runner, obj) => { });
        //            networkPlayerObject.gameObject.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("Player2Kitchen");
        //            PlayerList[playerRef].networkCharacter = networkPlayerObject;
        //        }
        //    }
        //}


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
            Disconnect();
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
}
