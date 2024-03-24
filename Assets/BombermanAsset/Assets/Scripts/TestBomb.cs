using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBomb : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject explosionPrefab;
    void Start()
    {
        Invoke("Explode", 3f);
    }

    void Explode()
    {
        GameObject Explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity); //1
        Explosion.GetComponent<BombExplosion>().Init(GameService.Instance.playerAction.bombManager.bombSize);
        GetComponent<MeshRenderer>().enabled = false; //2
        transform.Find("Collider").gameObject.SetActive(false); //3
        Destroy(gameObject, .3f); //4
        GameService.Instance.playerAction.bombManager.bombCounter++;
    }

}
