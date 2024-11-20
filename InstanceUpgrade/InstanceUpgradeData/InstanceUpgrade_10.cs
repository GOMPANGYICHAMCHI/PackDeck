using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstanceUpgrade_10", menuName = "Scriptable Object/InstanceUpgrade/InstanceUpgrade_10")]
public class InstanceUpgrade_10 : InstanceUpgradeBase
{
    [Header("핸드 크기 증가 제한(최대치)")]
    public int dumpSizeLimit;

    public override void Event_OnPurchase(PlayerData playerData, FlowBroadCaster flowBroadcaster)
    {
        base.Event_OnPurchase(playerData, flowBroadcaster);

        playerData.Add_DumpCost(1);
    }

    public override bool Event_CheckUpgradeAvailableCondition(PlayerData playerData)
    {
        if (playerData.Get_DumpCost() >= dumpSizeLimit)
            return false;

        return true;
    }
}
