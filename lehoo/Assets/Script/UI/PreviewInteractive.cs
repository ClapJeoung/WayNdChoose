using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PreviewPanelType { Turn,HP,Sanity,Gold,Map,Quest,Trait,Theme,Skill,EXP_long,EXP_short,Tendency,Selection,
  RewardHP,RewardSanity,RewardGold,RewardTrait,RewardTheme,RewardSkill,RewardExp,RewardSkillSelect,RewardExpSelect_long,RewardExpSelect_short,Discomfort,
Place,Environment,MadnessAccept,MadnessRefuse,MoveCostSanity,MoveCostGold,RestSanity,RestGold,CultPanel_Sabbat,CultPanel_Ritual,MovePoint,MoveCostGoldNogold}
public class PreviewInteractive :MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public PreviewPanelType PanelType=PreviewPanelType.Turn;
    [Space(15)]
    public TendencyTypeEnum MyTendency = TendencyTypeEnum.None;
  public TendencyTypeEnum MySelectionTendency= TendencyTypeEnum.None;
  public bool MySelectionTendencyDir = false;
  public SkillTypeEnum Myskill = SkillTypeEnum.Conversation;
  public int RewardValue = 0;
  public int ExpIndex = 0;
  public Experience MyEXP = null;
  public SectorTypeEnum MyPlaceType = SectorTypeEnum.NULL;
  public EnvironmentType MyEnvironmentType = EnvironmentType.NULL;
    public void OnPointerEnter(PointerEventData eventData)
    {
    Experience _exp = null;
    switch (PanelType)
    {
      case PreviewPanelType.Turn:UIManager.Instance.PreviewManager.OpenTurnPreview(transform as RectTransform);break;
        case PreviewPanelType.HP:UIManager.Instance.PreviewManager.OpenHPPreview(transform as RectTransform);break;
      case PreviewPanelType.Sanity:UIManager.Instance.PreviewManager.OpenSanityPreview(transform as RectTransform);break;
        case PreviewPanelType.Gold:UIManager.Instance.PreviewManager.OpenGoldPreview(transform as RectTransform);break;
      case PreviewPanelType.Map:UIManager.Instance.PreviewManager.OpenMapPreview();break;
      case PreviewPanelType.Quest:UIManager.Instance.PreviewManager.OpenQuestPreview();break;
      case PreviewPanelType.Skill:UIManager.Instance.PreviewManager.OpenSkillPreview(Myskill, transform as RectTransform);break;
      case PreviewPanelType.EXP_long:
         _exp = GameManager.Instance.MyGameData.LongTermEXP;
        if (_exp != null) UIManager.Instance.PreviewManager.OpenExpPreview(_exp, transform as RectTransform);
        else UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(GameManager.Instance.GetTextData("NoExp"), new Vector2(1.0f, 0.5f), transform as RectTransform);
        break;
        case PreviewPanelType.EXP_short:
         _exp = GameManager.Instance.MyGameData.ShortTermEXP[ExpIndex];
        if (_exp != null) UIManager.Instance.PreviewManager.OpenExpPreview(_exp, transform as RectTransform);
        else UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(GameManager.Instance.GetTextData("NoExp"),new Vector2(1.0f,0.5f), transform as RectTransform);
        break;
      case PreviewPanelType.Tendency:
        if (GameManager.Instance.MyGameData.GetTendencyLevel(MyTendency) == 0) return;
        UIManager.Instance.PreviewManager.OpenTendencyPreview(MyTendency, transform as RectTransform);break;
      case PreviewPanelType.Selection:
        SelectionData _selection = null;
        switch (MySelectionTendency)
        {
          case TendencyTypeEnum.None:
            _selection = GameManager.Instance.MyGameData.CurrentEvent.SelectionDatas[0];
            break;
          case TendencyTypeEnum.Head:
             _selection = GameManager.Instance.MyGameData.CurrentEvent.SelectionDatas[MySelectionTendencyDir==true?0:1];
            break;
          case TendencyTypeEnum.Body:
            _selection = GameManager.Instance.MyGameData.CurrentEvent.SelectionDatas[MySelectionTendencyDir == true ? 0 : 1];
            break;
        }
        switch (_selection.ThisSelectionType)
        {
          case SelectionTargetType.None: UIManager.Instance.PreviewManager.OpenSelectionNonePreview(_selection, MySelectionTendency, MySelectionTendencyDir, transform as RectTransform);break;
          case SelectionTargetType.Pay: UIManager.Instance.PreviewManager.OpenSelectionPayPreview(_selection, MySelectionTendency, MySelectionTendencyDir, transform as RectTransform); break;
          case SelectionTargetType.Check_Single:case SelectionTargetType.Check_Multy:
            UIManager.Instance.PreviewManager.OpenSelectionCheckPreview_skill(_selection, MySelectionTendency, MySelectionTendencyDir, transform as RectTransform); break;
          default: UIManager.Instance.PreviewManager.OpenSelectionElsePreview(_selection, MySelectionTendency, MySelectionTendencyDir, transform as RectTransform); break;
        }
        break;
      case PreviewPanelType.RewardHP:
        UIManager.Instance.PreviewManager.OpenRewardStatusPreview(StatusTypeEnum.HP, GameManager.Instance.MyGameData.RewardHPValue_modified, transform as RectTransform); break;
      case PreviewPanelType.RewardSanity:
        UIManager.Instance.PreviewManager.OpenRewardStatusPreview(StatusTypeEnum.Sanity, GameManager.Instance.MyGameData.RewardSanityValue_modified, transform as RectTransform); break;
      case PreviewPanelType.RewardGold:
        UIManager.Instance.PreviewManager.OpenRewardStatusPreview(StatusTypeEnum.Gold, GameManager.Instance.MyGameData.RewardGoldValue_modified, transform as RectTransform); break;
      case PreviewPanelType.RewardSkill:
        UIManager.Instance.PreviewManager.OpenRewardSkillPreview(Myskill, transform as RectTransform);  break;
      case PreviewPanelType.RewardExp:
        UIManager.Instance.PreviewManager.OpenRewardExpPreview(MyEXP, transform as RectTransform); break;
      case PreviewPanelType.RewardExpSelect_long:
        _exp = GameManager.Instance.MyGameData.LongTermEXP;
        if (_exp == null) UIManager.Instance.PreviewManager.OpenExpSelectionEmptyPreview(MyEXP,true, transform as RectTransform);
        else
        {
          if (_exp.ExpType.Equals(ExpTypeEnum.Normal)) UIManager.Instance.PreviewManager.OpenExpSelectionExistPreview(_exp, MyEXP, true, transform as RectTransform);
          else if (_exp.ExpType == ExpTypeEnum.Bad) UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(GameManager.Instance.GetTextData("CANNOTCHANGEBADEXP_NAME"), transform as RectTransform);
          else UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(GameManager.Instance.GetTextData("CANNOTCHANGEMADNESS_NAME"), transform as RectTransform);
        }
        break;
      case PreviewPanelType.RewardExpSelect_short:
        _exp = GameManager.Instance.MyGameData.ShortTermEXP[ExpIndex];
        if (_exp == null) UIManager.Instance.PreviewManager.OpenExpSelectionEmptyPreview(MyEXP,false, transform as RectTransform);
        else
        {
          if (_exp.ExpType.Equals(ExpTypeEnum.Normal)) UIManager.Instance.PreviewManager.OpenExpSelectionExistPreview(_exp, MyEXP,false, transform as RectTransform);
          else if (_exp.ExpType == ExpTypeEnum.Bad) UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(GameManager.Instance.GetTextData("CANNOTCHANGEBADEXP_NAME"), transform as RectTransform);
          else UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(GameManager.Instance.GetTextData("CANNOTCHANGEMADNESS_NAME"), transform as RectTransform);
        }
        break;
      case PreviewPanelType.Discomfort:
        UIManager.Instance.PreviewManager.OpenDisComfortPanel( transform as RectTransform);
        break;
      case PreviewPanelType.Environment:
        UIManager.Instance.PreviewManager.OpenEnvirPanel(MyEnvironmentType);
        break;
      case PreviewPanelType.MadnessAccept:
        UIManager.Instance.PreviewManager.OpenJustDescriptionPreview
          ( string.Format(GameManager.Instance.GetTextData("ACCEPTMADNESS_DESCRIPTOIN"), WNCText.GetSanityColor(ConstValues.MadnessMaxSanityLoseValue)), transform as RectTransform);
        break;
      case PreviewPanelType.MadnessRefuse:
        UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(
          string.Format(GameManager.Instance.GetTextData("REFUSEMADNESS_DESCRIPTION"),
          WNCText.GetHPColor(ConstValues.MadnessRefuseHPLoseCost)), transform as RectTransform);
        break;
      case PreviewPanelType.MoveCostSanity:
        UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(string.Format(GameManager.Instance.GetTextData("MAPCOSTTYPE_SANITY"), WNCText.GetSanityColor(UIManager.Instance.MyMap.SanityCost)), transform as RectTransform);
        break;
      case PreviewPanelType.MoveCostGold:
        UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(string.Format(GameManager.Instance.GetTextData("MAPCOSTTYPE_GOLD"), WNCText.GetGoldColor(UIManager.Instance.MyMap.GoldCost)), transform as RectTransform);
        break;
      case PreviewPanelType.RestSanity:
        UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(GameManager.Instance.GetTextData("REST_SANITY"), transform as RectTransform);
        break;
      case PreviewPanelType.RestGold:
        UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(GameManager.Instance.GetTextData("REST_GOLD"), transform as RectTransform);
        break;
      case PreviewPanelType.MovePoint:
        UIManager.Instance.PreviewManager.OpenMovePointPreview(transform as RectTransform);
        break;
      case PreviewPanelType.MoveCostGoldNogold:
        UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(GameManager.Instance.GetTextData("NOGOLD_TEXT"), transform as RectTransform);
        break;
    }
  }
    public void OnPointerExit(PointerEventData eventData) 
    {
    UIManager.Instance.PreviewManager.ClosePreview();
  }


}
