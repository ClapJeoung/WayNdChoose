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
  [SerializeField] private TextMeshProUGUI HPGenDescriptoin = null;
  [SerializeField] private TextMeshProUGUI HPDecreaseDescription = null;
  [SerializeField] private TextMeshProUGUI HpSubDescription = null;
  [Space(10)]
  [Space(10)]
  [SerializeField] private GameObject SanityPreview = null;
  [SerializeField] private TextMeshProUGUI SanityDescription = null;
  [SerializeField] private TextMeshProUGUI SanityGenDescriptoin = null;
  [SerializeField] private TextMeshProUGUI SanityDecreaseDescription = null;
  [SerializeField] private TextMeshProUGUI SanitySubDescription = null;
  [Space(10)]
  [SerializeField] private GameObject GoldPreview = null;
  [SerializeField] private TextMeshProUGUI GoldDescription = null;
  [SerializeField] private TextMeshProUGUI GoldGenDescriptoin = null;
  [SerializeField] private TextMeshProUGUI GoldDecreaseDescription = null;
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
  [SerializeField] private GameObject ThemePreview = null;
  [SerializeField] private TextMeshProUGUI ThemeName = null;
  [SerializeField] private TextMeshProUGUI ThemeLevel = null;
  [SerializeField] private Image ThemeIcon = null;
  [SerializeField] private TextMeshProUGUI ThemeLevelBySkill = null;
  [SerializeField] private TextMeshProUGUI ThemeLevelByExp = null;
  [SerializeField] private TextMeshProUGUI ThemeLevelByTendency = null;
  [SerializeField] private TextMeshProUGUI ThemeSubDescription = null;
  [Space(10)]
  [SerializeField] private GameObject SkillPreview = null;
  [SerializeField] private Image SkillIcon_A=null,SkillIcon_B=null;
  [SerializeField] private TextMeshProUGUI SkillName = null;
  [SerializeField] private TextMeshProUGUI SkillLevel = null;
  [SerializeField] private TextMeshProUGUI SkillLevelBySkill = null;
  [SerializeField] private TextMeshProUGUI SkillLevelByExp = null;
  [SerializeField] private TextMeshProUGUI SkillLevelByPlace = null;
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
  [SerializeField] private TextMeshProUGUI TendencyLevel = null;
  [SerializeField] private TextMeshProUGUI TendencyDescription = null;
  [SerializeField] private TextMeshProUGUI TendencySubDescription = null;
  [Space(10)]
  [SerializeField] private GameObject SelectionNonePanel = null;
  [SerializeField] private TextMeshProUGUI SelectionNoneText = null;
  [SerializeField] private Transform NoneRewardIcons = null;
  [Space(10)]
  [SerializeField] private GameObject SelectionPayPanel = null;
  [SerializeField] private Image PayIcon = null;
  [SerializeField] private TextMeshProUGUI PayTargetAmount = null;
  [SerializeField] private TextMeshProUGUI PayTargetDescription = null;
  [SerializeField] private TextMeshProUGUI PaySuccessPercent = null;
  [SerializeField] private TextMeshProUGUI PayDescription = null;
  [SerializeField] private Transform PayRewardIcons = null;
  [Space(10)]
  [SerializeField] private GameObject SelectionCheckPanel = null;
  [SerializeField] private TextMeshProUGUI SelectionCheckName = null;
  [SerializeField] private Image CheckIcon_A = null;
  [SerializeField] private Image CheckIcon_B = null;
  [SerializeField] private TextMeshProUGUI CheckTargetLevel = null;
  [SerializeField] private TextMeshProUGUI CheckLevelByOtherSkills = null;
  [SerializeField] private TextMeshProUGUI CheckPercent = null;
  [SerializeField] private Transform CheckRewardIcons = null;
  [SerializeField] private TextMeshProUGUI CheckDescription = null;
  [Space(10)]
  [SerializeField] private GameObject SelectionElsePanel = null;
  [SerializeField] private Image SelectionElseIcon = null;
  [SerializeField] private TextMeshProUGUI SelectionElseDescription = null;
  [Space(10)]
  [SerializeField] private GameObject RewardStatusPanel = null;
  [SerializeField] private Image RewardStatusIcon = null;
  [SerializeField] private TextMeshProUGUI RewardStatusAmount = null;
  [SerializeField] private TextMeshProUGUI RewardStatusDescription = null;
  [Space(10)]
  [SerializeField] private GameObject RewardTEPanel = null;
  [SerializeField] private TextMeshProUGUI RewardTEName = null;
  [SerializeField] private Image RewardTEIllust = null;
  [SerializeField] private TextMeshProUGUI RewardTEEffect = null;
  [Space(10)]
  [SerializeField] private GameObject RewardTSPanel = null;
  [SerializeField] private TextMeshProUGUI RewardTSName = null;
  [SerializeField] private Image RewardTSIcon_A = null, RewardTSIcon_B = null;
  [Space(10)]
  [SerializeField] private GameObject SkillSelectPanel = null;
  [SerializeField] private TextMeshProUGUI SkillSelectName = null;
  [SerializeField] private Image SkillSelectIcon_A=null,SkillSelectIon_B=null;
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
  [Space(10)]
  [SerializeField] private GameObject ExpSelectionBadPanel = null;
  [SerializeField] private TextMeshProUGUI ExpSelectionBadText = null;
  private List<CanvasGroup> AllCanvasGroup = new List<CanvasGroup>();
  private void Awake()
  {
    AllCanvasGroup.Add(TurnPreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(HPPreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(SanityPreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(GoldPreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(MapPreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(QuestPreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(ThemePreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(SkillPreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(ExpPreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(TendencyPreview.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(SelectionNonePanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(SelectionPayPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(SelectionCheckPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(SelectionElsePanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(RewardStatusPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(RewardTEPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(RewardTSPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(SkillSelectPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(ExpSelectEmptyPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(ExpSelectExistPanel.GetComponent<CanvasGroup>());
    AllCanvasGroup.Add(ExpSelectionBadPanel.GetComponent<CanvasGroup>());
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
        case RewardTarget.Theme: case RewardTarget.Skill:_rewardindex.Add(4);break;
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
      TextData _textdata = new TextData();
      Sprite _turnsprite = null;
      switch (_currentturn)
      {
        case 0: _turnsprite = GameManager.Instance.ImageHolder.SpringIcon_active; _textdata = GameManager.Instance.GetTextData("spring"); break;
        case 1: _turnsprite = GameManager.Instance.ImageHolder.SummerIcon_active; _textdata = GameManager.Instance.GetTextData("summer"); break;
        case 2: _turnsprite = GameManager.Instance.ImageHolder.FallIcon_active; _textdata = GameManager.Instance.GetTextData("fall"); break;
        case 3: _turnsprite = GameManager.Instance.ImageHolder.WinterIcon_active; _textdata = GameManager.Instance.GetTextData("winter"); break;
      }
      TurnIcon.sprite = _turnsprite;
      TurnDescription.text = _textdata.Description;
      TurnSubDescription.text = GameManager.Instance.GetTextData("turndescription").Name;
    CurrentPreview = TurnPreview.GetComponent<RectTransform>();
    IEnumerator _cor = null;
    _cor= fadepreview(TurnPreview, true);
    StartCoroutine(_cor);
  }//턴 미리보기 패널 세팅 후 열기
  public void OpenHPPreview()
  {
      TextData _textddata = GameManager.Instance.GetTextData("hp");
      string _str = _textddata.Description;
      HPDescription.text = _str;
      int _genvalue = ((int)GameManager.Instance.MyGameData.GetHPGenModify(false));
    if (_genvalue.Equals(0)) _str = $"{GameManager.Instance.GetTextData("hpincrease").Name} {GameManager.Instance.GetTextData("nochange").Name}";
    else
    {
      if (_genvalue > 0) _str = $"{GameManager.Instance.GetTextData("hpincrease").SuccessDescription} {ColorText.PositiveColor(_genvalue.ToString())}%";
      else _str = $"{GameManager.Instance.GetTextData("hpincrease").FailDescription} {ColorText.NegativeColor(Mathf.Abs(_genvalue).ToString())}";
    }
    HPGenDescriptoin.text = _str;
      int _lossvalue = ((int)GameManager.Instance.MyGameData.GetHPLossModify(false));
    if (_lossvalue.Equals(0)) _str = $"{GameManager.Instance.GetTextData("hpdecrease").Name} {GameManager.Instance.GetTextData("nochange").Name}";
    else
    {
      if (_lossvalue > 0) _str = $"{GameManager.Instance.GetTextData("hpdecrease").FailDescription} {ColorText.NegativeColor(_lossvalue.ToString())}%";
      else _str = $"{GameManager.Instance.GetTextData("hpdecrease").SuccessDescription} {ColorText.PositiveColor(Mathf.Abs(_lossvalue).ToString())}";
    }
    HPDecreaseDescription.text = _str;
      HpSubDescription.text = _textddata.SelectionSubDescription;
    CurrentPreview = HPPreview.GetComponent<RectTransform>();
 
    IEnumerator _cor = null;
    _cor = fadepreview(HPPreview, true);
    StartCoroutine(_cor);
  }//체력 설명, 증감량 표기 후 열기
  public void OpenSanityPreview()
  {
      TextData _textddata = GameManager.Instance.GetTextData("sanity");
      string _str = _textddata.Description;
      SanityDescription.text = _str;
      int _genvalue = ((int)GameManager.Instance.MyGameData.GetSanityGenModify(false));
    if (_genvalue.Equals(0)) _str = $"{GameManager.Instance.GetTextData("sanityincrease").Name} {GameManager.Instance.GetTextData("nochange").Name}";
    else
    {
      if (_genvalue > 0) _str = $"{GameManager.Instance.GetTextData("sanityincrease").SuccessDescription} {ColorText.PositiveColor(_genvalue.ToString())}%";
      else _str = $"{GameManager.Instance.GetTextData("sanityincrease").FailDescription} {ColorText.NegativeColor(Mathf.Abs(_genvalue).ToString())}";
    }
    SanityGenDescriptoin.text = _str;
      int _lossvalue = ((int)GameManager.Instance.MyGameData.GetSanityLossModify(false));
    if (_lossvalue.Equals(0)) _str = $"{GameManager.Instance.GetTextData("sanitydecrease").Name} {GameManager.Instance.GetTextData("nochange").Name}";
    else
    {
      if (_lossvalue > 0) _str = $"{GameManager.Instance.GetTextData("sanitydecrease").FailDescription} {ColorText.NegativeColor(_lossvalue.ToString())}%";
      else _str = $"{GameManager.Instance.GetTextData("sanitydecrease").SuccessDescription} {ColorText.PositiveColor(Mathf.Abs(_lossvalue).ToString())}";
    }
    SanityDecreaseDescription.text = _str;
      SanitySubDescription.text = _textddata.SelectionSubDescription;
    CurrentPreview = SanityPreview.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(SanityPreview, true);
    StartCoroutine(_cor);
  }//정신력 설명,증감량 표기 후 열기
  public void OpenGoldPreview()
  {
    TextData _textddata = GameManager.Instance.GetTextData("gold");
    string _str = _textddata.Description;
    GoldDescription.text = _str;
    int _genvalue = ((int)GameManager.Instance.MyGameData.GetGoldGenModify(false));
    if (_genvalue.Equals(0)) _str = $"{GameManager.Instance.GetTextData("goldincrease").Name} {GameManager.Instance.GetTextData("nochange").Name}";
    else
    {
      if (_genvalue > 0) _str = $"{GameManager.Instance.GetTextData("goldincrease").SuccessDescription} {ColorText.PositiveColor(_genvalue.ToString())}%";
      else _str = $"{GameManager.Instance.GetTextData("goldincrease").FailDescription} {ColorText.NegativeColor(Mathf.Abs(_genvalue).ToString())}";
    }
    GoldGenDescriptoin.text = _str;
    int _lossvalue = ((int)GameManager.Instance.MyGameData.GetGoldPayModify(false));
    if (_lossvalue.Equals(0)) _str = $"{GameManager.Instance.GetTextData("golddecrease").Name} {GameManager.Instance.GetTextData("nochange").Name}";
    else
    {
      if (_lossvalue > 0) _str = $"{GameManager.Instance.GetTextData("golddecrease").FailDescription} {ColorText.NegativeColor(_lossvalue.ToString())}%";
      else _str = $"{GameManager.Instance.GetTextData("golddecrease").SuccessDescription} {ColorText.PositiveColor(Mathf.Abs(_lossvalue).ToString())}";
    }
    GoldDecreaseDescription.text = _str;
    GoldSubDescription.text = _textddata.SelectionSubDescription;

    CurrentPreview = GoldPreview.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(GoldPreview, true);
    StartCoroutine(_cor);
  }//골드 설명,증감량 표기 후 열기
  public void OpenMapPreview()
  {
    TextData _textdata = GameManager.Instance.GetTextData("movedescription");
    if (GameManager.Instance.MyGameData.CanMove) MapMovableDescription.text = _textdata.SuccessDescription;
    else MapMovableDescription.text = _textdata.FailDescription;
    MapSubDescription.text = _textdata.SelectionSubDescription;

    CurrentPreview = MapPreview.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(MapPreview, true);
    StartCoroutine(_cor);
  }//현재 이동 가능 여부에 따라 텍스트만 출력
  public void OpenQuestPreview()
  {
    if (GameManager.Instance.MyGameData.CurrentQuest == null)
    {
      TextData _textdata = GameManager.Instance.GetTextData("donthaveanyquest");
      QuestName.text = _textdata.Name;
      QuestIllust.sprite = GameManager.Instance.ImageHolder.NothingQuestIllust;
      NextQuestEventDescription.text = _textdata.Description;
    }
    else
    {
      QuestHolder _currentquest = GameManager.Instance.MyGameData.CurrentQuest;
      QuestName.text = _currentquest.QuestName;
      QuestIllust.sprite = _currentquest.Illust;
      string _strid = $"{_currentquest.QuestID}_";
      switch (_currentquest.CurrentSequence)
      {
        case QuestSequence.Start:
        case QuestSequence.Rising:
          _strid += "rising";
          break;//퀘스트id_rising을 부 설명으로
        case QuestSequence.Climax:
          _strid += $"climax_{_currentquest.FinishedClimaxCount.ToString()}";
          break;//id_climax_순서 를 부 설명으로
        case QuestSequence.Falling:
          _strid += "falling";
          break;//id_falling을 부 설명으로
      }//퀘스트가 존재할경우
      NextQuestEventDescription.text = GameManager.Instance.GetTextData(_strid).Name;
    }
    QuestSubDescription.text = GameManager.Instance.GetTextData("quest").SelectionSubDescription;

    CurrentPreview = QuestPreview.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(QuestPreview, true);
    StartCoroutine(_cor);
  }//현재 퀘스트 이름, 일러스트, 다음 내용
  public void OpenThemePreview(ThemeType _theme)
  {
    TextData _themename = null;
    Sprite _icon = GameManager.Instance.ImageHolder.GetThemeIcon(_theme);
    _themename = GameManager.Instance.GetTextData(_theme);
    ThemeName.text = _themename.Name;
    int _onlyskill = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_theme);
    //기술로부터 나온 값
    int _onlyexp = GameManager.Instance.MyGameData.GetEffectThemeCount_Exp(_theme);
    //경험에서 나온 값
    int _onlytendency = GameManager.Instance.MyGameData.GetThemeLevelByTendency(_theme);
    int _sum = _onlyskill  + _onlyexp + _onlytendency;
    ThemeLevel.text = _sum.ToString();
    string _valuestr = "";
    string _str = "";

    makecolorvalue(_onlyskill);
    _str= GameManager.Instance.GetTextData("byskill").Name.Replace("#VALUE#", _valuestr);
    ThemeLevelBySkill.text = _str;

    makecolorvalue(_onlyexp);
    _str = GameManager.Instance.GetTextData("byexp").Name.Replace("#VALUE#", _valuestr);
    ThemeLevelByExp.text = _str;

    makecolorvalue(_onlytendency);
    _str=GameManager.Instance.GetTextData("bytendency").Name.Replace("#VALUE#",_valuestr);
    ThemeLevelByTendency.text= _str;

    ThemeSubDescription.text = _themename.SelectionSubDescription;
    ThemeIcon.sprite = _icon;

    void makecolorvalue(int value)
    {
      if (value.Equals(0)) _valuestr = value.ToString();
      else if (value > 0) _valuestr = ColorText.PositiveColor(value.ToString());
      else _valuestr=ColorText.NegativeColor(value.ToString());
    }
    CurrentPreview = ThemePreview.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(ThemePreview, true);
    StartCoroutine(_cor);
  }
  public void OpenSkillPreview(SkillName _skill)
  {
    TextData _skilltextdata=GameManager.Instance.GetTextData(_skill);
    string _name=_skilltextdata.Name;
    string _subdescription = _skilltextdata.SelectionSubDescription;
    int _level = GameManager.Instance.MyGameData.Skills[_skill].LevelForPreviewOrTheme;
    Sprite[] _icons = new Sprite[2];
    GameManager.Instance.ImageHolder.GetSkillIcons(_skill, ref _icons);
    Skill _targetskill = GameManager.Instance.MyGameData.Skills[_skill];
    string _valuestr = "";
    string _str = "";
    int _onlylevel = _targetskill.LevelByOwn;
    int _onlyexp = _targetskill.LevelByExp;
    int _onlyplace = _targetskill.LevelByPlace;

    makecolorvalue(_onlylevel);
    _str = GameManager.Instance.GetTextData("byownlevel").Name.Replace("#VALUE#", _valuestr);
    SkillLevelBySkill.text = _str;

    makecolorvalue(_onlyexp);
    _str = GameManager.Instance.GetTextData("byexp").Name.Replace("#VALUE#", _valuestr);
    SkillLevelByExp.text = _str;

    makecolorvalue(_onlyplace);
    _str = GameManager.Instance.GetTextData("byplace").Name.Replace("#VALUE#", _valuestr);
    SkillLevelByPlace.text = _str;


    SkillIcon_A.sprite= _icons[0];SkillIcon_B.sprite = _icons[1];
    SkillName.text = _name;
    SkillLevel.text = _level.ToString();
    SkillSubDescription.text = _subdescription;

    CurrentPreview=SkillPreview.GetComponent<RectTransform>();

    void makecolorvalue(int value)
    {
      if (value.Equals(0)) _valuestr = value.ToString();
      else if (value > 0) _valuestr = ColorText.PositiveColor(value.ToString());
      else _valuestr = ColorText.NegativeColor(value.ToString());
    }
    IEnumerator _cor = null;
    _cor = fadepreview(SkillPreview, true);
    StartCoroutine(_cor);
  }
  public void OpenExpPreview(Experience _exp)
  {
    ExpName.text =_exp.Name;
    ExpDuration.text = $"{_exp.Duration}";
    ExpEffect.text = _exp.ShortEffectString;
    ExpSubDescription.text = GameManager.Instance.GetTextData(_exp.ID).SelectionSubDescription;

    CurrentPreview = ExpPreview.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(ExpPreview, true);
    StartCoroutine(_cor);
  }
  public void OpenTendencyPreview(TendencyType _type)
  {
    Sprite _tendencyicon = null;
    string _tendencyname = "";
    TextData _textdata = new TextData();
    int _tendencylevel = 0;
    switch (_type)
    {
      case TendencyType.Rational:
        _tendencyicon = GameManager.Instance.ImageHolder.GetTendencyIcon(_type,GameManager.Instance.MyGameData.GetTendencyLevel(_type));
        _textdata = GameManager.Instance.GetTextData("rational");
        _tendencylevel = GameManager.Instance.MyGameData.Tendency_RP.Level * -1;
        break;
      case TendencyType.Physical:
        _tendencyicon = GameManager.Instance.ImageHolder.GetTendencyIcon(_type, GameManager.Instance.MyGameData.GetTendencyLevel(_type));
        _textdata = GameManager.Instance.GetTextData("physical");
        _tendencylevel = GameManager.Instance.MyGameData.Tendency_RP.Level * +1;
        break;
      case TendencyType.Mental:
        _tendencyicon = GameManager.Instance.ImageHolder.GetTendencyIcon(_type, GameManager.Instance.MyGameData.GetTendencyLevel(_type));
        _textdata = GameManager.Instance.GetTextData("mental");
        _tendencylevel = GameManager.Instance.MyGameData.Tendency_MM.Level * -1;
        break;
      case TendencyType.Material:
        _tendencyicon = GameManager.Instance.ImageHolder.GetTendencyIcon(_type, GameManager.Instance.MyGameData.GetTendencyLevel(_type));
        _textdata = GameManager.Instance.GetTextData("material");
        _tendencylevel = GameManager.Instance.MyGameData.Tendency_MM.Level * +1;
        break;
    }
    string _tendencyeffect = GameManager.Instance.MyGameData.GetTendencyEffectString(_type);

    _tendencyname = _textdata.Name;
    TendencyIcon.sprite = _tendencyicon;
    TendencyName.text = _tendencyname;
    TendencyLevel.text = _tendencylevel.ToString();
    TendencyDescription.text = _tendencyeffect;
    TendencySubDescription.text = _textdata.SelectionSubDescription;

    CurrentPreview = TendencyPreview.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(TendencyPreview, true);
    StartCoroutine(_cor);
  }
  public void OpenSelectionNonePreview(SelectionData _selection)
  {
    SelectionNoneText.text = _selection.SubDescription;
    SetRewardIcons(NoneRewardIcons, _selection.SelectionSuccesRewards);

    CurrentPreview = SelectionNonePanel.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(SelectionNonePanel, true);
    StartCoroutine(_cor);
  }
  public void OpenSelectionPayPreview(SelectionData _selection)
  {
    PayDescription.text = _selection.SubDescription;
    SetRewardIcons(PayRewardIcons, _selection.SelectionSuccesRewards);
    Sprite _icon = null;
    int _current = 0;
    int _target = 0;
    int _target_origin = 0;
    float _modify = 0;
    string _targetdescription = "";
    int _percent = -1;
    Color _descriptioncolor= Color.white;
    switch (_selection.SelectionPayTarget)
    {
      case PayOrLossTarget.HP:_icon = GameManager.Instance.ImageHolder.HPDecreaseIcon;_current = GameManager.Instance.MyGameData.HP;
        _current = GameManager.Instance.MyGameData.HP;
        _target_origin = GameManager.Instance.MyGameData.PayHPValue_origin;
        _modify = (int)GameManager.Instance.MyGameData.GetHPLossModify(false);
        _target = GameManager.Instance.MyGameData.PayHPValue_modified;
        if (_modify.Equals(0)) _targetdescription = "";
        else if (_modify > 0)
        {
                    _targetdescription = $"{GameManager.Instance.GetTextData("hp").Name} {GameManager.Instance.MyGameData.PayHPValue_origin}\n";
          _targetdescription += $"{GameManager.Instance.GetTextData("hp").FailDescription} {_modify}%";
          _descriptioncolor = NegativeColor;
        }//보정치가 0 이상이라면 부정적인것
        else
        {
                    _targetdescription = $"{GameManager.Instance.GetTextData("hp").Name} {GameManager.Instance.MyGameData.PayHPValue_origin}\n";
                    _targetdescription += $"{GameManager.Instance.GetTextData("hp").FailDescription} {_modify}%";
          _descriptioncolor = PositiveColor;
        }//보정치가 0 이하라면 긍정적인것
        break;//체력이라면 지불 기본값, 보정치, 최종값을 받아오고 보정치가 존재한다면 텍스트에 삽입
      case PayOrLossTarget.Sanity:_icon = GameManager.Instance.ImageHolder.SanityDecreaseIcon; _current = GameManager.Instance.MyGameData.CurrentSanity;
        _current = GameManager.Instance.MyGameData.CurrentSanity;
        _target_origin = GameManager.Instance.MyGameData.PaySanityValue_origin;
        _modify = (int)GameManager.Instance.MyGameData.GetSanityLossModify(false);
        _target = GameManager.Instance.MyGameData.PaySanityValue_modified;
        if (_modify.Equals(0)) _targetdescription = "";
        else if (_modify > 0)
        {
                    _targetdescription = $"{GameManager.Instance.GetTextData("sanity").Name} {GameManager.Instance.MyGameData.PaySanityValue_origin}\n";
                    _targetdescription += $"{GameManager.Instance.GetTextData("sanitydecrease").Name} {_modify}%";
          _descriptioncolor = NegativeColor;
        }//보정치가 0 이상이라면 부정적인것
        else
        {
                    _targetdescription = $"{GameManager.Instance.GetTextData("sanity").Name} {GameManager.Instance.MyGameData.PaySanityValue_origin}\n";
                    _targetdescription += $"{GameManager.Instance.GetTextData("sanitydecrease").Name} {_modify}%";
          _descriptioncolor = PositiveColor;
        }//보정치가 0 이하라면 긍정적인것
        break;//정신력이라면 지불 기본값,보정치,최종값을 받아오고 보정치가 존재한다면 텍스트에 삽입
      case PayOrLossTarget.Gold:_icon = GameManager.Instance.ImageHolder.GoldDecreaseIcon; _current = GameManager.Instance.MyGameData.Gold;
        _current = GameManager.Instance.MyGameData.Gold;
        _target_origin = GameManager.Instance.MyGameData.PayGoldValue_origin;
        _modify = (int)GameManager.Instance.MyGameData.GetGoldPayModify(false);
        _target = GameManager.Instance.MyGameData.PayGoldValue_modified;
        if (_modify.Equals(0)) _targetdescription = "";
        else if (_modify > 0)
        {
                    _targetdescription = $"{GameManager.Instance.GetTextData("gold").Name} {GameManager.Instance.MyGameData.PayGoldValue_origin}\n";
                    _targetdescription += $"{GameManager.Instance.GetTextData("gold").FailDescription} {_modify}%";
          _descriptioncolor = NegativeColor;
        }//보정치가 0 이상이라면 부정적인것
        else
        {
                    _targetdescription = $"{GameManager.Instance.GetTextData("gold").Name} {GameManager.Instance.MyGameData.PayGoldValue_origin}\n";
                    _targetdescription += $"{GameManager.Instance.GetTextData("gold").FailDescription} {_modify}%";
          _descriptioncolor = PositiveColor;
        }//보정치가 0 이하라면 긍정적인것
        if (_target > GameManager.Instance.MyGameData.Gold) _percent = GameManager.Instance.MyGameData.CheckPercent_money(_target);
        break;//골드라면 지불,기본값,보정치,최종값을 받아오고 보정치가 존재한다면 텍스트에 삽입, 최종값이 보유값을 넘는다면 실패 확률 확인
    }
    
    PayIcon.sprite = _icon;
    PayTargetAmount.text = (-_target).ToString();
    PayTargetDescription.color= _descriptioncolor;
    PayTargetDescription.text = _targetdescription;
    if(!_percent.Equals(-1))PaySuccessPercent.text = _percent.ToString()+"%";

    CurrentPreview = SelectionPayPanel.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(SelectionPayPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenSelectionCheckPreview_theme(SelectionData _selection)
  {
    CheckLevelByOtherSkills.text = "";
    CheckDescription.text = _selection.SubDescription;
    SetRewardIcons(CheckRewardIcons, _selection.SelectionSuccesRewards);
    Sprite _icon = null;
    int _currentlevel = 0;
    int  _byskill = 0, _byexp = 0, _bytendency = 0;
    int _targetlevel = GameManager.Instance.MyGameData.CheckThemeValue;
    int _percent = -1;
    string _name = "";
    ThemeType _themetype = _selection.SelectionCheckTheme;

    _icon = GameManager.Instance.ImageHolder.GetThemeIcon(_themetype);
    _byskill=GameManager.Instance.MyGameData.GetThemeLevelBySkill(_themetype);
    _byexp=GameManager.Instance.MyGameData.GetEffectThemeCount_Exp(_themetype);
    _bytendency=GameManager.Instance.MyGameData.GetThemeLevelByTendency(_themetype);

    string _themeid = "by";
    switch (_themetype)
    {
      case ThemeType.Conversation:_name= "conversation";break;
      case ThemeType.Force:_name= "force";break;
      case ThemeType.Wild:_name= "wild";break;
      case ThemeType.Intelligence:_name= "intelligence";break;
    }
    _themeid += _name;
    _currentlevel = GameManager.Instance.MyGameData.GetThemeLevel(_themetype);
    _percent = GameManager.Instance.MyGameData.CheckPercent_themeorskill(_currentlevel, _targetlevel);

    SelectionCheckName.text = GameManager.Instance.GetTextData(_themetype).Name;
    CheckIcon_A.sprite = _icon;
    CheckIcon_B.transform.parent.gameObject.SetActive(false);
    CheckTargetLevel.text = $"{_currentlevel} / {_targetlevel}";
    if(_percent!=-1) CheckPercent.text = _percent.ToString() + "%";

    CurrentPreview = SelectionCheckPanel.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(SelectionCheckPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenSelectionCheckPreview_skill(SelectionData _selection)
  {
    CheckDescription.text = _selection.SubDescription;
    SetRewardIcons(CheckRewardIcons, _selection.SelectionSuccesRewards);
    Sprite[] _icons = new Sprite[2];
    int _currentlevel = 0;
    int _byotherskills = 0;
    int _targetlevel = GameManager.Instance.MyGameData.CheckSkillValue;
    int _percent = 0;
    string _name = "";
    Skill _skill = GameManager.Instance.MyGameData.Skills[_selection.SelectionCheckSkill];
    GameManager.Instance.ImageHolder.GetSkillIcons(_skill.SkillType, ref _icons);
    _name = GameManager.Instance.GetTextData(_skill.SkillType).Name;

    _currentlevel = _skill.LevelForSkillCheck;
    _byotherskills = _skill.LevelByTheme;
    string _otherskilllevel = "";
    if (_byotherskills<=0) _otherskilllevel = "";
    else _otherskilllevel = GameManager.Instance.GetTextData("byotherskills").Name.Replace("#VALUE#", ColorText.PositiveColor(_otherskilllevel.ToString()));
    
    _percent = GameManager.Instance.MyGameData.CheckPercent_themeorskill(_currentlevel, _targetlevel);

    SelectionCheckName.text = _name;
    CheckIcon_A.sprite = _icons[0];
    if (CheckIcon_B.transform.parent.gameObject.activeSelf.Equals(false)) CheckIcon_B.transform.parent.gameObject.SetActive(true);
    CheckIcon_B.sprite = _icons[1];
    CheckLevelByOtherSkills.text = _otherskilllevel;
    CheckTargetLevel.text =$"{_currentlevel} / {_targetlevel}";
    CheckPercent.text = _percent.ToString() + "%";

    CurrentPreview = SelectionCheckPanel.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(SelectionCheckPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenSelectionElsePreview(SelectionData _selection)
  {
    SetRewardIcons(CheckRewardIcons, _selection.SelectionSuccesRewards);
    Sprite _icon = null;
    if (_selection.ThisSelectionType.Equals(SelectionTargetType.Skill)) _icon = GameManager.Instance.ImageHolder.SkillSelectionIcon;
    else if (_selection.ThisSelectionType.Equals(SelectionTargetType.Exp)) _icon = GameManager.Instance.ImageHolder.ExpSelectionIcon;
    else _icon = GameManager.Instance.ImageHolder.TendencySelectionIcon;

    SelectionElseIcon.sprite = _icon;
    SelectionElseDescription.text = _selection.SubDescription;

    CurrentPreview=SelectionElsePanel.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(SelectionElsePanel, true);
    StartCoroutine(_cor);
  }
  //보상 설명 : 체력,정신력,돈 설명?
  public void OpenRewardHPPreview(int _value)
  {
    Sprite _icon = null;
    float _modify = 0.0f;
    string _modifydescription = "";
    TextData _textdata = null;
    _icon = GameManager.Instance.ImageHolder.HPIcon;
    _textdata = GameManager.Instance.GetTextData("hp");
    _modify = (int)GameManager.Instance.MyGameData.GetHPGenModify(false);

    string _changedegree = "";
    _modifydescription = GameManager.Instance.GetTextData("default").Name + " " + _value.ToString() + "\n";
    if (_modify.Equals(0.0f)) _changedegree =ColorText.NeutralColor( GameManager.Instance.GetTextData("nochange").Name);
    else if(_modify>0)_changedegree=ColorText.PositiveColor(_modify.ToString());
    else _changedegree=ColorText.NegativeColor(_modify.ToString());
    _modifydescription+=$"{GameManager.Instance.GetTextData("modified").Name} {_changedegree}%";

    RewardStatusIcon.sprite = _icon;
    RewardStatusAmount.text ="+ "+ _value.ToString();
    RewardStatusDescription.text = _modifydescription;

    CurrentPreview = RewardStatusPanel.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(RewardStatusPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenRewardSanityPreview(int _value)
  {
    Sprite _icon = null;
    float _modify = 0.0f;
    string _modifydescription = "";
    TextData _textdata = null;
    _icon = GameManager.Instance.ImageHolder.SanityIcon;
    _textdata = GameManager.Instance.GetTextData("sanity");
    _modify = (int)GameManager.Instance.MyGameData.GetSanityGenModify(false);

    string _changedegree = "";
    _modifydescription = GameManager.Instance.GetTextData("default").Name + " " + _value.ToString() + "\n";
    if (_modify.Equals(0.0f)) _changedegree = ColorText.NeutralColor(GameManager.Instance.GetTextData("nochange").Name);
    else if (_modify > 0) _changedegree = ColorText.PositiveColor(_modify.ToString());
    else _changedegree = ColorText.NegativeColor(_modify.ToString());
    _modifydescription += $"{GameManager.Instance.GetTextData("modified").Name} {_changedegree}%";

    RewardStatusIcon.sprite = _icon;
    RewardStatusAmount.text = _value.ToString();
    RewardStatusDescription.text = _modifydescription;

    CurrentPreview = RewardStatusPanel.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(RewardStatusPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenRewardGoldPreview(int _value)
  {
    Sprite _icon = null;
    float _modify = 0.0f;
    string _modifydescription = "";
    TextData _textdata = null;
    _icon = GameManager.Instance.ImageHolder.GoldIcon;
    _textdata = GameManager.Instance.GetTextData("gold");
    _modify = (int)GameManager.Instance.MyGameData.GetGoldGenModify(false);

    string _changedegree = "";
    _modifydescription = GameManager.Instance.GetTextData("default").Name + " " + _value.ToString() + "\n";
    if (_modify.Equals(0.0f)) _changedegree = ColorText.NeutralColor(GameManager.Instance.GetTextData("nochange").Name);
    else if (_modify > 0) _changedegree = ColorText.PositiveColor(_modify.ToString());
    else _changedegree = ColorText.NegativeColor(_modify.ToString());
    _modifydescription += $"{GameManager.Instance.GetTextData("modified").Name} {_changedegree}%";

    RewardStatusIcon.sprite = _icon;
    RewardStatusAmount.text ="+"+ _value.ToString();
    RewardStatusDescription.text = _modifydescription;

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

    RewardTEName.text = _name;
    RewardTEIllust.sprite = _illust;
    RewardTEEffect.text = _effect;

    CurrentPreview = RewardTEPanel.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(RewardTEPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenRewardThemePreview(ThemeType _themetype)
  {
    TextData _textdata = GameManager.Instance.GetTextData(_themetype);
    string _name = $"{_textdata.Name} {GameManager.Instance.GetTextData("theme").Name} {GameManager.Instance.GetTextData("skill").Name} {GameManager.Instance.GetTextData("choice").Name}";
    Sprite _icon_a = GameManager.Instance.ImageHolder.GetThemeIcon(_themetype);

    RewardTSName.text = _name;
    RewardTSIcon_A.sprite = _icon_a;
    RewardTSIcon_B.sprite = GameManager.Instance.ImageHolder.UnknownTheme;

    CurrentPreview=RewardTSPanel.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(RewardTSPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenRewardSkillPreview(SkillName _skill)
  {
    TextData _textdata = GameManager.Instance.GetTextData(_skill);
    string _name = $"{_textdata.Name} {GameManager.Instance.GetTextData("defaultlevel").Name}";
    Sprite[] _icons = new Sprite[2];
    GameManager.Instance.ImageHolder.GetSkillIcons(_skill, ref _icons);

        SkillSelectName.text = _name;
        SkillSelectIcon_A.sprite = _icons[0]; SkillSelectIon_B.sprite = _icons[1];

    CurrentPreview = SkillSelectPanel.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(SkillSelectPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenSkillSelectPreview(SkillName _skill)
  {
    Sprite[] _icons = new Sprite[2];
    GameManager.Instance.ImageHolder.GetSkillIcons(_skill,ref _icons);
    SkillSelectIcon_A.sprite = _icons[0];SkillSelectIon_B.sprite = _icons[1];
    TextData _textdata = GameManager.Instance.GetTextData(_skill);
    SkillSelectName.text= _textdata.Name;

    CurrentPreview = SkillSelectPanel.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(SkillSelectPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenExpSelectionEmptyPreview(Experience _exp,bool islong)
  {
    string _name = _exp.Name;
    TextData _textdata = islong ? GameManager.Instance.GetTextData("longtermsave") : GameManager.Instance.GetTextData("shorttermsave");
    int _turn = islong ? ConstValues.LongTermStartTurn : ConstValues.ShortTermStartTurn;
    string _description = $"{_textdata.Name}\n\n{_textdata.Description.Replace("#TURN#",_turn.ToString()).Replace("#VALUE#",ConstValues.LongTermChangeCost.ToString())}";

    ExpSelectEmptyTurn.text = _turn.ToString();
    ExpSelectEmptyDescription.text = _description;
    CurrentPreview=ExpSelectEmptyPanel.GetComponent<RectTransform>();


    IEnumerator _cor = null;
    _cor = fadepreview(ExpSelectEmptyPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenExpSelectionExistPreview(Experience _origin,Experience _new,bool islong)
  {
    int _turn = islong ? ConstValues.LongTermStartTurn : ConstValues.ShortTermStartTurn;
    TextData _textdata = islong ? GameManager.Instance.GetTextData("longtermsave") : GameManager.Instance.GetTextData("shorttermsave");
    string _description = $"{_textdata.Name}\n\n{_textdata.Description.Replace("#TURN#", _turn.ToString()).Replace("#VALUE#", ConstValues.LongTermChangeCost.ToString())}";

    string _origineffect = _origin.ShortEffectString;
    ExpSelectOriginEffect.text = _origineffect;
    ExpSelectOriginTurn.text = _origin.Duration.ToString();

    string _neweffect = _new.ShortEffectString;
    ExpSelectNewEffect.text = _neweffect;
    ExpSelectNewTurn.text = _new.Duration.ToString();

    CurrentPreview = ExpSelectExistPanel.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(ExpSelectExistPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenExpSelectionBadPreview()
  {
    ExpSelectionBadText.text = GameManager.Instance.GetTextData("cannotchangebadexp").Name;

    CurrentPreview=ExpSelectionBadPanel.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(ExpSelectionBadPanel, true);
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
