using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jocker_18", menuName = "Scriptable Object/Jocker/Jocker_18")]
public class Jocker_18 : JockerBase
{
    [Header("b + addAmount")]
    public int addAmount;

    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        if (scoreData.patternCardCount[0] != 0)
        {
            scoreData.Additional.Pattern_B[0] += addAmount;
        }

        return scoreData;
    }
}
