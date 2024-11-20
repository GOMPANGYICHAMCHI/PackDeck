using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_23", menuName = "Scriptable Object/DeBuff/DeBuff_23")]
public class DeBuff_23 : DeBuffBase
{
    [Header("회복량(%)")]
    public int HealAmount;

    ulong lastRemainHealth;

    public override void Event_CardPlayStart(PlayerData playerdata)
    {
        lastRemainHealth = playerdata.Get_RemainHealth();
    }

    public override void Event_CardPlayEnd(PlayerData playerdata) 
    { 
        if(playerdata.Get_RemainHealth() != 0)
        {
            long GiveDamage = (long)(lastRemainHealth - playerdata.Get_RemainHealth());
            playerdata.Add_RemainHealth(GiveDamage / HealAmount);
        }
    }
}
