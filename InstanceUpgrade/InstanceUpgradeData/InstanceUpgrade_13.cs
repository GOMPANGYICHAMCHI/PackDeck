using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstanceUpgrade_13", menuName = "Scriptable Object/InstanceUpgrade/InstanceUpgrade_13")]
public class InstanceUpgrade_13 : InstanceUpgradeBase
{
    public override void Event_OnPurchase(PlayerData playerData, FlowBroadCaster flowBroadcaster)
    {
        base.Event_OnPurchase(playerData, flowBroadcaster);

        flowBroadcaster.InstanceUpgrade13_UpgradeCardNumber();
    }
}
