using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jocker_11", menuName = "Scriptable Object/Jocker/Jocker_11")]
public class Jocker_11 : JockerBase
{
    [Header("골드 추가 정도")]
    public int AddGoldAmount;

    int SellCost;

    public override void Event_GetItem(PlayerData playerData)
    {
        base.Event_GetItem(playerData);
        SellCost = Info.SellCost;
    }

    public override void Event_Sell(PlayerData playerData)
    {
        playerData.Add_AddGoldAmount(SellCost);
    }

    public override void Event_StageEnd(PlayerData playerData)
    {
        base.Event_StageEnd(playerData);
        SellCost += AddGoldAmount;
    }
}
