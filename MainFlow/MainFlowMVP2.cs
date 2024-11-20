using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using static FlowBroadCaster;

public class MainFlowMVP2 : MonoBehaviour
{
    //=============================================
    // 자손 스크립트
    //=============================================

    // 인게임 메시지 팝업 패널
    public IngameMsgPannel msgPannel;

    [SerializeField]
    private FlowBroadCaster flowBroadcaster;

    public UIManagerMVP2 uiManager;

    [SerializeField]
    private PlayerData playerData;

    public ScoreData scoreData;

    //=============================================

    public bool CheckDebuffCondition()
    {
        if (playerData.Get_CurrentStage() == 10 || playerData.Get_CurrentStage() % 3 == 0)
            return true;

        return false;
    }

    private void Start()
    {
        // 컴포넌트 초기화
        InitializeComponents();
        // 브로드캐스터 이벤트 설정
        SetBroadcasterEvent();

        // 게임시작
        StartGame();
        // 스테이지 시작
        StartStage();
    }

    private void InitializeComponents()
    {
        // 플레이데이터 초기화
        playerData.OnInitialize();
        // UI매니저 데이터 전달
        uiManager.GiveComponent(playerData, flowBroadcaster, msgPannel);
        // 이벤트 설정
        uiManager.SetEvent();

        scoreData = new ScoreData(playerData.PatternCount, playerData.ColorCount);

        playerData.Broadcaster.playerjocker_changed?.Invoke();
    }

    private void SetBroadcasterEvent()
    {
        // 스토어 종료
        flowBroadcaster.BroadcasterToMainflow_StoreExit += EndStore;

        // 스코어 체크
        flowBroadcaster.BroadcasterToMainflow_ScoreCheck += StartScoreCheck;

        // 카드 플레이 종료
        flowBroadcaster.BroadcasterToMainflow_CardPlayEnd += EndCardPlay;

        // 점수 적용
        flowBroadcaster.BroadcasterToMainflow_ApplyScore += CalculateScore;

        // 카드 버리기
        flowBroadcaster.BroadcasterToMainflow_CardDump += CardDump;

        // 게임 재시작 시
        flowBroadcaster.BroadcasterToMainflow_RestartGame += playerData.ResetValues;
        flowBroadcaster.BroadcasterToMainflow_RestartGame += StartGame;
        flowBroadcaster.BroadcasterToMainflow_RestartGame += StartStage;

        // 보상 확인 패널 에서 나오기
        //flowBroadcaster.BroadcasterToMainflow_StoreEnter += 
    }

    // 랜덤 디버프 선정
    private void PickRandomDeBuff()
    {
        // 랜덤 인덱스 픽
        int temp_index = UnityEngine.Random.Range(0, playerData.PercentSum_DeBuff);
        int temp_sum = 0;

        for (int i = 0; i < playerData.DeBuffPercent.Length; i++)
        {
            temp_sum += playerData.DeBuffPercent[i];

            if (temp_index <= temp_sum)
            {
                temp_index = i;
                break;
            }
        }

        // 현재 디버프 설정
        playerData.Set_CurrentDebuff(playerData.gameData.DeBuffData[temp_index]);
    }

    // 게임 오버 여부 판별
    private bool CheckGameOver()
    {
        // 게임오버 ( 행동 점수 소진 , 점수 달성 실패 시 )
        if (playerData.Get_CurrentActionCost() == 0 && playerData.Get_RemainHealth() != 0)
        {
            GameOver();
            return true;
        }

        // 승리 조건 달성시
        if (playerData.Get_RemainHealth() == 0)
        {
            // 스테이지 종료 이벤트 호출
            EndStage();
            // 골드 추가
            AddGoalAfterStage();

            // 디버프 종료 스테이지 일 경우,
            if (playerData.Get_CurrentStage() % 3 == 0)
            {
                //flowBroadcaster.MainflowToBroadcaster_DeactivateCurrentDebuff?.Invoke();
            }

            // 스테이지 보상 패널로
            flowBroadcaster.MainflowToBroadcaster_EnterRewardCheck?.Invoke();

            return true;
        }

        else
        {

        }

        return false;
    }

    // 점수 사전계산
    private void CalculateScore()
    {
        long FinalScore = 0;

        scoreData.ResetData();

        // 카드 수 및 카드 넘버 카운트
        for (int i = 0; i < playerData.Get_SelectDeckCount(); i++)
        {
            scoreData.patternCardCount[playerData.Get_SelectDeck(i).patternIndex]++;
            scoreData.colorCardCount[playerData.Get_SelectDeck(i).colorIndex]++;

            scoreData.patternNumberSum[playerData.Get_SelectDeck(i).patternIndex] += playerData.Get_SelectDeck(i).number;
            scoreData.colorNumberSum[playerData.Get_SelectDeck(i).colorIndex] += playerData.Get_SelectDeck(i).number;
        }

        scoreData.Additional.InputData(playerData.ScoreAdd);

        // 조커 효과 적용
        for (int i = 0; i < playerData.Get_PlayerJockerCount(); i++) 
        {
            if(playerData.Get_PlayerJocker(i).Get_isActive())
                scoreData = playerData.Get_PlayerJocker(i).Event_CheckScore(scoreData, playerData);
        }

        // 디버프 적용
        if(CheckDebuffCondition())
        {
            scoreData = playerData.Get_CurrentDebuff().Event_CheckScore(scoreData, playerData);
        }

        long temp_score = 0;

        // 최종 점수 계산
        // 패턴
        for (int i = 0; i < playerData.PatternCount; i++)
        {
            if(scoreData.patternCardCount[i] != 0)
            {
                // (패턴 카드 숫자 합 + 합산 가중치 A) * (패턴 카드 수 + 합산 가중치 B) * 곱 가중치
                temp_score = (long)((scoreData.patternNumberSum[i] + scoreData.Additional.Pattern_A[i])
                    * (scoreData.patternCardCount[i] + scoreData.Additional.Pattern_B[i])
                    * scoreData.Additional.Pattern_Multiply[i]);

                FinalScore += temp_score;
            }
        }

        // 컬러
        for (int i = 0; i < playerData.ColorCount; i++)
        {
            if (scoreData.colorCardCount[i] != 0)
            {
                // (패턴 카드 숫자 합 + 합산 가중치 A) * (패턴 카드 수 + 합산 가중치 B) * 곱 가중치
                temp_score = (long)((scoreData.colorNumberSum[i] + scoreData.Additional.Color_A[i])
                * (scoreData.colorCardCount[i] + scoreData.Additional.Color_B[i])
                * scoreData.Additional.Color_Multiply[i]);

                FinalScore += temp_score;
            }
        }

        // 현재 점수에 적용
        playerData.Add_RemainHealth(-FinalScore);
        // 스코어 데이터 저장
        playerData.Set_ScoreData(scoreData);

        // 점수 적용 이벤트
        flowBroadcaster.MainFlowToBroadcast_ScoreApplied?.Invoke(FinalScore);
    }

    // 스테이지 종료 이후 골드 추가
    private void AddGoalAfterStage()
    {
        GoldRewardData rewardData = new GoldRewardData();

        rewardData.roundClearReward = playerData.Get_AddGoldAmount();
        //Debug.Log(playerData.Get_AddGoldAmount());

        rewardData.roundClearReward += flowBroadcaster.additionalRoundClearReward?.Invoke() ?? 0;

        rewardData.overDealMultiply = playerData.Get_OverDealBonusMultiply();
        rewardData.overDealMultiply += flowBroadcaster.additionalOverDealMultiply?.Invoke() ?? 0;

        // 오버딜 보너스
        rewardData.overDealBonus = (int)(rewardData.overDealMultiply * playerData.Get_CurrentActionCost());

        // 이자 보너스
        rewardData.interestBonus = (playerData.Get_InterestDivideCardCount() != 0) ? playerData.Get_CurrentGold() / playerData.Get_InterestDivideCardCount() : 0;
        rewardData.interestBonus = Mathf.Clamp(rewardData.interestBonus, 0, playerData.Get_InterestLimit());

        // 조커 효과 적용
        for (int i = 0; i < playerData.Get_PlayerJockerCount(); i++)
        {
            if (playerData.Get_PlayerJocker(i).Get_isActive())
                rewardData = playerData.Get_PlayerJocker(i).Event_CheckAddGold(rewardData);
        }

        // 이벤트 호출
        flowBroadcaster.stageRewardApply?.Invoke(rewardData);
    }

    // 게임 시작
    private void StartGame()
    {
        // 유저 덱 리셋
        playerData.Reset_UserDeck();

        flowBroadcaster.MainflowToBroadcaster_GameStart?.Invoke();
    }

    // 스테이지 시작
    private void StartStage()
    {
        // 스테이지 추가
        playerData.Add_CurrentStage(1);

        // 디버프 적용
        if (CheckDebuffCondition())
        {
            playerData.Get_CurrentDebuff().Event_StageStart_Pre(playerData);
        }
        else if (playerData.Get_CurrentStage() % 3 == 1)
        {
            // 랜덤 디버프 선정
            PickRandomDeBuff();
        }

        // 코스트 초기화
        playerData.Set_CurrentActionCost(playerData.Get_ActionCost());
        playerData.Set_CurrentDumpCost(playerData.Get_DumpCost());

        // 목표 점수 증가
        if (playerData.Get_CurrentStage() != 1)
        {
            // 목표 점수 증가
            playerData.Add_BasicHealth(playerData.gameData.AddGoalScorePerStage);

            if (playerData.gameData.MultiplyGoalScorePerStage != 0)
            {
                // 목표점수 곱산
                playerData.Multiply_BasicHealth(playerData.gameData.MultiplyGoalScorePerStage);
            }
        }

        // 스코어 리셋
        playerData.Reset_RemainHealth();
        //playerData.Reset_SelectedScore();

        // 덱 리셋
        playerData.Reset_CurrentDeck();
        playerData.Reset_HandDeck();
        playerData.Reset_SelectDeck();

        // 디버프 적용
        if (CheckDebuffCondition())
        {
            playerData.Get_CurrentDebuff().Event_StageStart_Post(playerData);
        }

        flowBroadcaster.MainflowToBroadcaster_StageStart?.Invoke();
        StartCardPlay();
    }

    // 스테이지 종료
    private void EndStage()
    {
        // 디버프 적용
        if (CheckDebuffCondition())
        {
            playerData.Get_CurrentDebuff().Event_StageEnd(playerData);
        }

        flowBroadcaster.MainflowToBroadcaster_StageEnd?.Invoke();
    }

    // 카드 플레이 시작
    private void StartCardPlay()
    {
        // 선택 덱 초기화
        playerData.Reset_SelectDeck();

        flowBroadcaster.MainflowToBroadcaster_CardPlayStart?.Invoke();

        // 디버프 적용
        if (CheckDebuffCondition())
        {
            playerData.Get_CurrentDebuff().Event_CardPlayStart(playerData);
        }
    }

    // 현재 선택 카드 버리기
    private void CardDump()
    {
        
        flowBroadcaster.MainflowToBroadcaster_CardDump?.Invoke();
    }

    // 점수 확인 시작
    private void StartScoreCheck()
    {
        // 선택한 카드가 없을때 종료
        if (playerData.Get_SelectDeckCount() == 0)
        {
            msgPannel.PrintMessage("선택한 카드가 없습니다.");
            return;
        }

        // 행동 점수 감소
        playerData.Add_CurrentActionCost(-1);

        // 디버프 적용
        if (CheckDebuffCondition())
        {
            playerData.Get_CurrentDebuff().Event_StartScoreCheck(playerData);
        }

        // 스코어 체크 이벤트 호출
        flowBroadcaster.MainflowToBroadcaster_StartScoreCheck?.Invoke();
    }

    // 카드 플레이 종료
    private void EndCardPlay()
    {
        bool IsGameEnd;
        flowBroadcaster.MainflowToBroadcaster_CardPlayEndStart?.Invoke();

        // 조커 효과 적용
        for (int i = 0; i < playerData.Get_PlayerJockerCount(); i++)
        {
            if (playerData.Get_PlayerJocker(i).Get_isActive())
                playerData.Get_PlayerJocker(i).Event_CardPlayEndStart(playerData);
        }

        // 디버프 적용
        if (CheckDebuffCondition())
        {
            playerData.Get_CurrentDebuff().Event_CardPlayEnd(playerData);
        }

        // 게임 오버 여부 판별
        IsGameEnd = CheckGameOver();

        flowBroadcaster.MainflowToBroadcaster_CardPlayEndPost?.Invoke();

        // 게임오버 여부
        if (!IsGameEnd)
        {
            StartCardPlay();
        }
    }

    // 스토어에서 나올때 호출
    private void EndStore()
    {
        StartStage();
    }

    private void GameOver()
    {
        flowBroadcaster.MainflowToBroadcaster_GameOver?.Invoke();
    }
}
