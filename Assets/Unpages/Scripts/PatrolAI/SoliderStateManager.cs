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
    [SerializeField]public Dictionary<PlayerRef, NetworkPlayer> allPlayer { get; private set; }

    public List<GameObject> wayPoints;

    public Vector3 target;
    public NetworkObject targetPlayer = null;

    bool _canTriggerExplosion = true;
    readonly float _explosionWaitInterval = 0.5f;

    public bool isLocked = false;

    public bool isCache;


    public NetworkObject closestPlayer;
    public float circleRadius = 1f;
    public float circleFollowRadius = 5f;


    [Header("Agent Options")]
    public float patrolSpeed = 6;
    public float chaseSpeed = 8;

    private void Awake()
    {
        NetworkManager.Instance.OnNetworkPlayerCreated += UpdateCreatedPlayerList;
        //NetworkManager.Instance.OnNetworkPlayerDestroyed += UpdateDestroyedPlayerList;
        //NetworkManager.Instance.OnMasterClientChanged += Init;
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
        ChasePlayer();
        if (!NetworkManager.Instance.SessionRunner.IsSharedModeMasterClient) return;
        currentState?.UpdateState();
    }

    private void OnEnable()
    {
        NetworkManager.Instance.OnNetworkPlayerCreated += UpdateCreatedPlayerList;
    }

    private void OnDestroy()
    {
        NetworkManager.Instance.OnNetworkPlayerCreated -= UpdateCreatedPlayerList;
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
        float distanceHere = Vector3.Distance(transform.position, allPlayer[LocalPlayer].networkCharacter.transform.position);
        Debug.Log(distanceHere + " Distance Here : ");
        if (distanceHere < 1f)
        {
            Debug.Log("Girdim 1");
            GameService.Instance.playerAction.enabled = false;
            await UniTask.WaitForSeconds(3);
            GameService.Instance.playerAction.enabled = true;
            Debug.Log("Girdim 2");
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
