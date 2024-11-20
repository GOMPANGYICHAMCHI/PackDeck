using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandCardScript : CardBasic
{

    public int ObjectIndex;

    // 선택 테두리 강조 이미지
    public GameObject selectHighlightImage;

    // 메인 버튼
    public Button selfBtn;

    // 선택, 비선택 함수
    public Action SelectFunc;
    public Action UnSelectFunc;

    public bool isOn = false;

    private void Start()
    {
        selfBtn = GetComponent<Button>();
    }

    // 선택 강조 이미지 전환
    public void SetSelectPanel()
    {
        selectHighlightImage.SetActive(!selectHighlightImage.activeSelf);
    }

    // 메인 버튼 클릭 시
    public void SelfBtnPressed()
    {
        if(!isOn)
        {
            SelectFunc();
            //isOn = true;
        }
        else
        {
            UnSelectFunc();
            //isOn = false;
        }
    }
} 
