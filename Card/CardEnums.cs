using System;

[Serializable]
public struct Card
{
    // 0 : 원    / 1 : 사각형  / 2 : 삼각형
    // 0 : 빨강  / 1 : 초록    / 2 : 파랑

    public int Index;

    // 패턴 인덱스
    public int patternIndex;
    // 컬러 인덱스 
    public int colorIndex;
    // 카드 숫자
    public int number;

    public void CopyData(Card card)
    {
        card.Index = Index;
        card.patternIndex = patternIndex;
        card.colorIndex = colorIndex;
        card.number = number;
    }
}