using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_11", menuName = "Scriptable Object/DeBuff/DeBuff_11")]
public class DeBuff_11 : DeBuffBase
{
    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        scoreData.Additional.Pattern_Multiply[0] = 0;

        return scoreData;
    }
}
