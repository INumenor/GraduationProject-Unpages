using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    bool isActiv = false;
    private async void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (!isActiv && other.CompareTag("Box"))
        {
            isActiv = true;
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            other.gameObject.GetComponent<Rigidbody>().useGravity = false;
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.GetComponent<SoundOneShotPlay>().PlaySound();
            GameService.Instance.gameControl.ObjectSpawn(other.gameObject);
            //Destroy(other.gameObject);
            //other.gameObject.SetActive(false);
        }
        else if (!isActiv && other.CompareTag("Item"))
        {
            isActiv = true;
            if (GameService.Instance.mouseStateManager.expiredFood.Contains(other.GetComponent<NetworkObject>()))
            { 
                GameService.Instance.mouseStateManager.expiredFood.Remove(other.gameObject.GetComponent<NetworkObject>());
                GameService.Instance.mouseStateManager.targetFood = null;
            }
            Destroy(other.gameObject);
        }
        //else if (!isActiv && other.CompareTag("Character"))
        //{
        //    isActiv = true;
        //}
    }
}
