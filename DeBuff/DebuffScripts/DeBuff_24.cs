using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeBuff_24", menuName = "Scriptable Object/DeBuff/DeBuff_24")]
public class DeBuff_24 : DeBuffBase
{
    [HideInInspector]
    public List<DeBuffBase> additionalDebuff = new List<DeBuffBase>();

    void AddAdditionalDebuff(PlayerData playerdata)
    {
        int rand = Random.Range(0, 3);

        if(rand == 0)
        {
            rand = Random.Range(0, playerdata.DeBuffCount);
            additionalDebuff.Add(playerdata.gameData.DeBuffData[rand]);
        }
    }

    public override void Event_StageStart_Pre(PlayerData playerdata) 
    { 
        for(int i = 0; i <  additionalDebuff.Count; i++)
        {
            additionalDebuff[i].Event_StageStart_Pre(playerdata);
        }
    }

    public override void Event_StageEnd(PlayerData playerdata) 
    {
        for (int i = 0; i < additionalDebuff.Count; i++)
        {
            additionalDebuff[i].Event_StageEnd(playerdata);
        }
    }

    public override void Event_CardPlayStart(PlayerData playerdata) 
    {
        AddAdditionalDebuff(playerdata);

        for (int i = 0; i < additionalDebuff.Count; i++)
        {
            additionalDebuff[i].Event_CardPlayStart(playerdata);
        }
    }

    public override void Event_CardPlayEnd(PlayerData playerdata) 
    {
        for (int i = 0; i < additionalDebuff.Count; i++)
        {
            additionalDebuff[i].Event_CardPlayEnd(playerdata);
        }
    }

    public override void Event_StartScoreCheck(PlayerData playerdata) 
    {
        for (int i = 0; i < additionalDebuff.Count; i++)
        {
            additionalDebuff[i].Event_StartScoreCheck(playerdata);
        }
    }

    public override ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        for (int i = 0; i < additionalDebuff.Count; i++)
        {
            scoreData = additionalDebuff[i].Event_CheckScore(scoreData, playerData);
        }

        return scoreData;
    }
}
