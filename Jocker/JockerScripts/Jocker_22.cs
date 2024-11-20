using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jocker_22", menuName = "Scriptable Object/Jocker/Jocker_22")]
public class Jocker_22 : JockerBase
{
    [Header("a + addAmount")]
    public int addAmount;

    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        int curDumpCost = playerData.Get_CurrentDumpCost();

        for (int i = 0; i < scoreData.Additional.patternCount; i++)
        {
            scoreData.Additional.Pattern_A[i] += curDumpCost * addAmount;
        }

        for (int i = 0; i < scoreData.Additional.colorCount; i++)
        {
            scoreData.Additional.Color_A[i] += curDumpCost * addAmount;
        }

        return scoreData;
    }
}
