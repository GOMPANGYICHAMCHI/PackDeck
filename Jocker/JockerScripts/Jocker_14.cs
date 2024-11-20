using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jocker_14", menuName = "Scriptable Object/Jocker/Jocker_14")]
public class Jocker_14 : JockerBase
{
    [Header("c + addAmount")]
    public int addAmount;

    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        bool isConfirmed = true;

        for (int i = 0; i < scoreData.Additional.patternCount; i++)
        {
            if (scoreData.patternCardCount[i] == 0)
                isConfirmed = false;

            if (!isConfirmed)
                break;
        }

        if (isConfirmed)
        {
            for (int i = 0; i < scoreData.Additional.patternCount; i++)
            {
                scoreData.Additional.Pattern_Multiply[i] += addAmount;
            }
        }

        return scoreData;
    }
}
