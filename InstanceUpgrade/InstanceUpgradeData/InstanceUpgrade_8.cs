using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstanceUpgrade_8", menuName = "Scriptable Object/InstanceUpgrade/InstanceUpgrade_8")]
public class InstanceUpgrade_8 : InstanceUpgradeBase
{
    [Header("플레이 크기 증가 제한(최대치)")]
    public int playableSizeLimit;

    public override void Event_OnPurchase(PlayerData playerData, FlowBroadCaster flowBroadcaster)
    { 
        base.Event_OnPurchase(playerData, flowBroadcaster);

        playerData.Add_PlayableCards(1);
    }

    public override bool Event_CheckUpgradeAvailableCondition(PlayerData playerData)
    {
        if (playerData.Get_PlayableCards() >= playableSizeLimit)
            return false;

        return true;
    }
}
