using Cysharp.Threading.Tasks;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using Unpages.Network;

public class MapDefault : NetworkBehaviour
{
    public const string Tag = "Map";

    [SerializeField] private Transform Player1SpawnPoint;
    [SerializeField] private Transform Player2SpawnPoint;
    [SerializeField] private AngryBar Player1AngryBar;
    [SerializeField] private AngryBar Player2AngryBar;

    [SerializeField] private GameObject Player1AngryBarCanvas;
    [SerializeField] private GameObject Player2AngryBarCanvas;
    public TMP_Text timerText;

    [SerializeField] private NetworkObject taskSystemPrefab;
    private NetworkObject taskSystem;
    [SerializeField] private Transform taskTransform;

    public GameObject WaitOtherPlayerCanvas;
    public GameObject GameScrennCanvas;


    public AudioClip audioClip;

    public NetworkManager networkManager => NetworkManager.Instance;

    [Networked] public int ReadyPlayerCount { get; set; }
    [Networked] public NetworkBool AllPlayersReady { get; set; } = false;

    [SerializeField] private NetworkObject networkCharacterPrefab;
    [SerializeField] private NetworkObject networkCharacterPrefab2;

    [Header("Events")]
    public UnityEvent OnPlayersReady;


    private void OnValidate()
    {
        if (!gameObject.CompareTag(Tag)) gameObject.tag = Tag;
    }

    private void Start()
    {
        networkManager.GameMap = this;
    }
    public override void FixedUpdateNetwork()
    {
        if (!AllPlayersReady) CheckReadyPlayers();
    }
    public async void CheckReadyPlayers()
    {
        if (!AllPlayersReady && Runner.IsSharedModeMasterClient && NetworkManager.PlayerList.Count > 1)
        {
            int readyPlayers = 0;


            NetworkManager.DoToAllPlayers(x => { if (x.Ready) readyPlayers++; });
            ReadyPlayerCount = readyPlayers;
            if (readyPlayers == NetworkManager.PlayerList.Count)
            {
                AllPlayersReady = true;
                RPC_AllPlayerReady();
                RPC_SetParentTask(Runner.Spawn(taskSystemPrefab, Vector3.zero, transform.rotation));
                OnAllPlayersReady();
            }
            else
            {
                AllPlayersReady = false;
            }
        }
    }

    public void CharacterSpawn(NetworkRunner SessionRunner, PlayerRef playerRef)
    {
        if (playerRef == SessionRunner.LocalPlayer && networkCharacterPrefab != null)
        {
            NetworkObject networkPlayerObject;
            if (playerRef.PlayerId == 1)
            {
                networkPlayerObject = SessionRunner.Spawn(networkCharacterPrefab, new Vector3(Player1SpawnPoint.position.x, Player1SpawnPoint.position.y + 1f, Player1SpawnPoint.position.z), transform.rotation, playerRef, (runner, obj) => { });
                networkPlayerObject.gameObject.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("Player1Kitchen");
                NetworkManager.PlayerList[playerRef].networkCharacter = networkPlayerObject;
                GameService.Instance.playerTask.angryBar = Player1AngryBar;
            }
            else
            {
                networkPlayerObject = SessionRunner.Spawn(networkCharacterPrefab2, new Vector3(Player2SpawnPoint.position.x, Player2SpawnPoint.position.y + 1f, Player2SpawnPoint.position.z), transform.rotation, playerRef, (runner, obj) => { });
                networkPlayerObject.gameObject.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("Player2Kitchen");
                NetworkManager.PlayerList[playerRef].networkCharacter = networkPlayerObject;
                Player2AngryBar.GetComponent<NetworkObject>().RequestStateAuthority();
                GameService.Instance.playerTask.angryBar = Player2AngryBar;
            }
            NetworkManager.PlayerList[playerRef].Ready = true;
        }
    }

    [Rpc(RpcSources.StateAuthority,RpcTargets.All)]
    public async void RPC_AllPlayerReady()
    {
        await StartTimer();
    }

    public async UniTask StartTimer()
    {
        if (AllPlayersReady) 
        {
            for(int i=3; i>=0; i--)
            {
                timerText.text = "Game Start \n"+i+" Seconds";
                await UniTask.WaitForSeconds(1);
            }
            SoundManager.Instance.StartGameMusic(audioClip);
            WaitOtherPlayerCanvas.SetActive(false);
            GameScrennCanvas.SetActive(true);
            Player1AngryBarCanvas.SetActive(true);
            Player2AngryBarCanvas.SetActive(true);
            GameService.Instance.playerAction.enabled = true;
            //NetworkManager.Instance.CahngeGameScene();
        }
    }

    [Rpc(RpcSources.StateAuthority,RpcTargets.All)]
    public void RPC_SetParentTask(NetworkObject networkObject)
    {
        networkObject.transform.SetParent(taskTransform);
    }

    public virtual void OnAllPlayersReady() { OnPlayersReady?.Invoke(); }

}
