using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DebugPannel : UI_Basic
{
    public GameObject DebugPannel;

    [Header("����� ��� ��ư")]
    public Button btn_Bebug;

    // ����� ��� ����
    private bool DebugMode = false;

    public TMP_Text txt_selectDeck;
    public TMP_Text txt_handDeck;

    //=============================================
    // ī�� ���� �����
    //=============================================

    private int index = 1;

    [Header("ī�� ���� �����")]
    [SerializeField]
    private GameObject prefab_ScoreDebugPannel;
    [SerializeField]
    private GameObject prefab_Emptycard;
    [SerializeField]
    private Transform trans_ScoreDebugPannelHolder;

    //=============================================

    public override void Initialize()
    {
        // �����
        playerData.Broadcaster.selectdeck_add += Select;
        playerData.Broadcaster.selectdeck_remove += Select;
        playerData.Broadcaster.handdeck_add += Hand;
        playerData.Broadcaster.handdeck_remove += Hand;

        flowBroadcaster.MainFlowToBroadcast_ScoreApplied += GenerateScoreDebugPannel;

        btn_Bebug?.onClick.AddListener(SetDebug);
    }

    // ���ھ� ����� �г� ����
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
        // ����� ���
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
