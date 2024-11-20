using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_15", menuName = "Scriptable Object/DeBuff/DeBuff_15")]
public class DeBuff_15 : DeBuffBase
{
    [Header("c * mulAmount")]
    public float mulAmount;

    bool[] patternUsed;
    bool[] colorUsed;

    public override void Event_StageStart_Pre(PlayerData playerdata) 
    {
        patternUsed = new bool[playerdata.PatternCount];
        colorUsed = new bool[playerdata.ColorCount];

        Array.Fill(patternUsed, false);
        Array.Fill(colorUsed, false);
    }

    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        bool isDebuff = false;

        for (int i = 0; i < scoreData.patternCount; i++)
        {
            if (scoreData.patternCardCount[i] != 0)
            {
                if (patternUsed[i])
                {
                    isDebuff = true;
                    break;
                }
                else
                    patternUsed[i] = true;
            }
        }
        if (!isDebuff)
        {
            for (int i = 0; i < scoreData.colorCount; i++)
            {
                if (scoreData.colorCardCount[i] != 0)
                {
                    if (colorUsed[i])
                    {
                        isDebuff = true;
                        break;
                    }
                    else
                        colorUsed[i] = true;
                }
            }
        }
        
        if(isDebuff)
        {
            for (int i = 0; i < scoreData.patternCount; i++)
            {
                scoreData.Additional.Pattern_Multiply[i] *= mulAmount;
            }
            for (int i = 0; i < scoreData.colorCount; i++)
            {
                scoreData.Additional.Color_Multiply[i] *= mulAmount;
            }
        }

        return scoreData;
    }
}
