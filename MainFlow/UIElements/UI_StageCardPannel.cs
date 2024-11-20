using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_StageCardPannel : UI_Basic
{
    //=============================================
    // ������
    //=============================================

    [Header("������\n")]
    [Header("�ڵ�ī�� ������")]
    public GameObject CardPrefab;
    [Header("����ī�� ������")]
    public GameObject SelectedCardPrefab;

    //=============================================
    // ����
    //=============================================

    [Header("����")]
    public Sprite[] CardPattern;
    [Header("����")]
    public Color32[] CardColor;

    // ���ھ� üũ ������
    //private float ScoreCheckDelay = 1.2f;

    //=============================================
    // ������Ʈ
    //=============================================

    [Header("�ڵ� ī�� Ȧ��")]
    public Transform handCardHolder;
    [Header("���� ī�� Ȧ��")]
    public Transform selectCardHolder;

    public Transform CardVisualHolder;

    List<CardTransform> AllCard = new List<CardTransform>();

    // �ڵ� �� ���� ī�� ������Ʈ
    List<GameObject> HandCards = new List<GameObject>();
    List<GameObject> HighlightedSelectCards = new List<GameObject>();

    //=============================================

    public override void OnDebug()
    {
        base.OnDebug();
        for(int i = 0; i < CardVisualHolder.childCount; i++)
        {
            CardVisualHolder.GetChild(i).GetComponent<CardVisual>().DebugON();
        }
    }

    public override void OffDebug()
    {
        base.OffDebug();
        for (int i = 0; i < CardVisualHolder.childCount; i++)
        {
            CardVisualHolder.GetChild(i).GetComponent<CardVisual>().DebugOff();
        }
    }

    public override void Initialize()
    {
        CardPattern = playerData.gameData.CardSetting.CardPattern;
        CardColor = playerData.gameData.CardSetting.CardColor;
    }

    // ī�� �÷��� ����
    public void StartCardPlay()
    {
        DisableAllSelectCard();
        PickRandomCard();
    }

    // ī�� �÷��� ���� �ʱ�
    public void PreScoreCheck()
    {
        MakeSelectedCard();
        SetAllCardInteractable(false);
    }

    // ��ü ī�� ��ȣ�ۿ� ���� ��/Ȱ��ȭ
    public void SetAllCardInteractable(bool isInteractable)
    {
        for (int i = 0; i < AllCard.Count; i++) 
        {
            AllCard[i].SetInteractable(isInteractable);
        }
    }

    // �ڵ� ī�� ������Ʈ ����  
    public void MakeHandCard(Card data)
    {
        GameObject temp_object;
        CardTransform temp_card;

        // ī�� ������Ʈ ����
        temp_object = Instantiate(CardPrefab, handCardHolder);

        temp_card = temp_object.transform.GetChild(0).GetComponent<CardTransform>();
        // ī�� �ʱ�ȭ
        temp_card.Initialize
            (CardVisualHolder, 
            () => SelectHandCard(temp_card), 
            () => UnSelectCard(temp_card), data, 
            CardPattern[data.patternIndex], CardColor[data.colorIndex]);

        // ��ü ī�忡 �߰�
        AllCard.Add(temp_card);

        // �ڵ�ī�� ������Ʈ ����Ʈ�� ���� ������Ʈ �߰�
        HandCards.Add(temp_object);

        // �̺�Ʈ ȣ��
        flowBroadcaster.DeckToHand?.Invoke(data);
    }

    // ī�� ���� ����
    public void SelectHandCardForcibly(Card input)
    {
        int index = 0;
        CardTransform temp_card = AllCard[0];

        for (int i = 0; i < AllCard.Count; i++) 
        {
            if (AllCard[i].cardData.Index == input.Index)
            {
                index = i;
                temp_card = AllCard[i];
                break;
            }
        }

        temp_card.selected = !(temp_card.selected);

        // ���� �̹��� Ȱ��ȭ
        temp_card.SetSelect();

        // �ڵ� ī�� ������Ʈ ����Ʈ���� ���� ������Ʈ ����
        HandCards.Remove(temp_card.transform.parent.gameObject);
        // ���� ī�� ������Ʈ ����Ʈ�� ���� ������Ʈ �߰�
        HighlightedSelectCards.Add(temp_card.transform.parent.gameObject);

        // ���� �Ұ��� ����
        AllCard[index].SetInteractable(false);
    }

    // �ڵ�ī�� ����
    public void SelectHandCard(CardTransform selfCard)
    {
        // ���� ���õ� ī�尡 �ִ� ������ �ƴҶ�
        if (playerData.Get_SelectDeckCount() < playerData.Get_PlayableCards())
        {
            // �ڵ� ������ ���� ������ ������ ����
            playerData.HandToSelectDeck(selfCard.cardData);

            selfCard.selected = !(selfCard.selected);

            // ���� �̹��� Ȱ��ȭ
            selfCard.SetSelect();

            // �ڵ� ī�� ������Ʈ ����Ʈ���� ���� ������Ʈ ����
            HandCards.Remove(selfCard.transform.parent.gameObject);
            // ���� ī�� ������Ʈ ����Ʈ�� ���� ������Ʈ �߰�
            HighlightedSelectCards.Add(selfCard.transform.parent.gameObject);
        }
    }

    // ���õ� ī�� öȸ
    public void UnSelectCard(CardTransform selfCard)
    {
        // ���� ������ �ڵ� ������ ������ ����
        playerData.SelectToHandDeck(selfCard.cardData);

        selfCard.selected = !(selfCard.selected);

        // ���� �̹��� ��Ȱ��ȭ
        selfCard.SetSelect();

        // �ڵ� ī�� ������Ʈ ����Ʈ���� ���� ������Ʈ ����
        HighlightedSelectCards.Remove(selfCard.transform.parent.gameObject);
        // ���� ī�� ������Ʈ ����Ʈ�� ���� ������Ʈ �߰�
        HandCards.Add(selfCard.transform.parent.gameObject);
    }

    // ���� ���� ī�� ���� ���� �гο� ����
    public void MakeSelectedCard()
    {
        //playerData.Reset_SelectDeck();

        // �ڵ� ī�� ���� ī�� Ȧ���� �̵�
        for (int i = 0; i < HighlightedSelectCards.Count; i++) 
        {
            HighlightedSelectCards[i].transform.SetParent(selectCardHolder);
            HighlightedSelectCards[i].transform.GetChild(0).GetComponent<CardTransform>().SetCardVisualNormal();
            //SelectedCards.Add(HighlightedSelectCards[i]);
        }
    }

    // ���õ� ī�� ���̶���Ʈ
    public void HighLightSelectedCard(int index)
    {
        //SelectedCards[index].GetComponent<Animator>().SetTrigger("IsOn");
    }

    // ī�� ������
    public void DumpCard(Card input)
    {
        CardTransform tempCard = AllCard[0];

        for (int i = 0; i < AllCard.Count; i++) 
        {
            if(AllCard[i].cardData.Index == input.Index)
            {
                tempCard = AllCard[i];
            }
        }

        AllCard.Remove(tempCard);
        HandCards.Remove(tempCard.transform.parent.gameObject);

        // ������Ʈ ����
        Destroy(tempCard.transform.parent.gameObject);
    }

    // ���� ���� ī�� ������
    public void SelectCardDump()
    {
        //Debug.Log(playerData.Get_CurrentDumpCost());

        if (playerData.Get_SelectDeckCount() == 0)
        {
            msgPannel.PrintMessage("���õ� ī�尡 �����ϴ�!");
            return;
        }

        if (playerData.Get_CurrentDumpCost() > 0)
        {
            SetAllCardInteractable(false);

            // ������Ʈ ����
            for (int i = 0; i < HighlightedSelectCards.Count; i++)
            {
                AllCard.Remove(HighlightedSelectCards[i].transform.GetChild(0).GetComponent<CardTransform>());
                // ������Ʈ ����
                Destroy(HighlightedSelectCards[i]);
            }

            // ������ �ڽ�Ʈ ����
            playerData.Add_CurrentDumpCost(-1);

            // �� �ʱ�ȭ
            HighlightedSelectCards.Clear();

            // ���� �� �ʱ�ȭ
            playerData.Reset_SelectDeck();

            // ī�� �����
            PickRandomCard();

            SetAllCardInteractable(true);

            flowBroadcaster.BroadcasterToMainflow_CardDump();
        }
        else
        {
            msgPannel.PrintMessage("������ �ڽ�Ʈ�� �����մϴ�!");
        }
    }

    // ��� ����ī�� ��Ȱ��ȭ
    public void DisableAllSelectCard()
    {
        // ����ī�� �ڼհ��� �ҷ�����
        int count = selectCardHolder.childCount;

        if (count != 0)
        {
            for (int i = 0; i < count; i++)
            {
                AllCard.Remove(selectCardHolder.GetChild(i).transform.GetChild(0).GetComponent<CardTransform>());
                HighlightedSelectCards.Remove(selectCardHolder.GetChild(i).gameObject);
                Destroy(selectCardHolder.GetChild(i).gameObject);
            }
        }
    }

    // ��� �ڵ�ī�� ��Ȱ��ȭ
    public void DisableAllHandCard()
    {
        //Debug.Log("All Hand Card Disabled");

        // �ڵ�ī�� �ڼհ��� �ҷ�����
        int count = handCardHolder.childCount;

        if (count != 0)
        {
            for (int i = 0; i < count; i++)
            {
                AllCard.Remove(handCardHolder.GetChild(i).transform.GetChild(0).GetComponent<CardTransform>());
                Destroy(handCardHolder.GetChild(i).gameObject);
            }
        }
    }

    // ���� ī�� �� �� ����
    public void PickRandomCard()
    {
        int pickCount;
        int pickIndex;
        Card curCard;

        int handDeckCount = playerData.Get_HandDeckCount();
        int currentDeckCount = playerData.Get_CurrentDeckCount();

        // ���� ī�� ���� ����
        pickCount = playerData.Get_HandSize() - (handDeckCount + playerData.Get_SelectDeckCount());

        // ���� ī�尡 ���� �� ��ü �������� Ŭ ���,
        if (pickCount > currentDeckCount)
        {
            pickCount = currentDeckCount;
        }

        // ī�� �������� �� �� UI���� ����
        for (int i = 0; i < pickCount; i++)
        {
            pickIndex = UnityEngine.Random.Range(0, playerData.Get_CurrentDeckCount());
            curCard = playerData.Get_CurrentDeckByIndex(pickIndex);

            // �ڵ嵦�� ���� ���� ī�� �߰�
            playerData.Add_HandDeck(curCard);

            // ���給���� ���� ���� ī�� ����
            playerData.Remove_CurrentDeck(curCard);

            MakeHandCard(curCard);
        }
    }

    // ���ھ� üũ
    public void ScoreCheck()
    {
        PreScoreCheck();
        StartCoroutine(AllCardScoreCheck());
    }

    IEnumerator AllCardScoreCheck()
    {
        yield return new WaitForSeconds(1);

        for (int i = 0; i < HighlightedSelectCards.Count; i++)
        {
            // ī�� �ִϸ��̼�
            HighlightedSelectCards[i].transform.GetChild(0).GetComponent<CardTransform>().ShakeAnimation();
            //yield return new WaitForSeconds(ScoreCheckDelay);
        }
        // ���� ����
        flowBroadcaster.BroadcasterToMainflow_ApplyScore();

        yield return new WaitForSeconds(2);
        // ī�� �÷��� ����
        flowBroadcaster.BroadcasterToMainflow_CardPlayEnd();
    }
}
