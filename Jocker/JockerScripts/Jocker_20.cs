using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jocker_20", menuName = "Scriptable Object/Jocker/Jocker_20")]
public class Jocker_20 : JockerBase
{
    [Header("b + addAmount")]
    public int addAmount;

    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        if (scoreData.patternCardCount[1] != 0)
        {
            scoreData.Additional.Pattern_B[1] += addAmount;
        }

        return scoreData;
    }
}
