using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_12", menuName = "Scriptable Object/DeBuff/DeBuff_12")]
public class DeBuff_12 : DeBuffBase
{
    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        scoreData.Additional.Pattern_Multiply[2] = 0;

        return scoreData;
    }
}
