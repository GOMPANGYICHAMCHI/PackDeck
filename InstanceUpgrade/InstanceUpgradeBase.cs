using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceUpgradeBase : ScriptableObject
{
    [Header("인스턴스 업그레이드 명칭")]
    public string Name;
    [Header("인스턴스 업그레이드 설명")]
    public string Description;

    [Header("업그레이드 등장 슬롯 인덱스")]
    public int UpgradeSlotIndex;

    [Header("업그레이드 가격")]
    public int Cost;
    [Header("업그레이드 등장확률")]
    public int AppearancePercent;

    // 상점에 생성시
    public virtual void Event_OnInstantiatedInStore(PlayerData playerData) { }

    // 구매 가능 여부 판별
    public virtual bool Event_CheckUpgradePurchaseable(PlayerData playerData)
    {
        if (playerData.Get_CurrentGold() - Cost >= playerData.Get_PurchaseLimit()) 
        {
            return true;
        }

        return false;
    }

    // 업그레이드 등장 가능 여부 판별
    public virtual bool Event_CheckUpgradeAvailableCondition(PlayerData playerData) { return true; }

    // 구매시 이벤트
    public virtual void Event_OnPurchase(PlayerData playerData, FlowBroadCaster flowBroadcaster) 
    { 
        playerData.Add_CurrentGold(-Cost);
    }
}
