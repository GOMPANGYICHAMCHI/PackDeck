using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MainFlow : MonoBehaviour
{
    //=============================================
    public UIManager uimanager;

    public int CurrentCardIndex = 0;

    // 원본 게임 데이터 홀더
    public GameDataHolder gameDataHolder;

    //게임 데이터
    public GameData gameData;

    // 점수 조합 데이터
    //public ScroeCombination[] scoreData;

    //=============================================

    //패턴점수 가중치_A
    public int[] CardPatternScore_A = new int[3];
    //색상 점수 가중치_A
    public int[] CardColorScore_A = new int[3];

    //패턴점수 가중치_B
    public int[] CardPatternScore_B = new int[3];
    //색상 점수 가중치_B
    public int[] CardColorScore_B = new int[3];

    //=============================================

    // 점수 데이터
    //public ScoreData scoreData;

    public int ColorCount;
    public int PatternCount;
    public int JockerCount;

    //=============================================
    // 스테이지 및 점수 
    //=============================================

    // 현재 스테이지
    public int CurrentStage = 0;
    // 목표 점수
    public double GoalScore = 10;
    // 현재 점수
    public double CurrentScore = 0;
    // 조합된 점수
    public double SelectedScore = 0;
    
    // 점수 데이터
    public ScoreData scoreData;

    //=============================================

    // 플레이어 골드
    public int CurrentGold = 0;
    // 스테이지당 골드 추가 정도
    //int AddGoldAmount = 5;
    // 이자 보상 지급 카드수 ( 해당 수치 당 1 골드 )
    int InterestDivideCardCount = 5;
    int InterestLimit = 5;

    // 플레이어 업그레이드 레벨
    public int[] PatternLevel = new int[3] { 1, 1, 1 };
    public int[] ColorLevel = new int[3] { 1, 1, 1 };

    public int ScoreMultiplyAmount = 1;

    //=============================================

    public int ActionCost;
    public int DumpCost;

    // 현재 행동 점수
    public int CurrentActionCost;
    // 현재 버리기 점수
    public int CurrentDumpCost;

    // 최대 선택 가능 카드 수
    public int PlayableCards;
    // 핸드 카드 수
    public int HandSize;

    //=============================================
    // 카드 데이터 
    //=============================================

    // 유저 원본 덱
    public List<Card> userDeck = new List<Card>();
    // 현재 덱 ( 스테이지 마다 초기화 됨 )
    public List<Card> currentDeck = new List<Card>();

    // 핸드 카드덱
    public List<Card> handDeck = new List<Card>();
    // 선택된 카드덱
    public List<Card> selectDeck = new List<Card>();

    //=============================================
    // 조커 
    //=============================================

    public Transform JockerPos;

    public JockerBase[] JockerData;

    //public int[] JockerPercent;

    public List<JockerBase> PlayerJocker;

    //=============================================
    // 디버프 
    //=============================================

    public Transform DeBuffPos;

    public DeBuffBase[] DeBuffData;

    //public int[] DeBuffPercent;

    public DeBuffBase CurrentDeBuff;

    // 디버프 확률 확산
    private int PersentSum_DeBuff;

    // 디버프 전체 확률
    private int[] DeBuffPercent;

    //=============================================
    // 스코어 스택
    //public List<ScoreStackData> scoreStackData = new List<ScoreStackData>();
    public int[] scorestackUIIndex = new int[6];
    public int[] scorestackA = new int[6];
    public float ScoreStackDelay = 0.3f;

    private void Start()
    {
        // 게임 데이터 초기화
        gameData = gameDataHolder.OriginalGameData.gameData;
        LoadJockerData();
        LoadDeBuffData();
        //uimanager.LoadCardPercentData(gameData.CardSetting.CardPatternPercent,gameData.CardSetting.CardColorPercent);

        // 컬러, 패턴, 조커 카운트 초기화
        ColorCount = gameData.CardSetting.CardColor.Count();
        PatternCount = gameData.CardSetting.CardPattern.Count();
        JockerCount = JockerData.Count();

        // 패턴 및 컬러 점수 가중치 초기화
        scoreData = new ScoreData(PatternCount, ColorCount);
        ResetScore();

        // 골드 텍스트 초기화
        uimanager.ResetGoldText();
        //uimanager.VoucherResetStage = gameData.VoucherResetStage;

        // UI 매니저에게 카드 패턴 스프라이트 및 컬러 할당
        uimanager.PatternSprites = gameData.CardSetting.CardPattern;
        uimanager.PatternColors = gameData.CardSetting.CardColor;

        // 점수 조합 정보 생성
        uimanager.GenerateCombinationScorePanel();

        StartGame();
    }

    // 조커 데이터 로드
    private void LoadJockerData()

    {
        JockerData = new JockerBase[JockerPos.childCount];
        int[] JockerPercent = new int[JockerPos.childCount];

        // 조커 데이터 위치의 자손 전체 순회
        for (int i = 0; i < JockerPos.childCount; i++)
        {
            // 컴포넌트 로드
            JockerData[i] = JockerPos.GetChild(i).GetComponent<JockerBase>();
            // 컴포넌트 데이터 리셋
            //JockerData[i].ResetJocker(gameData.JockerData[i]);

            // 확률 입력
            //JockerPercent[i] = gameData.JockerData[i].Percent;
        }

        // UI매니저에게 확률 관련 데이터 전달
        uimanager.LoadJockerPercentData(JockerPercent);
    }

    // 디버프 데이터 로드
    private void LoadDeBuffData()
    {
        DeBuffData = new DeBuffBase[DeBuffPos.childCount];
        DeBuffPercent = new int[DeBuffPos.childCount];

        // 디버프 데이터 위치의 자손 전체 순회
        for (int i = 0; i < DeBuffPos.childCount; i++) 
        {
            // 컴포넌트 로드
            DeBuffData[i] = DeBuffPos.GetChild(i).GetComponent<DeBuffBase>();
            // 컴포넌트 데이터 리셋
            //DeBuffData[i].ResetDeBuff(gameData.DeBuffData[i]);

            // 확률 입력
            //DeBuffPercent[i] = gameData.DeBuffData[i].Percent;
            // 확률 합산 ++
            PersentSum_DeBuff += DeBuffPercent[i];
        }
    }

    private void ResetScoreStack()
    {
        for (int i = 0; i < ColorCount + PatternCount; i++) 
        {
            //scoreStackData.Add(new ScoreStackData());
        }
    }

    private void ResetCost()
    {
        CurrentActionCost = ActionCost;
        CurrentDumpCost = DumpCost;
    }

    // 업그레이드 레벨 초기화
    private void ResetUpgradeLevel()
    {
        PatternLevel[0] = 1;
        PatternLevel[1] = 1;
        PatternLevel[2] = 1;

        ColorLevel[0] = 1;
        ColorLevel[1] = 1;
        ColorLevel[2] = 1;
    }

    private void ResetScore()
    {
        //CardPatternScore_A[0] = gameData.CardSetting.CardPatternAdditionalScore_A[0];
        //CardPatternScore_A[1] = gameData.CardSetting.CardPatternAdditionalScore_A[1];
        //CardPatternScore_A[2] = gameData.CardSetting.CardPatternAdditionalScore_A[2];
        //
        //CardColorScore_A[0] = gameData.CardSetting.CardColorAdditionalScore_A[0];
        //CardColorScore_A[1] = gameData.CardSetting.CardColorAdditionalScore_A[1];
        //CardColorScore_A[2] = gameData.CardSetting.CardColorAdditionalScore_A[2];
        //
        //CardPatternScore_B[0] = gameData.CardSetting.CardPatternAdditionalScore_B[0];
        //CardPatternScore_B[1] = gameData.CardSetting.CardPatternAdditionalScore_B[1];
        //CardPatternScore_B[2] = gameData.CardSetting.CardPatternAdditionalScore_B[2];
        //
        //CardColorScore_B[0] = gameData.CardSetting.CardColorAdditionalScore_B[0];
        //CardColorScore_B[1] = gameData.CardSetting.CardColorAdditionalScore_B[1];
        //CardColorScore_B[2] = gameData.CardSetting.CardColorAdditionalScore_B[2];

        CurrentGold = 0;
    }

    // 게임시작 시
    public void StartGame()
    {
        // 게임 데이터 초기화
        gameData = gameDataHolder.OriginalGameData.gameData;

        // 카드 인덱스 초기화
        CurrentCardIndex = 0;
        // 전체 카드 에셋 오브젝트 제거
        uimanager.DeleteAllDeckCard();
        // 카드 데이터 및 전체 에셋 생성
        CardDataGeneratre();

        // 패턴 및 컬러 점수 가중치 초기화
        ResetScore();
        // 업그레이드 레벨 초기화
        ResetUpgradeLevel();

        // 스테이지 당 골드 추가 정도 리셋
        //AddGoldAmount = gameData.AddGoldamountPerStage;

        // 코스트 초기화
        ActionCost = gameData.ActionCost;
        DumpCost = gameData.DumpCost;
        PlayableCards = gameData.SelectableCardAmount;
        HandSize = gameData.HandSize;

        // 코스트 초기화
        ResetCost();

        // 현재 스테이지 초기화
        CurrentStage = 0;
        // 목표 점수 초기화
        GoalScore = gameData.StartMonsterHealth;
        
        // 게임 오버 패널 비활성화
        uimanager.gameoverPanel.SetActive(false);

        // 스테이지 시작
        StartStage();
    }

    // 스테이지 시작시
    public void StartStage()
    {
        // 조합 정보창 갱신
        uimanager.SetCombinationScoresText();

        // 현재 스테이지++
        CurrentStage++;

        // 유저 코스트 리셋
        ResetCost();

        // 목표 점수 증가
        if(CurrentStage != 1)
        {
            // 목표 점수 증가
            GoalScore += gameData.AddGoalScorePerStage;
            if (gameData.MultiplyGoalScorePerStage != 0)
            {
                GoalScore *= gameData.MultiplyGoalScorePerStage;
                // 소수점 버리기
                GoalScore = Math.Truncate(GoalScore);
            }
        }

        // 현재 점수 초기화
        CurrentScore = 0;
        // 조합된 점수 초기화
        SelectedScore = 0;

        // 덱 정보 리셋
        currentDeck = userDeck.ToList();

        // 핸드덱 리셋
        handDeck.Clear();
        // 선택된 카드덱 리셋
        selectDeck.Clear();

        // 디버프 적용 ( 스테이지가 3의 배수 일때 )
        if (CurrentStage % 3 == 0)
        {
            //CurrentDeBuff.Event_StageStart(this);
        }
        else if(CurrentStage % 3 == 1)
        {
            PickRandomDeBuff();
        }
        uimanager.ResetDeBuffUI();

        // UI 스테이지 시작 함수 호출
        uimanager.StartStage();

        // 랜덤 카드 픽 및 오브젝트 생성
        PickRandomCard();
    }

    public void StartScoreStack()
    {
        // 선택된 카드가 없을때
        if(selectDeck.Count == 0)
        {
            uimanager.msgPannel.PrintMessage("선택된 카드가 없습니다!");
            return;
        }

        // 현재 행동 점수 감소
        CurrentActionCost--;

        // 스코어 및 코스트 텍스트 갱신
        uimanager.ResetScoreCostText();

        uimanager.ButtonCover.SetActive(true);

        // 선택 카드 오브젝트 생성 및 핸드카드 오브젝트 삭제
        uimanager.MakeSelectedCard();

        // 점수 확인
        SelectedScore = CheckCurrentScore();

        // 스코어 스택 시퀀스 재생
        StartCoroutine("PlayScoreSequence");
    }

    // 스코어 스택 프리뷰 / 핸드카드 선택 혹은 취소시
    public void ScoreStackPreview()
    {
        CheckCurrentScore();
        uimanager.ActivePreviewScoreStack();
    }

    // 조커 10 관련 함수
    public void FreeUpgrade_Jocker10()
    {
        // UI 비활성화
        uimanager.DisableJockerUI(9,false);

        //
        uimanager.EnableFreeUpgradePannel();
    }

    // 랜덤 디버프 선정
    public void PickRandomDeBuff()
    {
        // 랜덤 인덱스 픽
        int temp_index = UnityEngine.Random.Range(0, PersentSum_DeBuff);
        int temp_sum = 0;

        for (int i = 0; i < DeBuffPercent.Count(); i++) 
        {
            temp_sum += DeBuffPercent[i];

            if(temp_index < temp_sum)
            {
                temp_index = i;
                break;
            }
        }

        // 현재 디버프 설정
        CurrentDeBuff = DeBuffData[temp_index];

        // 디버프 관련 UI 갱신
        uimanager.ResetDeBuffUI();
    }

    // 다음 행동시
    public void NextAction()
    {
        // 스코어 스택 패널 Off
        uimanager.DisableAllScoreStack();
        uimanager.ButtonCover.SetActive(false);

        //// 현재 행동 점수 감소
        //CurrentActionCost--;

        //// 스코어 및 코스트 텍스트 갱신
        //uimanager.ResetScoreCostText();

        // 게임오버 혹은 승리 여부 판별
        if (CheckGameOver() == true)
            return;

        // 선택된 덱 초기화
        selectDeck.Clear();

        // 조합된 점수 초기화
        SelectedScore = 0;

        // 선택 카드 초기화
        uimanager.DisableAllSelectCard();

        // 카드 선택 및 생성
        PickRandomCard();
    }

    // 게임 오버 여부 확인
    bool CheckGameOver()
    {
        // 게임오버
        if(CurrentActionCost == 0 && CurrentScore < GoalScore)
        {
            uimanager.gameoverPanel.SetActive(true);
            return true;
        }

        // 승리 조건
        if(CurrentScore >= GoalScore)
        {
            uimanager.stageClearPanel.SetActive(true);

            if (CurrentStage % 3 == 0)
            {
                // 디버프 스테이지 종료 함수 호출
                //CurrentDeBuff.Event_StageEnd(this);
                // 새로운 디버프 랜덤 픽
                PickRandomDeBuff();
            }

            //if (CurrentStage % gameData.StorePerStage == 0)
            //{
            //    uimanager.SwitchStoreScorePanel(true);
            //    uimanager.StageClear_btn.transform.GetChild(0).GetComponent<TMP_Text>().text = "업그레이드 선택";
            //
            //    // 골드 추가 및 골드 텍스트 갱신
            //    //Gold += AddGoldAmount;
            //    AddGoldAfterRound();
            //    uimanager.ResetGoldText();
            //
            //    // 점수 2배 제거
            //    ScoreMultiplyAmount = 1;
            //}
            //else
            //{
            //    uimanager.StageClear_btn.transform.GetChild(0).GetComponent<TMP_Text>().text = "다음 스테이지로";
            //}

            // 최종 점수 2배 텍스트 비활성화
            uimanager.ScoreDoublePannel.SetActive(false);
            uimanager.ResetDeckCards();

            // 조커 전체 순회 및 스테이지 종료 함수 적용
            for (int i = 0; i < PlayerJocker.Count; i++) 
            {
                //PlayerJocker[i].Event_StageEnd(TODO);
            }

            // 조커 정보창 및 디버프 정보 창 비활성화
            uimanager.DisableJockerInfoPannel();
            uimanager.SetActiveDeBuffInfoPannel(false);

            return true;
        }

        return false;
    }

    // 라운드 후 골드 추가
    public int AddGoldAfterRound()
    {
        int AddAmount = 0;

        // 오버딜 계산 및 추가
        float OverDealPercent = (float)(CurrentScore / GoalScore);
        OverDealPercent = Mathf.Floor(OverDealPercent * 10f) / 10f;
        // 추가
        AddAmount += Mathf.Clamp((int)(OverDealPercent * CurrentActionCost), 0, 10);

        // 이자 보상
        int Interest = CurrentGold / InterestDivideCardCount;
        // 추가
        AddAmount += Mathf.Clamp(Interest, 0, InterestLimit);

        return AddAmount;
    }
     
    // 현재 선택 점수 확인
    public double CheckCurrentScore()
    {
        double FinalScore = 0;

        // 스코어 스택 UI 인덱스 리셋
        Array.Fill(scorestackUIIndex, -1);
        //int curstackIndex = 0;

        scoreData.ResetData();
        //scoreStackData.Clear();

        // 카드 수 및 카드 넘버 카운트
        for (int i = 0; i < selectDeck.Count(); i++) 
        {
            scoreData.patternCardCount[selectDeck[i].patternIndex]++;
            scoreData.colorCardCount[selectDeck[i].colorIndex]++;

            scoreData.patternNumberSum[selectDeck[i].patternIndex] += selectDeck[i].number;
            scoreData.colorNumberSum[selectDeck[i].colorIndex] += selectDeck[i].number;
        }

        // 조커 점수 계산 적용
        for (int i = 0; i < PlayerJocker.Count(); i++) 
        {
            //PlayerJocker[i].Event_CheckScore(this,scoreData);
        }

        // 디버프 적용
        if (CurrentStage % 3 == 0)
        {
            // 디버프 스테이지 종료 함수 호출
            //CurrentDeBuff.Event_ScoreCalculate(this,scoreData);
        }

        //double temp_score;
        //ScoreStackData stackData;

        // 최종 점수 계산
        // 패턴
        for (int i = 0; i < PatternCount; i++)
        {
            //temp_score = 
            //    (scoreData.patternNumberSum[i] * Mathf.Pow(CardPatternScore[i] + scoreData.additional_PatternB[i], scoreData.patternCount[i]));

            // (패턴 카드 숫자 합 * (A가중치 + 추가 A가중치)) + (패턴 카드 수 * (B가중치 + 추가 B가중치))
            //temp_score = 
            //    ((scoreData.patternNumberSum[i] * (CardPatternScore_A[i] + scoreData.additional_PatternA[i])) + (scoreData.patternCount[i] * (CardPatternScore_B[i] + scoreData.additional_PatternB[i])));

            //FinalScore += temp_score;

            // 스코어 스택 데이터
            //stackData = new ScoreStackData();
            //stackData.a = scoreData.patternNumberSum[i] * (CardPatternScore_A[i] + scoreData.additional_PatternA[i]);
            //stackData.b = scoreData.patternCount[i] * (CardPatternScore_B[i] + scoreData.additional_PatternB[i]);
            //
            ////stackData.c = scoreData.patternCount[i];
            //stackData.sum = temp_score;
            //
            //if(temp_score != 0 )
            //{
            //    stackData.isScoreValid = true;
            //
            //    scorestackUIIndex[i] = curstackIndex;
            //    curstackIndex++;
            //}
            //
            //scoreStackData.Add(stackData);
        }
        // 컬러
        for (int i = 0; i < ColorCount; i++)
        {
            //temp_score = 
            //    (scoreData.colorNumberSum[i] * Mathf.Pow(CardColorScore[i] + scoreData.additional_ColorB[i], scoreData.colorCount[i]));

            // (컬러 카드 숫자 합 * (A가중치 + 추가 A가중치)) + (컬러 카드 수 * (B가중치 + 추가 B가중치))
            //temp_score = 
            //    ((scoreData.colorNumberSum[i] * (CardColorScore_A[i] + scoreData.additional_ColorA[i])) + (scoreData.colorCount[i] * (CardColorScore_B[i] + scoreData.additional_ColorB[i])));

            //FinalScore += temp_score;

            // 스코어 스택 데이터
            //stackData = new ScoreStackData();
            //stackData.a = scoreData.colorNumberSum[i] * (CardColorScore_A[i] + scoreData.additional_ColorA[i]);
            //stackData.b = scoreData.colorCount[i] * (CardColorScore_B[i] + scoreData.additional_ColorB[i]);
            //
            ////stackData.c = scoreData.colorCount[i];
            //stackData.sum = temp_score;
            //
            //if (temp_score != 0)
            //{
            //    stackData.isScoreValid = true;
            //
            //    scorestackUIIndex[PatternCount + i] = curstackIndex;
            //    curstackIndex++;
            //}
            //
            //scoreStackData.Add(stackData);
        }

        //Debug.Log("최종 배율 : " + scoreData.FinalScoreMultiply);

        //if(scoreData.FinalScoreMultiply > 0)
        //    FinalScore *= scoreData.FinalScoreMultiply;

        FinalScore *= ScoreMultiplyAmount;
        // 소수점 버리기
        FinalScore = Math.Truncate(FinalScore);

        return FinalScore;
    }

    // 랜덤 카드 픽 및 생성
    public void PickRandomCard(bool isSingle = false)
    {
        int pickCount = 1;
        int pickIndex;
        Card curCard;

        if (isSingle == false)
        {
            // 뽑을 카드 갯수 선정
            pickCount = HandSize - (handDeck.Count + selectDeck.Count);
            
            // 뽑을 카드가 현재 덱 전체 갯수보다 클 경우,
            if (pickCount > currentDeck.Count)
            {
                pickCount = currentDeck.Count;
            }
        }

        // 카드 랜덤으로 픽 및 UI에셋 생성
        for(int i = 0; i < pickCount; i++)
        {
            pickIndex = UnityEngine.Random.Range(0, currentDeck.Count);
            curCard = currentDeck[pickIndex];

            // 핸드덱에 현재 생성 카드 추가
            handDeck.Add(curCard);
            // 현재덱에서 현재 생성 카드 제외
            currentDeck.Remove(curCard);

            // 카드 오브젝트 생성
            uimanager.MakeHandCard(curCard);
        }

        // 스코어 및 코스트 텍스트 갱신
        uimanager.ResetScoreCostText();
    }

    // 카드데이터 생성
    void CardDataGeneratre()
    {
        Card generatedCard;
        int index = 0;

        for (int p = 0; p < gameData.CardSetting.CardPattern.Count(); p++)
        {
            for (int c = 0; c < gameData.CardSetting.CardColor.Count(); c++)
            {
                for (int n = 0; n < gameData.CardSetting.CardNumber; n++)
                {
                    generatedCard = new Card();

                    generatedCard.colorIndex = c;
                    generatedCard.patternIndex = p;
                    generatedCard.number = n + 1;
                    generatedCard.Index = CurrentCardIndex;
                    CurrentCardIndex++;

                    userDeck.Add(generatedCard);
                    index++;
                }
            }
        }

        // 덱복사
        currentDeck = userDeck;

        // 전체덱 보기 패널 카드 오브젝트 생성
        uimanager.GenerateAllDeckCard();
    }

    // 추가 랜덤 카드 생성
    public Card MakeAdditionalRandomCard()
    {
        // 랜덤 인덱스 산출
        int rand_pattern = UnityEngine.Random.Range(0,gameData.CardSetting.CardPattern.Count());
        int rand_color = UnityEngine.Random.Range(0, gameData.CardSetting.CardColor.Count());
        int rand_number = UnityEngine.Random.Range(1,gameData.CardSetting.CardNumber + 1);

        // 카드 데이터 할당
        Card newCard = new Card();
        newCard.colorIndex = rand_color;
        newCard.patternIndex = rand_pattern;
        newCard.number = rand_number;
        newCard.Index = CurrentCardIndex;
        CurrentCardIndex++;

        // 전체 카드 덱에 새카드 추가
        userDeck.Add(newCard);

        // 전체 카드 덱 UI 에셋 생성
        uimanager.MakeAdditionalAllDeckCard(newCard);

        // 새로운 카드 데이터 반환
        return newCard;
    }

    IEnumerator PlayScoreStackSequence()
    {
        //string temp_content;
        Array.Fill(scorestackA, 0);

        yield return new WaitForSeconds(1);

        // 선택 덱 순회
        for (int i = 0; i < selectDeck.Count(); i++) 
        {
            //=========================================================================================================
            if(scorestackUIIndex[selectDeck[i].patternIndex] != -1)
            {
                // 선택된 카드 하이라이트
                uimanager.HighLightSelectedCard(i);
                yield return new WaitForSeconds(ScoreStackDelay);

                scorestackA[selectDeck[i].patternIndex] += selectDeck[i].number;
                uimanager.HighLightScoreStack(scorestackUIIndex[selectDeck[i].patternIndex]);
                //temp_content
                //    = scorestackA[selectDeck[i].patternIndex].ToString() + " * "
                //    + scoreStackData[selectDeck[i].patternIndex].b.ToString();// + " ^ "
                //    //+ scoreStackData[selectDeck[i].patternIndex].c.ToString();
                //uimanager.ActiveIndivisualScoreStackText(scorestackUIIndex[selectDeck[i].patternIndex], temp_content);

                yield return new WaitForSeconds(ScoreStackDelay);
            }
            //=========================================================================================================
            if(scorestackUIIndex[PatternCount + selectDeck[i].colorIndex] != -1)
            {
                // 선택된 카드 하이라이트
                uimanager.HighLightSelectedCard(i);
                yield return new WaitForSeconds(ScoreStackDelay);

                scorestackA[PatternCount + selectDeck[i].colorIndex] += selectDeck[i].number;
                uimanager.HighLightScoreStack(scorestackUIIndex[PatternCount + selectDeck[i].colorIndex]);
                //temp_content
                //    = scorestackA[PatternCount + selectDeck[i].colorIndex].ToString() + " * "
                //    + scoreStackData[PatternCount + selectDeck[i].colorIndex].b.ToString();// + " ^ "
                //    //+ scoreStackData[PatternCount + selectDeck[i].colorIndex].c.ToString();
                //uimanager.ActiveIndivisualScoreStackText(scorestackUIIndex[PatternCount + selectDeck[i].colorIndex], temp_content);

                yield return new WaitForSeconds(ScoreStackDelay * 2);
            }
            //=========================================================================================================
            
        }

        int index = 0;
        for (int i = 0; i < 6; i++) 
        {
            if (uimanager.scoreStackPannel[i].activeSelf)
            {
                //while (scoreStackData[index].sum == 0)
                //{
                //    index++;
                //}

                uimanager.HighLightScoreStack(i);
                //temp_content
                //= scoreStackData[index].a.ToString() + " * "
                //+ scoreStackData[index].b.ToString(); //+ " ^ "
                ////+ scoreStackData[index].c.ToString();
                //uimanager.ActiveIndivisualScoreStackText(i, temp_content);

                yield return new WaitForSeconds(ScoreStackDelay);

                uimanager.HighLightScoreStack(i);
                //uimanager.ActiveIndivisualScoreStack(i, Math.Truncate(scoreStackData[index].sum));
                index++;

                yield return new WaitForSeconds(ScoreStackDelay);
            }
        }

        /*for (int i = 0; i < 6; i++) 
        {
            // 점수가 0이 아닐때
            if (scoreStackData[i].isScoreValid)
            {
                if(i < PatternCount)
                {
                    temp_name = gameData.CardSetting.CardPatternName[i];
                }
                else
                {
                    temp_name = gameData.CardSetting.CardColorName[i - 3];
                }

                //uimanager.ActiveScoreStack(temp_name, scoreStackData[i]);

                yield return new WaitForSeconds(0.7f);
            }
        }*/

        // 현재 점수에 선택 점수 합산
        CurrentScore += SelectedScore;

        // 스코어 및 코스트 텍스트 갱신
        uimanager.ResetScoreCostText();

        yield return new WaitForSeconds(ScoreStackDelay);

        // 다음 스테이지로 넘어가기
        NextAction();
    }
}