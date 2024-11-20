using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnIndex : MonoBehaviour
{
    // ��ư ���� �ε���
    public int Index;

    // ��ư Ÿ�� ( 0 : ���� ���� / 1 : �ൿ / 2 : ��ȸ�� )
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
