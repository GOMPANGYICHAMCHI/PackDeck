using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TMPro;
using UnityEngine;

public class TextCode : MonoBehaviour
{
    public GameObject Prefab;
    public Transform PannelPos;

    int ObjectCount;

    public int[] AllPercent;
    int PercentSum = 0;

    int[] Count;
    TMP_Text[] allText;
    TMP_Text[] allPercentText;

    public TMP_Text randText;
    public TMP_Text randCountPerButtonText;

    int RandCount;

    int randCountPerButton;

    void Start()
    {
        ObjectCount = AllPercent.Count();

        Count = new int[ObjectCount];
        Array.Fill(Count,0);

        allText = new TMP_Text[ObjectCount];
        allPercentText = new TMP_Text[ObjectCount];

        GenerateTextAsset();
        ResetText();
    }

    private void GenerateTextAsset()
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            PercentSum += AllPercent[i];
        }

        for (int i = 0; i < ObjectCount; i++)
        {
            GameObject temp_obj;
            temp_obj = Instantiate(Prefab, PannelPos).gameObject;

            allText[i] = temp_obj.transform.GetChild(0).GetComponent<TMP_Text>();
            allPercentText[i] = temp_obj.transform.GetChild(2).GetComponent<TMP_Text>();

            temp_obj.transform.GetChild(1).GetComponent<TMP_Text>().text = (Mathf.Floor(((float)AllPercent[i] /  (float)PercentSum * 100f)) / 100f).ToString();
        }
    }

    public void ResetText()
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            allText[i].text = Count[i].ToString();
            allPercentText[i].text = (Mathf.Floor(((float)Count[i] / (float)RandCount) * 100f) / 100f).ToString();
        }

        randText.text = RandCount.ToString();
        randCountPerButtonText.text = randCountPerButton.ToString();
    }

    public void RandCountAdjust(bool isPlus)
    {
        if(isPlus)
        {
            randCountPerButton++;
        }

        else
        {
            randCountPerButton--;
        }
    }

    // ����� ���׷��̵� ���� ���� �� UI ����
    public void RandomStoreUpgrade()
    {
        for(int a = 0; a< 20; a++)
        {
            int scorePercentSum = 0;
            List<int> scorePercent = new List<int>();

            // ���� Ǯ
            List<int> pool_score = new List<int>();

            // ���� �� �÷� Ǯ ����
            for (int i = 0; i < ObjectCount; i++)
            {
                pool_score.Add(i);
                scorePercentSum += AllPercent[i];
                scorePercent.Add(AllPercent[i]);
            }

            //=============================================

            int temp_index;
            int temp_sum;

            // ���� ���� ���׷��̵� ����
            for (int i = 0; i < randCountPerButton; i++)
            {
                Debug.Log(scorePercentSum);

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
                //scorePercent.RemoveAt(temp_index);

                Count[temp_index]++;

                RandCount++;
            }
        }
        ResetText();
    }
}
