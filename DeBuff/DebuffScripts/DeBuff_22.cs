using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_22", menuName = "Scriptable Object/DeBuff/DeBuff_22")]
public class DeBuff_22 : DeBuffBase
{
    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        int rand_index = Random.Range(0, playerData.ColorCount);

        for (int i = 0; i < playerData.ColorCount; i++)
        {
            if (i != rand_index)
            {
                scoreData.colorCardCount[rand_index] += scoreData.colorCardCount[i];
                scoreData.colorNumberSum[rand_index] += scoreData.colorNumberSum[i];

                scoreData.colorCardCount[i] = 0;
                scoreData.colorNumberSum[i] = 0;
            }
        }

        return scoreData;
    }
}
