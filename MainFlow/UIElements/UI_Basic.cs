using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Basic : MonoBehaviour
{
    protected PlayerData playerData;

    protected FlowBroadCaster flowBroadcaster;

    protected IngameMsgPannel msgPannel;

    virtual public void Initialize() { }

    virtual public void OnDebug() { }

    virtual public void OffDebug() { }

    public void SetBasicData(
        PlayerData input_playerdata,
        FlowBroadCaster input_flowmanager,
        IngameMsgPannel input_msgpannel
        )
    {
        playerData = input_playerdata;
        flowBroadcaster = input_flowmanager;
        msgPannel = input_msgpannel;    
    }
}
