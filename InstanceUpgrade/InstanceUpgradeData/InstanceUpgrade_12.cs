using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstanceUpgrade_12", menuName = "Scriptable Object/InstanceUpgrade/InstanceUpgrade_12")]
public class InstanceUpgrade_12 : InstanceUpgradeBase
{
    public override void Event_OnPurchase(PlayerData playerData, FlowBroadCaster flowBroadcaster)
    {
        base.Event_OnPurchase(playerData, flowBroadcaster);

        flowBroadcaster.InstanceUpgrade12_AddDeckCard();
    }
}
