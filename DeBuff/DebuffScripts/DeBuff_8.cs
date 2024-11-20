using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_8", menuName = "Scriptable Object/DeBuff/DeBuff_8")]
public class DeBuff_8 : DeBuffBase
{
    // 점수 확인 시
    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        scoreData.Additional.Color_Multiply[2] = 0;

        return scoreData;
    }
}
