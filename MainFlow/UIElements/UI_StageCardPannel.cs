using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_StageCardPannel : UI_Basic
{
    //=============================================
    // 프리팹
    //=============================================

    [Header("프리팹\n")]
    [Header("핸드카드 프리팹")]
    public GameObject CardPrefab;
    [Header("선택카드 프리팹")]
    public GameObject SelectedCardPrefab;

    //=============================================
    // 변수
    //=============================================

    [Header("패턴")]
    public Sprite[] CardPattern;
    [Header("색상")]
    public Color32[] CardColor;

    // 스코어 체크 딜레이
    //private float ScoreCheckDelay = 1.2f;

    //=============================================
    // 오브젝트
    //=============================================

    [Header("핸드 카드 홀더")]
    public Transform handCardHolder;
    [Header("선택 카드 홀더")]
    public Transform selectCardHolder;

    public Transform CardVisualHolder;

    List<CardTransform> AllCard = new List<CardTransform>();

    // 핸드 및 선택 카드 오브젝트
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

    // 카드 플레이 시작
    public void StartCardPlay()
    {
        DisableAllSelectCard();
        PickRandomCard();
    }

    // 카드 플레이 종료 초기
    public void PreScoreCheck()
    {
        MakeSelectedCard();
        SetAllCardInteractable(false);
    }

    // 전체 카드 상호작용 여부 비/활성화
    public void SetAllCardInteractable(bool isInteractable)
    {
        for (int i = 0; i < AllCard.Count; i++) 
        {
            AllCard[i].SetInteractable(isInteractable);
        }
    }

    // 핸드 카드 오브젝트 생성  
    public void MakeHandCard(Card data)
    {
        GameObject temp_object;
        CardTransform temp_card;

        // 카드 오브젝트 생성
        temp_object = Instantiate(CardPrefab, handCardHolder);

        temp_card = temp_object.transform.GetChild(0).GetComponent<CardTransform>();
        // 카드 초기화
        temp_card.Initialize
            (CardVisualHolder, 
            () => SelectHandCard(temp_card), 
            () => UnSelectCard(temp_card), data, 
            CardPattern[data.patternIndex], CardColor[data.colorIndex]);

        // 전체 카드에 추가
        AllCard.Add(temp_card);

        // 핸드카드 오브젝트 리스트에 현재 오브젝트 추가
        HandCards.Add(temp_object);

        // 이벤트 호출
        flowBroadcaster.DeckToHand?.Invoke(data);
    }

    // 카드 강제 선택
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

        // 선택 이미지 활성화
        temp_card.SetSelect();

        // 핸드 카드 오브젝트 리스트에서 현재 오브젝트 제거
        HandCards.Remove(temp_card.transform.parent.gameObject);
        // 선택 카드 오브젝트 리스트에 현재 오브젝트 추가
        HighlightedSelectCards.Add(temp_card.transform.parent.gameObject);

        // 선택 불가로 고정
        AllCard[index].SetInteractable(false);
    }

    // 핸드카드 선택
    public void SelectHandCard(CardTransform selfCard)
    {
        // 현재 선택된 카드가 최대 갯수가 아닐때
        if (playerData.Get_SelectDeckCount() < playerData.Get_PlayableCards())
        {
            // 핸드 덱에서 선택 덱으로 데이터 이전
            playerData.HandToSelectDeck(selfCard.cardData);

            selfCard.selected = !(selfCard.selected);

            // 선택 이미지 활성화
            selfCard.SetSelect();

            // 핸드 카드 오브젝트 리스트에서 현재 오브젝트 제거
            HandCards.Remove(selfCard.transform.parent.gameObject);
            // 선택 카드 오브젝트 리스트에 현재 오브젝트 추가
            HighlightedSelectCards.Add(selfCard.transform.parent.gameObject);
        }
    }

    // 선택된 카드 철회
    public void UnSelectCard(CardTransform selfCard)
    {
        // 선택 덱에서 핸드 덱으로 데이터 이전
        playerData.SelectToHandDeck(selfCard.cardData);

        selfCard.selected = !(selfCard.selected);

        // 선택 이미지 비활성화
        selfCard.SetSelect();

        // 핸드 카드 오브젝트 리스트에서 현재 오브젝트 제거
        HighlightedSelectCards.Remove(selfCard.transform.parent.gameObject);
        // 선택 카드 오브젝트 리스트에 현재 오브젝트 추가
        HandCards.Add(selfCard.transform.parent.gameObject);
    }

    // 현재 선택 카드 점수 조합 패널에 생성
    public void MakeSelectedCard()
    {
        //playerData.Reset_SelectDeck();

        // 핸드 카드 선택 카드 홀더로 이동
        for (int i = 0; i < HighlightedSelectCards.Count; i++) 
        {
            HighlightedSelectCards[i].transform.SetParent(selectCardHolder);
            HighlightedSelectCards[i].transform.GetChild(0).GetComponent<CardTransform>().SetCardVisualNormal();
            //SelectedCards.Add(HighlightedSelectCards[i]);
        }
    }

    // 선택된 카드 하이라이트
    public void HighLightSelectedCard(int index)
    {
        //SelectedCards[index].GetComponent<Animator>().SetTrigger("IsOn");
    }

    // 카드 버리기
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

        // 오브젝트 제거
        Destroy(tempCard.transform.parent.gameObject);
    }

    // 현재 선택 카드 버리기
    public void SelectCardDump()
    {
        //Debug.Log(playerData.Get_CurrentDumpCost());

        if (playerData.Get_SelectDeckCount() == 0)
        {
            msgPannel.PrintMessage("선택된 카드가 없습니다!");
            return;
        }

        if (playerData.Get_CurrentDumpCost() > 0)
        {
            SetAllCardInteractable(false);

            // 오브젝트 삭제
            for (int i = 0; i < HighlightedSelectCards.Count; i++)
            {
                AllCard.Remove(HighlightedSelectCards[i].transform.GetChild(0).GetComponent<CardTransform>());
                // 오브젝트 제거
                Destroy(HighlightedSelectCards[i]);
            }

            // 버리기 코스트 감소
            playerData.Add_CurrentDumpCost(-1);

            // 덱 초기화
            HighlightedSelectCards.Clear();

            // 선택 덱 초기화
            playerData.Reset_SelectDeck();

            // 카드 재생성
            PickRandomCard();

            SetAllCardInteractable(true);

            flowBroadcaster.BroadcasterToMainflow_CardDump();
        }
        else
        {
            msgPannel.PrintMessage("버리기 코스트가 부족합니다!");
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
                AllCard.Remove(selectCardHolder.GetChild(i).transform.GetChild(0).GetComponent<CardTransform>());
                HighlightedSelectCards.Remove(selectCardHolder.GetChild(i).gameObject);
                Destroy(selectCardHolder.GetChild(i).gameObject);
            }
        }
    }

    // 모든 핸드카드 비활성화
    public void DisableAllHandCard()
    {
        //Debug.Log("All Hand Card Disabled");

        // 핸드카드 자손갯수 불러오기
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

    // 랜덤 카드 픽 및 생성
    public void PickRandomCard()
    {
        int pickCount;
        int pickIndex;
        Card curCard;

        int handDeckCount = playerData.Get_HandDeckCount();
        int currentDeckCount = playerData.Get_CurrentDeckCount();

        // 뽑을 카드 갯수 선정
        pickCount = playerData.Get_HandSize() - (handDeckCount + playerData.Get_SelectDeckCount());

        // 뽑을 카드가 현재 덱 전체 갯수보다 클 경우,
        if (pickCount > currentDeckCount)
        {
            pickCount = currentDeckCount;
        }

        // 카드 랜덤으로 픽 및 UI에셋 생성
        for (int i = 0; i < pickCount; i++)
        {
            pickIndex = UnityEngine.Random.Range(0, playerData.Get_CurrentDeckCount());
            curCard = playerData.Get_CurrentDeckByIndex(pickIndex);

            // 핸드덱에 현재 생성 카드 추가
            playerData.Add_HandDeck(curCard);

            // 현재덱에서 현재 생성 카드 제외
            playerData.Remove_CurrentDeck(curCard);

            MakeHandCard(curCard);
        }
    }

    // 스코어 체크
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
            // 카드 애니메이션
            HighlightedSelectCards[i].transform.GetChild(0).GetComponent<CardTransform>().ShakeAnimation();
            //yield return new WaitForSeconds(ScoreCheckDelay);
        }
        // 점수 적용
        flowBroadcaster.BroadcasterToMainflow_ApplyScore();

        yield return new WaitForSeconds(2);
        // 카드 플레이 종료
        flowBroadcaster.BroadcasterToMainflow_CardPlayEnd();
    }
}
