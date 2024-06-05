using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviourSingleton<UIController>
{
    public TMP_Text HighestScoreText;
    public TMP_Text ScoreText;

    public void SetHighestScoreText(int score)
    {
        HighestScoreText.text = "Highest: " + score.ToString();
    }

    public void SetScoreText(int score)
    {
        ScoreText.text = score.ToString();
    }
}
