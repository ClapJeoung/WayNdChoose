using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PreviewPanelType { Turn,HP,Sanity,Gold,Map,Quest,Trait,Theme,Skill,EXP_long,EXP_short,Tendency,Selection,
  RewardHP,RewardSanity,RewardGold,RewardTrait,RewardTheme,RewardSkill,RewardExp,RewardSkillSelect,RewardExpSelect_long,RewardExpSelect_short,Discomfort,
Place}
public class PreviewInteractive :MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public PreviewPanelType PanelType=PreviewPanelType.Turn;
    [Space(15)]
    public ThemeType MyTheme = ThemeType.Conversation;
    public TendencyType MyTendency = TendencyType.None;
  public TendencyType MySelectionTendency= TendencyType.None;
  public bool MySelectionTendencyDir = false;
  public int MySelectionTendencyIndex = 0;
  public SkillName MySkillName = SkillName.Speech;
  public int RewardValue = 0;
  public int ExpIndex = 0;
  public Experience MyEXP = null;
  public PlaceType MyPlaceType = PlaceType.NULL;
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
      case PreviewPanelType.Theme:UIManager.Instance.PreviewManager.OpenThemePreview(MyTheme);break;
      case PreviewPanelType.Skill:UIManager.Instance.PreviewManager.OpenSkillPreview(MySkillName);break;
      case PreviewPanelType.EXP_long:
         _exp = GameManager.Instance.MyGameData.LongTermEXP[ExpIndex];
        if(_exp!=null)UIManager.Instance.PreviewManager.OpenExpPreview(_exp);
        break;
        case PreviewPanelType.EXP_short:
         _exp = GameManager.Instance.MyGameData.ShortTermEXP[ExpIndex];
        if (_exp != null) UIManager.Instance.PreviewManager.OpenExpPreview(_exp);
        break;
      case PreviewPanelType.Tendency:UIManager.Instance.PreviewManager.OpenTendencyPreview(MyTendency);break;
      case PreviewPanelType.Selection:
        SelectionData _selection = new SelectionData();
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
          case SelectionTargetType.Check_Theme:UIManager.Instance.PreviewManager.OpenSelectionCheckPreview_theme(_selection, MySelectionTendency, MySelectionTendencyDir); break;
          case SelectionTargetType.Check_Skill:UIManager.Instance.PreviewManager.OpenSelectionCheckPreview_skill(_selection, MySelectionTendency, MySelectionTendencyDir); break;
          default: UIManager.Instance.PreviewManager.OpenSelectionElsePreview(_selection, MySelectionTendency, MySelectionTendencyDir); break;
        }
        break;
      case PreviewPanelType.RewardHP:
        UIManager.Instance.PreviewManager.OpenRewardHPPreview(GameManager.Instance.MyGameData.RewardHPValue_origin); break;
      case PreviewPanelType.RewardSanity:
        UIManager.Instance.PreviewManager.OpenRewardSanityPreview(GameManager.Instance.MyGameData.RewardSanityValue_origin); break;
      case PreviewPanelType.RewardGold:
        UIManager.Instance.PreviewManager.OpenRewardGoldPreview(GameManager.Instance.MyGameData.RewardGoldValue_origin); break;
      case PreviewPanelType.RewardTheme:
        UIManager.Instance.PreviewManager.OpenRewardThemePreview(MyTheme); break;
      case PreviewPanelType.RewardSkill:
        UIManager.Instance.PreviewManager.OpenRewardSkillPreview(MySkillName);  break;
      case PreviewPanelType.RewardExp:
        UIManager.Instance.PreviewManager.OpenRewardExpPreview(MyEXP); break;
      case PreviewPanelType.RewardSkillSelect:
        UIManager.Instance.PreviewManager.OpenSkillSelectPreview(MySkillName);break;
      case PreviewPanelType.RewardExpSelect_long:
        _exp = GameManager.Instance.MyGameData.LongTermEXP[ExpIndex];
        if (_exp == null) UIManager.Instance.PreviewManager.OpenExpSelectionEmptyPreview(MyEXP,true);
        else
        {
          if (_exp.ExpType.Equals(ExpTypeEnum.Normal)) UIManager.Instance.PreviewManager.OpenExpSelectionExistPreview(_exp, MyEXP,true);
          else UIManager.Instance.PreviewManager.OpenExpSelectionBadPreview();
        }
        break;
      case PreviewPanelType.RewardExpSelect_short:
        _exp = GameManager.Instance.MyGameData.ShortTermEXP[ExpIndex];
        if (_exp == null) UIManager.Instance.PreviewManager.OpenExpSelectionEmptyPreview(MyEXP,false);
        else
        {
          if (_exp.ExpType.Equals(ExpTypeEnum.Normal)) UIManager.Instance.PreviewManager.OpenExpSelectionExistPreview(_exp, MyEXP,false);
          else UIManager.Instance.PreviewManager.OpenExpSelectionBadPreview();
        }
        break;
      case PreviewPanelType.Discomfort:
        UIManager.Instance.PreviewManager.OpenDisComfortPanel();
        break;
      case PreviewPanelType.Place:
        if (!GameManager.Instance.MyGameData.PlaceEffects.ContainsKey(MyPlaceType)) return;
        UIManager.Instance.PreviewManager.OpenPlacePanel(MyPlaceType);
        break;

    }
    }
    public void OnPointerExit(PointerEventData eventData) 
    {
    UIManager.Instance.PreviewManager.ClosePreview();
  }


}
