using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowBroadCaster : MonoBehaviour
{
    public delegate void VoidDelegate();
    public delegate void InputUlong(long  value);
    public delegate void CardDelegate(Card data);
    public delegate void StageReward(GoldRewardData goldReward);
    public delegate int Additional_RoundClearReward();
    public delegate float Additional_OverDealMultiply();

    //================================================================

    // 디버그 옵션
    public VoidDelegate MainflowToBroadcaster_DebugOn;
    public VoidDelegate MainflowToBroadcaster_DebugOff;

    // 카드 플레이 시작
    public VoidDelegate MainflowToBroadcaster_CardPlayStart;

    // 카드 플레이 종료
    public VoidDelegate BroadcasterToMainflow_CardPlayEnd;
    public VoidDelegate MainflowToBroadcaster_CardPlayEndStart;
    public VoidDelegate MainflowToBroadcaster_CardPlayEndPost;

    // 카드 버리기
    public VoidDelegate BroadcasterToUI_CardDump;
    public VoidDelegate BroadcasterToMainflow_CardDump;
    public VoidDelegate MainflowToBroadcaster_CardDump;

    // 스테이지 시작, 종료
    public VoidDelegate MainflowToBroadcaster_StageStart;
    public VoidDelegate MainflowToBroadcaster_StageEnd;

    // 게임 시작, 종료
    public VoidDelegate MainflowToBroadcaster_GameStart;
    public VoidDelegate MainflowToBroadcaster_GameOver;

    // 게임 재시작
    public VoidDelegate BroadcasterToMainflow_RestartGame;

    // 스코어 확인
    public VoidDelegate BroadcasterToMainflow_ScoreCheck;
    public VoidDelegate MainflowToBroadcaster_StartScoreCheck;
    public VoidDelegate BroadcasterToMainflow_ApplyScore;
    public InputUlong MainFlowToBroadcast_ScoreApplied;

    // 상점 진입 및 퇴출
    public VoidDelegate BroadcasterToUI_StoreEnter;
    public VoidDelegate BroadcasterToMainflow_StoreExit;
    
    // 조커 구입 판매
    public VoidDelegate BroadcasterToMainflow_JockerBuy;
    public VoidDelegate BroadcasterToMainflow_JockerSell;

    public VoidDelegate MainflowToBroadcaster_DeactivateCurrentDebuff;

    // 점수 확인 및 적용
    public VoidDelegate BroadcastToMainFlow_CheckScore;  
    public VoidDelegate MainflowToBroadcaster_ApplyScore;

    // 스테이지 보상 적용
    public StageReward stageRewardApply;
    public Additional_RoundClearReward additionalRoundClearReward;
    public Additional_OverDealMultiply additionalOverDealMultiply;
    // 스테이지 보상 패널로
    public VoidDelegate MainflowToBroadcaster_EnterRewardCheck;

    public CardDelegate SelectCard;
    public CardDelegate UnSelectCard;
    public CardDelegate DeckToHand;

    //================================================================
    // 인스턴스 업그레이드 이벤트
    //================================================================

    public VoidDelegate InstanceUpgrade11_DeleteDeckCard;
    public VoidDelegate InstanceUpgrade12_AddDeckCard;
    public VoidDelegate InstanceUpgrade13_UpgradeCardNumber;
}
