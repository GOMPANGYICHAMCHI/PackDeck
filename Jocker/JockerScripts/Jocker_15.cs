using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jocker_15", menuName = "Scriptable Object/Jocker/Jocker_15")]
public class Jocker_15 : JockerBase
{
    [Header("b + addAmount")]
    public int addAmount;

    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        if (scoreData.colorCardCount[2] != 0)
        {
            scoreData.Additional.Color_B[2] += addAmount;
        }

        return scoreData;
    }
}
