using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_5", menuName = "Scriptable Object/DeBuff/DeBuff_5")]
public class DeBuff_5 : DeBuffBase
{
    public override void Event_StageStart_Pre(PlayerData playerdata) 
    {
        playerdata.Set_CurrentDumpable(false);
    }

    public override void Event_StageEnd(PlayerData playerdata) 
    {
        playerdata.Set_CurrentDumpable(true);
    }
}
