using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine.UI;

enum AllCardBtnInputType
{ 
    Normal,
    Delete,
    UpgradeNumber
}

public class UI_AllCardDeck : UI_Basic
{
    //=============================================
    // ������Ʈ 
    //=============================================

    [Header("��ü ī�� Ȯ�� �г�")]
    [SerializeField]
    private GameObject AllcardPannel;

    [Header("ī�� Ȧ��")]
    [SerializeField]
    private Transform CardHolder;

    public Button btn_DeckPannelOpen;

    public Button btn_DeckPannelClose;

    public TMP_Text txt_Explain;

    //=============================================
    // ������ 
    //=============================================

    [Header("��üī�� ������")]
    [SerializeField]
    private GameObject AllCardPrefab;

    //=============================================
    // ������ 
    //=============================================

    private Dictionary<Card,AllDeckCardScript> allDeckCard = new Dictionary<Card, AllDeckCardScript>();
    private List<AllDeckCardScript> allDeckList = new List<AllDeckCardScript>();
    private List<Button> allDeckButtonList = new List<Button>();

    private AllCardBtnInputType curInputType = AllCardBtnInputType.Normal;

    //=============================================

    public override void Initialize()
    {
        SetButtonEvent();
        SetEvent();
    }

    private void SetEvent()
    {
        flowBroadcaster.InstanceUpgrade11_DeleteDeckCard += InstanceUpgrade11_DeleteDeckCard;
        flowBroadcaster.InstanceUpgrade13_UpgradeCardNumber += InstanceUpgrade13_UpgradeCardNumber;
    }

    private void SetButtonEvent()
    {
        btn_DeckPannelOpen.onClick.AddListener(() => OnOffPannel(true));
        btn_DeckPannelClose.onClick.AddListener(() => OnOffPannel(false));
    }

    // ��ü ī�� ��ư ��ȣ�ۿ� ���� ���� ����
    private void SetInteracableAllBtn(bool isInteracable)
    {
        // null ����
        allDeckButtonList.RemoveAll(x => x == null);

        // ��ü ��ư Ȱ��ȭ ���� ����
        foreach (Button temp_btn in allDeckButtonList)
        {
            temp_btn.interactable = isInteracable;
        }
    }

    // �ν��Ͻ� 11 �̺�Ʈ
    private void InstanceUpgrade11_DeleteDeckCard()
    {
        // �ݱ� ��ư ��Ȱ��ȭ
        btn_DeckPannelClose.gameObject.SetActive(false);
        // ���� ���� ���� ����
        curInputType = AllCardBtnInputType.Delete;
        // ��ü ī�� ��ư ��ȣ�ۿ� �������� ����
        SetInteracableAllBtn(true);

        // ���� �ؽ�Ʈ ���� �� Ȱ��ȭ
        txt_Explain.text = "Ŭ���� �ش� ī�� ����";
        txt_Explain.gameObject.SetActive(true);

        // �г� ON
        OnOffPannel(true);
    }

    // �ν��Ͻ� 13 �̺�Ʈ
    private void InstanceUpgrade13_UpgradeCardNumber()
    {
        // �ݱ� ��ư ��Ȱ��ȭ
        btn_DeckPannelClose.gameObject.SetActive(false);
        // ���� ���� ���� ����
        curInputType = AllCardBtnInputType.UpgradeNumber;
        // ��ü ī�� ��ư ��ȣ�ۿ� �������� ����
        SetInteracableAllBtn(true);

        // ���� �ؽ�Ʈ ���� �� Ȱ��ȭ
        txt_Explain.text = "Ŭ���� �ش� ī�� �ѹ� ���׷��̵�";
        txt_Explain.gameObject.SetActive(true);

        // �г� ON
        OnOffPannel(true);
    }

    // ��ü ī�� ��ư Ŭ����
    private void AllDeckCardButtonOnClicked(AllDeckCardScript inputCard)
    {
        Card card = inputCard.cardData;

        switch (curInputType)
        {
            // ī�� ������
            case AllCardBtnInputType.Delete:
                // ���������� ����
                playerData.Remove_UserDeck(card);
                // ������Ʈ ����
                RemoveCard(card);

                break;

            // ī�� �ѹ� ���׷��̵�
            case AllCardBtnInputType.UpgradeNumber:
                int addAmount;

                // ��Ŀ 39�� �����
                if (playerData.Get_AllPlayerJocker().Find(x => x == playerData.gameData.JockerData[38]))
                    addAmount = 5;
                else
                    addAmount = UnityEngine.Random.Range(1, 6);

                AllDeckCardScript temp_deckScrip = allDeckCard[card];
                allDeckCard.Remove(card);

                card.number += addAmount;
                playerData.Update_UserDeck(card);

                allDeckCard.Add(card, temp_deckScrip);
                temp_deckScrip.SetNumber(card.number);

                break;
        }

        // �г� �ݱ� ��ư Ȱ��ȭ
        btn_DeckPannelClose.gameObject.SetActive(true);
        // ��ü ī�� ��ư ��ȣ�ۿ� ���� ���� ��Ȱ��ȭ
        SetInteracableAllBtn(false);
        // ���� ��ǲ ���� ����
        curInputType = AllCardBtnInputType.Normal;
        // ���� �ؽ�Ʈ ������Ʈ ��Ȱ��ȭ
        txt_Explain.gameObject.SetActive(false);

        // �г� ��Ȱ��ȭ
        OnOffPannel(false);
    }

    public void ClearAllData()
    {
        allDeckCard.Clear();
        allDeckList.Clear();

        for (int i = 0; i < CardHolder.childCount; i++) 
        {
            Destroy(CardHolder.GetChild(i).gameObject);
        }
    }

    // ��ü ī�� ���� ����
    public void GenerateAllCards()
    {
        ResetAllDeckObject();

        int CardCount = playerData.Get_UserDeckCount();

        Card temp_data;
        Button temp_btn;

        for (int i = 0; i < CardCount; i++) 
        {
            // ī�� ������Ʈ ����
            AllDeckCardScript temp_card = Instantiate(AllCardPrefab, CardHolder).transform.GetComponent<AllDeckCardScript>();


            // ī�� ������ �Է�
            temp_data = playerData.Get_UserDeck(i);
            temp_card.cardData = temp_data;

            // ��ư �̺�Ʈ �Ҵ� �� ��ư ��Ȱ��ȭ
            temp_btn = temp_card.transform.GetComponent<Button>();
            temp_btn.onClick.AddListener(() => AllDeckCardButtonOnClicked(temp_card));
            temp_btn.interactable = false;
            allDeckButtonList.Add(temp_btn);

            // ī�� �̹��� ����
            temp_card.SetCardApear
                (
                playerData.gameData.CardSetting.CardPattern[temp_data.patternIndex],
                playerData.gameData.CardSetting.CardColor[temp_data.colorIndex],
                temp_data.number
                );

            // ��ųʸ��� �߰�
            allDeckCard.Add(temp_data, temp_card);
            // ����Ʈ�� �߰�
            allDeckList.Add(temp_card);
        }
    }

    // ī�� Off ( ��Ȱ��ȭ )
    public void DeActiveCardCover(Card input_card)
    {
        allDeckCard[input_card].ControlCover(true);
    }

    // ��ü ī�� Ŀ�� Ȱ��ȭ ����
    public void ActiveAllCardCover(bool isOn)
    {
        for (int i = 0; i < allDeckList.Count; i++)
        {
            allDeckList[i].ControlCover(isOn);
        }
    }

    // ī�� �߰�
    public void AddCard(Card input_card)
    {
        // ī�� ������Ʈ ����
        AllDeckCardScript temp_card = Instantiate(AllCardPrefab, CardHolder).transform.GetComponent<AllDeckCardScript>();

        // ī�� �̹��� ����
        temp_card.SetCardApear
            (
            playerData.gameData.CardSetting.CardPattern[input_card.patternIndex],
            playerData.gameData.CardSetting.CardColor[input_card.colorIndex],
            input_card.number
            );

        // ��ųʸ��� �߰�
        allDeckCard.Add(input_card, temp_card);
        // ����Ʈ�� �߰�
        allDeckList.Add(temp_card);
    }

    // ī�� ����
    public void RemoveCard(Card input_card)
    {
        AllDeckCardScript temp_card = allDeckCard[input_card];

        allDeckList.Remove(temp_card);
        allDeckCard.Remove(input_card);

        Destroy(temp_card.gameObject);
    }

    // ��ü ī�� ������Ʈ ����
    public void ResetAllDeckObject()
    {
        int count = CardHolder.childCount;

        for (int i = 0; i < count; i++) 
        {
            Destroy(CardHolder.GetChild(i).gameObject);
        }
    }

    // �г� On/Off
    public void OnOffPannel(bool isOpen)
    {
        AllcardPannel.SetActive(isOpen);
    }
}
