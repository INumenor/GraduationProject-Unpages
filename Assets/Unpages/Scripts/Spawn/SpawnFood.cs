using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFood : MonoBehaviour
{
    [SerializeField] private GameObject _spawnFoodPrefab;
    int GameAreaWidth;
    int GameAreaHeight;
    private void Start()
    {
        int index = 0;
        while (index < 50)
        {
            GetRandomNumber();
            Instantiate(_spawnFoodPrefab, new Vector3(Mathf.RoundToInt(GameAreaWidth),
                 0.1f, Mathf.RoundToInt(GameAreaHeight)),
                 _spawnFoodPrefab.transform.rotation);
            index++;
        }
    }
    public int GetRandomNumber()
    {
        do
        {
            GameAreaWidth = Random.Range(0, 51); // 0 ile 50 arasýnda bir sayý (51 hariç)
            GameAreaHeight = Random.Range(0, 31); 
        } while (GameAreaWidth < 11 || GameAreaWidth > 21);
        return GameAreaWidth;
    }
}
