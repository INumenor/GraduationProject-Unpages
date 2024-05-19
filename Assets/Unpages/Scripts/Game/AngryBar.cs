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

    public void UpdateUI()
    {
        textAngryBarScore.text = playerScore.ToString();
    }
}
