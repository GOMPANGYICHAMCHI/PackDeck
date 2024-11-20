using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jocker_25", menuName = "Scriptable Object/Jocker/Jocker_25")]
public class Jocker_25 : JockerBase
{
    [Header("b + addAmount")]
    public int addAmount;

    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        bool isConfirmed = true;
        List<Card> handCards = playerData.Get_HandDeckAll();

        for (int i = 0; i < handCards.Count; i++)
        {
            if (handCards[i].patternIndex != 1 || handCards[i].colorIndex != 1)
            {
                isConfirmed = false;
                break;
            }
        }

        if (isConfirmed)
        {
            for (int i = 0; i < scoreData.Additional.patternCount; i++)
            {
                scoreData.Additional.Pattern_B[i] += addAmount;
            }

            for (int i = 0; i < scoreData.Additional.colorCount; i++)
            {
                scoreData.Additional.Color_B[i] += addAmount;
            }
        }

        return scoreData;
    }
}
