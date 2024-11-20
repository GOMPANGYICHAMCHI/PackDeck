using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstanceUpgrade_4", menuName = "Scriptable Object/InstanceUpgrade/InstanceUpgrade_4")]
public class InstanceUpgrade_4 : InstanceUpgradeBase
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

        playerData.ScoreAdd.Color_A[2] += UpgradeAmount_A;
        playerData.ScoreAdd.Color_B[2] += UpgradeAmount_B;
        playerData.ScoreAdd.Color_Multiply[2] += UpgradeAmount_C;
    }
}
