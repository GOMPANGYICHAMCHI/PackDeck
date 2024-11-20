using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstanceUpgrade_9", menuName = "Scriptable Object/InstanceUpgrade/InstanceUpgrade_9")]
public class InstanceUpgrade_9 : InstanceUpgradeBase
{
    [Header("행동 크기 증가 제한(최대치)")]
    public int actionSizeLimit;

    public override void Event_OnPurchase(PlayerData playerData, FlowBroadCaster flowBroadcaster)
    { 
        base.Event_OnPurchase(playerData, flowBroadcaster);

        playerData.Add_ActionCost(1);
    }

    public override bool Event_CheckUpgradeAvailableCondition(PlayerData playerData)
    {
        if (playerData.Get_ActionCost() >= actionSizeLimit)
            return false;

        return true;
    }
}
