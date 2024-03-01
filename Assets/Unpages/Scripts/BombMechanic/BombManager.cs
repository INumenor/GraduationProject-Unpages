using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BombManager : MonoBehaviour
{
    [SerializeField] GameObject Player;

    [SerializeField] private GameObject bombPrefab;
    public static BombManager BombInstance { get; set; }

    public void DropBomb()
    {
        Debug.Log("sA");
              Instantiate(bombPrefab, new Vector3(Mathf.RoundToInt(Player.transform.position.x),
              bombPrefab.transform.position.y, Mathf.RoundToInt(Player.transform.position.z)),
              bombPrefab.transform.rotation);
    }
}
