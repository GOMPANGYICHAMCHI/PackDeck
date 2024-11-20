using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_19", menuName = "Scriptable Object/DeBuff/DeBuff_19")]
public class DeBuff_19 : DeBuffBase
{
    [Header("증가 액션 코스트")]
    public int[] AdditionalActionCost = new int[6];

    int curIndex = 0;

    public override void Event_StartScoreCheck(PlayerData playerdata) 
    {
        playerdata.Add_CurrentActionCost(1);
        playerdata.Add_CurrentActionCost(-AdditionalActionCost[curIndex]);
        curIndex++;
        Mathf.Clamp(curIndex, 0, AdditionalActionCost.Length - 1);
    }
}
