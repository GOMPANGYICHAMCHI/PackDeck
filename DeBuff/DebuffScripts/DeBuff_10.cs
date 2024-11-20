using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_10", menuName = "Scriptable Object/DeBuff/DeBuff_10")]
public class DeBuff_10 : DeBuffBase
{
    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        scoreData.Additional.Color_Multiply[1] = 0;

        return scoreData;
    }
}
