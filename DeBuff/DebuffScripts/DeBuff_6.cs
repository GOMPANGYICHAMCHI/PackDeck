using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_6", menuName = "Scriptable Object/DeBuff/DeBuff_6")]
public class DeBuff_6 : DeBuffBase
{
    public override void Event_StageStart_Pre(PlayerData playerdata) 
    {
        playerdata.Add_PlayableCards(-1);
    }

    public override void Event_StageEnd(PlayerData playerdata) 
    {
        playerdata.Add_PlayableCards(1);
    }
}
