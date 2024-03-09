using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject pObject;
  
    public void PlayerGrabAndDrop(GameObject cube = null)
    {
        if (pObject == null)
        {
            cube.transform.SetParent(this.transform, true);
            pObject = cube;
                     
        }
        //else if (pObject != null)
        //{
        //    Debug.Log("Tsasda");  
        //    pObject.transform.parent = null;
        //}
    }
  
}
