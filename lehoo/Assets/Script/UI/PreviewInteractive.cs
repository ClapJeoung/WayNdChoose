using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PreviewPanelType { Turn,HP,Sanity,Gold,Map,Quest,Trait,Theme,Skill,EXP_long,EXP_short,Tendency,Selection,
  RewardHP,RewardSanity,RewardGold,RewardTrait,RewardTheme,RewardSkill,RewardExp,RewardSkillSelect,RewardExpSelect_long,RewardExpSelect_short,Discomfort,
Place,Environment,MadnessAccept,MadnessRefuse,MoveCostSanity,MoveCostGold,RestSanity,RestGold,WolfPanel_Cult,WolfPanel_Wolf}
public class PreviewInteractive :MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public PreviewPanelType PanelType=PreviewPanelType.Turn;
    [Space(15)]
    public TendencyType MyTendency = TendencyType.None;
  public TendencyType MySelectionTendency= TendencyType.None;
  public bool MySelectionTendencyDir = false;
  public int MySelectionTendencyIndex = 0;
  public SkillType Myskill = SkillType.Conversation;
  public int RewardValue = 0;
  public int ExpIndex = 0;
  public Experience MyEXP = null;
  public SectorType MyPlaceType = SectorType.NULL;
  public EnvironmentType MyEnvironmentType = EnvironmentType.NULL;
    public void OnPointerEnter(PointerEventData eventData)
    {
    Experience _exp = null;
    switch (PanelType)
    {
      case PreviewPanelType.Turn:UIManager.Instance.PreviewManager.OpenTurnPreview();break;
        case PreviewPanelType.HP:UIManager.Instance.PreviewManager.OpenHPPreview();break;
      case PreviewPanelType.Sanity:UIManager.Instance.PreviewManager.OpenSanityPreview();break;
        case PreviewPanelType.Gold:UIManager.Instance.PreviewManager.OpenGoldPreview();break;
      case PreviewPanelType.Map:UIManager.Instance.PreviewManager.OpenMapPreview();break;
      case PreviewPanelType.Quest:UIManager.Instance.PreviewManager.OpenQuestPreview();break;
      case PreviewPanelType.Skill:UIManager.Instance.PreviewManager.OpenSkillPreview(Myskill);break;
      case PreviewPanelType.EXP_long:
         _exp = GameManager.Instance.MyGameData.LongTermEXP;
        if(_exp!=null)UIManager.Instance.PreviewManager.OpenExpPreview(_exp);
        break;
        case PreviewPanelType.EXP_short:
         _exp = GameManager.Instance.MyGameData.ShortTermEXP[ExpIndex];
        if (_exp != null) UIManager.Instance.PreviewManager.OpenExpPreview(_exp);
        break;
      case PreviewPanelType.Tendency:UIManager.Instance.PreviewManager.OpenTendencyPreview(MyTendency);break;
      case PreviewPanelType.Selection:
        SelectionData _selection = new SelectionData(null,0);
        switch (MySelectionTendency)
        {
          case TendencyType.None:
            _selection = GameManager.Instance.MyGameData.CurrentEvent.SelectionDatas[0];
            break;
          case TendencyType.Head:
             _selection = GameManager.Instance.MyGameData.CurrentEvent.SelectionDatas[MySelectionTendencyIndex];
            break;
          case TendencyType.Body:
            _selection = GameManager.Instance.MyGameData.CurrentEvent.SelectionDatas[MySelectionTendencyIndex];
            break;
        }
        switch (_selection.ThisSelectionType)
        {
          case SelectionTargetType.None:UIManager.Instance.PreviewManager.OpenSelectionNonePreview(_selection,MySelectionTendency,MySelectionTendencyDir); break;
          case SelectionTargetType.Pay: UIManager.Instance.PreviewManager.OpenSelectionPayPreview(_selection, MySelectionTendency, MySelectionTendencyDir); break;
          case SelectionTargetType.Check_Single:case SelectionTargetType.Check_Multy:
            UIManager.Instance.PreviewManager.OpenSelectionCheckPreview_skill(_selection, MySelectionTendency, MySelectionTendencyDir); break;
          default: UIManager.Instance.PreviewManager.OpenSelectionElsePreview(_selection, MySelectionTendency, MySelectionTendencyDir); break;
        }
        break;
      case PreviewPanelType.RewardHP:
        UIManager.Instance.PreviewManager.OpenRewardStatusPreview(StatusType.HP, GameManager.Instance.MyGameData.RewardHPValue_modified); break;
      case PreviewPanelType.RewardSanity:
        UIManager.Instance.PreviewManager.OpenRewardStatusPreview(StatusType.Sanity, GameManager.Instance.MyGameData.RewardSanityValue_modified); break;
      case PreviewPanelType.RewardGold:
        UIManager.Instance.PreviewManager.OpenRewardStatusPreview(StatusType.Gold, GameManager.Instance.MyGameData.RewardGoldValue_modified); break;
      case PreviewPanelType.RewardSkill:
        UIManager.Instance.PreviewManager.OpenRewardSkillPreview(Myskill);  break;
      case PreviewPanelType.RewardExp:
        UIManager.Instance.PreviewManager.OpenRewardExpPreview(MyEXP); break;
      case PreviewPanelType.RewardExpSelect_long:
        _exp = GameManager.Instance.MyGameData.LongTermEXP;
        if (_exp == null) UIManager.Instance.PreviewManager.OpenExpSelectionEmptyPreview(MyEXP,true);
        else
        {
          if (_exp.ExpType.Equals(ExpTypeEnum.Normal)) UIManager.Instance.PreviewManager.OpenExpSelectionExistPreview(_exp, MyEXP, true);
          else if (_exp.ExpType == ExpTypeEnum.Bad) UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(GameManager.Instance.GetTextData("CANNOTCHANGEBADEXP_NAME"));
          else UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(GameManager.Instance.GetTextData("CANNOTCHANGEMADNESS_NAME"));
        }
        break;
      case PreviewPanelType.RewardExpSelect_short:
        _exp = GameManager.Instance.MyGameData.ShortTermEXP[ExpIndex];
        if (_exp == null) UIManager.Instance.PreviewManager.OpenExpSelectionEmptyPreview(MyEXP,false);
        else
        {
          if (_exp.ExpType.Equals(ExpTypeEnum.Normal)) UIManager.Instance.PreviewManager.OpenExpSelectionExistPreview(_exp, MyEXP,false);
          else if (_exp.ExpType == ExpTypeEnum.Bad) UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(GameManager.Instance.GetTextData("CANNOTCHANGEBADEXP_NAME"));
          else UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(GameManager.Instance.GetTextData("CANNOTCHANGEMADNESS_NAME"));
        }
        break;
      case PreviewPanelType.Discomfort:
        UIManager.Instance.PreviewManager.OpenDisComfortPanel();
        break;
      case PreviewPanelType.Environment:
        UIManager.Instance.PreviewManager.OpenEnvirPanel(MyEnvironmentType);
        break;
      case PreviewPanelType.MadnessAccept:
        UIManager.Instance.PreviewManager.OpenJustDescriptionPreview
          ( string.Format(GameManager.Instance.GetTextData("ACCEPTMADNESS_DESCRIPTOIN"), WNCText.GetSanityColor(ConstValues.SanityLoseByMadnessExp)));
        break;
      case PreviewPanelType.MadnessRefuse:
        UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(
          string.Format(GameManager.Instance.GetTextData("REFUSEMADNESS_DESCRIPTION"),
          WNCText.GetHPColor(ConstValues.MaddnesRefuseHPCost),
          WNCText.GetSanityColor(ConstValues.MadnessRefuseSanityRestore)));
        break;
      case PreviewPanelType.MoveCostSanity:
        UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(string.Format(GameManager.Instance.GetTextData("MAPCOSTTYPE_SANITY"), UIManager.Instance.MyMap.SanityCost));
        break;
      case PreviewPanelType.MoveCostGold:
        UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(string.Format(GameManager.Instance.GetTextData("MAPCOSTTYPE_GOLD"), UIManager.Instance.MyMap.GoldCost));
        break;
      case PreviewPanelType.RestSanity:
        UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(GameManager.Instance.GetTextData("REST_SANITY"));
        break;
      case PreviewPanelType.RestGold:
        UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(GameManager.Instance.GetTextData("REST_GOLD"));
        break;
      case PreviewPanelType.WolfPanel_Cult:
        UIManager.Instance.PreviewManager.OpenIconAndDescriptionPanel(GameManager.Instance.ImageHolder.QuestIcon_Hideout_Idle, GameManager.Instance.GetTextData("Quest_Wolf_Cult_Sidepanel_Preview"), new Vector2(1.1f, 0.5f)) ;
        break;
      case PreviewPanelType.WolfPanel_Wolf:
        UIManager.Instance.PreviewManager.OpenIconAndDescriptionPanel(GameManager.Instance.ImageHolder.QuestIcon_Ritual_Idle,GameManager.Instance.GetTextData("Quest_Wolf_Wolf_Sidepanel_Preview"), new Vector2(1.1f, 0.5f));
        break;
    }
  }
    public void OnPointerExit(PointerEventData eventData) 
    {
    UIManager.Instance.PreviewManager.ClosePreview();
  }


}
