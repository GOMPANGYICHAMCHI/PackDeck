using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MainFlow : MonoBehaviour
{
    //=============================================
    public UIManager uimanager;

    public int CurrentCardIndex = 0;

    // ���� ���� ������ Ȧ��
    public GameDataHolder gameDataHolder;

    //���� ������
    public GameData gameData;

    // ���� ���� ������
    //public ScroeCombination[] scoreData;

    //=============================================

    //�������� ����ġ_A
    public int[] CardPatternScore_A = new int[3];
    //���� ���� ����ġ_A
    public int[] CardColorScore_A = new int[3];

    //�������� ����ġ_B
    public int[] CardPatternScore_B = new int[3];
    //���� ���� ����ġ_B
    public int[] CardColorScore_B = new int[3];

    //=============================================

    // ���� ������
    //public ScoreData scoreData;

    public int ColorCount;
    public int PatternCount;
    public int JockerCount;

    //=============================================
    // �������� �� ���� 
    //=============================================

    // ���� ��������
    public int CurrentStage = 0;
    // ��ǥ ����
    public double GoalScore = 10;
    // ���� ����
    public double CurrentScore = 0;
    // ���յ� ����
    public double SelectedScore = 0;
    
    // ���� ������
    public ScoreData scoreData;

    //=============================================

    // �÷��̾� ���
    public int CurrentGold = 0;
    // ���������� ��� �߰� ����
    //int AddGoldAmount = 5;
    // ���� ���� ���� ī��� ( �ش� ��ġ �� 1 ��� )
    int InterestDivideCardCount = 5;
    int InterestLimit = 5;

    // �÷��̾� ���׷��̵� ����
    public int[] PatternLevel = new int[3] { 1, 1, 1 };
    public int[] ColorLevel = new int[3] { 1, 1, 1 };

    public int ScoreMultiplyAmount = 1;

    //=============================================

    public int ActionCost;
    public int DumpCost;

    // ���� �ൿ ����
    public int CurrentActionCost;
    // ���� ������ ����
    public int CurrentDumpCost;

    // �ִ� ���� ���� ī�� ��
    public int PlayableCards;
    // �ڵ� ī�� ��
    public int HandSize;

    //=============================================
    // ī�� ������ 
    //=============================================

    // ���� ���� ��
    public List<Card> userDeck = new List<Card>();
    // ���� �� ( �������� ���� �ʱ�ȭ �� )
    public List<Card> currentDeck = new List<Card>();

    // �ڵ� ī�嵦
    public List<Card> handDeck = new List<Card>();
    // ���õ� ī�嵦
    public List<Card> selectDeck = new List<Card>();

    //=============================================
    // ��Ŀ 
    //=============================================

    public Transform JockerPos;

    public JockerBase[] JockerData;

    //public int[] JockerPercent;

    public List<JockerBase> PlayerJocker;

    //=============================================
    // ����� 
    //=============================================

    public Transform DeBuffPos;

    public DeBuffBase[] DeBuffData;

    //public int[] DeBuffPercent;

    public DeBuffBase CurrentDeBuff;

    // ����� Ȯ�� Ȯ��
    private int PersentSum_DeBuff;

    // ����� ��ü Ȯ��
    private int[] DeBuffPercent;

    //=============================================
    // ���ھ� ����
    //public List<ScoreStackData> scoreStackData = new List<ScoreStackData>();
    public int[] scorestackUIIndex = new int[6];
    public int[] scorestackA = new int[6];
    public float ScoreStackDelay = 0.3f;

    private void Start()
    {
        // ���� ������ �ʱ�ȭ
        gameData = gameDataHolder.OriginalGameData.gameData;
        LoadJockerData();
        LoadDeBuffData();
        //uimanager.LoadCardPercentData(gameData.CardSetting.CardPatternPercent,gameData.CardSetting.CardColorPercent);

        // �÷�, ����, ��Ŀ ī��Ʈ �ʱ�ȭ
        ColorCount = gameData.CardSetting.CardColor.Count();
        PatternCount = gameData.CardSetting.CardPattern.Count();
        JockerCount = JockerData.Count();

        // ���� �� �÷� ���� ����ġ �ʱ�ȭ
        scoreData = new ScoreData(PatternCount, ColorCount);
        ResetScore();

        // ��� �ؽ�Ʈ �ʱ�ȭ
        uimanager.ResetGoldText();
        //uimanager.VoucherResetStage = gameData.VoucherResetStage;

        // UI �Ŵ������� ī�� ���� ��������Ʈ �� �÷� �Ҵ�
        uimanager.PatternSprites = gameData.CardSetting.CardPattern;
        uimanager.PatternColors = gameData.CardSetting.CardColor;

        // ���� ���� ���� ����
        uimanager.GenerateCombinationScorePanel();

        StartGame();
    }

    // ��Ŀ ������ �ε�
    private void LoadJockerData()

    {
        JockerData = new JockerBase[JockerPos.childCount];
        int[] JockerPercent = new int[JockerPos.childCount];

        // ��Ŀ ������ ��ġ�� �ڼ� ��ü ��ȸ
        for (int i = 0; i < JockerPos.childCount; i++)
        {
            // ������Ʈ �ε�
            JockerData[i] = JockerPos.GetChild(i).GetComponent<JockerBase>();
            // ������Ʈ ������ ����
            //JockerData[i].ResetJocker(gameData.JockerData[i]);

            // Ȯ�� �Է�
            //JockerPercent[i] = gameData.JockerData[i].Percent;
        }

        // UI�Ŵ������� Ȯ�� ���� ������ ����
        uimanager.LoadJockerPercentData(JockerPercent);
    }

    // ����� ������ �ε�
    private void LoadDeBuffData()
    {
        DeBuffData = new DeBuffBase[DeBuffPos.childCount];
        DeBuffPercent = new int[DeBuffPos.childCount];

        // ����� ������ ��ġ�� �ڼ� ��ü ��ȸ
        for (int i = 0; i < DeBuffPos.childCount; i++) 
        {
            // ������Ʈ �ε�
            DeBuffData[i] = DeBuffPos.GetChild(i).GetComponent<DeBuffBase>();
            // ������Ʈ ������ ����
            //DeBuffData[i].ResetDeBuff(gameData.DeBuffData[i]);

            // Ȯ�� �Է�
            //DeBuffPercent[i] = gameData.DeBuffData[i].Percent;
            // Ȯ�� �ջ� ++
            PersentSum_DeBuff += DeBuffPercent[i];
        }
    }

    private void ResetScoreStack()
    {
        for (int i = 0; i < ColorCount + PatternCount; i++) 
        {
            //scoreStackData.Add(new ScoreStackData());
        }
    }

    private void ResetCost()
    {
        CurrentActionCost = ActionCost;
        CurrentDumpCost = DumpCost;
    }

    // ���׷��̵� ���� �ʱ�ȭ
    private void ResetUpgradeLevel()
    {
        PatternLevel[0] = 1;
        PatternLevel[1] = 1;
        PatternLevel[2] = 1;

        ColorLevel[0] = 1;
        ColorLevel[1] = 1;
        ColorLevel[2] = 1;
    }

    private void ResetScore()
    {
        //CardPatternScore_A[0] = gameData.CardSetting.CardPatternAdditionalScore_A[0];
        //CardPatternScore_A[1] = gameData.CardSetting.CardPatternAdditionalScore_A[1];
        //CardPatternScore_A[2] = gameData.CardSetting.CardPatternAdditionalScore_A[2];
        //
        //CardColorScore_A[0] = gameData.CardSetting.CardColorAdditionalScore_A[0];
        //CardColorScore_A[1] = gameData.CardSetting.CardColorAdditionalScore_A[1];
        //CardColorScore_A[2] = gameData.CardSetting.CardColorAdditionalScore_A[2];
        //
        //CardPatternScore_B[0] = gameData.CardSetting.CardPatternAdditionalScore_B[0];
        //CardPatternScore_B[1] = gameData.CardSetting.CardPatternAdditionalScore_B[1];
        //CardPatternScore_B[2] = gameData.CardSetting.CardPatternAdditionalScore_B[2];
        //
        //CardColorScore_B[0] = gameData.CardSetting.CardColorAdditionalScore_B[0];
        //CardColorScore_B[1] = gameData.CardSetting.CardColorAdditionalScore_B[1];
        //CardColorScore_B[2] = gameData.CardSetting.CardColorAdditionalScore_B[2];

        CurrentGold = 0;
    }

    // ���ӽ��� ��
    public void StartGame()
    {
        // ���� ������ �ʱ�ȭ
        gameData = gameDataHolder.OriginalGameData.gameData;

        // ī�� �ε��� �ʱ�ȭ
        CurrentCardIndex = 0;
        // ��ü ī�� ���� ������Ʈ ����
        uimanager.DeleteAllDeckCard();
        // ī�� ������ �� ��ü ���� ����
        CardDataGeneratre();

        // ���� �� �÷� ���� ����ġ �ʱ�ȭ
        ResetScore();
        // ���׷��̵� ���� �ʱ�ȭ
        ResetUpgradeLevel();

        // �������� �� ��� �߰� ���� ����
        //AddGoldAmount = gameData.AddGoldamountPerStage;

        // �ڽ�Ʈ �ʱ�ȭ
        ActionCost = gameData.ActionCost;
        DumpCost = gameData.DumpCost;
        PlayableCards = gameData.SelectableCardAmount;
        HandSize = gameData.HandSize;

        // �ڽ�Ʈ �ʱ�ȭ
        ResetCost();

        // ���� �������� �ʱ�ȭ
        CurrentStage = 0;
        // ��ǥ ���� �ʱ�ȭ
        GoalScore = gameData.StartMonsterHealth;
        
        // ���� ���� �г� ��Ȱ��ȭ
        uimanager.gameoverPanel.SetActive(false);

        // �������� ����
        StartStage();
    }

    // �������� ���۽�
    public void StartStage()
    {
        // ���� ����â ����
        uimanager.SetCombinationScoresText();

        // ���� ��������++
        CurrentStage++;

        // ���� �ڽ�Ʈ ����
        ResetCost();

        // ��ǥ ���� ����
        if(CurrentStage != 1)
        {
            // ��ǥ ���� ����
            GoalScore += gameData.AddGoalScorePerStage;
            if (gameData.MultiplyGoalScorePerStage != 0)
            {
                GoalScore *= gameData.MultiplyGoalScorePerStage;
                // �Ҽ��� ������
                GoalScore = Math.Truncate(GoalScore);
            }
        }

        // ���� ���� �ʱ�ȭ
        CurrentScore = 0;
        // ���յ� ���� �ʱ�ȭ
        SelectedScore = 0;

        // �� ���� ����
        currentDeck = userDeck.ToList();

        // �ڵ嵦 ����
        handDeck.Clear();
        // ���õ� ī�嵦 ����
        selectDeck.Clear();

        // ����� ���� ( ���������� 3�� ��� �϶� )
        if (CurrentStage % 3 == 0)
        {
            //CurrentDeBuff.Event_StageStart(this);
        }
        else if(CurrentStage % 3 == 1)
        {
            PickRandomDeBuff();
        }
        uimanager.ResetDeBuffUI();

        // UI �������� ���� �Լ� ȣ��
        uimanager.StartStage();

        // ���� ī�� �� �� ������Ʈ ����
        PickRandomCard();
    }

    public void StartScoreStack()
    {
        // ���õ� ī�尡 ������
        if(selectDeck.Count == 0)
        {
            uimanager.msgPannel.PrintMessage("���õ� ī�尡 �����ϴ�!");
            return;
        }

        // ���� �ൿ ���� ����
        CurrentActionCost--;

        // ���ھ� �� �ڽ�Ʈ �ؽ�Ʈ ����
        uimanager.ResetScoreCostText();

        uimanager.ButtonCover.SetActive(true);

        // ���� ī�� ������Ʈ ���� �� �ڵ�ī�� ������Ʈ ����
        uimanager.MakeSelectedCard();

        // ���� Ȯ��
        SelectedScore = CheckCurrentScore();

        // ���ھ� ���� ������ ���
        StartCoroutine("PlayScoreSequence");
    }

    // ���ھ� ���� ������ / �ڵ�ī�� ���� Ȥ�� ��ҽ�
    public void ScoreStackPreview()
    {
        CheckCurrentScore();
        uimanager.ActivePreviewScoreStack();
    }

    // ��Ŀ 10 ���� �Լ�
    public void FreeUpgrade_Jocker10()
    {
        // UI ��Ȱ��ȭ
        uimanager.DisableJockerUI(9,false);

        //
        uimanager.EnableFreeUpgradePannel();
    }

    // ���� ����� ����
    public void PickRandomDeBuff()
    {
        // ���� �ε��� ��
        int temp_index = UnityEngine.Random.Range(0, PersentSum_DeBuff);
        int temp_sum = 0;

        for (int i = 0; i < DeBuffPercent.Count(); i++) 
        {
            temp_sum += DeBuffPercent[i];

            if(temp_index < temp_sum)
            {
                temp_index = i;
                break;
            }
        }

        // ���� ����� ����
        CurrentDeBuff = DeBuffData[temp_index];

        // ����� ���� UI ����
        uimanager.ResetDeBuffUI();
    }

    // ���� �ൿ��
    public void NextAction()
    {
        // ���ھ� ���� �г� Off
        uimanager.DisableAllScoreStack();
        uimanager.ButtonCover.SetActive(false);

        //// ���� �ൿ ���� ����
        //CurrentActionCost--;

        //// ���ھ� �� �ڽ�Ʈ �ؽ�Ʈ ����
        //uimanager.ResetScoreCostText();

        // ���ӿ��� Ȥ�� �¸� ���� �Ǻ�
        if (CheckGameOver() == true)
            return;

        // ���õ� �� �ʱ�ȭ
        selectDeck.Clear();

        // ���յ� ���� �ʱ�ȭ
        SelectedScore = 0;

        // ���� ī�� �ʱ�ȭ
        uimanager.DisableAllSelectCard();

        // ī�� ���� �� ����
        PickRandomCard();
    }

    // ���� ���� ���� Ȯ��
    bool CheckGameOver()
    {
        // ���ӿ���
        if(CurrentActionCost == 0 && CurrentScore < GoalScore)
        {
            uimanager.gameoverPanel.SetActive(true);
            return true;
        }

        // �¸� ����
        if(CurrentScore >= GoalScore)
        {
            uimanager.stageClearPanel.SetActive(true);

            if (CurrentStage % 3 == 0)
            {
                // ����� �������� ���� �Լ� ȣ��
                //CurrentDeBuff.Event_StageEnd(this);
                // ���ο� ����� ���� ��
                PickRandomDeBuff();
            }

            //if (CurrentStage % gameData.StorePerStage == 0)
            //{
            //    uimanager.SwitchStoreScorePanel(true);
            //    uimanager.StageClear_btn.transform.GetChild(0).GetComponent<TMP_Text>().text = "���׷��̵� ����";
            //
            //    // ��� �߰� �� ��� �ؽ�Ʈ ����
            //    //Gold += AddGoldAmount;
            //    AddGoldAfterRound();
            //    uimanager.ResetGoldText();
            //
            //    // ���� 2�� ����
            //    ScoreMultiplyAmount = 1;
            //}
            //else
            //{
            //    uimanager.StageClear_btn.transform.GetChild(0).GetComponent<TMP_Text>().text = "���� ����������";
            //}

            // ���� ���� 2�� �ؽ�Ʈ ��Ȱ��ȭ
            uimanager.ScoreDoublePannel.SetActive(false);
            uimanager.ResetDeckCards();

            // ��Ŀ ��ü ��ȸ �� �������� ���� �Լ� ����
            for (int i = 0; i < PlayerJocker.Count; i++) 
            {
                //PlayerJocker[i].Event_StageEnd(TODO);
            }

            // ��Ŀ ����â �� ����� ���� â ��Ȱ��ȭ
            uimanager.DisableJockerInfoPannel();
            uimanager.SetActiveDeBuffInfoPannel(false);

            return true;
        }

        return false;
    }

    // ���� �� ��� �߰�
    public int AddGoldAfterRound()
    {
        int AddAmount = 0;

        // ������ ��� �� �߰�
        float OverDealPercent = (float)(CurrentScore / GoalScore);
        OverDealPercent = Mathf.Floor(OverDealPercent * 10f) / 10f;
        // �߰�
        AddAmount += Mathf.Clamp((int)(OverDealPercent * CurrentActionCost), 0, 10);

        // ���� ����
        int Interest = CurrentGold / InterestDivideCardCount;
        // �߰�
        AddAmount += Mathf.Clamp(Interest, 0, InterestLimit);

        return AddAmount;
    }
     
    // ���� ���� ���� Ȯ��
    public double CheckCurrentScore()
    {
        double FinalScore = 0;

        // ���ھ� ���� UI �ε��� ����
        Array.Fill(scorestackUIIndex, -1);
        //int curstackIndex = 0;

        scoreData.ResetData();
        //scoreStackData.Clear();

        // ī�� �� �� ī�� �ѹ� ī��Ʈ
        for (int i = 0; i < selectDeck.Count(); i++) 
        {
            scoreData.patternCardCount[selectDeck[i].patternIndex]++;
            scoreData.colorCardCount[selectDeck[i].colorIndex]++;

            scoreData.patternNumberSum[selectDeck[i].patternIndex] += selectDeck[i].number;
            scoreData.colorNumberSum[selectDeck[i].colorIndex] += selectDeck[i].number;
        }

        // ��Ŀ ���� ��� ����
        for (int i = 0; i < PlayerJocker.Count(); i++) 
        {
            //PlayerJocker[i].Event_CheckScore(this,scoreData);
        }

        // ����� ����
        if (CurrentStage % 3 == 0)
        {
            // ����� �������� ���� �Լ� ȣ��
            //CurrentDeBuff.Event_ScoreCalculate(this,scoreData);
        }

        //double temp_score;
        //ScoreStackData stackData;

        // ���� ���� ���
        // ����
        for (int i = 0; i < PatternCount; i++)
        {
            //temp_score = 
            //    (scoreData.patternNumberSum[i] * Mathf.Pow(CardPatternScore[i] + scoreData.additional_PatternB[i], scoreData.patternCount[i]));

            // (���� ī�� ���� �� * (A����ġ + �߰� A����ġ)) + (���� ī�� �� * (B����ġ + �߰� B����ġ))
            //temp_score = 
            //    ((scoreData.patternNumberSum[i] * (CardPatternScore_A[i] + scoreData.additional_PatternA[i])) + (scoreData.patternCount[i] * (CardPatternScore_B[i] + scoreData.additional_PatternB[i])));

            //FinalScore += temp_score;

            // ���ھ� ���� ������
            //stackData = new ScoreStackData();
            //stackData.a = scoreData.patternNumberSum[i] * (CardPatternScore_A[i] + scoreData.additional_PatternA[i]);
            //stackData.b = scoreData.patternCount[i] * (CardPatternScore_B[i] + scoreData.additional_PatternB[i]);
            //
            ////stackData.c = scoreData.patternCount[i];
            //stackData.sum = temp_score;
            //
            //if(temp_score != 0 )
            //{
            //    stackData.isScoreValid = true;
            //
            //    scorestackUIIndex[i] = curstackIndex;
            //    curstackIndex++;
            //}
            //
            //scoreStackData.Add(stackData);
        }
        // �÷�
        for (int i = 0; i < ColorCount; i++)
        {
            //temp_score = 
            //    (scoreData.colorNumberSum[i] * Mathf.Pow(CardColorScore[i] + scoreData.additional_ColorB[i], scoreData.colorCount[i]));

            // (�÷� ī�� ���� �� * (A����ġ + �߰� A����ġ)) + (�÷� ī�� �� * (B����ġ + �߰� B����ġ))
            //temp_score = 
            //    ((scoreData.colorNumberSum[i] * (CardColorScore_A[i] + scoreData.additional_ColorA[i])) + (scoreData.colorCount[i] * (CardColorScore_B[i] + scoreData.additional_ColorB[i])));

            //FinalScore += temp_score;

            // ���ھ� ���� ������
            //stackData = new ScoreStackData();
            //stackData.a = scoreData.colorNumberSum[i] * (CardColorScore_A[i] + scoreData.additional_ColorA[i]);
            //stackData.b = scoreData.colorCount[i] * (CardColorScore_B[i] + scoreData.additional_ColorB[i]);
            //
            ////stackData.c = scoreData.colorCount[i];
            //stackData.sum = temp_score;
            //
            //if (temp_score != 0)
            //{
            //    stackData.isScoreValid = true;
            //
            //    scorestackUIIndex[PatternCount + i] = curstackIndex;
            //    curstackIndex++;
            //}
            //
            //scoreStackData.Add(stackData);
        }

        //Debug.Log("���� ���� : " + scoreData.FinalScoreMultiply);

        //if(scoreData.FinalScoreMultiply > 0)
        //    FinalScore *= scoreData.FinalScoreMultiply;

        FinalScore *= ScoreMultiplyAmount;
        // �Ҽ��� ������
        FinalScore = Math.Truncate(FinalScore);

        return FinalScore;
    }

    // ���� ī�� �� �� ����
    public void PickRandomCard(bool isSingle = false)
    {
        int pickCount = 1;
        int pickIndex;
        Card curCard;

        if (isSingle == false)
        {
            // ���� ī�� ���� ����
            pickCount = HandSize - (handDeck.Count + selectDeck.Count);
            
            // ���� ī�尡 ���� �� ��ü �������� Ŭ ���,
            if (pickCount > currentDeck.Count)
            {
                pickCount = currentDeck.Count;
            }
        }

        // ī�� �������� �� �� UI���� ����
        for(int i = 0; i < pickCount; i++)
        {
            pickIndex = UnityEngine.Random.Range(0, currentDeck.Count);
            curCard = currentDeck[pickIndex];

            // �ڵ嵦�� ���� ���� ī�� �߰�
            handDeck.Add(curCard);
            // ���給���� ���� ���� ī�� ����
            currentDeck.Remove(curCard);

            // ī�� ������Ʈ ����
            uimanager.MakeHandCard(curCard);
        }

        // ���ھ� �� �ڽ�Ʈ �ؽ�Ʈ ����
        uimanager.ResetScoreCostText();
    }

    // ī�嵥���� ����
    void CardDataGeneratre()
    {
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
                    generatedCard.Index = CurrentCardIndex;
                    CurrentCardIndex++;

                    userDeck.Add(generatedCard);
                    index++;
                }
            }
        }

        // ������
        currentDeck = userDeck;

        // ��ü�� ���� �г� ī�� ������Ʈ ����
        uimanager.GenerateAllDeckCard();
    }

    // �߰� ���� ī�� ����
    public Card MakeAdditionalRandomCard()
    {
        // ���� �ε��� ����
        int rand_pattern = UnityEngine.Random.Range(0,gameData.CardSetting.CardPattern.Count());
        int rand_color = UnityEngine.Random.Range(0, gameData.CardSetting.CardColor.Count());
        int rand_number = UnityEngine.Random.Range(1,gameData.CardSetting.CardNumber + 1);

        // ī�� ������ �Ҵ�
        Card newCard = new Card();
        newCard.colorIndex = rand_color;
        newCard.patternIndex = rand_pattern;
        newCard.number = rand_number;
        newCard.Index = CurrentCardIndex;
        CurrentCardIndex++;

        // ��ü ī�� ���� ��ī�� �߰�
        userDeck.Add(newCard);

        // ��ü ī�� �� UI ���� ����
        uimanager.MakeAdditionalAllDeckCard(newCard);

        // ���ο� ī�� ������ ��ȯ
        return newCard;
    }

    IEnumerator PlayScoreStackSequence()
    {
        //string temp_content;
        Array.Fill(scorestackA, 0);

        yield return new WaitForSeconds(1);

        // ���� �� ��ȸ
        for (int i = 0; i < selectDeck.Count(); i++) 
        {
            //=========================================================================================================
            if(scorestackUIIndex[selectDeck[i].patternIndex] != -1)
            {
                // ���õ� ī�� ���̶���Ʈ
                uimanager.HighLightSelectedCard(i);
                yield return new WaitForSeconds(ScoreStackDelay);

                scorestackA[selectDeck[i].patternIndex] += selectDeck[i].number;
                uimanager.HighLightScoreStack(scorestackUIIndex[selectDeck[i].patternIndex]);
                //temp_content
                //    = scorestackA[selectDeck[i].patternIndex].ToString() + " * "
                //    + scoreStackData[selectDeck[i].patternIndex].b.ToString();// + " ^ "
                //    //+ scoreStackData[selectDeck[i].patternIndex].c.ToString();
                //uimanager.ActiveIndivisualScoreStackText(scorestackUIIndex[selectDeck[i].patternIndex], temp_content);

                yield return new WaitForSeconds(ScoreStackDelay);
            }
            //=========================================================================================================
            if(scorestackUIIndex[PatternCount + selectDeck[i].colorIndex] != -1)
            {
                // ���õ� ī�� ���̶���Ʈ
                uimanager.HighLightSelectedCard(i);
                yield return new WaitForSeconds(ScoreStackDelay);

                scorestackA[PatternCount + selectDeck[i].colorIndex] += selectDeck[i].number;
                uimanager.HighLightScoreStack(scorestackUIIndex[PatternCount + selectDeck[i].colorIndex]);
                //temp_content
                //    = scorestackA[PatternCount + selectDeck[i].colorIndex].ToString() + " * "
                //    + scoreStackData[PatternCount + selectDeck[i].colorIndex].b.ToString();// + " ^ "
                //    //+ scoreStackData[PatternCount + selectDeck[i].colorIndex].c.ToString();
                //uimanager.ActiveIndivisualScoreStackText(scorestackUIIndex[PatternCount + selectDeck[i].colorIndex], temp_content);

                yield return new WaitForSeconds(ScoreStackDelay * 2);
            }
            //=========================================================================================================
            
        }

        int index = 0;
        for (int i = 0; i < 6; i++) 
        {
            if (uimanager.scoreStackPannel[i].activeSelf)
            {
                //while (scoreStackData[index].sum == 0)
                //{
                //    index++;
                //}

                uimanager.HighLightScoreStack(i);
                //temp_content
                //= scoreStackData[index].a.ToString() + " * "
                //+ scoreStackData[index].b.ToString(); //+ " ^ "
                ////+ scoreStackData[index].c.ToString();
                //uimanager.ActiveIndivisualScoreStackText(i, temp_content);

                yield return new WaitForSeconds(ScoreStackDelay);

                uimanager.HighLightScoreStack(i);
                //uimanager.ActiveIndivisualScoreStack(i, Math.Truncate(scoreStackData[index].sum));
                index++;

                yield return new WaitForSeconds(ScoreStackDelay);
            }
        }

        /*for (int i = 0; i < 6; i++) 
        {
            // ������ 0�� �ƴҶ�
            if (scoreStackData[i].isScoreValid)
            {
                if(i < PatternCount)
                {
                    temp_name = gameData.CardSetting.CardPatternName[i];
                }
                else
                {
                    temp_name = gameData.CardSetting.CardColorName[i - 3];
                }

                //uimanager.ActiveScoreStack(temp_name, scoreStackData[i]);

                yield return new WaitForSeconds(0.7f);
            }
        }*/

        // ���� ������ ���� ���� �ջ�
        CurrentScore += SelectedScore;

        // ���ھ� �� �ڽ�Ʈ �ؽ�Ʈ ����
        uimanager.ResetScoreCostText();

        yield return new WaitForSeconds(ScoreStackDelay);

        // ���� ���������� �Ѿ��
        NextAction();
    }
}