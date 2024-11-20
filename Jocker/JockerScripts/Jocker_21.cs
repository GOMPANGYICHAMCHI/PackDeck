using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jocker_21", menuName = "Scriptable Object/Jocker/Jocker_21")]
public class Jocker_21 : JockerBase
{
    [Header("효과 발동 조건 카드 수(이하)")]
    public int cardCountForBuff;

    [Header("c + addAmount")]
    public int addAmount;

    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        int entireCount = 0;

        for (int i = 0; i < scoreData.Additional.patternCount; i++)
        {
            entireCount += scoreData.patternCardCount[i];
            if (entireCount > cardCountForBuff)
                break;
        }

        for (int i = 0; i < scoreData.Additional.colorCount; i++)
        {
            entireCount += scoreData.colorCardCount[i];
            if (entireCount > cardCountForBuff)
                break;
        }

        if(entireCount <= cardCountForBuff)
        {
            for (int i = 0; i < scoreData.Additional.patternCount; i++)
            {
                scoreData.Additional.Pattern_Multiply[i] += addAmount;
            }

            for (int i = 0; i < scoreData.Additional.colorCount; i++)
            {
                scoreData.Additional.Color_Multiply[i] += addAmount;
            }
        }

        return scoreData;
    }
}
