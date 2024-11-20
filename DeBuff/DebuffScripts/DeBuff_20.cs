using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_20", menuName = "Scriptable Object/DeBuff/DeBuff_20")]
public class DeBuff_20 : DeBuffBase
{
    public override void Event_StartScoreCheck(PlayerData playerdata) 
    { 
        int rand_index = Random.Range(0, playerdata.Get_HandDeckCount());

        playerdata.Dump_HandDeck(playerdata.Get_HandDeck(rand_index));
    }
}
