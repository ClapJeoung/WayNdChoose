using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OpenCvSharp.Tracking;
using JetBrains.Annotations;

public class PreviewManager : MonoBehaviour
{
  [SerializeField] private RectTransform WholeRect = null;
  [SerializeField] private Camera MainCamera = null;
  [SerializeField] private Color PositiveColor;
  [SerializeField] private Color NegativeColor;
  [Space(10)]
  [SerializeField] private GameObject TurnPreview = null;
  [SerializeField] private Image TurnIcon = null;
  [SerializeField] private TextMeshProUGUI TurnDescription = null;
  [SerializeField] private TextMeshProUGUI TurnSubDescription = null;
  [Space(10)]
  [SerializeField] private GameObject HPPreview = null;
  [SerializeField] private TextMeshProUGUI HPDescription = null;
  [SerializeField] private TextMeshProUGUI HPInfoText = null;
  [SerializeField] private TextMeshProUGUI HpSubDescription = null;
  [Space(10)]
  [Space(10)]
  [SerializeField] private GameObject SanityPreview = null;
  [SerializeField] private TextMeshProUGUI SanityDescription = null;
  [SerializeField] private Image[] SanityMadnessIcons = new Image[3];
  [SerializeField] private TextMeshProUGUI SanityInfoText = null;
  [SerializeField] private TextMeshProUGUI SanitySubDescription = null;
  [Space(10)]
  [SerializeField] private GameObject GoldPreview = null;
  [SerializeField] private TextMeshProUGUI GoldDescription = null;
  [SerializeField] private TextMeshProUGUI GoldInfoText = null;
  [SerializeField] private TextMeshProUGUI GoldSubDescription = null;
  [Space(10)]
  [SerializeField] private GameObject MapPreview = null;
  [SerializeField] private TextMeshProUGUI MapMovableDescription = null;
  [SerializeField] private TextMeshProUGUI MapSubDescription = null;
  [Space(10)]
  [SerializeField] private GameObject QuestPreview = null;
  [SerializeField] private TextMeshProUGUI QuestName = null;
  [SerializeField] private Image QuestIllust = null;
  [SerializeField] private TextMeshProUGUI NextQuestEventDescription = null;
  [SerializeField] private TextMeshProUGUI QuestSubDescription = null;
  [Space(10)]
  [SerializeField] private GameObject SkillPreview = null;
  [SerializeField] private TextMeshProUGUI SkillName = null;
  [SerializeField] private TextMeshProUGUI SkillLevel = null;
  [SerializeField] private Image SkillIcon = null;
  [SerializeField] private TextMeshProUGUI SkillSubDescription = null;
  [Space(10)]
  [SerializeField] private GameObject ExpPreview = null;
  [SerializeField] private TextMeshProUGUI ExpName = null;
  [SerializeField] private TextMeshProUGUI ExpDuration = null;
  [SerializeField] private TextMeshProUGUI ExpEffect = null;
  [SerializeField] private TextMeshProUGUI ExpSubDescription = null;
  [Space(10)]
  [SerializeField] private GameObject TendencyPreview = null;
  [SerializeField] private Image TendencyIcon = null;
  [SerializeField] private TextMeshProUGUI TendencyName = null;
  [SerializeField] private TextMeshProUGUI TendencyDescription = null;
  [SerializeField] private TextMeshProUGUI TendencySubDescription = null;
  [Space(10)]
  [SerializeField] private GameObject SelectionNonePanel = null;
  [SerializeField] private Image SelectionNoneBackground = null;
  [SerializeField] private TextMeshProUGUI SelectionNoneText = null;
  [SerializeField] private PreviewSelectionTendency SelectionNoneTendency = null;
  [SerializeField] private Transform NoneRewardIcons = null;
  [Space(10)]
  [SerializeField] private GameObject SelectionPayPanel = null;
  [SerializeField] private Image SelectionPayBackground = null;
  [SerializeField] private Image PayIcon = null;
  [SerializeField] private TextMeshProUGUI PayInfo = null;
  [SerializeField] private TextMeshProUGUI PayRequireValue = null;
  [SerializeField] private GameObject PayNoGoldHolder = null;
  [SerializeField] private TextMeshProUGUI PayNoGoldText = null;
  [SerializeField] private TextMeshProUGUI PayNoGoldValue = null;
  [SerializeField] private TextMeshProUGUI PayNoGoldPercentText = null;
  [SerializeField] private TextMeshProUGUI PayNoGoldPercentValue = null;
  [SerializeField] private TextMeshProUGUI PaySubDescription = null;
  [SerializeField] private Transform PayRewardIcons = null;
  [SerializeField] private PreviewSelectionTendency SelectionPayTendendcy = null;
  [Space(10)]
  [SerializeField] private GameObject SelectionCheckPanel = null;
  [SerializeField] private Image SelectionCheckBackground = null;
  [SerializeField] private Image[] SelectionCheckIcons = null;
  [SerializeField] private TextMeshProUGUI SelectionCheckRequireLevel = null;
  [SerializeField] private TextMeshProUGUI SelectionCheckCurrentLevel = null;
  [SerializeField] private TextMeshProUGUI SelectionCheckPercent_text = null;
  [SerializeField] private TextMeshProUGUI SelectionCheckPercent_int = null;
  [SerializeField] private Transform CheckRewardIcons = null;
  [SerializeField] private TextMeshProUGUI SelectionCheckDescription = null;
  [SerializeField] private PreviewSelectionTendency SelectionCheckTendendcy = null;
  [Space(10)]
  [SerializeField] private GameObject SelectionElsePanel = null;
  [SerializeField] private Image SelectionElseBackground = null;
  [SerializeField] private Image SelectionElseIcon = null;
  [SerializeField] private TextMeshProUGUI SelectionElseDescription = null;
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
  [SerializeField] private GameObject JustDescriptionPanel = null;
  [SerializeField] private TextMeshProUGUI JustDescriptionText = null;
  [Space(10)]
  [SerializeField] private GameObject DisComfortPanel = null;
  [SerializeField] private TextMeshProUGUI DiscomfortText = null;
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
    AllCanvasGroup.Add(TurnPreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(HPPreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(SanityPreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(GoldPreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(MapPreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(QuestPreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(SkillPreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(ExpPreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(TendencyPreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(SelectionNonePanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(SelectionPayPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(SelectionCheckPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(SelectionElsePanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(RewardStatusPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(RewardExpPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(RewardSkillPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(ExpSelectEmptyPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(ExpSelectExistPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(JustDescriptionPanel.GetComponent<CanvasGroup>());
  }
  private RectTransform CurrentPreview = null;

  public void SetRewardIcons(Transform _holder,List<RewardTarget> _rewards)
  {
    List<int> _rewardindex=new List<int>();
    foreach (var _target in _rewards) switch (_target)
      {
        case RewardTarget.Experience:_rewardindex.Add(5);break;
        case RewardTarget.HP: _rewardindex.Add(0);break;
        case RewardTarget.Sanity:_rewardindex.Add(1);break;
        case RewardTarget.Gold:_rewardindex.Add(2);break;
        case RewardTarget.Skill: _rewardindex.Add(4);break;
      }
    for(int i = 0; i < _holder.childCount; i++)
    {
      GameObject _chilcicon = _holder.GetChild(i).gameObject;
      if (_rewardindex.Contains(i)) _chilcicon.SetActive(true);
      else _chilcicon.SetActive(false);
    }
  }
  public void OpenTurnPreview()
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
    TurnIcon.sprite = _turnsprite;
      TurnDescription.text = _description;
      TurnSubDescription.text = GameManager.Instance.GetTextData("TURN_DESCRIPTION");
    CurrentPreview = TurnPreview.GetComponent<RectTransform>();
    IEnumerator _cor = null;
    _cor= fadepreview(TurnPreview, true);
    StartCoroutine(_cor);
  }//턴 미리보기 패널 세팅 후 열기
  public void OpenHPPreview()
  {
    StatusType _currenttype = StatusType.HP;

    string _description = "", _subdescription = "", _gendescription = "", _paydescription = "", _infodescription = "";
    int _genvalue = 0, _payvalue = 0;

    _genvalue = (int)GameManager.Instance.MyGameData.GetHPGenModify(false);
    _payvalue = (int)GameManager.Instance.MyGameData.GetHPLossModify(false);

    if (_genvalue > 0)
    {
      _gendescription = GameManager.Instance.GetTextData(_currenttype, 12) + " " + string.Format("{0}%", WNCText.PositiveColor("+"+_genvalue.ToString()));
      _infodescription += _gendescription;
    }
    if (_payvalue > 0)
    {
      if (_infodescription != "") _infodescription += "\n\n";
      _paydescription = GameManager.Instance.GetTextData(_currenttype, 15) + " " + string.Format("{0}%", WNCText.NegativeColor("+" + _payvalue.ToString()));
    }

    HPDescription.text = _description;
    if (_infodescription == "")
    {
      if (HPInfoText.gameObject.activeInHierarchy.Equals(false)) HPInfoText.gameObject.SetActive(true);
      HPInfoText.text = _infodescription;
    }
    else
    {
      if(HPInfoText.gameObject.activeInHierarchy.Equals(true))HPInfoText.gameObject.SetActive(false);
    }
    HpSubDescription.text = _subdescription;
    CurrentPreview = HPPreview.GetComponent<RectTransform>();
 
    IEnumerator _cor = null;
    _cor = fadepreview(HPPreview, true);
    StartCoroutine(_cor);
  }//체력 설명, 증감량 표기 후 열기
  public void OpenSanityPreview()
  {
    StatusType _currenttype = StatusType.Sanity;
    string _description = "", _subdescription = "", _gendescription = "", _paydescription = "", _infodescription = "";
    int _genvalue = 0, _payvalue = 0;

    _genvalue = (int)GameManager.Instance.MyGameData.GetSanityGenModify(false);
    _payvalue = (int)GameManager.Instance.MyGameData.GetSanityLossModify(false);

    if (_genvalue > 0)
    {
      _gendescription = GameManager.Instance.GetTextData(_currenttype, 12) + " " + string.Format("{0}%", WNCText.PositiveColor("+" + _genvalue.ToString()));
      _infodescription += _gendescription;
    }
    if (_payvalue > 0)
    {
      if (_infodescription != "") _infodescription += "\n\n";
      _paydescription = GameManager.Instance.GetTextData(_currenttype, 15) + " " + string.Format("{0}%", WNCText.NegativeColor("+" + _payvalue.ToString()));
    }

    SanityDescription.text = _description;
    if (_infodescription == "")
    {
      if (SanityInfoText.gameObject.activeInHierarchy.Equals(false)) SanityInfoText.gameObject.SetActive(true);
      SanityInfoText.text = _infodescription;
    }
    else
    {
      if (SanityInfoText.gameObject.activeInHierarchy.Equals(true)) SanityInfoText.gameObject.SetActive(false);
    }
    SanitySubDescription.text = _subdescription;
    CurrentPreview = SanityPreview.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(SanityPreview, true);
    StartCoroutine(_cor);
  }//정신력 설명,증감량 표기 후 열기
  public void OpenGoldPreview()
  {
    StatusType _currenttype = StatusType.Gold;
    string _description = "", _subdescription = "", _gendescription = "", _paydescription = "", _infodescription = "";
    int _genvalue = 0, _payvalue = 0;

    _genvalue = (int)GameManager.Instance.MyGameData.GetGoldGenModify(false);
    _payvalue = (int)GameManager.Instance.MyGameData.GetGoldPayModify(false);

    if (_genvalue > 0)
    {
      _gendescription = GameManager.Instance.GetTextData(_currenttype, 12) + " " + string.Format("{0}%", WNCText.PositiveColor("+" + _genvalue.ToString()));
      _infodescription += _gendescription;
    }
    if (_payvalue > 0)
    {
      if (_infodescription != "") _infodescription += "\n\n";
      _paydescription = GameManager.Instance.GetTextData(_currenttype, 15) + " " + string.Format("{0}%", WNCText.NegativeColor("+" + _payvalue.ToString()));
    }

    GoldDescription.text = _description;
    if (_infodescription == "")
    {
      if (GoldInfoText.gameObject.activeInHierarchy.Equals(false)) GoldInfoText.gameObject.SetActive(true);
      GoldInfoText.text = _infodescription;
    }
    else
    {
      if (GoldInfoText.gameObject.activeInHierarchy.Equals(true)) GoldInfoText.gameObject.SetActive(false);
    }
    GoldSubDescription.text = _subdescription;
    CurrentPreview = GoldPreview.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(GoldPreview, true);
    StartCoroutine(_cor);
  }//골드 설명,증감량 표기 후 열기
  public void OpenMapPreview()
  {
    //                                                                                                          퀘스트에 따라 다른 기능 구현 필요(기획 이후)
    CurrentPreview = MapPreview.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(MapPreview, true);
    StartCoroutine(_cor);
  }//현재 이동 가능 여부에 따라 텍스트만 출력
  public void OpenQuestPreview()
  {
  }//현재 퀘스트 이름, 일러스트, 다음 내용                             수정요망
  public void OpenSkillPreview(SkillType _skilltype)
  {
    Sprite _icon = GameManager.Instance.ImageHolder.GetSkillIcon(_skilltype);
    string _skillname = GameManager.Instance.GetTextData(_skilltype,0);
    SkillName.text = _skillname;

    int _level = GameManager.Instance.MyGameData.GetSkill(_skilltype).Level ;
    SkillLevel.text = _level.ToString();

    SkillSubDescription.text = GameManager.Instance.GetTextData(_skilltype,4);
    SkillIcon.sprite = _icon;

    CurrentPreview = SkillPreview.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(SkillPreview, true);
    StartCoroutine(_cor);
  }
  public void OpenExpPreview(Experience _exp)
  {
    ExpName.text =_exp.Name;
    ExpDuration.text = $"{_exp.Duration}";
    ExpEffect.text = _exp.ShortEffectString;
    ExpSubDescription.text = _exp.SubDescription;

    CurrentPreview = ExpPreview.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(ExpPreview, true);
    StartCoroutine(_cor);
  }
  public void OpenTendencyPreview(TendencyType _type)
  {
    Sprite _tendencyicon = null;
    Tendency _targettendency = null;
    switch (_type)
    {
      case TendencyType.Head:
        _tendencyicon = GameManager.Instance.ImageHolder.GetTendencyIcon(_type,GameManager.Instance.MyGameData.GetTendencyLevel(_type));
        _targettendency = GameManager.Instance.MyGameData.Tendency_Head;
        break;
      case TendencyType.Body:
        _tendencyicon = GameManager.Instance.ImageHolder.GetTendencyIcon(_type, GameManager.Instance.MyGameData.GetTendencyLevel(_type));
        _targettendency = GameManager.Instance.MyGameData.Tendency_Body;
        break;
    }
    string _tendencyeffect = GameManager.Instance.MyGameData.GetTendencyEffectString_short(_type);

    TendencyIcon.sprite = _tendencyicon;
    TendencyName.text = _targettendency.Name;
    TendencyDescription.text = _tendencyeffect;
    TendencySubDescription.text = _targettendency.SubDescription;

    CurrentPreview = TendencyPreview.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(TendencyPreview, true);
    StartCoroutine(_cor);
  }
  public void OpenSelectionNonePreview(SelectionData _selection,TendencyType tendencytype,bool dir)
  {
    SelectionNoneBackground.sprite = GameManager.Instance.ImageHolder.SelectionBackground(tendencytype, dir);

    SelectionNoneText.text = _selection.SubDescription;
    SetRewardIcons(NoneRewardIcons, _selection.SelectionSuccesRewards);

    CurrentPreview = SelectionNonePanel.GetComponent<RectTransform>();

    switch (tendencytype)
    {
      case TendencyType.None:
        if (SelectionNoneTendency.gameObject.activeInHierarchy.Equals(true)) SelectionNoneTendency.gameObject.SetActive(false);
        break;
      case TendencyType.Body:
        if (SelectionNoneTendency.gameObject.activeInHierarchy.Equals(false)) SelectionNoneTendency.gameObject.SetActive(true);
        SelectionNoneTendency.Setup(GameManager.Instance.MyGameData.Tendency_Body,dir);
        break;
      case TendencyType.Head:
        if (SelectionNoneTendency.gameObject.activeInHierarchy.Equals(false)) SelectionNoneTendency.gameObject.SetActive(true);
        SelectionNoneTendency.Setup(GameManager.Instance.MyGameData.Tendency_Head, dir);
        break;
    }

    IEnumerator _cor = null;
    _cor = fadepreview(SelectionNonePanel, true);
    StartCoroutine(_cor);
  }
  public void OpenSelectionPayPreview(SelectionData _selection, TendencyType tendencytype, bool dir)
  {
    SelectionPayBackground.sprite = GameManager.Instance.ImageHolder.SelectionBackground(tendencytype, dir);

    PaySubDescription.text = _selection.SubDescription;
    SetRewardIcons(PayRewardIcons, _selection.SelectionSuccesRewards);
    Sprite _icon = null;
    int _modifiedvalue = 0;
    int _modify = 0;
    string _payvaluetext="", _statusinfo = "";
    int _percent = -1;
    StatusType _status = StatusType.HP;
    switch (_selection.SelectionPayTarget)
    {
      case StatusType.HP:
        _status = StatusType.HP;
        _icon = GameManager.Instance.ImageHolder.HPDecreaseIcon;
        _modify = (int)GameManager.Instance.MyGameData.GetHPLossModify(false);
        _modifiedvalue = GameManager.Instance.MyGameData.PayHPValue_modified;
        _payvaluetext = string.Format(GameManager.Instance.GetTextData("PAYVALUE_TEXT"),WNCText.GetHPColor(_modifiedvalue.ToString()));
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
        _icon = GameManager.Instance.ImageHolder.SanityDecreaseIcon;
        _modify = (int)GameManager.Instance.MyGameData.GetSanityLossModify(false);
        _modifiedvalue = GameManager.Instance.MyGameData.PaySanityValue_modified;
        _payvaluetext = string.Format(GameManager.Instance.GetTextData("PAYVALUE_TEXT"),WNCText.GetSanityColor(_modifiedvalue.ToString()));
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
        _icon = GameManager.Instance.ImageHolder.GoldDecreaseIcon;
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

          PayNoGoldText.text = GameManager.Instance.GetTextData("NOGOLD_TEXT");
          PayNoGoldValue.text = string.Format(GameManager.Instance.GetTextData("NOGOLD_PAYVALUE"),
            string.Format("{0}<b>{1}</b>", GameManager.Instance.GetTextData(StatusType.Gold, 2),WNCText.GetGoldColor(GameManager.Instance.MyGameData.Gold)),
            string.Format("{0}<b>{1}</b>", GameManager.Instance.GetTextData(StatusType.Sanity, 2),WNCText.GetSanityColor(_sanitypayvalue.ToString())));
          PayNoGoldPercentText.text = GameManager.Instance.GetTextData("NOGOLD_PERCENTAGE_TEXT");
          PayNoGoldPercentValue.text = WNCText.PercentageColor(_percent);

          if (PayNoGoldHolder.activeInHierarchy.Equals(false)) PayNoGoldHolder.SetActive(true);
          if (PayRequireValue.gameObject.activeInHierarchy.Equals(true)) PayRequireValue.gameObject.SetActive(false);

        }//지불 골드 값이 보유 값에 비해 높을 때
        else
        {
          PayRequireValue.text = string.Format(GameManager.Instance.GetTextData("PAYVALUE_TEXT"),WNCText.GetGoldColor(_modifiedvalue));

          if(PayNoGoldHolder.activeInHierarchy.Equals(true))PayNoGoldHolder.SetActive(false);
          if (PayRequireValue.gameObject.activeInHierarchy.Equals(false)) PayRequireValue.gameObject.SetActive(true);

        }//골드 지불이 가능할 때
        break;//골드라면 지불,기본값,보정치,최종값을 받아오고 보정치가 존재한다면 텍스트에 삽입, 최종값이 보유값을 넘는다면 실패 확률 확인
    }

    PayIcon.sprite = _icon;
    PayRequireValue.text = _payvaluetext;

    CurrentPreview = SelectionPayPanel.GetComponent<RectTransform>();

    switch (tendencytype)//성향 존재하는거면 그거 활성화
    {
      case TendencyType.None:
        if (SelectionPayTendendcy.gameObject.activeInHierarchy.Equals(true)) SelectionPayTendendcy.gameObject.SetActive(false);
        break;
      case TendencyType.Body:
        if (SelectionPayTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionPayTendendcy.gameObject.SetActive(true);
        SelectionPayTendendcy.Setup(GameManager.Instance.MyGameData.Tendency_Body, dir);
        break;
      case TendencyType.Head:
        if (SelectionPayTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionPayTendendcy.gameObject.SetActive(true);
        SelectionPayTendendcy.Setup(GameManager.Instance.MyGameData.Tendency_Head, dir);
        break;
    }

    IEnumerator _cor = null;
    _cor = fadepreview(SelectionPayPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenSelectionCheckPreview_skill(SelectionData _selection, TendencyType tendencytype, bool dir)
  {
    SelectionCheckBackground.sprite = GameManager.Instance.ImageHolder.SelectionBackground(tendencytype, dir);

    Sprite[] _icons = new Sprite[2];
    Skill[] _skills= new Skill[2];
    int _requirelevel = 0, _currentlevel = 0, _percentage = 0;
    string _requiretext = "", _currenttext = "", _skillinfo = "", _percentage_text = "", _percentage_int = "", _subdescription = "";

    _subdescription=GameManager.Instance.GetTextData(_selection.SubDescription);
    _percentage_text = GameManager.Instance.GetTextData("SUCCESSPERCENT_TEXT");

    if (_selection.SelectionCheckSkill.Equals(SelectionTargetType.Check_Single))
    {
      _requirelevel = GameManager.Instance.MyGameData.CheckSkillSingleValue;

      _skills[0] = GameManager.Instance.MyGameData.GetSkill(_selection.SelectionCheckSkill[0]);
      _icons[0]=GameManager.Instance.ImageHolder.GetSkillIcon(_skills[0].MySkillType);
      _currentlevel = _skills[0].Level;
      _skillinfo += string.Format("{0}<b>{1}</b>", GameManager.Instance.GetTextData(_skills[0].MySkillType, 1), _skills[0].Level);


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
    _percentage_int = _percentage.ToString();
    _currenttext = string.Format(GameManager.Instance.GetTextData("CURRENTLEVEL_TEXT") + "\n{1}", _currentlevel, _skillinfo);

    SelectionCheckIcons[0].sprite = _icons[0];
    SelectionCheckIcons[1].sprite=_icons[1];
    SelectionCheckRequireLevel.text = _requiretext;
    SelectionCheckCurrentLevel.text= _currenttext;
    SelectionCheckPercent_text.text = _percentage_text;
    SelectionCheckPercent_int.text = _percentage_int;
    SelectionCheckDescription.text = _subdescription;

    switch (tendencytype)//성향 존재하는거면 그거 활성화
    {
      case TendencyType.None:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(true)) SelectionCheckTendendcy.gameObject.SetActive(false);
        break;
      case TendencyType.Body:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionCheckTendendcy.gameObject.SetActive(true);
        SelectionCheckTendendcy.Setup(GameManager.Instance.MyGameData.Tendency_Body, dir);
        break;
      case TendencyType.Head:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionCheckTendendcy.gameObject.SetActive(true);
        SelectionCheckTendendcy.Setup(GameManager.Instance.MyGameData.Tendency_Head, dir);
        break;
    }

    IEnumerator _cor = null;
    _cor = fadepreview(SelectionCheckPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenSelectionElsePreview(SelectionData _selection, TendencyType tendencytype, bool dir)
  {
    SelectionElseBackground.sprite = GameManager.Instance.ImageHolder.SelectionBackground(tendencytype, dir);

    SetRewardIcons(CheckRewardIcons, _selection.SelectionSuccesRewards);
    Sprite _icon = null;
    if (_selection.ThisSelectionType.Equals(SelectionTargetType.Exp)) _icon = GameManager.Instance.ImageHolder.ExpSelectionIcon;
    else _icon = GameManager.Instance.ImageHolder.TendencySelectionIcon;

    SelectionElseIcon.sprite = _icon;
    SelectionElseDescription.text = _selection.SubDescription;

    CurrentPreview=SelectionElsePanel.GetComponent<RectTransform>();

    switch (tendencytype)
    {
      case TendencyType.None:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(true)) SelectionCheckTendendcy.gameObject.SetActive(false);
        break;
      case TendencyType.Body:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionCheckTendendcy.gameObject.SetActive(true);
        SelectionElseTendency.Setup(GameManager.Instance.MyGameData.Tendency_Body, dir);
        break;
      case TendencyType.Head:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionCheckTendendcy.gameObject.SetActive(true);
        SelectionElseTendency.Setup(GameManager.Instance.MyGameData.Tendency_Head, dir);
        break;
    }

    IEnumerator _cor = null;
    _cor = fadepreview(SelectionElsePanel, true);
    StartCoroutine(_cor);
  }
  //보상 설명 : 체력,정신력,돈 설명?
  public void OpenRewardStatusPreview(StatusType status, int _value)
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

    CurrentPreview = RewardStatusPanel.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(RewardStatusPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenRewardExpPreview(Experience _exp)
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


    CurrentPreview = RewardExpPanel.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(RewardExpPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenRewardSkillPreview(SkillType skilltype)
  {
    string _name = $"{GameManager.Instance.GetTextData(skilltype,0)} +1";
    Sprite _icon = GameManager.Instance.ImageHolder.GetSkillIcon(skilltype);

    RewardSkillIcon.sprite = _icon;
    RewardSkillName.text = _name;
    RewardSkillClickText.text= GameManager.Instance.GetTextData("CLICKTOGET_TEXT");

    CurrentPreview =RewardSkillPanel.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(RewardSkillPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenExpSelectionEmptyPreview(Experience _exp,bool islong)
  {
    string _name, _turn, _description;

    _name = _exp.Name;
    _turn=islong?ConstValues.LongTermStartTurn.ToString():ConstValues.ShortTermStartTurn.ToString();
    if (islong)
    {
      _description =GameManager.Instance.GetTextData("LONGTERMSAVE_NAME")+ string.Format(GameManager.Instance.GetTextData("LONGTERMSAVE_DESCRIPTION"),ConstValues.LongTermStartTurn, ConstValues.LongTermChangeCost);
    }
    else
    {
      _description = GameManager.Instance.GetTextData("LONGTERMSAVE_NAME") + string.Format(GameManager.Instance.GetTextData("SHORTTERMSAVE_DESCRIPTION"), ConstValues.ShortTermStartTurn);
    }

    ExpSelectEmptyTurn.text = _turn.ToString();
    ExpSelectEmptyDescription.text = _description;
    CurrentPreview=ExpSelectEmptyPanel.GetComponent<RectTransform>();
    ExpSelectClickText.text= GameManager.Instance.GetTextData("CLICKTOGET_TEXT");
    if (ExpSelectClickText.gameObject.activeInHierarchy.Equals(false)) ExpSelectClickText.gameObject.SetActive(true);

    IEnumerator _cor = null;
    _cor = fadepreview(ExpSelectEmptyPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenExpSelectionExistPreview(Experience _origin,Experience _new,bool islong)
  {
    int _turn = islong ? ConstValues.LongTermStartTurn : ConstValues.ShortTermStartTurn;
    string _description = "";
    if (islong)
    {
      _description = GameManager.Instance.GetTextData("LONGTERMSAVE_NAME") + string.Format(GameManager.Instance.GetTextData("LONGTERMSAVE_DESCRIPTION"), ConstValues.LongTermStartTurn, ConstValues.LongTermChangeCost);
    }
    else
    {
      _description = GameManager.Instance.GetTextData("LONGTERMSAVE_NAME") + string.Format(GameManager.Instance.GetTextData("SHORTTERMSAVE_DESCRIPTION"), ConstValues.ShortTermStartTurn);
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

    IEnumerator _cor = null;
    _cor = fadepreview(ExpSelectExistPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenJustDescriptionPreview(string text)
  {
    JustDescriptionText.text = text;

    CurrentPreview = JustDescriptionPanel.GetComponent<RectTransform>();
    if (ExpSelectClickText.gameObject.activeInHierarchy.Equals(true)) ExpSelectClickText.gameObject.SetActive(false);

    IEnumerator _cor = null;
    _cor = fadepreview(JustDescriptionPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenDisComfortPanel()
  {
     DiscomfortText.text = GameManager.Instance.GetTextData("CANNOTCHANGEMADNESS_NAME");

    CurrentPreview = DisComfortPanel.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(DisComfortPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenEnvirPanel(EnvironmentType envir)
  {
    EnvirIcon.sprite = GameManager.Instance.ImageHolder.GetEnvirTile(envir);
    EnvirName.text = GameManager.Instance.GetTextData(envir, 0);
    EnvirDescription.text = GameManager.Instance.GetTextData(envir,1);

    CurrentPreview = EnvirPanel.GetComponent<RectTransform>();
    IEnumerator _cor = null;
    _cor = fadepreview(EnvirPanel, true);
    StartCoroutine(_cor);
  }
  private Vector2 Newpos = Vector2.zero;
  public void Update()
  {
    if (CurrentPreview == null) return;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(WholeRect, Input.mousePosition, MainCamera, out Newpos);
    CurrentPreview.localPosition = Newpos;
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

  private IEnumerator fadepreview(GameObject _targetobj, bool _isopen)
  {
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
