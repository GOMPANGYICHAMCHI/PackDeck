using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDebugPannel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txt_PannelNumber;

    [SerializeField]
    private TMP_Text[] txt_PatternCount;
    [SerializeField]
    private TMP_Text[] txt_PatternNumberSum;
    [SerializeField]
    private TMP_Text[] txt_PatternAddAmount;
    [SerializeField]
    private TMP_Text[] txt_PatternMultiply;

    [SerializeField]
    private TMP_Text[] txt_ColorCount;
    [SerializeField]
    private TMP_Text[] txt_ColorNumberSum;
    [SerializeField]
    private TMP_Text[] txt_ColorAddAmount;
    [SerializeField]
    private TMP_Text[] txt_ColorMultiply;

    [SerializeField]
    private TMP_Text txt_FianlScore;
    [SerializeField]
    private TMP_Text txt_Stage;

    [SerializeField]
    private Transform trans_cardHolder;

    private Sprite[] sprites;
    private Color32[] colors;

    public void SetCardAsset(Sprite[] input_sprite, Color32[] input_color)
    {
        sprites = input_sprite;
        colors = input_color;
    }

    public void SetInfo(ScoreData scoreData, int index, long finalScore, GameObject EmptyCard, List<Card> selectedDeck, int stage)
    {
        txt_PannelNumber.text = index.ToString();

        for(int i = 0; i < scoreData.patternCardCount.Length; i++) 
        {
            txt_PatternCount[i].text = scoreData.patternCardCount[i].ToString();
            txt_PatternNumberSum[i].text = scoreData.patternNumberSum[i].ToString();
            txt_PatternAddAmount[i].text =
                scoreData.Additional.Pattern_B[i].ToString() + "/" +
                scoreData.Additional.Pattern_A[i].ToString();

            txt_PatternMultiply[i].text = scoreData.Additional.Pattern_Multiply[i].ToString();
        }

        for (int i = 0; i < scoreData.colorCardCount.Length; i++)
        {
            txt_ColorCount[i].text = scoreData.colorCardCount[i].ToString();
            txt_ColorNumberSum[i].text = scoreData.colorNumberSum[i].ToString();
            txt_ColorAddAmount[i].text =
                scoreData.Additional.Color_B[i].ToString() + "/" +
                scoreData.Additional.Color_A[i].ToString();

            txt_ColorMultiply[i].text = scoreData.Additional.Color_Multiply[i].ToString();
        }

        txt_FianlScore.text = finalScore.ToString();
        txt_Stage.text = "Stage" + stage.ToString();

        for (int i = 0; i < selectedDeck.Count; i++) 
        {
            CardBasic temp = Instantiate(EmptyCard, trans_cardHolder).GetComponent<CardBasic>();
            temp.SetCardApear(sprites[selectedDeck[i].patternIndex], colors[selectedDeck[i].colorIndex], selectedDeck[i].number);
        }
    }
}
