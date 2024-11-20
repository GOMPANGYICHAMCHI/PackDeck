using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstanceUpgrade_11", menuName = "Scriptable Object/InstanceUpgrade/InstanceUpgrade_11")]
public class InstanceUpgrade_11 : InstanceUpgradeBase
{
    [Header("소모 재화 (순서대로)")]
    public int[] CostValues = new int[5];
    int curIndex = 0;

    public override void Event_OnInstantiatedInStore(PlayerData playerData) 
    {
        curIndex = 0;
        Cost = CostValues[curIndex];
    }

    public override bool Event_CheckUpgradePurchaseable(PlayerData playerData)
    {
        bool purchaseable = base.Event_CheckUpgradePurchaseable(playerData);

        if (purchaseable)
        {
            if (playerData.Get_UserDeckCount() == 1)
                return false;
        }

        return purchaseable;
    }

    public override void Event_OnPurchase(PlayerData playerData, FlowBroadCaster flowBroadcaster)
    {
        base.Event_OnPurchase(playerData, flowBroadcaster);

        if (curIndex < CostValues.Length -1)
        {
            curIndex++;
            Cost = CostValues[curIndex];
        }
            
        flowBroadcaster.InstanceUpgrade11_DeleteDeckCard();
    }
}
