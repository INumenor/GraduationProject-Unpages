using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AngryBar : NetworkBehaviour
{

    public TMP_Text textPlayerName;
    public TMP_Text textAngryBarScore;
    [Networked ,OnChangedRender(nameof(UpdateUI))] public int playerScore {get; set;}

    public void UpdateAngryBar(int recipeScore)
    {
        RPC_UpdateAngryBar(Runner.LocalPlayer,recipeScore);
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
}
