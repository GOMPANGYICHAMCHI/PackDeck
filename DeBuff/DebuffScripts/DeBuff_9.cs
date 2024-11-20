using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_9", menuName = "Scriptable Object/DeBuff/DeBuff_9")]
public class DeBuff_9 : DeBuffBase
{
    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        scoreData.Additional.Color_Multiply[0] = 0;

        return scoreData;
    }
}
