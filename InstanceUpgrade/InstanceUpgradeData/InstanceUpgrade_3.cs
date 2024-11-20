using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstanceUpgrade_3", menuName = "Scriptable Object/InstanceUpgrade/InstanceUpgrade_3")]
public class InstanceUpgrade_3 : InstanceUpgradeBase
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

        playerData.ScoreAdd.Pattern_A[0] += UpgradeAmount_A;
        playerData.ScoreAdd.Pattern_B[0] += UpgradeAmount_B;
        playerData.ScoreAdd.Pattern_Multiply[0] += UpgradeAmount_C;
    }
}
