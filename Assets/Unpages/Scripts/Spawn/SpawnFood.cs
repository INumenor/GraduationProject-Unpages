using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpawnFood : MonoBehaviour
{
    [SerializeField] private GameObject _spawnFoodPrefab;
    private List<GameObject> _spawnedFoodBox = new List<GameObject>();
   // private List<Vector3> _spawnedPositions = new List<Vector3>();
    private int _maxSpawnAttempts = 50;
    private int _spawnedCount = 0;

    private void Update()
    {
        SpawnBoxObject();
    }
    private void SpawnBoxObject()
    {
        if (_spawnedCount >= _maxSpawnAttempts)
        {
            return;
        }
        int SpawnBoxXPoint;
        int SpawnBoxYPoint;
        bool validSpawn = false;

        while (!validSpawn && _spawnedCount < _maxSpawnAttempts)
        {
            GetRandomNumber(out SpawnBoxXPoint, out SpawnBoxYPoint);
            Vector3 spawnPosition = new Vector3(Mathf.RoundToInt(SpawnBoxXPoint), 0.1f, Mathf.RoundToInt(SpawnBoxYPoint));

            bool positionOccupied = _spawnedFoodBox.Exists(food => Vector3.Distance(food.transform.position, spawnPosition) < 0.1f);

            if (!positionOccupied)
            {

                GameObject newFood = Instantiate(_spawnFoodPrefab, spawnPosition, _spawnFoodPrefab.transform.rotation);
                _spawnedFoodBox.Add(newFood);               
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
        } while ((SpawnBoxXPoint > 19 && SpawnBoxXPoint < 31) || (SpawnBoxYPoint>10 && SpawnBoxYPoint<20));
    }

}
