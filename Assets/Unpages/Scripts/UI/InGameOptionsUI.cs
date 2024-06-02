using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;

public class InGameOptionsUI : MonoBehaviour
{
    [SerializeField] GameObject optionsCanvasUI;
    public void OptionsCanvasOpen()
    {
        optionsCanvasUI.SetActive(true);
    }

    public void OptionsCanvasClose()
    {
        optionsCanvasUI.SetActive(false);
    }

    public void ExitGame()
    {
        NetworkManager.Instance.Disconnect();
    }
}
