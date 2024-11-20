using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_2", menuName = "Scriptable Object/DeBuff/DeBuff_2")]
public class DeBuff_2 : DeBuffBase
{
    [Header("목표점수 배수")]
    public int GoalScoreMultiplyAmount;

    public override void Event_StageStart_Post(PlayerData playerdata)
    {
        playerdata.Multiply_RemainHealth(GoalScoreMultiplyAmount);
    }
}
