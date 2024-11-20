using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jocker_1", menuName = "Scriptable Object/Jocker/Jocker_1")]
public class Jocker_1 : JockerBase
{
    [Header("a + addAmount")]
    public int addAmount;

    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        for (int i = 0; i < scoreData.Additional.patternCount; i++) 
        {
            scoreData.Additional.Pattern_A[i] += addAmount;
        }

        for (int i = 0; i < scoreData.Additional.colorCount; i++)
        {
            scoreData.Additional.Color_A[i] += addAmount;
        }

        return scoreData;
    }
}
