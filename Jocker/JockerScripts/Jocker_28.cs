using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jocker_28", menuName = "Scriptable Object/Jocker/Jocker_28")]
public class Jocker_28 : JockerBase
{
    [Header("그린 버프 c 곱산양")]
    public float mulBuff;

    [Header("나머지 색 디버프 c 곱산양")]
    public float mulDeBuff;

    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        for (int i = 0; i < scoreData.colorCardCount[1]; i++)
        {
            scoreData.Additional.Color_Multiply[1] *= mulBuff;
        }

        if (scoreData.colorCardCount[0] != 0 || scoreData.colorCardCount[2] != 0)
        {
            for (int i = 0; i < scoreData.Additional.patternCount; i++)
            {
                scoreData.Additional.Pattern_Multiply[i] *= mulDeBuff;
            }

            for (int i = 0; i < scoreData.Additional.colorCount; i++)
            {
                scoreData.Additional.Color_Multiply[i] *= mulDeBuff;
            }
        }

        return scoreData;
    }
}
