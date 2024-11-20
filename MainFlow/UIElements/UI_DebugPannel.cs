using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DebugPannel : UI_Basic
{
    public GameObject DebugPannel;

    [Header("디버그 모드 버튼")]
    public Button btn_Bebug;

    // 디버그 모드 여부
    private bool DebugMode = false;

    public TMP_Text txt_selectDeck;
    public TMP_Text txt_handDeck;

    //=============================================
    // 카드 점수 디버그
    //=============================================

    private int index = 1;

    [Header("카드 점수 디버그")]
    [SerializeField]
    private GameObject prefab_ScoreDebugPannel;
    [SerializeField]
    private GameObject prefab_Emptycard;
    [SerializeField]
    private Transform trans_ScoreDebugPannelHolder;

    //=============================================

    public override void Initialize()
    {
        // 디버그
        playerData.Broadcaster.selectdeck_add += Select;
        playerData.Broadcaster.selectdeck_remove += Select;
        playerData.Broadcaster.handdeck_add += Hand;
        playerData.Broadcaster.handdeck_remove += Hand;

        flowBroadcaster.MainFlowToBroadcast_ScoreApplied += GenerateScoreDebugPannel;

        btn_Bebug?.onClick.AddListener(SetDebug);
    }

    // 스코어 디버그 패널 생성
    public void GenerateScoreDebugPannel(long finalScore)
    {
        ScoreDebugPannel temp = Instantiate(prefab_ScoreDebugPannel, trans_ScoreDebugPannelHolder).GetComponent<ScoreDebugPannel>();
        temp.SetCardAsset(playerData.gameData.CardSetting.CardPattern, playerData.gameData.CardSetting.CardColor);
        temp.SetInfo(playerData.Get_ScoreData(),index, finalScore, prefab_Emptycard,playerData.Get_SelectDeckAll(),playerData.Get_CurrentStage());
        index++;
    }

    private void SetDebug()
    {
        if (DebugMode)
        {
            flowBroadcaster.MainflowToBroadcaster_DebugOff?.Invoke();
        }
        else
        {
            flowBroadcaster.MainflowToBroadcaster_DebugOn?.Invoke();
        }

        DebugMode = !DebugMode;
    }

    public void Update()
    {
        // 디버그 모드
        if (Input.GetKeyDown(KeyCode.F1))
        {
            //btn_Bebug.gameObject.SetActive(!btn_Bebug.gameObject.activeSelf);
            DebugPannel.SetActive(!DebugPannel.activeSelf);
        }
    }

    public override void OnDebug()
    {
        base.OnDebug();
        DebugPannel.SetActive(true);
    }

    public override void OffDebug()
    {
        base.OffDebug();
        DebugPannel.SetActive(false);
    }

    public void Select(Card data)
    {
        string temp = "SelectDeck / ";

        for (int i = 0; i < playerData.Get_SelectDeckCount(); i++)
        {
            temp += playerData.Get_SelectDeck(i).Index.ToString() + " / ";
        }

        txt_selectDeck.text = temp;
    }

    public void Hand(Card data)
    {
        string temp = "HandDeck / ";

        for (int i = 0; i < playerData.Get_HandDeckCount(); i++)
        {
            temp += playerData.Get_HandDeck(i).Index.ToString() + " / ";
        }

        txt_handDeck.text = temp;
    }
}
