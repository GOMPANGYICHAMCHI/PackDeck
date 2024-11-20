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

    // 0 : ��    / 1 : �簢��  / 2 : �ﰢ��
    // 0 : ����  / 1 : �ʷ�    / 2 : �Ķ�

    // ==========================
    // �ѹ� �ջ�
    // ==========================
    // ���� ī�� �ѹ� �ջ�
    public int[] patternNumberSum;
    // �÷� ī�� �ѹ� �ջ�
    public int[] colorNumberSum;

    // ==========================
    // �� �ջ�
    // ==========================
    // ���� ī�� �� 
    public int[] patternCardCount;
    // �÷� ī�� �� 
    public int[] colorCardCount;

    // ==========================
    // �߰� ����ġ
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
