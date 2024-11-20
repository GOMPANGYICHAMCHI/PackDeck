using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jocker_2", menuName = "Scriptable Object/Jocker/Jocker_2")]
public class Jocker_2 : JockerBase
{
    [Header("a + addAmount")]
    public int addAmount;

    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        for (int i = 0; i < scoreData.Additional.patternCount; i++) 
        {
            scoreData.Additional.Pattern_A[i] += addAmount;
        }

        return scoreData;
    }
}
