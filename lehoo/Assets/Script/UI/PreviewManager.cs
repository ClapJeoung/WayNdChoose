using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PreviewManager : MonoBehaviour
{
//  [SerializeField] private RectTransform WholeRect = null;
  [SerializeField] private Camera MainCamera = null;
  [Space(10)]
  [SerializeField] private GameObject IconAndDescription_Panel = null;
  [SerializeField] private Image IconAndDescription_IconBackground = null;
  [SerializeField] private Image IconAndDescription_Icon = null;
  [SerializeField] private TextMeshProUGUI IconAndDescription_Description = null;
  [Space(10)]
  [SerializeField] private GameObject JustDescription_Panel = null;
  [SerializeField] private TextMeshProUGUI JustDescriptionText = null;
  private int EffectFontSize = 20;
  private int SubdescriptionSize = 16;
  private Vector2 TurnPivot = new Vector2(0.5f, 1.1f);
  private Vector2 HPPivot= new Vector2(0.5f, 1.1f);
  private Vector2 SanityPivot= new Vector2(0.5f, 1.1f);
  private Vector2 GoldPivot= new Vector2(0.5f, 1.1f);
  private Vector2 MovePointPivot= new Vector2(1.1f, 1.1f);
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
  [SerializeField] private TextMeshProUGUI ExpDuration = null;
  [SerializeField] private Image ExpIllust = null;
  [SerializeField] private TextMeshProUGUI ExpDescription = null;
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
  [SerializeField] private PreviewSelectionTendency SelectionNoneTendency = null;
  [SerializeField] private TextMeshProUGUI SelectionNoneRewardText = null;
  [SerializeField] private Image SelectionNoneRewardIcon = null;
  [SerializeField] private TextMeshProUGUI SelectionNonePenaltyText = null;
  [SerializeField] private Image SelectionNonePenaltyIcon = null;
  [Space(10)]
  [SerializeField] private GameObject SelectionPayPanel = null;
  [SerializeField] private Image SelectionPayBackground = null;
//  [SerializeField] private Image PayIcon = null;
 // [SerializeField] private TextMeshProUGUI PayInfo = null;
  [SerializeField] private TextMeshProUGUI PayRequireValue = null;
  [SerializeField] private GameObject PayNoGoldHolder = null;
  [SerializeField] private TextMeshProUGUI PayNoGold_Text = null;
  [SerializeField] private TextMeshProUGUI PayNoGold_PercentText = null;
  [SerializeField] private TextMeshProUGUI PayNoGold_PercentValue = null;
  [SerializeField] private TextMeshProUGUI PayNoGold_Alternative = null;
  // [SerializeField] private TextMeshProUGUI PaySubDescription = null;
  [SerializeField] private TextMeshProUGUI SelectionPayRewardText = null;
  [SerializeField] private Image SelectionPayRewardIcon = null;
  [SerializeField] private TextMeshProUGUI SelectionPayPenaltyText = null;
  [SerializeField] private Image SelectionPayPenaltyIcon = null;
  [SerializeField] private PreviewSelectionTendency SelectionPayTendendcy = null;
  [Space(10)]
  [SerializeField] private GameObject SelectionCheckPanel = null;
  [SerializeField] private Image SelectionCheckBackground = null;
 // [SerializeField] private Image[] SelectionCheckIcons = null;
  [SerializeField] private TextMeshProUGUI SelectionCheckInfo = null;
 // [SerializeField] private TextMeshProUGUI SelectionCheckCurrentLevel = null;
  [SerializeField] private TextMeshProUGUI SelectionCheckPercent_text = null;
  [SerializeField] private TextMeshProUGUI SelectionCheckPercent_int = null;
  [SerializeField] private TextMeshProUGUI SelectionCheckRewardText = null;
  [SerializeField] private Image SelectionCheckRewardIcon = null;
  [SerializeField] private TextMeshProUGUI SelectionCheckPenaltyText = null;
  [SerializeField] private Image SelectionCheckPenaltyIcon = null;
  //  [SerializeField] private TextMeshProUGUI SelectionCheckDescription = null;
  [SerializeField] private PreviewSelectionTendency SelectionCheckTendendcy = null;
  [Space(10)]
  [SerializeField] private GameObject SelectionElsePanel = null;
  [SerializeField] private Image SelectionElseBackground = null;
  [SerializeField] private Image SelectionElseIcon = null;
 // [SerializeField] private TextMeshProUGUI SelectionElseDescription = null;
  [SerializeField] private PreviewSelectionTendency SelectionElseTendency = null;
  [Space(10)]
  [SerializeField] private GameObject RewardStatusPanel = null;
  [SerializeField] private Image RewardStatusIcon = null;
  [SerializeField] private TextMeshProUGUI RewardStatusValue = null;
  [SerializeField] private TextMeshProUGUI RewardStatusModify = null;
  [SerializeField] private TextMeshProUGUI RewardStatusClickText = null;
  [Space(10)]
  [SerializeField] private GameObject RewardExpPanel = null;
  [SerializeField] private TextMeshProUGUI RewardExpName = null;
  [SerializeField] private Image RewardExpIllust = null;
  [SerializeField] private TextMeshProUGUI RewardExpEffect = null;
  [SerializeField] private TextMeshProUGUI RewardExpClickText = null;
  [Space(10)]
  [SerializeField] private GameObject RewardSkillPanel = null;
  [SerializeField] private TextMeshProUGUI RewardSkillName = null;
  [SerializeField] private Image RewardSkillIcon = null;
  [SerializeField] private TextMeshProUGUI RewardSkillClickText = null;
  [Space(10)]
  [SerializeField] private GameObject ExpSelectEmptyPanel = null;
  [SerializeField] private TextMeshProUGUI ExpSelectEmptyDescription = null;
  [SerializeField] private TextMeshProUGUI ExpSelectEmptyTurn = null;
  [SerializeField] private TextMeshProUGUI ExpSelectEmptyEffect = null;
  [Space(10)]
  [SerializeField] private GameObject ExpSelectExistPanel = null;
  [SerializeField] private TextMeshProUGUI ExpSelectOriginTurn = null;
  [SerializeField] private TextMeshProUGUI ExpSelectOriginEffect = null;
  [SerializeField] private TextMeshProUGUI ExpSelectNewTurn = null;
  [SerializeField] private TextMeshProUGUI ExpSelectNewEffect = null;
  [SerializeField] private TextMeshProUGUI ExpSelecitonExistDescription = null;
  [SerializeField] private TextMeshProUGUI ExpSelectClickText = null;
  private RectTransform CurrentPreview = null;
  private void OpenPreviewPanel(GameObject panel,Vector2 pivot,RectTransform rect)
  {
    CurrentPreview = panel.GetComponent<RectTransform>();
    CurrentPreview.pivot = pivot;

    LayoutRebuilder.ForceRebuildLayoutImmediate(panel.transform as RectTransform);
   
    CurrentPreview.position = rect.position;
    CurrentPreview.anchoredPosition3D = new Vector3(CurrentPreview.anchoredPosition.x, CurrentPreview.anchoredPosition.y, 0.0f);
  
    CurrentPreview.GetComponent<CanvasGroup>().alpha = 1.0f;
  }
  private void OpenPreviewPanel(GameObject panel,RectTransform rect)
  {
    CurrentPreview = panel.GetComponent<RectTransform>();

    LayoutRebuilder.ForceRebuildLayoutImmediate(panel.transform as RectTransform);

    CurrentPreview.position = rect.position;
    CurrentPreview.anchoredPosition3D = new Vector3(CurrentPreview.anchoredPosition.x, CurrentPreview.anchoredPosition.y, 0.0f);

    CurrentPreview.GetComponent<CanvasGroup>().alpha = 1.0f;
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

    string _description = "";
    int _genvalue = 0, _payvalue = 0;

    _genvalue = (int)GameManager.Instance.MyGameData.GetHPGenModify(false);
    _payvalue = (int)GameManager.Instance.MyGameData.GetHPLossModify(false);

    _description = GameManager.Instance.GetTextData(_currenttype, 3);
    if (_genvalue > 0)
    {
      _description += "<br><br>" + GameManager.Instance.GetTextData(_currenttype, 12) + " " + string.Format("{0}%", WNCText.PositiveColor("+" + _genvalue.ToString()));
    }
    if (_payvalue > 0)
    {
      if (_genvalue == 0) _description += "<br><br>" + GameManager.Instance.GetTextData(_currenttype, 15) + " " + string.Format("{0}%", WNCText.NegativeColor("+" + _payvalue.ToString()));
      else _description += "<br>" + GameManager.Instance.GetTextData(_currenttype, 15) + " " + string.Format("{0}%", WNCText.NegativeColor("+" + _payvalue.ToString()));
    }
    //  _description+="<br><br>"+WNCText.SetSize(SubdescriptionSize,WNCText.GetSubdescriptionColor(GameManager.Instance.GetTextData(_currenttype, 4)));

    OpenIconAndDescriptionPanel(GameManager.Instance.ImageHolder.HPIcon, _description, HPPivot,true,rect);
  }//체력 설명, 증감량 표기 후 열기
  public void OpenSanityPreview(RectTransform rect)
  {
    StatusTypeEnum _currenttype = StatusTypeEnum.Sanity;

    string _description = "";
    int _genvalue = 0, _payvalue = 0;

    _genvalue = (int)GameManager.Instance.MyGameData.GetSanityGenModify(false);
    _payvalue = (int)GameManager.Instance.MyGameData.GetSanityLossModify(false);

    _description = string.Format(GameManager.Instance.GetTextData(_currenttype, 3));
    if (_genvalue > 0)
    {
      _description += "<br><br>" + GameManager.Instance.GetTextData(_currenttype, 12) + " " + string.Format("{0}%", WNCText.PositiveColor("+" + _genvalue.ToString()));
    }
    if (_payvalue > 0)
    {
      if (_genvalue == 0) _description += "<br><br>" + GameManager.Instance.GetTextData(_currenttype, 15) + " " + string.Format("{0}%", WNCText.NegativeColor("+" + _payvalue.ToString()));
      else _description += "<br>" + GameManager.Instance.GetTextData(_currenttype, 15) + " " + string.Format("{0}%", WNCText.NegativeColor("+" + _payvalue.ToString()));
    }
    //   _description += "<br><br>" + WNCText.SetSize(SubdescriptionSize, WNCText.GetSubdescriptionColor(GameManager.Instance.GetTextData(_currenttype, 4)));


    OpenIconAndDescriptionPanel(GameManager.Instance.ImageHolder.SanityIcon, _description, SanityPivot,true, rect);
  }//정신력 설명,증감량 표기 후 열기
  public void OpenGoldPreview( RectTransform rect)
  {
    StatusTypeEnum _currenttype = StatusTypeEnum.Gold;

    string _description = "";
    int _genvalue = 0, _payvalue = 0;

    _genvalue = (int)GameManager.Instance.MyGameData.GetGoldGenModify(false);
    _payvalue = (int)GameManager.Instance.MyGameData.GetGoldLossModify(false);

    _description = GameManager.Instance.GetTextData(_currenttype, 3);
    if (_genvalue > 0)
    {
      _description += "<br><br>" + GameManager.Instance.GetTextData(_currenttype, 12) + " " + string.Format("{0}%", WNCText.PositiveColor("+" + _genvalue.ToString()));
    }
    if (_payvalue > 0)
    {
      if (_genvalue == 0) _description += "<br><br>" + GameManager.Instance.GetTextData(_currenttype, 15) + " " + string.Format("{0}%", WNCText.NegativeColor("+" + _payvalue.ToString()));
      else _description += "<br>" + GameManager.Instance.GetTextData(_currenttype, 15) + " " + string.Format("{0}%", WNCText.NegativeColor("+" + _payvalue.ToString()));
    }
    //  _description += "<br><br>" + WNCText.SetSize(SubdescriptionSize, WNCText.GetSubdescriptionColor(GameManager.Instance.GetTextData(_currenttype, 4)));

    OpenIconAndDescriptionPanel(GameManager.Instance.ImageHolder.GoldIcon, _description, GoldPivot,true, rect);
  }//골드 설명,증감량 표기 후 열기
  public void OpenMovePointPreview( RectTransform rect)
  {
    Sprite _icon = GameManager.Instance.ImageHolder.MovePointIcon_Enable;
    string _description = GameManager.Instance.GetTextData("MOVEPOINT_DESCRIPTION");


    OpenIconAndDescriptionPanel(_icon, _description, MovePointPivot, true, rect);
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
    Sprite _icon = GameManager.Instance.ImageHolder.GetSkillIcon(_skilltype,true);

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

          SkillMadnessInfo.text = string.Format(GameManager.Instance.GetTextData("Madness_Conversation_Info"), ConstValues.MadnessEffect_Conversation);
          _leveltext = WNCText.GetMadnessSkillColor(_level);
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

          SkillMadnessInfo.text = string.Format(GameManager.Instance.GetTextData("Madness_Force_Info"), ConstValues.MadnessEffect_Force);
          _leveltext = WNCText.GetMadnessSkillColor(_level);
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

          SkillMadnessInfo.text = string.Format(GameManager.Instance.GetTextData("Madness_Wild_Info"), ConstValues.MadnessEffect_Wild);
          _leveltext = WNCText.GetMadnessSkillColor(_level);
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

          SkillMadnessInfo.text = string.Format(GameManager.Instance.GetTextData("Madness_Intelligence_Info"), ConstValues.MadnessEffect_Intelligence);
          _leveltext = WNCText.GetMadnessSkillColor(_level);
        }
        else
        {
          if (SkillMadnessHolder.activeInHierarchy == true) SkillMadnessHolder.SetActive(false);
        }
        break;
    }
    SkillLevel.text = _leveltext;
    SkillSubdescription.text = WNCText.GetSubdescriptionColor(GameManager.Instance.GetTextData(_skilltype, 4));
    OpenPreviewPanel(SkillPreview,rect);
  }
  public void OpenExpPreview(Experience _exp, RectTransform rect)
  {
    ExpName.text =_exp.Name;
    ExpDuration.text = $"{_exp.Duration}";
    ExpIllust.sprite = _exp.Illust;
    string _description = WNCText.SetSize(EffectFontSize, _exp.EffectString) + "<br><br>" + WNCText.SetSize(SubdescriptionSize, WNCText.GetSubdescriptionColor(_exp.Description));
    ExpDescription.text = _description;

    OpenPreviewPanel(ExpPreview,rect);
  }
  public void OpenTendencyPreview(TendencyTypeEnum _type, RectTransform rect)
  {
    Tendency _targettendency = null;
    Sprite _arrowsprite_left = null, _arrowsprite_right = null;
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
    Sprite _icon = _targettendency.CurrentIcon;
    Sprite _icon_left=_targettendency.GetNextIcon(true), _icon_right=_targettendency.GetNextIcon(false);
    int _progress = Mathf.Abs(_targettendency.Progress) - 1;
    switch (_targettendency.Level)
    {
      case -2:
        if(TendencyProgress_Left.activeInHierarchy==true) TendencyProgress_Left.SetActive(false);

        for(int i = 0; i < ConstValues.TendencyProgress_1to2; i++)
        {
          if (i <= _progress)
          {
              if (TendencyArrows_Right[i].gameObject.activeInHierarchy == false) TendencyArrows_Right[i].gameObject.SetActive(true);
              TendencyArrows_Right[i].sprite = _arrowsprite_right;
            }
          else 
            {
              if (i < ConstValues.TendencyRegress)
              {
                if (TendencyArrows_Right[i].gameObject.activeInHierarchy == false) TendencyArrows_Right[i].gameObject.SetActive(true);
                TendencyArrows_Right[i].sprite = GameManager.Instance.ImageHolder.Arrow_Empty;
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
        if (TendencyProgress_Right.activeInHierarchy == false) TendencyProgress_Right.SetActive(true);
        for (int i =0; i < ConstValues.TendencyProgress_1to2; i++)
        {
          if (i <= _progress)
          {
            if (TendencyArrows_Left[i].gameObject.activeInHierarchy == false) TendencyArrows_Left[i].gameObject.SetActive(true);
            TendencyArrows_Left[i].sprite = _arrowsprite_left;
          }
          else
          {
            if (i < ConstValues.TendencyProgress_1to2)
            {
              if (TendencyArrows_Left[i].gameObject.activeInHierarchy == false) TendencyArrows_Left[i].gameObject.SetActive(true);
              TendencyArrows_Left[i].sprite = GameManager.Instance.ImageHolder.Arrow_Empty;
            }
            else
            {
              if (TendencyArrows_Left[i].gameObject.activeInHierarchy == true) TendencyArrows_Left[i].gameObject.SetActive(false);
            }
          }
        }
        for (int i = 0; i < ConstValues.TendencyProgress_1to2; i++)
        {
          if (i <= _progress)
          {
            if (TendencyArrows_Right[i].gameObject.activeInHierarchy == false) TendencyArrows_Right[i].gameObject.SetActive(true);
            TendencyArrows_Right[i].sprite = _arrowsprite_right;
          }
          else
          {
            if (i < ConstValues.TendencyRegress)
            {
              if (TendencyArrows_Right[i].gameObject.activeInHierarchy == false) TendencyArrows_Right[i].gameObject.SetActive(true);
              TendencyArrows_Right[i].sprite = GameManager.Instance.ImageHolder.Arrow_Empty;
            }
            else
            {
              if (TendencyArrows_Right[i].gameObject.activeInHierarchy == true) TendencyArrows_Right[i].gameObject.SetActive(false);
            }
          }
        }
        break;
      case 1:
        if (TendencyProgress_Left.activeInHierarchy == false) TendencyProgress_Left.SetActive(true);
        if (TendencyProgress_Right.activeInHierarchy == false) TendencyProgress_Right.SetActive(true);
        for (int i = 0; i < ConstValues.TendencyProgress_1to2; i++)
        {
          if (i <= _progress)
          {
            if (TendencyArrows_Left[i].gameObject.activeInHierarchy == false) TendencyArrows_Left[i].gameObject.SetActive(true);
            TendencyArrows_Left[i].sprite = _arrowsprite_left;
          }
          else
          {
            if (i < ConstValues.TendencyRegress)
            {
              if (TendencyArrows_Left[i].gameObject.activeInHierarchy == false) TendencyArrows_Left[i].gameObject.SetActive(true);
              TendencyArrows_Left[i].sprite = GameManager.Instance.ImageHolder.Arrow_Empty;
            }
            else
            {
              if (TendencyArrows_Left[i].gameObject.activeInHierarchy == true) TendencyArrows_Left[i].gameObject.SetActive(false);
            }
          }
        }
        for (int i = 0; i < ConstValues.TendencyProgress_1to2; i++)
        {
          if (i <= _progress)
          {
            if (TendencyArrows_Right[i].gameObject.activeInHierarchy == false) TendencyArrows_Right[i].gameObject.SetActive(true);
            TendencyArrows_Right[i].sprite = _arrowsprite_right;
          }
          else
          {
            if (i < ConstValues.TendencyProgress_1to2)
            {
              if (TendencyArrows_Right[i].gameObject.activeInHierarchy == false) TendencyArrows_Right[i].gameObject.SetActive(true);
              TendencyArrows_Right[i].sprite = GameManager.Instance.ImageHolder.Arrow_Empty;
            }
            else
            {
              if (TendencyArrows_Right[i].gameObject.activeInHierarchy == true) TendencyArrows_Right[i].gameObject.SetActive(false);
            }
          }
        }
        break;
      case 2:
        for (int i = 0; i < ConstValues.TendencyProgress_1to2; i++)
        {
          if (i <= _progress)
          {
            if (TendencyArrows_Left[i].gameObject.activeInHierarchy == false) TendencyArrows_Left[i].gameObject.SetActive(true);
            TendencyArrows_Left[i].sprite = _arrowsprite_left;
          }
          else
          {
            if (i < ConstValues.TendencyRegress)
            {
              if (TendencyArrows_Left[i].gameObject.activeInHierarchy == false) TendencyArrows_Left[i].gameObject.SetActive(true);
              TendencyArrows_Left[i].sprite = GameManager.Instance.ImageHolder.Arrow_Empty;
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
    string _description = WNCText.SetSize(EffectFontSize, _targettendency.GetTendencyEffectString) +
      "<br><br>" + WNCText.SetSize(SubdescriptionSize, WNCText.GetSubdescriptionColor(_targettendency.SubDescription));

    TendencyName.text = _name;
    TendencyDescription.text = _description;
    TendencyIcon_Left.sprite = _icon_left;
    TendencyIcon_Right.sprite = _icon_right;

    OpenPreviewPanel(TendencyPreview, rect);
  }

  private void SetRewardInfo(SuccessData successdata, TextMeshProUGUI rewardtext,Image rewardicon,
    FailData failuredata,TextMeshProUGUI penaltytext,Image penaltyicon)
  {
    Sprite _rewardicon = null;
    switch (successdata.Reward_Type)
    {
      case RewardTypeEnum.Status:
        switch (successdata.Reward_StatusType)
        {
          case StatusTypeEnum.HP: _rewardicon = GameManager.Instance.ImageHolder.HPIcon; break;
          case StatusTypeEnum.Sanity: _rewardicon = GameManager.Instance.ImageHolder.SanityIcon; break;
          case StatusTypeEnum.Gold: _rewardicon = GameManager.Instance.ImageHolder.GoldIcon; break;
        }
        break;
      case RewardTypeEnum.Experience: _rewardicon = GameManager.Instance.ImageHolder.UnknownExpRewardIcon; break;
      case RewardTypeEnum.Skill:
        _rewardicon = GameManager.Instance.ImageHolder.GetSkillIcon(successdata.Reward_SkillType, false);
        break;
    }
    Sprite _penaltyicon = null;
    if (failuredata != null)
    {
      switch (failuredata.Penelty_target)
      {
        case PenaltyTarget.Status:
          switch (failuredata.StatusType)
          {
            case StatusTypeEnum.HP: _penaltyicon = GameManager.Instance.ImageHolder.HPDecreaseIcon; break;
            case StatusTypeEnum.Sanity: _penaltyicon = GameManager.Instance.ImageHolder.SanityDecreaseIcon; break;
            case StatusTypeEnum.Gold: _penaltyicon = GameManager.Instance.ImageHolder.GoldDecreaseIcon; break;
          }
          break;
      }
    }

    if (_rewardicon == null && _penaltyicon == null)
    {
      rewardicon.transform.parent.transform.gameObject.SetActive(false);
    }
    else
    {
      rewardicon.transform.parent.transform.gameObject.SetActive(true);

      if (_rewardicon == null)
      {
        if (rewardtext.gameObject.activeInHierarchy == true) rewardtext.gameObject.SetActive(false);
        if (rewardicon.gameObject.activeInHierarchy == true) rewardicon.gameObject.SetActive(false);
      }
      else
      {
        if (rewardtext.gameObject.activeInHierarchy == false) rewardtext.gameObject.SetActive(true);
        if (rewardicon.gameObject.activeInHierarchy == false) rewardicon.gameObject.SetActive(true);
        rewardtext.text = GameManager.Instance.GetTextData("SuccessReward");
        rewardicon.sprite = _rewardicon;
      }
      if (_penaltyicon == null)
      {
        if (penaltytext.gameObject.activeInHierarchy == true) penaltytext.gameObject.SetActive(false);
        if (penaltyicon.gameObject.activeInHierarchy == true) penaltyicon.gameObject.SetActive(false);
      }
      else
      {
        if (penaltytext.gameObject.activeInHierarchy == false) penaltytext.gameObject.SetActive(true);
        if (penaltyicon.gameObject.activeInHierarchy == false) penaltyicon.gameObject.SetActive(true);
        penaltytext.text = GameManager.Instance.GetTextData("FailPenalty");
        penaltyicon.sprite = _penaltyicon;
      }
    }

  }
  public void OpenSelectionNonePreview(SelectionData _selection,TendencyTypeEnum tendencytype,bool dir, RectTransform rect)
  {
    SelectionNoneBackground.sprite = GameManager.Instance.ImageHolder.SelectionBackground(tendencytype, dir);

  //  SelectionNoneText.text = _selection.SubDescription;
  SetRewardInfo(_selection.SuccessData,SelectionNoneRewardText,SelectionNoneRewardIcon,
    _selection.FailureData,SelectionNonePenaltyText,SelectionNonePenaltyIcon);

    Vector2 _pivot = new Vector2(1.1f, 0.5f);
    switch (tendencytype)
    {
      case TendencyTypeEnum.None:
        if (SelectionNoneTendency.gameObject.activeInHierarchy.Equals(true)) SelectionNoneTendency.gameObject.SetActive(false);
        break;
      case TendencyTypeEnum.Body:
        if (SelectionNoneTendency.gameObject.activeInHierarchy.Equals(false))SelectionNoneTendency.gameObject.SetActive(true);
        SelectionNoneTendency.Setup(GameManager.Instance.MyGameData.Tendency_Body,dir);
        _pivot = dir == true ? new Vector2(1.1f, 0.5f) : new Vector2(-0.1f, 0.5f);
        break;
      case TendencyTypeEnum.Head:
        if (SelectionNoneTendency.gameObject.activeInHierarchy.Equals(false)) SelectionNoneTendency.gameObject.SetActive(true);
        SelectionNoneTendency.Setup(GameManager.Instance.MyGameData.Tendency_Head, dir);
        _pivot = dir == true ? new Vector2(1.1f, 0.5f) : new Vector2(-0.1f, 0.5f);
        break;
    }

    OpenPreviewPanel(SelectionNonePanel,_pivot,rect);
  }
  public void OpenSelectionPayPreview(SelectionData _selection, TendencyTypeEnum tendencytype, bool dir, RectTransform rect)
  {
    SelectionPayBackground.sprite = GameManager.Instance.ImageHolder.SelectionBackground(tendencytype, dir);

    //PaySubDescription.text = _selection.SubDescription;

    SetRewardInfo(_selection.SuccessData, SelectionPayRewardText, SelectionPayRewardIcon,
      _selection.FailureData, SelectionPayPenaltyText, SelectionPayPenaltyIcon);

    Sprite _payicon = null;
    int _modifiedvalue = 0;
    string _payvaluetext = "";
    int _percent = -1;
    StatusTypeEnum _status = StatusTypeEnum.HP;
    switch (_selection.SelectionPayTarget)
    {
      case StatusTypeEnum.HP:
        _status = StatusTypeEnum.HP;
        _payicon = GameManager.Instance.ImageHolder.HPDecreaseIcon;
        _modifiedvalue = GameManager.Instance.MyGameData.PayHPValue_modified;
        _payvaluetext = string.Format(GameManager.Instance.GetTextData("PAYVALUE_TEXT"),GameManager.Instance.GetTextData(StatusTypeEnum.HP,1), WNCText.GetHPColor("-"+_modifiedvalue.ToString()));
        if (PayNoGoldHolder.activeInHierarchy.Equals(true)) PayNoGoldHolder.SetActive(false);
        if (PayRequireValue.gameObject.activeInHierarchy.Equals(false)) PayRequireValue.gameObject.SetActive(true);
        break;//체력이라면 지불 기본값, 보정치, 최종값을 받아오고 보정치가 존재한다면 텍스트에 삽입

      case StatusTypeEnum.Sanity:
        _status = StatusTypeEnum.Sanity;
        _payicon = GameManager.Instance.ImageHolder.SanityDecreaseIcon;
        _modifiedvalue = GameManager.Instance.MyGameData.PaySanityValue_modified;
        _payvaluetext = string.Format(GameManager.Instance.GetTextData("PAYVALUE_TEXT"), GameManager.Instance.GetTextData(StatusTypeEnum.Sanity, 1), WNCText.GetSanityColor("-" + _modifiedvalue.ToString()));

        if (PayNoGoldHolder.activeInHierarchy.Equals(true)) PayNoGoldHolder.SetActive(false);
        if (PayRequireValue.gameObject.activeInHierarchy.Equals(false)) PayRequireValue.gameObject.SetActive(true);
        break;//정신력이라면 지불 기본값,보정치,최종값을 받아오고 보정치가 존재한다면 텍스트에 삽입
      case StatusTypeEnum.Gold:
        _status = StatusTypeEnum.Gold;
        _payicon = GameManager.Instance.ImageHolder.GoldDecreaseIcon;
        _modifiedvalue = GameManager.Instance.MyGameData.PayGoldValue_modified;
        if (_modifiedvalue > GameManager.Instance.MyGameData.Gold)
        {
          _percent = GameManager.Instance.MyGameData.CheckPercent_money(_modifiedvalue);
          int _sanitypayvalue = (int)((_modifiedvalue - GameManager.Instance.MyGameData.Gold) * ConstValues.GoldSanityPayAmplifiedValue);

          PayNoGold_Text.text = GameManager.Instance.GetTextData("NOGOLD_TEXT");
          PayNoGold_PercentText.text = GameManager.Instance.GetTextData("SUCCESSPERCENT_TEXT");
          PayNoGold_PercentValue.text = WNCText.PercentageColor(_percent);
          PayNoGold_Alternative.text = string.Format(GameManager.Instance.GetTextData("NOGOLD_PERCENTAGE_TEXT"),
            GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 2), WNCText.GetGoldColor(GameManager.Instance.MyGameData.Gold),
            GameManager.Instance.GetTextData(StatusTypeEnum.Sanity, 2), WNCText.GetSanityColor(_sanitypayvalue.ToString()));

          if (PayNoGoldHolder.activeInHierarchy.Equals(false)) PayNoGoldHolder.SetActive(true);
          if (PayRequireValue.gameObject.activeInHierarchy.Equals(true)) PayRequireValue.gameObject.SetActive(false);

        }//지불 골드 값이 보유 값에 비해 높을 때
        else
        {
          PayRequireValue.text = string.Format(GameManager.Instance.GetTextData("PAYVALUE_TEXT"), GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 1), WNCText.GetGoldColor(_modifiedvalue));

          if(PayNoGoldHolder.activeInHierarchy.Equals(true))PayNoGoldHolder.SetActive(false);
          if (PayRequireValue.gameObject.activeInHierarchy.Equals(false)) PayRequireValue.gameObject.SetActive(true);

        }//골드 지불이 가능할 때
        break;//골드라면 지불,기본값,보정치,최종값을 받아오고 보정치가 존재한다면 텍스트에 삽입, 최종값이 보유값을 넘는다면 실패 확률 확인
    }

  //  PayIcon.sprite = _payicon;
    PayRequireValue.text = _payvaluetext;

    Vector2 _pivot = new Vector2(1.1f, 0.5f);
    switch (tendencytype)//성향 존재하는거면 그거 활성화
    {
      case TendencyTypeEnum.None:
        if (SelectionPayTendendcy.gameObject.activeInHierarchy.Equals(true)) SelectionPayTendendcy.gameObject.SetActive(false);
        break;
      case TendencyTypeEnum.Body:
        if (SelectionPayTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionPayTendendcy.gameObject.SetActive(true);
        SelectionPayTendendcy.Setup(GameManager.Instance.MyGameData.Tendency_Body, dir);
        _pivot = dir == true ? new Vector2(1.1f, 0.5f) : new Vector2(-0.1f, 0.5f);
        break;
      case TendencyTypeEnum.Head:
        if (SelectionPayTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionPayTendendcy.gameObject.SetActive(true);
        SelectionPayTendendcy.Setup(GameManager.Instance.MyGameData.Tendency_Head, dir);
        _pivot = dir == true ? new Vector2(1.1f, 0.5f) : new Vector2(-0.1f, 0.5f);
        break;
    }

    OpenPreviewPanel(SelectionPayPanel,_pivot,rect);
  }
  public void OpenSelectionCheckPreview_skill(SelectionData _selection, TendencyTypeEnum tendencytype, bool dir, RectTransform rect)
  {
    SelectionCheckBackground.sprite = GameManager.Instance.ImageHolder.SelectionBackground(tendencytype, dir);

    SetRewardInfo(_selection.SuccessData, SelectionCheckRewardText, SelectionCheckRewardIcon,
      _selection.FailureData, SelectionCheckPenaltyText, SelectionCheckPenaltyIcon);

    //  Sprite[] _icons = new Sprite[2];
    Skill[] _skills= new Skill[2];
    int _requirelevel = 0, _currentlevel = 0, _percentage = 0;
    string _requiretext = "",  _percentage_text = "", _percentage_int = "";//, _subdescription = "";

  //  _subdescription= _selection.SubDescription;
    _percentage_text = GameManager.Instance.GetTextData("SUCCESSPERCENT_TEXT");

    if (_selection.ThisSelectionType.Equals(SelectionTargetType.Check_Single))
    {
      _requirelevel = GameManager.Instance.MyGameData.CheckSkillSingleValue;

      _skills[0] = GameManager.Instance.MyGameData.GetSkill(_selection.SelectionCheckSkill[0]);
   //   _icons[0]=GameManager.Instance.ImageHolder.GetSkillIcon(_skills[0].MySkillType);
      _currentlevel = _skills[0].Level;
      _requiretext = string.Format(GameManager.Instance.GetTextData("LevelCheck_Text"),GameManager.Instance.GetTextData(_skills[0].MySkillType,2), _requirelevel);

    //  if (SelectionCheckIcons[1].transform.parent.gameObject.activeInHierarchy.Equals(true)) SelectionCheckIcons[1].transform.parent.gameObject.SetActive(false);
    }
    else
    {
      _requirelevel = GameManager.Instance.MyGameData.CheckSkillMultyValue;

      for(int i = 0; i < 2; i++)
      {
        _skills[i] = GameManager.Instance.MyGameData.GetSkill(_selection.SelectionCheckSkill[i]);
     //   _icons[i] = GameManager.Instance.ImageHolder.GetSkillIcon(_skills[i].MySkillType);
        _currentlevel += _skills[i].Level;
      }
      _requiretext = string.Format(GameManager.Instance.GetTextData("LevelCheck_Text"), 
        GameManager.Instance.GetTextData(_skills[0].MySkillType, 2)+"+"+ GameManager.Instance.GetTextData(_skills[1].MySkillType, 2),
        _requirelevel);

     // if (SelectionCheckIcons[1].transform.parent.gameObject.activeInHierarchy.Equals(false)) SelectionCheckIcons[1].transform.parent.gameObject.SetActive(true);
    }

    _percentage = GameManager.Instance.MyGameData.CheckPercent_themeorskill(_currentlevel, _requirelevel);
    _percentage_int = WNCText.PercentageColor(_percentage);

  //  SelectionCheckIcons[0].sprite = _icons[0];
  //  SelectionCheckIcons[1].sprite=_icons[1];
    SelectionCheckInfo.text = _requiretext;
    SelectionCheckPercent_text.text = _percentage_text;
    SelectionCheckPercent_int.text = _percentage_int;
    // SelectionCheckDescription.text = _subdescription;

    Vector2 _pivot = new Vector2(1.1f, 0.5f);
    switch (tendencytype)//성향 존재하는거면 그거 활성화
    {
      case TendencyTypeEnum.None:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(true)) SelectionCheckTendendcy.gameObject.SetActive(false);
        break;
      case TendencyTypeEnum.Body:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionCheckTendendcy.gameObject.SetActive(true);
        SelectionCheckTendendcy.Setup(GameManager.Instance.MyGameData.Tendency_Body, dir);
        _pivot = dir == true ? new Vector2(1.1f, 0.5f) : new Vector2(-0.1f, 0.5f);
        break;
      case TendencyTypeEnum.Head:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionCheckTendendcy.gameObject.SetActive(true);
        SelectionCheckTendendcy.Setup(GameManager.Instance.MyGameData.Tendency_Head, dir);
        _pivot = dir == true ? new Vector2(1.1f, 0.5f) : new Vector2(-0.1f, 0.5f);
        break;
    }

    OpenPreviewPanel(SelectionCheckPanel,_pivot,rect);
  }
  public void OpenSelectionElsePreview(SelectionData _selection, TendencyTypeEnum tendencytype, bool dir, RectTransform rect)
  {
    //안쓰는 상태
    SelectionElseBackground.sprite = GameManager.Instance.ImageHolder.SelectionBackground(tendencytype, dir);

    Sprite _icon = null;

    SelectionElseIcon.sprite = _icon;
  //  SelectionElseDescription.text = _selection.SubDescription;

    CurrentPreview=SelectionElsePanel.GetComponent<RectTransform>();

    switch (tendencytype)
    {
      case TendencyTypeEnum.None:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(true)) SelectionCheckTendendcy.gameObject.SetActive(false);
        break;
      case TendencyTypeEnum.Body:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionCheckTendendcy.gameObject.SetActive(true);
        SelectionElseTendency.Setup(GameManager.Instance.MyGameData.Tendency_Body, dir);
        break;
      case TendencyTypeEnum.Head:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionCheckTendendcy.gameObject.SetActive(true);
        SelectionElseTendency.Setup(GameManager.Instance.MyGameData.Tendency_Head, dir);
        break;
    }

    OpenPreviewPanel(SelectionElsePanel,rect);
  }
  //보상 설명 : 체력,정신력,돈 설명?
  public void OpenRewardStatusPreview(StatusTypeEnum status, int _value, RectTransform rect)
  {
    Sprite _icon = null;
    int  _modify = 0;
    string _valuetext="",_modifydescription = "";

    _icon = GameManager.Instance.ImageHolder.StatusIcon(status);
    _modify = (int)GameManager.Instance.MyGameData.GetHPGenModify(false);
    _valuetext = "+" + _value.ToString();
    if (_modify > 0)
    {
      _modifydescription = $"(+{GameManager.Instance.GetTextData(status,13)}{WNCText.PositiveColor(_modify.ToString())})";
      if (RewardStatusModify.gameObject.activeInHierarchy.Equals(false)) RewardStatusModify.gameObject.SetActive(true);
    }
    else
    {
      if (RewardStatusModify.gameObject.activeInHierarchy.Equals(true)) RewardStatusModify.gameObject.SetActive(false);
    }

    RewardStatusIcon.sprite = _icon;
    RewardStatusValue.text = _valuetext;
    RewardStatusModify.text= _modifydescription;
    RewardStatusClickText.text = GameManager.Instance.GetTextData("CLICKTOGET_TEXT");

    OpenPreviewPanel(RewardStatusPanel,rect);
  }
  public void OpenRewardExpPreview(Experience _exp, RectTransform rect)
  {
    string _name = "";
    Sprite _illust = null;
    string _effect = "";
    _name = _exp.Name;
    _illust = GameManager.Instance.ImageHolder.GetEXPIllust(_exp.ID);
    _effect = _exp.EffectString;

    RewardExpName.text = _name;
    RewardExpIllust.sprite = _illust;
    RewardExpEffect.text = _effect;
    RewardExpClickText.text= GameManager.Instance.GetTextData("CLICKTOGET_TEXT");


    OpenPreviewPanel(RewardExpPanel,rect);
  }
  public void OpenRewardSkillPreview(SkillTypeEnum skilltype, RectTransform rect)
  {
    string _name = $"{GameManager.Instance.GetTextData(skilltype,0)} +1";
    Sprite _icon = GameManager.Instance.ImageHolder.GetSkillIcon(skilltype,false);

    RewardSkillIcon.sprite = _icon;
    RewardSkillName.text = _name;
    RewardSkillClickText.text= GameManager.Instance.GetTextData("CLICKTOGET_TEXT");

    OpenPreviewPanel(RewardSkillPanel,rect);
  }
  public void OpenExpSelectionEmptyPreview(Experience _exp,bool islong, RectTransform rect)
  {
    string _turn, _description;

    _turn=islong?ConstValues.LongTermStartTurn.ToString():ConstValues.ShortTermStartTurn.ToString();
    if (islong)
    {
      _description =GameManager.Instance.GetTextData("LONGTERMSAVE_NAME")+"<br><br>"+ string.Format(GameManager.Instance.GetTextData("LONGTERMSAVE_DESCRIPTION"),ConstValues.LongTermStartTurn, ConstValues.LongTermChangeCost);
    }
    else
    {
      _description = GameManager.Instance.GetTextData("SHORTTERMSAVE_NAME") + "<br><br>" + string.Format(GameManager.Instance.GetTextData("SHORTTERMSAVE_DESCRIPTION"), ConstValues.ShortTermStartTurn);
    }

    ExpSelectEmptyTurn.text = _turn.ToString();
    ExpSelectEmptyEffect.text = _exp.EffectString;
    ExpSelectEmptyDescription.text = _description;
    ExpSelectClickText.text= GameManager.Instance.GetTextData("CLICKTOGET_TEXT");
    if (ExpSelectClickText.gameObject.activeInHierarchy.Equals(false)) ExpSelectClickText.gameObject.SetActive(true);

    OpenPreviewPanel(ExpSelectEmptyPanel,rect);
  }
  public void OpenExpSelectionExistPreview(Experience _origin,Experience _new,bool islong, RectTransform rect)
  {
    int _turn = islong ? ConstValues.LongTermStartTurn : ConstValues.ShortTermStartTurn;
    string _description = "";
    if (islong)
    {
      _description = GameManager.Instance.GetTextData("LONGTERMSHIFT_NAME") + string.Format(GameManager.Instance.GetTextData("LONGTERMSAVE_DESCRIPTION"), ConstValues.LongTermStartTurn, ConstValues.LongTermChangeCost);
    }
    else
    {
      _description = GameManager.Instance.GetTextData("SHORTTERMSHIFT_NAME") + string.Format(GameManager.Instance.GetTextData("SHORTTERMSAVE_DESCRIPTION"), ConstValues.ShortTermStartTurn);
    }

    string _origineffect = _origin.EffectString;
    ExpSelectOriginEffect.text = _origineffect;
    ExpSelectOriginTurn.text = _origin.Duration.ToString();

    string _neweffect = _new.EffectString;
    ExpSelectNewEffect.text = _neweffect;
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
    IconAndDescription_IconBackground.sprite = isstatus ? GameManager.Instance.ImageHolder.IconBackground_status : GameManager.Instance.ImageHolder.IconBackground_normal;
    IconAndDescription_Description.text = text;

    OpenPreviewPanel(IconAndDescription_Panel,rect);
  }
  public void OpenIconAndDescriptionPanel(Sprite icon, string text,Vector2 pivot, bool isstatus, RectTransform rect)
  {
    IconAndDescription_Icon.sprite = icon;
    IconAndDescription_IconBackground.sprite = isstatus ? GameManager.Instance.ImageHolder.IconBackground_status : GameManager.Instance.ImageHolder.IconBackground_normal;
    IconAndDescription_Description.text = text;

    OpenPreviewPanel(IconAndDescription_Panel,pivot,rect);
  }
  public void OpenDisComfortPanel(RectTransform rect)
  {
    OpenIconAndDescriptionPanel(GameManager.Instance.ImageHolder.DisComfort, GameManager.Instance.GetTextData("DISCOMFORT_DESECRIPTION"), DiscomfortPivot, false, rect);
  }
  public void OpenEnvirPanel(EnvironmentType envir)
  {
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
    SelectionNoneTendency.StopEffect();
    SelectionPayTendendcy.StopEffect();
    SelectionCheckTendendcy.StopEffect();
    SelectionElseTendency.StopEffect();
    CurrentPreview = null; 
  }


}
