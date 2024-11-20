using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StorePannel : UI_Basic
{
    //=============================================
    // 오브젝트
    //=============================================

    [Header("스토어 전체 패널")]
    public GameObject StorePannel;

    [Header("랜덤 카드 오브젝트")]
    public Button[] btn_RandomCardSelect;
    public Image[] img_RandomCardPattern;
    public TMP_Text[] txt_RandomCardNumber;

    [Header("리롤 버튼")]
    public Button btn_Reroll;
    [Header("리롤 텍스트")]
    public TMP_Text txt_ReRoll;

    [Header("카드 선택 패널")]
    public GameObject CardSelectPannel;

    [Header("아이템 구매 패널")]
    public GameObject ItemBuyPannel;

    [Header("카드 선택 스킵 버튼")]
    public Button btn_SkipCardSelect;

    [Header("스토어 나가기 버튼")]
    public Button btn_ExitStore;

    [Header("업그레이드 항목 버튼")]
    public Button[] btn_UpgradeSlot0;
    public Button btn_UpgradeSlot1;
    public Button btn_UpgradeSlot2;

    [Header("업그레이드 항목 버튼 텍스트")]
    public TMP_Text[] txt_UpgradeSlot0 = new TMP_Text[2];
    public TMP_Text txt_UpgradeSlot1;
    public TMP_Text txt_UpgradeSlot2;

    //=============================================
    // 코스트 및 변수
    //=============================================

    // 현재 리롤 코스트
    private int CurrentReRollCost;

    // 선택된 인스턴스 인덱스
    int[] selectedType_slot0 = new int[2];
    int[] selectedIndex_slot0 = new int[2];
    int selectedIndex_slot1;
    int selectedIndex_slot2;

    // 스토어 룰
    private StoreData storeRule;

    bool isAddtionalRandomCard = false;

    //=============================================
    // 랜덤 선정 카드
    //=============================================

    private Card[] RandomCards = new Card[3];

    //=============================================

    public override void Initialize()
    {
        base.Initialize();

        // 랜덤 카드 데이터 초기화
        InitializeRandomCard();

        // 스토어 룰 로드
        LoadStoreRule();
        // 버튼 이벤트 지정
        SetBtnEvent();

        // 이벤트 지정
        SetEvent();
    }

    private void SetEvent()
    {
        //flowBroadcaster.InstanceUpgrade11_DeleteDeckCard += InstanceUpgrade11_DeleteDeckCard;
        flowBroadcaster.InstanceUpgrade12_AddDeckCard += InstanceUpgrade12_AddDeckCard;
        //flowBroadcaster.InstanceUpgrade13_UpgradeCardNumber += InstanceUpgrade13_UpgradeCardNumber;
    }

    // 인스턴스 12 
    private void InstanceUpgrade12_AddDeckCard()
    {
        isAddtionalRandomCard = true;

        // 카드 선택 창 활성화 및 아이템 구매 창 비활성화
        CardSelectPannel.SetActive(true);
        ItemBuyPannel.SetActive(false);

        // 랜덤 지급 카드 선정
        RandomCardPick();
    }

    // 스토어 최초 진입 ( 랜덤 카드 선택 창 )
    public void EnterStore()
    {
        // 카드 추가 구매 여부 비활성화
        isAddtionalRandomCard = false;

        // 스토어 패널 활성화
        StorePannel.SetActive(true);

        // 카드 선택 창 활성화 및 아이템 구매 창 비활성화
        CardSelectPannel.SetActive(true);
        ItemBuyPannel.SetActive(false);

        // 랜덤 지급 카드 선정
        RandomCardPick();
    }

    // 스토어 나가기 
    private void ExitStore()
    {
        StorePannel.SetActive(false);

        // 플로우 브로드캐스터에게 전달
        flowBroadcaster.BroadcasterToMainflow_StoreExit();
    }

    // 카드 선택 이후 아이템 구매
    public void CardSelectToItemBuy()
    {
        // 카드 선택 패널 비활성화
        CardSelectPannel.SetActive(false);
        // 아이템 구매 패널 활성화
        ItemBuyPannel.SetActive(true);

        if(!isAddtionalRandomCard)
        {
            // 리롤 코스트 초기화
            ResetReRollCost();

            // 업그레이드 패널 초기화
            ResetUpgradeSlot0();
            ResetUpgradeSlotRight();
        }

    }

    // 버튼 이벤트 설정
    private void SetBtnEvent()
    {
        // 랜덤 지급 카드이벤트
        btn_RandomCardSelect[0].onClick.AddListener(() => SelectRandomCard(0));
        btn_RandomCardSelect[1].onClick.AddListener(() => SelectRandomCard(1));
        btn_RandomCardSelect[2].onClick.AddListener(() => SelectRandomCard(2));

        // 카드 선택 스킵 버튼
        btn_SkipCardSelect.onClick.AddListener(CardSelectToItemBuy);

        // 리롤 버튼
        btn_Reroll.onClick.AddListener(ReRoll);

        // 업그레이드 버튼
        btn_UpgradeSlot0[0].onClick.AddListener(() => UpgradSlot0(0));
        btn_UpgradeSlot0[1].onClick.AddListener(() => UpgradSlot0(1));

        btn_UpgradeSlot1.onClick.AddListener(UpgradeSlot1);
        btn_UpgradeSlot2.onClick.AddListener(UpgradeSlot2);

        // 상점 나가기 버튼
        btn_ExitStore.onClick.AddListener(ExitStore);
    }

    // 카드 선택시
    private void SelectRandomCard(int index)
    {
        RandomCards[index].Index = playerData.LastCardIndex;
        playerData.LastCardIndex++;

        // 현재 덱에 추가
        playerData.Add_CurrentDeck(RandomCards[index]);

        // 아이템 선택창으로 넘어가기
        CardSelectToItemBuy();
    }

    // 스토어 룰 로드
    private void LoadStoreRule()
    {
        storeRule = playerData.gameData.StoreRule;
    }

    // 랜덤 카드 데이터 초기화
    private void InitializeRandomCard()
    {
        for (int i = 0; i < 3; i++)
        {
            RandomCards[i] = new Card();
        }
    }

    // 선택 가능 랜덤 카드 선정 및 외형 설정
    private void RandomCardPick()
    {
        // 전체 카드 인덱스 초기화
        List<int> nums = new List<int>();
        int CardCount = playerData.Get_OriginalDeckCount();

        for (int i = 0; i < CardCount; i++)
        {
            nums.Add(i);
        }

        int tempIndex;
        for (int i = 0; i < 3; i++)
        {
            // 카드 산출
            tempIndex = nums[UnityEngine.Random.Range(0, nums.Count)];
            nums.Remove(tempIndex);

            // 카드 데이터 복사
            RandomCards[i] = playerData.Get_OriginalDeck(tempIndex);

            // 카드 이미지 생성
            img_RandomCardPattern[i].sprite = playerData.gameData.CardSetting.CardPattern[RandomCards[i].patternIndex];
            img_RandomCardPattern[i].color = playerData.gameData.CardSetting.CardColor[RandomCards[i].colorIndex];
            txt_RandomCardNumber[i].text = RandomCards[i].number.ToString();
        }
    }

    // 리롤 코스트 초기화
    private void ResetReRollCost()
    {
        CurrentReRollCost = storeRule.ReRollCost;
        UpdateReRollText();
    }

    // 리롤 코스트 텍스트 업데이트
    public void UpdateReRollText()
    {
        txt_ReRoll.text = "리롤 " + CurrentReRollCost.ToString() + "골드"; 
    }

    // 리롤 구역 업그레이드 랜덤 선정
    public void ResetUpgradeSlot0()
    {
        string temp_text;
        int temp_rand;
        int temp_sum = 0;

        int temp_instancepercentSum = 0;
        List<int> availableInstanceIndex = new List<int>();

        int temp_jockerpercentSum = 0;
        List<int> availableJockerIndex = new List<int>();

        // 슬롯0 업그레이드 요소 전체순회 및 등장조건 확인
        for (int i = 0; i < playerData.InstanceUpgradeCount_slot0; i++)
        {
            if(playerData.InstanceUpgradeSlot0[i].Event_CheckUpgradeAvailableCondition(playerData))
            {
                //Debug.Log(i);
                availableInstanceIndex.Add(i);
                temp_instancepercentSum += playerData.InstanceUpgradeSlot0[i].AppearancePercent;
            }
        }

        // 슬롯0 유물 정보 전체순회 및 등장조건 확인
        for (int i = 0; i < playerData.JockerCount; i++)
        {
            if (playerData.Get_AllPlayerJocker().Find(x => x == playerData.gameData.JockerData[i]) == null)
            {
                availableJockerIndex.Add(i);
                temp_jockerpercentSum += playerData.gameData.JockerData[i].Info.Percent;
            }
        }

        bool isNotRerollable = false;
        int temp_curePick = 0; 

        for (int i = 0; i < 2; i++) 
        {
            temp_curePick = 0;

            if (availableInstanceIndex.Count == 0 && availableJockerIndex.Count == 0)
            {
                // 버튼 텍스트 설정
                txt_UpgradeSlot0[i].text = "가능한 업그레이드가 없습니다";
                btn_UpgradeSlot0[i].interactable = false;

                // 리롤 버튼 비활성화
                btn_Reroll.interactable = false;
                isNotRerollable = true;

                temp_curePick = -1;
            }

            // 인스턴스가 없을때
            else if(availableInstanceIndex.Count == 0)
                temp_curePick = 0;
            // 조커가 없을때
            else if(availableJockerIndex.Count == 0)
                temp_curePick = 1;
            else
                temp_curePick = UnityEngine.Random.Range(0, 2);

            // 조커
            if(temp_curePick == 0) 
            {
                temp_sum = 0;

                // 산출
                temp_rand = UnityEngine.Random.Range(0, temp_jockerpercentSum);

                // 슬롯0 업그레이드 요소 전체 확률 순회
                for (int j = 0; j < availableJockerIndex.Count; j++)
                {
                    temp_sum += playerData.gameData.JockerData[availableJockerIndex[j]].Info.Percent;

                    if (temp_rand <= temp_sum)
                    {
                        selectedIndex_slot0[i] = availableJockerIndex[j];
                        availableJockerIndex.Remove(availableJockerIndex[j]);
                        selectedType_slot0[i] = 0;
                        break;
                    }
                }

                // 버튼 텍스트 설정
                temp_text = playerData.gameData.JockerData[selectedIndex_slot0[i]].Info.Name + " " +
                    playerData.gameData.JockerData[selectedIndex_slot0[i]].Info.PurchaseCost.ToString() + " 골드";
                txt_UpgradeSlot0[i].text = temp_text;

                // 버튼 활성화
                btn_UpgradeSlot0[i].interactable = true;
            }
            // 인스턴스
            else if(temp_curePick == 1) 
            {
                temp_sum = 0;

                // 산출
                temp_rand = UnityEngine.Random.Range(0, temp_instancepercentSum);

                // 슬롯0 업그레이드 요소 전체 확률 순회
                for (int j = 0; j < availableInstanceIndex.Count; j++)
                {
                    temp_sum += playerData.Percent_InstanceUpgradeSlot0[availableInstanceIndex[j]];

                    if (temp_rand <= temp_sum)
                    {
                        selectedIndex_slot0[i] = availableInstanceIndex[j];
                        availableInstanceIndex.Remove(availableInstanceIndex[j]);
                        selectedType_slot0[i] = 1;
                        break;
                    }

                    else if (j == availableInstanceIndex.Count - 1) 
                    {
                        selectedIndex_slot0[i] = availableInstanceIndex[j];
                        availableInstanceIndex.Remove(availableInstanceIndex[j]);
                        selectedType_slot0[i] = 1;
                        break;
                    }
                }

                if(selectedIndex_slot0[i] < 0 || playerData.InstanceUpgradeSlot0.Count() <= selectedIndex_slot0[i])
                {
                    //Debug.Log("가능한 요소 수" + availableInstanceIndex.Count());
                    Debug.Log("뽑힌 인덱스" + selectedIndex_slot0[i]);
                    Debug.Log("전체 업그레이드0 수" + playerData.InstanceUpgradeSlot0.Count());
                }

                // 초기화 함수 호출
                playerData.InstanceUpgradeSlot0[selectedIndex_slot0[i]].Event_OnInstantiatedInStore(playerData);

                // 버튼 텍스트 설정
                temp_text = playerData.InstanceUpgradeSlot0[selectedIndex_slot0[i]].Name + " " +
                    playerData.InstanceUpgradeSlot0[selectedIndex_slot0[i]].Cost.ToString() + " 골드";
                txt_UpgradeSlot0[i].text = temp_text;

                // 버튼 활성화
                btn_UpgradeSlot0[i].interactable = true;
            }
        }

        // 리롤이 가능할때 리롤 버튼 활성화
        if (!isNotRerollable && (availableInstanceIndex.Count != 0 || availableJockerIndex.Count != 0))
            btn_Reroll.interactable = true;
        else
            btn_Reroll.interactable = false;
    }

    // 1,2 구역 업그레이드 랜덤 선정
    public void ResetUpgradeSlotRight()
    {
        string temp_text;
        int temp_rand;
        int temp_sum = 0;

        //======================================================================================
        // 슬롯1
        //======================================================================================

        int temp_percentSum = 0;
        List<int> availableIndex = new List<int>();

        // 슬롯1 업그레이드 요소 전체순회 및 등장조건 확인
        for (int i = 0; i < playerData.InstanceUpgradeCount_slot1; i++)
        {
            if (playerData.InstanceUpgradeSlot1[i].Event_CheckUpgradeAvailableCondition(playerData))
            {
                availableIndex.Add(i);
                temp_percentSum += playerData.InstanceUpgradeSlot1[i].AppearancePercent;
            }
        }

        if (availableIndex.Count != 0)
        {
            temp_sum = 0;

            // 산출
            temp_rand = UnityEngine.Random.Range(0, temp_percentSum);

            // 슬롯1 업그레이드 요소 전체 확률 순회
            for (int i = 0; i < availableIndex.Count; i++)
            {
                temp_sum += playerData.Percent_InstanceUpgradeSlot1[i];

                if (temp_rand <= temp_sum)
                {
                    selectedIndex_slot1 = availableIndex[i];
                    break;
                }
            }

            // 초기화 함수 호출
            playerData.InstanceUpgradeSlot1[selectedIndex_slot1].Event_OnInstantiatedInStore(playerData);

            // 버튼 텍스트 설정
            temp_text = playerData.InstanceUpgradeSlot1[selectedIndex_slot1].Name + " " +
                playerData.InstanceUpgradeSlot1[selectedIndex_slot1].Cost.ToString() + " 골드";

            txt_UpgradeSlot1.text = temp_text;
            btn_UpgradeSlot1.interactable = true;
        }
        else
        {
            txt_UpgradeSlot1.text = "가능한 업그레이드가 없습니다";
            btn_UpgradeSlot1.interactable = false;
        }

        //======================================================================================
        // 슬롯2
        //======================================================================================

        temp_percentSum = 0;
        availableIndex.Clear();

        // 슬롯2 업그레이드 요소 전체순회 및 등장조건 확인
        for (int i = 0; i < playerData.InstanceUpgradeCount_slot2; i++)
        {
            if (playerData.InstanceUpgradeSlot2[i].Event_CheckUpgradeAvailableCondition(playerData))
            {
                availableIndex.Add(i);
                temp_percentSum += playerData.InstanceUpgradeSlot2[i].AppearancePercent;
            }
        }

        if (availableIndex.Count != 0)
        {
            temp_sum = 0;

            // 산출
            temp_rand = UnityEngine.Random.Range(0, temp_percentSum);

            // 슬롯2 업그레이드 요소 전체 확률 순회
            for (int i = 0; i < availableIndex.Count; i++)
            {
                temp_sum += playerData.Percent_InstanceUpgradeSlot2[i];

                if (temp_rand <= temp_sum)
                {
                    selectedIndex_slot2 = availableIndex[i];
                    break;
                }
            }

            // 초기화 함수 호출
            playerData.InstanceUpgradeSlot2[selectedIndex_slot2].Event_OnInstantiatedInStore(playerData);

            // 버튼 텍스트 설정
            temp_text = playerData.InstanceUpgradeSlot2[selectedIndex_slot2].Name + " " +
                playerData.InstanceUpgradeSlot2[selectedIndex_slot2].Cost.ToString() + " 골드";

            txt_UpgradeSlot2.text = temp_text;
            btn_UpgradeSlot2.interactable = true;
        }
        else
        {
            txt_UpgradeSlot2.text = "가능한 업그레이드가 없습니다";
            btn_UpgradeSlot2.interactable = false;
        }
    }

    // 리롤
    public void ReRoll()
    {
        // 골드 부족
        if (playerData.Get_CurrentGold() < CurrentReRollCost)
        {
            msgPannel.PrintMessage("골드가 부족합니다");
            return;
        }

        // 골드 감소
        playerData.Add_CurrentGold(-CurrentReRollCost);
        CurrentReRollCost++;

        // 리롤 텍스트 갱신
        UpdateReRollText();

        // 아이템 재선정
        ResetUpgradeSlot0();
    }

    // 업그레이드 슬롯 0 버튼클릭
    public void UpgradSlot0(int index)
    {
        // 조커
        if (selectedType_slot0[index] == 0)
        {
            JockerBase temp_jocker = playerData.gameData.JockerData[selectedIndex_slot0[index]];

            if(playerData.Get_CurrentGold() - temp_jocker.Info.PurchaseCost < playerData.Get_PurchaseLimit())
            {
                msgPannel.PrintMessage("골드가 부족합니다!");
            }
            else if(playerData.Get_PlayerJockerCount() >= 5)
            {
                msgPannel.PrintMessage("이미 유물이 5개 입니다!");
            }
            else
            {
                temp_jocker.Event_GetItem(playerData);
                playerData.Add_PlayerJocker(temp_jocker);
                btn_UpgradeSlot0[index].interactable = false;
            }
        }
        // 인스턴스
        else
        {
            InstanceUpgradeBase temp_upgrade = playerData.InstanceUpgradeSlot0[selectedIndex_slot0[index]];

            if (temp_upgrade.Event_CheckUpgradePurchaseable(playerData))
            {
                temp_upgrade.Event_OnPurchase(playerData, flowBroadcaster);
                btn_UpgradeSlot0[index].interactable = false;

                playerData.Broadcaster.instanceUpgradeBtnClicked?.Invoke();
            }
            else
            {
                msgPannel.PrintMessage("구매 할 수 없습니다!");
            }
        }
    }

    // 업그레이드 슬롯 1 버튼클릭
    public void UpgradeSlot1()
    {
        InstanceUpgradeBase temp_upgrade = playerData.InstanceUpgradeSlot1[selectedIndex_slot1];

        if (temp_upgrade.Event_CheckUpgradePurchaseable(playerData))
        {
            temp_upgrade.Event_OnPurchase(playerData, flowBroadcaster);

            // 버튼 텍스트 설정
            string temp_text = temp_upgrade.Name + " " +
                temp_upgrade.Cost.ToString() + " 골드";

            txt_UpgradeSlot1.text = temp_text;
        }
        else
        {
            msgPannel.PrintMessage("구매 할 수 없습니다!");
        }
    }

    // 업그레이드 슬롯 2 버튼클릭
    public void UpgradeSlot2()
    {
        InstanceUpgradeBase temp_upgrade = playerData.InstanceUpgradeSlot2[selectedIndex_slot2];

        if (temp_upgrade.Event_CheckUpgradePurchaseable(playerData))
        {
            temp_upgrade.Event_OnPurchase(playerData, flowBroadcaster);
            btn_UpgradeSlot2.interactable = false;
        }
        else
        {
            msgPannel.PrintMessage("구매 할 수 없습니다!");
        }
    }

    public void SetActiveCardPannel()
    {
        ItemBuyPannel.SetActive(false);
        CardSelectPannel.SetActive(true);
    }

    public void SetActiveItemPannel()
    {
        ItemBuyPannel.SetActive(true);
        CardSelectPannel.SetActive(false);
    }
}
