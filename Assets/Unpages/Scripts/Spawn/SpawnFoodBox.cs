using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;
using NetworkPlayer = Unpages.Network.NetworkPlayer;

public class SpawnFoodBox : MonoBehaviour
{
    //[SerializeField] private NetworkObject _spawnBoxPrefab;
    //[SerializeField] private NetworkObject _wall;
    //[SerializeField] private GameObject _spawnedBox;
    //private List<NetworkObject> _spawnedFoodBox = new List<NetworkObject>();
    //private List<NetworkObject> _walls = new List<NetworkObject>();
    //private int _wallSpawnedCount = 0;
    //private int _boxSpawnedCount = 0;
    //int SpawnXPoint;
    //int SpawnYPoint;
    [SerializeField] private NetworkObject _spawnBoxPrefab;
    [SerializeField] private NetworkObject _wallPrefab;
    [SerializeField] private GameObject _spawnedBox;
    private List<NetworkObject> _spawnedFoodBox = new List<NetworkObject>();
    private List<NetworkObject> _spawnedWalls = new List<NetworkObject>();
    private List<Vector3> _occupiedPositions = new List<Vector3>();
    private int _boxSpawnedCount = 0;
    private int _wallSpawnedCount = 0;
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
        if (NetworkManager.Instance.SessionRunner.IsSharedModeMasterClient)
        {
            for (int i = 0; i < gridAreas.Count; i++)
            {
                if (i == 0 || i == 2)
                {
                    Debug.Log("BURAYA kaç kez giriyor");
                    SpawnObjectsInGrid(gridAreas[i], 10, 10);
                }
                else
                {
                    Debug.Log("i artmýyor mu");
                    SpawnObjectsInGrid(gridAreas[i], 5, 7);
                   
                }
            }
            //GameService.Instance.aiManagerSystem.AreaBake();
        }       
    }
    private List<Vector3[]> gridAreas = new List<Vector3[]>()
    {
        new Vector3[]
        {
            new Vector3(5, 0, 10),
            new Vector3(12, 0, 10),
            new Vector3(12, 0, 20),
            new Vector3(5, 0, 20)
        },
        new Vector3[]
        {
            new Vector3(21, 0, 21),
            new Vector3(28, 0, 21),
            new Vector3(28, 0, 27),
            new Vector3(21, 0, 27)        
        },
        new Vector3[]
        {
            new Vector3(37, 0, 10),
            new Vector3(44, 0, 10),
            new Vector3(44, 0, 20),
            new Vector3(37, 0, 20)                       
        },
        new Vector3[]
        {          
            new Vector3(21, 0, 3),
            new Vector3(28, 0, 3),
            new Vector3(28, 0, 9),
            new Vector3(21, 0, 9)                   
        }
    };
    private void SpawnObjectsInGrid(Vector3[] area, int boxCount, int wallCount)
    {
        List<Vector3> availablePositions = new List<Vector3>();

        for (float x = area[0].x + 1; x < area[1].x; x++)
        {
            for (float z = area[0].z + 1; z < area[2].z; z++)
            {
                Vector3 position = new Vector3(x, 0.5f, z);
                if (!_occupiedPositions.Contains(position))
                    availablePositions.Add(position);
            }
        }

        ShuffleList(availablePositions);

        int boxSpawned = 0;
        int wallSpawned = 0;

        while (boxSpawned < boxCount || wallSpawned < wallCount)
        {
            if (boxSpawned < boxCount)
            {
                if (availablePositions.Count == 0)
                    break; // No available positions left
                Vector3 position = availablePositions[0];
                NetworkObject newBox = NetworkManager.Instance.SessionRunner.Spawn(_spawnBoxPrefab, position, Quaternion.identity);
                newBox.gameObject.transform.SetParent(_spawnedBox.transform, false);
                _occupiedPositions.Add(position);
                _boxSpawnedCount++;
                boxSpawned++;
                availablePositions.RemoveAt(0);
            }

            if (wallSpawned < wallCount)
            {
                if (availablePositions.Count == 0)
                    break; // No available positions left
                Vector3 position = availablePositions[0];
                NetworkObject newWall = NetworkManager.Instance.SessionRunner.Spawn(_wallPrefab, new Vector3(position.x, 1.1f, position.z), Quaternion.identity);
                newWall.gameObject.transform.SetParent(_spawnedBox.transform, false);
                _occupiedPositions.Add(position);
                _wallSpawnedCount++;
                wallSpawned++;
                availablePositions.RemoveAt(0);
            }
        }
    }
    private void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    //private void SpawnBoxObject()
    //{
    //    bool validSpawn = false;

    //    while (!validSpawn)
    //    {
    //        GetRandomNumber(out SpawnXPoint, out SpawnYPoint);
    //        Vector3 spawnPosition = new Vector3(Mathf.RoundToInt(SpawnXPoint), 0.5f, Mathf.RoundToInt(SpawnYPoint));

    //        // Kutularýn ve duvarlarýn konumunu ayrý ayrý kontrol et
    //        bool positionOccupied = _spawnedFoodBox.Exists(obj => Mathf.RoundToInt(obj.transform.position.x) == Mathf.RoundToInt(spawnPosition.x) &&
    //                                                           Mathf.RoundToInt(obj.transform.position.z) == Mathf.RoundToInt(spawnPosition.z)) ||
    //                             _walls.Exists(obj => Mathf.RoundToInt(obj.transform.position.x) == Mathf.RoundToInt(spawnPosition.x) &&
    //                                                   Mathf.RoundToInt(obj.transform.position.z) == Mathf.RoundToInt(spawnPosition.z));
    //        if (!positionOccupied)
    //        {
    //            NetworkObject newFood = NetworkManager.Instance.SessionRunner.Spawn(_spawnBoxPrefab, spawnPosition, _spawnBoxPrefab.transform.rotation);
    //            newFood.gameObject.transform.SetParent(_spawnedBox.transform, false);
    //            _spawnedFoodBox.Add(newFood);
    //            _boxSpawnedCount++;
    //            validSpawn = true;
    //        }
    //    }
    //}

    //private void SpawnWallObject()
    //{
    //    bool validSpawn = false;

    //    while (!validSpawn)
    //    {
    //        GetRandomNumber(out SpawnXPoint, out SpawnYPoint);
    //        Vector3 spawnPosition = new Vector3(Mathf.RoundToInt(SpawnXPoint), 1.1f, Mathf.RoundToInt(SpawnYPoint));

    //        // Kutularýn ve duvarlarýn konumunu ayrý ayrý kontrol et
    //        bool positionOccupied = _spawnedFoodBox.Exists(obj => Vector3Int.RoundToInt(obj.transform.position) == Vector3Int.RoundToInt(spawnPosition)) ||
    //                                _walls.Exists(obj => Vector3Int.RoundToInt(obj.transform.position) == Vector3Int.RoundToInt(spawnPosition));

    //        if (!positionOccupied)
    //        {
    //            NetworkObject newWall = NetworkManager.Instance.SessionRunner.Spawn(_wall, spawnPosition, _wall.transform.rotation);
    //            newWall.gameObject.transform.SetParent(_spawnedBox.transform, false);
    //            _walls.Add(newWall);
    //            _wallSpawnedCount++;
    //            validSpawn = true;
    //        }
    //    }
    //}
    //private void GetRandomNumber(out int SpawnXPoint, out int SpawnYPoint)
    //{
    //    do
    //    {
    //        SpawnXPoint = Random.Range(0, 51);
    //        SpawnYPoint = Random.Range(0, 31);
    //    } while ((SpawnXPoint >= 19 && SpawnXPoint <= 30) && (SpawnYPoint >= 10 && SpawnYPoint <= 20));
    //}
}
