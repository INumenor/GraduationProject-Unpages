using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BombManager : MonoBehaviour
{

    [SerializeField] private GameObject bombPrefab;

    public void DropBomb(GameObject playerPreb)
    {
        Instantiate(bombPrefab, new Vector3(Mathf.RoundToInt(playerPreb.transform.position.x),
        bombPrefab.transform.position.y, Mathf.RoundToInt(playerPreb.transform.position.z)),
        bombPrefab.transform.rotation);
    }
}
