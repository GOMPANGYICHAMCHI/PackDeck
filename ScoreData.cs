using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface testfield
{
    string name { get; set; }
}

[Serializable]
public struct ScoreData 
{
    public int patternCount;
    public int colorCount;

    // 0 : 원    / 1 : 사각형  / 2 : 삼각형
    // 0 : 빨강  / 1 : 초록    / 2 : 파랑

    // ==========================
    // 넘버 합산
    // ==========================
    // 패턴 카드 넘버 합산
    public int[] patternNumberSum;
    // 컬러 카드 넘버 합산
    public int[] colorNumberSum;

    // ==========================
    // 수 합산
    // ==========================
    // 패턴 카드 수 
    public int[] patternCardCount;
    // 컬러 카드 수 
    public int[] colorCardCount;

    // ==========================
    // 추가 가중치
    // ==========================

    public ScoreAdditional Additional;

    // ==========================

    public void ResetData()
    {
        for (int i = 0; i < patternNumberSum.Length; i++) 
        {
            patternNumberSum[i] = 0;
            patternCardCount[i] = 0;
        }
        
        for (int i = 0; i < colorNumberSum.Length; i++) 
        {
            colorNumberSum[i] = 0;
            colorCardCount[i] = 0;
        }

        Additional.ResetData();
    }

    public ScoreData(int PatternCount,int ColorCount)
    {
        patternCardCount = new int[PatternCount];
        colorCardCount = new int[ColorCount];

        patternNumberSum = new int[PatternCount];
        colorNumberSum = new int[ColorCount];

        Additional = new ScoreAdditional(PatternCount,ColorCount);

        patternCount = PatternCount;
        colorCount = ColorCount;
    }
}
