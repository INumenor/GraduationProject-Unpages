using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    bool isActiv = false;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (!isActiv && other.CompareTag("TouchableItem"))
        {
            isActiv = true;
            GameService.Instance.gameControl.ObjectSpawn(other.gameObject);
            //Destroy(other.gameObject);
            //other.gameObject.SetActive(false);
        }
        else if (!isActiv && other.CompareTag("Item"))
        {
            isActiv = true;
            Destroy(other.gameObject);
        }
        //else if (!isActiv && other.CompareTag("Character"))
        //{
        //    isActiv = true;
        //}
    }
}
