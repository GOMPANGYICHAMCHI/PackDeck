using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerMVP2 : MonoBehaviour
{
    //=============================================
    // 참조 스크립트
    //=============================================

    private PlayerData playerData;
    private FlowBroadCaster flowBroadcaster;
    private IngameMsgPannel msgPannel;

    //============================================= 
    // 자손 스크립트
    //=============================================

    public UI_StorePannel store_UI;
    public UI_AllCardDeck allcard_UI;
    public UI_AllTimePannel alltime_UI;
    public UI_ScoreCombinationPannel scorecombination_UI;
    public UI_StageCardPannel stagecard_UI;
    public UI_Instage instage_UI;
    public UI_StageMonsterPannel stagemonster_UI;
    public UI_RewardCheckPannel rewardcheck_UI;

    public UI_DebugPannel debugtext_UI;

    //=============================================

    // 데이터 전달
    public void GiveComponent
        (PlayerData input_playerdata, 
        FlowBroadCaster input_flowmanager,
        IngameMsgPannel input_msgpannel)
    {
        playerData = input_playerdata;
        flowBroadcaster = input_flowmanager;
        msgPannel = input_msgpannel;

        ResetAllUIData();
        SetDebugEvent();
    }

    // 디버그 초기화
    private void SetDebugEvent()
    {
        flowBroadcaster.MainflowToBroadcaster_DebugOn += store_UI.OnDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOn += allcard_UI.OnDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOn += alltime_UI.OnDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOn += scorecombination_UI.OnDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOn += stagecard_UI.OnDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOn += instage_UI.OnDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOn += stagemonster_UI.OnDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOn += debugtext_UI.OnDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOn += rewardcheck_UI.OnDebug;

        flowBroadcaster.MainflowToBroadcaster_DebugOff += store_UI.OffDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOff += allcard_UI.OffDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOff += alltime_UI.OffDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOff += scorecombination_UI.OffDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOff += stagecard_UI.OffDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOff += instage_UI.OffDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOff += stagemonster_UI.OffDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOff += debugtext_UI.OffDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOff += rewardcheck_UI.OffDebug;
    }

    // 자손 스크립트 필요 컴포넌트 전달
    private void ResetAllUIData()
    {
        store_UI.SetBasicData(playerData, flowBroadcaster, msgPannel);
        allcard_UI.SetBasicData(playerData, flowBroadcaster, msgPannel);
        alltime_UI.SetBasicData(playerData, flowBroadcaster, msgPannel);
        scorecombination_UI.SetBasicData(playerData, flowBroadcaster, msgPannel);
        stagecard_UI.SetBasicData(playerData, flowBroadcaster, msgPannel);
        instage_UI.SetBasicData(playerData, flowBroadcaster, msgPannel);
        stagemonster_UI.SetBasicData(playerData, flowBroadcaster, msgPannel);
        rewardcheck_UI.SetBasicData(playerData, flowBroadcaster, msgPannel);

        rewardcheck_UI.Initialize();
        store_UI.Initialize();
        allcard_UI.Initialize();
        alltime_UI.Initialize();
        scorecombination_UI.Initialize();
        stagecard_UI.Initialize();
        instage_UI.Initialize();
        stagemonster_UI.Initialize();

        // 디버그
        debugtext_UI.SetBasicData(playerData, flowBroadcaster, msgPannel);
        debugtext_UI.Initialize();
    }

    public void SetEvent()
    {
        //=============================================
        // 플로우 브로드캐스터
        //=============================================

        // 게임 시작 시
        flowBroadcaster.MainflowToBroadcaster_GameStart += allcard_UI.GenerateAllCards;
        flowBroadcaster.MainflowToBroadcaster_GameStart += () => instage_UI.SetActiveGameoverPannel(false);
        flowBroadcaster.MainflowToBroadcaster_GameStart += instage_UI.UpdateScoreAdditionals;

        // 스테이지 시작 시
        flowBroadcaster.MainflowToBroadcaster_StageStart += () => allcard_UI.ActiveAllCardCover(false);
        flowBroadcaster.MainflowToBroadcaster_StageStart += () => instage_UI.SetActiveIngamePannel(true);
        flowBroadcaster.MainflowToBroadcaster_StageStart += () => instage_UI.SetActiveCardPlayPannel(true);
        flowBroadcaster.MainflowToBroadcaster_StageStart += stagecard_UI.DisableAllSelectCard;
        flowBroadcaster.MainflowToBroadcaster_StageStart += stagecard_UI.DisableAllHandCard;

        // 카드 플레이 시작 시
        //flowBroadcaster.MainflowToBroadcaster_CardPlayStart += playerData.Reset_SelectDeck;
        flowBroadcaster.MainflowToBroadcaster_CardPlayStart += stagecard_UI.StartCardPlay;
        flowBroadcaster.MainflowToBroadcaster_CardPlayStart += () => instage_UI.SetInterectableAllButton(true);
        flowBroadcaster.MainflowToBroadcaster_CardPlayStart += () => stagecard_UI.SetAllCardInteractable(true);

        // 스코어 체크
        flowBroadcaster.MainflowToBroadcaster_StartScoreCheck += () => instage_UI.SetInterectableAllButton(false);
        flowBroadcaster.MainflowToBroadcaster_StartScoreCheck += stagecard_UI.ScoreCheck;

        // 덱에서 핸드로 카드 이동시
        flowBroadcaster.DeckToHand += allcard_UI.DeActiveCardCover;

        // 카드 버리기
        flowBroadcaster.BroadcasterToUI_CardDump += stagecard_UI.SelectCardDump;

        // 보상 확인 창 진입 시
        flowBroadcaster.MainflowToBroadcaster_EnterRewardCheck += rewardcheck_UI.EnterRewardCheck;
        flowBroadcaster.MainflowToBroadcaster_EnterRewardCheck += () => instage_UI.SetActiveCardPlayPannel(false);
        flowBroadcaster.MainflowToBroadcaster_EnterRewardCheck += () => allcard_UI.OnOffPannel(false);

        // 보상 확인 창 텍스트 업데이트
        flowBroadcaster.stageRewardApply += rewardcheck_UI.UpdateScorePannelText;

        // 스토어 진입
        flowBroadcaster.BroadcasterToUI_StoreEnter += () => allcard_UI.ActiveAllCardCover(false);
        flowBroadcaster.BroadcasterToUI_StoreEnter += () => allcard_UI.OnOffPannel(false);
        flowBroadcaster.BroadcasterToUI_StoreEnter += () => instage_UI.SetActiveIngamePannel(false);
        flowBroadcaster.BroadcasterToUI_StoreEnter += store_UI.EnterStore;

        // 게임 오버시
        flowBroadcaster.MainflowToBroadcaster_GameOver += () => instage_UI.SetActiveGameoverPannel(true);

        // 점수 적용 확정
        flowBroadcaster.MainFlowToBroadcast_ScoreApplied += stagemonster_UI.DamagePopupText;

        // 게임 재시작 시
        flowBroadcaster.BroadcasterToMainflow_RestartGame += allcard_UI.ClearAllData;

        //=============================================
        // 플레이어 데이터 브로드캐스터
        //=============================================

        // 현재 덱에 카드 추가시
        playerData.Broadcaster.currentdeck_add += allcard_UI.AddCard;

        // 현재 골드 변경 시
        playerData.Broadcaster.currentgold_changed += alltime_UI.UpdateText_CurrentGold;

        // 현재 스테이지 숫자 변동 시
        playerData.Broadcaster.currentstage_changed += alltime_UI.UpdateText_CurrentStage;

        // 플레이어 조커 변동 시
        playerData.Broadcaster.playerjocker_changed += alltime_UI.Update_JockerPannel;
        playerData.Broadcaster.playerjocker_changed += alltime_UI.Update_CurrentJockerCount;

        playerData.Broadcaster.playerjocker_add += alltime_UI.Add_PlayerJocker;
        playerData.Broadcaster.playerjocker_remove += alltime_UI.Remove_PlayerJocker;

        // 현재 코스트 변동 시
        playerData.Broadcaster.currentcost_changed += instage_UI.UpdateActionCost;

        // 현재 스코어 변동시
        playerData.Broadcaster.remainhealth_changed += (empty) => stagemonster_UI.UpdateMonsterHealthBar();

        //playerData.Broadcaster.currentscore_changed += stagemonster_UI.DamagePopupText;

        // 목표 스코어 변동시
        playerData.Broadcaster.basichealth_changed += stagemonster_UI.UpdateMonsterHealthBar;

        // 버리기 가능 여부 변동시
        playerData.Broadcaster.currentdumpable_changed += instage_UI.SetInteracterableDumpButton;

        // 카드 강제 선택
        playerData.Broadcaster.cardselected_forcibly += stagecard_UI.SelectHandCardForcibly;

        // 카드 강제 버리기
        playerData.Broadcaster.handdeck_dumped += stagecard_UI.DumpCard;

        // 인스턴스 성장시 추가 점수 현황판 갱신
        playerData.Broadcaster.instanceUpgradeBtnClicked += instage_UI.UpdateScoreAdditionals;
    }
}
