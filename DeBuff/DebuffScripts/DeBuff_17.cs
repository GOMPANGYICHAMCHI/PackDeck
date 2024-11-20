using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_17", menuName = "Scriptable Object/DeBuff/DeBuff_17")]
public class DeBuff_17 : DeBuffBase
{
    bool isActive = true;
    int playerJockerCount;

    public override void Event_StageStart_Pre(PlayerData playerdata) 
    {
        playerJockerCount = playerdata.Get_PlayerJockerCount();
    }

    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        if(playerJockerCount != playerData.Get_PlayerJockerCount())
        {
            isActive = false;
        }

        if(isActive)
        {
            for (int i = 0; i < scoreData.patternCount; i++)
            {
                scoreData.patternCardCount[i] = 0;
                scoreData.patternNumberSum[i] = 0;
            }
            for (int i = 0; i < scoreData.colorCount; i++)
            {
                scoreData.colorCardCount[i] = 0;
                scoreData.colorNumberSum[i] = 0;
            }
        }

        return scoreData;
    }
}