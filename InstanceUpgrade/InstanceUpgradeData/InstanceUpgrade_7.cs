using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstanceUpgrade_7", menuName = "Scriptable Object/InstanceUpgrade/InstanceUpgrade_7")]
public class InstanceUpgrade_7 : InstanceUpgradeBase
{
    [Header("핸드 크기 증가 제한(최대치)")]
    public int handSizeLimit;

    public override void Event_OnPurchase(PlayerData playerData, FlowBroadCaster flowBroadcaster)
    {
        base.Event_OnPurchase(playerData, flowBroadcaster);

        playerData.Add_HandSize(1);
    }

    public override bool Event_CheckUpgradeAvailableCondition(PlayerData playerData) 
    {
        if (playerData.Get_HandSize() >= handSizeLimit)
            return false;

        return true; 
    }
}
