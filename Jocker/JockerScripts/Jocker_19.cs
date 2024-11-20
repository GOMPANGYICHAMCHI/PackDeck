using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jocker_19", menuName = "Scriptable Object/Jocker/Jocker_19")]
public class Jocker_19 : JockerBase
{
    [Header("b + addAmount")]
    public int addAmount;

    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        if (scoreData.patternCardCount[2] != 0)
        {
            scoreData.Additional.Pattern_B[2] += addAmount;
        }

        return scoreData;
    }
}
