using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnIndex : MonoBehaviour
{
    // 버튼 순서 인덱스
    public int Index;

    // 버튼 타입 ( 0 : 조합 점수 / 1 : 행동 / 2 : 일회성 )
    public int Type = 0;

    public Button selfBtn;

    public UIManager uimanager;

    private void Awake()
    {
        selfBtn = GetComponent<Button>();
        selfBtn.onClick.AddListener(BtnOnClick);
    }

    void BtnOnClick()
    {
        switch (Type)
        {
            case 0:
                uimanager.ScoreUpgrade(Index, selfBtn);
                break;

            case 1:
                uimanager.ActionUpgrade(Index, selfBtn);
                break;

            case 2:
                uimanager.SingleUseUpgrade(Index, selfBtn);
                break;
        }
    }
}
