using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_14", menuName = "Scriptable Object/DeBuff/DeBuff_14")]
public class DeBuff_14 : DeBuffBase
{
    [Header("유물 발동 확률")]
    public int JockerActivePercent;

    void RandomActivePlayerJocker(PlayerData playerdata)
    {
        int rand;

        for (int i = 0; i < playerdata.Get_PlayerJockerCount(); i++)
        {
            rand = Random.Range(0, 101);

            if(rand > JockerActivePercent)
            {
                playerdata.Get_PlayerJocker(i).Set_isActive(false, playerdata);
            }
        }
    }

    public override void Event_StageStart_Pre(PlayerData playerdata) 
    {
        RandomActivePlayerJocker(playerdata);
    }

    public override void Event_CardPlayStart(PlayerData playerdata) 
    {
        RandomActivePlayerJocker(playerdata);
    }

    public override void Event_StageEnd(PlayerData playerdata) 
    {
        for (int i = 0; i < playerdata.Get_PlayerJockerCount(); i++) 
        {
            playerdata.Get_PlayerJocker(i).Set_isActive(true, playerdata);
        }
    }
}
