using System;

[Serializable]
public struct Card
{
    // 0 : ��    / 1 : �簢��  / 2 : �ﰢ��
    // 0 : ����  / 1 : �ʷ�    / 2 : �Ķ�

    public int Index;

    // ���� �ε���
    public int patternIndex;
    // �÷� �ε��� 
    public int colorIndex;
    // ī�� ����
    public int number;

    public void CopyData(Card card)
    {
        card.Index = Index;
        card.patternIndex = patternIndex;
        card.colorIndex = colorIndex;
        card.number = number;
    }
}