using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jocker_10", menuName = "Scriptable Object/Jocker/Jocker_10")]
public class Jocker_10 : JockerBase
{
    [Header("보상 지급 요구 스테이지")]
    public int RewardGiveStage;

    [Header("골드 추가 정도")]
    public int addGoldAmount;

    int stageCheck = 0;

    public override void Event_StageEnd(PlayerData playerData) 
    {
        stageCheck++;

        if(stageCheck == RewardGiveStage)
        {
            playerData.Add_AddGoldAmount(addGoldAmount);
            playerData.Remove_PlayerJocker(this);
        }
    }
}
