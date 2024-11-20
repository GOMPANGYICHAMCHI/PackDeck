using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jocker_16", menuName = "Scriptable Object/Jocker/Jocker_16")]
public class Jocker_16 : JockerBase
{
    [Header("b + addAmount")]
    public int addAmount;

    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        if (scoreData.colorCardCount[0] != 0)
        {
            scoreData.Additional.Color_B[0] += addAmount;
        }

        return scoreData;
    }
}
