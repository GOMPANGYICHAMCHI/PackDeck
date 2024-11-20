using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

// 골드 보상 데이터
public struct GoldRewardData
{
    // 클리어 보상
    public int roundClearReward;
    // 오버딜 배수
    public float overDealMultiply;
    // 오버딜 보너스
    public int overDealBonus;
    // 이자
    public int interestBonus;
}

[Serializable]
// 카드 생성 규칙
public struct CardGenerationSetting
{
    [Header("패턴")]
    public Sprite[] CardPattern;
    [Header("색상 갯수")]
    public Color32[] CardColor;
    [Header("카드 숫자(1 ~ n)")]
    public int CardNumber;

    [Header("패턴 명칭")]
    public string[] CardPatternName;
    [Header("색상 명칭")]
    public string[] CardColorName;
}

[Serializable]
// 점수 가중치 정보
public struct ScoreAdditional
{
    [HideInInspector]
    public int patternCount;
    [HideInInspector]
    public int colorCount;

    [Header("패턴 넘버 합산 점수 합 가중치")]
    public float[] Pattern_A;
    [Header("패턴 수 합산 점수 합 가중치")]
    public float[] Pattern_B;

    [Header("패턴 점수 곱 가중치")]
    public float[] Pattern_Multiply;

    // ==========================================

    [Header("색상 넘버 합산 점수 합 가중치")]
    public float[] Color_A;
    [Header("색상 수 합산 점수 합 가중치")]
    public float[] Color_B;

    [Header("색상 점수 곱 가중치")]
    public float[] Color_Multiply;

    // ==========================================

    public void InputData(ScoreAdditional input)
    {
        Pattern_A = (float[])input.Pattern_A.Clone();
        Pattern_B = (float[])input.Pattern_B.Clone();
        Pattern_Multiply = (float[])input.Pattern_Multiply.Clone();

        Color_A = (float[])input.Color_A.Clone();
        Color_B = (float[])input.Color_B.Clone();
        Color_Multiply = (float[])input.Color_Multiply.Clone();
    }

    //public static ScoreAdditional operator + (ScoreAdditional a, ScoreAdditional b)
    //{
    //    if(a.Pattern_A == null)
    //    {
    //        return b;
    //    }
    //    else if(b.Pattern_A == null)
    //    {
    //        return a;
    //    }
    //
    //    for (int i = 0; i < a.Pattern_A.Length; i++) 
    //    {
    //        a.Pattern_A[i] += b.Pattern_A[i];
    //        a.Pattern_B[i] += b.Pattern_B[i];
    //        a.Pattern_Multiply[i] += b.Pattern_Multiply[i];
    //    }
    //    for (int i = 0; i < a.Color_A.Length; i++)
    //    {
    //        a.Color_A[i] += b.Color_A[i];
    //        a.Color_B[i] += b.Color_B[i];
    //        a.Color_Multiply[i] += b.Color_Multiply[i];
    //    }
    //
    //    a.AfterAllMultiply += b.AfterAllMultiply;
    //
    //    return a;
    //}

    public ScoreAdditional(int PatternCount, int ColorCount, bool isForHanover = false)
    {
        Pattern_A = new float[PatternCount];
        Pattern_B = new float[PatternCount];
        Pattern_Multiply = new float[PatternCount];

        Color_A = new float[ColorCount];
        Color_B = new float[ColorCount];
        Color_Multiply = new float[ColorCount];

        for (int i = 0; i < PatternCount; i++) 
        {
            Pattern_A[i] = 0;
            Pattern_B[i] = 0;
            Pattern_Multiply[i] = (isForHanover)?  0 : 1;
        }

        for (int i = 0; i < ColorCount; i++)
        {
            Color_A[i] = 0;
            Color_B[i] = 0;
            Color_Multiply[i] = (isForHanover) ? 0 : 1;
        }

        patternCount = PatternCount;
        colorCount = ColorCount;
    }

    public void ResetData()
    {
        for (int i = 0; i < Pattern_A.Length; i++)
        {
            Pattern_A[i] = 0;
            Pattern_B[i] = 0;
            Pattern_Multiply[i] = 1;
        }

        for (int i = 0; i < Color_A.Length; i++)
        {
            Color_A[i] = 0;
            Color_B[i] = 0;
            Color_Multiply[i] = 1;
        }
    }
}

[Serializable]
// 조커 정보
public struct JockerInfo
{
    [Header("조커 명칭")]
    public string Name;

    [Header("등장확률 ( 0.7 -> 7 ) 전체 확률이 같을때는 1로 기입")]
    public int Percent;

    [Header("구매 가격")]
    public int PurchaseCost;

    [Header("처분 가격")]
    public int SellCost;

    [Header("조커 색상")]
    public Color32 JockerColor;

    [Header("조커 설명")]
    public string ExplainText;
}

[Serializable]
// 디버프 데이터
public struct DeBuffData
{
    [Header("디버프 명칭")]
    public string Name;

    [Header("디버프 설명")]
    public string Description;

    [Header("등장확률 % (전체가 동일하다면 1로 기입)")]
    public int Percent;
}

[Serializable]
// 스토어 데이터
public struct StoreData
{
    //[Header("카드 추가 등장 확률")]
    //public ushort percent_AdditionalCardAppear;
    //[Header("조커 등장 확률")]
    //public ushort percent_AdditionbalJockerAppear;
    //[Header("족보 강화 등장 확률")]
    //public ushort percent_UpgradeScoreAppear;
    //[Header("타로/유령 이벤트 등장 확률")]
    //public ushort percent_EventAppear;
    //
    //[Header("카드 추가 가격")]
    //public ushort cost_AdditionalCard;
    //[Header("조커 추가 가격")]
    //public ushort cost_AdditionbalJocker;
    //[Header("족보 강화 가격")]
    //public ushort cost_UpgradeScore;
    //[Header("타로/유령 이벤트 가격")]
    //public ushort cost_Event;
    //
    //[Header("패턴 상점 등장 확률")]
    //public int[] CardPatternPercent;
    //[Header("색상 상점 등장 확률")]
    //public int[] CardColorPercent;

    [Header("리롤 코스트")]
    public int ReRollCost;

    [Header("리롤 코스트 증가 정도 (리롤마다)")]
    public int ReRollCostAddAmountPerReRoll;
}

[Serializable]
// 스테이지 보상 데이터
public struct StageRewardData
{
    [Header("스테이지 당 골드 추가 정도")]
    public int AddGoldamountPerStage;

    [Header("오버딜 기본 배율")]
    public float OverDealBonusMultiply;

    [Header("이자 보상 지급 골드수 ( 해당 수치 당 1 골드 )")]
    public int InterestDivideGoldCount;

    [Header("이자 보상 한도")]
    public int InterestLimit;
}

[Serializable]
//  게임 데이터
public struct GameData
{
    [Header("시작 몬스터 체력")]
    public int StartMonsterHealth;

    [Header("스테이지 마다 목표 점수 증가 정도")]
    public int AddGoalScorePerStage;

    [Header("스테이지 마다 목표 점수 곱셈 정도")]
    public float MultiplyGoalScorePerStage;

    //[Header("바우처 초기화 스테이지 수")]
    //public int VoucherResetStage;

    [Header("시작 행동 점수")]
    public int ActionCost;

    [Header("시작 버리기 점수")]
    public int DumpCost;

    [Header("랜덤 픽 카드 수( 핸드 사이즈 )")]
    public int HandSize;

    [Header("선택가능 카드 수")]
    public int SelectableCardAmount;

    //[Header("상점 도달 스테이지 수")]
    //public int StorePerStage;

    [Header("점수 가중치 데이터")]
    public ScoreAdditional ScoreAdditionalData;

    [Header("스테이지 보상 데이터")]
    public StageRewardData StageRewardData;

    [Header("카드 생성 설정")]
    public CardGenerationSetting CardSetting;

    [Header("조커 데이터 ( 인덱스 순서로 기입 )")]
    public JockerBase[] JockerData;

    [Header("디버프 데이터 ( 인덱스 순서로 기입 )")]
    public DeBuffBase[] DeBuffData;

    [Header("인스턴스 성장 업그레이드 데이터 ( 인덱스 순서로 기입 )")]
    public InstanceUpgradeBase[] InstanceUpgradeData;

    [Header("스토어 관련 데이터")]
    public StoreData StoreRule;
}
