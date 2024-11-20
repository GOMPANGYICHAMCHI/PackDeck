using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jocker_27", menuName = "Scriptable Object/Jocker/Jocker_27")]
public class Jocker_27 : JockerBase
{
    [Header("파랑 버프 c 곱산양")]
    public float mulBuff;

    [Header("나머지 색 디버프 c 곱산양")]
    public float mulDeBuff;

    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        for (int i = 0; i < scoreData.colorCardCount[2]; i++)
        {
            scoreData.Additional.Color_Multiply[2] *= mulBuff;
        }

        if (scoreData.colorCardCount[1] != 0 || scoreData.colorCardCount[0] != 0)
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
