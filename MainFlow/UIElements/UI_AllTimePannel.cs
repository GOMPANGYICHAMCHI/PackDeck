using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 상시 떠있는 UI를 관리하는 스크립트
// 현재 스테이지, 조커, 현재 골드

public class UI_AllTimePannel : UI_Basic
{
    //=============================================
    // 프리팹
    //=============================================

    [SerializeField]
    private GameObject prefab_Jocker;

    //=============================================
    // 오브젝트
    //=============================================

    [SerializeField]
    private Transform JockerHolder;

    [SerializeField]
    private JockerUIScript[] JockerPannel = new JockerUIScript[5];

    //[SerializeField]
    //private Button btn_AllDeckBtn;

    [Header("조커 설명창")]
    [SerializeField]
    private GameObject obj_JockerExplainPannel;

    [SerializeField]
    private TMP_Text txt_JockerExplain;

    [SerializeField]
    private TMP_Text txt_JockerName;

    [SerializeField]
    private TMP_Text txt_JockerCost;

    [SerializeField]
    private Image[] img_JockerInfoBackground;

    //=============================================
    // UI 엘레멘트
    //=============================================

    [SerializeField]
    private TMP_Text txt_currentgold;

    [SerializeField]
    private TMP_Text txt_currentstage;

    [SerializeField]
    private TMP_Text txt_currentjockercount;

    //=============================================
    // 정보
    //=============================================

    Vector3 vec_JockerInfoPannelPos;

    [SerializeField]
    private Vector2 vec2_JockerInfoPannelPosOffset = new Vector2(150, 200);

    JockerBase curFocusJocker;

    //=============================================

    public override void Initialize()
    {
        UpdateText_CurrentGold();
        UpdateText_CurrentStage();
        SetEvent();
    }

    private void SetEvent()
    {
        for (int i = 0; i < JockerPannel.Length; i++)
        {
            JockerPannel[i].func_PoinetEnter += JcokerOnPointerEnter;
            JockerPannel[i].func_PoinetExit += JcokerOnPointerExit;
        }
    }

    public void JcokerOnPointerEnter(JockerBase input_jocker)
    {
        obj_JockerExplainPannel.SetActive(true);
        txt_JockerName.text = input_jocker.Info.Name;
        txt_JockerExplain.text = input_jocker.Info.ExplainText;
        txt_JockerCost.text = "판매금액 : " + input_jocker.Info.SellCost.ToString() + " 골드";
        curFocusJocker = input_jocker;

        for (int i = 0; i < img_JockerInfoBackground.Length; i++) 
        {
            img_JockerInfoBackground[i].color = input_jocker.Info.JockerColor;
        }
    }

    public void JcokerOnPointerExit(JockerBase input_jocker)
    {
        obj_JockerExplainPannel.SetActive(false);
        curFocusJocker = null;
    }

    private void Update()
    {
        // 조커위에 마우스 호버 상태 일때
        if (obj_JockerExplainPannel.activeSelf)
        {
            vec_JockerInfoPannelPos = new Vector3(Input.mousePosition.x + vec2_JockerInfoPannelPosOffset.x, Input.mousePosition.y - vec2_JockerInfoPannelPosOffset.y, 0);

            // 조커 설명 오브젝트 위치 설정
            obj_JockerExplainPannel.transform.position = vec_JockerInfoPannelPos;

            // Q 버튼 클릭시 ( 조커 판매 )
            if(Input.GetKeyDown(KeyCode.Q) && curFocusJocker != null)
            {
                curFocusJocker.Event_Sell(playerData);
                playerData.Remove_PlayerJocker(curFocusJocker);

                // 조커 설명 오브젝트 비활성화
                obj_JockerExplainPannel.SetActive(false);
                curFocusJocker = null;
            }
        }
    }

    // 현재 골드 텍스트 업데이트
    public void UpdateText_CurrentGold()
    {
        txt_currentgold.text = playerData.Get_CurrentGold().ToString();
    }

    // 현재 스테이지 텍스트 업데이트
    public void UpdateText_CurrentStage()
    {
        txt_currentstage.text = playerData.Get_CurrentStage().ToString();
    }

    // 현재 조커 카운드 텍스트 업데이트
    public void Update_CurrentJockerCount()
    {
        txt_currentjockercount.text = playerData.Get_PlayerJockerCount().ToString();
    }

    // 플레이어 조커 패널 업데이트
    public void Update_JockerPannel()
    {
        for (int i = 0; i < 5; i++)
        {
            JockerPannel[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < playerData.Get_PlayerJockerCount(); i++)
        {
            JockerPannel[i].gameObject.SetActive(true);
            JockerPannel[i].gameObject.GetComponent<Image>().color = playerData.Get_PlayerJocker(i).Info.JockerColor;
            JockerPannel[i].transform.GetChild(0).GetComponent<TMP_Text>().text
                = playerData.Get_PlayerJocker(i).Info.Name;
            JockerPannel[i].GetComponent<JockerUIScript>().Index = i;
            JockerPannel[i].jockerInfo = playerData.Get_PlayerJocker(i);
        }
    }

    // 플레이어 조커 제거
    public void Remove_PlayerJocker(JockerBase input_Jocker)
    {

    }
    
    // 플레이어 조커 추가
    public void Add_PlayerJocker(JockerBase input_Jocker)
    {
        
    }
}
