using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    int tomatoCount = 0;
    int cheeseCount = 0;
    int lettuceCount = 0;
    private void OnCollisionEnter(Collision other)
    {
            if (this.gameObject.name == other.gameObject.name)
            {
                switch (other.gameObject.name)
                {
                    case "TomatoFood":
                        tomatoCount++;
                    Destroy(other.gameObject);
                        break;
                    case "CheeseFood":
                        cheeseCount++;
                    Destroy(other.gameObject);
                    break;
                    case "LettuceFood":
                        lettuceCount++;
                    Destroy(other.gameObject);
                    break;
                }
            }
     }
}
