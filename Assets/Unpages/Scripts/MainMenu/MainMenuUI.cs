using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public GameObject currentCanvas;
    
    public GameObject optionsCanvas;
    public GameObject mainMenuCanvas;
    public GameObject playMenuCanvas;

    private void Start()
    {
        currentCanvas = mainMenuCanvas;
    }

    public void OptionsButton()
    {
        currentCanvas.SetActive(false);
        currentCanvas = optionsCanvas;
        currentCanvas.SetActive(true);
    }

    public void MainMenuButton()
    {
        currentCanvas.SetActive(false);
        currentCanvas = mainMenuCanvas;
        currentCanvas.SetActive(true);
    }

    public void PlayMenuButton()
    {
        currentCanvas.SetActive(false);
        currentCanvas = playMenuCanvas;
        currentCanvas.SetActive(true);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
