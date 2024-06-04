using DG.Tweening;
using Fusion;
using TMPro;
using UnityEngine;

public class AngryBar : NetworkBehaviour
{

    public TMP_Text textPlayerName;
    public TMP_Text textAngryBarScore;
    public int GameFinalScore;
    public Transform angryScoreBarImage;

    public GameObject gameSceneCanvas;
    public GameObject gameWinCanvas;
    public GameObject gameLoseCanvas;
    [Networked, OnChangedRender(nameof(UpdateUI))] public int playerScore { get; set; }
    [Networked, OnChangedRender(nameof(GameDone))] public NetworkBool isGameFinish { get; set; }

    public void UpdateAngryBar()
    {
        //RPC_UpdateAngryBar(Runner.LocalPlayer, recipeScore);
        if (playerScore >= GameFinalScore)
        {
            isGameFinish = true;
        }
    }


    //[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    //public void RPC_UpdateAngryBar(PlayerRef playerRef, int recipeScore)
    //{
    //    Debug.Log("playerscore : " + playerScore.ToString());
    //    if (Runner.LocalPlayer != playerRef && playerScore > 5 ) GameService.Instance.playerTask.angryBar.playerScore -= 5;
    //}

    public void UpdateUI()
    {
        angryScoreBarImage.DOScale(new Vector3(playerScore, 0.6f, 1), 0.1f).SetEase(Ease.Linear);
        //textAngryBarScore.text = playerScore.ToString();
    }

    public void GameDone()
    {
        if(GameService.Instance.playerTask.angryBar.playerScore < 30)
        {
            gameLoseCanvas.SetActive(true);
        }
        else
        {
            gameWinCanvas.SetActive(true);
        }
        gameSceneCanvas.SetActive(false);
        GameService.Instance.playerAction.enabled = false;
    }
}
