using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandCardScript : CardBasic
{

    public int ObjectIndex;

    // ���� �׵θ� ���� �̹���
    public GameObject selectHighlightImage;

    // ���� ��ư
    public Button selfBtn;

    // ����, ���� �Լ�
    public Action SelectFunc;
    public Action UnSelectFunc;

    public bool isOn = false;

    private void Start()
    {
        selfBtn = GetComponent<Button>();
    }

    // ���� ���� �̹��� ��ȯ
    public void SetSelectPanel()
    {
        selectHighlightImage.SetActive(!selectHighlightImage.activeSelf);
    }

    // ���� ��ư Ŭ�� ��
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
