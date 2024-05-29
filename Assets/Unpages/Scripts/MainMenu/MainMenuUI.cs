using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public GameObject optionsCanvas;

    public void OptionsButton()
    {
        optionsCanvas.active = true;
    }
}
