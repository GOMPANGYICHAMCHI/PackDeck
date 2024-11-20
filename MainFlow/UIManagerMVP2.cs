using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerMVP2 : MonoBehaviour
{
    //=============================================
    // ���� ��ũ��Ʈ
    //=============================================

    private PlayerData playerData;
    private FlowBroadCaster flowBroadcaster;
    private IngameMsgPannel msgPannel;

    //============================================= 
    // �ڼ� ��ũ��Ʈ
    //=============================================

    public UI_StorePannel store_UI;
    public UI_AllCardDeck allcard_UI;
    public UI_AllTimePannel alltime_UI;
    public UI_ScoreCombinationPannel scorecombination_UI;
    public UI_StageCardPannel stagecard_UI;
    public UI_Instage instage_UI;
    public UI_StageMonsterPannel stagemonster_UI;
    public UI_RewardCheckPannel rewardcheck_UI;

    public UI_DebugPannel debugtext_UI;

    //=============================================

    // ������ ����
    public void GiveComponent
        (PlayerData input_playerdata, 
        FlowBroadCaster input_flowmanager,
        IngameMsgPannel input_msgpannel)
    {
        playerData = input_playerdata;
        flowBroadcaster = input_flowmanager;
        msgPannel = input_msgpannel;

        ResetAllUIData();
        SetDebugEvent();
    }

    // ����� �ʱ�ȭ
    private void SetDebugEvent()
    {
        flowBroadcaster.MainflowToBroadcaster_DebugOn += store_UI.OnDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOn += allcard_UI.OnDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOn += alltime_UI.OnDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOn += scorecombination_UI.OnDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOn += stagecard_UI.OnDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOn += instage_UI.OnDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOn += stagemonster_UI.OnDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOn += debugtext_UI.OnDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOn += rewardcheck_UI.OnDebug;

        flowBroadcaster.MainflowToBroadcaster_DebugOff += store_UI.OffDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOff += allcard_UI.OffDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOff += alltime_UI.OffDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOff += scorecombination_UI.OffDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOff += stagecard_UI.OffDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOff += instage_UI.OffDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOff += stagemonster_UI.OffDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOff += debugtext_UI.OffDebug;
        flowBroadcaster.MainflowToBroadcaster_DebugOff += rewardcheck_UI.OffDebug;
    }

    // �ڼ� ��ũ��Ʈ �ʿ� ������Ʈ ����
    private void ResetAllUIData()
    {
        store_UI.SetBasicData(playerData, flowBroadcaster, msgPannel);
        allcard_UI.SetBasicData(playerData, flowBroadcaster, msgPannel);
        alltime_UI.SetBasicData(playerData, flowBroadcaster, msgPannel);
        scorecombination_UI.SetBasicData(playerData, flowBroadcaster, msgPannel);
        stagecard_UI.SetBasicData(playerData, flowBroadcaster, msgPannel);
        instage_UI.SetBasicData(playerData, flowBroadcaster, msgPannel);
        stagemonster_UI.SetBasicData(playerData, flowBroadcaster, msgPannel);
        rewardcheck_UI.SetBasicData(playerData, flowBroadcaster, msgPannel);

        rewardcheck_UI.Initialize();
        store_UI.Initialize();
        allcard_UI.Initialize();
        alltime_UI.Initialize();
        scorecombination_UI.Initialize();
        stagecard_UI.Initialize();
        instage_UI.Initialize();
        stagemonster_UI.Initialize();

        // �����
        debugtext_UI.SetBasicData(playerData, flowBroadcaster, msgPannel);
        debugtext_UI.Initialize();
    }

    public void SetEvent()
    {
        //=============================================
        // �÷ο� ��ε�ĳ����
        //=============================================

        // ���� ���� ��
        flowBroadcaster.MainflowToBroadcaster_GameStart += allcard_UI.GenerateAllCards;
        flowBroadcaster.MainflowToBroadcaster_GameStart += () => instage_UI.SetActiveGameoverPannel(false);
        flowBroadcaster.MainflowToBroadcaster_GameStart += instage_UI.UpdateScoreAdditionals;

        // �������� ���� ��
        flowBroadcaster.MainflowToBroadcaster_StageStart += () => allcard_UI.ActiveAllCardCover(false);
        flowBroadcaster.MainflowToBroadcaster_StageStart += () => instage_UI.SetActiveIngamePannel(true);
        flowBroadcaster.MainflowToBroadcaster_StageStart += () => instage_UI.SetActiveCardPlayPannel(true);
        flowBroadcaster.MainflowToBroadcaster_StageStart += stagecard_UI.DisableAllSelectCard;
        flowBroadcaster.MainflowToBroadcaster_StageStart += stagecard_UI.DisableAllHandCard;

        // ī�� �÷��� ���� ��
        //flowBroadcaster.MainflowToBroadcaster_CardPlayStart += playerData.Reset_SelectDeck;
        flowBroadcaster.MainflowToBroadcaster_CardPlayStart += stagecard_UI.StartCardPlay;
        flowBroadcaster.MainflowToBroadcaster_CardPlayStart += () => instage_UI.SetInterectableAllButton(true);
        flowBroadcaster.MainflowToBroadcaster_CardPlayStart += () => stagecard_UI.SetAllCardInteractable(true);

        // ���ھ� üũ
        flowBroadcaster.MainflowToBroadcaster_StartScoreCheck += () => instage_UI.SetInterectableAllButton(false);
        flowBroadcaster.MainflowToBroadcaster_StartScoreCheck += stagecard_UI.ScoreCheck;

        // ������ �ڵ�� ī�� �̵���
        flowBroadcaster.DeckToHand += allcard_UI.DeActiveCardCover;

        // ī�� ������
        flowBroadcaster.BroadcasterToUI_CardDump += stagecard_UI.SelectCardDump;

        // ���� Ȯ�� â ���� ��
        flowBroadcaster.MainflowToBroadcaster_EnterRewardCheck += rewardcheck_UI.EnterRewardCheck;
        flowBroadcaster.MainflowToBroadcaster_EnterRewardCheck += () => instage_UI.SetActiveCardPlayPannel(false);
        flowBroadcaster.MainflowToBroadcaster_EnterRewardCheck += () => allcard_UI.OnOffPannel(false);

        // ���� Ȯ�� â �ؽ�Ʈ ������Ʈ
        flowBroadcaster.stageRewardApply += rewardcheck_UI.UpdateScorePannelText;

        // ����� ����
        flowBroadcaster.BroadcasterToUI_StoreEnter += () => allcard_UI.ActiveAllCardCover(false);
        flowBroadcaster.BroadcasterToUI_StoreEnter += () => allcard_UI.OnOffPannel(false);
        flowBroadcaster.BroadcasterToUI_StoreEnter += () => instage_UI.SetActiveIngamePannel(false);
        flowBroadcaster.BroadcasterToUI_StoreEnter += store_UI.EnterStore;

        // ���� ������
        flowBroadcaster.MainflowToBroadcaster_GameOver += () => instage_UI.SetActiveGameoverPannel(true);

        // ���� ���� Ȯ��
        flowBroadcaster.MainFlowToBroadcast_ScoreApplied += stagemonster_UI.DamagePopupText;

        // ���� ����� ��
        flowBroadcaster.BroadcasterToMainflow_RestartGame += allcard_UI.ClearAllData;

        //=============================================
        // �÷��̾� ������ ��ε�ĳ����
        //=============================================

        // ���� ���� ī�� �߰���
        playerData.Broadcaster.currentdeck_add += allcard_UI.AddCard;

        // ���� ��� ���� ��
        playerData.Broadcaster.currentgold_changed += alltime_UI.UpdateText_CurrentGold;

        // ���� �������� ���� ���� ��
        playerData.Broadcaster.currentstage_changed += alltime_UI.UpdateText_CurrentStage;

        // �÷��̾� ��Ŀ ���� ��
        playerData.Broadcaster.playerjocker_changed += alltime_UI.Update_JockerPannel;
        playerData.Broadcaster.playerjocker_changed += alltime_UI.Update_CurrentJockerCount;

        playerData.Broadcaster.playerjocker_add += alltime_UI.Add_PlayerJocker;
        playerData.Broadcaster.playerjocker_remove += alltime_UI.Remove_PlayerJocker;

        // ���� �ڽ�Ʈ ���� ��
        playerData.Broadcaster.currentcost_changed += instage_UI.UpdateActionCost;

        // ���� ���ھ� ������
        playerData.Broadcaster.remainhealth_changed += (empty) => stagemonster_UI.UpdateMonsterHealthBar();

        //playerData.Broadcaster.currentscore_changed += stagemonster_UI.DamagePopupText;

        // ��ǥ ���ھ� ������
        playerData.Broadcaster.basichealth_changed += stagemonster_UI.UpdateMonsterHealthBar;

        // ������ ���� ���� ������
        playerData.Broadcaster.currentdumpable_changed += instage_UI.SetInteracterableDumpButton;

        // ī�� ���� ����
        playerData.Broadcaster.cardselected_forcibly += stagecard_UI.SelectHandCardForcibly;

        // ī�� ���� ������
        playerData.Broadcaster.handdeck_dumped += stagecard_UI.DumpCard;

        // �ν��Ͻ� ����� �߰� ���� ��Ȳ�� ����
        playerData.Broadcaster.instanceUpgradeBtnClicked += instage_UI.UpdateScoreAdditionals;
    }
}
