using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeBuffBase : ScriptableObject
{
    // 디버프 데이터
    public DeBuffData data;

    public virtual void Event_StageStart_Pre(PlayerData playerdata) { }

    public virtual void Event_StageStart_Post(PlayerData playerdata) { }

    public virtual void Event_StageEnd(PlayerData playerdata) { }

    public virtual void Event_CardPlayStart(PlayerData playerdata) { }

    public virtual void Event_CardPlayEnd(PlayerData playerdata) { }

    public virtual void Event_StartScoreCheck(PlayerData playerdata) { }

    public virtual void Event_OnDisable(PlayerData playerdata) { }

    public virtual ScoreData Event_CheckScore(ScoreData scoreData, PlayerData playerData)
    {
        return scoreData;
    }
}
