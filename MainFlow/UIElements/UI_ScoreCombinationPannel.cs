using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ScoreCombinationPannel : UI_Basic
{
    //=============================================
    // 게임 오브젝트
    //=============================================

    public Transform ScoreInfoPannelHolder;

    public List<ScoreCombinationInfoScript> ScoreCombinationInfoObject;

    //=============================================
    // 프리팹
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

        //    // 명칭 텍스트 설정
        //    switch (i)
        //    {
        //        case 0:
        //            {
        //                tempName = "도형-원";
        //                break;
        //            }
        //        case 1:
        //            {
        //                tempName = "도형-사각형";
        //                break;
        //            }
        //        case 2:
        //            {
        //                tempName = "도형-삼각형";
        //                break;
        //            }
        //        case 3:
        //            {
        //                tempName = "색상-빨강";
        //                break;
        //            }
        //        case 4:
        //            {
        //                tempName = "색상-초록";
        //                break;
        //            }
        //        case 5:
        //            {
        //                tempName = "색상-파랑";
        //                break;
        //            }
        //    }

        //    // 오브젝트 생성
        //    generated = Instantiate(prefab_ScoreInfoPannel, ScoreInfoPannelHolder).GetComponent<ScoreCombinationInfoScript>();

        //    if (i < 3)
        //    {
        //        // 텍스트 설정
        //        //generated.SetText
        //        //    (tempName, playerData.CardPatternScore[i].ToString());
        //        generated.ExampleImage.sprite = playerData.gameData.CardSetting.CardPattern[i];
        //    }
        //    else
        //    {
        //        // 텍스트 설정
        //        //generated.SetText
        //        //    (tempName, playerData.CardColorScore[i - 3].ToString());
        //        generated.ExampleImage.color = playerData.gameData.CardSetting.CardColor[i - 3];
        //    }

        //    // 조합 정보 모음에 추가
        //    ScoreCombinationInfoObject.Add(generated);
        //}
    }
}
