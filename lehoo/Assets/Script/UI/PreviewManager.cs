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
  [SerializeField] private RectTransform WholeRect = null;
  [SerializeField] private Camera MainCamera = null;
  [Space(10)]
  [SerializeField] private GameObject IconAndDescription_Panel = null;
  [SerializeField] private Image IconAndDescription_Icon = null;
  [SerializeField] private TextMeshProUGUI IconAndDescription_Description = null;
  [Space(10)]
  [SerializeField] private GameObject JustDescription_Panel = null;
  [SerializeField] private TextMeshProUGUI JustDescriptionText = null;
  private int EffectFontSize = 25;
  private int SubdescriptionSize = 17;
  private Vector2 TurnPivot = new Vector2(0.5f, 1.1f);
  private Vector2 HPPivot= new Vector2(0.5f, 1.1f);
  private Vector2 SanityPivot= new Vector2(0.5f, 1.1f);
  private Vector2 GoldPivot= new Vector2(0.5f, 1.1f);
  private Vector2 MovePointPivot= new Vector2(0.5f, 1.1f);
  private Vector2 MapPivot= new Vector2(0.5f, 1.1f);
  private Vector2 TendencyPivot = new Vector2(1.1f, -0.1f);
  private Vector2 DiscomfortPivot = new Vector2(0.5f, 1.1f);
  [Space(10)]
  [SerializeField] private GameObject SkillPreview = null;
  [SerializeField] private Image SkillIcon = null;
  [SerializeField] private TextMeshProUGUI SkillLevel = null;
  [SerializeField] private TextMeshProUGUI SkillDescription = null;
  [Space(10)]
  [SerializeField] private GameObject ExpPreview = null;
  [SerializeField] private TextMeshProUGUI ExpName = null;
  [SerializeField] private TextMeshProUGUI ExpDuration = null;
  [SerializeField] private TextMeshProUGUI ExpDescription = null;
  [Space(10)]
  [SerializeField] private GameObject SelectionNonePanel = null;
  [SerializeField] private Image SelectionNoneBackground = null;
  //[SerializeField] private TextMeshProUGUI SelectionNoneText = null;
  [SerializeField] private PreviewSelectionTendency SelectionNoneTendency = null;
  [SerializeField] private Image SelectionNoneRewardIcon = null;
  [Space(10)]
  [SerializeField] private GameObject SelectionPayPanel = null;
  [SerializeField] private Image SelectionPayBackground = null;
  [SerializeField] private Image PayIcon = null;
  [SerializeField] private TextMeshProUGUI PayInfo = null;
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
  [SerializeField] private Image[] SelectionCheckIcons = null;
  [SerializeField] private TextMeshProUGUI SelectionCheckRequireLevel = null;
  [SerializeField] private TextMeshProUGUI SelectionCheckCurrentLevel = null;
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
  [SerializeField] private TextMeshProUGUI ExpSelectEmptyTurn = null;
  [SerializeField] private TextMeshProUGUI ExpSelectEmptyDescription = null;
  [Space(10)]
  [SerializeField] private GameObject ExpSelectExistPanel = null;
  [SerializeField] private TextMeshProUGUI ExpSelectOriginTurn = null;
  [SerializeField] private TextMeshProUGUI ExpSelectOriginEffect = null;
  [SerializeField] private TextMeshProUGUI ExpSelectNewTurn = null;
  [SerializeField] private TextMeshProUGUI ExpSelectNewEffect = null;
  [SerializeField] private TextMeshProUGUI ExpSelecitonExistDescription = null;
  [SerializeField] private TextMeshProUGUI ExpSelectClickText = null;
  [Space(10)]
  [SerializeField] private GameObject PlacePanel = null;
  [SerializeField] private Image PlaceIcon = null;
  [SerializeField] private Image PlaceThemeIcon = null;
  [SerializeField] private TextMeshProUGUI PlaceTurn = null;
  [SerializeField] private TextMeshProUGUI PlaceName = null;
  [SerializeField] private TextMeshProUGUI PlaceDescription = null;
  [SerializeField] private TextMeshProUGUI PlaceSubDescription = null;
  [Space(10)]
  [SerializeField] private GameObject EnvirPanel = null;
  [SerializeField] private Image EnvirIcon = null;
  [SerializeField] private TextMeshProUGUI EnvirName = null;
  [SerializeField] private TextMeshProUGUI EnvirDescription = null;
  private List<CanvasGroup> AllCanvasGroup = new List<CanvasGroup>();
  private void Awake()
  {
    AllCanvasGroup.Add(IconAndDescription_Panel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(JustDescription_Panel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(SkillPreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(ExpPreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(SelectionNonePanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(SelectionPayPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(SelectionCheckPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(SelectionElsePanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(RewardStatusPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(RewardExpPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(RewardSkillPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(ExpSelectEmptyPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(ExpSelectExistPanel.GetComponent<CanvasGroup>());
  }
  private RectTransform CurrentPreview = null;
  private void OpenPreviewPanel(GameObject panel,Vector2 pivot,RectTransform rect)
  {
    CurrentPreview = panel.GetComponent<RectTransform>();
    CurrentPreview.pivot = pivot;
    IEnumerator _cor = null;
    _cor = fadepreview(panel, true, rect);
    StartCoroutine(_cor);
  }
  private void OpenPreviewPanel(GameObject panel,RectTransform rect)
  {
    CurrentPreview = panel.GetComponent<RectTransform>();
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
    IconAndDescription_Icon.sprite = _turnsprite;
    IconAndDescription_Description.text = _name + "<br><br>" + WNCText.SetSize(SubdescriptionSize,WNCText.GetSubdescriptionColor(_description));

    OpenPreviewPanel(IconAndDescription_Panel,TurnPivot,rect);
  }//턴 미리보기 패널 세팅 후 열기
  public void OpenHPPreview(RectTransform rect)
  {
    StatusType _currenttype = StatusType.HP;

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


    IconAndDescription_Icon.sprite = GameManager.Instance.ImageHolder.HPIcon;
    IconAndDescription_Description.text = _description;
    OpenPreviewPanel(IconAndDescription_Panel,HPPivot,rect);
  }//체력 설명, 증감량 표기 후 열기
  public void OpenSanityPreview(RectTransform rect)
  {
    StatusType _currenttype = StatusType.Sanity;

    string _description = "";
    int _genvalue = 0, _payvalue = 0;

    _genvalue = (int)GameManager.Instance.MyGameData.GetSanityGenModify(false);
    _payvalue = (int)GameManager.Instance.MyGameData.GetSanityLossModify(false);

    _description = string.Format(GameManager.Instance.GetTextData(_currenttype, 3),GameManager.Instance.MyGameData.MaxSanity);
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


    IconAndDescription_Icon.sprite = GameManager.Instance.ImageHolder.SanityIcon;
    IconAndDescription_Description.text = _description;
    OpenPreviewPanel(IconAndDescription_Panel,SanityPivot,rect);
  }//정신력 설명,증감량 표기 후 열기
  public void OpenGoldPreview( RectTransform rect)
  {
    StatusType _currenttype = StatusType.Gold;

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


    IconAndDescription_Icon.sprite = GameManager.Instance.ImageHolder.GoldIcon;
    IconAndDescription_Description.text = _description;
    OpenPreviewPanel(IconAndDescription_Panel,GoldPivot,rect);
  }//골드 설명,증감량 표기 후 열기
  public void OpenMovePointPreview( RectTransform rect)
  {
    Sprite _icon = GameManager.Instance.ImageHolder.MovePointIcon_Enable;
    string _description = GameManager.Instance.GetTextData("MOVEPOINT_DESCRIPTION") + "<br><br>" + WNCText.SetSize(SubdescriptionSize, WNCText.GetSubdescriptionColor(GameManager.Instance.GetTextData("MOVEPOINT_SUBDESCRIPTION")));

    IconAndDescription_Icon.sprite = _icon;
    IconAndDescription_Description.text = _description;

    OpenPreviewPanel(IconAndDescription_Panel,MovePointPivot,rect);
  }
  public void OpenMapPreview()
  {
    Debug.Log("이거 어디서 나옴???");
  }//현재 이동 가능 여부에 따라 텍스트만 출력
  public void OpenQuestPreview()
  {
  }//현재 퀘스트 이름, 일러스트, 다음 내용                             수정요망
  public void OpenSkillPreview(SkillType _skilltype,RectTransform rect)
  {
    Sprite _icon = GameManager.Instance.ImageHolder.GetSkillIcon(_skilltype);
    string _description = GameManager.Instance.GetTextData(_skilltype,0)+"<br><br>"+WNCText.SetSize(SubdescriptionSize,WNCText.GetSubdescriptionColor(GameManager.Instance.GetTextData(_skilltype, 4)));

    int _level = GameManager.Instance.MyGameData.GetSkill(_skilltype).Level ;
    SkillLevel.text = _level.ToString();

    SkillDescription.text = _description;
    SkillIcon.sprite = _icon;

    OpenPreviewPanel(SkillPreview,rect);
  }
  public void OpenExpPreview(Experience _exp, RectTransform rect)
  {
    ExpName.text =_exp.Name;
    ExpDuration.text = $"{_exp.Duration}";
    string _description = WNCText.SetSize(EffectFontSize, _exp.ShortEffectString) + "<br><br>" + WNCText.SetSize(SubdescriptionSize, WNCText.GetSubdescriptionColor(_exp.SubDescription));
    ExpDescription.text = _description;

    LayoutRebuilder.ForceRebuildLayoutImmediate(ExpName.transform.parent.transform as RectTransform);

    OpenPreviewPanel(ExpPreview,rect);
  }
  public void OpenTendencyPreview(TendencyTypeEnum _type, RectTransform rect)
  {
    Sprite _tendencyicon = null;
    Tendency _targettendency = null;
    switch (_type)
    {
      case TendencyTypeEnum.Head:
        _tendencyicon = GameManager.Instance.ImageHolder.GetTendencyIcon(_type,GameManager.Instance.MyGameData.GetTendencyLevel(_type));
        _targettendency = GameManager.Instance.MyGameData.Tendency_Head;
        break;
      case TendencyTypeEnum.Body:
        _tendencyicon = GameManager.Instance.ImageHolder.GetTendencyIcon(_type, GameManager.Instance.MyGameData.GetTendencyLevel(_type));
        _targettendency = GameManager.Instance.MyGameData.Tendency_Body;
        break;
    }
    string _description = WNCText.SetSize(20, "<b>" + _targettendency.Name + "</b>") + "<br><br>" + 
      WNCText.SetSize(EffectFontSize, GameManager.Instance.MyGameData.GetTendencyEffectString_short(_type)) +
      "<br><br>" + WNCText.SetSize(SubdescriptionSize, WNCText.GetSubdescriptionColor(_targettendency.SubDescription));

    IconAndDescription_Icon.sprite = _tendencyicon;
    IconAndDescription_Description.text = _description;

    OpenPreviewPanel(IconAndDescription_Panel, TendencyPivot,rect);
  }
  public void OpenSelectionNonePreview(SelectionData _selection,TendencyTypeEnum tendencytype,bool dir, RectTransform rect)
  {
    SelectionNoneBackground.sprite = GameManager.Instance.ImageHolder.SelectionBackground(tendencytype, dir);

  //  SelectionNoneText.text = _selection.SubDescription;

    Sprite _rewardsprite = null;
    switch (_selection.SelectionSuccesReward)
    {
      case RewardTarget.HP:_rewardsprite = GameManager.Instance.ImageHolder.HPIcon;break;
      case RewardTarget.Sanity:_rewardsprite = GameManager.Instance.ImageHolder.SanityIcon;break;
      case RewardTarget.Gold:_rewardsprite = GameManager.Instance.ImageHolder.GoldIcon;break;
      case RewardTarget.Experience:_rewardsprite = GameManager.Instance.ImageHolder.UnknownExpRewardIcon;break;
      case RewardTarget.Skill:
        switch (_selection.SelectionRewardSkillType)
        {
          case SkillType.Conversation:_rewardsprite = GameManager.Instance.ImageHolder.ThemeIcon_Conversation;break;
          case SkillType.Force: _rewardsprite = GameManager.Instance.ImageHolder.ThemeIcon_Force; break;
          case SkillType.Wild: _rewardsprite = GameManager.Instance.ImageHolder.ThemeIcon_Wild; break;
          case SkillType.Intelligence: _rewardsprite = GameManager.Instance.ImageHolder.ThemeIcon_Intelligence; break;
        }
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
    switch (_selection.SelectionSuccesReward)
    {
      case RewardTarget.HP: _rewardsprite = GameManager.Instance.ImageHolder.HPIcon; break;
      case RewardTarget.Sanity: _rewardsprite = GameManager.Instance.ImageHolder.SanityIcon; break;
      case RewardTarget.Gold: _rewardsprite = GameManager.Instance.ImageHolder.GoldIcon; break;
      case RewardTarget.Experience: _rewardsprite = GameManager.Instance.ImageHolder.UnknownExpRewardIcon; break;
      case RewardTarget.Skill:
        switch (_selection.SelectionRewardSkillType)
        {
          case SkillType.Conversation: _rewardsprite = GameManager.Instance.ImageHolder.ThemeIcon_Conversation; break;
          case SkillType.Force: _rewardsprite = GameManager.Instance.ImageHolder.ThemeIcon_Force; break;
          case SkillType.Wild: _rewardsprite = GameManager.Instance.ImageHolder.ThemeIcon_Wild; break;
          case SkillType.Intelligence: _rewardsprite = GameManager.Instance.ImageHolder.ThemeIcon_Intelligence; break;
        }
        break;
    }
    PayRewardIcon.sprite = _rewardsprite;

    Sprite _payicon = null;
    int _modifiedvalue = 0;
    int _modify = 0;
    string _payvaluetext="", _statusinfo = "";
    int _percent = -1;
    StatusType _status = StatusType.HP;
    switch (_selection.SelectionPayTarget)
    {
      case StatusType.HP:
        _status = StatusType.HP;
        _payicon = GameManager.Instance.ImageHolder.HPDecreaseIcon;
        _modify = (int)GameManager.Instance.MyGameData.GetHPLossModify(false);
        _modifiedvalue = GameManager.Instance.MyGameData.PayHPValue_modified;
        _payvaluetext = string.Format(GameManager.Instance.GetTextData("PAYVALUE_TEXT"),GameManager.Instance.GetTextData(StatusType.HP,0), WNCText.GetHPColor(_modifiedvalue.ToString()));
        if (_modify.Equals(0)) _statusinfo = "";
        else if (_modify > 0)
        {
          _statusinfo = $"({GameManager.Instance.GetTextData(_status,15)}{WNCText.NegativeColor("+"+_modify.ToString())}%)";
        }//보정치가 0 이상이라면 부정적인것
        if (PayNoGoldHolder.activeInHierarchy.Equals(true)) PayNoGoldHolder.SetActive(false);
        if (PayRequireValue.gameObject.activeInHierarchy.Equals(false)) PayRequireValue.gameObject.SetActive(true);
        break;//체력이라면 지불 기본값, 보정치, 최종값을 받아오고 보정치가 존재한다면 텍스트에 삽입

      case StatusType.Sanity:
        _status = StatusType.Sanity;
        _payicon = GameManager.Instance.ImageHolder.SanityDecreaseIcon;
        _modify = (int)GameManager.Instance.MyGameData.GetSanityLossModify(false);
        _modifiedvalue = GameManager.Instance.MyGameData.PaySanityValue_modified;
        _payvaluetext = string.Format(GameManager.Instance.GetTextData("PAYVALUE_TEXT"), GameManager.Instance.GetTextData(StatusType.Sanity, 0), WNCText.GetSanityColor(_modifiedvalue.ToString()));
        if (_modify.Equals(0)) _statusinfo = "";
        else if (_modify > 0)
        {
          _statusinfo = $"{GameManager.Instance.GetTextData(_status, 15)} {WNCText.NegativeColor("+" + _modify.ToString())}%";
          PayInfo.text = _statusinfo;
        }//보정치가 0 이상이라면 부정적인것
        else
        {
          PayInfo.text = "";
        }//보정치가 없다면 빈 내용으로

        if (PayNoGoldHolder.activeInHierarchy.Equals(true)) PayNoGoldHolder.SetActive(false);
        if (PayRequireValue.gameObject.activeInHierarchy.Equals(false)) PayRequireValue.gameObject.SetActive(true);
        break;//정신력이라면 지불 기본값,보정치,최종값을 받아오고 보정치가 존재한다면 텍스트에 삽입
      case StatusType.Gold:
        _status = StatusType.Gold;
        _payicon = GameManager.Instance.ImageHolder.GoldDecreaseIcon;
        _modify = (int)GameManager.Instance.MyGameData.GetGoldPayModify(false);
        _modifiedvalue = GameManager.Instance.MyGameData.PayGoldValue_modified;
        if (_modify.Equals(0)) _statusinfo = "";
        else if (_modify > 0)
        {
          _statusinfo = $"{GameManager.Instance.GetTextData(_status, 15)} {WNCText.NegativeColor("+" + _modify.ToString())}%";
        }//보정치가 0 이상이라면 부정적인것

        if (_modifiedvalue > GameManager.Instance.MyGameData.Gold)
        {
          _percent = GameManager.Instance.MyGameData.CheckPercent_money(_modifiedvalue);
          int _sanitypayvalue = (int)((_modifiedvalue - GameManager.Instance.MyGameData.Gold) * ConstValues.GoldSanityPayAmplifiedValue);

          PayNoGold_Text.text = GameManager.Instance.GetTextData("NOGOLD_TEXT");
          PayNoGold_PercentText.text = GameManager.Instance.GetTextData("SUCCESSPERCENT_TEXT");
          PayNoGold_PercentValue.text = WNCText.PercentageColor(_percent);
          PayNoGold_Alternative.text = string.Format(GameManager.Instance.GetTextData("NOGOLD_PERCENTAGE_TEXT"),
            GameManager.Instance.GetTextData(StatusType.Gold, 2), WNCText.GetGoldColor(GameManager.Instance.MyGameData.Gold),
            GameManager.Instance.GetTextData(StatusType.Sanity, 2), WNCText.GetSanityColor(_sanitypayvalue.ToString()));

          if (PayNoGoldHolder.activeInHierarchy.Equals(false)) PayNoGoldHolder.SetActive(true);
          if (PayRequireValue.gameObject.activeInHierarchy.Equals(true)) PayRequireValue.gameObject.SetActive(false);

        }//지불 골드 값이 보유 값에 비해 높을 때
        else
        {
          PayRequireValue.text = string.Format(GameManager.Instance.GetTextData("PAYVALUE_TEXT"), GameManager.Instance.GetTextData(StatusType.Gold, 0), WNCText.GetGoldColor(_modifiedvalue));

          if(PayNoGoldHolder.activeInHierarchy.Equals(true))PayNoGoldHolder.SetActive(false);
          if (PayRequireValue.gameObject.activeInHierarchy.Equals(false)) PayRequireValue.gameObject.SetActive(true);

        }//골드 지불이 가능할 때
        break;//골드라면 지불,기본값,보정치,최종값을 받아오고 보정치가 존재한다면 텍스트에 삽입, 최종값이 보유값을 넘는다면 실패 확률 확인
    }

    PayIcon.sprite = _payicon;
    PayRequireValue.text = _payvaluetext;


    switch (tendencytype)//성향 존재하는거면 그거 활성화
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
    switch (_selection.SelectionSuccesReward)
    {
      case RewardTarget.HP: _rewardsprite = GameManager.Instance.ImageHolder.HPIcon; break;
      case RewardTarget.Sanity: _rewardsprite = GameManager.Instance.ImageHolder.SanityIcon; break;
      case RewardTarget.Gold: _rewardsprite = GameManager.Instance.ImageHolder.GoldIcon; break;
      case RewardTarget.Experience: _rewardsprite = GameManager.Instance.ImageHolder.UnknownExpRewardIcon; break;
      case RewardTarget.Skill:
        switch (_selection.SelectionRewardSkillType)
        {
          case SkillType.Conversation: _rewardsprite = GameManager.Instance.ImageHolder.ThemeIcon_Conversation; break;
          case SkillType.Force: _rewardsprite = GameManager.Instance.ImageHolder.ThemeIcon_Force; break;
          case SkillType.Wild: _rewardsprite = GameManager.Instance.ImageHolder.ThemeIcon_Wild; break;
          case SkillType.Intelligence: _rewardsprite = GameManager.Instance.ImageHolder.ThemeIcon_Intelligence; break;
        }
        break;
    }
    CheckRewardIcon.sprite = _rewardsprite;


    Sprite[] _icons = new Sprite[2];
    Skill[] _skills= new Skill[2];
    int _requirelevel = 0, _currentlevel = 0, _percentage = 0;
    string _requiretext = "", _currenttext = "", _skillinfo = "", _percentage_text = "", _percentage_int = "", _subdescription = "";

  //  _subdescription= _selection.SubDescription;
    _percentage_text = GameManager.Instance.GetTextData("SUCCESSPERCENT_TEXT");

    if (_selection.ThisSelectionType.Equals(SelectionTargetType.Check_Single))
    {
      _requirelevel = GameManager.Instance.MyGameData.CheckSkillSingleValue;

      _skills[0] = GameManager.Instance.MyGameData.GetSkill(_selection.SelectionCheckSkill[0]);
      _icons[0]=GameManager.Instance.ImageHolder.GetSkillIcon(_skills[0].MySkillType);
      _currentlevel = _skills[0].Level;
      _skillinfo = "";

      if (SelectionCheckIcons[1].transform.parent.gameObject.activeInHierarchy.Equals(true)) SelectionCheckIcons[1].transform.parent.gameObject.SetActive(false);
    }
    else
    {
      _requirelevel = GameManager.Instance.MyGameData.CheckSkillMultyValue;

      for(int i = 0; i < 2; i++)
      {
        _skills[i] = GameManager.Instance.MyGameData.GetSkill(_selection.SelectionCheckSkill[i]);
        _icons[i] = GameManager.Instance.ImageHolder.GetSkillIcon(_skills[i].MySkillType);
        _currentlevel += _skills[i].Level;
        if (_skillinfo != "") _skillinfo += " ";
        _skillinfo += string.Format("{0}<b>{1}</b>", GameManager.Instance.GetTextData(_skills[i].MySkillType, 1), _skills[i].Level);
      }
      if (SelectionCheckIcons[1].transform.parent.gameObject.activeInHierarchy.Equals(false)) SelectionCheckIcons[1].transform.parent.gameObject.SetActive(true);
    }

    _requiretext = string.Format(GameManager.Instance.GetTextData("REQUIRELEVEL_TEXT"), _requirelevel);
    _percentage = GameManager.Instance.MyGameData.CheckPercent_themeorskill(_currentlevel, _requirelevel);
    _percentage_int = WNCText.PercentageColor(_percentage);
    _currenttext = string.Format(GameManager.Instance.GetTextData("CURRENTLEVEL_TEXT") + "\n{1}", _currentlevel, _skillinfo);

    SelectionCheckIcons[0].sprite = _icons[0];
    SelectionCheckIcons[1].sprite=_icons[1];
    SelectionCheckRequireLevel.text = _requiretext;
    SelectionCheckCurrentLevel.text= _currenttext;
    SelectionCheckPercent_text.text = _percentage_text;
    SelectionCheckPercent_int.text = _percentage_int;
   // SelectionCheckDescription.text = _subdescription;

    switch (tendencytype)//성향 존재하는거면 그거 활성화
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
  public void OpenRewardStatusPreview(StatusType status, int _value, RectTransform rect)
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
  public void OpenRewardSkillPreview(SkillType skilltype, RectTransform rect)
  {
    string _name = $"{GameManager.Instance.GetTextData(skilltype,0)} +1";
    Sprite _icon = GameManager.Instance.ImageHolder.GetSkillIcon(skilltype);

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
  public void OpenIconAndDescriptionPanel(Sprite icon,string text, RectTransform rect)
  {
    IconAndDescription_Icon.sprite = icon;
    IconAndDescription_Description.text = text;

    OpenPreviewPanel(IconAndDescription_Panel,rect);
  }
  public void OpenIconAndDescriptionPanel(Sprite icon, string text,Vector2 pivot, RectTransform rect)
  {
    IconAndDescription_Icon.sprite = icon;
    IconAndDescription_Description.text = text;

    OpenPreviewPanel(IconAndDescription_Panel,pivot,rect);
  }
  public void OpenDisComfortPanel(RectTransform rect)
  {
    IconAndDescription_Icon.sprite = GameManager.Instance.ImageHolder.DisComfort;
     IconAndDescription_Description.text = GameManager.Instance.GetTextData("DISCOMFORT_DESECRIPTION");

    OpenPreviewPanel(IconAndDescription_Panel,DiscomfortPivot,rect);
  }
  public void OpenEnvirPanel(EnvironmentType envir)
  {
    EnvirIcon.sprite = GameManager.Instance.ImageHolder.GetEnvirTile(envir);
    EnvirName.text = GameManager.Instance.GetTextData(envir, 0);
    EnvirDescription.text = GameManager.Instance.GetTextData(envir,1);

    CurrentPreview = EnvirPanel.GetComponent<RectTransform>();
    IEnumerator _cor = null;
    StartCoroutine(_cor);
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
    LayoutRebuilder.ForceRebuildLayoutImmediate(_targetobj.transform as RectTransform);

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
