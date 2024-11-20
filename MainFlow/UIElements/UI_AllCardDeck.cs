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
    // 오브젝트 
    //=============================================

    [Header("전체 카드 확인 패널")]
    [SerializeField]
    private GameObject AllcardPannel;

    [Header("카드 홀더")]
    [SerializeField]
    private Transform CardHolder;

    public Button btn_DeckPannelOpen;

    public Button btn_DeckPannelClose;

    public TMP_Text txt_Explain;

    //=============================================
    // 프리팹 
    //=============================================

    [Header("전체카드 프리팹")]
    [SerializeField]
    private GameObject AllCardPrefab;

    //=============================================
    // 데이터 
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

    // 전체 카드 버튼 상호작용 가능 여부 변경
    private void SetInteracableAllBtn(bool isInteracable)
    {
        // null 제거
        allDeckButtonList.RemoveAll(x => x == null);

        // 전체 버튼 활성화 여부 변경
        foreach (Button temp_btn in allDeckButtonList)
        {
            temp_btn.interactable = isInteracable;
        }
    }

    // 인스턴스 11 이벤트
    private void InstanceUpgrade11_DeleteDeckCard()
    {
        // 닫기 버튼 비활성화
        btn_DeckPannelClose.gameObject.SetActive(false);
        // 현재 선택 상태 변경
        curInputType = AllCardBtnInputType.Delete;
        // 전체 카드 버튼 상호작용 가능으로 변경
        SetInteracableAllBtn(true);

        // 설명 텍스트 변경 및 활성화
        txt_Explain.text = "클릭시 해당 카드 삭제";
        txt_Explain.gameObject.SetActive(true);

        // 패널 ON
        OnOffPannel(true);
    }

    // 인스턴스 13 이벤트
    private void InstanceUpgrade13_UpgradeCardNumber()
    {
        // 닫기 버튼 비활성화
        btn_DeckPannelClose.gameObject.SetActive(false);
        // 현재 선택 상태 변경
        curInputType = AllCardBtnInputType.UpgradeNumber;
        // 전체 카드 버튼 상호작용 가능으로 변경
        SetInteracableAllBtn(true);

        // 설명 텍스트 변경 및 활성화
        txt_Explain.text = "클릭시 해당 카드 넘버 업그레이드";
        txt_Explain.gameObject.SetActive(true);

        // 패널 ON
        OnOffPannel(true);
    }

    // 전체 카드 버튼 클릭시
    private void AllDeckCardButtonOnClicked(AllDeckCardScript inputCard)
    {
        Card card = inputCard.cardData;

        switch (curInputType)
        {
            // 카드 삭제시
            case AllCardBtnInputType.Delete:
                // 유저덱에서 제거
                playerData.Remove_UserDeck(card);
                // 오브젝트 삭제
                RemoveCard(card);

                break;

            // 카드 넘버 업그레이드
            case AllCardBtnInputType.UpgradeNumber:
                int addAmount;

                // 조커 39번 존재시
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

        // 패널 닫기 버튼 활성화
        btn_DeckPannelClose.gameObject.SetActive(true);
        // 전체 카드 버튼 상호작용 가능 여부 비활성화
        SetInteracableAllBtn(false);
        // 현재 인풋 상태 변경
        curInputType = AllCardBtnInputType.Normal;
        // 설명 텍스트 오브젝트 비활성화
        txt_Explain.gameObject.SetActive(false);

        // 패널 비활성화
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

    // 전체 카드 에셋 생성
    public void GenerateAllCards()
    {
        ResetAllDeckObject();

        int CardCount = playerData.Get_UserDeckCount();

        Card temp_data;
        Button temp_btn;

        for (int i = 0; i < CardCount; i++) 
        {
            // 카드 오브젝트 생성
            AllDeckCardScript temp_card = Instantiate(AllCardPrefab, CardHolder).transform.GetComponent<AllDeckCardScript>();


            // 카드 데이터 입력
            temp_data = playerData.Get_UserDeck(i);
            temp_card.cardData = temp_data;

            // 버튼 이벤트 할당 및 버튼 비활성화
            temp_btn = temp_card.transform.GetComponent<Button>();
            temp_btn.onClick.AddListener(() => AllDeckCardButtonOnClicked(temp_card));
            temp_btn.interactable = false;
            allDeckButtonList.Add(temp_btn);

            // 카드 이미지 설정
            temp_card.SetCardApear
                (
                playerData.gameData.CardSetting.CardPattern[temp_data.patternIndex],
                playerData.gameData.CardSetting.CardColor[temp_data.colorIndex],
                temp_data.number
                );

            // 딕셔너리에 추가
            allDeckCard.Add(temp_data, temp_card);
            // 리스트에 추가
            allDeckList.Add(temp_card);
        }
    }

    // 카드 Off ( 비활성화 )
    public void DeActiveCardCover(Card input_card)
    {
        allDeckCard[input_card].ControlCover(true);
    }

    // 전체 카드 커버 활성화 여부
    public void ActiveAllCardCover(bool isOn)
    {
        for (int i = 0; i < allDeckList.Count; i++)
        {
            allDeckList[i].ControlCover(isOn);
        }
    }

    // 카드 추가
    public void AddCard(Card input_card)
    {
        // 카드 오브젝트 생성
        AllDeckCardScript temp_card = Instantiate(AllCardPrefab, CardHolder).transform.GetComponent<AllDeckCardScript>();

        // 카드 이미지 설정
        temp_card.SetCardApear
            (
            playerData.gameData.CardSetting.CardPattern[input_card.patternIndex],
            playerData.gameData.CardSetting.CardColor[input_card.colorIndex],
            input_card.number
            );

        // 딕셔너리에 추가
        allDeckCard.Add(input_card, temp_card);
        // 리스트에 추가
        allDeckList.Add(temp_card);
    }

    // 카드 제거
    public void RemoveCard(Card input_card)
    {
        AllDeckCardScript temp_card = allDeckCard[input_card];

        allDeckList.Remove(temp_card);
        allDeckCard.Remove(input_card);

        Destroy(temp_card.gameObject);
    }

    // 전체 카드 오브젝트 삭제
    public void ResetAllDeckObject()
    {
        int count = CardHolder.childCount;

        for (int i = 0; i < count; i++) 
        {
            Destroy(CardHolder.GetChild(i).gameObject);
        }
    }

    // 패널 On/Off
    public void OnOffPannel(bool isOpen)
    {
        AllcardPannel.SetActive(isOpen);
    }
}
