using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ScoreCombinationPannel : UI_Basic
{
    //=============================================
    // ���� ������Ʈ
    //=============================================

    public Transform ScoreInfoPannelHolder;

    public List<ScoreCombinationInfoScript> ScoreCombinationInfoObject;

    //=============================================
    // ������
    //=============================================

    public GameObject prefab_ScoreInfoPannel;

    //=============================================

    private void GenerateScoreCombinationPannel()
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
        //    generated = Instantiate(prefab_ScoreInfoPannel, ScoreInfoPannelHolder).GetComponent<ScoreCombinationInfoScript>();

        //    if (i < 3)
        //    {
        //        // �ؽ�Ʈ ����
        //        //generated.SetText
        //        //    (tempName, playerData.CardPatternScore[i].ToString());
        //        generated.ExampleImage.sprite = playerData.gameData.CardSetting.CardPattern[i];
        //    }
        //    else
        //    {
        //        // �ؽ�Ʈ ����
        //        //generated.SetText
        //        //    (tempName, playerData.CardColorScore[i - 3].ToString());
        //        generated.ExampleImage.color = playerData.gameData.CardSetting.CardColor[i - 3];
        //    }

        //    // ���� ���� ������ �߰�
        //    ScoreCombinationInfoObject.Add(generated);
        //}
    }
}
