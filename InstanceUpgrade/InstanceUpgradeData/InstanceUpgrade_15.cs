using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstanceUpgrade_15", menuName = "Scriptable Object/InstanceUpgrade/InstanceUpgrade_15")]
public class InstanceUpgrade_15 : InstanceUpgradeBase
{
    [Header("A 업그레이드 정도")]
    public int UpgradeAmount_A;
    [Header("B 업그레이드 정도")]
    public int UpgradeAmount_B;
    [Header("C 업그레이드 정도")]
    public int UpgradeAmount_C;

    public override void Event_OnPurchase(PlayerData playerData, FlowBroadCaster flowBroadcaster)
    {
        base.Event_OnPurchase(playerData, flowBroadcaster);

        for (int i = 0; i < playerData.ColorCount; i++)
        {
            playerData.ScoreAdd.Color_A[i] += UpgradeAmount_A;
            playerData.ScoreAdd.Color_B[i] += UpgradeAmount_B;
            playerData.ScoreAdd.Color_Multiply[i] += UpgradeAmount_C;
        }
    }
}
