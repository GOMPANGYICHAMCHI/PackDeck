using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ��� ���ִ� UI�� �����ϴ� ��ũ��Ʈ
// ���� ��������, ��Ŀ, ���� ���

public class UI_AllTimePannel : UI_Basic
{
    //=============================================
    // ������
    //=============================================

    [SerializeField]
    private GameObject prefab_Jocker;

    //=============================================
    // ������Ʈ
    //=============================================

    [SerializeField]
    private Transform JockerHolder;

    [SerializeField]
    private JockerUIScript[] JockerPannel = new JockerUIScript[5];

    //[SerializeField]
    //private Button btn_AllDeckBtn;

    [Header("��Ŀ ����â")]
    [SerializeField]
    private GameObject obj_JockerExplainPannel;

    [SerializeField]
    private TMP_Text txt_JockerExplain;

    [SerializeField]
    private TMP_Text txt_JockerName;

    [SerializeField]
    private TMP_Text txt_JockerCost;

    [SerializeField]
    private Image[] img_JockerInfoBackground;

    //=============================================
    // UI ������Ʈ
    //=============================================

    [SerializeField]
    private TMP_Text txt_currentgold;

    [SerializeField]
    private TMP_Text txt_currentstage;

    [SerializeField]
    private TMP_Text txt_currentjockercount;

    //=============================================
    // ����
    //=============================================

    Vector3 vec_JockerInfoPannelPos;

    [SerializeField]
    private Vector2 vec2_JockerInfoPannelPosOffset = new Vector2(150, 200);

    JockerBase curFocusJocker;

    //=============================================

    public override void Initialize()
    {
        UpdateText_CurrentGold();
        UpdateText_CurrentStage();
        SetEvent();
    }

    private void SetEvent()
    {
        for (int i = 0; i < JockerPannel.Length; i++)
        {
            JockerPannel[i].func_PoinetEnter += JcokerOnPointerEnter;
            JockerPannel[i].func_PoinetExit += JcokerOnPointerExit;
        }
    }

    public void JcokerOnPointerEnter(JockerBase input_jocker)
    {
        obj_JockerExplainPannel.SetActive(true);
        txt_JockerName.text = input_jocker.Info.Name;
        txt_JockerExplain.text = input_jocker.Info.ExplainText;
        txt_JockerCost.text = "�Ǹűݾ� : " + input_jocker.Info.SellCost.ToString() + " ���";
        curFocusJocker = input_jocker;

        for (int i = 0; i < img_JockerInfoBackground.Length; i++) 
        {
            img_JockerInfoBackground[i].color = input_jocker.Info.JockerColor;
        }
    }

    public void JcokerOnPointerExit(JockerBase input_jocker)
    {
        obj_JockerExplainPannel.SetActive(false);
        curFocusJocker = null;
    }

    private void Update()
    {
        // ��Ŀ���� ���콺 ȣ�� ���� �϶�
        if (obj_JockerExplainPannel.activeSelf)
        {
            vec_JockerInfoPannelPos = new Vector3(Input.mousePosition.x + vec2_JockerInfoPannelPosOffset.x, Input.mousePosition.y - vec2_JockerInfoPannelPosOffset.y, 0);

            // ��Ŀ ���� ������Ʈ ��ġ ����
            obj_JockerExplainPannel.transform.position = vec_JockerInfoPannelPos;

            // Q ��ư Ŭ���� ( ��Ŀ �Ǹ� )
            if(Input.GetKeyDown(KeyCode.Q) && curFocusJocker != null)
            {
                curFocusJocker.Event_Sell(playerData);
                playerData.Remove_PlayerJocker(curFocusJocker);

                // ��Ŀ ���� ������Ʈ ��Ȱ��ȭ
                obj_JockerExplainPannel.SetActive(false);
                curFocusJocker = null;
            }
        }
    }

    // ���� ��� �ؽ�Ʈ ������Ʈ
    public void UpdateText_CurrentGold()
    {
        txt_currentgold.text = playerData.Get_CurrentGold().ToString();
    }

    // ���� �������� �ؽ�Ʈ ������Ʈ
    public void UpdateText_CurrentStage()
    {
        txt_currentstage.text = playerData.Get_CurrentStage().ToString();
    }

    // ���� ��Ŀ ī��� �ؽ�Ʈ ������Ʈ
    public void Update_CurrentJockerCount()
    {
        txt_currentjockercount.text = playerData.Get_PlayerJockerCount().ToString();
    }

    // �÷��̾� ��Ŀ �г� ������Ʈ
    public void Update_JockerPannel()
    {
        for (int i = 0; i < 5; i++)
        {
            JockerPannel[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < playerData.Get_PlayerJockerCount(); i++)
        {
            JockerPannel[i].gameObject.SetActive(true);
            JockerPannel[i].gameObject.GetComponent<Image>().color = playerData.Get_PlayerJocker(i).Info.JockerColor;
            JockerPannel[i].transform.GetChild(0).GetComponent<TMP_Text>().text
                = playerData.Get_PlayerJocker(i).Info.Name;
            JockerPannel[i].GetComponent<JockerUIScript>().Index = i;
            JockerPannel[i].jockerInfo = playerData.Get_PlayerJocker(i);
        }
    }

    // �÷��̾� ��Ŀ ����
    public void Remove_PlayerJocker(JockerBase input_Jocker)
    {

    }
    
    // �÷��̾� ��Ŀ �߰�
    public void Add_PlayerJocker(JockerBase input_Jocker)
    {
        
    }
}
