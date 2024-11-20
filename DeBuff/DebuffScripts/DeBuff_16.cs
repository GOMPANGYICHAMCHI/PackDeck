using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_16", menuName = "Scriptable Object/DeBuff/DeBuff_16")]
public class DeBuff_16 : DeBuffBase
{
    [Header("고정 핸드 값")]
    public int FixedHandSize;

    int OriginalHandSize;

    public override void Event_StageStart_Pre(PlayerData playerdata) 
    {
        OriginalHandSize = playerdata.Get_HandSize();
        playerdata.Set_HandSize(FixedHandSize);
    }

    public override void Event_StageEnd(PlayerData playerdata) 
    {
        playerdata.Set_HandSize(OriginalHandSize);
    }
}
