using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DeBuff_7", menuName = "Scriptable Object/DeBuff/DeBuff_7")]
public class DeBuff_7 : DeBuffBase
{
    //JockerBase DisabledJocker;
    int curDisabledJockerIndex = -1;

    public override void Event_CardPlayStart(PlayerData playerdata)
    {
        base.Event_StageStart_Pre(playerdata);
        
        if(playerdata.Get_PlayerJockerCount() != 0)
        {
            curDisabledJockerIndex = UnityEngine.Random.Range(0, playerdata.Get_PlayerJockerCount());

            playerdata.Get_PlayerJocker(curDisabledJockerIndex).Set_isActive(false, playerdata);
        }
        else
        {
            curDisabledJockerIndex = -1;
        }
    }

    public override void Event_CardPlayEnd(PlayerData playerdata)
    {
        base.Event_StageEnd(playerdata);

        if (curDisabledJockerIndex != -1)
        {
            playerdata.Get_PlayerJocker(curDisabledJockerIndex).Set_isActive(true, playerdata);
            curDisabledJockerIndex = -1;
        }
    }
}
