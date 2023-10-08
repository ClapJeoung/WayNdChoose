using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OpenCvSharp.Tracking;
using JetBrains.Annotations;
using OpenCvSharp;

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
  private Vector2 DiscomfortPivot = new Vector2(0.5f, 1.1f);
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
  [SerializeField] private Image SelectionNoneRewardIcon = null;
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
  [SerializeField] private Image PayRewardIcon = null;
  [SerializeField] private PreviewSelectionTendency SelectionPayTendendcy = null;
  [Space(10)]
  [SerializeField] private GameObject SelectionCheckPanel = null;
  [SerializeField] private Image SelectionCheckBackground = null;
 // [SerializeField] private Image[] SelectionCheckIcons = null;
  [SerializeField] private TextMeshProUGUI SelectionCheckInfo = null;
 // [SerializeField] private TextMeshProUGUI SelectionCheckCurrentLevel = null;
  [SerializeField] private TextMeshProUGUI SelectionCheckPercent_text = null;
  [SerializeField] private TextMeshProUGUI SelectionCheckPercent_int = null;
  [SerializeField] private Image CheckRewardIcon = null;
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

    IEnumerator _cor = null;
    _cor = fadepreview(panel, true, rect);
    StartCoroutine(_cor);
  }
  private void OpenPreviewPanel(GameObject panel,RectTransform rect)
  {
    CurrentPreview = panel.GetComponent<RectTransform>();

    LayoutRebuilder.ForceRebuildLayoutImmediate(panel.transform as RectTransform);
    
    IEnumerator _cor = null;
    _cor = fadepreview(panel, true,rect);
    StartCoroutine(_cor);
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
  }//�� �̸����� �г� ���� �� ����
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
  }//ü�� ����, ������ ǥ�� �� ����
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
  }//���ŷ� ����,������ ǥ�� �� ����
  public void OpenGoldPreview( RectTransform rect)
  {
    StatusTypeEnum _currenttype = StatusTypeEnum.Gold;

    string _description = "";
    int _genvalue = 0, _payvalue = 0;

    _genvalue = (int)GameManager.Instance.MyGameData.GetGoldGenModify(false);
    _payvalue = (int)GameManager.Instance.MyGameData.GetGoldPayModify(false);

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
  }//��� ����,������ ǥ�� �� ����
  public void OpenMovePointPreview( RectTransform rect)
  {
    Sprite _icon = GameManager.Instance.ImageHolder.MovePointIcon_Enable;
    string _description = GameManager.Instance.GetTextData("MOVEPOINT_DESCRIPTION") + "<br><br>" + WNCText.SetSize(SubdescriptionSize, WNCText.GetSubdescriptionColor(GameManager.Instance.GetTextData("MOVEPOINT_SUBDESCRIPTION")));


    OpenIconAndDescriptionPanel(_icon, _description, MovePointPivot, true, rect);
  }
  public void OpenMapPreview()
  {
    Debug.Log("�̰� ��� ����???");
  }//���� �̵� ���� ���ο� ���� �ؽ�Ʈ�� ���
  public void OpenQuestPreview()
  {
  }//���� ����Ʈ �̸�, �Ϸ���Ʈ, ���� ����                             �������
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
    string _description = WNCText.SetSize(EffectFontSize, _exp.ShortEffectString) + "<br><br>" + WNCText.SetSize(SubdescriptionSize, WNCText.GetSubdescriptionColor(_exp.SubDescription));
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
    Sprite _icon_left=_targettendency.GetNextIcon(false), _icon_right=_targettendency.GetNextIcon(true);
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
    string _description = WNCText.SetSize(EffectFontSize, GameManager.Instance.MyGameData.GetTendencyEffectString_short(_type)) +
      "<br><br>" + WNCText.SetSize(SubdescriptionSize, WNCText.GetSubdescriptionColor(_targettendency.SubDescription));

    OpenPreviewPanel(TendencyPreview, TendencyPreview.transform as RectTransform);
  }
  public void OpenSelectionNonePreview(SelectionData _selection,TendencyTypeEnum tendencytype,bool dir, RectTransform rect)
  {
    SelectionNoneBackground.sprite = GameManager.Instance.ImageHolder.SelectionBackground(tendencytype, dir);

  //  SelectionNoneText.text = _selection.SubDescription;

    Sprite _rewardsprite = null;
    switch (_selection.SuccessData.Reward_Type)
    {
      case RewardTypeEnum.Status:
        switch (_selection.SuccessData.Reward_StatusType)
        {
          case StatusTypeEnum.HP: _rewardsprite = GameManager.Instance.ImageHolder.HPIcon; break;
          case StatusTypeEnum.Sanity: _rewardsprite = GameManager.Instance.ImageHolder.SanityIcon; break;
          case StatusTypeEnum.Gold: _rewardsprite = GameManager.Instance.ImageHolder.GoldIcon; break;
        }
        break;
      case RewardTypeEnum.Experience:_rewardsprite = GameManager.Instance.ImageHolder.UnknownExpRewardIcon;break;
      case RewardTypeEnum.Skill:
        _rewardsprite = GameManager.Instance.ImageHolder.GetSkillIcon(_selection.SuccessData.Reward_SkillType, false);
        break;
    }
    SelectionNoneRewardIcon.sprite= _rewardsprite;

    switch (tendencytype)
    {
      case TendencyTypeEnum.None:
        if (SelectionNoneTendency.gameObject.activeInHierarchy.Equals(true)) SelectionNoneTendency.gameObject.SetActive(false);
        break;
      case TendencyTypeEnum.Body:
        if (SelectionNoneTendency.gameObject.activeInHierarchy.Equals(false)) SelectionNoneTendency.gameObject.SetActive(true);
        SelectionNoneTendency.Setup(GameManager.Instance.MyGameData.Tendency_Body,dir);
        break;
      case TendencyTypeEnum.Head:
        if (SelectionNoneTendency.gameObject.activeInHierarchy.Equals(false)) SelectionNoneTendency.gameObject.SetActive(true);
        SelectionNoneTendency.Setup(GameManager.Instance.MyGameData.Tendency_Head, dir);
        break;
    }

    OpenPreviewPanel(SelectionNonePanel,rect);
  }
  public void OpenSelectionPayPreview(SelectionData _selection, TendencyTypeEnum tendencytype, bool dir, RectTransform rect)
  {
    SelectionPayBackground.sprite = GameManager.Instance.ImageHolder.SelectionBackground(tendencytype, dir);

    //PaySubDescription.text = _selection.SubDescription;

    Sprite _rewardsprite = null;
    switch (_selection.SuccessData.Reward_Type)
    {
      case RewardTypeEnum.Status:
        switch (_selection.SuccessData.Reward_StatusType)
        {
          case StatusTypeEnum.HP: _rewardsprite = GameManager.Instance.ImageHolder.HPIcon; break;
          case StatusTypeEnum.Sanity: _rewardsprite = GameManager.Instance.ImageHolder.SanityIcon; break;
          case StatusTypeEnum.Gold: _rewardsprite = GameManager.Instance.ImageHolder.GoldIcon; break;
        }
        break;
      case RewardTypeEnum.Experience: _rewardsprite = GameManager.Instance.ImageHolder.UnknownExpRewardIcon; break;
      case RewardTypeEnum.Skill:
        _rewardsprite = GameManager.Instance.ImageHolder.GetSkillIcon(_selection.SuccessData.Reward_SkillType, false);
        break;
    }
    PayRewardIcon.sprite = _rewardsprite;

    Sprite _payicon = null;
    int _modifiedvalue = 0;
    int _modify = 0;
    string _payvaluetext="", _statusinfo = "";
    int _percent = -1;
    StatusTypeEnum _status = StatusTypeEnum.HP;
    switch (_selection.SelectionPayTarget)
    {
      case StatusTypeEnum.HP:
        _status = StatusTypeEnum.HP;
        _payicon = GameManager.Instance.ImageHolder.HPDecreaseIcon;
        _modify = (int)GameManager.Instance.MyGameData.GetHPLossModify(false);
        _modifiedvalue = GameManager.Instance.MyGameData.PayHPValue_modified;
        _payvaluetext = string.Format(GameManager.Instance.GetTextData("PAYVALUE_TEXT"),GameManager.Instance.GetTextData(StatusTypeEnum.HP,1), WNCText.GetHPColor(_modifiedvalue.ToString()));
        if (_modify.Equals(0)) _statusinfo = "";
        else if (_modify > 0)
        {
          _statusinfo = $"({GameManager.Instance.GetTextData(_status,15)}{WNCText.NegativeColor("+"+_modify.ToString())}%)";
        }//����ġ�� 0 �̻��̶�� �������ΰ�
        if (PayNoGoldHolder.activeInHierarchy.Equals(true)) PayNoGoldHolder.SetActive(false);
        if (PayRequireValue.gameObject.activeInHierarchy.Equals(false)) PayRequireValue.gameObject.SetActive(true);
        break;//ü���̶�� ���� �⺻��, ����ġ, �������� �޾ƿ��� ����ġ�� �����Ѵٸ� �ؽ�Ʈ�� ����

      case StatusTypeEnum.Sanity:
        _status = StatusTypeEnum.Sanity;
        _payicon = GameManager.Instance.ImageHolder.SanityDecreaseIcon;
        _modify = (int)GameManager.Instance.MyGameData.GetSanityLossModify(false);
        _modifiedvalue = GameManager.Instance.MyGameData.PaySanityValue_modified;
        _payvaluetext = string.Format(GameManager.Instance.GetTextData("PAYVALUE_TEXT"), GameManager.Instance.GetTextData(StatusTypeEnum.Sanity, 1), WNCText.GetSanityColor(_modifiedvalue.ToString()));
        if (_modify.Equals(0)) _statusinfo = "";
        else if (_modify > 0)
        {
          _statusinfo = $"{GameManager.Instance.GetTextData(_status, 15)} {WNCText.NegativeColor("+" + _modify.ToString())}%";
      //    PayInfo.text = _statusinfo;
        }//����ġ�� 0 �̻��̶�� �������ΰ�
        else
        {
       //   PayInfo.text = "";
        }//����ġ�� ���ٸ� �� ��������

        if (PayNoGoldHolder.activeInHierarchy.Equals(true)) PayNoGoldHolder.SetActive(false);
        if (PayRequireValue.gameObject.activeInHierarchy.Equals(false)) PayRequireValue.gameObject.SetActive(true);
        break;//���ŷ��̶�� ���� �⺻��,����ġ,�������� �޾ƿ��� ����ġ�� �����Ѵٸ� �ؽ�Ʈ�� ����
      case StatusTypeEnum.Gold:
        _status = StatusTypeEnum.Gold;
        _payicon = GameManager.Instance.ImageHolder.GoldDecreaseIcon;
        _modify = (int)GameManager.Instance.MyGameData.GetGoldPayModify(false);
        _modifiedvalue = GameManager.Instance.MyGameData.PayGoldValue_modified;
        if (_modify.Equals(0)) _statusinfo = "";
        else if (_modify > 0)
        {
          _statusinfo = $"{GameManager.Instance.GetTextData(_status, 15)} {WNCText.NegativeColor("+" + _modify.ToString())}%";
        }//����ġ�� 0 �̻��̶�� �������ΰ�

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

        }//���� ��� ���� ���� ���� ���� ���� ��
        else
        {
          PayRequireValue.text = string.Format(GameManager.Instance.GetTextData("PAYVALUE_TEXT"), GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 1), WNCText.GetGoldColor(_modifiedvalue));

          if(PayNoGoldHolder.activeInHierarchy.Equals(true))PayNoGoldHolder.SetActive(false);
          if (PayRequireValue.gameObject.activeInHierarchy.Equals(false)) PayRequireValue.gameObject.SetActive(true);

        }//��� ������ ������ ��
        break;//����� ����,�⺻��,����ġ,�������� �޾ƿ��� ����ġ�� �����Ѵٸ� �ؽ�Ʈ�� ����, �������� �������� �Ѵ´ٸ� ���� Ȯ�� Ȯ��
    }

  //  PayIcon.sprite = _payicon;
    PayRequireValue.text = _payvaluetext;


    switch (tendencytype)//���� �����ϴ°Ÿ� �װ� Ȱ��ȭ
    {
      case TendencyTypeEnum.None:
        if (SelectionPayTendendcy.gameObject.activeInHierarchy.Equals(true)) SelectionPayTendendcy.gameObject.SetActive(false);
        break;
      case TendencyTypeEnum.Body:
        if (SelectionPayTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionPayTendendcy.gameObject.SetActive(true);
        SelectionPayTendendcy.Setup(GameManager.Instance.MyGameData.Tendency_Body, dir);
        break;
      case TendencyTypeEnum.Head:
        if (SelectionPayTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionPayTendendcy.gameObject.SetActive(true);
        SelectionPayTendendcy.Setup(GameManager.Instance.MyGameData.Tendency_Head, dir);
        break;
    }

    OpenPreviewPanel(SelectionPayPanel,rect);
  }
  public void OpenSelectionCheckPreview_skill(SelectionData _selection, TendencyTypeEnum tendencytype, bool dir, RectTransform rect)
  {
    SelectionCheckBackground.sprite = GameManager.Instance.ImageHolder.SelectionBackground(tendencytype, dir);

    Sprite _rewardsprite = null;
    switch (_selection.SuccessData.Reward_Type)
    {
      case RewardTypeEnum.Status:
        switch (_selection.SuccessData.Reward_StatusType)
        {
          case StatusTypeEnum.HP: _rewardsprite = GameManager.Instance.ImageHolder.HPIcon; break;
          case StatusTypeEnum.Sanity: _rewardsprite = GameManager.Instance.ImageHolder.SanityIcon; break;
          case StatusTypeEnum.Gold: _rewardsprite = GameManager.Instance.ImageHolder.GoldIcon; break;
        }
        break;
      case RewardTypeEnum.Experience: _rewardsprite = GameManager.Instance.ImageHolder.UnknownExpRewardIcon; break;
      case RewardTypeEnum.Skill:
        _rewardsprite = GameManager.Instance.ImageHolder.GetSkillIcon(_selection.SuccessData.Reward_SkillType, false);
        break;
    }
    CheckRewardIcon.sprite = _rewardsprite;


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

    switch (tendencytype)//���� �����ϴ°Ÿ� �װ� Ȱ��ȭ
    {
      case TendencyTypeEnum.None:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(true)) SelectionCheckTendendcy.gameObject.SetActive(false);
        break;
      case TendencyTypeEnum.Body:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionCheckTendendcy.gameObject.SetActive(true);
        SelectionCheckTendendcy.Setup(GameManager.Instance.MyGameData.Tendency_Body, dir);
        break;
      case TendencyTypeEnum.Head:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionCheckTendendcy.gameObject.SetActive(true);
        SelectionCheckTendendcy.Setup(GameManager.Instance.MyGameData.Tendency_Head, dir);
        break;
    }

    OpenPreviewPanel(SelectionCheckPanel,rect);
  }
  public void OpenSelectionElsePreview(SelectionData _selection, TendencyTypeEnum tendencytype, bool dir, RectTransform rect)
  {
    //�Ⱦ��� ����
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
  //���� ���� : ü��,���ŷ�,�� ����?
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
    ExpSelectEmptyEffect.text = _exp.ShortEffectString;
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

    string _origineffect = _origin.ShortEffectString;
    ExpSelectOriginEffect.text = _origineffect;
    ExpSelectOriginTurn.text = _origin.Duration.ToString();

    string _neweffect = _new.ShortEffectString;
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
  //  CurrentPreview.anchoredPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
  }
  public void ClosePreview() 
  {
    if (CurrentPreview == null) return;
    StopAllCoroutines();
        CurrentPreview.GetComponent<CanvasGroup>().alpha = 0.0f;
    SelectionNoneTendency.StopEffect();
    SelectionPayTendendcy.StopEffect();
    SelectionCheckTendendcy.StopEffect();
    SelectionElseTendency.StopEffect();
    CurrentPreview = null; 
  }

  private IEnumerator fadepreview(GameObject _targetobj, bool _isopen, RectTransform targetrect)
  {
    CurrentPreview.position = targetrect.position;
    CurrentPreview.anchoredPosition3D = new Vector3(CurrentPreview.anchoredPosition3D.x, CurrentPreview.anchoredPosition3D.y, 0.0f);

    CanvasGroup _mygroup = _targetobj.GetComponent<CanvasGroup>();
    if (_isopen) yield return new WaitForSeconds(0.1f);

    float _startalpha = _isopen == true ? 0.0f : 1.0f;
    float _targetalpha = _isopen == true ? 1.0f : 0.0f;
    float _targettime = UIManager.Instance.PreviewFadeTime;
    float _currentalpha = _mygroup.alpha;
    float _currenttime = _isopen ? _targettime * _currentalpha : _targettime * (1 - _currentalpha);
    while (_currenttime < _targettime)
    {
      _currentalpha = Mathf.Lerp(_startalpha, _targetalpha, _currenttime / _targettime);
      _mygroup.alpha = _currentalpha;

      _currenttime += Time.deltaTime;
      yield return null;
    }
    _mygroup.alpha = _targetalpha;

  }

}
