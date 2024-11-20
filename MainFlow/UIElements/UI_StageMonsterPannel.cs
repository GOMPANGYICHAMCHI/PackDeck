using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageMonsterPannel : UI_Basic
{
    //=============================================
    // 오브젝트
    //=============================================

    [Header("현재 몬스터 명칭 텍스트")]
    public TMP_Text txt_MonsterName;

    [Header("목표 점수")]
    public TMP_Text txt_GoalScore;

    [Header("현재 몬스터 이미지")]
    public Image img_CurrentMonsterSprite;

    [Header("현재 몬스터 체력 바 이미지")]
    public Image img_CurrentMonsterHealthBar;

    //

    [Header("데미지 팝업 텍스트")]
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
        // 체력 게이지 이미지 조정
        img_CurrentMonsterHealthBar.fillAmount = (float)((double)playerData.Get_RemainHealth() / (double)playerData.Get_BasicHealth());

        // 목표점수 달성 여부 텍스트 갱신
        txt_GoalScore.text = playerData.Get_RemainHealth().ToString() + " / " + playerData.Get_BasicHealth().ToString();
    }
}
