namespace Unpages.Network
{
    using Cysharp.Threading.Tasks;
    using Fusion;
    using Fusion.Sockets;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using UnityEngine;
    using UnityEngine.SceneManagement;


    public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        public static NetworkManager Instance { get; private set; }

        public NetworkRunner SessionRunner;

        [SerializeField] private GameObject networkRunnerPrefab;
        [SerializeField] private NetworkObject playerPreafab;
        [SerializeField] private NetworkObject networkCharacterPrefab;

        public static Dictionary<PlayerRef,NetworkPlayer> PlayerList { get; private set; }
        //[SerializeField] private NetworkObject networkCharacterPrefab;

        public Action<PlayerRef, NetworkPlayer> OnNetworkPlayerCreated { get; private set; }



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
            PlayerList = new();
            ConnectGame();
        }

        private void CreateNetworkRunner()
        {
            if (!SessionRunner) SessionRunner = Instantiate(networkRunnerPrefab, transform).GetComponent<NetworkRunner>();
            SessionRunner.AddCallbacks(this);
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
                CharacterSpawn(playerRef);
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

        public void CharacterSpawn(PlayerRef playerRef)
        {
            if (playerRef == SessionRunner.LocalPlayer && networkCharacterPrefab != null)
            {
                NetworkObject networkPlayerObject = SessionRunner.Spawn(networkCharacterPrefab, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.rotation, playerRef, (runner, obj) => { });

                PlayerList[playerRef].networkCharacter = networkPlayerObject;

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
}
