using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_13", menuName = "Scriptable Object/DeBuff/DeBuff_13")]
public class DeBuff_13 : DeBuffBase
{
    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        scoreData.Additional.Pattern_Multiply[1] = 0;

        return scoreData;
    }
}
