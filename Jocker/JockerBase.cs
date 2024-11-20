//using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JockerBase", menuName = "name")]
public class JockerBase : ScriptableObject
{
    // 조커 정보
    public JockerInfo Info;
    // 활성화 여부
    protected bool isActive = false;

    public bool Get_isActive()
    {
        return isActive;
    }

    public void Set_isActive(bool value, PlayerData playerData)
    {
        if(isActive !=  value)
        {
            isActive = value;
            Event_isActiveChanged(playerData);
        }
    }

    // 활성화 여부 변경시 호출
    public virtual void Event_isActiveChanged(PlayerData playerData) { }

    // 습득 시
    public virtual void Event_GetItem(PlayerData playerData) 
    {
        playerData.Add_CurrentGold(-Info.PurchaseCost);
    }

    // 판매 시
    public virtual void Event_Sell(PlayerData playerData) 
    {
        playerData.Add_CurrentGold(Info.SellCost);
    }

    // 스테이지 종료 시
    public virtual void Event_StageEnd(PlayerData playerData) { }

    // 점수 확인 시
    public virtual ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData) 
    {
        return scoreData;
    }

    // 골드 보상 확인 시
    public virtual GoldRewardData Event_CheckAddGold(GoldRewardData goldReward) 
    {
        return goldReward;
    }

    // 카드 플레이 종료 초기 시
    public virtual void Event_CardPlayEndStart(PlayerData playerData) { }
}