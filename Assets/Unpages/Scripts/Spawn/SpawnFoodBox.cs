using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;
using NetworkPlayer = Unpages.Network.NetworkPlayer;

public class SpawnFoodBox : MonoBehaviour
{
    [SerializeField] private NetworkObject _spawnBoxPrefab;
    [SerializeField] private NetworkObject _wall;
    [SerializeField] private GameObject _spawnedBox;
    private List<NetworkObject> _spawnedFoodBox = new List<NetworkObject>();
    private List<NetworkObject> _walls = new List<NetworkObject>();
    private int _wallSpawnedCount = 0;
    private int _boxSpawnedCount = 0;
    int SpawnXPoint;
    int SpawnYPoint;

    private void OnEnable()
    {
        NetworkManager.Instance.OnNetworkPlayerCreated += Spawn;
    }
    private void OnDisable()
    {
        NetworkManager.Instance.OnNetworkPlayerCreated -= Spawn;
    }
    public void Spawn(PlayerRef playerRef, NetworkPlayer player)
    {
        Debug.Log(playerRef.PlayerId);
        if (playerRef.PlayerId==1)
        {
            while (_wallSpawnedCount < 30)
            {
                SpawnWallObject();
            }
            if (_wallSpawnedCount == 30)
            {
                while (_boxSpawnedCount < 50)
                {
                    SpawnBoxObject();
                }
            }
        }       
    }
    private void SpawnBoxObject()
    {
        bool validSpawn = false;

        while (!validSpawn)
        {
            GetRandomNumber(out SpawnXPoint, out SpawnYPoint);
            Vector3 spawnPosition = new Vector3(Mathf.RoundToInt(SpawnXPoint), 0.5f, Mathf.RoundToInt(SpawnYPoint));

            // Kutularýn ve duvarlarýn konumunu ayrý ayrý kontrol et
            bool positionOccupied = _spawnedFoodBox.Exists(obj => Mathf.RoundToInt(obj.transform.position.x) == Mathf.RoundToInt(spawnPosition.x) &&
                                                               Mathf.RoundToInt(obj.transform.position.z) == Mathf.RoundToInt(spawnPosition.z)) ||
                                 _walls.Exists(obj => Mathf.RoundToInt(obj.transform.position.x) == Mathf.RoundToInt(spawnPosition.x) &&
                                                       Mathf.RoundToInt(obj.transform.position.z) == Mathf.RoundToInt(spawnPosition.z));
            if (!positionOccupied)
            {
                NetworkObject newFood = NetworkManager.Instance.SessionRunner.Spawn(_spawnBoxPrefab, spawnPosition, _spawnBoxPrefab.transform.rotation);
                newFood.gameObject.transform.SetParent(_spawnedBox.transform, false);
                _spawnedFoodBox.Add(newFood);
                _boxSpawnedCount++;
                validSpawn = true;
            }
        }
    }

    private void SpawnWallObject()
    {
        bool validSpawn = false;

        while (!validSpawn)
        {
            GetRandomNumber(out SpawnXPoint, out SpawnYPoint);
            Vector3 spawnPosition = new Vector3(Mathf.RoundToInt(SpawnXPoint), 1.1f, Mathf.RoundToInt(SpawnYPoint));

            // Kutularýn ve duvarlarýn konumunu ayrý ayrý kontrol et
            bool positionOccupied = _spawnedFoodBox.Exists(obj => Vector3Int.RoundToInt(obj.transform.position) == Vector3Int.RoundToInt(spawnPosition)) ||
                                    _walls.Exists(obj => Vector3Int.RoundToInt(obj.transform.position) == Vector3Int.RoundToInt(spawnPosition));

            if (!positionOccupied)
            {
                NetworkObject newWall = NetworkManager.Instance.SessionRunner.Spawn(_wall, spawnPosition, _wall.transform.rotation);
                newWall.gameObject.transform.SetParent(_spawnedBox.transform, false);
                _walls.Add(newWall);
                _wallSpawnedCount++;
                validSpawn = true;
            }
        }
    }
    private void GetRandomNumber(out int SpawnXPoint, out int SpawnYPoint)
    {
        do
        {
            SpawnXPoint = Random.Range(0, 51);
            SpawnYPoint = Random.Range(0, 31);
        } while ((SpawnXPoint >= 19 && SpawnXPoint <= 30) && (SpawnYPoint >= 10 && SpawnYPoint <= 20));
    }
}
