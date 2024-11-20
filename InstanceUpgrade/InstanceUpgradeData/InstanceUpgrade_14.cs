using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstanceUpgrade_14", menuName = "Scriptable Object/InstanceUpgrade/InstanceUpgrade_14")]
public class InstanceUpgrade_14 : InstanceUpgradeBase
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

        for (int i = 0; i < playerData.PatternCount; i++) 
        {
            playerData.ScoreAdd.Pattern_A[i] += UpgradeAmount_A;
            playerData.ScoreAdd.Pattern_B[i] += UpgradeAmount_B;
            playerData.ScoreAdd.Pattern_Multiply[i] += UpgradeAmount_C;
        }
    }
}
