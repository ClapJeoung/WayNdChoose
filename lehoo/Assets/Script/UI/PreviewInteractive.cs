using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum PreviewPanelType { Turn,HP,Sanity,Gold,Map,Quest,Trait,Theme,Skill,EXP_long,EXP_short,Tendency,Selection,
  RewardHP,RewardSanity,RewardGold,RewardTrait,RewardTheme,RewardSkill,RewardExp,RewardSkillSelect,RewardExpSelect_long,RewardExpSelect_short,Discomfort,
Place,Environment,MadnessAccept,MadnessRefuse,MoveCostSanity,MoveCostGold,RestSanity,RestGold,CultPanel_Sabbat,CultPanel_Ritual,MovePoint,MoveCostGoldNogold,
CultSidePanel,TileInfo,TurnInfo,EndingPreview,ChatList,SkillProgress,SettlementReturn,EndingEvents}
public class PreviewInteractive :MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
  public PreviewPanelType PanelType=PreviewPanelType.Turn;
  public RectTransform OtherRect = null;
  public RectTransform OtherRect_other = null;
  [Space(15)]
  public TendencyTypeEnum MyTendency = TendencyTypeEnum.None;
  public TendencyTypeEnum MySelectionTendency= TendencyTypeEnum.None;
  public bool MySelectionTendencyDir = false;
  [SerializeField] private SkillTypeEnum Myskill = SkillTypeEnum.Conversation;
  [SerializeField] private int ExpIndex = 0;
  [SerializeField] private Experience MyEXP = null;
  [SerializeField] private EnvironmentType MyEnvironmentType = EnvironmentType.NULL;
  public TileData MyTileData = null;
  [SerializeField] private bool IsCultSidePanel = false;
  [SerializeField] private Button ExpButton = null;
  public string EndingID = "";
    public void OnPointerEnter(PointerEventData eventData)
    {
    UIManager.Instance.PreviewManager.ClosePreview();
    Experience _exp = null;
    switch (PanelType)
    {
      case PreviewPanelType.Turn:UIManager.Instance.PreviewManager.OpenTurnPreview(OtherRect==null?transform as RectTransform : OtherRect);break;
        case PreviewPanelType.HP:UIManager.Instance.PreviewManager.OpenHPPreview(OtherRect==null?transform as RectTransform : OtherRect);break;
      case PreviewPanelType.Sanity:UIManager.Instance.PreviewManager.OpenSanityPreview(OtherRect==null?transform as RectTransform : OtherRect);break;
        case PreviewPanelType.Gold:UIManager.Instance.PreviewManager.OpenGoldPreview(OtherRect==null?transform as RectTransform : OtherRect);break;
      case PreviewPanelType.Map:UIManager.Instance.PreviewManager.OpenMapPreview();break;
      case PreviewPanelType.Quest:UIManager.Instance.PreviewManager.OpenQuestPreview();break;
      case PreviewPanelType.Skill:UIManager.Instance.PreviewManager.OpenSkillPreview(Myskill, OtherRect==null?transform as RectTransform : OtherRect);break;
      case PreviewPanelType.EXP_long:
         _exp = GameManager.Instance.MyGameData.LongExp;
        if (_exp != null) UIManager.Instance.PreviewManager.OpenExpPreview(_exp, ExpButton, OtherRect==null?transform as RectTransform : OtherRect);
        else UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(GameManager.Instance.GetTextData("NoExp"),OtherRect==null?transform as RectTransform : OtherRect, new Vector2(1.0f, 0.5f));
        break;
        case PreviewPanelType.EXP_short:
         _exp = ExpIndex==0? GameManager.Instance.MyGameData.ShortExp_A: GameManager.Instance.MyGameData.ShortExp_B;
        if (_exp != null) UIManager.Instance.PreviewManager.OpenExpPreview(_exp, ExpButton, OtherRect ==null?transform as RectTransform : OtherRect);
        else UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(GameManager.Instance.GetTextData("NoExp"), OtherRect==null?transform as RectTransform : OtherRect, new Vector2(1.0f, 0.5f));
        break;
      case PreviewPanelType.Tendency:
        if (GameManager.Instance.MyGameData.GetTendencyLevel(MyTendency) == 0) return;
        UIManager.Instance.PreviewManager.OpenTendencyPreview(MyTendency, OtherRect==null?transform as RectTransform : OtherRect);break;
      case PreviewPanelType.Selection:
        SelectionData _selection = null;
        switch (MySelectionTendency)
        {
          case TendencyTypeEnum.None:
            _selection = GameManager.Instance.MyGameData.CurrentEvent.SelectionDatas[0];
            break;
          case TendencyTypeEnum.Head:
             _selection = GameManager.Instance.MyGameData.CurrentEvent.SelectionDatas[MySelectionTendencyDir==true?0:1];
            if (MySelectionTendencyDir && GameManager.Instance.MyGameData.Tendency_Head.Level == -2)
            {
              UIManager.Instance.PreviewManager.OpenSelectionOverTendencyPreview(MySelectionTendency, MySelectionTendencyDir, OtherRect);
              return;
            }
            else if (!MySelectionTendencyDir && GameManager.Instance.MyGameData.Tendency_Head.Level == 2)
            {
              UIManager.Instance.PreviewManager.OpenSelectionOverTendencyPreview(MySelectionTendency, MySelectionTendencyDir, OtherRect);
              return;
            }
            break;
          case TendencyTypeEnum.Body:
            _selection = GameManager.Instance.MyGameData.CurrentEvent.SelectionDatas[MySelectionTendencyDir == true ? 0 : 1];
            if (MySelectionTendencyDir && GameManager.Instance.MyGameData.Tendency_Body.Level == -2)
            {
              UIManager.Instance.PreviewManager.OpenSelectionOverTendencyPreview(MySelectionTendency, MySelectionTendencyDir, OtherRect);
              return;
            }
            else if (!MySelectionTendencyDir && GameManager.Instance.MyGameData.Tendency_Body.Level == 2)
            {
              UIManager.Instance.PreviewManager.OpenSelectionOverTendencyPreview(MySelectionTendency, MySelectionTendencyDir, OtherRect);
              return;
            }
            break;
        }
        switch (_selection.ThisSelectionType)
        {
          case SelectionTargetType.None: UIManager.Instance.PreviewManager.OpenSelectionNonePreview(_selection, MySelectionTendency, MySelectionTendencyDir, OtherRect);break;
          case SelectionTargetType.Pay: UIManager.Instance.PreviewManager.OpenSelectionPayPreview(_selection, MySelectionTendency, MySelectionTendencyDir, OtherRect); break;
          case SelectionTargetType.Check_Single:case SelectionTargetType.Check_Multy:
            UIManager.Instance.PreviewManager.OpenSelectionCheckPreview_skill(_selection, MySelectionTendency, MySelectionTendencyDir, OtherRect); break;
          default: UIManager.Instance.PreviewManager.OpenSelectionElsePreview(_selection, MySelectionTendency, MySelectionTendencyDir, OtherRect); break;
        }
        break;
      case PreviewPanelType.RewardHP:
        UIManager.Instance.PreviewManager.OpenRewardStatusPreview(StatusTypeEnum.HP, GameManager.Instance.MyGameData.RewardHPValue, OtherRect==null?transform as RectTransform : OtherRect); break;
      case PreviewPanelType.RewardSanity:
        UIManager.Instance.PreviewManager.OpenRewardStatusPreview(StatusTypeEnum.Sanity, GameManager.Instance.MyGameData.RewardSanityValue, OtherRect==null?transform as RectTransform : OtherRect); break;
      case PreviewPanelType.RewardGold:
        UIManager.Instance.PreviewManager.OpenRewardStatusPreview(StatusTypeEnum.Gold, GameManager.Instance.MyGameData.RewardGoldValue, OtherRect==null?transform as RectTransform : OtherRect); break;
      case PreviewPanelType.RewardSkill:
        UIManager.Instance.PreviewManager.OpenRewardSkillPreview(Myskill, OtherRect==null?transform as RectTransform : OtherRect);  break;
      case PreviewPanelType.RewardExp:
        UIManager.Instance.PreviewManager.OpenRewardExpPreview(MyEXP, OtherRect==null?transform as RectTransform : OtherRect); break;
      case PreviewPanelType.RewardExpSelect_long:
        _exp = UIManager.Instance.ExpRewardUI.CurrentExp;
        UIManager.Instance.PreviewManager.OpenExpSelectionEmptyPreview(_exp, true, OtherRect==null?transform as RectTransform : OtherRect);
        break;
        break;
      case PreviewPanelType.RewardExpSelect_short:
        _exp = UIManager.Instance.ExpRewardUI.CurrentExp;
        UIManager.Instance.PreviewManager.OpenExpSelectionEmptyPreview(_exp, false, OtherRect==null?transform as RectTransform : OtherRect);
        break;
        break;
      case PreviewPanelType.Discomfort:
        UIManager.Instance.PreviewManager.OpenDisComfortPanel( OtherRect==null?transform as RectTransform : OtherRect);
        break;
      case PreviewPanelType.Environment:
        UIManager.Instance.PreviewManager.OpenEnvirPanel(MyEnvironmentType);
        break;
      case PreviewPanelType.MadnessAccept:
        break;
      case PreviewPanelType.MadnessRefuse:
        break;
      case PreviewPanelType.MoveCostSanity:
        string _sanitycosttext = string.Format(GameManager.Instance.GetTextData("MoveCost_Santiy"),
          WNCText.GetSanityColor(UIManager.Instance.MapUI.MoveCost_Sanity)+(UIManager.Instance.MapUI.IsMad?WNCText.GetMadnessColor("?"):""));
        if (UIManager.Instance.MapUI.TotalSupplyCost > GameManager.Instance.MyGameData.Supply)
          _sanitycosttext += string.Format(string.Format(GameManager.Instance.GetTextData("MoveCost_Penalty"),
            UIManager.Instance.MapUI.TotalSupplyCost - GameManager.Instance.MyGameData.Supply,
           WNCText.GetSanityColor((UIManager.Instance.MapUI.TotalSupplyCost - GameManager.Instance.MyGameData.Supply) * GameManager.Instance.MyGameData.Movecost_supplylack)));

        UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(
          _sanitycosttext
          ,OtherRect==null?transform as RectTransform : OtherRect, new Vector2(0.5f, -0.05f));
        break;
      case PreviewPanelType.MoveCostGold:
        int _defaultvalue = UIManager.Instance.MapUI.MoveCost_Sanity;
        int _requiregold = UIManager.Instance.MapUI.MoveCost_Gold;
        string _goldcosttext = "";
        if (GameManager.Instance.MyGameData.Gold >= _requiregold)
          _goldcosttext = string.Format(GameManager.Instance.GetTextData("MoveCost_Gold_full"), WNCText.GetGoldColor(_requiregold) + (UIManager.Instance.MapUI.IsMad ? WNCText.GetMadnessColor("?") : ""));
        else
          _goldcosttext = string.Format(GameManager.Instance.GetTextData("MoveCost_Gold_lack"),
            WNCText.GetGoldColor(GameManager.Instance.MyGameData.Gold),
           WNCText.GetSanityColor(_defaultvalue - GameManager.Instance.MyGameData.Gold * 2)) + (UIManager.Instance.MapUI.IsMad ? WNCText.GetMadnessColor("?") : "");

        if (UIManager.Instance.MapUI.TotalSupplyCost > GameManager.Instance.MyGameData.Supply)
          _goldcosttext += string.Format(GameManager.Instance.GetTextData("MoveCost_Penalty"),
            UIManager.Instance.MapUI.TotalSupplyCost - GameManager.Instance.MyGameData.Supply,
            WNCText.GetSanityColor((UIManager.Instance.MapUI.TotalSupplyCost - GameManager.Instance.MyGameData.Supply) * GameManager.Instance.MyGameData.Movecost_supplylack));

        UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(
  _goldcosttext
  , OtherRect == null ? transform as RectTransform : OtherRect, new Vector2(0.5f, -0.05f));

        break;
      case PreviewPanelType.RestSanity:
        UIManager.Instance.PreviewManager.OpenRestPanel(OtherRect, UIManager.Instance.DialogueUI.DiscomfortValue,
         UIManager.Instance.DialogueUI.SanityCost, UIManager.Instance.DialogueUI.SupplyValue);

        break;
      case PreviewPanelType.RestGold:
        UIManager.Instance.PreviewManager.OpenRestPanel(OtherRect, UIManager.Instance.DialogueUI.DiscomfortValue,
       UIManager.Instance.DialogueUI.GoldCost, UIManager.Instance.DialogueUI.SupplyValue, GameManager.Instance.Status.RestSanityRestore);

        break;
      case PreviewPanelType.MovePoint:
        UIManager.Instance.PreviewManager.OpenMovePointPreview(OtherRect==null?transform as RectTransform : OtherRect);
        break;
      case PreviewPanelType.MoveCostGoldNogold:
        UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(GameManager.Instance.GetTextData("NOGOLD_TEXT"), OtherRect==null?transform as RectTransform : OtherRect);
        break;
      case PreviewPanelType.CultSidePanel:
        string _cultinfo = string.Format(GameManager.Instance.GetTextData("Cult_Preview_progress"),Mathf.FloorToInt(GameManager.Instance.MyGameData.Quest_Cult_Progress));
        string _sametext = "";
        switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
        {
          case 0:
            _sametext = GameManager.Instance.GetTextData("Village");
            _cultinfo += string.Format(GameManager.Instance.GetTextData("Cult_Preview_Settlement"),
              _sametext, GameManager.Instance.MyGameData.GetCultProgress(0),
              GameManager.Instance.Status.Quest_Cult_Village_Bonus);
            break;
          case 1:
            _sametext = GameManager.Instance.GetTextData("Town");
            _cultinfo += string.Format(GameManager.Instance.GetTextData("Cult_Preview_Settlement"),
             _sametext, GameManager.Instance.MyGameData.GetCultProgress(1),
             GameManager.Instance.Status.Quest_Cult_Town_Bonus);
            break;
          case 2:
            _sametext = GameManager.Instance.GetTextData("City");
            _cultinfo += string.Format(GameManager.Instance.GetTextData("Cult_Preview_Settlement"),
            _sametext, GameManager.Instance.MyGameData.GetCultProgress(2),
            GameManager.Instance.Status.Quest_Cult_City_Bonus);
            break;
          case 3:
            _sametext = GameManager.Instance.GetTextData(GameManager.Instance.MyGameData.Cult_SabbatSector,0);
            _cultinfo += string.Format(GameManager.Instance.GetTextData("Cult_Preview_Sabbat"),
           _sametext, GameManager.Instance.MyGameData.Cult_Sabbat_MinDiscomfort,_sametext, GameManager.Instance.MyGameData.GetCultProgress(3)
           , GameManager.Instance.Status.Quest_Cult_Sabbat_Bonus);
            break;
          case 4:
            _cultinfo += string.Format(GameManager.Instance.GetTextData("Cult_Preview_Ritual"),
            GameManager.Instance.MyGameData.Cult_Ritual_MinLength, GameManager.Instance.MyGameData.GetCultProgress(4),
            GameManager.Instance.Status.Quest_Cult_Ritual_Bonus);
            break;
        }
        UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(_cultinfo,OtherRect==null?transform as RectTransform : OtherRect, IsCultSidePanel ? new Vector2(1.05f, 0.5f) : new Vector2(0.5f, 1.05f));
        break;
      case PreviewPanelType.TileInfo:
        UIManager.Instance.PreviewManager.OpenTileInfoPreveiew( MyTileData,
          MyTileData.TileSettle != null && (MyTileData.Coordinate.y - GameManager.Instance.MyGameData.Coordinate.y == -3) ? OtherRect_other : MyTileData.Coordinate.y <= GameManager.Instance.MyGameData.Coordinate.y ? OtherRect:OtherRect_other);
        break;
      case PreviewPanelType.TurnInfo:

        string _year = GameManager.Instance.MyGameData.Year.ToString();
        string _season = "";
        switch (GameManager.Instance.MyGameData.Turn)
        {
          case 0:
            _season = GameManager.Instance.GetTextData("Spring");
            break;
          case 1:
            _season = GameManager.Instance.GetTextData("Summer");
            break;
          case 2:
            _season = GameManager.Instance.GetTextData("Autumn");
            break;
          case 3:
            _season = GameManager.Instance.GetTextData("Winter");
            break;
        }

        string _text = string.Format(GameManager.Instance.GetTextData("TurnPreviewInfo"),
          _year, _season,
          (int)(Mathf.Lerp(GameManager.Instance.Status.MoveCost_Min, GameManager.Instance.Status.MoveCost_Max, GameManager.Instance.MyGameData.LerpByTurn)/ GameManager.Instance.Status.MoveCost_Min * 100.0f-100),
          (int)(Mathf.Lerp(GameManager.Instance.Status.RestCost_Default_Min, GameManager.Instance.Status.RestCost_Default_Max, GameManager.Instance.MyGameData.LerpByTurn)/ GameManager.Instance.Status.RestCost_Default_Min * 100.0f-100),
          (int)GameManager.Instance.MyGameData.MinSuccesPer,
          GameManager.Instance.MyGameData.CheckSkillSingleValue,
          GameManager.Instance.MyGameData.CheckSkillMultyValue,
          WNCText.GetHPColor("+"+(int)(Mathf.Lerp(GameManager.Instance.Status.PayHP_min, GameManager.Instance.Status.PayHP_max, GameManager.Instance.MyGameData.LerpByTurn)/ GameManager.Instance.Status.PayHP_min * 100.0f-100)+"%"),
          WNCText.GetSanityColor("+" + (int)(Mathf.Lerp(GameManager.Instance.Status.PaySanity_min, GameManager.Instance.Status.PaySanity_max, GameManager.Instance.MyGameData.LerpByTurn)/ GameManager.Instance.Status.PaySanity_min * 100.0f-100) + "%"),
          WNCText.GetGoldColor("+" + (int)(Mathf.Lerp(GameManager.Instance.Status.PayGold_min, GameManager.Instance.Status.PayGold_max, GameManager.Instance.MyGameData.LerpByTurn)/ GameManager.Instance.Status.PayGold_min * 100.0f-100) + "%")
          );

        UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(_text,OtherRect,new Vector2(-0.05f,1.05f));        break;
      case PreviewPanelType.EndingPreview:
        if (!GameManager.Instance.ProgressData.EndingLists.Contains(EndingID))
          UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(GameManager.Instance.GetTextData(EndingID + "_preview_closed"), OtherRect,new Vector2(0.5f,1.05f));
        else
        {
          EndingData _ending = GameManager.Instance.ImageHolder.GetEndingData(EndingID);
          UIManager.Instance.PreviewManager.OpenEndingPreviewPanel(OtherRect, _ending.PreviewIcon, _ending.Preview_Name, _ending.Preview_Opened);
        }
        break;
      case PreviewPanelType.ChatList:
        UIManager.Instance.PreviewManager.OpenChatListPanel(OtherRect, MySelectionTendency,MySelectionTendencyDir);
        break;
      case PreviewPanelType.SkillProgress:
        UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(
          GameManager.Instance.MyGameData.SkillProgress >= GameManager.Instance.MyGameData.SkillProgressRequire ?
          GameManager.Instance.GetTextData("SkillProgress_Preview_full") :
          string.Format(GameManager.Instance.GetTextData("SkillProgress_Preview_less"),
          GameManager.Instance.MyGameData.SkillProgressRequire - GameManager.Instance.MyGameData.SkillProgress),
          OtherRect, new Vector2(1.05f, 0.5f));
        break;
      case PreviewPanelType.SettlementReturn:
        string[] _strs = GameManager.Instance.GetTextData("ReturntoSettlement_add").Split('@');
        UIManager.Instance.PreviewManager.OpenJustDescriptionPreview(
          GameManager.Instance.GetTextData("ReturntoSettlement_default") + _strs[Random.Range(0, _strs.Length)], OtherRect, new Vector2(0.5f, -0.05f));
        break;
      case PreviewPanelType.EndingEvents:
        string _success = "", _fail = "";
        switch (MyTendency)
        {
          case TendencyTypeEnum.Body:
            if (MySelectionTendencyDir)
            {
              _success = UIManager.Instance.CurtainUI.Success_Logical;
              _fail = UIManager.Instance.CurtainUI.Fail_Logical;
            }
            else
            {
              _success = UIManager.Instance.CurtainUI.Success_Physical;
              _fail = UIManager.Instance.CurtainUI.Fail_Physical;
            }
            break;
          case TendencyTypeEnum.Head:
            if (MySelectionTendencyDir)
            {
              _success = UIManager.Instance.CurtainUI.Success_Mental;
              _fail = UIManager.Instance.CurtainUI.Fail_Mental;
            }
            else
            {
              _success = UIManager.Instance.CurtainUI.Success_Material;
              _fail = UIManager.Instance.CurtainUI.Fail_Material;
            }
            break;
        }
        if (_success == "" && _fail == "") return;
        Sprite _background = GameManager.Instance.ImageHolder.SelectionBackground(MyTendency, MySelectionTendencyDir);
        UIManager.Instance.PreviewManager.OpenEndingEventPanel(OtherRect, _background, _success, _fail);
        break;
    }
  }
    public void OnPointerExit(PointerEventData eventData) 
    {
    UIManager.Instance.PreviewManager.ClosePreview();
  }


}
