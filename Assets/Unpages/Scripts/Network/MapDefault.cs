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

    public TMP_Text timerText;

    public NetworkManager networkManager => NetworkManager.Instance;

    [Networked] public int ReadyPlayerCount { get; set; }
    [Networked,OnChangedRender(nameof(Timer))] public NetworkBool AllPlayersReady { get; set; } = false;

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
        CheckReadyPlayers();
    }
    public void CheckReadyPlayers()
    {
        if (!AllPlayersReady)
        {
            int readyPlayers = 0;


            NetworkManager.DoToAllPlayers(x => { if (x.Ready) readyPlayers++; });
            Debug.Log("22asd");
            ReadyPlayerCount = readyPlayers;
            if (readyPlayers == NetworkManager.PlayerList.Count)
            {
                AllPlayersReady = true;
                Debug.Log("Ýyisin");
                OnAllPlayersReady();
            }
            else
            {
                Debug.Log("Dineme");
                AllPlayersReady = false;
            }
        }
        Debug.Log("Deneme");

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

            }
            else
            {
                networkPlayerObject = SessionRunner.Spawn(networkCharacterPrefab2, new Vector3(Player2SpawnPoint.position.x, Player2SpawnPoint.position.y + 1f, Player2SpawnPoint.position.z), transform.rotation, playerRef, (runner, obj) => { });
                networkPlayerObject.gameObject.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("Player2Kitchen");
                NetworkManager.PlayerList[playerRef].networkCharacter = networkPlayerObject;
            }
        }
    }

    public void Timer()
    {
        if (AllPlayersReady) 
        {
            //NetworkManager.SetAllPlayersNotReady();
            NetworkManager.Instance.CahngeGameScene();
            timerText.text = "Hazýrýz";
            //NetworkManager.Instance;
        }
        else timerText.text = "Hazýr Deðiliz";
    }

    public virtual void OnAllPlayersReady() { OnPlayersReady?.Invoke(); }

}
