using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using Unpages.Network;

public class AngryBar : NetworkBehaviour
{

    public TMP_Text textPlayerName;
    public TMP_Text textAngryBarScore;
    public int GameFinalScore;

    public GameObject gameSceneCanvas;
    public GameObject gameFinishCanvas;
    [Networked ,OnChangedRender(nameof(UpdateUI))] public int playerScore {get; set;}
    [Networked, OnChangedRender(nameof(GameDone))] public NetworkBool isGameFinish {get; set;}

    public void UpdateAngryBar(int recipeScore)
    {
        RPC_UpdateAngryBar(Runner.LocalPlayer,recipeScore);
        if(playerScore > GameFinalScore)
        {
            isGameFinish = true;
        }
    }


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_UpdateAngryBar(PlayerRef playerRef,int recipeScore)
    {
        if(Runner.LocalPlayer != playerRef)GameService.Instance.playerTask.angryBar.playerScore -= 5;
    }

    public void UpdateUI()
    {
        textAngryBarScore.text = playerScore.ToString();
    }

    public void GameDone()
    {
        gameFinishCanvas.SetActive(true);
        gameSceneCanvas.SetActive(false);
        GameService.Instance.playerAction.enabled = false;
        Debug.Log("OYUNNNNNN BÝTTTTTÝ");
    }
}
