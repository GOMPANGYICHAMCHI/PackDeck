//using Palmmedia.ReportGenerator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class UIManager : MonoBehaviour
{
    // ���� �÷ο�
    public MainFlow mainflow;

    // ī�����
    public Color32[] PatternColors;
    // ī�� ���� ��������Ʈ
    public Sprite[] PatternSprites;

    // �޽��� ���� �г�
    public IngameMsgPannel msgPannel;

    [Header("������\n")]
    [Header("�ڵ�ī�� ������")]
    public GameObject HandCardPrefab;
    [Header("����ī�� ������")]
    public GameObject SelectedCardPrefab;
    [Header("��üī�� ������")]
    public GameObject AllCardPrefab;

    [Header("�������� ���� �г� ������")]
    public GameObject ScoreConbinationInfoPanelPrefab;

    [Header("���� ���� 2�� �ؽ�Ʈ �г�")]
    public GameObject ScoreDoublePannel;

    [Header("�����г��ؽ�Ʈ\n")]
    [Header("���� �������� �ؽ�Ʈ")]
    public TMP_Text CurrentStage_txt;
    [Header("��ǥ ���� �ؽ�Ʈ")]
    public TMP_Text GoalScore_txt;
    [Header("���� ���� �ؽ�Ʈ")]
    public TMP_Text CurrentScore_txt;
    [Header("�ൿ ���� �ؽ�Ʈ")]
    public TMP_Text ActionCost_txt;
    [Header("������ ���� �ؽ�Ʈ")]
    public TMP_Text DumpCost_txt;
    [Header("���� �ݵ� �ؽ�Ʈ")]
    public TMP_Text Gold_txt;

    [Header("���� ���õ� ���� �ؽ�Ʈ")]
    public TMP_Text SelectScoreFinal_txt;
    [Header("���õ� ���� �ջ� �ؽ�Ʈ")]
    public TMP_Text SelectScoreAdd_txt;
    [Header("���õ� ���� ���� �ؽ�Ʈ")]
    public TMP_Text SelectScoreMultiply_txt;

    [Header("�ڵ� ������ �ؽ�Ʈ")]
    public TMP_Text HandSize_txt;
    [Header("�÷��� ������ �ؽ�Ʈ")]
    public TMP_Text PlaySize_txt;

    [Header("��������")]
    public TMP_Text[] CombinationScoreTxt;

    //=============================================
    // ���� 
    //=============================================

    // ���׷��̵� ��ư ������
    public GameObject UpgradeBtnPrefab;
    // ���׷��̵� ��ư ���� �迭
    public Color32[] UpgradeBtnColor = new Color32[3];

    // ���׷��̵� ��ư Ȧ����
    public Transform UpgradeHolder1;
    public Transform UpgradeHolder2;
    public Transform UpgradeHolder3;

    // ���� ��ư�� �Ҵ�� ���׷��̵� �ε���
    public int[] UpgradeIndex1 = new int[3];
    // ���� ��ư�� �Ҵ�� ���׷��̵� �ε���
    public int[] UpgradeIndex2 = new int[2];
    // ���� ��ư�� �Ҵ�� ���׷��̵� �ε���
    public int[] UpgradeIndex3 = new int[2];

    // ���� ���� �ؽ�Ʈ
    public TMP_Text txt_ReRollCost;
    static public int ReRollCost = 5;
    public int CurrentReRollCost = ReRollCost;

    // �ٿ�ó ���� �ʱ�ȭ ���� ���� �������� �ؽ�Ʈ
    public TMP_Text txt_LeftStageToResetVoucher;

    //=============================================
    // ��Ŀ �� ����� Ȯ�� ����
    //=============================================

    // ��Ŀ ��ü Ȯ��
    private int[] JockerPercent;

    // ī�� ���� Ȯ��
    private int[] CardPatternPercent;
    // ī�� �÷� Ȯ��
    private int[] CardColorPercent;

    // �ٿ�ó ���� ��������
    public int VoucherResetStage;

    //=============================================

    [Header("�������� ���� ��ư")]
    public Button StageClear_btn;

    [Header("��ü �� �г�")]
    public GameObject allDeckPanel;
    [Header("��ü �� ī��Ȧ��")]
    public Transform allDeckCardHolder;
    public GameObject allDeckCardCloseButton;
    public GameObject allDeckCardMask;

    [Header("�ڵ� ī�� Ȧ��")]
    public Transform handCardHolder;
    [Header("���� ī�� Ȧ��")]  
    public Transform selectCardHolder;
    public List<GameObject> SelectedCard;

    [Header("���� ���� ����â �г� Ȧ��")]
    public Transform ScoreCombinationInfoHolder;
    // ���� ���� ����â ������Ʈ����
    List<ScoreCombinationInfoScript> ScoreCombinationInfoObject;

    [Header("����� �г�")]
    public GameObject storePanel;
    [Header("�������� �г�")]
    public GameObject stagePanel;
    [Header("���� ���� �г�")]
    public GameObject combinationPanel;
    [Header("���ӿ��� �г�")]
    public GameObject gameoverPanel;
    [Header("�������� Ŭ���� �г�")]
    public GameObject stageClearPanel;

    [Header("��Ŀ �� ǥ���ؽ�Ʈ")]
    public TMP_Text txt_JockerCount;
    [Header("��Ŀ �г�")]
    public GameObject[] JockerPannel;
    [Header("��Ŀ ����")]
    public Color32[] JockerColors;
    [Header("��Ŀ ����â ���� ������Ʈ")]
    public Button JockerInfoPannel;
    public RectTransform JockerInfoPannelTrans;
    public TMP_Text txt_JockerInfoName;
    public TMP_Text txt_JockerInfoDescription;
    public TMP_Text txt_JockerInfoSell;
    JockerUIScript CurrentFocusJocker;

    [Header("����� ����")]
    public Image DeBuffPannel;
    public TMP_Text txt_DeBuffName;
    public TMP_Text txt_LeftStage;
    public Color32[] DeBuffColor;

    public Image DeBuffInfoPannel;
    public TMP_Text txt_DeBuffInfoName;
    public TMP_Text txt_DeBuffInfoDescription;

    [Header("��Ŀ 10 ���� ���׷��̵� ���� �׸�")]
    public GameObject FreeUpgradePannel;
    public Button[] btn_FreeUpgrade;
    public int[] FreeUpgradeIndex;

    // ��ǥ����, �������� �г�
    public GameObject[] StageScorePanel;
    // ��ſ� ���׷��̵� �ð�! �г�
    public GameObject StoreScorePanel;

    // ����, ��, ������ ��ư �������� �ϴ� Ŀ��
    public GameObject ButtonCover;

    // ��ü ī�嵦
    Dictionary<Card, AllDeckCardScript> AllDeck = new Dictionary<Card, AllDeckCardScript>();

    // ������Ʈ ����Ʈ
    List<GameObject> HandCards = new List<GameObject>();
    List<GameObject> SelectCards = new List<GameObject>();

    int StageCount = 0;

    // ���ھ� ���� �г�
    public GameObject[] scoreStackPannel = new GameObject[6];

    // ���ĺ� ǥ����� ���� ����Ʈ
    string[] CurrencyUnits = new string[] { "", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp", "cq", "cr", "cs", "ct", "cu", "cv", "cw", "cx", };

    // ��Ŀ Ȯ�� ���� ����
    public void LoadJockerPercentData(int[] inputJockerPercent)
    {
        JockerPercent = inputJockerPercent;
    }

    // ī�� ���� Ȯ�� ����
    public void LoadCardPercentData(int[] inputPatternPercent, int[] inputColorPercent)
    {
        CardPatternPercent = inputPatternPercent;
        CardColorPercent = inputColorPercent;
    }

    // ū�� ��������ǥ��� �Լ�
    public string LargeNumberToString(double number)
    {
        string zero = "0";

        if (-1d < number && number < 1d)
        {
            return zero;
        }

        if (double.IsInfinity(number))
        {
            return "Infinity";
        }

        //  ��ȣ ��� ���ڿ�
        string significant = (number < 0) ? "-" : string.Empty;

        //  ������ ����
        string showNumber = string.Empty;

        //  ���� ���ڿ�
        string unityString = string.Empty;

        //  ������ �ܼ�ȭ ��Ű�� ���� ������ ���� ǥ�������� ������ �� ó��
        string[] partsSplit = number.ToString("E").Split('+');

        //  ����
        if (partsSplit.Length < 2)
        {
            return zero;
        }

        //  ���� (�ڸ��� ǥ��)
        if (!int.TryParse(partsSplit[1], out int exponent))
        {
            Debug.LogWarningFormat("Failed - ToCurrentString({0}) : partSplit[1] = {1}", number, partsSplit[1]);
            return zero;
        }

        //  ���� ���ڿ� �ε���
        int quotient = exponent / 3;

        //  �������� ������ �ڸ��� ��꿡 ���(10�� �ŵ������� ���)
        int remainder = exponent % 3;

        //  1A �̸��� �׳� ǥ��
        if (exponent < 3)
        {
            showNumber = System.Math.Truncate(number).ToString();
        }
        else
        {
            //  10�� �ŵ������� ���ؼ� �ڸ��� ǥ������ ����� �ش�.
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);
            //Debug.Log(temp);

            //  �Ҽ� ù°�ڸ������� ����Ѵ�. dhqjdnj
            showNumber = temp.ToString("F1").Replace(".00", "");
        }

        unityString = CurrencyUnits[quotient];

        return string.Format("{0}{1}{2}", significant, showNumber, unityString);
    }

    // ���� ǥ���
    public string ExponentialNumber(double number)
    {
        string snumber = " ";

        return snumber;
    }

    // ��ü �� ���� �г� ī�� ������Ʈ ����
    public void GenerateAllDeckCard()
    {
        // ���� ����
        Card curData;
        CardBasic generated;

        for (int i = 0; i < mainflow.userDeck.Count; i++) 
        {
            curData = mainflow.userDeck[i];

            // ������Ʈ ����
            generated = Instantiate(AllCardPrefab, allDeckCardHolder).GetComponent<CardBasic>();

            //generated.GetComponent<AllDeckCardScript>().uimanager = this;

            // ī�� ������ ����
            generated.cardData = curData;
            // ī�� ���� ����
            generated.SetCardApear(PatternSprites[curData.patternIndex],PatternColors[curData.colorIndex], curData.number);

            // ��ü �� ī�� ������Ʈ �迭�� �߰�
            AllDeck.Add(curData, generated.GetComponent<AllDeckCardScript>());
        }
    }

    // ��ü �� ������Ʈ ����
    public void DeleteAllDeckCard()
    {
        for (int i = 0; i < allDeckCardHolder.childCount; i++)
        {
            Destroy(allDeckCardHolder.GetChild(i).gameObject);
        }

        AllDeck.Clear();
    }

    // ��ü �� ī�忡�� �߰� ī�� ����
    public void MakeAdditionalAllDeckCard(Card AddData)
    {
        // ���� ����
        CardBasic generated;

        // ������Ʈ ����
        generated = Instantiate(AllCardPrefab, allDeckCardHolder).GetComponent<CardBasic>();

        //generated.GetComponent<AllDeckCardScript>().uimanager = this;

        // ī�� ������ ����
        generated.cardData = AddData;
        // ī�� ���� ����
        generated.SetCardApear(PatternSprites[AddData.patternIndex], PatternColors[AddData.colorIndex], AddData.number);

        // ��ü �� ī�� ������Ʈ �迭�� �߰�
        AllDeck.Add(AddData, generated.GetComponent<AllDeckCardScript>());
    }

    public void StartStage()
    {
        // �� ī�� Ŀ�� �ʱ�ȭ
        ResetDeckCards();

        // ������ UI �ؽ�Ʈ ���� ����
        ResetScoreCostText();

        // �ڵ� ī�� ��Ȱ��ȭ
        DisableAllHandCard();

        // ���õ� ī�� �ʱ�ȭ
        DisableAllSelectCard();

        // ��Ŀ �� �ؽ�Ʈ ����
        ResetJockerCountText();
    }

    // �ڵ� ī�� ������Ʈ ����  
    public void MakeHandCard(Card data)
    {
        HandCardScript generated;

        // ī�� ������Ʈ ����
        generated = Instantiate(HandCardPrefab, handCardHolder).GetComponent<HandCardScript>();
        // ī�� ���� ����
        generated.SetCardApear(PatternSprites[data.patternIndex], PatternColors[data.colorIndex], data.number);
        // ī�� ������ ����
        generated.cardData = data;

        // ���� �� ���� �Լ� ����
        generated.SelectFunc = (() => SelectHandCard(generated));
        generated.UnSelectFunc = (() => UnSelectCard(generated));

        // ��ü �� ���� ���õ� ī�� Ŀ�� Ȱ��ȭ
        AllDeck[data].ControlCover(true);

        // �ڵ�ī�� ������Ʈ ����Ʈ�� ���� ������Ʈ �߰�
        HandCards.Add(generated.gameObject);
    }

    // �ڵ�ī�� ����
    public void SelectHandCard(HandCardScript selfCard)
    {
        // ���� ���õ� ī�尡 �ִ� ������ �ƴҶ�
        if (mainflow.selectDeck.Count < mainflow.PlayableCards)
        {
            // �ڵ嵦���� ���� ī�� ����
            mainflow.handDeck.Remove(selfCard.cardData);
            // ���õ��� ���� ī�� �߰�
            mainflow.selectDeck.Add(selfCard.cardData);
            // ���� �̹��� Ȱ��ȭ
            selfCard.SetSelectPanel();

            // �ڵ� ī�� ������Ʈ ����Ʈ���� ���� ������Ʈ ����
            HandCards.Remove(selfCard.gameObject);
            // ���� ī�� ������Ʈ ����Ʈ�� ���� ������Ʈ �߰�
            SelectCards.Add(selfCard.gameObject);

            selfCard.isOn = true;

            // ���ھ� ���� ������
            mainflow.CheckCurrentScore();
            ActivePreviewScoreStack();
        }
    }

    // ���� ���� ī�� ���� ���� �гο� ����
    public void MakeSelectedCard()
    {
        CardBasic cardBasic;
        SelectedCard.Clear();

        for (int i = 0; i < mainflow.selectDeck.Count(); i++) 
        {
            // ī�� ������Ʈ ����
            cardBasic = Instantiate(SelectedCardPrefab, selectCardHolder).GetComponent<CardBasic>();
            // ī�� ���� ����
            cardBasic.SetCardApear
                (
                PatternSprites[mainflow.selectDeck[i].patternIndex], 
                PatternColors[mainflow.selectDeck[i].colorIndex], 
                mainflow.selectDeck[i].number
                );

            SelectedCard.Add(cardBasic.gameObject);
        }

        for (int j = 0; j < SelectCards.Count; j++)
        {
            // �ڵ�ī�� ������Ʈ ����
            Destroy(SelectCards[j]);
        }
    }

    // ���ھ� ���� ����
    public void DisableAllScoreStack()
    {
        for (int i = 0; i < 6; i++) 
        {
            scoreStackPannel[i].SetActive(false);
        }
    }

    // ���ھ� ���� ������ ( ī�� �ѹ� �ջ� ������ )
    public void ActivePreviewScoreStack()
    {
        DisableAllScoreStack();
        //int index = 0;
        //float temp = 0;

        //for (int i = 0; i < 6; i++)
        //{
        //    if (mainflow.scoreStackData[i].sum != 0)
        //    {
        //        scoreStackPannel[index].SetActive(true);
        //
        //        // ���� ��Ī
        //        if (i < mainflow.PatternCount)
        //        {
        //            scoreStackPannel[index].transform.GetChild(0).GetComponent<TMP_Text>().text = mainflow.gameData.CardSetting.CardPatternName[i];
        //            temp = mainflow.scoreData.additional_PatternA[i];
        //        }
        //        else
        //        {
        //            scoreStackPannel[index].transform.GetChild(0).GetComponent<TMP_Text>().text = mainflow.gameData.CardSetting.CardColorName[i - mainflow.PatternCount];
        //            temp = mainflow.scoreData.additional_ColorA[i - mainflow.PatternCount];
        //        }
        //
        //        if(temp != 0)
        //        {
        //            // ����
        //            scoreStackPannel[index].transform.GetChild(1).GetComponent<TMP_Text>().text = "((" + temp.ToString() + ") + " +
        //                "?) * " + mainflow.scoreStackData[i].b.ToString();// + " ^ " + mainflow.scoreStackData[i].c.ToString();
        //        }
        //        else
        //        {
        //            // ����
        //            scoreStackPannel[index].transform.GetChild(1).GetComponent<TMP_Text>().text =
        //                "? * " + mainflow.scoreStackData[i].b.ToString();// + " ^ " + mainflow.scoreStackData[i].c.ToString();
        //        }
        //
        //        index++;
        //    }
        //}
    }

    // ���õ� ī�� ���̶���Ʈ
    public void HighLightSelectedCard(int index)
    {
        SelectedCard[index].GetComponent<Animator>().SetTrigger("IsOn");
    }

    // ���ھ� ���� ���̶���Ʈ
    public void HighLightScoreStack(int index)
    {
        scoreStackPannel[index].GetComponent<Animator>().SetTrigger("IsOn");
    }

    // ���ھ� ���� ���� �ؽ�Ʈ ����
    public void ActiveIndivisualScoreStack(int index, double content)
    {
        //scoreStackPannel[index].transform.GetChild(1).GetComponent<TMP_Text>().text = LargeNumberToString(content);
    }

    // ���ھ� ���� ���� �ؽ�Ʈ ����
    public void ActiveIndivisualScoreStackText(int index, string content)
    {
        scoreStackPannel[index].transform.GetChild(1).GetComponent<TMP_Text>().text = content;
    }

    // ���ھ� ���� Ȱ��ȭ �� �ؽ�Ʈ ����
    public void ActiveScoreStack(string scoreName)
    {
        /*int index = 0;

        // ��Ȱ��ȭ�� ������Ʈ Ž��
        for (int i = 0; i < 6; i++)
        {
            if(scoreStackPannel[i].activeSelf == false)
            {
                index = i;
                break;
            }
        }

        // ������Ʈ �ؽ�Ʈ ����
        scoreStackPannel[index].transform.GetChild(0).GetComponent<TMP_Text>().text = scoreName;
        scoreStackPannel[index].transform.GetChild(1).GetComponent<TMP_Text>().text = 
            stackData.a.ToString() + " * " + stackData.b.ToString() + " ^ " + stackData.c.ToString();

        // ������Ʈ Ȱ��ȭ
        scoreStackPannel[index].SetActive(true);*/
    }

    // ��� �ؽ�Ʈ ����
    public void ResetGoldText()
    {
        Gold_txt.text = mainflow.CurrentGold.ToString();
    }

    // ���� ���� ī�� ������
    public void SelectCardDump()
    {
        if(mainflow.selectDeck.Count() == 0)
        {
            msgPannel.PrintMessage("���õ� ī�尡 �����ϴ�!");
            return;
        }

        if(mainflow.CurrentDumpCost > 0)
        {
            // ������Ʈ ����
            for (int i = 0; i < SelectCards.Count; i++) 
            {
                // ������Ʈ ����
                Destroy(SelectCards[i]);
            }

            // ������ �ڽ�Ʈ ����
            mainflow.CurrentDumpCost -= 1;
            ResetScoreCostText();

            // �� �ʱ�ȭ
            SelectCards.Clear();

            // ���� �� �ʱ�ȭ
            mainflow.selectDeck.Clear();

            // ī�� �����
            mainflow.PickRandomCard();

            // ���ھ� ���� ������
            mainflow.CheckCurrentScore();
            ActivePreviewScoreStack();
        }
        else
        {
            msgPannel.PrintMessage("������ �ڽ�Ʈ�� �����մϴ�!");
        }
    }

    // ���õ� ī�� öȸ
    public void UnSelectCard(HandCardScript selfCard)
    {
        // ���õ� ���� ���� ī�� ����
        mainflow.selectDeck.Remove(selfCard.cardData);
        // �ڵ嵦�� ���� ī�� �߰�
        mainflow.handDeck.Add(selfCard.cardData);
        // ���� �̹��� ��Ȱ��ȭ
        selfCard.SetSelectPanel();

        // �ڵ� ī�� ������Ʈ ����Ʈ���� ���� ������Ʈ ����
        SelectCards.Remove(selfCard.gameObject);
        // ���� ī�� ������Ʈ ����Ʈ�� ���� ������Ʈ �߰�
        HandCards.Add(selfCard.gameObject);

        selfCard.isOn = false;

        // ���ھ� ���� ������
        mainflow.CheckCurrentScore();
        ActivePreviewScoreStack();
    }

    // ���� �� ���� �ڽ�Ʈ, �������� �ؽ�Ʈ ����
    public void ResetScoreCostText()
    {
        CurrentScore_txt.text = LargeNumberToString(mainflow.CurrentScore);
        GoalScore_txt.text = LargeNumberToString(mainflow.GoalScore);

        ActionCost_txt.text = mainflow.CurrentActionCost.ToString();
        DumpCost_txt.text = mainflow.CurrentDumpCost.ToString();
        CurrentStage_txt.text = mainflow.CurrentStage.ToString();

        HandSize_txt.text = mainflow.HandSize.ToString();
        PlaySize_txt.text = mainflow.PlayableCards.ToString();
    }

    // �� ���� â �ʱ�ȭ
    public void ResetDeckCards()
    {
        int leng = allDeckCardHolder.childCount;

        for(int i = 0; i < leng; i++)
        {
            allDeckCardHolder.GetChild(i).GetComponent<AllDeckCardScript>().ControlCover(false);
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
                Destroy(selectCardHolder.GetChild(i).gameObject);
            }
        }
    }

    // ��� �ڵ�ī�� ��Ȱ��ȭ
    public void DisableAllHandCard()
    {
        // �ڵ�ī�� �ڼհ��� �ҷ�����
        int count = handCardHolder.childCount;

        if (count != 0)
        {
            for (int i = 0; i < count; i++) 
            {
                Destroy(handCardHolder.GetChild(i).gameObject);
            }
        }
    }

    // ����� ���� Ȥ�� ����� ���� �г� �ѱ�/����
    public void SwitchStoreScorePanel(bool isStore)
    {
        for(int i = 0; i < StageScorePanel.Length; i++)
        {
            StageScorePanel[i].SetActive(!isStore);
        }

        StoreScorePanel.SetActive(isStore);
    }

    public void StageClearToNext()
    {
        //if(mainflow.CurrentStage % mainflow.gameData.StorePerStage == 0)
        //{
        //    stageClearPanel.SetActive(false);
        //    stagePanel.SetActive(false);
        //    storePanel.SetActive(true);
        //    DestroyAllUpgradeBtn();
        //    RandomStoreUpgrade();
        //}
        //else
        //{
        //    stageClearPanel.SetActive(false);
        //    stagePanel.SetActive(true);
        //    mainflow.StartStage();
        //}
    }

    public void StoreToStage()
    {
        storePanel.SetActive(false);
        stagePanel.SetActive(true);
        SwitchStoreScorePanel(false);
        DestroyAllUpgradeBtn();

        mainflow.StartStage();
    }

    // ����� ���׷��̵� ���� ���� �� UI ����
    private void RandomStoreUpgrade()
    {
        // ���� �ڽ�Ʈ �ʱ�ȭ
        CurrentReRollCost = ReRollCost;
        // ���� ���� �ؽ�Ʈ ����
        txt_ReRollCost.text = "���� " + CurrentReRollCost.ToString() + "���";

        int scorePercentSum = 0;
        List<int> scorePercent = new List<int>();

        // ���� Ǯ
        List<int> pool_score = new List<int>();
        List<int> pool_action = new List<int>();
        List<int> pool_singleuse = new List<int>();

        // ���� �� �÷� Ǯ ����
        for (int i = 0; i < mainflow.PatternCount; i++)
        {
            if (mainflow.PatternLevel[i] < 100)
            {
                pool_score.Add(i);
                scorePercentSum += CardPatternPercent[i];
                scorePercent.Add(CardPatternPercent[i]);
            }
        }
        // �÷�
        for (int i = 0; i < mainflow.ColorCount; i++)
        {
            if (mainflow.ColorLevel[i] < 100)
            {
                pool_score.Add(i + mainflow.PatternCount);
                scorePercentSum += CardColorPercent[i];
                scorePercent.Add(CardColorPercent[i]);
            }
        }
        // ��Ŀ Ǯ ����
        for (int i = 0; i < mainflow.JockerData.Count(); i++) 
        {
            if(!mainflow.PlayerJocker.Contains(mainflow.JockerData[i]))
            {
                pool_score.Add(i + mainflow.PatternCount + mainflow.ColorCount);
                scorePercentSum += JockerPercent[i];
                scorePercent.Add(JockerPercent[i]);
            }
        }

        //=============================================

        Button temp_btn;
        BtnIndex temp_btnindex;
        int temp_index;
        int temp_sum;

        // ���� ���� ���׷��̵� ����
        for (int i = 0; i < 3; i++)
        {
            // ���� �ε��� ����
            temp_index = UnityEngine.Random.Range(0, scorePercentSum);

            temp_sum = 0;

            // ��ü Ȯ�� ��ȸ
            for (int j = 0; j < scorePercent.Count(); j++) 
            {
                temp_sum += scorePercent[j];

                if (temp_index < temp_sum)
                {
                    temp_index = pool_score[j];

                    scorePercentSum -= scorePercent[j];
                    scorePercent.RemoveAt(j);
                    break;
                }
            }

            // ����� �ε��� Ǯ���� ����
            pool_score.Remove(temp_index);

            // ���׷��̵� �ε��� �Ҵ�
            UpgradeIndex1[i] = temp_index;

            // ��ư ������Ʈ ����
            temp_btn = Instantiate(UpgradeBtnPrefab, UpgradeHolder1).GetComponent<Button>();
            // ��ư ���� ����
            temp_btn.GetComponent<Image>().color = UpgradeBtnColor[0];

            
            temp_btnindex = temp_btn.GetComponent<BtnIndex>();
            // �ε��� ����
            temp_btnindex.Index = i;
            temp_btnindex.Type = 0;
            temp_btnindex.uimanager = this;

            // ���� �϶�
            if (temp_index < 3)
            {
                string temp = " ";

                // ��Ī ����
                temp = mainflow.gameData.CardSetting.CardPatternName[temp_index];

                temp_btn.transform.GetChild(0).GetComponent<TMP_Text>().text
                = temp + " ����ġ +1";

                //temp_btn.transform.GetChild(1).GetComponent<TMP_Text>().text
                //= "���� ����ġ : " + mainflow.CardPatternScore[temp_index];
            }
            // �÷� �϶�
            else if (temp_index < 6)
            {
                string temp = " ";

                // ��Ī ����
                temp = mainflow.gameData.CardSetting.CardColorName[temp_index - mainflow.PatternCount];

                temp_btn.transform.GetChild(0).GetComponent<TMP_Text>().text
                = temp + " ����ġ +1";

                //temp_btn.transform.GetChild(1).GetComponent<TMP_Text>().text
                //= "���� ����ġ : " + mainflow.CardColorScore[temp_index - 3];
            }
            // ��Ŀ �϶�
            else
            {
                string temp = " ";

                //temp = "��Ŀ / " + mainflow.JockerData[temp_index - (mainflow.PatternCount + mainflow.ColorCount)].Name;

                temp_btn.transform.GetChild(0).GetComponent<TMP_Text>().text
                = temp;

                //temp_btn.transform.GetChild(1).GetComponent<TMP_Text>().text
                //= mainflow.JockerData[temp_index - (mainflow.PatternCount + mainflow.ColorCount)].Description;
            }

            // ���� �ؽ�Ʈ ����
            if (temp_index < 6)
            {
                temp_btn.transform.GetChild(2).GetComponent<TMP_Text>().text
                = "10���";
            }
            else
            {
                //temp_btn.transform.GetChild(2).GetComponent<TMP_Text>().text
                //= mainflow.JockerData[temp_index - (mainflow.PatternCount + mainflow.ColorCount)].Cost + "���";
            }

            if (pool_score.Count <= 0 )
                break;
        }

        //Debug.Log(VoucherResetStage);

        StageCount++;

        // �ൿ ���׷��̵�, 3�������� ���� �����Ǹ�, 4�������� ���� �� �ʱ�ȭ
        if (StageCount == VoucherResetStage || mainflow.CurrentStage == 1)
        {
            StageCount = 0;
            //txt_LeftStageToResetVoucher.gameObject.SetActive(false);

            // ������Ʈ ����
            for (int i = 0; i < UpgradeHolder2.childCount; i++)
            {
                Destroy(UpgradeHolder2.GetChild(i).gameObject);
            }

            // �ൿ Ǯ ����
            if (mainflow.HandSize < 8)
            {
                pool_action.Add(0);
            }
            if (mainflow.PlayableCards < 6)
            {
                pool_action.Add(1);
            }
            if (mainflow.ActionCost < 8)
            {
                pool_action.Add(2);
            }
            if (mainflow.DumpCost < 8)
            {
                pool_action.Add(3);
            }

            // �ൿ ���׷��̵� ����
            if (pool_action.Count > 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    // ���� �ε��� ����
                    temp_index = pool_action[UnityEngine.Random.Range(0, pool_action.Count)];

                    // ����� �ε��� Ǯ���� ����
                    pool_action.Remove(temp_index);

                    // ���׷��̵� �ε��� �Ҵ�
                    UpgradeIndex2[i] = temp_index;

                    // ��ư ������Ʈ ����
                    temp_btn = Instantiate(UpgradeBtnPrefab, UpgradeHolder2).GetComponent<Button>();
                    // ��ư ���� ����
                    temp_btn.GetComponent<Image>().color = UpgradeBtnColor[1];

                    temp_btn.GetComponent<BtnIndex>().Index = i;

                    temp_btnindex = temp_btn.GetComponent<BtnIndex>();
                    // �ε��� ����
                    temp_btnindex.Index = i;
                    temp_btnindex.Type = 1;
                    temp_btnindex.uimanager = this;

                    string temp = " ";
                    temp_sum = -419;

                    // ���� ��Ī
                    switch (temp_index)
                    {
                        case 0:
                            {
                                temp = "�ڵ� ũ��";
                                temp_sum = mainflow.HandSize;
                                break;
                            }
                        case 1:
                            {
                                temp = "�÷��� ũ��";
                                temp_sum = mainflow.PlayableCards;
                                break;
                            }
                        case 2:
                            {
                                temp = "�ൿ Ƚ��";
                                temp_sum = mainflow.ActionCost;
                                break;
                            }
                        case 3:
                            {
                                temp = "������ Ƚ��";
                                temp_sum = mainflow.DumpCost;
                                break;
                            }
                    }

                    temp_btn.transform.GetChild(0).GetComponent<TMP_Text>().text
                    = temp + " +1";

                    temp_btn.transform.GetChild(1).GetComponent<TMP_Text>().text
                    = "���� " + temp + " : " + temp_sum.ToString();

                    temp_btn.transform.GetChild(2).GetComponent<TMP_Text>().text
                    = "20���";

                    if (pool_action.Count <= 0)
                        break;
                }
            }
        }

        // �ٿ�ó ���� ���� �������� �ؽ�Ʈ �ʱ�ȭ
        txt_LeftStageToResetVoucher.text = (VoucherResetStage - StageCount).ToString() + "�������� ���� ������Ʈ";

        // ��ȸ�� Ǯ ����
        for (int i = 0; i < 4; i++)
            {
                pool_singleuse.Add(i);
            }

        // ��ȸ�� ���׷��̵� 
        for (int i = 0; i < 2; i++)
            {
                // ���� �ε��� ����
                temp_index = pool_singleuse[UnityEngine.Random.Range(0, pool_singleuse.Count)];

                // ����� �ε��� Ǯ���� ����
                pool_singleuse.Remove(temp_index);

                // ���׷��̵� �ε��� �Ҵ�
                UpgradeIndex3[i] = temp_index;

                // ��ư ������Ʈ ����
                temp_btn = Instantiate(UpgradeBtnPrefab, UpgradeHolder3).GetComponent<Button>();
                // ��ư ���� ����
                temp_btn.GetComponent<Image>().color = UpgradeBtnColor[2];

                temp_btn.GetComponent<BtnIndex>().Index = i;

                temp_btnindex = temp_btn.GetComponent<BtnIndex>();
                // �ε��� ����
                temp_btnindex.Index = i;
                temp_btnindex.Type = 2;
                temp_btnindex.uimanager = this;

                string temp = " ";
                string temp2 = " ";

                int goldamount = 0;

                // ���� ��Ī
                switch (temp_index)
                {
                    case 0:
                        {
                            temp = "�� ����";
                            temp2 = "�� ���� ī�� ���� ����";
                            goldamount = 15;
                            break;
                        }
                    case 1:
                        {
                            temp = "�� �߰�";
                            temp2 = "���� ������ ī�� ���� �߰�";
                            goldamount = 15;
                            break;
                        }
                    case 2:
                        {
                            temp = "���� ���� 2��";
                            temp2 = "���� ���� ���� ���� ���� 2��";
                            goldamount = 10;
                            break;
                        }
                    case 3:
                        {
                            temp = "���� ��� 2��";
                            temp2 = "( �ִ� 40 )";
                            goldamount = 10;
                            break;
                        }
                }

                temp_btn.transform.GetChild(0).GetComponent<TMP_Text>().text
                = temp;

                temp_btn.transform.GetChild(1).GetComponent<TMP_Text>().text
                = temp2;

                temp_btn.transform.GetChild(2).GetComponent<TMP_Text>().text
                = goldamount.ToString() + "���";

                if (pool_singleuse.Count <= 0)
                    break;
            }
    }

    // ����
    public void ReRoll()
    {
        // ��� ����
        if (mainflow.CurrentGold < CurrentReRollCost)
        {
            msgPannel.PrintMessage("��尡 �����մϴ�");
            return;
        }

        // ��� ����
        mainflow.CurrentGold -= CurrentReRollCost;
        CurrentReRollCost++;
        // ���� ���� �ؽ�Ʈ ����
        txt_ReRollCost.text = "���� " + CurrentReRollCost.ToString() + "���";
        // ��� �ؽ�Ʈ ����
        ResetGoldText();

        // ���� ������Ʈ ����
        for (int i = 0; i < UpgradeHolder1.childCount; i++)
        {
            Destroy(UpgradeHolder1.GetChild(i).gameObject);
        }

        // ���� Ǯ
        List<int> pool_score = new List<int>();

        // ���� �� �÷� Ǯ ����
        for (int i = 0; i < mainflow.PatternCount; i++)
        {
            if (mainflow.PatternLevel[i] < 100)
            {
                pool_score.Add(i);
            }
        }
        // �÷�
        for (int i = 0; i < mainflow.ColorCount; i++)
        {
            if (mainflow.ColorLevel[i] < 100)
            {
                pool_score.Add(i + mainflow.PatternCount);
            }
        }
        // ��Ŀ Ǯ ����
        for (int i = 0; i < mainflow.JockerData.Count(); i++)
        {
            if (!mainflow.PlayerJocker.Contains(mainflow.JockerData[i]))
            {
                pool_score.Add(i + mainflow.PatternCount + mainflow.ColorCount);
            }
        }

        //=============================================

        Button temp_btn;
        BtnIndex temp_btnindex;
        int temp_index;

        // ���� ���� ���׷��̵� ����
        for (int i = 0; i < 3; i++)
        {
            // ���� �ε��� ����
            temp_index = pool_score[UnityEngine.Random.Range(0, pool_score.Count)];

            // ����� �ε��� Ǯ���� ����
            pool_score.Remove(temp_index);

            // ���׷��̵� �ε��� �Ҵ�
            UpgradeIndex1[i] = temp_index;

            // ��ư ������Ʈ ����
            temp_btn = Instantiate(UpgradeBtnPrefab, UpgradeHolder1).GetComponent<Button>();
            // ��ư ���� ����
            temp_btn.GetComponent<Image>().color = UpgradeBtnColor[0];


            temp_btnindex = temp_btn.GetComponent<BtnIndex>();
            // �ε��� ����
            temp_btnindex.Index = i;
            temp_btnindex.Type = 0;
            temp_btnindex.uimanager = this;

            // ���� �϶�
            if (temp_index < 3)
            {
                string temp = " ";

                // ��Ī ����
                temp = mainflow.gameData.CardSetting.CardPatternName[temp_index];

                temp_btn.transform.GetChild(0).GetComponent<TMP_Text>().text
                = temp + " ����ġ +1";

                //temp_btn.transform.GetChild(1).GetComponent<TMP_Text>().text
                //= "���� ����ġ : " + mainflow.CardPatternScore[temp_index];
            }
            // �÷� �϶�
            else if (temp_index < 6)
            {
                string temp = " ";

                // ��Ī ����
                temp = mainflow.gameData.CardSetting.CardColorName[temp_index - mainflow.PatternCount];

                temp_btn.transform.GetChild(0).GetComponent<TMP_Text>().text
                = temp + " ����ġ +1";

                //temp_btn.transform.GetChild(1).GetComponent<TMP_Text>().text
                //= "���� ����ġ : " + mainflow.CardColorScore[temp_index - 3];
            }
            // ��Ŀ �϶�
            else
            {
                string temp = " ";

                //temp = "��Ŀ / " + mainflow.JockerData[temp_index - (mainflow.PatternCount + mainflow.ColorCount)].Name;

                temp_btn.transform.GetChild(0).GetComponent<TMP_Text>().text
                = temp;

                //temp_btn.transform.GetChild(1).GetComponent<TMP_Text>().text
                //= mainflow.JockerData[temp_index - (mainflow.PatternCount + mainflow.ColorCount)].Description;
            }

            // ���� �ؽ�Ʈ ����
            if (temp_index < 6)
            {
                temp_btn.transform.GetChild(2).GetComponent<TMP_Text>().text
                = "10���";
            }
            else
            {
                //temp_btn.transform.GetChild(2).GetComponent<TMP_Text>().text
                //= mainflow.JockerData[temp_index - (mainflow.PatternCount + mainflow.ColorCount)].Cost + "���";
            }

            if (pool_score.Count <= 0)
                break;
        }
    }

    // ���׷��̵� ��ư ������Ʈ ��ü ����
    public void DestroyAllUpgradeBtn()
    {
        for (int i = 0; i < UpgradeHolder1.childCount; i++) 
        {
            Destroy(UpgradeHolder1.GetChild(i).gameObject);
        }
        //for (int i = 0; i < UpgradeHolder2.childCount; i++)
        //{
        //    Destroy(UpgradeHolder2.GetChild(i).gameObject);
        //}
        for (int i = 0; i < UpgradeHolder3.childCount; i++)
        {
            Destroy(UpgradeHolder3.GetChild(i).gameObject);
        }
    }

    // ���� ���� �� ��Ŀ ���׷��̵�
    public void ScoreUpgrade(int index, Button selfBtn)
    {
        // ��� ����
        if (UpgradeIndex1[index] < 6)
        {
            if(mainflow.CurrentGold < 10)
            {
                msgPannel.PrintMessage("��尡 �����մϴ�");
                return;
            }
        }
        //else if (mainflow.CurrentGold < mainflow.JockerData[UpgradeIndex1[index] - (mainflow.PatternCount + mainflow.ColorCount)].Cost)
        //{
        //    msgPannel.PrintMessage("��尡 �����մϴ�");
        //    return;
        //}

        // ��Ŀ ���� �ʰ�
        if(UpgradeIndex1[index] >= 6 && mainflow.PlayerJocker.Count() == 5)
        {
            msgPannel.PrintMessage("��Ŀ�� �̹� 5�� �Դϴ�");
            return;
        }

        // ���� �϶�
        if (UpgradeIndex1[index] < 3)
        {
            //mainflow.CardPatternScore[UpgradeIndex1[index]] += 1;
            mainflow.PatternLevel[UpgradeIndex1[index]] += 1;
        }
        // �÷� �϶�
        else if (UpgradeIndex1[index] < 6)
        {
            //mainflow.CardColorScore[UpgradeIndex1[index] - 3] += 1;
            mainflow.ColorLevel[UpgradeIndex1[index] - 3] += 1;
        }
        // ��Ŀ �϶�
        else
        {
            mainflow.PlayerJocker.Add(mainflow.JockerData[UpgradeIndex1[index] - (mainflow.PatternCount + mainflow.ColorCount)]);
            // ���� �� �̺�Ʈ ȣ��
            //mainflow.JockerData[UpgradeIndex1[index] - (mainflow.PatternCount + mainflow.ColorCount)].Event_GetItem(TODO);
            EnableJockerUI(UpgradeIndex1[index] - (mainflow.PatternCount + mainflow.ColorCount));
        }

        // ��� ����
        if(UpgradeIndex1[index] < 6)
        {
            mainflow.CurrentGold -= 10;
        }
        else
        {
            //mainflow.CurrentGold -= mainflow.JockerData[UpgradeIndex1[index] - (mainflow.PatternCount + mainflow.ColorCount)].Cost;
        }

        SetCombinationScoresText();
        ResetGoldText();

        // ��ư ��Ȱ��ȭ
        UnActiveBtn(selfBtn);
    }
    
    // �ൿ ���׷��̵�
    public void ActionUpgrade(int index, Button selfBtn)
    {
        if (mainflow.CurrentGold < 20)
        {
            msgPannel.PrintMessage("��尡 �����մϴ�");
        }
        else
        {
            switch (UpgradeIndex2[index])
            {
                case 0:
                    mainflow.HandSize++;
                    break;
                case 1:
                    mainflow.PlayableCards++;
                    break;
                case 2:
                    mainflow.ActionCost++;
                    break;
                case 3:
                    mainflow.DumpCost++;
                    break;
            }

            mainflow.CurrentGold -= 20;
            ResetGoldText();

            // ��ư ��Ȱ��ȭ
            UnActiveBtn(selfBtn);
        }
    }

    // ��ȸ�� ���׷��̵�
    public void SingleUseUpgrade(int index, Button selfBtn)
    {
        if(mainflow.CurrentGold < 10)
        {
            msgPannel.PrintMessage("��尡 �����մϴ�");
        }
        else if((mainflow.CurrentGold < 15) && (UpgradeIndex3[index] == 0 || UpgradeIndex3[index] == 1))
        {
            msgPannel.PrintMessage("��尡 �����մϴ�");
        }
        else
        {
            switch (UpgradeIndex3[index])
            {
                // �� ����
                case 0:
                    Debug.Log("�� ����");
                    // ��ü �� �г� ON
                    ControlDeckPanel(true);

                    // Ŀ�� �� �ݱ� ��ư ��Ȱ��ȭ
                    allDeckCardMask.SetActive(false);
                    allDeckCardCloseButton.SetActive(false);

                    // ��� ����
                    mainflow.CurrentGold -= 15;
                    break;

                // �� �߰�
                case 1:
                    Debug.Log("�� �߰�");
                    // ���� ī�� �߰�
                    Card newCard = mainflow.MakeAdditionalRandomCard();

                    string msg = mainflow.gameData.CardSetting.CardColorName[newCard.colorIndex] + " " +
                        mainflow.gameData.CardSetting.CardPatternName[newCard.patternIndex] + " " + 
                        newCard.number + " ī�� �߰� �Ϸ�";

                    msgPannel.PrintMessage(msg);

                    // ��� ����
                    mainflow.CurrentGold -= 15;
                    break;

                // ���� ���� 2��
                case 2:
                    Debug.Log("�������� 2��");
                    ScoreDoublePannel.SetActive(true);
                    // ���� ���� 2�� ����
                    mainflow.ScoreMultiplyAmount = 2;

                    // ��� ����
                    mainflow.CurrentGold -= 10;
                    break;

                // ���� ��� 2��
                case 3:
                    Debug.Log("��� 2��");
                    msgPannel.PrintMessage("���� ��差�� 2�谡 �Ǿ����ϴ�");

                    // ��� ����
                    mainflow.CurrentGold -= 10;
                    // ��� 2��
                    mainflow.CurrentGold = mainflow.CurrentGold * 2;
                    break;
            }

            ResetGoldText();

            // ��ư ��Ȱ��ȭ
            UnActiveBtn(selfBtn);
        }
    }

    // ����� UI ����
    public void ResetDeBuffUI()
    {
        //txt_DeBuffName.text = mainflow.CurrentDeBuff.Name;
        if(mainflow.CurrentStage % 3 == 0)
        {
            txt_LeftStage.text = "����� �����";
        }
        else
        {
            txt_LeftStage.text = (3 - (mainflow.CurrentStage % 3)).ToString() + "�������� ����";
        }
       // DeBuffPannel.color = DeBuffColor[mainflow.CurrentDeBuff.index];

        //txt_DeBuffInfoName.text = mainflow.CurrentDeBuff.Name;
       //txt_DeBuffInfoDescription.text = mainflow.CurrentDeBuff.Description;
        //DeBuffInfoPannel.color = DeBuffColor[mainflow.CurrentDeBuff.index];
    }

    // ����� ���� â Ȱ��ȭ, ��Ȱ��ȭ
    public void SetActiveDeBuffInfoPannel(bool isActive)
    {
        DeBuffInfoPannel.gameObject.SetActive(isActive);

        if(isActive)
        {
            Vector3 temp_pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

            // ������Ʈ ��ġ ����
            DeBuffInfoPannel.transform.position = temp_pos;
        }
    }

    // ��Ŀ ����â Ȱ��ȭ �Լ�
    public void EnableJockerInfoPannel(JockerUIScript selfJocker)
    {
        // ������Ʈ Ȱ��ȭ
        JockerInfoPannel.gameObject.SetActive(true);

        // �гλ��� ����
        JockerInfoPannel.GetComponent<Image>().color = JockerColors[selfJocker.Index];

        // �ؽ�Ʈ ����
        //txt_JockerInfoName.text = mainflow.JockerData[selfJocker.Index].Name;
        //txt_JockerInfoDescription.text = mainflow.JockerData[selfJocker.Index].Description;
        //txt_JockerInfoSell.text = mainflow.JockerData[selfJocker.Index].SellCost.ToString() + "��忡 �Ǹ�";

        Vector3 temp_pos = new Vector3(Input.mousePosition.x + 150, Input.mousePosition.y - 200, 0);

        // ������Ʈ ��ġ ����
        JockerInfoPannelTrans.position = temp_pos;

        // ���� �ε��� ����
        CurrentFocusJocker = selfJocker;
    }

    // ��Ŀ ����â ��Ȱ��ȭ �Լ�
    public void DisableJockerInfoPannel()
    {
        // ������Ʈ ��Ȱ��ȭ
        JockerInfoPannel.gameObject.SetActive(false);
    }

    // ��Ŀ �Ǹ�
    public void SellJocker()
    {
        // ��Ŀ ���� â ��Ȱ��ȭ
        DisableJockerInfoPannel();
        // ��Ŀ UI ��Ȱ��ȭ
        DisableJockerUI(CurrentFocusJocker.Index);

        // ��� �ؽ�Ʈ ������Ʈ
        ResetGoldText();
        // ��Ŀ �� �ؽ�Ʈ ����
        ResetJockerCountText();
    }

    // ��Ŀ �� ǥ�� �ؽ�Ʈ ����
    public void ResetJockerCountText()
    {
        txt_JockerCount.text = mainflow.PlayerJocker.Count().ToString() + " / 5";
    }

    // ��Ŀ UI ����
    public void EnableJockerUI(int index)
    {
        for (int i = 0; i < 5; i++) 
        {
            if (!JockerPannel[i].activeSelf)
            {
                JockerPannel[i].SetActive(true);
                JockerPannel[i].GetComponent<Image>().color = JockerColors[index]; 
                //JockerPannel[i].transform.GetChild(0).GetComponent<TMP_Text>().text
                //    = mainflow.JockerData[index].Name;
                JockerPannel[i].GetComponent<JockerUIScript>().Index = index;

                // ��Ŀ �� ī��Ʈ �ؽ�Ʈ �ʱ�ȭ
                ResetJockerCountText();

                return;
            }
        }
    }

    // ��Ŀ UI ����, �����÷ο� �÷��̾� ��Ŀ �����Ϳ��� ����
    public void DisableJockerUI(int index,bool isSell = true)
    {
        for (int i = 0; i < 5; i++)
        {
            if (JockerPannel[i].activeSelf)
            {
                if(JockerPannel[i].GetComponent<JockerUIScript>().Index == index)
                {
                    JockerPannel[i].SetActive(false);
                    mainflow.PlayerJocker.Remove(mainflow.JockerData[index]);
                    //mainflow.JockerData[index].Event_Sell(TODO);
                    break;
                }
            }
        }

        if(isSell)
        {
            //mainflow.CurrentGold += mainflow.JockerData[index].SellCost;
        }
    }

    // ��Ŀ ����ȭ �г� ����
    public int SetActiveJockerDisablePannel(bool isActive, int index)
    {
        for (int i = 0; i < 5; i++)
        {
            if (JockerPannel[i].activeSelf)
            {
                if (JockerPannel[i].GetComponent<JockerUIScript>().Index == index)
                {
                    JockerPannel[i].GetComponent<JockerUIScript>().SetActiveDisablePannel(isActive);
                    return i;
                }
            }
        }
        return -1;
    }

    // ��Ŀ 10 ���� ���׷��̵� �г� Ȱ��ȭ
    public void EnableFreeUpgradePannel()
    {
        // �г� ������Ʈ Ȱ��ȭ
        FreeUpgradePannel.SetActive(true);

        btn_FreeUpgrade[0].gameObject.SetActive(false);
        btn_FreeUpgrade[1].gameObject.SetActive(false);
        btn_FreeUpgrade[2].gameObject.SetActive(false);

        // ���� Ǯ
        List<int> pool_score = new List<int>();

        // ���� �� �÷� Ǯ ����
        for (int i = 0; i < mainflow.PatternCount; i++)
        {
            if (mainflow.PatternLevel[i] < 100)
            {
                pool_score.Add(i);
            }
        }
        // �÷�
        for (int i = 0; i < mainflow.ColorCount; i++)
        {
            if (mainflow.ColorLevel[i] < 100)
            {
                pool_score.Add(i + mainflow.PatternCount);
            }
        }

        Button temp_btn;
        int temp_index;

        // ���� ���� ���׷��̵� ����
        for (int i = 0; i < 3; i++)
        {
            // ���� �ε��� ����
            temp_index = pool_score[UnityEngine.Random.Range(0, pool_score.Count)];

            // ����� �ε��� Ǯ���� ����
            pool_score.Remove(temp_index);

            // ���׷��̵� �ε��� �Ҵ�
            FreeUpgradeIndex[i] = temp_index;

            // ��ư ������Ʈ 
            temp_btn = btn_FreeUpgrade[i];

            // ���� �϶�
            if (temp_index < 3)
            {
                string temp = " ";

                // ��Ī ����
                temp = mainflow.gameData.CardSetting.CardPatternName[temp_index];

                temp_btn.transform.GetChild(0).GetComponent<TMP_Text>().text
                = temp + " ����ġ +1";

                //temp_btn.transform.GetChild(1).GetComponent<TMP_Text>().text
                //= "���� ����ġ : " + mainflow.CardPatternScore[temp_index];
            }
            // �÷� �϶�
            else if (temp_index < 6)
            {
                string temp = " ";

                // ��Ī ����
                temp = mainflow.gameData.CardSetting.CardColorName[temp_index - mainflow.PatternCount];

                temp_btn.transform.GetChild(0).GetComponent<TMP_Text>().text
                = temp + " ����ġ +1";

                //temp_btn.transform.GetChild(1).GetComponent<TMP_Text>().text
                //= "���� ����ġ : " + mainflow.CardColorScore[temp_index - 3];
            }

            // ��ư ������Ʈ Ȱ��ȭ
            btn_FreeUpgrade[i].gameObject.SetActive(true);

            if (pool_score.Count <= 0)
                break;
        }
    }

    // ���� ���׷��̵�
    public void FreeUpgrade(int index)
    {
        // ���� �϶�
        if (FreeUpgradeIndex[index] < 3)
        {
            //mainflow.CardPatternScore[FreeUpgradeIndex[index]] += 1;
            mainflow.PatternLevel[FreeUpgradeIndex[index]] += 1;
        }
        // �÷� �϶�
        else if (FreeUpgradeIndex[index] < 6)
        {
            //mainflow.CardColorScore[FreeUpgradeIndex[index] - 3] += 1;
            mainflow.ColorLevel[FreeUpgradeIndex[index] - 3] += 1;
        }

        SetCombinationScoresText();

        // �г� ������Ʈ Ȱ��ȭ
        FreeUpgradePannel.SetActive(false);
    }

    // ��ü �� ���� ���� ī�� ����
    public void ClickAllDeckCardDelete(Card curData, GameObject targetCard)
    {
        // ��ü ������ ����
        // ���� �÷ο� ������ ����
        mainflow.userDeck.Remove(curData);

        // ��ü �� ������Ʈ ����
        Destroy(targetCard);
        // ��ü �� �����Ϳ��� ����
        AllDeck.Remove(curData);

        // ��ü �� �г� Off
        ControlDeckPanel(false);

        // Ŀ�� �� �ݱ� ��ư Ȱ��ȭ
        allDeckCardMask.SetActive(true);
        allDeckCardCloseButton.SetActive(true);
    }

    public void UnActiveBtn(Button button)
    {
        button.interactable = false;
    }

    // ���� ���� â ���� ���� �ؽ�Ʈ ����
    public void SetCombinationScoresText()
    {
        for (int i = 0; i < 6; i++)
        {
            if (i < 3)
            {
                // �ؽ�Ʈ ����
                //ScoreCombinationInfoObject[i].UpdateScore
                //    (mainflow.CardPatternScore[i].ToString());
            }
            else
            {
                // �ؽ�Ʈ ����
                //ScoreCombinationInfoObject[i].UpdateScore
                //    (mainflow.CardColorScore[i - 3].ToString());
            }
        }
    }

    // ���� ���� �г� ����
    public void GenerateCombinationScorePanel()
    {
        //ScoreCombinationInfoScript generated;
        //ScoreCombinationInfoObject = new List<ScoreCombinationInfoScript>();

        //for (int i = 0; i < 6; i++) 
        //{
        //    string tempName = " ";

        //    // ��Ī �ؽ�Ʈ ����
        //    switch (i)
        //    { 
        //        case 0:
        //            {
        //                tempName = "����-��";
        //                break;
        //            }
        //        case 1:
        //            {
        //                tempName = "����-�簢��";
        //                break;
        //            }
        //        case 2:
        //            {
        //                tempName = "����-�ﰢ��";
        //                break;
        //            }
        //        case 3:
        //            {
        //                tempName = "����-����";
        //                break;
        //            }
        //        case 4:
        //            {
        //                tempName = "����-�ʷ�";
        //                break;
        //            }
        //        case 5:
        //            {
        //                tempName = "����-�Ķ�";
        //                break;
        //            }
        //    }

        //    // ������Ʈ ����
        //    generated = Instantiate(ScoreConbinationInfoPanelPrefab, ScoreCombinationInfoHolder).GetComponent<ScoreCombinationInfoScript>();
            
        //    if(i < 3)
        //    {
        //        // �ؽ�Ʈ ����
        //        //generated.SetText
        //        //    (tempName, mainflow.CardPatternScore[i].ToString());
        //        generated.ExampleImage.sprite = PatternSprites[i];
        //    }
        //    else
        //    {
        //        // �ؽ�Ʈ ����
        //        //generated.SetText
        //        //    (tempName,mainflow.CardColorScore[i - 3].ToString());
        //        generated.ExampleImage.color = PatternColors[i - 3];
        //    }

        //    // ���� ���� ������ �߰�
        //    ScoreCombinationInfoObject.Add(generated);
        //}
    }

    // ��ü �� ���� �г� �ѱ�/���� ����
    public void ControlDeckPanel(bool isActive)
    {
        allDeckPanel.SetActive(isActive);
    }

    public void ControlCombinationPanel(bool isActive)
    {
        combinationPanel.SetActive(isActive);
    }

    public void ControlStagePanel(bool isOpenStore)
    {
        stagePanel.SetActive(!isOpenStore);
        storePanel.SetActive(isOpenStore);
    }

    public void ExitProgram()
    {
        Application.Quit();
    }

    public void ReLoadScene()
    {
        SceneManager.LoadScene("FirstTestScene");
    }
}
