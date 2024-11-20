using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jocker_17", menuName = "Scriptable Object/Jocker/Jocker_17")]
public class Jocker_17 : JockerBase
{
    [Header("b + addAmount")]
    public int addAmount;

    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        if (scoreData.colorCardCount[1] != 0)
        {
            scoreData.Additional.Color_B[1] += addAmount;
        }

        return scoreData;
    }
}
