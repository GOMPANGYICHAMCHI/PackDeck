using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Instage : UI_Basic
{
    //=============================================
    [Header("프리팹")]
    //=============================================

    public GameObject prefab_ActionCost;

    //=============================================
    [Header("게임 오브젝트")]
    //=============================================

    public Transform ActionCostHolder;

    public GameObject obj_InStagePannel;

    public GameObject obj_CardStagePannel;

    public GameObject obj_GameOverPannel;

    public Button btn_Attack;

    public Button btn_Dump;

    public Button btn_RestartGame;

    //

    [SerializeField]
    private ScoreAdditionalPannel[] scoreAdditionalPannel;

    //=============================================

    private void Start()
    {
        btn_Attack.onClick.AddListener(AttcakButtonOnclick);
        btn_Dump.onClick.AddListener(DumpButtonOnClick);
        btn_RestartGame.onClick.AddListener(OnclickRestartButton);
    }

    public void OnclickRestartButton()
    {
        SetActiveGameoverPannel(false);
        flowBroadcaster.BroadcasterToMainflow_RestartGame();
    }

    public void UpdateScoreAdditionals()
    {
        for (int i = 0; i < 3; i++)
        {
            scoreAdditionalPannel[i].SetText
                (
                playerData.ScoreAdd.Color_A[i],
                playerData.ScoreAdd.Color_B[i],
                playerData.ScoreAdd.Color_Multiply[i]
                );
        }

        for (int i = 0; i < 3; i++)
        {
            scoreAdditionalPannel[3 + i].SetText
                (
                playerData.ScoreAdd.Pattern_A[i],
                playerData.ScoreAdd.Pattern_B[i],
                playerData.ScoreAdd.Pattern_Multiply[i]
                );
        }
    }

    // 행동 점수 표시 업데이트
    public void UpdateActionCost()
    {
        int RemainCost = playerData.Get_CurrentActionCost() - ActionCostHolder.childCount;

        // 더 생성 해야 할때
        if(RemainCost > 0 )
        {
            for (int i = 0; i < RemainCost; i++) 
            {
                Instantiate(prefab_ActionCost, ActionCostHolder);
            }
        }
        // 기존 생성을 줄여야 할때
        else
        {
            RemainCost *= -1;
            for (int i = 0; i < RemainCost; i++)
            {
                Destroy(ActionCostHolder.GetChild(0).gameObject);
            }
        }
    }

    public void SetActiveGameoverPannel(bool isActive)
    {
        obj_GameOverPannel.SetActive(isActive);
    }

    // 카드플레이 스테이지 전체 패널
    public void SetActiveCardPlayPannel(bool isActive)
    {
        obj_CardStagePannel.SetActive(isActive);
    }

    public void SetInterectableAllButton(bool isInterectable)
    {
        SetInteracterableAttackButton(isInterectable);
        SetInteracterableDumpButton(isInterectable);
    }

    // 인스테이지 패널 ON/OFF
    public void SetActiveIngamePannel(bool isActive)
    {
        obj_InStagePannel.SetActive(isActive);
    }

    // 공격 버튼 상호작용 가능 여부 설정
    public void SetInteracterableAttackButton(bool isActive)
    {
        btn_Attack.interactable = isActive;
    }

    // 버리기 버튼 상호작용 가능 여부 설정
    public void SetInteracterableDumpButton(bool isActive)
    {
        if(playerData.Get_CurrentDumpable())
        {
            btn_Dump.interactable = isActive;
        }
        else
        {
            btn_Dump.interactable = false;
        }
    }

    // 공격 버튼 클릭 시
    private void AttcakButtonOnclick()
    {
        flowBroadcaster.BroadcasterToMainflow_ScoreCheck();
    }

    // 버리기 버튼 클릭 시
    private void DumpButtonOnClick()
    {
        flowBroadcaster.BroadcasterToUI_CardDump();
    }
}
