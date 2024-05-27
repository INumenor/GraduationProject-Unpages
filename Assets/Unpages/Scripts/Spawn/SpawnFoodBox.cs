using Cysharp.Threading.Tasks;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using Unpages.Network;
using NetworkPlayer = Unpages.Network.NetworkPlayer;

public class SpawnFoodBox : NetworkBehaviour
{
    [SerializeField] private NetworkObject _spawnBoxPrefab;
    [SerializeField] private NetworkObject _wallPrefab;
    [SerializeField] private GameObject _spawnedBox;
    [SerializeField] private List<NetworkObject> _spawnedFoodBox = new List<NetworkObject>();

    private List<Vector3> _occupiedPositions = new List<Vector3>();
    private int _boxSpawnedCount = 0;
    private int _wallSpawnedCount = 0;
    private void OnEnable()
    {
        NetworkManager.Instance.OnNetworkPlayerCreated += Spawn;
        GameService.Instance.gameControl.BoxSpawn += SpawnMissingBox;
        GameService.Instance.gameControl.BoxDestroy += RemoveListBox;
    }
    private void OnDisable()
    {
        NetworkManager.Instance.OnNetworkPlayerCreated -= Spawn;
        GameService.Instance.gameControl.BoxSpawn -= SpawnMissingBox;
        GameService.Instance.gameControl.BoxDestroy -= RemoveListBox;
    }
    public async void Spawn(PlayerRef playerRef, NetworkPlayer player)
    {
        Debug.Log(playerRef.PlayerId);
        if (NetworkManager.Instance.SessionRunner.IsSharedModeMasterClient && NetworkManager.PlayerList.Count<2)
        {
            for (int i = 0; i < gridAreas.Count; i++)
            {
                if (i == 0 || i == 2)
                {
                    SpawnObjectsInGrid(gridAreas[i], 10, 10);
                }
                else
                {
                    SpawnObjectsInGrid(gridAreas[i], 5, 7);
                   
                }
            }
            await UniTask.WaitForSeconds(5f);
            Debug.Log("bake alýyor");
            //GameService.Instance.mouseStateManager.AreaBake();
        }       
    }
    private List<Vector3[]> gridAreas = new List<Vector3[]>()
    {
        new Vector3[]
        {
            new Vector3(7, 0, 10),
            new Vector3(12, 0, 10),
            new Vector3(12, 0, 20),
            new Vector3(7, 0, 20)
        },
        new Vector3[]
        {
            new Vector3(21, 0, 19),
            new Vector3(28, 0, 19),
            new Vector3(28, 0, 25),
            new Vector3(21, 0, 25)        
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
                    break; 
                Vector3 position = availablePositions[0];
                NetworkObject newBox = NetworkManager.Instance.SessionRunner.Spawn(_spawnBoxPrefab, position, Quaternion.identity);
                newBox.gameObject.transform.SetParent(_spawnedBox.transform, false);
                _occupiedPositions.Add(position);
                _spawnedFoodBox.Add(newBox);
                _boxSpawnedCount++;
                boxSpawned++;
                availablePositions.RemoveAt(0);
            }

            if (wallSpawned < wallCount)
            {
                if (availablePositions.Count == 0)
                    break; 
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
    private int CountBoxesInGrid(Vector3[] area)
    {
        int count = 0;
        foreach (var box in _spawnedFoodBox)
        {
            Debug.Log("1");

            if (IsWithinGrid(area, box.transform.position))
            {
                Debug.Log("2");
                count++;
            }
            Debug.Log(count + " : 3");

        }
        return count;
    }

    private bool IsWithinGrid(Vector3[] area, Vector3 position)
    {
        return position.x > area[0].x && position.x < area[1].x &&
               position.z > area[0].z && position.z < area[2].z;
    }

    public void SpawnMissingBox()
    {     
            for (int i = 0; i < gridAreas.Count; i++)
            {
                Vector3[] area = gridAreas[i];
                int currentBoxCount = CountBoxesInGrid(area);
                Debug.Log("4");
                int requiredBoxCount = (i == 0 || i == 2) ? 10 : 5;
                Debug.Log("5");
                int boxesToSpawn = requiredBoxCount - currentBoxCount;
                Debug.Log("6"); 
                if (boxesToSpawn > 0)
                {            
                    Debug.Log("7");
                    SpawnObjectsInGrid(area, boxesToSpawn, 0);
                }
            }
        GameService.Instance.mouseStateManager.AreaBake();
    }
    public void RemoveListBox(GameObject boxObject)
    {
        _spawnedFoodBox.Remove(boxObject.GetComponent<NetworkObject>());
        Destroy(boxObject);
        //if (_spawnedFoodBox.Count <= 20)
        //{
        //    SpawnMissingBox();
        //}
    }
}
