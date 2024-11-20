//using Palmmedia.ReportGenerator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class UIManager : MonoBehaviour
{
    // 메인 플로우
    public MainFlow mainflow;

    // 카드색상
    public Color32[] PatternColors;
    // 카드 패턴 스프라이트
    public Sprite[] PatternSprites;

    // 메시지 전달 패널
    public IngameMsgPannel msgPannel;

    [Header("프리팹\n")]
    [Header("핸드카드 프리팹")]
    public GameObject HandCardPrefab;
    [Header("선택카드 프리팹")]
    public GameObject SelectedCardPrefab;
    [Header("전체카드 프리팹")]
    public GameObject AllCardPrefab;

    [Header("점수조합 정보 패널 프리팹")]
    public GameObject ScoreConbinationInfoPanelPrefab;

    [Header("최종 점수 2배 텍스트 패널")]
    public GameObject ScoreDoublePannel;

    [Header("점수패널텍스트\n")]
    [Header("현재 스테이지 텍스트")]
    public TMP_Text CurrentStage_txt;
    [Header("목표 점수 텍스트")]
    public TMP_Text GoalScore_txt;
    [Header("현재 점수 텍스트")]
    public TMP_Text CurrentScore_txt;
    [Header("행동 점수 텍스트")]
    public TMP_Text ActionCost_txt;
    [Header("버리기 점수 텍스트")]
    public TMP_Text DumpCost_txt;
    [Header("현재 콜드 텍스트")]
    public TMP_Text Gold_txt;

    [Header("최종 선택된 점수 텍스트")]
    public TMP_Text SelectScoreFinal_txt;
    [Header("선택된 점수 합산 텍스트")]
    public TMP_Text SelectScoreAdd_txt;
    [Header("선택된 점수 배율 텍스트")]
    public TMP_Text SelectScoreMultiply_txt;

    [Header("핸드 사이즈 텍스트")]
    public TMP_Text HandSize_txt;
    [Header("플레이 사이즈 텍스트")]
    public TMP_Text PlaySize_txt;

    [Header("조합점수")]
    public TMP_Text[] CombinationScoreTxt;

    //=============================================
    // 상점 
    //=============================================

    // 업그레이드 버튼 프리팹
    public GameObject UpgradeBtnPrefab;
    // 업그레이드 버튼 색상 배열
    public Color32[] UpgradeBtnColor = new Color32[3];

    // 업그레이드 버튼 홀더들
    public Transform UpgradeHolder1;
    public Transform UpgradeHolder2;
    public Transform UpgradeHolder3;

    // 현재 버튼에 할당된 업그레이드 인덱스
    public int[] UpgradeIndex1 = new int[3];
    // 현재 버튼에 할당된 업그레이드 인덱스
    public int[] UpgradeIndex2 = new int[2];
    // 현재 버튼에 할당된 업그레이드 인덱스
    public int[] UpgradeIndex3 = new int[2];

    // 리롤 가격 텍스트
    public TMP_Text txt_ReRollCost;
    static public int ReRollCost = 5;
    public int CurrentReRollCost = ReRollCost;

    // 바우처 구역 초기화 까지 남은 스테이지 텍스트
    public TMP_Text txt_LeftStageToResetVoucher;

    //=============================================
    // 조커 및 디버프 확률 관련
    //=============================================

    // 조커 전체 확률
    private int[] JockerPercent;

    // 카드 패턴 확률
    private int[] CardPatternPercent;
    // 카드 컬러 확률
    private int[] CardColorPercent;

    // 바우처 리셋 스테이지
    public int VoucherResetStage;

    //=============================================

    [Header("스테이지 종료 버튼")]
    public Button StageClear_btn;

    [Header("전체 덱 패널")]
    public GameObject allDeckPanel;
    [Header("전체 덱 카드홀더")]
    public Transform allDeckCardHolder;
    public GameObject allDeckCardCloseButton;
    public GameObject allDeckCardMask;

    [Header("핸드 카드 홀더")]
    public Transform handCardHolder;
    [Header("선택 카드 홀더")]  
    public Transform selectCardHolder;
    public List<GameObject> SelectedCard;

    [Header("점수 조합 정보창 패널 홀더")]
    public Transform ScoreCombinationInfoHolder;
    // 점수 조합 정보창 오브젝트모음
    List<ScoreCombinationInfoScript> ScoreCombinationInfoObject;

    [Header("스토어 패널")]
    public GameObject storePanel;
    [Header("스테이지 패널")]
    public GameObject stagePanel;
    [Header("점수 조합 패널")]
    public GameObject combinationPanel;
    [Header("게임오버 패널")]
    public GameObject gameoverPanel;
    [Header("스테이지 클리어 패널")]
    public GameObject stageClearPanel;

    [Header("조커 수 표시텍스트")]
    public TMP_Text txt_JockerCount;
    [Header("조커 패널")]
    public GameObject[] JockerPannel;
    [Header("조커 색상")]
    public Color32[] JockerColors;
    [Header("조커 정보창 게임 오브젝트")]
    public Button JockerInfoPannel;
    public RectTransform JockerInfoPannelTrans;
    public TMP_Text txt_JockerInfoName;
    public TMP_Text txt_JockerInfoDescription;
    public TMP_Text txt_JockerInfoSell;
    JockerUIScript CurrentFocusJocker;

    [Header("디버프 관련")]
    public Image DeBuffPannel;
    public TMP_Text txt_DeBuffName;
    public TMP_Text txt_LeftStage;
    public Color32[] DeBuffColor;

    public Image DeBuffInfoPannel;
    public TMP_Text txt_DeBuffInfoName;
    public TMP_Text txt_DeBuffInfoDescription;

    [Header("조커 10 무료 업그레이드 관련 항목")]
    public GameObject FreeUpgradePannel;
    public Button[] btn_FreeUpgrade;
    public int[] FreeUpgradeIndex;

    // 목표점수, 현재점수 패널
    public GameObject[] StageScorePanel;
    // 즐거운 업그레이드 시간! 패널
    public GameObject StoreScorePanel;

    // 제출, 덱, 버리기 버튼 못누르게 하는 커버
    public GameObject ButtonCover;

    // 전체 카드덱
    Dictionary<Card, AllDeckCardScript> AllDeck = new Dictionary<Card, AllDeckCardScript>();

    // 오브젝트 리스트
    List<GameObject> HandCards = new List<GameObject>();
    List<GameObject> SelectCards = new List<GameObject>();

    int StageCount = 0;

    // 스코어 스택 패널
    public GameObject[] scoreStackPannel = new GameObject[6];

    // 알파벳 표기법용 문자 리스트
    string[] CurrencyUnits = new string[] { "", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp", "cq", "cr", "cs", "ct", "cu", "cv", "cw", "cx", };

    // 조커 확률 관련 리셋
    public void LoadJockerPercentData(int[] inputJockerPercent)
    {
        JockerPercent = inputJockerPercent;
    }

    // 카드 관련 확률 리셋
    public void LoadCardPercentData(int[] inputPatternPercent, int[] inputColorPercent)
    {
        CardPatternPercent = inputPatternPercent;
        CardColorPercent = inputColorPercent;
    }

    // 큰수 변형지수표기법 함수
    public string LargeNumberToString(double number)
    {
        string zero = "0";

        if (-1d < number && number < 1d)
        {
            return zero;
        }

        if (double.IsInfinity(number))
        {
            return "Infinity";
        }

        //  부호 출력 문자열
        string significant = (number < 0) ? "-" : string.Empty;

        //  보여줄 숫자
        string showNumber = string.Empty;

        //  단위 문자열
        string unityString = string.Empty;

        //  패턴을 단순화 시키기 위해 무조건 지수 표현식으로 변경한 후 처리
        string[] partsSplit = number.ToString("E").Split('+');

        //  예외
        if (partsSplit.Length < 2)
        {
            return zero;
        }

        //  지수 (자릿수 표현)
        if (!int.TryParse(partsSplit[1], out int exponent))
        {
            Debug.LogWarningFormat("Failed - ToCurrentString({0}) : partSplit[1] = {1}", number, partsSplit[1]);
            return zero;
        }

        //  몫은 문자열 인덱스
        int quotient = exponent / 3;

        //  나머지는 정수부 자릿수 계산에 사용(10의 거듭제곱을 사용)
        int remainder = exponent % 3;

        //  1A 미만은 그냥 표현
        if (exponent < 3)
        {
            showNumber = System.Math.Truncate(number).ToString();
        }
        else
        {
            //  10의 거듭제곱을 구해서 자릿수 표현값을 만들어 준다.
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);
            //Debug.Log(temp);

            //  소수 첫째자리까지만 출력한다. dhqjdnj
            showNumber = temp.ToString("F1").Replace(".00", "");
        }

        unityString = CurrencyUnits[quotient];

        return string.Format("{0}{1}{2}", significant, showNumber, unityString);
    }

    // 지수 표기법
    public string ExponentialNumber(double number)
    {
        string snumber = " ";

        return snumber;
    }

    // 전체 덱 보기 패널 카드 오브젝트 생성
    public void GenerateAllDeckCard()
    {
        // 지역 변수
        Card curData;
        CardBasic generated;

        for (int i = 0; i < mainflow.userDeck.Count; i++) 
        {
            curData = mainflow.userDeck[i];

            // 오브젝트 생성
            generated = Instantiate(AllCardPrefab, allDeckCardHolder).GetComponent<CardBasic>();

            //generated.GetComponent<AllDeckCardScript>().uimanager = this;

            // 카드 데이터 이전
            generated.cardData = curData;
            // 카드 외형 설정
            generated.SetCardApear(PatternSprites[curData.patternIndex],PatternColors[curData.colorIndex], curData.number);

            // 전체 덱 카드 오브젝트 배열에 추가
            AllDeck.Add(curData, generated.GetComponent<AllDeckCardScript>());
        }
    }

    // 전체 덱 오브젝트 삭제
    public void DeleteAllDeckCard()
    {
        for (int i = 0; i < allDeckCardHolder.childCount; i++)
        {
            Destroy(allDeckCardHolder.GetChild(i).gameObject);
        }

        AllDeck.Clear();
    }

    // 전체 덱 카드에서 추가 카드 생성
    public void MakeAdditionalAllDeckCard(Card AddData)
    {
        // 지역 변수
        CardBasic generated;

        // 오브젝트 생성
        generated = Instantiate(AllCardPrefab, allDeckCardHolder).GetComponent<CardBasic>();

        //generated.GetComponent<AllDeckCardScript>().uimanager = this;

        // 카드 데이터 이전
        generated.cardData = AddData;
        // 카드 외형 설정
        generated.SetCardApear(PatternSprites[AddData.patternIndex], PatternColors[AddData.colorIndex], AddData.number);

        // 전체 덱 카드 오브젝트 배열에 추가
        AllDeck.Add(AddData, generated.GetComponent<AllDeckCardScript>());
    }

    public void StartStage()
    {
        // 덱 카드 커버 초기화
        ResetDeckCards();

        // 점수판 UI 텍스트 정보 갱신
        ResetScoreCostText();

        // 핸드 카드 비활성화
        DisableAllHandCard();

        // 선택된 카드 초기화
        DisableAllSelectCard();

        // 조커 수 텍스트 리셋
        ResetJockerCountText();
    }

    // 핸드 카드 오브젝트 생성  
    public void MakeHandCard(Card data)
    {
        HandCardScript generated;

        // 카드 오브젝트 생성
        generated = Instantiate(HandCardPrefab, handCardHolder).GetComponent<HandCardScript>();
        // 카드 외형 설정
        generated.SetCardApear(PatternSprites[data.patternIndex], PatternColors[data.colorIndex], data.number);
        // 카드 데이터 전달
        generated.cardData = data;

        // 선택 및 비선택 함수 전달
        generated.SelectFunc = (() => SelectHandCard(generated));
        generated.UnSelectFunc = (() => UnSelectCard(generated));

        // 전체 덱 에서 선택된 카드 커버 활성화
        AllDeck[data].ControlCover(true);

        // 핸드카드 오브젝트 리스트에 현재 오브젝트 추가
        HandCards.Add(generated.gameObject);
    }

    // 핸드카드 선택
    public void SelectHandCard(HandCardScript selfCard)
    {
        // 현재 선택된 카드가 최대 갯수가 아닐때
        if (mainflow.selectDeck.Count < mainflow.PlayableCards)
        {
            // 핸드덱에서 현재 카드 삭제
            mainflow.handDeck.Remove(selfCard.cardData);
            // 선택덱에 현재 카드 추가
            mainflow.selectDeck.Add(selfCard.cardData);
            // 선택 이미지 활성화
            selfCard.SetSelectPanel();

            // 핸드 카드 오브젝트 리스트에서 현재 오브젝트 제거
            HandCards.Remove(selfCard.gameObject);
            // 선택 카드 오브젝트 리스트에 현재 오브젝트 추가
            SelectCards.Add(selfCard.gameObject);

            selfCard.isOn = true;

            // 스코어 스택 프리뷰
            mainflow.CheckCurrentScore();
            ActivePreviewScoreStack();
        }
    }

    // 현재 선택 카드 점수 조합 패널에 생성
    public void MakeSelectedCard()
    {
        CardBasic cardBasic;
        SelectedCard.Clear();

        for (int i = 0; i < mainflow.selectDeck.Count(); i++) 
        {
            // 카드 오브젝트 생성
            cardBasic = Instantiate(SelectedCardPrefab, selectCardHolder).GetComponent<CardBasic>();
            // 카드 외형 설정
            cardBasic.SetCardApear
                (
                PatternSprites[mainflow.selectDeck[i].patternIndex], 
                PatternColors[mainflow.selectDeck[i].colorIndex], 
                mainflow.selectDeck[i].number
                );

            SelectedCard.Add(cardBasic.gameObject);
        }

        for (int j = 0; j < SelectCards.Count; j++)
        {
            // 핸드카드 오브젝트 삭제
            Destroy(SelectCards[j]);
        }
    }

    // 스코어 스택 리셋
    public void DisableAllScoreStack()
    {
        for (int i = 0; i < 6; i++) 
        {
            scoreStackPannel[i].SetActive(false);
        }
    }

    // 스코어 스택 프리뷰 ( 카드 넘버 합산 미제공 )
    public void ActivePreviewScoreStack()
    {
        DisableAllScoreStack();
        //int index = 0;
        //float temp = 0;

        //for (int i = 0; i < 6; i++)
        //{
        //    if (mainflow.scoreStackData[i].sum != 0)
        //    {
        //        scoreStackPannel[index].SetActive(true);
        //
        //        // 점수 명칭
        //        if (i < mainflow.PatternCount)
        //        {
        //            scoreStackPannel[index].transform.GetChild(0).GetComponent<TMP_Text>().text = mainflow.gameData.CardSetting.CardPatternName[i];
        //            temp = mainflow.scoreData.additional_PatternA[i];
        //        }
        //        else
        //        {
        //            scoreStackPannel[index].transform.GetChild(0).GetComponent<TMP_Text>().text = mainflow.gameData.CardSetting.CardColorName[i - mainflow.PatternCount];
        //            temp = mainflow.scoreData.additional_ColorA[i - mainflow.PatternCount];
        //        }
        //
        //        if(temp != 0)
        //        {
        //            // 점수
        //            scoreStackPannel[index].transform.GetChild(1).GetComponent<TMP_Text>().text = "((" + temp.ToString() + ") + " +
        //                "?) * " + mainflow.scoreStackData[i].b.ToString();// + " ^ " + mainflow.scoreStackData[i].c.ToString();
        //        }
        //        else
        //        {
        //            // 점수
        //            scoreStackPannel[index].transform.GetChild(1).GetComponent<TMP_Text>().text =
        //                "? * " + mainflow.scoreStackData[i].b.ToString();// + " ^ " + mainflow.scoreStackData[i].c.ToString();
        //        }
        //
        //        index++;
        //    }
        //}
    }

    // 선택된 카드 하이라이트
    public void HighLightSelectedCard(int index)
    {
        SelectedCard[index].GetComponent<Animator>().SetTrigger("IsOn");
    }

    // 스코어 스택 하이라이트
    public void HighLightScoreStack(int index)
    {
        scoreStackPannel[index].GetComponent<Animator>().SetTrigger("IsOn");
    }

    // 스코어 스택 개별 텍스트 설정
    public void ActiveIndivisualScoreStack(int index, double content)
    {
        //scoreStackPannel[index].transform.GetChild(1).GetComponent<TMP_Text>().text = LargeNumberToString(content);
    }

    // 스코어 스택 개별 텍스트 설정
    public void ActiveIndivisualScoreStackText(int index, string content)
    {
        scoreStackPannel[index].transform.GetChild(1).GetComponent<TMP_Text>().text = content;
    }

    // 스코어 스택 활성화 및 텍스트 설정
    public void ActiveScoreStack(string scoreName)
    {
        /*int index = 0;

        // 비활성화된 오브젝트 탐색
        for (int i = 0; i < 6; i++)
        {
            if(scoreStackPannel[i].activeSelf == false)
            {
                index = i;
                break;
            }
        }

        // 오브젝트 텍스트 설정
        scoreStackPannel[index].transform.GetChild(0).GetComponent<TMP_Text>().text = scoreName;
        scoreStackPannel[index].transform.GetChild(1).GetComponent<TMP_Text>().text = 
            stackData.a.ToString() + " * " + stackData.b.ToString() + " ^ " + stackData.c.ToString();

        // 오브젝트 활성화
        scoreStackPannel[index].SetActive(true);*/
    }

    // 골드 텍스트 갱신
    public void ResetGoldText()
    {
        Gold_txt.text = mainflow.CurrentGold.ToString();
    }

    // 현재 선택 카드 버리기
    public void SelectCardDump()
    {
        if(mainflow.selectDeck.Count() == 0)
        {
            msgPannel.PrintMessage("선택된 카드가 없습니다!");
            return;
        }

        if(mainflow.CurrentDumpCost > 0)
        {
            // 오브젝트 삭제
            for (int i = 0; i < SelectCards.Count; i++) 
            {
                // 오브젝트 제거
                Destroy(SelectCards[i]);
            }

            // 버리기 코스트 감소
            mainflow.CurrentDumpCost -= 1;
            ResetScoreCostText();

            // 덱 초기화
            SelectCards.Clear();

            // 선택 덱 초기화
            mainflow.selectDeck.Clear();

            // 카드 재생성
            mainflow.PickRandomCard();

            // 스코어 스택 프리뷰
            mainflow.CheckCurrentScore();
            ActivePreviewScoreStack();
        }
        else
        {
            msgPannel.PrintMessage("버리기 코스트가 부족합니다!");
        }
    }

    // 선택된 카드 철회
    public void UnSelectCard(HandCardScript selfCard)
    {
        // 선택덱 에서 현재 카드 삭제
        mainflow.selectDeck.Remove(selfCard.cardData);
        // 핸드덱에 현재 카드 추가
        mainflow.handDeck.Add(selfCard.cardData);
        // 선택 이미지 비활성화
        selfCard.SetSelectPanel();

        // 핸드 카드 오브젝트 리스트에서 현재 오브젝트 제거
        SelectCards.Remove(selfCard.gameObject);
        // 선택 카드 오브젝트 리스트에 현재 오브젝트 추가
        HandCards.Add(selfCard.gameObject);

        selfCard.isOn = false;

        // 스코어 스택 프리뷰
        mainflow.CheckCurrentScore();
        ActivePreviewScoreStack();
    }

    // 점수 및 유저 코스트, 스테이지 텍스트 갱신
    public void ResetScoreCostText()
    {
        CurrentScore_txt.text = LargeNumberToString(mainflow.CurrentScore);
        GoalScore_txt.text = LargeNumberToString(mainflow.GoalScore);

        ActionCost_txt.text = mainflow.CurrentActionCost.ToString();
        DumpCost_txt.text = mainflow.CurrentDumpCost.ToString();
        CurrentStage_txt.text = mainflow.CurrentStage.ToString();

        HandSize_txt.text = mainflow.HandSize.ToString();
        PlaySize_txt.text = mainflow.PlayableCards.ToString();
    }

    // 덱 정보 창 초기화
    public void ResetDeckCards()
    {
        int leng = allDeckCardHolder.childCount;

        for(int i = 0; i < leng; i++)
        {
            allDeckCardHolder.GetChild(i).GetComponent<AllDeckCardScript>().ControlCover(false);
        }
    }

    // 모든 선택카드 비활성화
    public void DisableAllSelectCard()
    {
        // 선택카드 자손갯수 불러오기
        int count = selectCardHolder.childCount;

        if (count != 0)
        {
            for (int i = 0; i < count; i++) 
            {
                Destroy(selectCardHolder.GetChild(i).gameObject);
            }
        }
    }

    // 모든 핸드카드 비활성화
    public void DisableAllHandCard()
    {
        // 핸드카드 자손갯수 불러오기
        int count = handCardHolder.childCount;

        if (count != 0)
        {
            for (int i = 0; i < count; i++) 
            {
                Destroy(handCardHolder.GetChild(i).gameObject);
            }
        }
    }

    // 스토어 집입 혹은 퇴출시 점수 패널 켜기/끄기
    public void SwitchStoreScorePanel(bool isStore)
    {
        for(int i = 0; i < StageScorePanel.Length; i++)
        {
            StageScorePanel[i].SetActive(!isStore);
        }

        StoreScorePanel.SetActive(isStore);
    }

    public void StageClearToNext()
    {
        //if(mainflow.CurrentStage % mainflow.gameData.StorePerStage == 0)
        //{
        //    stageClearPanel.SetActive(false);
        //    stagePanel.SetActive(false);
        //    storePanel.SetActive(true);
        //    DestroyAllUpgradeBtn();
        //    RandomStoreUpgrade();
        //}
        //else
        //{
        //    stageClearPanel.SetActive(false);
        //    stagePanel.SetActive(true);
        //    mainflow.StartStage();
        //}
    }

    public void StoreToStage()
    {
        storePanel.SetActive(false);
        stagePanel.SetActive(true);
        SwitchStoreScorePanel(false);
        DestroyAllUpgradeBtn();

        mainflow.StartStage();
    }

    // 스토어 업그레이드 랜덤 선정 및 UI 설정
    private void RandomStoreUpgrade()
    {
        // 리롤 코스트 초기화
        CurrentReRollCost = ReRollCost;
        // 리롤 가격 텍스트 갱신
        txt_ReRollCost.text = "리롤 " + CurrentReRollCost.ToString() + "골드";

        int scorePercentSum = 0;
        List<int> scorePercent = new List<int>();

        // 랜덤 풀
        List<int> pool_score = new List<int>();
        List<int> pool_action = new List<int>();
        List<int> pool_singleuse = new List<int>();

        // 패턴 및 컬러 풀 지정
        for (int i = 0; i < mainflow.PatternCount; i++)
        {
            if (mainflow.PatternLevel[i] < 100)
            {
                pool_score.Add(i);
                scorePercentSum += CardPatternPercent[i];
                scorePercent.Add(CardPatternPercent[i]);
            }
        }
        // 컬러
        for (int i = 0; i < mainflow.ColorCount; i++)
        {
            if (mainflow.ColorLevel[i] < 100)
            {
                pool_score.Add(i + mainflow.PatternCount);
                scorePercentSum += CardColorPercent[i];
                scorePercent.Add(CardColorPercent[i]);
            }
        }
        // 조커 풀 지정
        for (int i = 0; i < mainflow.JockerData.Count(); i++) 
        {
            if(!mainflow.PlayerJocker.Contains(mainflow.JockerData[i]))
            {
                pool_score.Add(i + mainflow.PatternCount + mainflow.ColorCount);
                scorePercentSum += JockerPercent[i];
                scorePercent.Add(JockerPercent[i]);
            }
        }

        //=============================================

        Button temp_btn;
        BtnIndex temp_btnindex;
        int temp_index;
        int temp_sum;

        // 조합 점수 업그레이드 설정
        for (int i = 0; i < 3; i++)
        {
            // 랜덤 인덱스 산출
            temp_index = UnityEngine.Random.Range(0, scorePercentSum);

            temp_sum = 0;

            // 전체 확률 순회
            for (int j = 0; j < scorePercent.Count(); j++) 
            {
                temp_sum += scorePercent[j];

                if (temp_index < temp_sum)
                {
                    temp_index = pool_score[j];

                    scorePercentSum -= scorePercent[j];
                    scorePercent.RemoveAt(j);
                    break;
                }
            }

            // 산출된 인덱스 풀에서 제거
            pool_score.Remove(temp_index);

            // 업그레이드 인덱스 할당
            UpgradeIndex1[i] = temp_index;

            // 버튼 오브젝트 생성
            temp_btn = Instantiate(UpgradeBtnPrefab, UpgradeHolder1).GetComponent<Button>();
            // 버튼 색상 지정
            temp_btn.GetComponent<Image>().color = UpgradeBtnColor[0];

            
            temp_btnindex = temp_btn.GetComponent<BtnIndex>();
            // 인덱스 지정
            temp_btnindex.Index = i;
            temp_btnindex.Type = 0;
            temp_btnindex.uimanager = this;

            // 패턴 일때
            if (temp_index < 3)
            {
                string temp = " ";

                // 명칭 설정
                temp = mainflow.gameData.CardSetting.CardPatternName[temp_index];

                temp_btn.transform.GetChild(0).GetComponent<TMP_Text>().text
                = temp + " 가중치 +1";

                //temp_btn.transform.GetChild(1).GetComponent<TMP_Text>().text
                //= "현재 가중치 : " + mainflow.CardPatternScore[temp_index];
            }
            // 컬러 일때
            else if (temp_index < 6)
            {
                string temp = " ";

                // 명칭 설정
                temp = mainflow.gameData.CardSetting.CardColorName[temp_index - mainflow.PatternCount];

                temp_btn.transform.GetChild(0).GetComponent<TMP_Text>().text
                = temp + " 가중치 +1";

                //temp_btn.transform.GetChild(1).GetComponent<TMP_Text>().text
                //= "현재 가중치 : " + mainflow.CardColorScore[temp_index - 3];
            }
            // 조커 일때
            else
            {
                string temp = " ";

                //temp = "조커 / " + mainflow.JockerData[temp_index - (mainflow.PatternCount + mainflow.ColorCount)].Name;

                temp_btn.transform.GetChild(0).GetComponent<TMP_Text>().text
                = temp;

                //temp_btn.transform.GetChild(1).GetComponent<TMP_Text>().text
                //= mainflow.JockerData[temp_index - (mainflow.PatternCount + mainflow.ColorCount)].Description;
            }

            // 가격 텍스트 설정
            if (temp_index < 6)
            {
                temp_btn.transform.GetChild(2).GetComponent<TMP_Text>().text
                = "10골드";
            }
            else
            {
                //temp_btn.transform.GetChild(2).GetComponent<TMP_Text>().text
                //= mainflow.JockerData[temp_index - (mainflow.PatternCount + mainflow.ColorCount)].Cost + "골드";
            }

            if (pool_score.Count <= 0 )
                break;
        }

        //Debug.Log(VoucherResetStage);

        StageCount++;

        // 행동 업그레이드, 3스테이지 동안 유지되며, 4스테이지 도달 시 초기화
        if (StageCount == VoucherResetStage || mainflow.CurrentStage == 1)
        {
            StageCount = 0;
            //txt_LeftStageToResetVoucher.gameObject.SetActive(false);

            // 오브젝트 제거
            for (int i = 0; i < UpgradeHolder2.childCount; i++)
            {
                Destroy(UpgradeHolder2.GetChild(i).gameObject);
            }

            // 행동 풀 지정
            if (mainflow.HandSize < 8)
            {
                pool_action.Add(0);
            }
            if (mainflow.PlayableCards < 6)
            {
                pool_action.Add(1);
            }
            if (mainflow.ActionCost < 8)
            {
                pool_action.Add(2);
            }
            if (mainflow.DumpCost < 8)
            {
                pool_action.Add(3);
            }

            // 행동 업그레이드 설정
            if (pool_action.Count > 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    // 랜덤 인덱스 산출
                    temp_index = pool_action[UnityEngine.Random.Range(0, pool_action.Count)];

                    // 산출된 인덱스 풀에서 제거
                    pool_action.Remove(temp_index);

                    // 업그레이드 인덱스 할당
                    UpgradeIndex2[i] = temp_index;

                    // 버튼 오브젝트 생성
                    temp_btn = Instantiate(UpgradeBtnPrefab, UpgradeHolder2).GetComponent<Button>();
                    // 버튼 색상 지정
                    temp_btn.GetComponent<Image>().color = UpgradeBtnColor[1];

                    temp_btn.GetComponent<BtnIndex>().Index = i;

                    temp_btnindex = temp_btn.GetComponent<BtnIndex>();
                    // 인덱스 지정
                    temp_btnindex.Index = i;
                    temp_btnindex.Type = 1;
                    temp_btnindex.uimanager = this;

                    string temp = " ";
                    temp_sum = -419;

                    // 도형 명칭
                    switch (temp_index)
                    {
                        case 0:
                            {
                                temp = "핸드 크기";
                                temp_sum = mainflow.HandSize;
                                break;
                            }
                        case 1:
                            {
                                temp = "플레이 크기";
                                temp_sum = mainflow.PlayableCards;
                                break;
                            }
                        case 2:
                            {
                                temp = "행동 횟수";
                                temp_sum = mainflow.ActionCost;
                                break;
                            }
                        case 3:
                            {
                                temp = "버리기 횟수";
                                temp_sum = mainflow.DumpCost;
                                break;
                            }
                    }

                    temp_btn.transform.GetChild(0).GetComponent<TMP_Text>().text
                    = temp + " +1";

                    temp_btn.transform.GetChild(1).GetComponent<TMP_Text>().text
                    = "현재 " + temp + " : " + temp_sum.ToString();

                    temp_btn.transform.GetChild(2).GetComponent<TMP_Text>().text
                    = "20골드";

                    if (pool_action.Count <= 0)
                        break;
                }
            }
        }

        // 바우처 리셋 남은 스테이지 텍스트 초기화
        txt_LeftStageToResetVoucher.text = (VoucherResetStage - StageCount).ToString() + "스테이지 이후 업데이트";

        // 일회성 풀 지정
        for (int i = 0; i < 4; i++)
            {
                pool_singleuse.Add(i);
            }

        // 일회성 업그레이드 
        for (int i = 0; i < 2; i++)
            {
                // 랜덤 인덱스 산출
                temp_index = pool_singleuse[UnityEngine.Random.Range(0, pool_singleuse.Count)];

                // 산출된 인덱스 풀에서 제거
                pool_singleuse.Remove(temp_index);

                // 업그레이드 인덱스 할당
                UpgradeIndex3[i] = temp_index;

                // 버튼 오브젝트 생성
                temp_btn = Instantiate(UpgradeBtnPrefab, UpgradeHolder3).GetComponent<Button>();
                // 버튼 색상 지정
                temp_btn.GetComponent<Image>().color = UpgradeBtnColor[2];

                temp_btn.GetComponent<BtnIndex>().Index = i;

                temp_btnindex = temp_btn.GetComponent<BtnIndex>();
                // 인덱스 지정
                temp_btnindex.Index = i;
                temp_btnindex.Type = 2;
                temp_btnindex.uimanager = this;

                string temp = " ";
                string temp2 = " ";

                int goldamount = 0;

                // 도형 명칭
                switch (temp_index)
                {
                    case 0:
                        {
                            temp = "덱 삭제";
                            temp2 = "덱 에서 카드 한장 삭제";
                            goldamount = 15;
                            break;
                        }
                    case 1:
                        {
                            temp = "덱 추가";
                            temp2 = "덱에 랜덤한 카드 한장 추가";
                            goldamount = 15;
                            break;
                        }
                    case 2:
                        {
                            temp = "최종 점수 2배";
                            temp2 = "다음 라운드 동안 최종 점수 2배";
                            goldamount = 10;
                            break;
                        }
                    case 3:
                        {
                            temp = "현재 골드 2배";
                            temp2 = "( 최대 40 )";
                            goldamount = 10;
                            break;
                        }
                }

                temp_btn.transform.GetChild(0).GetComponent<TMP_Text>().text
                = temp;

                temp_btn.transform.GetChild(1).GetComponent<TMP_Text>().text
                = temp2;

                temp_btn.transform.GetChild(2).GetComponent<TMP_Text>().text
                = goldamount.ToString() + "골드";

                if (pool_singleuse.Count <= 0)
                    break;
            }
    }

    // 리롤
    public void ReRoll()
    {
        // 골드 부족
        if (mainflow.CurrentGold < CurrentReRollCost)
        {
            msgPannel.PrintMessage("골드가 부족합니다");
            return;
        }

        // 골드 감소
        mainflow.CurrentGold -= CurrentReRollCost;
        CurrentReRollCost++;
        // 리롤 가격 텍스트 갱신
        txt_ReRollCost.text = "리롤 " + CurrentReRollCost.ToString() + "골드";
        // 골드 텍스트 갱신
        ResetGoldText();

        // 기존 오브젝트 삭제
        for (int i = 0; i < UpgradeHolder1.childCount; i++)
        {
            Destroy(UpgradeHolder1.GetChild(i).gameObject);
        }

        // 랜덤 풀
        List<int> pool_score = new List<int>();

        // 패턴 및 컬러 풀 지정
        for (int i = 0; i < mainflow.PatternCount; i++)
        {
            if (mainflow.PatternLevel[i] < 100)
            {
                pool_score.Add(i);
            }
        }
        // 컬러
        for (int i = 0; i < mainflow.ColorCount; i++)
        {
            if (mainflow.ColorLevel[i] < 100)
            {
                pool_score.Add(i + mainflow.PatternCount);
            }
        }
        // 조커 풀 지정
        for (int i = 0; i < mainflow.JockerData.Count(); i++)
        {
            if (!mainflow.PlayerJocker.Contains(mainflow.JockerData[i]))
            {
                pool_score.Add(i + mainflow.PatternCount + mainflow.ColorCount);
            }
        }

        //=============================================

        Button temp_btn;
        BtnIndex temp_btnindex;
        int temp_index;

        // 조합 점수 업그레이드 설정
        for (int i = 0; i < 3; i++)
        {
            // 랜덤 인덱스 산출
            temp_index = pool_score[UnityEngine.Random.Range(0, pool_score.Count)];

            // 산출된 인덱스 풀에서 제거
            pool_score.Remove(temp_index);

            // 업그레이드 인덱스 할당
            UpgradeIndex1[i] = temp_index;

            // 버튼 오브젝트 생성
            temp_btn = Instantiate(UpgradeBtnPrefab, UpgradeHolder1).GetComponent<Button>();
            // 버튼 색상 지정
            temp_btn.GetComponent<Image>().color = UpgradeBtnColor[0];


            temp_btnindex = temp_btn.GetComponent<BtnIndex>();
            // 인덱스 지정
            temp_btnindex.Index = i;
            temp_btnindex.Type = 0;
            temp_btnindex.uimanager = this;

            // 패턴 일때
            if (temp_index < 3)
            {
                string temp = " ";

                // 명칭 설정
                temp = mainflow.gameData.CardSetting.CardPatternName[temp_index];

                temp_btn.transform.GetChild(0).GetComponent<TMP_Text>().text
                = temp + " 가중치 +1";

                //temp_btn.transform.GetChild(1).GetComponent<TMP_Text>().text
                //= "현재 가중치 : " + mainflow.CardPatternScore[temp_index];
            }
            // 컬러 일때
            else if (temp_index < 6)
            {
                string temp = " ";

                // 명칭 설정
                temp = mainflow.gameData.CardSetting.CardColorName[temp_index - mainflow.PatternCount];

                temp_btn.transform.GetChild(0).GetComponent<TMP_Text>().text
                = temp + " 가중치 +1";

                //temp_btn.transform.GetChild(1).GetComponent<TMP_Text>().text
                //= "현재 가중치 : " + mainflow.CardColorScore[temp_index - 3];
            }
            // 조커 일때
            else
            {
                string temp = " ";

                //temp = "조커 / " + mainflow.JockerData[temp_index - (mainflow.PatternCount + mainflow.ColorCount)].Name;

                temp_btn.transform.GetChild(0).GetComponent<TMP_Text>().text
                = temp;

                //temp_btn.transform.GetChild(1).GetComponent<TMP_Text>().text
                //= mainflow.JockerData[temp_index - (mainflow.PatternCount + mainflow.ColorCount)].Description;
            }

            // 가격 텍스트 설정
            if (temp_index < 6)
            {
                temp_btn.transform.GetChild(2).GetComponent<TMP_Text>().text
                = "10골드";
            }
            else
            {
                //temp_btn.transform.GetChild(2).GetComponent<TMP_Text>().text
                //= mainflow.JockerData[temp_index - (mainflow.PatternCount + mainflow.ColorCount)].Cost + "골드";
            }

            if (pool_score.Count <= 0)
                break;
        }
    }

    // 업그레이드 버튼 오브젝트 전체 삭제
    public void DestroyAllUpgradeBtn()
    {
        for (int i = 0; i < UpgradeHolder1.childCount; i++) 
        {
            Destroy(UpgradeHolder1.GetChild(i).gameObject);
        }
        //for (int i = 0; i < UpgradeHolder2.childCount; i++)
        //{
        //    Destroy(UpgradeHolder2.GetChild(i).gameObject);
        //}
        for (int i = 0; i < UpgradeHolder3.childCount; i++)
        {
            Destroy(UpgradeHolder3.GetChild(i).gameObject);
        }
    }

    // 조합 점수 및 조커 업그레이드
    public void ScoreUpgrade(int index, Button selfBtn)
    {
        // 골드 부족
        if (UpgradeIndex1[index] < 6)
        {
            if(mainflow.CurrentGold < 10)
            {
                msgPannel.PrintMessage("골드가 부족합니다");
                return;
            }
        }
        //else if (mainflow.CurrentGold < mainflow.JockerData[UpgradeIndex1[index] - (mainflow.PatternCount + mainflow.ColorCount)].Cost)
        //{
        //    msgPannel.PrintMessage("골드가 부족합니다");
        //    return;
        //}

        // 조커 갯수 초과
        if(UpgradeIndex1[index] >= 6 && mainflow.PlayerJocker.Count() == 5)
        {
            msgPannel.PrintMessage("조커가 이미 5장 입니다");
            return;
        }

        // 패턴 일때
        if (UpgradeIndex1[index] < 3)
        {
            //mainflow.CardPatternScore[UpgradeIndex1[index]] += 1;
            mainflow.PatternLevel[UpgradeIndex1[index]] += 1;
        }
        // 컬러 일때
        else if (UpgradeIndex1[index] < 6)
        {
            //mainflow.CardColorScore[UpgradeIndex1[index] - 3] += 1;
            mainflow.ColorLevel[UpgradeIndex1[index] - 3] += 1;
        }
        // 조커 일때
        else
        {
            mainflow.PlayerJocker.Add(mainflow.JockerData[UpgradeIndex1[index] - (mainflow.PatternCount + mainflow.ColorCount)]);
            // 구매 시 이벤트 호출
            //mainflow.JockerData[UpgradeIndex1[index] - (mainflow.PatternCount + mainflow.ColorCount)].Event_GetItem(TODO);
            EnableJockerUI(UpgradeIndex1[index] - (mainflow.PatternCount + mainflow.ColorCount));
        }

        // 골드 감소
        if(UpgradeIndex1[index] < 6)
        {
            mainflow.CurrentGold -= 10;
        }
        else
        {
            //mainflow.CurrentGold -= mainflow.JockerData[UpgradeIndex1[index] - (mainflow.PatternCount + mainflow.ColorCount)].Cost;
        }

        SetCombinationScoresText();
        ResetGoldText();

        // 버튼 비활성화
        UnActiveBtn(selfBtn);
    }
    
    // 행동 업그레이드
    public void ActionUpgrade(int index, Button selfBtn)
    {
        if (mainflow.CurrentGold < 20)
        {
            msgPannel.PrintMessage("골드가 부족합니다");
        }
        else
        {
            switch (UpgradeIndex2[index])
            {
                case 0:
                    mainflow.HandSize++;
                    break;
                case 1:
                    mainflow.PlayableCards++;
                    break;
                case 2:
                    mainflow.ActionCost++;
                    break;
                case 3:
                    mainflow.DumpCost++;
                    break;
            }

            mainflow.CurrentGold -= 20;
            ResetGoldText();

            // 버튼 비활성화
            UnActiveBtn(selfBtn);
        }
    }

    // 일회성 업그레이드
    public void SingleUseUpgrade(int index, Button selfBtn)
    {
        if(mainflow.CurrentGold < 10)
        {
            msgPannel.PrintMessage("골드가 부족합니다");
        }
        else if((mainflow.CurrentGold < 15) && (UpgradeIndex3[index] == 0 || UpgradeIndex3[index] == 1))
        {
            msgPannel.PrintMessage("골드가 부족합니다");
        }
        else
        {
            switch (UpgradeIndex3[index])
            {
                // 덱 삭제
                case 0:
                    Debug.Log("덱 삭제");
                    // 전체 덱 패널 ON
                    ControlDeckPanel(true);

                    // 커버 및 닫기 버튼 비활성화
                    allDeckCardMask.SetActive(false);
                    allDeckCardCloseButton.SetActive(false);

                    // 골드 감소
                    mainflow.CurrentGold -= 15;
                    break;

                // 덱 추가
                case 1:
                    Debug.Log("덱 추가");
                    // 랜덤 카드 추가
                    Card newCard = mainflow.MakeAdditionalRandomCard();

                    string msg = mainflow.gameData.CardSetting.CardColorName[newCard.colorIndex] + " " +
                        mainflow.gameData.CardSetting.CardPatternName[newCard.patternIndex] + " " + 
                        newCard.number + " 카드 추가 완료";

                    msgPannel.PrintMessage(msg);

                    // 골드 감소
                    mainflow.CurrentGold -= 15;
                    break;

                // 최종 점수 2배
                case 2:
                    Debug.Log("최종점수 2배");
                    ScoreDoublePannel.SetActive(true);
                    // 최종 점수 2배 적용
                    mainflow.ScoreMultiplyAmount = 2;

                    // 골드 감소
                    mainflow.CurrentGold -= 10;
                    break;

                // 현재 골드 2배
                case 3:
                    Debug.Log("골드 2배");
                    msgPannel.PrintMessage("현재 골드량이 2배가 되었습니다");

                    // 골드 감소
                    mainflow.CurrentGold -= 10;
                    // 골드 2배
                    mainflow.CurrentGold = mainflow.CurrentGold * 2;
                    break;
            }

            ResetGoldText();

            // 버튼 비활성화
            UnActiveBtn(selfBtn);
        }
    }

    // 디버프 UI 갱신
    public void ResetDeBuffUI()
    {
        //txt_DeBuffName.text = mainflow.CurrentDeBuff.Name;
        if(mainflow.CurrentStage % 3 == 0)
        {
            txt_LeftStage.text = "디버프 적용됨";
        }
        else
        {
            txt_LeftStage.text = (3 - (mainflow.CurrentStage % 3)).ToString() + "스테이지 이후";
        }
       // DeBuffPannel.color = DeBuffColor[mainflow.CurrentDeBuff.index];

        //txt_DeBuffInfoName.text = mainflow.CurrentDeBuff.Name;
       //txt_DeBuffInfoDescription.text = mainflow.CurrentDeBuff.Description;
        //DeBuffInfoPannel.color = DeBuffColor[mainflow.CurrentDeBuff.index];
    }

    // 디버프 설명 창 활성화, 비활성화
    public void SetActiveDeBuffInfoPannel(bool isActive)
    {
        DeBuffInfoPannel.gameObject.SetActive(isActive);

        if(isActive)
        {
            Vector3 temp_pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

            // 오브젝트 위치 설정
            DeBuffInfoPannel.transform.position = temp_pos;
        }
    }

    // 조커 정보창 활성화 함수
    public void EnableJockerInfoPannel(JockerUIScript selfJocker)
    {
        // 오브젝트 활성화
        JockerInfoPannel.gameObject.SetActive(true);

        // 패널색상 설정
        JockerInfoPannel.GetComponent<Image>().color = JockerColors[selfJocker.Index];

        // 텍스트 설정
        //txt_JockerInfoName.text = mainflow.JockerData[selfJocker.Index].Name;
        //txt_JockerInfoDescription.text = mainflow.JockerData[selfJocker.Index].Description;
        //txt_JockerInfoSell.text = mainflow.JockerData[selfJocker.Index].SellCost.ToString() + "골드에 판매";

        Vector3 temp_pos = new Vector3(Input.mousePosition.x + 150, Input.mousePosition.y - 200, 0);

        // 오브젝트 위치 설정
        JockerInfoPannelTrans.position = temp_pos;

        // 현재 인덱스 설정
        CurrentFocusJocker = selfJocker;
    }

    // 조커 정보창 비활성화 함수
    public void DisableJockerInfoPannel()
    {
        // 오브젝트 비활성화
        JockerInfoPannel.gameObject.SetActive(false);
    }

    // 조커 판매
    public void SellJocker()
    {
        // 조커 정보 창 비활성화
        DisableJockerInfoPannel();
        // 조커 UI 비활성화
        DisableJockerUI(CurrentFocusJocker.Index);

        // 골드 텍스트 업데이트
        ResetGoldText();
        // 조커 수 텍스트 리셋
        ResetJockerCountText();
    }

    // 조커 수 표시 텍스트 리셋
    public void ResetJockerCountText()
    {
        txt_JockerCount.text = mainflow.PlayerJocker.Count().ToString() + " / 5";
    }

    // 조커 UI 생성
    public void EnableJockerUI(int index)
    {
        for (int i = 0; i < 5; i++) 
        {
            if (!JockerPannel[i].activeSelf)
            {
                JockerPannel[i].SetActive(true);
                JockerPannel[i].GetComponent<Image>().color = JockerColors[index]; 
                //JockerPannel[i].transform.GetChild(0).GetComponent<TMP_Text>().text
                //    = mainflow.JockerData[index].Name;
                JockerPannel[i].GetComponent<JockerUIScript>().Index = index;

                // 조커 수 카운트 텍스트 초기화
                ResetJockerCountText();

                return;
            }
        }
    }

    // 조커 UI 제거, 메인플로우 플레이어 조커 데이터에서 제거
    public void DisableJockerUI(int index,bool isSell = true)
    {
        for (int i = 0; i < 5; i++)
        {
            if (JockerPannel[i].activeSelf)
            {
                if(JockerPannel[i].GetComponent<JockerUIScript>().Index == index)
                {
                    JockerPannel[i].SetActive(false);
                    mainflow.PlayerJocker.Remove(mainflow.JockerData[index]);
                    //mainflow.JockerData[index].Event_Sell(TODO);
                    break;
                }
            }
        }

        if(isSell)
        {
            //mainflow.CurrentGold += mainflow.JockerData[index].SellCost;
        }
    }

    // 조커 무력화 패널 설정
    public int SetActiveJockerDisablePannel(bool isActive, int index)
    {
        for (int i = 0; i < 5; i++)
        {
            if (JockerPannel[i].activeSelf)
            {
                if (JockerPannel[i].GetComponent<JockerUIScript>().Index == index)
                {
                    JockerPannel[i].GetComponent<JockerUIScript>().SetActiveDisablePannel(isActive);
                    return i;
                }
            }
        }
        return -1;
    }

    // 조커 10 무료 업그레이드 패널 활성화
    public void EnableFreeUpgradePannel()
    {
        // 패널 오브젝트 활성화
        FreeUpgradePannel.SetActive(true);

        btn_FreeUpgrade[0].gameObject.SetActive(false);
        btn_FreeUpgrade[1].gameObject.SetActive(false);
        btn_FreeUpgrade[2].gameObject.SetActive(false);

        // 랜덤 풀
        List<int> pool_score = new List<int>();

        // 패턴 및 컬러 풀 지정
        for (int i = 0; i < mainflow.PatternCount; i++)
        {
            if (mainflow.PatternLevel[i] < 100)
            {
                pool_score.Add(i);
            }
        }
        // 컬러
        for (int i = 0; i < mainflow.ColorCount; i++)
        {
            if (mainflow.ColorLevel[i] < 100)
            {
                pool_score.Add(i + mainflow.PatternCount);
            }
        }

        Button temp_btn;
        int temp_index;

        // 조합 점수 업그레이드 설정
        for (int i = 0; i < 3; i++)
        {
            // 랜덤 인덱스 산출
            temp_index = pool_score[UnityEngine.Random.Range(0, pool_score.Count)];

            // 산출된 인덱스 풀에서 제거
            pool_score.Remove(temp_index);

            // 업그레이드 인덱스 할당
            FreeUpgradeIndex[i] = temp_index;

            // 버튼 오브젝트 
            temp_btn = btn_FreeUpgrade[i];

            // 패턴 일때
            if (temp_index < 3)
            {
                string temp = " ";

                // 명칭 설정
                temp = mainflow.gameData.CardSetting.CardPatternName[temp_index];

                temp_btn.transform.GetChild(0).GetComponent<TMP_Text>().text
                = temp + " 가중치 +1";

                //temp_btn.transform.GetChild(1).GetComponent<TMP_Text>().text
                //= "현재 가중치 : " + mainflow.CardPatternScore[temp_index];
            }
            // 컬러 일때
            else if (temp_index < 6)
            {
                string temp = " ";

                // 명칭 설정
                temp = mainflow.gameData.CardSetting.CardColorName[temp_index - mainflow.PatternCount];

                temp_btn.transform.GetChild(0).GetComponent<TMP_Text>().text
                = temp + " 가중치 +1";

                //temp_btn.transform.GetChild(1).GetComponent<TMP_Text>().text
                //= "현재 가중치 : " + mainflow.CardColorScore[temp_index - 3];
            }

            // 버튼 오브젝트 활성화
            btn_FreeUpgrade[i].gameObject.SetActive(true);

            if (pool_score.Count <= 0)
                break;
        }
    }

    // 무료 업그레이드
    public void FreeUpgrade(int index)
    {
        // 패턴 일때
        if (FreeUpgradeIndex[index] < 3)
        {
            //mainflow.CardPatternScore[FreeUpgradeIndex[index]] += 1;
            mainflow.PatternLevel[FreeUpgradeIndex[index]] += 1;
        }
        // 컬러 일때
        else if (FreeUpgradeIndex[index] < 6)
        {
            //mainflow.CardColorScore[FreeUpgradeIndex[index] - 3] += 1;
            mainflow.ColorLevel[FreeUpgradeIndex[index] - 3] += 1;
        }

        SetCombinationScoresText();

        // 패널 오브젝트 활성화
        FreeUpgradePannel.SetActive(false);
    }

    // 전체 덱 에서 선택 카드 삭제
    public void ClickAllDeckCardDelete(Card curData, GameObject targetCard)
    {
        // 전체 덱에서 삭제
        // 메인 플로우 덱에서 삭제
        mainflow.userDeck.Remove(curData);

        // 전체 덱 오브젝트 삭제
        Destroy(targetCard);
        // 전체 덱 데이터에서 삭제
        AllDeck.Remove(curData);

        // 전체 덱 패널 Off
        ControlDeckPanel(false);

        // 커버 및 닫기 버튼 활성화
        allDeckCardMask.SetActive(true);
        allDeckCardCloseButton.SetActive(true);
    }

    public void UnActiveBtn(Button button)
    {
        button.interactable = false;
    }

    // 조합 정보 창 조합 점수 텍스트 갱신
    public void SetCombinationScoresText()
    {
        for (int i = 0; i < 6; i++)
        {
            if (i < 3)
            {
                // 텍스트 설정
                //ScoreCombinationInfoObject[i].UpdateScore
                //    (mainflow.CardPatternScore[i].ToString());
            }
            else
            {
                // 텍스트 설정
                //ScoreCombinationInfoObject[i].UpdateScore
                //    (mainflow.CardColorScore[i - 3].ToString());
            }
        }
    }

    // 점수 조합 패널 생성
    public void GenerateCombinationScorePanel()
    {
        //ScoreCombinationInfoScript generated;
        //ScoreCombinationInfoObject = new List<ScoreCombinationInfoScript>();

        //for (int i = 0; i < 6; i++) 
        //{
        //    string tempName = " ";

        //    // 명칭 텍스트 설정
        //    switch (i)
        //    { 
        //        case 0:
        //            {
        //                tempName = "도형-원";
        //                break;
        //            }
        //        case 1:
        //            {
        //                tempName = "도형-사각형";
        //                break;
        //            }
        //        case 2:
        //            {
        //                tempName = "도형-삼각형";
        //                break;
        //            }
        //        case 3:
        //            {
        //                tempName = "색상-빨강";
        //                break;
        //            }
        //        case 4:
        //            {
        //                tempName = "색상-초록";
        //                break;
        //            }
        //        case 5:
        //            {
        //                tempName = "색상-파랑";
        //                break;
        //            }
        //    }

        //    // 오브젝트 생성
        //    generated = Instantiate(ScoreConbinationInfoPanelPrefab, ScoreCombinationInfoHolder).GetComponent<ScoreCombinationInfoScript>();
            
        //    if(i < 3)
        //    {
        //        // 텍스트 설정
        //        //generated.SetText
        //        //    (tempName, mainflow.CardPatternScore[i].ToString());
        //        generated.ExampleImage.sprite = PatternSprites[i];
        //    }
        //    else
        //    {
        //        // 텍스트 설정
        //        //generated.SetText
        //        //    (tempName,mainflow.CardColorScore[i - 3].ToString());
        //        generated.ExampleImage.color = PatternColors[i - 3];
        //    }

        //    // 조합 정보 모음에 추가
        //    ScoreCombinationInfoObject.Add(generated);
        //}
    }

    // 전체 덱 보기 패널 켜기/끄기 제어
    public void ControlDeckPanel(bool isActive)
    {
        allDeckPanel.SetActive(isActive);
    }

    public void ControlCombinationPanel(bool isActive)
    {
        combinationPanel.SetActive(isActive);
    }

    public void ControlStagePanel(bool isOpenStore)
    {
        stagePanel.SetActive(!isOpenStore);
        storePanel.SetActive(isOpenStore);
    }

    public void ExitProgram()
    {
        Application.Quit();
    }

    public void ReLoadScene()
    {
        SceneManager.LoadScene("FirstTestScene");
    }
}
