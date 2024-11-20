using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_18", menuName = "Scriptable Object/DeBuff/DeBuff_18")]
public class DeBuff_18 : DeBuffBase
{
    public override void Event_CardPlayStart(PlayerData playerdata) 
    { 
        int rand_index = Random.Range(0,playerdata.Get_HandDeckCount());

        playerdata.HandToSelectForcibly(playerdata.Get_HandDeck(rand_index));
    }
}
