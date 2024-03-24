using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;
using NetworkPlayer = Unpages.Network.NetworkPlayer;

public class SpawnFoodBox : MonoBehaviour
{
    [SerializeField] private NetworkObject _spawnBoxPrefab;
    [SerializeField] private GameObject _spawnedBox;
    private List<NetworkObject> _spawnedFoodBox = new List<NetworkObject>();
    private List<NetworkObject> _spawnedUntouchableBox = new List<NetworkObject>();
    private int _maxSpawnAttempts = 50;
    private int _spawnedCount = 0;
    int SpawnBoxXPoint;
    int SpawnBoxYPoint;

    private void OnEnable()
    {
        NetworkManager.Instance.OnNetworkPlayerCreated += SpawnBox;
    }
    private void OnDisable()
    {
        NetworkManager.Instance.OnNetworkPlayerCreated -= SpawnBox;
    }
    public void SpawnBox(PlayerRef playerRef, NetworkPlayer player)
    {
        Debug.Log(playerRef.PlayerId);
        if (playerRef.PlayerId==1)
        {
            while (_spawnedCount < 50)
            {
                SpawnUntouchableBoxObject();              
            }
        }       
    }

    private void SpawnBoxObject()
    {
    
        bool validSpawn = false;

        while (!validSpawn)
        {
            GetRandomNumber(out SpawnBoxXPoint, out SpawnBoxYPoint);
            Vector3 spawnPosition = new Vector3(Mathf.RoundToInt(SpawnBoxXPoint), 0.5f, Mathf.RoundToInt(SpawnBoxYPoint));

            bool positionOccupied = _spawnedFoodBox.Exists(food => Vector3.Distance(food.transform.position, spawnPosition) < 0.1f);

            if (!positionOccupied)
            {

                NetworkObject newFood = NetworkManager.Instance.SessionRunner.Spawn(_spawnBoxPrefab, spawnPosition, _spawnBoxPrefab.transform.rotation);
                newFood.gameObject.transform.SetParent(_spawnedBox.transform,false);
                _spawnedFoodBox.Add(newFood);
                _spawnedCount++;
                validSpawn = true;
            }
        }
    }
    private void SpawnUntouchableBoxObject()
    {

        bool validSpawn = false;

        while (!validSpawn)
        {
            GetRandomNumber(out SpawnBoxXPoint, out SpawnBoxYPoint);
            Vector3 spawnPosition = new Vector3(Mathf.RoundToInt(SpawnBoxXPoint), 0.5f, Mathf.RoundToInt(SpawnBoxYPoint));

            bool positionOccupied = _spawnedFoodBox.Exists(food => Vector3.Distance(food.transform.position, spawnPosition) < 0.1f);

            if (!positionOccupied)
            {

                NetworkObject newFood = NetworkManager.Instance.SessionRunner.Spawn(_spawnBoxPrefab, spawnPosition, _spawnBoxPrefab.transform.rotation);
                newFood.gameObject.transform.SetParent(_spawnedBox.transform, false);
                _spawnedUntouchableBox.Add(newFood);
                _spawnedCount++;
                validSpawn = true;
            }
        }
    }
    private void GetRandomNumber(out int SpawnBoxXPoint, out int SpawnBoxYPoint)
    {
        do
        {
            SpawnBoxXPoint = Random.Range(0, 51);
            SpawnBoxYPoint = Random.Range(0, 31);
        } while ((SpawnBoxXPoint > 18 && SpawnBoxXPoint < 31) || (SpawnBoxYPoint > 9 && SpawnBoxYPoint < 20));
    }
}
