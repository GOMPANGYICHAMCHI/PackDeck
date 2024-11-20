using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageMonsterPannel : UI_Basic
{
    //=============================================
    // ������Ʈ
    //=============================================

    [Header("���� ���� ��Ī �ؽ�Ʈ")]
    public TMP_Text txt_MonsterName;

    [Header("��ǥ ����")]
    public TMP_Text txt_GoalScore;

    [Header("���� ���� �̹���")]
    public Image img_CurrentMonsterSprite;

    [Header("���� ���� ü�� �� �̹���")]
    public Image img_CurrentMonsterHealthBar;

    //

    [Header("������ �˾� �ؽ�Ʈ")]
    public TMP_Text txt_damagePopup;
    public Animator anim_damagePopupText;

    //=============================================

    void Start()
    {

    }

    public void DamagePopupText(long damage)
    {
        if(damage != 0)
        {
            txt_damagePopup.text = damage.ToString();
            anim_damagePopupText.Play("anim_DamagePopupText", 0, 0f);
        }
    }

    public void UpdateMonsterHealthBar()
    {
        // ü�� ������ �̹��� ����
        img_CurrentMonsterHealthBar.fillAmount = (float)((double)playerData.Get_RemainHealth() / (double)playerData.Get_BasicHealth());

        // ��ǥ���� �޼� ���� �ؽ�Ʈ ����
        txt_GoalScore.text = playerData.Get_RemainHealth().ToString() + " / " + playerData.Get_BasicHealth().ToString();
    }
}
