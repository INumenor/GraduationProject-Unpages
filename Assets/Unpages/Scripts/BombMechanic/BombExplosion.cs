using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BombExplosion : MonoBehaviour
{
    public GameObject bombExplosionParticalPrefabs;

    public void Init(int ExplosionSize) 
    {
        Instantiate(bombExplosionParticalPrefabs, transform.position, Quaternion.identity,transform);
        gameObject.AddComponent<BoxCollider>().transform.position = transform.position;
        for (int i = 1; i <= ExplosionSize; i++)
        {
            Instantiate(bombExplosionParticalPrefabs, new Vector3(transform.position.x + i, transform.position.y, transform.position.z), Quaternion.identity, transform);
            Instantiate(bombExplosionParticalPrefabs, new Vector3(transform.position.x - i, transform.position.y, transform.position.z), Quaternion.identity, transform);
            Instantiate(bombExplosionParticalPrefabs, new Vector3(transform.position.x, transform.position.y, transform.position.z + i), Quaternion.identity, transform);
            Instantiate(bombExplosionParticalPrefabs, new Vector3(transform.position.x, transform.position.y, transform.position.z - i), Quaternion.identity, transform);
            //Instantiate(bombExplosionParticalPrefabs, new Vector3(transform.position.x + i, transform.position.y, transform.position.z - i), Quaternion.identity, transform);
            //Instantiate(bombExplosionParticalPrefabs, new Vector3(transform.position.x + i, transform.position.y, transform.position.z + i), Quaternion.identity, transform);
            //Instantiate(bombExplosionParticalPrefabs, new Vector3(transform.position.x - i, transform.position.y, transform.position.z - i), Quaternion.identity, transform);
            //Instantiate(bombExplosionParticalPrefabs, new Vector3(transform.position.x - i, transform.position.y, transform.position.z + i), Quaternion.identity, transform);
        }
        GameService.Instance.playerAction.bombManager.bombCounter++;
        Destroy(gameObject, .3f);
    }
}
