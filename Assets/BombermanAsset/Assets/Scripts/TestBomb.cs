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
        Instantiate(explosionPrefab, transform.position, Quaternion.identity); //1

        GetComponent<MeshRenderer>().enabled = false; //2
        transform.Find("Collider").gameObject.SetActive(false); //3
        Destroy(gameObject, .3f); //4
    }

}
