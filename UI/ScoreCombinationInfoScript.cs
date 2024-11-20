using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCombinationInfoScript : MonoBehaviour
{
    public TMP_Text mainText;
    public TMP_Text score_A;
    public TMP_Text score_B;
    public Image ExampleImage;

    public void SetText(string main,string input_scoreA, string input_scoreB)
    {
        mainText.text = main;
        score_A.text = input_scoreA;
        score_B.text = input_scoreB;
    }

    public void UpdateScore(string input_scoreA, string input_scoreB)
    {
        score_A.text = input_scoreA;
        score_B.text = input_scoreB;
    }
}
