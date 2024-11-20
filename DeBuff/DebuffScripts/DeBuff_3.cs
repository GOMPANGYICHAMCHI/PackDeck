using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_3", menuName = "Scriptable Object/DeBuff/DeBuff_3")]
public class DeBuff_3 : DeBuffBase
{
    [Header("c *= mulAmount")]
    public float mulAmount;

    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        for (int i = 0; i < scoreData.patternCount; i++) 
        {
            scoreData.Additional.Pattern_Multiply[i] *= mulAmount;
        }
        for (int i = 0; i < scoreData.colorCount; i++) 
        {
            scoreData.Additional.Color_Multiply[i] *= mulAmount;
        }

        return scoreData;
    }
}
