using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_4", menuName = "Scriptable Object/DeBuff/DeBuff_4")]
public class DeBuff_4 : DeBuffBase
{
    public override void Event_StageStart_Pre(PlayerData playerdata) 
    {
        playerdata.Add_HandSize(-1);
    }

    public override void Event_StageEnd(PlayerData playerdata) 
    {
        playerdata.Add_HandSize(1);
    }
}
