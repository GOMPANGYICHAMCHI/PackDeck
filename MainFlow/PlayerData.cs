using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.Windows;

public class PlayerData : MonoBehaviour
{
    //=============================================

    // ���� ���� ������ Ȧ��
    //public GameDataHolder gameDataHolder;

    // �÷��̾� ������ ��ε�ĳ����
    public PlayerDataBroadcaster Broadcaster;

    //���� ������
    public GameData gameData;

    // ��ü ī��Ʈ
    public int ColorCount;
    public int PatternCount;
    public int MaxCardNumber;

    public int JockerCount;
    public int DeBuffCount;

    public int InstanceUpgradeCount_slot0;
    public int InstanceUpgradeCount_slot1;
    public int InstanceUpgradeCount_slot2;

    //=============================================

    // ���� ����ġ ������
    public ScoreAdditional ScoreAdd;

    // �÷��̾� ���׷��̵� ����
    private int[] PatternLevel;
    private int[] ColorLevel;

    public int[] Get_PatternLevel()
    {
        return PatternLevel;
    }
    public int[] Get_ColorLevel()
    {
        return ColorLevel;
    }

    public void Add_PatternLevel(int[] add_amount)
    {
        for (int i = 0; i < PatternCount; i++)
        {
            PatternLevel[i] += add_amount[i];
        }
        Broadcaster.patternlevel_changed?.Invoke();
    }

    public void Add_ColorLevel(int[] add_amount)
    {
        for (int i = 0; i < ColorCount; i++)
        {
            ColorLevel[i] += add_amount[i];
        }
        Broadcaster.colorlevel_changed?.Invoke();
    }

    //=============================================
    // �������� �� ���� 
    //=============================================

    // ���� ��������
    private int CurrentStage = 0;
    // ��ǥ ü��
    private ulong BasicHealth = 10;
    // ���� ����
    private ulong RemainHealth = 0;
    // ������ ���ھ� ������
    private ScoreData scoreData;

    // ���յ� ���� ���� 
    private List<ulong> SelectedScoreAll = new List<ulong>();

    public ScoreData Get_ScoreData()
    {
        return scoreData;
    }
      
    public void Set_ScoreData(ScoreData input)
    {
        scoreData = input;
    }

    public int Get_CurrentStage()
    {
        return CurrentStage;
    }
    public ulong Get_BasicHealth()
    {
        return BasicHealth;
    }
    public ulong Get_RemainHealth()
    {
        return RemainHealth;
    }
    public ulong Get_SelectScoreAll(int index)
    {
        return SelectedScoreAll[index];
    }

    public void Reset_CurrentStage()
    {
        CurrentStage = 0;
        Broadcaster.currentstage_changed?.Invoke();
    }
    public void Reset_BasicHealth()
    {
        BasicHealth = (ulong)gameData.StartMonsterHealth;
        Broadcaster.basichealth_changed?.Invoke();
    }
    public void Reset_RemainHealth()
    {
        RemainHealth = BasicHealth;
        Broadcaster.remainhealth_changed?.Invoke(RemainHealth);
    }

    public void Reset_SelectScoreAll()
    {
        SelectedScoreAll.Clear();
    }

    public void Add_SelectScoreAll(ulong score)
    {
        SelectedScoreAll.Add(score);
    }
    public void Add_CurrentStage(int add_amount)
    {
        CurrentStage += add_amount;

        Broadcaster.currentstage_changed?.Invoke();
    }
    public void Add_BasicHealth(long add_amount)
    {
        if (add_amount < 0)
        {
            BasicHealth = BasicHealth - (ulong)add_amount;
        }
        else
        {
            BasicHealth = BasicHealth + (ulong)add_amount;
        }
        Broadcaster.basichealth_changed?.Invoke();
    }
    public void Add_RemainHealth(long add_amount)
    {
        if (Mathf.Abs(add_amount) > RemainHealth)
            RemainHealth = 0;
        else
            RemainHealth += (ulong)add_amount;

        //UnityEngine.Debug.Log(add_amount);
        //
        //if (add_amount < 0)
        //{
        //    RemainHealth = RemainHealth - (ulong)add_amount;
        //    UnityEngine.Debug.Log("minus");
        //}
        //else
        //{
        //    RemainHealth = RemainHealth + (ulong)add_amount;
        //    UnityEngine.Debug.Log("plus");
        //}
        Broadcaster.remainhealth_changed?.Invoke(RemainHealth);
    }

    public void Set_RemainHealth(ulong set_input)
    {
        RemainHealth = set_input;
        Broadcaster.remainhealth_changed?.Invoke(RemainHealth);
    }

    public void Set_BasicHealth(ulong set_input)
    {
        BasicHealth = set_input;
        Broadcaster.basichealth_changed?.Invoke();
    }

    public void Multiply_BasicHealth(float mul_amount)
    {
        BasicHealth = (ulong)(BasicHealth * mul_amount);
        Broadcaster.basichealth_changed?.Invoke();
    }

    public void Multiply_RemainHealth(float mul_amount)
    {
        RemainHealth = (ulong)(RemainHealth * mul_amount);
        Broadcaster.remainhealth_changed?.Invoke(RemainHealth);
    }

    //=============================================

    // �÷��̾� ���
    private int CurrentGold = 0;
    // �÷��̾� ���� ���̳ʽ� �ѵ� (��)
    private int PurchaseMinusLimit = 0;
    // ���������� ��� �߰� ����
    private int AddGoldAmount;
    // ������ ���
    private float OverDealBonusMultiply;
    // ���� ���� ���� ���� ( �ش� ��ġ �� 1 ��� )
    private int InterestDivideGoldCount;
    // ���� ���� �ѵ�
    private int InterestLimit;

    public int Get_CurrentGold()
    {
        return CurrentGold;
    }
    public int Get_AddGoldAmount()
    {
        return AddGoldAmount;
    }
    public int Get_InterestDivideCardCount()
    {
        return InterestDivideGoldCount;
    }
    public int Get_InterestLimit()
    {
        return InterestLimit;
    }
    public float Get_OverDealBonusMultiply()
    {
        return OverDealBonusMultiply;
    }
    public int Get_PurchaseLimit()
    {
        return PurchaseMinusLimit;
    }

    public void Add_CurrentGold(int add_amount)
    {
        CurrentGold += add_amount;
        Broadcaster.currentgold_changed?.Invoke();
    }
    public void Add_AddGoldAmount(int add_amount)
    {
        AddGoldAmount += add_amount;
        Broadcaster.addgoldamount_changed?.Invoke();
    }
    public void Add_InterestDivideCardCount(int add_amount)
    {
        InterestDivideGoldCount += add_amount;
        Broadcaster.interestdividecardcount_changed?.Invoke();
    }
    public void Add_InterestLimit(int add_amount)
    {
        InterestLimit += add_amount;
        Broadcaster.interestlimit_changed?.Invoke();
    }
    public void Add_OverDealBonusMultiply(float add_amount)
    {
        OverDealBonusMultiply += add_amount;
    }
    public void Set_PurchaseLimit(int input_amount)
    {
        PurchaseMinusLimit = input_amount;
        Broadcaster.purchaseminuslimit_changed?.Invoke();
    }
    public void Reset_PurchaseLimit()
    {
        PurchaseMinusLimit = 0;
        Broadcaster.purchaseminuslimit_changed?.Invoke();
    }
    public void Reset_CurrentGold()
    {
        CurrentGold = 0;
        Broadcaster.currentgold_changed?.Invoke();
    }

    //=============================================

    // �ൿ ����
    private int ActionCost;
    // ������ ����
    private int DumpCost;

    // ���� �ൿ ����
    private int CurrentActionCost;
    // ���� ������ ����
    private int CurrentDumpCost;
    // ���� ������ ���� ����
    private bool CurrentDumpable = true;

    // �ִ� ���� ���� ī�� ��
    private int PlayableCards;
    // �ڵ� ī�� ��
    private int HandSize;

    public bool Get_CurrentDumpable()
    {
        return CurrentDumpable;
    }
    public void Set_CurrentDumpable(bool input)
    {
        CurrentDumpable = input;

    }

    public int Get_ActionCost()
    {
        return ActionCost;
    }
    public int Get_DumpCost()
    {
        return DumpCost;
    }

    public int Get_CurrentActionCost()
    {
        return CurrentActionCost;
    }
    public int Get_CurrentDumpCost()
    {
        return CurrentDumpCost;
    }

    public int Get_PlayableCards()
    {
        return PlayableCards;
    }
    public int Get_HandSize()
    {
        return HandSize;
    }

    public void Add_ActionCost(int add_amount)
    {
        ActionCost += add_amount;
        Broadcaster.maxcost_changed?.Invoke();
    }
    public void Add_DumpCost(int add_amount)
    {
        DumpCost += add_amount;
        Broadcaster.maxcost_changed?.Invoke();
    }
    public void Add_PlayableCards(int add_amount)
    {
        PlayableCards += add_amount;
        Broadcaster.maxcost_changed?.Invoke();
    }
    public void Add_HandSize(int add_amount)
    {
        HandSize += add_amount;
        Broadcaster.maxcost_changed?.Invoke();
    }

    public void Add_CurrentActionCost(int add_amount)
    {
        CurrentActionCost += add_amount;
        Broadcaster.currentcost_changed?.Invoke();
    }
    public void Add_CurrentDumpCost(int add_amount)
    {
        CurrentDumpCost += add_amount;
        Broadcaster.currentcost_changed?.Invoke();
    }

    public void Set_PlayableCard(int input)
    {
        PlayableCards = input;
        Broadcaster.maxcost_changed?.Invoke();
    }
    public void Set_HandSize(int input)
    {
        HandSize = input;
        Broadcaster.maxcost_changed?.Invoke();
    }
    public void Set_ActionCost(int input)
    {
        ActionCost = input;
        Broadcaster.maxcost_changed?.Invoke();
    }    
    public void Set_Dumpcost(int input)
    {
        DumpCost = input;
        Broadcaster.maxcost_changed?.Invoke();
    }
    public void Set_CurrentActionCost(int input)
    {
        CurrentActionCost = input;
        Broadcaster.currentcost_changed?.Invoke();
    }
    public void Set_CurrentDumpCost(int input)
    {
        CurrentDumpCost = input;
        Broadcaster.currentcost_changed?.Invoke();
    }

    //=============================================
    // ī�� ������ 
    //=============================================

    // ���� ������ ��
    private List<Card> originalDeck = new List<Card>();
    // ���� ��
    private List<Card> userDeck = new List<Card>();
    // ���� �� ( �������� ���� �ʱ�ȭ �� )
    private List<Card> currentDeck = new List<Card>();

    // �ڵ� ī�嵦
    private List<Card> handDeck = new List<Card>();
    // ���õ� ī�嵦
    private List<Card> selectDeck = new List<Card>();

    // ������ ī�� ������ �ε���
    public int LastCardIndex;

    public void Update_UserDeck(Card card)
    {
        int index = userDeck.FindIndex(x => x.Index == card.Index);

        if(index != -1)
        {
            userDeck[index] = card;
        }
        Broadcaster.handdeck_changed?.Invoke();
    }

    public Card Get_OriginalDeck(int index)
    {
        return originalDeck[index];
    }
    public Card Get_UserDeck(int index)
    {
        return userDeck[index];
    }
    public Card Get_CurrentDeck(int index)
    {
        return currentDeck[index];
    }
    public Card Get_HandDeck(int index)
    {
        return handDeck[index];
    }
    public Card Get_SelectDeck(int index)
    {
        return selectDeck[index];
    }

    public List<Card> Get_OriginalDeckAll()
    {
        List<Card> temp_list = new List<Card>();
        temp_list = originalDeck.ToList(); 

        return temp_list;
    }
    public List<Card> Get_UserDeckAll()
    {
        List<Card> temp_list = new List<Card>();
        temp_list = userDeck.ToList();

        return temp_list;
    }
    public List<Card> Get_CurrentDeckAll()
    {
        List<Card> temp_list = new List<Card>();
        temp_list = currentDeck.ToList();

        return temp_list;
    }
    public List<Card> Get_HandDeckAll()
    {
        List<Card> temp_list = new List<Card>();
        temp_list = handDeck.ToList();

        return temp_list;
    }
    public List<Card> Get_SelectDeckAll()
    {
        List<Card> temp_list = new List<Card>();
        temp_list = selectDeck.ToList();

        return temp_list;
    }

    public Card Get_CurrentDeckByIndex(int index)
    {
        return currentDeck[index];
    }

    public int Get_OriginalDeckCount()
    {
        return originalDeck.Count;
    }
    public int Get_UserDeckCount()
    {
        return userDeck.Count;
    }
    public int Get_CurrentDeckCount()
    {
        return currentDeck.Count;
    }
    public int Get_HandDeckCount()
    {
        return handDeck.Count;
    }
    public int Get_SelectDeckCount()
    {
        return selectDeck.Count;
    }

    public void Add_UserDeck(Card input_card)
    {
        userDeck.Add(input_card);
        Broadcaster.userdeck_add?.Invoke(input_card);
    }
    public void Add_CurrentDeck(Card input_card)
    {
        currentDeck.Add(input_card);
        Broadcaster.currentdeck_add?.Invoke(input_card);
    }
    public void Add_HandDeck(Card input_card)
    {
        handDeck.Add(input_card);
        Broadcaster.handdeck_add?.Invoke(input_card);
    }
    public void Add_SelectDeck(Card input_card)
    {
        selectDeck.Add(input_card);
        Broadcaster.selectdeck_add?.Invoke(input_card);
    }

    public void Remove_UserDeck(Card input_card)
    {
        userDeck.Remove(input_card);
        Broadcaster.userdeck_remove?.Invoke(input_card);
    }
    public void Remove_CurrentDeck(Card input_card)
    {
        currentDeck.Remove(input_card);
        Broadcaster.currentdeck_remove?.Invoke(input_card);
    }
    public void Remove_HandDeck(Card input_card)
    {
        handDeck.Remove(input_card);
        Broadcaster.handdeck_remove?.Invoke(input_card);
    }
    public void Remove_SelectDeck(Card input_card)
    {
        selectDeck.Remove(input_card);
        Broadcaster.selectdeck_remove?.Invoke(input_card);
    }
    public void Dump_HandDeck(Card input_card)
    {
        handDeck.Remove(input_card);
        Broadcaster.handdeck_dumped?.Invoke(input_card);
    }

    public void Reset_CurrentDeck()
    {
        currentDeck.Clear();
        currentDeck = userDeck.ToList();
        Broadcaster.currentdeck_changed?.Invoke();
    }
    public void Reset_UserDeck()
    {
        userDeck.Clear();
        userDeck = originalDeck.ToList();
        Broadcaster.userdeck_changed?.Invoke();
    }
    public void Reset_HandDeck()
    {
        handDeck.Clear();
        Broadcaster.handdeck_changed?.Invoke();
    }
    public void Reset_SelectDeck()
    {
        selectDeck.Clear();
        Broadcaster.selectdeck_changed?.Invoke();
    }

    // ���� ���� �ڵ�� (ī�� ��ο�)
    public void CurrentToHandDeck(Card input_card)
    {
        currentDeck.Remove(input_card);
        handDeck.Add(input_card);
        Broadcaster.currentdeck_remove?.Invoke(input_card);
        Broadcaster.handdeck_add?.Invoke(input_card);
    }
    // �ڵ� ���� ���� (ī�� ����)
    public void HandToSelectDeck(Card input_card)
    {
        handDeck.Remove(input_card);
        selectDeck.Add(input_card);
        Broadcaster.handdeck_remove?.Invoke(input_card);
        Broadcaster.selectdeck_add?.Invoke(input_card);
    }
    // ���� ���� �ڵ� (���� ���)
    public void SelectToHandDeck(Card input_card)
    {
        selectDeck.Remove(input_card);
        handDeck.Add(input_card);
        Broadcaster.handdeck_add?.Invoke(input_card);
        Broadcaster.selectdeck_remove?.Invoke(input_card);
    }

    // ī�� ���� ����
    public void HandToSelectForcibly(Card input_card)
    {
        handDeck.Remove(input_card);
        selectDeck.Add(input_card);
        Broadcaster.cardselected_forcibly?.Invoke(input_card);
    }

    //=============================================
    // ��Ŀ 
    //=============================================

    public int[] JockerPercent;

    private List<JockerBase> PlayerJocker = new List<JockerBase>();

    public List<JockerBase> Get_AllPlayerJocker()
    {
        return PlayerJocker;
    }

    public int Get_PlayerJockerCount()
    {
        return PlayerJocker.Count();
    }
    public JockerBase Get_PlayerJocker(int index)
    {
        return PlayerJocker[index];
    }

    public void Add_PlayerJocker(JockerBase input_jocker)
    {
        PlayerJocker.Add(input_jocker);
        Broadcaster.playerjocker_add?.Invoke(input_jocker);
        Broadcaster.playerjocker_changed?.Invoke();
    }
    public void Remove_PlayerJocker(JockerBase input_jocker)
    {
        PlayerJocker.Remove(input_jocker);
        Broadcaster.playerjocker_remove?.Invoke(input_jocker);
        Broadcaster.playerjocker_changed?.Invoke();
    }

    public void ReSetPlayerJocker()
    {
        PlayerJocker.Clear();
        Broadcaster.playerjocker_changed?.Invoke();
    }

    //=============================================
    // ����� 
    //=============================================

    private DeBuffBase CurrentDeBuff;

    // ����� Ȯ�� Ȯ��
    public int PercentSum_DeBuff;

    // ����� ��ü Ȯ��
    public int[] DeBuffPercent;

    public void Set_CurrentDebuff(DeBuffBase input_debuff)
    {
        CurrentDeBuff = input_debuff;
        Broadcaster.currentdebuff_changed?.Invoke();
    }

    public DeBuffBase Get_CurrentDebuff()
    {
        return CurrentDeBuff;
    }

    public void remove_Debuff()
    {
        CurrentDeBuff = null;
        Broadcaster.currentdebuff_removed?.Invoke();
    }

    //=============================================
    // �ν��Ͻ� ����  
    //=============================================

    // �ν��Ͻ� ���׷��̵� �׸��
    public List<InstanceUpgradeBase> InstanceUpgradeSlot0 = new List<InstanceUpgradeBase>();
    public List<InstanceUpgradeBase> InstanceUpgradeSlot1 = new List<InstanceUpgradeBase>();
    public List<InstanceUpgradeBase> InstanceUpgradeSlot2 = new List<InstanceUpgradeBase>();

    // �ν��Ͻ� ���� ����0 Ȯ�� Ȯ��
    //public int PercentSum_InstanceUpgradeSlot0;
    // �ν��Ͻ� ���� ����1 Ȯ�� Ȯ��
    //public int PercentSum_InstanceUpgradeSlot1;
    // �ν��Ͻ� ���� ����2 Ȯ�� Ȯ��
    //public int PercentSum_InstanceUpgradeSlot2;

    // �ν��Ͻ� ���� ����0 ��ü Ȯ��
    public int[] Percent_InstanceUpgradeSlot0;
    // �ν��Ͻ� ���� ����1 ��ü Ȯ��
    public int[] Percent_InstanceUpgradeSlot1;
    // �ν��Ͻ� ���� ����2 ��ü Ȯ��
    public int[] Percent_InstanceUpgradeSlot2;

    //=============================================

    // ��Ŀ ������ �ε�
    // todo
    private void LoadJockerData()
    {
        // ��ü ��Ŀ ��
        JockerCount = gameData.JockerData.Count();
        JockerPercent = new int[JockerCount];

        // ��Ŀ ������ ��ü ��ȸ
        for (int i = 0; i < JockerCount; i++)
        {
            // Ȯ�� �Է�
            JockerPercent[i] = gameData.JockerData[i].Info.Percent;
        }
    }

    // ����� ������ �ε�
    // todo
    private void LoadDeBuffData()
    {
        DeBuffCount = gameData.DeBuffData.Count();
        DeBuffPercent = new int[DeBuffCount];

        // ����� ������ ��ü ��ȸ
        for (int i = 0; i < DeBuffCount; i++)
        {
            // Ȯ�� �Է�
            DeBuffPercent[i] = gameData.DeBuffData[i].data.Percent;
            // Ȯ�� �ջ� ++
            PercentSum_DeBuff += DeBuffPercent[i];
        }
    }

    // �ν��Ͻ� ���׷��̵� ������ �ε�
    private void LoadInstanceUpgradeData()
    {
        int allInstanceCount = gameData.InstanceUpgradeData.Count();

        List<int> percent_slot0 = new List<int>();
        List<int> percent_slot1 = new List<int>();
        List<int> percent_slot2 = new List<int>();

        // �ν��Ͻ� ���׷��̵� ������ ��ü ��ȸ
        for (int i = 0; i < allInstanceCount; i++)
        {
            switch (gameData.InstanceUpgradeData[i].UpgradeSlotIndex)
            {
                case 0:
                    percent_slot0.Add(gameData.InstanceUpgradeData[i].AppearancePercent);
                    //PercentSum_InstanceUpgradeSlot0 += gameData.InstanceUpgradeData[i].AppearancePercent;
                    InstanceUpgradeSlot0.Add(gameData.InstanceUpgradeData[i]);

                    break;

                case 1:
                    percent_slot1.Add(gameData.InstanceUpgradeData[i].AppearancePercent);
                    //PercentSum_InstanceUpgradeSlot1 += gameData.InstanceUpgradeData[i].AppearancePercent;
                    InstanceUpgradeSlot1.Add(gameData.InstanceUpgradeData[i]);

                    break;

                case 2:
                    percent_slot2.Add(gameData.InstanceUpgradeData[i].AppearancePercent);
                    //PercentSum_InstanceUpgradeSlot2 += gameData.InstanceUpgradeData[i].AppearancePercent;
                    InstanceUpgradeSlot2.Add(gameData.InstanceUpgradeData[i]);

                    break;
            }
        }

        InstanceUpgradeCount_slot0 = percent_slot0.Count;
        InstanceUpgradeCount_slot1 = percent_slot1.Count;
        InstanceUpgradeCount_slot2 = percent_slot2.Count;

        Percent_InstanceUpgradeSlot0 = percent_slot0.ToArray();
        Percent_InstanceUpgradeSlot1 = percent_slot1.ToArray();
        Percent_InstanceUpgradeSlot2 = percent_slot2.ToArray();
    }

    // ī�嵥���� ����
    void GeneratreCardData()
    {
        LastCardIndex = 0;
        Card generatedCard;
        int index = 0;

        for (int p = 0; p < gameData.CardSetting.CardPattern.Count(); p++)
        {
            for (int c = 0; c < gameData.CardSetting.CardColor.Count(); c++)
            {
                for (int n = 0; n < gameData.CardSetting.CardNumber; n++)
                {
                    generatedCard = new Card();

                    generatedCard.colorIndex = c;
                    generatedCard.patternIndex = p;
                    generatedCard.number = n + 1;
                    generatedCard.Index = LastCardIndex;
                    LastCardIndex++;

                    originalDeck.Add(generatedCard);
                    index++;
                }
            }
        }

        userDeck = originalDeck.ToList();
    }

    // �������� ���� ���� �ʱ�ȭ
    public void ResetStageReward()
    {
        // �������� �� ��� �߰� ���� �ʱ�ȭ
        AddGoldAmount = gameData.StageRewardData.AddGoldamountPerStage;
        // ������ ��� �ʱ�ȭ
        OverDealBonusMultiply = gameData.StageRewardData.OverDealBonusMultiply;
        // ���� ���� ���� ī��� �ʱ�ȭ
        InterestDivideGoldCount = gameData.StageRewardData.InterestDivideGoldCount;
        // ���� ���� ���� �ѵ� �ʱ�ȭ
        InterestLimit = gameData.StageRewardData.InterestLimit;
    }

    // ��ü �ڽ�Ʈ �ʱ�ȭ
    public void ResetCost()
    {
        // �ڽ�Ʈ �ʱ�ȭ
        Set_ActionCost(gameData.ActionCost);
        Set_Dumpcost(gameData.DumpCost);
        Set_PlayableCard(gameData.SelectableCardAmount);
        Set_HandSize(gameData.HandSize);

        Set_CurrentActionCost(ActionCost);
        Set_CurrentDumpCost(DumpCost);

        Reset_BasicHealth();
        Reset_CurrentStage();
        Reset_CurrentGold();
        //UnityEngine.Debug.Log(BasicHealth);
    }

    // ���ھ� ������ ����
    private void ResetScoreData()
    {
        PatternLevel = new int[PatternCount];
        ColorLevel = new int[ColorCount];
    }

    // ���� ����ġ �� ���׷��̵� ���� �ʱ�ȭ
    public void ResetScore()
    {
        ScoreAdd.InputData(gameData.ScoreAdditionalData);

        for (int i = 0; i < PatternCount; i++) 
        {
            PatternLevel[i] = 1;
        }

        for (int i = 0; i < ColorCount; i++)
        {
            ColorLevel[i] = 1;
        }

        CurrentGold = 0;
    }

    // ����ġ ���׷��̵� ���� �ʱ�ȭ
    public void ResetPlayerCardScoreUpgradeLevel()
    {

    }

    private void ResetCount()
    {
        // �÷�, ����, ��Ŀ ī��Ʈ �ʱ�ȭ
        ColorCount = gameData.CardSetting.CardColor.Count();
        PatternCount = gameData.CardSetting.CardPattern.Count();
        JockerCount = gameData.JockerData.Count();
        DeBuffCount = gameData.DeBuffData.Count();
        MaxCardNumber = gameData.CardSetting.CardNumber;
    }

    public void OnInitialize()
    {
        // ���� ������ �ʱ�ȭ
        gameData = GameObject.Find("GameDataHolder").GetComponent<GameDataHolder>().OriginalGameData.gameData;

        // ��Ŀ ������ �ε�
        LoadJockerData();
        // ����� ������ �ε�
        LoadDeBuffData();
        // �ν��Ͻ� ������ �ε�
        LoadInstanceUpgradeData();

        // ī�� ������ ����
        GeneratreCardData();
        // ��ü ī��Ʈ �ʱ�ȭ
        ResetCount();
        // ���ھ� ������ �ʱ�ȭ
        ResetScoreData();

        ResetValues();
    }

    public void ResetValues()
    {
        ResetScore();
        ResetCost();
        ResetStageReward();
        ReSetPlayerJocker();
    }

    //=============================================
}