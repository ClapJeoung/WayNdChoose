using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PreviewManager : MonoBehaviour
{
//  [SerializeField] private RectTransform WholeRect = null;
  [SerializeField] private GameObject IconAndDescription_Panel = null;
 // [SerializeField] private Image IconAndDescription_IconBackground = null;
  [SerializeField] private Image IconAndDescription_Icon = null;
  [SerializeField] private TextMeshProUGUI IconAndDescription_Description = null;
  [Space(10)]
  [SerializeField] private GameObject JustDescription_Panel = null;
  [SerializeField] private TextMeshProUGUI JustDescriptionText = null;
  private int EffectFontSize = 20;
  private int SubdescriptionSize = 16;
  private Vector2 TurnPivot = new Vector2(0.5f, 1.1f);
  private Vector2 StatusPivot= new Vector2(1.05f, 1.05f);
  private Vector2 MapPivot= new Vector2(0.5f, 1.1f);
  private Vector2 TendencyPivot = new Vector2(1.1f, -0.1f);
  private Vector2 DiscomfortPivot = new Vector2(0.5f, 1.3f);
  [Space(10)]
  [SerializeField] private GameObject SkillPreview = null;
  [SerializeField] private Image SkillIcon = null;
  [SerializeField] private TextMeshProUGUI SkillLevel = null;
  [SerializeField] private TextMeshProUGUI SkillName = null;
  [SerializeField] private GameObject SkillMadnessHolder = null;
  [SerializeField] private TextMeshProUGUI SkillMadnessInfo = null;
  [SerializeField] private TextMeshProUGUI SkillSubdescription = null;
  [Space(10)]
  [SerializeField] private GameObject ExpPreview = null;
  [SerializeField] private TextMeshProUGUI ExpName = null;
  [SerializeField] private Image ExpIllust = null;
  [SerializeField] private TextMeshProUGUI ExpDescription = null;
  [SerializeField] private TextMeshProUGUI ExpSubdescription = null;
  [Space(10)]
  [SerializeField] private GameObject TendencyPreview = null;
  [SerializeField] private Image TendencyIcon_Current = null;
  [SerializeField] private GameObject TendencyProgress_Left = null;
  [SerializeField] private Image TendencyIcon_Left = null;
  [SerializeField] private Image[] TendencyArrows_Left = null;
  [SerializeField] private GameObject TendencyProgress_Right = null;
  [SerializeField] private Image TendencyIcon_Right = null;
  [SerializeField] private Image[] TendencyArrows_Right = null;
  [SerializeField] private TextMeshProUGUI TendencyName = null;
  [SerializeField] private TextMeshProUGUI TendencyDescription = null;
  [Space(10)]
  [SerializeField] private GameObject SelectionNonePanel = null;
  [SerializeField] private Image SelectionNoneBackground = null;
  //[SerializeField] private TextMeshProUGUI SelectionNoneText = null;
  // [SerializeField] private PreviewSelectionTendency SelectionNoneTendency = null;
  [SerializeField] private PreviewRewardGroup SelectionNoneReward = null;
  [Space(10)]
  [SerializeField] private GameObject SelectionPayPanel = null;
  [SerializeField] private Image SelectionPayBackground = null;
//  [SerializeField] private Image PayIcon = null;
 // [SerializeField] private TextMeshProUGUI PayInfo = null;
//  [SerializeField] private TextMeshProUGUI PayRequireValue = null;
 // [SerializeField] private GameObject PayNoGoldHolder = null;
  [SerializeField] private TextMeshProUGUI PayNoGold_Text = null;
  // [SerializeField] private TextMeshProUGUI PayNoGold_PercentText = null;
  //[SerializeField] private TextMeshProUGUI PayNoGold_PercentValue = null;
  //[SerializeField] private TextMeshProUGUI PayNoGold_Alternative = null;
  // [SerializeField] private TextMeshProUGUI PaySubDescription = null;
  [SerializeField] private PreviewRewardGroup SelectionPayReward = null;
  // [SerializeField] private PreviewSelectionTendency SelectionPayTendendcy = null;
  [Space(10)]
  [SerializeField] private GameObject SelectionCheckPanel = null;
  [SerializeField] private Image SelectionCheckBackground = null;
 // [SerializeField] private Image[] SelectionCheckIcons = null;
//  [SerializeField] private TextMeshProUGUI SelectionCheckInfo = null;
 // [SerializeField] private TextMeshProUGUI SelectionCheckCurrentLevel = null;
  [SerializeField] private TextMeshProUGUI SelectionCheckPercent_text = null;
  [SerializeField] private TextMeshProUGUI SelectionCheckPercent_int = null;
  [SerializeField] private PreviewRewardGroup SelectionCheckReward = null;
  //  [SerializeField] private TextMeshProUGUI SelectionCheckDescription = null;
  // [SerializeField] private PreviewSelectionTendency SelectionCheckTendendcy = null;
  [Space(10)]
  [SerializeField] private GameObject SelectionElsePanel = null;
  [SerializeField] private Image SelectionElseBackground = null;
  [SerializeField] private Image SelectionElseIcon = null;
  [Space(10)]
  [SerializeField] private GameObject ExpSelectEmptyPanel = null;
  [SerializeField] private Image ExpSelectEmptyIllust = null;
  [SerializeField] private TextMeshProUGUI ExpSelectEmptyDescription = null;
  [SerializeField] private TextMeshProUGUI ExpSelectEmptyTurn = null;
  [SerializeField] private TextMeshProUGUI ExpSelectEmptyEffect = null;
  [Space(10)]
  [SerializeField] private GameObject ExpSelectExistPanel = null;
  [SerializeField] private Image ExpSelectExistOriginIllust = null;
  [SerializeField] private TextMeshProUGUI ExpSelectOriginTurn = null;
  [SerializeField] private TextMeshProUGUI ExpSelectOriginEffect = null;
  [SerializeField] private Image ExpSelectExistNewIllust = null;
  [SerializeField] private TextMeshProUGUI ExpSelectNewTurn = null;
  [SerializeField] private TextMeshProUGUI ExpSelectNewEffect = null;
  [SerializeField] private TextMeshProUGUI ExpSelecitonExistDescription = null;
  [SerializeField] private TextMeshProUGUI ExpSelectClickText = null;
  [Space(10)]
  [SerializeField] private GameObject TileInfoPanel = null;
  [Space(10)]
  [SerializeField] private GameObject SettlementInfoPanel = null;
  [SerializeField] private List<Image> SettlementSectorIcons = new List<Image>();
  [SerializeField] private Image SettlementIcon = null;
  [SerializeField] private TextMeshProUGUI SettlementInfoName = null;
  [SerializeField] private TextMeshProUGUI SettlementInfoDiscomfort = null;
  private RectTransform CurrentPreview = null;
  private void OpenPreviewPanel(GameObject panel,Vector2 pivot,RectTransform rect)
  {
    CurrentPreview = panel.GetComponent<RectTransform>();
    CurrentPreview.pivot = pivot;

    LayoutRebuilder.ForceRebuildLayoutImmediate(panel.transform as RectTransform);
   
    CurrentPreview.position = rect.position;
    CurrentPreview.anchoredPosition3D = new Vector3(CurrentPreview.anchoredPosition.x, CurrentPreview.anchoredPosition.y, 0.0f);
  
    CurrentPreview.GetComponent<CanvasGroup>().alpha = 1.0f;

    UIManager.Instance.AudioManager.PlaySFX(0, "preview");
  }
  private void OpenPreviewPanel(GameObject panel,RectTransform rect)
  {
    CurrentPreview = panel.GetComponent<RectTransform>();

    LayoutRebuilder.ForceRebuildLayoutImmediate(panel.transform as RectTransform);

    CurrentPreview.position = rect.position;
    CurrentPreview.anchoredPosition3D = new Vector3(CurrentPreview.anchoredPosition.x, CurrentPreview.anchoredPosition.y, 0.0f);

    CurrentPreview.GetComponent<CanvasGroup>().alpha = 1.0f;
  
    UIManager.Instance.AudioManager.PlaySFX(0,"preview");
  }
  public void OpenTurnPreview(RectTransform rect)
  {
      int _currentturn = GameManager.Instance.MyGameData.Turn;
      Sprite _turnsprite = null;
    string _name = "", _description = "";
      switch (_currentturn)
      {
        case 0: _turnsprite = GameManager.Instance.ImageHolder.SpringIcon_active;
        _name = GameManager.Instance.GetTextData("SPRING_NAME");
        _description = GameManager.Instance.GetTextData("SPRING_DESCRIPTION");
        break;

        case 1: _turnsprite = GameManager.Instance.ImageHolder.SummerIcon_active; 
        _name = GameManager.Instance.GetTextData("SUMMER_NAME");
        _description = GameManager.Instance.GetTextData("SUMMER_DESCRIPTION");
        break;

      case 2: _turnsprite = GameManager.Instance.ImageHolder.FallIcon_active;
        _name = GameManager.Instance.GetTextData("AUTUMN_NAME");
        _description = GameManager.Instance.GetTextData("AUTUMN_DESCRIPTION");
        break;

      case 3: _turnsprite = GameManager.Instance.ImageHolder.WinterIcon_active; 
        _name = GameManager.Instance.GetTextData("WINTER_NAME");
        _description = GameManager.Instance.GetTextData("WINTER_DESCRIPTION");
        break;

    }

    OpenIconAndDescriptionPanel(_turnsprite, _name + "<br><br>" + WNCText.SetSize(SubdescriptionSize, WNCText.GetSubdescriptionColor(_description)), TurnPivot,false, rect);
  }//턴 미리보기 패널 세팅 후 열기
  public void OpenHPPreview(RectTransform rect)
  {
    StatusTypeEnum _currenttype = StatusTypeEnum.HP;

    string _description =string.Format(GameManager.Instance.GetTextData(_currenttype, 3),GameManager.Instance.MyGameData.MadnessSafe?
      GameManager.Instance.GetTextData("HP_MadnessSafe"):
      string.Format(GameManager.Instance.GetTextData("HP_MadnessDanger"),WNCText.GetHPColor(GameManager.Instance.MyGameData.MinMadnessHP+1)));

    int _modifyvalue = (int)GameManager.Instance.MyGameData.GetHPLossModify(false,0);
    if (_modifyvalue == 100)
    {

    }
    else
    {
      _description += "<br><br>" + GameManager.Instance.GetTextData(_currenttype, 12) + " "
        + string.Format("{0}%", WNCText.PositiveColor("+" + (100 - _modifyvalue).ToString()));
    }

    OpenIconAndDescriptionPanel(GameManager.Instance.ImageHolder.HPIcon, _description, StatusPivot, true,rect);
  }//체력 설명, 증감량 표기 후 열기
  public void OpenSanityPreview(RectTransform rect)
  {
    StatusTypeEnum _currenttype = StatusTypeEnum.Sanity;

    string _description = GameManager.Instance.GetTextData(_currenttype, 3)+
      (GameManager.Instance.MyGameData.Sanity>100?string.Format(GameManager.Instance.GetTextData("Sanity_Over100"),WNCText.GetMaxSanityColor(100)):"");

    int  _modifyvalue= (int)GameManager.Instance.MyGameData.GetSanityLossModify(false,0);
    if (_modifyvalue == 100)
    {

    }
    else
    {
      _description+="<br><br>"+ GameManager.Instance.GetTextData(_currenttype, 12) + " " 
        + string.Format("{0}%", WNCText.PositiveColor("+" + (100- _modifyvalue).ToString()));
    }

    OpenIconAndDescriptionPanel(GameManager.Instance.ImageHolder.SanityIcon, _description, StatusPivot, true, rect);
  }//정신력 설명,증감량 표기 후 열기
  public void OpenGoldPreview( RectTransform rect)
  {
    StatusTypeEnum _currenttype = StatusTypeEnum.Gold;

    string _description = GameManager.Instance.GetTextData(_currenttype, 3);

    int _modifyvalue = (int)GameManager.Instance.MyGameData.GetGoldGenModify(false);
    if (_modifyvalue == 100)
    {

    }
    else
    {
      _description += "<br><br>" + GameManager.Instance.GetTextData(_currenttype, 12) + " "
        + string.Format("{0}%", WNCText.PositiveColor("+" + (_modifyvalue-100).ToString()));
    }

    OpenIconAndDescriptionPanel(GameManager.Instance.ImageHolder.GoldIcon, _description, StatusPivot, true, rect);
  }//골드 설명,증감량 표기 후 열기
  public void OpenMovePointPreview( RectTransform rect)
  {
    Sprite _icon = GameManager.Instance.ImageHolder.MovePointIcon_Enable;
    string _description = GameManager.Instance.GetTextData("MOVEPOINT_DESCRIPTION");
    if (GameManager.Instance.MyGameData.Supply < 0) _description +="<br>"+ GameManager.Instance.GetTextData("Movepoint_NoSupplies");


    OpenIconAndDescriptionPanel(_icon, _description, StatusPivot, true, rect);
  }
  public void OpenMapPreview()
  {
    Debug.Log("이거 어디서 나옴???");
  }//현재 이동 가능 여부에 따라 텍스트만 출력
  public void OpenQuestPreview()
  {
  }//현재 퀘스트 이름, 일러스트, 다음 내용                             수정요망
  public void OpenSkillPreview(SkillTypeEnum _skilltype,RectTransform rect)
  {
    Sprite _icon = GameManager.Instance.ImageHolder.GetSkillIcon(_skilltype,false);

    int _level = GameManager.Instance.MyGameData.GetSkill(_skilltype).Level ;
    string _leveltext = WNCText.UIIdleColor(_level);
    SkillName.text = GameManager.Instance.GetTextData(_skilltype, 0);
    SkillIcon.sprite = _icon;

    switch (_skilltype)
    {
      case SkillTypeEnum.Conversation:
        if (GameManager.Instance.MyGameData.Madness_Conversation == true)
        {
          if (SkillMadnessHolder.activeInHierarchy == false) SkillMadnessHolder.SetActive(true);

          SkillMadnessInfo.text = string.Format(GameManager.Instance.GetTextData("Madness_Conversation_Preview"), ConstValues.MadnessEffect_Conversation);
          _leveltext = WNCText.GetMadnessColor(_level);
        }
        else
        {
          if (SkillMadnessHolder.activeInHierarchy == true) SkillMadnessHolder.SetActive(false);
        }
        break;
      case SkillTypeEnum.Force:
        if (GameManager.Instance.MyGameData.Madness_Force == true)
        {
          if (SkillMadnessHolder.activeInHierarchy == false) SkillMadnessHolder.SetActive(true);

          SkillMadnessInfo.text = string.Format(GameManager.Instance.GetTextData("Madness_Force_Preview"),
            GameManager.Instance.MyGameData.TotalRestCount%ConstValues.MadnessEffect_Force+1, ConstValues.MadnessEffect_Force);
          _leveltext = WNCText.GetMadnessColor(_level);
        }
        else
        {
          if (SkillMadnessHolder.activeInHierarchy == true) SkillMadnessHolder.SetActive(false);
        }
        break;
      case SkillTypeEnum.Wild:
        if (GameManager.Instance.MyGameData.Madness_Wild == true)
        {
          if (SkillMadnessHolder.activeInHierarchy == false) SkillMadnessHolder.SetActive(true);

          SkillMadnessInfo.text = string.Format(GameManager.Instance.GetTextData("Madness_Wild_Preview"),
            GameManager.Instance.MyGameData.TotalMoveCount % ConstValues.MadnessEffect_Wild_temporary+1, ConstValues.MadnessEffect_Wild_temporary,
            ConstValues.MadnessEffect_Wild_range);
          _leveltext = WNCText.GetMadnessColor(_level);
        }
        else
        {
          if (SkillMadnessHolder.activeInHierarchy == true) SkillMadnessHolder.SetActive(false);
        }
        break;
      case SkillTypeEnum.Intelligence:
        if (GameManager.Instance.MyGameData.Madness_Intelligence == true)
        {
          if (SkillMadnessHolder.activeInHierarchy == false) SkillMadnessHolder.SetActive(true);

          SkillMadnessInfo.text = string.Format(GameManager.Instance.GetTextData("Madness_Intelligence_Preview"), ConstValues.MadnessEffect_Intelligence);
          _leveltext = WNCText.GetMadnessColor(_level);
        }
        else
        {
          if (SkillMadnessHolder.activeInHierarchy == true) SkillMadnessHolder.SetActive(false);
        }
        break;
    }
    SkillLevel.text = _leveltext;
    SkillSubdescription.text = GameManager.Instance.GetTextData(_skilltype, 4);
    OpenPreviewPanel(SkillPreview,rect);
  }
  public void OpenExpPreview(Experience _exp, RectTransform rect)
  {
    ExpName.text =_exp.Name;
//    ExpDuration.text = $"{_exp.Duration}";
    ExpIllust.sprite = _exp.Illust;
    string _description = WNCText.SetSize(EffectFontSize, _exp.EffectString);
    ExpDescription.text = _description;
    ExpSubdescription.text = WNCText.SetSize(SubdescriptionSize, WNCText.GetSubdescriptionColor(_exp.Description));

    OpenPreviewPanel(ExpPreview,rect);
  }
  public void OpenTendencyPreview(TendencyTypeEnum _type, RectTransform rect)
  {
    Tendency _targettendency = null;
    Sprite _arrowsprite_left = null, _arrowsprite_right = null;
    Sprite _emptyarrow = GameManager.Instance.ImageHolder.Arrow_Empty;
  //  Sprite _disablearrow = GameManager.Instance.ImageHolder.Arrow_White;
 switch (_type)
    {
      case TendencyTypeEnum.Head:
        _targettendency = GameManager.Instance.MyGameData.Tendency_Head;
        _arrowsprite_left = GameManager.Instance.ImageHolder.Arrow_Active_mental;
        _arrowsprite_right = GameManager.Instance.ImageHolder.Arrow_Active_material;
        break;
      case TendencyTypeEnum.Body:
        _targettendency = GameManager.Instance.MyGameData.Tendency_Body;
        _arrowsprite_left = GameManager.Instance.ImageHolder.Arrow_Active_rational;
        _arrowsprite_right = GameManager.Instance.ImageHolder.Arrow_Active_physical;
        break;
    }
    string _targetselectionname = _targettendency.Type == TendencyTypeEnum.Body ?
      _targettendency.Level < 0 ? GameManager.Instance.GetTextData("Selection_logic") : GameManager.Instance.GetTextData("Selection_physical") :
      _targettendency.Level < 0 ? GameManager.Instance.GetTextData("Selection_mental") : GameManager.Instance.GetTextData("Selection_material");
    string _targetpenaltynames = Mathf.Abs(_targettendency.Level) == 1 ? GameManager.Instance.GetTextData("Fail") :
      GameManager.Instance.GetTextData("Success") + "," + GameManager.Instance.GetTextData("Fail");
    Sprite _icon = _targettendency.CurrentIcon;
    Sprite _icon_left=_targettendency.GetNextIcon(true), _icon_right=_targettendency.GetNextIcon(false);
    int _progress =_targettendency.Progress;
    switch (_targettendency.Level)
    {
      case -2:
        if(TendencyProgress_Left.activeInHierarchy==true) TendencyProgress_Left.SetActive(false);

        if (_progress <= 0)
        {
          for (int i = 0; i < ConstValues.TendencyProgress_1to2;i++)
          {
            if (i < ConstValues.TendencyRegress)
            {
              if (TendencyArrows_Right[i].gameObject.activeInHierarchy == false) TendencyArrows_Right[i].gameObject.SetActive(true);
              TendencyArrows_Right[i].sprite = _emptyarrow;
            }
            else
            {
              if (TendencyArrows_Right[i].gameObject.activeInHierarchy == true) TendencyArrows_Right[i].gameObject.SetActive(false);
            }
          }
        }
        else
        {
          for (int i = 0; i < ConstValues.TendencyProgress_1to2; i++)
          {
            if (i <= _progress - 1)
            {
              if (TendencyArrows_Right[i].gameObject.activeInHierarchy == false) TendencyArrows_Right[i].gameObject.SetActive(true);
              TendencyArrows_Right[i].sprite = _arrowsprite_right;
            }
            else if (i < ConstValues.TendencyRegress)
            {
              if (TendencyArrows_Right[i].gameObject.activeInHierarchy == false) TendencyArrows_Right[i].gameObject.SetActive(true);
              TendencyArrows_Right[i].sprite = _emptyarrow;
            }
            else
            {
              if (TendencyArrows_Right[i].gameObject.activeInHierarchy == true) TendencyArrows_Right[i].gameObject.SetActive(false);
            }
          }

        }
        break;
      case -1:
        if (TendencyProgress_Left.activeInHierarchy == false) TendencyProgress_Left.SetActive(true);
        if (_progress >= 0)
        {
          for(int i = 0; i < 3; i++)
          {
            if (i < ConstValues.TendencyProgress_1to2)
            {
              if (TendencyArrows_Left[i].gameObject.activeInHierarchy == false) TendencyArrows_Left[i].gameObject.SetActive(true);
              TendencyArrows_Left[i].sprite = _emptyarrow;
            }
            else
            { 
              if (TendencyArrows_Left[i].gameObject.activeInHierarchy == true) TendencyArrows_Left[i].gameObject.SetActive(false);
            }
          }
        }
        else
        {
          for (int i = 0;i< 3; i++)
          {
            if (i <= _progress*-1-1)
            {
              if (TendencyArrows_Left[i].gameObject.activeInHierarchy == false) TendencyArrows_Left[i].gameObject.SetActive(true);
              TendencyArrows_Left[i].sprite = _arrowsprite_left;
            }
            else if (i < ConstValues.TendencyProgress_1to2)
            {
              if (TendencyArrows_Left[i].gameObject.activeInHierarchy == false) TendencyArrows_Left[i].gameObject.SetActive(true);
              TendencyArrows_Left[i].sprite = _emptyarrow;
            }
            else
            {
              if (TendencyArrows_Left[i].gameObject.activeInHierarchy == true) TendencyArrows_Left[i].gameObject.SetActive(false);
            }

          }
        }

        if (TendencyProgress_Right.activeInHierarchy == false) TendencyProgress_Right.SetActive(true);
        if (_progress <= 0)
        {
          for (int i = 0; i < 3; i++)
          {
            if (i < ConstValues.TendencyRegress)
            {
              if (TendencyArrows_Right[i].gameObject.activeInHierarchy == false) TendencyArrows_Right[i].gameObject.SetActive(true);
              TendencyArrows_Right[i].sprite = _emptyarrow;
            }
            else
            {
              if (TendencyArrows_Right[i].gameObject.activeInHierarchy == true) TendencyArrows_Right[i].gameObject.SetActive(false);
            }
          }
        }
        else
        {
          for (int i = 0; i < ConstValues.TendencyProgress_1to2; i++)
          {
            if (i <= _progress - 1)
            {
              if (TendencyArrows_Right[i].gameObject.activeInHierarchy == false) TendencyArrows_Right[i].gameObject.SetActive(true);
              TendencyArrows_Right[i].sprite = _arrowsprite_right;
            }
            else
            {
              if (i < ConstValues.TendencyRegress)
              {
                if (TendencyArrows_Right[i].gameObject.activeInHierarchy == false) TendencyArrows_Right[i].gameObject.SetActive(true);
                TendencyArrows_Right[i].sprite = _emptyarrow;
              }
              else
              {
                if (TendencyArrows_Right[i].gameObject.activeInHierarchy == true) TendencyArrows_Right[i].gameObject.SetActive(false);
              }
            }
          }
        }
        break;
      case 1:
        if (TendencyProgress_Left.activeInHierarchy == false) TendencyProgress_Left.SetActive(true);
        if (_progress >= 0)
        {
          for (int i = 0; i < 3; i++)
          {
            if (i < ConstValues.TendencyRegress)
            {
              if (TendencyArrows_Left[i].gameObject.activeInHierarchy == false) TendencyArrows_Left[i].gameObject.SetActive(true);
              TendencyArrows_Left[i].sprite = _emptyarrow;
            }
            else
            {
              if (TendencyArrows_Left[i].gameObject.activeInHierarchy == true) TendencyArrows_Left[i].gameObject.SetActive(false);
            }
          }
        }
        else
        {
          for (int i = 0; i < 3; i++)
          {
            if (i <= _progress*-1 - 1)
            {
              if (TendencyArrows_Left[i].gameObject.activeInHierarchy == false) TendencyArrows_Left[i].gameObject.SetActive(true);
              TendencyArrows_Left[i].sprite = _arrowsprite_left;
            }
            else if (i < ConstValues.TendencyRegress)
            {
              if (TendencyArrows_Left[i].gameObject.activeInHierarchy == false) TendencyArrows_Left[i].gameObject.SetActive(true);
              TendencyArrows_Left[i].sprite = _emptyarrow;
            }
            else
            {
              if (TendencyArrows_Left[i].gameObject.activeInHierarchy == true) TendencyArrows_Left[i].gameObject.SetActive(false);
            }

          }
        }

        if (TendencyProgress_Right.activeInHierarchy == false) TendencyProgress_Right.SetActive(true);
        if (_progress <= 0)
        {
          for (int i = 0; i < 3; i++)
          {
            if (i < ConstValues.TendencyProgress_1to2)
            {
              if (TendencyArrows_Right[i].gameObject.activeInHierarchy == false) TendencyArrows_Right[i].gameObject.SetActive(true);
              TendencyArrows_Right[i].sprite = _emptyarrow;
            }
            else
            {
              if (TendencyArrows_Right[i].gameObject.activeInHierarchy == true) TendencyArrows_Right[i].gameObject.SetActive(false);
            }
          }
        }
        else
        {
          for (int i = 0; i < ConstValues.TendencyProgress_1to2; i++)
          {
            if (i <= _progress - 1)
            {
              if (TendencyArrows_Right[i].gameObject.activeInHierarchy == false) TendencyArrows_Right[i].gameObject.SetActive(true);
              TendencyArrows_Right[i].sprite = _arrowsprite_right;
            }
            else
            {
              if (i < ConstValues.TendencyProgress_1to2)
              {
                if (TendencyArrows_Right[i].gameObject.activeInHierarchy == false) TendencyArrows_Right[i].gameObject.SetActive(true);
                TendencyArrows_Right[i].sprite = _emptyarrow;
              }
              else
              {
                if (TendencyArrows_Right[i].gameObject.activeInHierarchy == true) TendencyArrows_Right[i].gameObject.SetActive(false);
              }
            }
          }
        }
        break;
      case 2:
        if (_progress >= 0)
        {
          for (int i = 0; i < ConstValues.TendencyProgress_1to2; i++)
          {
            if (i < ConstValues.TendencyRegress)
            {
              if (TendencyArrows_Left[i].gameObject.activeInHierarchy == false) TendencyArrows_Left[i].gameObject.SetActive(true);
              TendencyArrows_Left[i].sprite = _emptyarrow;
            }
            else
            {
              if (TendencyArrows_Left[i].gameObject.activeInHierarchy == true) TendencyArrows_Left[i].gameObject.SetActive(false);
            }
          }
        }
        else
        {
          for (int i = 0; i < ConstValues.TendencyProgress_1to2; i++)
          {
            if (i <= _progress * -1 - 1)
            {
              if (TendencyArrows_Left[i].gameObject.activeInHierarchy == false) TendencyArrows_Left[i].gameObject.SetActive(true);
              TendencyArrows_Left[i].sprite = _arrowsprite_left;
            }
            else if (i < ConstValues.TendencyRegress)
            {
              if (TendencyArrows_Left[i].gameObject.activeInHierarchy == false) TendencyArrows_Left[i].gameObject.SetActive(true);
              TendencyArrows_Left[i].sprite = _emptyarrow;
            }
            else
            {
              if (TendencyArrows_Left[i].gameObject.activeInHierarchy == true) TendencyArrows_Left[i].gameObject.SetActive(false);
            }
          }
        }

        if (TendencyProgress_Right.activeInHierarchy == true) TendencyProgress_Right.SetActive(false);
        break;
    }

    TendencyIcon_Current.sprite = _icon;

    string _name = _targettendency.Name;
    string _description = WNCText.SetSize(EffectFontSize, _targettendency.GetTendencyEffectString) + "<br><br>"+
    string.Format(GameManager.Instance.GetTextData("TendencyPenalty"), _targetselectionname, _targetpenaltynames);
  //    "<br><br>" + WNCText.SetSize(SubdescriptionSize, WNCText.GetSubdescriptionColor(_targettendency.SubDescription));

    TendencyName.text = _name;
    TendencyDescription.text = _description;
    TendencyIcon_Left.sprite = _icon_left != null?_icon_left:GameManager.Instance.ImageHolder.Transparent;
    TendencyIcon_Right.sprite = _icon_right != null ? _icon_right : GameManager.Instance.ImageHolder.Transparent;

    OpenPreviewPanel(TendencyPreview, rect);
  }
  public void OpenSelectionNonePreview(SelectionData _selection,TendencyTypeEnum tendencytype,bool dir, RectTransform toprect)
  {
    if (SelectionNoneReward.Setup(_selection) == false) return;

    SelectionNoneBackground.sprite = GameManager.Instance.ImageHolder.SelectionBackground(tendencytype, dir);
    
    OpenPreviewPanel(SelectionNonePanel, toprect);
  }
  public void OpenSelectionPayPreview(SelectionData _selection, TendencyTypeEnum tendencytype, bool dir, RectTransform toprect)
  {
    if (SelectionPayReward.Setup(_selection) == false) return;
    if (UIManager.Instance.Mouse.MouseState == MouseStateEnum.DragExp) return;

    SelectionPayBackground.sprite = GameManager.Instance.ImageHolder.SelectionBackground(tendencytype, dir);
    //PaySubDescription.text = _selection.SubDescription;


    Sprite _payicon = null;
    int _payvalue = UIManager.Instance.DialogueUI.GetRequireValue(dir);
    string _payvaluetext = "";
    int _percent = -1;
    StatusTypeEnum _status = StatusTypeEnum.HP;
    switch (_selection.SelectionPayTarget)
    {
      case StatusTypeEnum.HP:
        if (_selection.SuccessData.Reward_Type == RewardTypeEnum.None) return;

        _status = StatusTypeEnum.HP;
        _payicon = GameManager.Instance.ImageHolder.HPDecreaseIcon;
        _payvaluetext = string.Format(GameManager.Instance.GetTextData("PAYVALUE_TEXT"), GameManager.Instance.GetTextData(StatusTypeEnum.HP, 1), WNCText.GetHPColor("-" + _payvalue.ToString()));
        if (PayNoGold_Text.gameObject.activeInHierarchy.Equals(true)) PayNoGold_Text.gameObject.SetActive(false);
        //  if (PayRequireValue.gameObject.activeInHierarchy.Equals(false)) PayRequireValue.gameObject.SetActive(true);

        break;//체력이라면 지불 기본값, 보정치, 최종값을 받아오고 보정치가 존재한다면 텍스트에 삽입

      case StatusTypeEnum.Sanity:
        if (_selection.SuccessData.Reward_Type == RewardTypeEnum.None) return;

        _status = StatusTypeEnum.Sanity;
        _payicon = GameManager.Instance.ImageHolder.SanityDecreaseIcon;
        _payvaluetext = string.Format(GameManager.Instance.GetTextData("PAYVALUE_TEXT"), GameManager.Instance.GetTextData(StatusTypeEnum.Sanity, 1), WNCText.GetSanityColor("-" + _payvalue.ToString()));

        if (PayNoGold_Text.gameObject.activeInHierarchy.Equals(true)) PayNoGold_Text.gameObject.SetActive(false);
        //  if (PayRequireValue.gameObject.activeInHierarchy.Equals(false)) PayRequireValue.gameObject.SetActive(true);
        break;//정신력이라면 지불 기본값,보정치,최종값을 받아오고 보정치가 존재한다면 텍스트에 삽입
      case StatusTypeEnum.Gold:
        _status = StatusTypeEnum.Gold;
        _payicon = GameManager.Instance.ImageHolder.GoldDecreaseIcon;
        if (_payvalue > GameManager.Instance.MyGameData.Gold)
        {
          _percent = GameManager.Instance.MyGameData.RequireValue_Money(_payvalue);
          int _sanitypayvalue = GameManager.Instance.MyGameData.PayOverSanityValue;

          if (PayNoGold_Text.gameObject.activeInHierarchy == false) PayNoGold_Text.gameObject.SetActive(true);
          PayNoGold_Text.text = string.Format(GameManager.Instance.GetTextData("Nogold_Text"),
             _sanitypayvalue,
             GameManager.Instance.MyGameData.Gold,
            WNCText.PercentageColor((100 - _percent).ToString(), (100 -_percent)/100.0f));
        }//지불 골드 값이 보유 값에 비해 높을 때
        else
        {
          if (_selection.SuccessData.Reward_Type == RewardTypeEnum.None) return;

          if (PayNoGold_Text.gameObject.activeInHierarchy.Equals(true)) PayNoGold_Text.gameObject.SetActive(false);
        }//골드 지불이 가능할 때
        break;//골드라면 지불,기본값,보정치,최종값을 받아오고 보정치가 존재한다면 텍스트에 삽입, 최종값이 보유값을 넘는다면 실패 확률 확인
    }


    //  PayIcon.sprite = _payicon;


    OpenPreviewPanel(SelectionPayPanel, toprect);
  }
  public void OpenSelectionCheckPreview_skill(SelectionData _selection, TendencyTypeEnum tendencytype, bool dir, RectTransform toprect)
  {
    if (SelectionCheckReward.Setup(_selection) == false) return;
    if (UIManager.Instance.Mouse.MouseState == MouseStateEnum.DragExp) return;

    SelectionCheckBackground.sprite = GameManager.Instance.ImageHolder.SelectionBackground(tendencytype, dir);

    Skill[] _skills= new Skill[2];
    int _requirelevel = 0, _currentlevel = UIManager.Instance.DialogueUI.GetRequireValue(dir), _percentage = 0;
    string  _percentage_text = "", _percentage_int = "";//, _subdescription = "";

    _percentage_text = GameManager.Instance.GetTextData("SUCCESSPERCENT_TEXT");

    if (_selection.ThisSelectionType.Equals(SelectionTargetType.Check_Single))
    {
      _requirelevel = GameManager.Instance.MyGameData.CheckSkillSingleValue;
      _skills[0] = GameManager.Instance.MyGameData.GetSkill(_selection.SelectionCheckSkill[0]);
    }
    else
    {
      _requirelevel = GameManager.Instance.MyGameData.CheckSkillMultyValue;
      for(int i = 0; i < 2; i++)
      {
        _skills[i] = GameManager.Instance.MyGameData.GetSkill(_selection.SelectionCheckSkill[i]);
      }
    }

    _percentage =GameManager.Instance.MyGameData.RequireValue_SkillCheck(_currentlevel, _requirelevel);
    _percentage_int = WNCText.PercentageColor((100 - _percentage).ToString(), (100 - _percentage)/100.0f)+"%";

    SelectionCheckPercent_text.text = _percentage_text;
    SelectionCheckPercent_int.text = _percentage_int;

    OpenPreviewPanel(SelectionCheckPanel, toprect);
  }
  public void OpenSelectionElsePreview(SelectionData _selection, TendencyTypeEnum tendencytype, bool dir, RectTransform rect)
  {
    //안쓰는 상태
    SelectionElseBackground.sprite = GameManager.Instance.ImageHolder.SelectionBackground(tendencytype, dir);

    Sprite _icon = null;

    SelectionElseIcon.sprite = _icon;
  //  SelectionElseDescription.text = _selection.SubDescription;

    CurrentPreview=SelectionElsePanel.GetComponent<RectTransform>();


    OpenPreviewPanel(SelectionElsePanel,rect);
  }
  public void OpenRewardStatusPreview(StatusTypeEnum status, int _value, RectTransform rect)
  {
  }
  public void OpenRewardExpPreview(Experience _exp, RectTransform rect)
  {
  }
  public void OpenRewardSkillPreview(SkillTypeEnum skilltype, RectTransform rect)
  {
  }
  public void OpenExpSelectionEmptyPreview(Experience _exp,bool islong, RectTransform rect)
  {
    string _turn, _description;

    _turn=islong ? ConstValues.EXPMaxTurn_long_idle.ToString() : ConstValues.EXPMaxTurn_short_idle.ToString();
    if (islong)
    {
      _description = string.Format(GameManager.Instance.GetTextData("LONGTERMSAVE_DESCRIPTION"), ConstValues.EXPMaxTurn_long_idle,
        (int)(ConstValues.LongTermChangeCost * GameManager.Instance.MyGameData.GetSanityLossModify(true,0)));
    }
    else
    {
      _description =string.Format(GameManager.Instance.GetTextData("SHORTTERMSAVE_DESCRIPTION"), ConstValues.EXPMaxTurn_short_idle);
    }

    ExpSelectEmptyTurn.text = _turn.ToString();
    ExpSelectEmptyEffect.text = _exp.EffectString;
    ExpSelectEmptyIllust.sprite = _exp.Illust;
    ExpSelectEmptyDescription.text = _description;
    ExpSelectClickText.text= GameManager.Instance.GetTextData("CLICKTOGET_TEXT");
    if (ExpSelectClickText.gameObject.activeInHierarchy.Equals(false)) ExpSelectClickText.gameObject.SetActive(true);

    OpenPreviewPanel(ExpSelectEmptyPanel,rect);
  }
  public void OpenExpSelectionExistPreview(Experience _origin,Experience _new,bool islong, RectTransform rect)
  {
    int _turn = islong ? ConstValues.EXPMaxTurn_long_idle : ConstValues.EXPMaxTurn_short_idle;
    string _description = "";
    if (islong)
    {
      _description =string.Format(GameManager.Instance.GetTextData("LONGTERMSAVE_DESCRIPTION"), ConstValues.EXPMaxTurn_long_idle, ConstValues.LongTermChangeCost);
    }
    else
    {
      _description = string.Format(GameManager.Instance.GetTextData("SHORTTERMSAVE_DESCRIPTION"), ConstValues.EXPMaxTurn_short_idle);
    }

    string _origineffect = _origin.EffectString;
    ExpSelectOriginEffect.text = _origineffect;
    ExpSelectExistOriginIllust.sprite = _origin.Illust;
    ExpSelectOriginTurn.text = _origin.Duration.ToString();

    string _neweffect = _new.EffectString;
    ExpSelectNewEffect.text = _neweffect;
    ExpSelectExistNewIllust.sprite= _new.Illust;
    ExpSelectNewTurn.text = _turn.ToString();
    ExpSelecitonExistDescription.text = _description;
    ExpSelectClickText.text=

    ExpSelectClickText.text = GameManager.Instance.GetTextData("CLICKTOGET_TEXT");
    if (ExpSelectClickText.gameObject.activeInHierarchy.Equals(false)) ExpSelectClickText.gameObject.SetActive(true);

    OpenPreviewPanel(ExpSelectExistPanel,rect);
  }
  public void OpenJustDescriptionPreview(string text, RectTransform rect)
  {
    JustDescriptionText.text = text;

    OpenPreviewPanel(JustDescription_Panel,rect);
  }
  public void OpenJustDescriptionPreview(string text,Vector2 pivot, RectTransform rect)
  { 
    JustDescriptionText.text = text;

    OpenPreviewPanel(JustDescription_Panel,pivot,rect);
  }
  public void OpenIconAndDescriptionPanel(Sprite icon,string text,bool isstatus, RectTransform rect)
  {
    IconAndDescription_Icon.sprite = icon;
  //  IconAndDescription_IconBackground.sprite = isstatus ? GameManager.Instance.ImageHolder.IconBackground_status : GameManager.Instance.ImageHolder.IconBackground_normal;
    IconAndDescription_Description.text = text;

    OpenPreviewPanel(IconAndDescription_Panel,rect);
  }
  public void OpenIconAndDescriptionPanel(Sprite icon, string text,Vector2 pivot, bool isstatus, RectTransform rect)
  {
    IconAndDescription_Icon.sprite = icon;
  //  IconAndDescription_IconBackground.sprite = isstatus ? GameManager.Instance.ImageHolder.IconBackground_status : GameManager.Instance.ImageHolder.IconBackground_normal;
    IconAndDescription_Description.text = text;

    OpenPreviewPanel(IconAndDescription_Panel,pivot,rect);
  }
  public void OpenDisComfortPanel(RectTransform rect)
  {
    OpenIconAndDescriptionPanel(GameManager.Instance.ImageHolder.DisComfort, 
     string.Format(GameManager.Instance.GetTextData("DISCOMFORT_DESECRIPTION"),ConstValues. DiscomfortDownValue,
     GameManager.Instance.MyGameData.GetDiscomfortValue(GameManager.Instance.MyGameData.CurrentSettlement.Discomfort)),
      DiscomfortPivot, false, rect);
  }
  public void OpenEnvirPanel(EnvironmentType envir)
  {
  }
  public void OpenTileInfoPreveiew(TileData tileData, RectTransform tilerect)
  {
    if (tileData == GameManager.Instance.MyGameData.CurrentTile) return;
    if (UIManager.Instance.Mouse.MouseState == MouseStateEnum.DragMap) return;

    Vector2 _pivot = new Vector2(0.5f,tileData.Coordinate.y<=GameManager.Instance.MyGameData.Coordinate.y?1.05f:-0.05f);
    if (UIManager.Instance.MapUI.Destinations.Contains(tileData))
    {
      OpenJustDescriptionPreview(GameManager.Instance.GetTextData("RemoveDestination"), _pivot, tilerect);
    }
    else if (tileData.TileSettle == null)
    {
    }
    else
    {
      if (tileData.Coordinate.y - GameManager.Instance.MyGameData.Coordinate.y == -3) _pivot.y = -0.05f;

      var _sectorlist = tileData.TileSettle.Sectors;
      for(int i = 0; i < SettlementSectorIcons.Count; i++)
      {
        if (i >= _sectorlist.Count)
        {
          if(SettlementSectorIcons[i].gameObject.activeInHierarchy)SettlementSectorIcons[i].gameObject.SetActive(false);
        }
        else
        {
          SettlementSectorIcons[i].sprite = GameManager.Instance.ImageHolder.GetSectorIcon(_sectorlist[i], false);
        }
      }
      LayoutRebuilder.ForceRebuildLayoutImmediate(SettlementSectorIcons[0].transform.parent.transform as RectTransform);
      Sprite _settlementicon = null;
      switch (tileData.TileSettle.SettlementType)
      {
        case SettlementType.Village: _settlementicon = GameManager.Instance.ImageHolder.VillageIcon_white; break;
        case SettlementType.Town: _settlementicon = GameManager.Instance.ImageHolder.TownIcon_white; break;
        case SettlementType.City: _settlementicon = GameManager.Instance.ImageHolder.CityIcon_white; break;
      }
      SettlementIcon.sprite = _settlementicon;
      SettlementInfoName.text =string.Format(GameManager.Instance.GetTextData("SettlementInfoNames"),
        GameManager.Instance.GetTextData(tileData.TileSettle.SettlementType));

      SettlementInfoDiscomfort.text = tileData.TileSettle.Discomfort.ToString();
      LayoutRebuilder.ForceRebuildLayoutImmediate(SettlementInfoDiscomfort.transform.parent.transform as RectTransform);

      OpenPreviewPanel(SettlementInfoPanel, _pivot, tilerect);
    }
  }

  private Vector2 Newpos = Vector2.zero;
  public void Update()
  {
    if (CurrentPreview == null) return;
  }
  public void ClosePreview() 
  {
    if (CurrentPreview == null) return;

    CurrentPreview.GetComponent<CanvasGroup>().alpha = 0.0f;
    CurrentPreview = null; 
  }


}
