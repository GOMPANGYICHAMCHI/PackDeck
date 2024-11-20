using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RewardCheckPannel : UI_Basic
{
    //=============================================
    // 자체 변수
    //=============================================

    // 현재 지급 보상
    int SumReward;

    //=============================================
    [Header("게임 오브젝트")]
    //=============================================

    // 스코어 체크 전체 패널
    public GameObject RewardCheckPannel;

    // 라운드 클리어 보상
    public TMP_Text txt_roundClearReward;
    // 오버딜 배수
    public TMP_Text txt_overDealMultiply;
    // 남은 행동
    public TMP_Text txt_remainAction;
    // 오버딜 보너스
    public TMP_Text txt_overDealBonus;
    // 이자보상
    public TMP_Text txt_interestReward;

    // 골드 획득 버튼
    public Button btn_getReward;
    public TMP_Text txt_getRewardButton;

    //=============================================

    // 초기화
    public override void Initialize()
    {
        base.Initialize();
        btn_getReward.onClick.AddListener(OnClickGetRewardButton);
    }

    // 보상 확인 패널 진입 시
    public void EnterRewardCheck()
    {
        OnOffRewardCheckPannel(true);
    }

    // 보상 확인 패널 ON/OFF
    public void OnOffRewardCheckPannel(bool setActive)
    {
        RewardCheckPannel.SetActive(setActive);
    }

    // 보상 확인 패널 텍스트 업데이트
    public void UpdateScorePannelText(GoldRewardData goldReward)
    {
        txt_roundClearReward.text = goldReward.roundClearReward.ToString();
        txt_overDealMultiply.text = goldReward.overDealMultiply.ToString();
        txt_remainAction.text = playerData.Get_CurrentActionCost().ToString();
        txt_overDealBonus.text = goldReward.overDealBonus.ToString();
        txt_interestReward.text = goldReward.interestBonus.ToString();

        int sumReward = goldReward.roundClearReward + goldReward.overDealBonus + goldReward.interestBonus;

        txt_getRewardButton.text = sumReward.ToString() + " 골드 획득";
        SumReward = sumReward;
    }

    // 보상 획득 버튼 클릭시
    public void OnClickGetRewardButton()
    {
        // 패널 비활성화
        OnOffRewardCheckPannel(false);
        // 현재 골드에 추가
        playerData.Add_CurrentGold(SumReward);
        // 스토어 진입
        flowBroadcaster.BroadcasterToUI_StoreEnter?.Invoke();
    }
}
