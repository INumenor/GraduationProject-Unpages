using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenAreaDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.tag == "Player")
        {
            GameService.Instance.playerAction.isOpen = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameService.Instance.playerAction.isOpen = false;
        }
    }
}
