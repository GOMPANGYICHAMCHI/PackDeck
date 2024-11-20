using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowBroadCaster : MonoBehaviour
{
    public delegate void VoidDelegate();
    public delegate void InputUlong(long  value);
    public delegate void CardDelegate(Card data);
    public delegate void StageReward(GoldRewardData goldReward);
    public delegate int Additional_RoundClearReward();
    public delegate float Additional_OverDealMultiply();

    //================================================================

    // ����� �ɼ�
    public VoidDelegate MainflowToBroadcaster_DebugOn;
    public VoidDelegate MainflowToBroadcaster_DebugOff;

    // ī�� �÷��� ����
    public VoidDelegate MainflowToBroadcaster_CardPlayStart;

    // ī�� �÷��� ����
    public VoidDelegate BroadcasterToMainflow_CardPlayEnd;
    public VoidDelegate MainflowToBroadcaster_CardPlayEndStart;
    public VoidDelegate MainflowToBroadcaster_CardPlayEndPost;

    // ī�� ������
    public VoidDelegate BroadcasterToUI_CardDump;
    public VoidDelegate BroadcasterToMainflow_CardDump;
    public VoidDelegate MainflowToBroadcaster_CardDump;

    // �������� ����, ����
    public VoidDelegate MainflowToBroadcaster_StageStart;
    public VoidDelegate MainflowToBroadcaster_StageEnd;

    // ���� ����, ����
    public VoidDelegate MainflowToBroadcaster_GameStart;
    public VoidDelegate MainflowToBroadcaster_GameOver;

    // ���� �����
    public VoidDelegate BroadcasterToMainflow_RestartGame;

    // ���ھ� Ȯ��
    public VoidDelegate BroadcasterToMainflow_ScoreCheck;
    public VoidDelegate MainflowToBroadcaster_StartScoreCheck;
    public VoidDelegate BroadcasterToMainflow_ApplyScore;
    public InputUlong MainFlowToBroadcast_ScoreApplied;

    // ���� ���� �� ����
    public VoidDelegate BroadcasterToUI_StoreEnter;
    public VoidDelegate BroadcasterToMainflow_StoreExit;
    
    // ��Ŀ ���� �Ǹ�
    public VoidDelegate BroadcasterToMainflow_JockerBuy;
    public VoidDelegate BroadcasterToMainflow_JockerSell;

    public VoidDelegate MainflowToBroadcaster_DeactivateCurrentDebuff;

    // ���� Ȯ�� �� ����
    public VoidDelegate BroadcastToMainFlow_CheckScore;  
    public VoidDelegate MainflowToBroadcaster_ApplyScore;

    // �������� ���� ����
    public StageReward stageRewardApply;
    public Additional_RoundClearReward additionalRoundClearReward;
    public Additional_OverDealMultiply additionalOverDealMultiply;
    // �������� ���� �гη�
    public VoidDelegate MainflowToBroadcaster_EnterRewardCheck;

    public CardDelegate SelectCard;
    public CardDelegate UnSelectCard;
    public CardDelegate DeckToHand;

    //================================================================
    // �ν��Ͻ� ���׷��̵� �̺�Ʈ
    //================================================================

    public VoidDelegate InstanceUpgrade11_DeleteDeckCard;
    public VoidDelegate InstanceUpgrade12_AddDeckCard;
    public VoidDelegate InstanceUpgrade13_UpgradeCardNumber;
}
