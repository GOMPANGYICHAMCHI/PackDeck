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
    // ������Ʈ
    //=============================================

    [Header("����� ��ü �г�")]
    public GameObject StorePannel;

    [Header("���� ī�� ������Ʈ")]
    public Button[] btn_RandomCardSelect;
    public Image[] img_RandomCardPattern;
    public TMP_Text[] txt_RandomCardNumber;

    [Header("���� ��ư")]
    public Button btn_Reroll;
    [Header("���� �ؽ�Ʈ")]
    public TMP_Text txt_ReRoll;

    [Header("ī�� ���� �г�")]
    public GameObject CardSelectPannel;

    [Header("������ ���� �г�")]
    public GameObject ItemBuyPannel;

    [Header("ī�� ���� ��ŵ ��ư")]
    public Button btn_SkipCardSelect;

    [Header("����� ������ ��ư")]
    public Button btn_ExitStore;

    [Header("���׷��̵� �׸� ��ư")]
    public Button[] btn_UpgradeSlot0;
    public Button btn_UpgradeSlot1;
    public Button btn_UpgradeSlot2;

    [Header("���׷��̵� �׸� ��ư �ؽ�Ʈ")]
    public TMP_Text[] txt_UpgradeSlot0 = new TMP_Text[2];
    public TMP_Text txt_UpgradeSlot1;
    public TMP_Text txt_UpgradeSlot2;

    //=============================================
    // �ڽ�Ʈ �� ����
    //=============================================

    // ���� ���� �ڽ�Ʈ
    private int CurrentReRollCost;

    // ���õ� �ν��Ͻ� �ε���
    int[] selectedType_slot0 = new int[2];
    int[] selectedIndex_slot0 = new int[2];
    int selectedIndex_slot1;
    int selectedIndex_slot2;

    // ����� ��
    private StoreData storeRule;

    bool isAddtionalRandomCard = false;

    //=============================================
    // ���� ���� ī��
    //=============================================

    private Card[] RandomCards = new Card[3];

    //=============================================

    public override void Initialize()
    {
        base.Initialize();

        // ���� ī�� ������ �ʱ�ȭ
        InitializeRandomCard();

        // ����� �� �ε�
        LoadStoreRule();
        // ��ư �̺�Ʈ ����
        SetBtnEvent();

        // �̺�Ʈ ����
        SetEvent();
    }

    private void SetEvent()
    {
        //flowBroadcaster.InstanceUpgrade11_DeleteDeckCard += InstanceUpgrade11_DeleteDeckCard;
        flowBroadcaster.InstanceUpgrade12_AddDeckCard += InstanceUpgrade12_AddDeckCard;
        //flowBroadcaster.InstanceUpgrade13_UpgradeCardNumber += InstanceUpgrade13_UpgradeCardNumber;
    }

    // �ν��Ͻ� 12 
    private void InstanceUpgrade12_AddDeckCard()
    {
        isAddtionalRandomCard = true;

        // ī�� ���� â Ȱ��ȭ �� ������ ���� â ��Ȱ��ȭ
        CardSelectPannel.SetActive(true);
        ItemBuyPannel.SetActive(false);

        // ���� ���� ī�� ����
        RandomCardPick();
    }

    // ����� ���� ���� ( ���� ī�� ���� â )
    public void EnterStore()
    {
        // ī�� �߰� ���� ���� ��Ȱ��ȭ
        isAddtionalRandomCard = false;

        // ����� �г� Ȱ��ȭ
        StorePannel.SetActive(true);

        // ī�� ���� â Ȱ��ȭ �� ������ ���� â ��Ȱ��ȭ
        CardSelectPannel.SetActive(true);
        ItemBuyPannel.SetActive(false);

        // ���� ���� ī�� ����
        RandomCardPick();
    }

    // ����� ������ 
    private void ExitStore()
    {
        StorePannel.SetActive(false);

        // �÷ο� ��ε�ĳ���Ϳ��� ����
        flowBroadcaster.BroadcasterToMainflow_StoreExit();
    }

    // ī�� ���� ���� ������ ����
    public void CardSelectToItemBuy()
    {
        // ī�� ���� �г� ��Ȱ��ȭ
        CardSelectPannel.SetActive(false);
        // ������ ���� �г� Ȱ��ȭ
        ItemBuyPannel.SetActive(true);

        if(!isAddtionalRandomCard)
        {
            // ���� �ڽ�Ʈ �ʱ�ȭ
            ResetReRollCost();

            // ���׷��̵� �г� �ʱ�ȭ
            ResetUpgradeSlot0();
            ResetUpgradeSlotRight();
        }

    }

    // ��ư �̺�Ʈ ����
    private void SetBtnEvent()
    {
        // ���� ���� ī���̺�Ʈ
        btn_RandomCardSelect[0].onClick.AddListener(() => SelectRandomCard(0));
        btn_RandomCardSelect[1].onClick.AddListener(() => SelectRandomCard(1));
        btn_RandomCardSelect[2].onClick.AddListener(() => SelectRandomCard(2));

        // ī�� ���� ��ŵ ��ư
        btn_SkipCardSelect.onClick.AddListener(CardSelectToItemBuy);

        // ���� ��ư
        btn_Reroll.onClick.AddListener(ReRoll);

        // ���׷��̵� ��ư
        btn_UpgradeSlot0[0].onClick.AddListener(() => UpgradSlot0(0));
        btn_UpgradeSlot0[1].onClick.AddListener(() => UpgradSlot0(1));

        btn_UpgradeSlot1.onClick.AddListener(UpgradeSlot1);
        btn_UpgradeSlot2.onClick.AddListener(UpgradeSlot2);

        // ���� ������ ��ư
        btn_ExitStore.onClick.AddListener(ExitStore);
    }

    // ī�� ���ý�
    private void SelectRandomCard(int index)
    {
        RandomCards[index].Index = playerData.LastCardIndex;
        playerData.LastCardIndex++;

        // ���� ���� �߰�
        playerData.Add_CurrentDeck(RandomCards[index]);

        // ������ ����â���� �Ѿ��
        CardSelectToItemBuy();
    }

    // ����� �� �ε�
    private void LoadStoreRule()
    {
        storeRule = playerData.gameData.StoreRule;
    }

    // ���� ī�� ������ �ʱ�ȭ
    private void InitializeRandomCard()
    {
        for (int i = 0; i < 3; i++)
        {
            RandomCards[i] = new Card();
        }
    }

    // ���� ���� ���� ī�� ���� �� ���� ����
    private void RandomCardPick()
    {
        // ��ü ī�� �ε��� �ʱ�ȭ
        List<int> nums = new List<int>();
        int CardCount = playerData.Get_OriginalDeckCount();

        for (int i = 0; i < CardCount; i++)
        {
            nums.Add(i);
        }

        int tempIndex;
        for (int i = 0; i < 3; i++)
        {
            // ī�� ����
            tempIndex = nums[UnityEngine.Random.Range(0, nums.Count)];
            nums.Remove(tempIndex);

            // ī�� ������ ����
            RandomCards[i] = playerData.Get_OriginalDeck(tempIndex);

            // ī�� �̹��� ����
            img_RandomCardPattern[i].sprite = playerData.gameData.CardSetting.CardPattern[RandomCards[i].patternIndex];
            img_RandomCardPattern[i].color = playerData.gameData.CardSetting.CardColor[RandomCards[i].colorIndex];
            txt_RandomCardNumber[i].text = RandomCards[i].number.ToString();
        }
    }

    // ���� �ڽ�Ʈ �ʱ�ȭ
    private void ResetReRollCost()
    {
        CurrentReRollCost = storeRule.ReRollCost;
        UpdateReRollText();
    }

    // ���� �ڽ�Ʈ �ؽ�Ʈ ������Ʈ
    public void UpdateReRollText()
    {
        txt_ReRoll.text = "���� " + CurrentReRollCost.ToString() + "���"; 
    }

    // ���� ���� ���׷��̵� ���� ����
    public void ResetUpgradeSlot0()
    {
        string temp_text;
        int temp_rand;
        int temp_sum = 0;

        int temp_instancepercentSum = 0;
        List<int> availableInstanceIndex = new List<int>();

        int temp_jockerpercentSum = 0;
        List<int> availableJockerIndex = new List<int>();

        // ����0 ���׷��̵� ��� ��ü��ȸ �� �������� Ȯ��
        for (int i = 0; i < playerData.InstanceUpgradeCount_slot0; i++)
        {
            if(playerData.InstanceUpgradeSlot0[i].Event_CheckUpgradeAvailableCondition(playerData))
            {
                //Debug.Log(i);
                availableInstanceIndex.Add(i);
                temp_instancepercentSum += playerData.InstanceUpgradeSlot0[i].AppearancePercent;
            }
        }

        // ����0 ���� ���� ��ü��ȸ �� �������� Ȯ��
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
                // ��ư �ؽ�Ʈ ����
                txt_UpgradeSlot0[i].text = "������ ���׷��̵尡 �����ϴ�";
                btn_UpgradeSlot0[i].interactable = false;

                // ���� ��ư ��Ȱ��ȭ
                btn_Reroll.interactable = false;
                isNotRerollable = true;

                temp_curePick = -1;
            }

            // �ν��Ͻ��� ������
            else if(availableInstanceIndex.Count == 0)
                temp_curePick = 0;
            // ��Ŀ�� ������
            else if(availableJockerIndex.Count == 0)
                temp_curePick = 1;
            else
                temp_curePick = UnityEngine.Random.Range(0, 2);

            // ��Ŀ
            if(temp_curePick == 0) 
            {
                temp_sum = 0;

                // ����
                temp_rand = UnityEngine.Random.Range(0, temp_jockerpercentSum);

                // ����0 ���׷��̵� ��� ��ü Ȯ�� ��ȸ
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

                // ��ư �ؽ�Ʈ ����
                temp_text = playerData.gameData.JockerData[selectedIndex_slot0[i]].Info.Name + " " +
                    playerData.gameData.JockerData[selectedIndex_slot0[i]].Info.PurchaseCost.ToString() + " ���";
                txt_UpgradeSlot0[i].text = temp_text;

                // ��ư Ȱ��ȭ
                btn_UpgradeSlot0[i].interactable = true;
            }
            // �ν��Ͻ�
            else if(temp_curePick == 1) 
            {
                temp_sum = 0;

                // ����
                temp_rand = UnityEngine.Random.Range(0, temp_instancepercentSum);

                // ����0 ���׷��̵� ��� ��ü Ȯ�� ��ȸ
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
                    //Debug.Log("������ ��� ��" + availableInstanceIndex.Count());
                    Debug.Log("���� �ε���" + selectedIndex_slot0[i]);
                    Debug.Log("��ü ���׷��̵�0 ��" + playerData.InstanceUpgradeSlot0.Count());
                }

                // �ʱ�ȭ �Լ� ȣ��
                playerData.InstanceUpgradeSlot0[selectedIndex_slot0[i]].Event_OnInstantiatedInStore(playerData);

                // ��ư �ؽ�Ʈ ����
                temp_text = playerData.InstanceUpgradeSlot0[selectedIndex_slot0[i]].Name + " " +
                    playerData.InstanceUpgradeSlot0[selectedIndex_slot0[i]].Cost.ToString() + " ���";
                txt_UpgradeSlot0[i].text = temp_text;

                // ��ư Ȱ��ȭ
                btn_UpgradeSlot0[i].interactable = true;
            }
        }

        // ������ �����Ҷ� ���� ��ư Ȱ��ȭ
        if (!isNotRerollable && (availableInstanceIndex.Count != 0 || availableJockerIndex.Count != 0))
            btn_Reroll.interactable = true;
        else
            btn_Reroll.interactable = false;
    }

    // 1,2 ���� ���׷��̵� ���� ����
    public void ResetUpgradeSlotRight()
    {
        string temp_text;
        int temp_rand;
        int temp_sum = 0;

        //======================================================================================
        // ����1
        //======================================================================================

        int temp_percentSum = 0;
        List<int> availableIndex = new List<int>();

        // ����1 ���׷��̵� ��� ��ü��ȸ �� �������� Ȯ��
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

            // ����
            temp_rand = UnityEngine.Random.Range(0, temp_percentSum);

            // ����1 ���׷��̵� ��� ��ü Ȯ�� ��ȸ
            for (int i = 0; i < availableIndex.Count; i++)
            {
                temp_sum += playerData.Percent_InstanceUpgradeSlot1[i];

                if (temp_rand <= temp_sum)
                {
                    selectedIndex_slot1 = availableIndex[i];
                    break;
                }
            }

            // �ʱ�ȭ �Լ� ȣ��
            playerData.InstanceUpgradeSlot1[selectedIndex_slot1].Event_OnInstantiatedInStore(playerData);

            // ��ư �ؽ�Ʈ ����
            temp_text = playerData.InstanceUpgradeSlot1[selectedIndex_slot1].Name + " " +
                playerData.InstanceUpgradeSlot1[selectedIndex_slot1].Cost.ToString() + " ���";

            txt_UpgradeSlot1.text = temp_text;
            btn_UpgradeSlot1.interactable = true;
        }
        else
        {
            txt_UpgradeSlot1.text = "������ ���׷��̵尡 �����ϴ�";
            btn_UpgradeSlot1.interactable = false;
        }

        //======================================================================================
        // ����2
        //======================================================================================

        temp_percentSum = 0;
        availableIndex.Clear();

        // ����2 ���׷��̵� ��� ��ü��ȸ �� �������� Ȯ��
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

            // ����
            temp_rand = UnityEngine.Random.Range(0, temp_percentSum);

            // ����2 ���׷��̵� ��� ��ü Ȯ�� ��ȸ
            for (int i = 0; i < availableIndex.Count; i++)
            {
                temp_sum += playerData.Percent_InstanceUpgradeSlot2[i];

                if (temp_rand <= temp_sum)
                {
                    selectedIndex_slot2 = availableIndex[i];
                    break;
                }
            }

            // �ʱ�ȭ �Լ� ȣ��
            playerData.InstanceUpgradeSlot2[selectedIndex_slot2].Event_OnInstantiatedInStore(playerData);

            // ��ư �ؽ�Ʈ ����
            temp_text = playerData.InstanceUpgradeSlot2[selectedIndex_slot2].Name + " " +
                playerData.InstanceUpgradeSlot2[selectedIndex_slot2].Cost.ToString() + " ���";

            txt_UpgradeSlot2.text = temp_text;
            btn_UpgradeSlot2.interactable = true;
        }
        else
        {
            txt_UpgradeSlot2.text = "������ ���׷��̵尡 �����ϴ�";
            btn_UpgradeSlot2.interactable = false;
        }
    }

    // ����
    public void ReRoll()
    {
        // ��� ����
        if (playerData.Get_CurrentGold() < CurrentReRollCost)
        {
            msgPannel.PrintMessage("��尡 �����մϴ�");
            return;
        }

        // ��� ����
        playerData.Add_CurrentGold(-CurrentReRollCost);
        CurrentReRollCost++;

        // ���� �ؽ�Ʈ ����
        UpdateReRollText();

        // ������ �缱��
        ResetUpgradeSlot0();
    }

    // ���׷��̵� ���� 0 ��ưŬ��
    public void UpgradSlot0(int index)
    {
        // ��Ŀ
        if (selectedType_slot0[index] == 0)
        {
            JockerBase temp_jocker = playerData.gameData.JockerData[selectedIndex_slot0[index]];

            if(playerData.Get_CurrentGold() - temp_jocker.Info.PurchaseCost < playerData.Get_PurchaseLimit())
            {
                msgPannel.PrintMessage("��尡 �����մϴ�!");
            }
            else if(playerData.Get_PlayerJockerCount() >= 5)
            {
                msgPannel.PrintMessage("�̹� ������ 5�� �Դϴ�!");
            }
            else
            {
                temp_jocker.Event_GetItem(playerData);
                playerData.Add_PlayerJocker(temp_jocker);
                btn_UpgradeSlot0[index].interactable = false;
            }
        }
        // �ν��Ͻ�
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
                msgPannel.PrintMessage("���� �� �� �����ϴ�!");
            }
        }
    }

    // ���׷��̵� ���� 1 ��ưŬ��
    public void UpgradeSlot1()
    {
        InstanceUpgradeBase temp_upgrade = playerData.InstanceUpgradeSlot1[selectedIndex_slot1];

        if (temp_upgrade.Event_CheckUpgradePurchaseable(playerData))
        {
            temp_upgrade.Event_OnPurchase(playerData, flowBroadcaster);

            // ��ư �ؽ�Ʈ ����
            string temp_text = temp_upgrade.Name + " " +
                temp_upgrade.Cost.ToString() + " ���";

            txt_UpgradeSlot1.text = temp_text;
        }
        else
        {
            msgPannel.PrintMessage("���� �� �� �����ϴ�!");
        }
    }

    // ���׷��̵� ���� 2 ��ưŬ��
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
            msgPannel.PrintMessage("���� �� �� �����ϴ�!");
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
