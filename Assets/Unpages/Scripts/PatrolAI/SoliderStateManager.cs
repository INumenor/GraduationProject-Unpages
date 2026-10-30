using Cysharp.Threading.Tasks;
using Fusion;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Unpages.Network;
using NetworkPlayer = Unpages.Network.NetworkPlayer;


public class SoliderStateManager : SerializedMonoBehaviour
{
    public ISoliderState currentState;
    public ISoliderState startState;
    public NetworkObject parentNetworkObject;
    public NavMeshAgent agent;
    public NavMeshSurface navMeshSurface;

    public PlayerRef LocalPlayer;
    //public Animator characterAnimator;
    public string tagToWayPoints;
    public Dictionary<PlayerRef, NetworkPlayer> allPlayer { get; private set; }

    public List<GameObject> wayPoints;

    public Vector3 target;
    public NetworkObject targetPlayer = null;


    public bool isLocked = false;

    public bool isCatch;


    public NetworkObject closestPlayer;
    public float circleRadius = 1f;
    public float circleFollowRadius = 5f;


    [Header("Agent Options")]
    public float patrolSpeed = 6;
    public float chaseSpeed = 8;

    private bool _isCatching;

    private void Awake()
    {
        // Agents can spawn after the players were created, so don't wait for the event to fill the list.
        allPlayer = NetworkManager.PlayerList;
    }
    public void Start()
    {
        Init();
    }
    public void Init()
    {
        LocalPlayer = NetworkManager.Instance.SessionRunner.LocalPlayer;
        if (!NetworkManager.Instance.SessionRunner.IsSharedModeMasterClient) return;
        wayPoints.AddRange(GameObject.FindGameObjectsWithTag(tagToWayPoints));
        currentState = startState;
        currentState.soliderStateManager = this;
        currentState.EnterState();
        //aiNetworkUpdate.stateManager = this;
    }
    public void ChangeState(ISoliderState newState)
    {
        if (!NetworkManager.Instance.SessionRunner.IsSharedModeMasterClient) return;
        currentState.ExitState();
        currentState = newState;
        currentState.soliderStateManager = this;
        currentState.EnterState();
    }

    private void Update()
    {
        if (NetworkManager.Instance == null || NetworkManager.Instance.SessionRunner == null) return;
        ChasePlayer();
        if (!NetworkManager.Instance.SessionRunner.IsSharedModeMasterClient) return;
        currentState?.UpdateState();
    }

    private void OnEnable()
    {
        if (NetworkManager.Instance != null) NetworkManager.Instance.OnNetworkPlayerCreated += UpdateCreatedPlayerList;
    }

    private void OnDisable()
    {
        if (NetworkManager.Instance != null) NetworkManager.Instance.OnNetworkPlayerCreated -= UpdateCreatedPlayerList;
    }

    public void UpdateCreatedPlayerList(PlayerRef playerRef, NetworkPlayer networkPlayer)
    {
        allPlayer = NetworkManager.PlayerList;
    }

    public void UpdateDestroyedPlayerList(PlayerRef playerRef)
    {
        allPlayer = NetworkManager.PlayerList;
    }

    public async void ChasePlayer()
    {
        if (_isCatching || allPlayer == null) return;
        if (!allPlayer.TryGetValue(LocalPlayer, out NetworkPlayer localPlayer) || localPlayer.networkCharacter == null) return;

        PlayerAction playerAction = GameService.Instance != null ? GameService.Instance.playerAction : null;
        if (playerAction == null) return;

        float distanceHere = Vector3.Distance(transform.position, localPlayer.networkCharacter.transform.position);
        if (distanceHere < 1f)
        {
            _isCatching = true;
            playerAction.stateManager.characterAnimator.SetTrigger("isCatch");
            playerAction.enabled = false;
            if (await UniTask.WaitForSeconds(3f, cancellationToken: destroyCancellationToken).SuppressCancellationThrow()) return;
            // Don't give control back if the match ended while the player was stunned.
            if (playerAction != null && !playerAction.stateManager.isWin && !playerAction.stateManager.isLose)
                playerAction.enabled = true;
            _isCatching = false;
        }
    }
    //#region Player RPC & Explosion
    //private void KillPlayer(Transform target)
    //{
    //    if (_canTriggerExplosion)
    //    {
    //        aiNetworkUpdate.RPC_SpawnExplosion(target.transform.position);
    //        CanTriggerExplosion();
    //    }
    //    aiNetworkUpdate.TeleportPlayerToSpawn();
    //}
    //private async void CanTriggerExplosion()
    //{
    //    _canTriggerExplosion = false;
    //    await UniTask.WaitForSeconds(_explosionWaitInterval);
    //    _canTriggerExplosion = true;
    //}
}
