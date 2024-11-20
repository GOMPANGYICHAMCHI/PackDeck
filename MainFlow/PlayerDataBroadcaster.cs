using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataBroadcaster : MonoBehaviour
{
    public delegate void PlayerDataDelegate();

    public delegate void BoolInputDelegate(bool input);

    public delegate void InputUlong(ulong value);

    public delegate void CardDelegate(Card input_caard);

    public delegate void JockerDelegate(JockerBase input_Jocker);

    public delegate void DeBuffDelegate(DeBuffBase input_Debuff);
    
    // 카드 점수 변동시
    public PlayerDataDelegate cardscore_changed;

    // 현재 스테이지 변동시       
    public PlayerDataDelegate currentstage_changed;

    // 현재 점수 변동시        
    public InputUlong remainhealth_changed;
    // 목표 점수 변동시
    public PlayerDataDelegate basichealth_changed;
    // 현재 선택점수 변동시
    public PlayerDataDelegate selectscore_changed;
    // 점수 데이터 변경시
    public PlayerDataDelegate scoredata_changed; 

    // 현재 골드 변동시
    public PlayerDataDelegate currentgold_changed;
    // 스테이지 당 추가 골드 변동시
    public PlayerDataDelegate addgoldamount_changed;
    // 이자 골드 조건 카드 수 변동시
    public PlayerDataDelegate interestdividecardcount_changed;
    // 이자 골드 최대치 변동시
    public PlayerDataDelegate interestlimit_changed;
    // 플레이어 구매 마이너스 한도 (빛) 변동시
    public PlayerDataDelegate purchaseminuslimit_changed;

    // 패턴 가중치 변동시
    public PlayerDataDelegate patternlevel_changed;
    // 컬러 가중치 변동시       
    public PlayerDataDelegate colorlevel_changed;

    // 최대 행동점수, 버리기점수, 플레이 가능 카드 수, 핸드사이즈 변동시       
    public PlayerDataDelegate maxcost_changed;
    // 현재 행동점수, 버리기점수 변동시       
    public PlayerDataDelegate currentcost_changed;

    // 버리기 가능 여부 변경시
    public BoolInputDelegate currentdumpable_changed;

    // 현재 유저덱 추가시       
    public CardDelegate userdeck_add;
    // 현재 덱 추가시       
    public CardDelegate currentdeck_add;
    // 핸드 덱 추가시       
    public CardDelegate handdeck_add;
    // 선택 덱 추가시       
    public CardDelegate selectdeck_add;

    // 현재 유저덱 제거시       
    public CardDelegate userdeck_remove;
    // 현재 덱 제거시       
    public CardDelegate currentdeck_remove;
    // 핸드 덱 제거시       
    public CardDelegate handdeck_remove;
    // 선택 덱 제거시       
    public CardDelegate selectdeck_remove;

    // 핸드 덱 버리기       
    public CardDelegate handdeck_dumped;

    // 카드 강제 선택시    
    public CardDelegate cardselected_forcibly;

    // 유저덱 변경시
    public PlayerDataDelegate userdeck_changed;
    // 현재덱 변경시
    public PlayerDataDelegate currentdeck_changed;
    // 핸드덱 변경시
    public PlayerDataDelegate handdeck_changed;
    // 선택덱 변경시
    public PlayerDataDelegate selectdeck_changed;

    // 플레이어 조커 변동시       
    public PlayerDataDelegate playerjocker_changed;
    // 플레이어 조커 제거시       
    public JockerDelegate playerjocker_remove;
    // 플레이어 조커 추가시       
    public JockerDelegate playerjocker_add;

    // 인스턴스 성장시
    public PlayerDataDelegate instanceUpgradeBtnClicked;

    // 디버프 변동시       
    public PlayerDataDelegate currentdebuff_changed;
    // 디버프 제거시       
    public PlayerDataDelegate currentdebuff_removed;
}
