using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("TouchableItem"))
        {
            GameService.Instance.gameControl.ObjectSpawn(other.gameObject);
            //Destroy(other.gameObject);
            //other.gameObject.SetActive(false);
        }
    }
}
