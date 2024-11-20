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
    
    // ī�� ���� ������
    public PlayerDataDelegate cardscore_changed;

    // ���� �������� ������       
    public PlayerDataDelegate currentstage_changed;

    // ���� ���� ������        
    public InputUlong remainhealth_changed;
    // ��ǥ ���� ������
    public PlayerDataDelegate basichealth_changed;
    // ���� �������� ������
    public PlayerDataDelegate selectscore_changed;
    // ���� ������ �����
    public PlayerDataDelegate scoredata_changed; 

    // ���� ��� ������
    public PlayerDataDelegate currentgold_changed;
    // �������� �� �߰� ��� ������
    public PlayerDataDelegate addgoldamount_changed;
    // ���� ��� ���� ī�� �� ������
    public PlayerDataDelegate interestdividecardcount_changed;
    // ���� ��� �ִ�ġ ������
    public PlayerDataDelegate interestlimit_changed;
    // �÷��̾� ���� ���̳ʽ� �ѵ� (��) ������
    public PlayerDataDelegate purchaseminuslimit_changed;

    // ���� ����ġ ������
    public PlayerDataDelegate patternlevel_changed;
    // �÷� ����ġ ������       
    public PlayerDataDelegate colorlevel_changed;

    // �ִ� �ൿ����, ����������, �÷��� ���� ī�� ��, �ڵ������ ������       
    public PlayerDataDelegate maxcost_changed;
    // ���� �ൿ����, ���������� ������       
    public PlayerDataDelegate currentcost_changed;

    // ������ ���� ���� �����
    public BoolInputDelegate currentdumpable_changed;

    // ���� ������ �߰���       
    public CardDelegate userdeck_add;
    // ���� �� �߰���       
    public CardDelegate currentdeck_add;
    // �ڵ� �� �߰���       
    public CardDelegate handdeck_add;
    // ���� �� �߰���       
    public CardDelegate selectdeck_add;

    // ���� ������ ���Ž�       
    public CardDelegate userdeck_remove;
    // ���� �� ���Ž�       
    public CardDelegate currentdeck_remove;
    // �ڵ� �� ���Ž�       
    public CardDelegate handdeck_remove;
    // ���� �� ���Ž�       
    public CardDelegate selectdeck_remove;

    // �ڵ� �� ������       
    public CardDelegate handdeck_dumped;

    // ī�� ���� ���ý�    
    public CardDelegate cardselected_forcibly;

    // ������ �����
    public PlayerDataDelegate userdeck_changed;
    // ���給 �����
    public PlayerDataDelegate currentdeck_changed;
    // �ڵ嵦 �����
    public PlayerDataDelegate handdeck_changed;
    // ���õ� �����
    public PlayerDataDelegate selectdeck_changed;

    // �÷��̾� ��Ŀ ������       
    public PlayerDataDelegate playerjocker_changed;
    // �÷��̾� ��Ŀ ���Ž�       
    public JockerDelegate playerjocker_remove;
    // �÷��̾� ��Ŀ �߰���       
    public JockerDelegate playerjocker_add;

    // �ν��Ͻ� �����
    public PlayerDataDelegate instanceUpgradeBtnClicked;

    // ����� ������       
    public PlayerDataDelegate currentdebuff_changed;
    // ����� ���Ž�       
    public PlayerDataDelegate currentdebuff_removed;
}
