using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_21", menuName = "Scriptable Object/DeBuff/DeBuff_21")]
public class DeBuff_21 : DeBuffBase
{
    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        int rand_index = Random.Range(0, playerData.PatternCount);

        for (int i = 0; i < playerData.PatternCount; i++)
        {
            if(i !=  rand_index)
            {
                scoreData.patternCardCount[rand_index] += scoreData.patternCardCount[i];
                scoreData.patternNumberSum[rand_index] += scoreData.patternNumberSum[i];

                scoreData.patternCardCount[i] = 0;
                scoreData.patternNumberSum[i] = 0;
            }
        }

        return scoreData;
    }
}
