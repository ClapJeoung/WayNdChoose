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
  [SerializeField] private Image[] SanityMadnessIcons = new Image[3];
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
  [SerializeField] private TextMeshProUGUI TendencyDescription = null;
  [SerializeField] private TextMeshProUGUI TendencySubDescription = null;
  [Space(10)]
  [SerializeField] private GameObject SelectionNonePanel = null;
  [SerializeField] private TextMeshProUGUI SelectionNoneText = null;
  [SerializeField] private PreviewSelectionTendency SelectionNoneTendency = null;
  [SerializeField] private Transform NoneRewardIcons = null;
  [SerializeField] private Vector2 SelectionNoneNoTendencySize = new Vector2(360.0f, 200.0f);
  [SerializeField] private Vector2 SelectionNoneTendencySize = new Vector2(360.0f, 320.0f);
  [Space(10)]
  [SerializeField] private GameObject SelectionPayPanel = null;
  [SerializeField] private Image PayIcon = null;
  [SerializeField] private TextMeshProUGUI PayTargetAmount = null;
  [SerializeField] private TextMeshProUGUI PayTargetDescription = null;
  [SerializeField] private TextMeshProUGUI PaySuccessPercent = null;
  [SerializeField] private TextMeshProUGUI PayDescription = null;
  [SerializeField] private Transform PayRewardIcons = null;
  [SerializeField] private PreviewSelectionTendency SelectionPayTendendcy = null;
  private Vector2 SelectionPayNoneTendencySize = new Vector2(360.0f, 350.0f);
  [SerializeField] private RectTransform SelectionPayInfoRect = null;
   private float SelectionPayDescriptionLength = 47.0f;
   private float SelectionPayPerLength = 80.0f;
   private float SelectionPayTendendcyLength = 80.0f;
   private float SelectionPayInfoSpace = 25.0f;
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
  [SerializeField] private PreviewSelectionTendency SelectionCheckTendendcy = null;
  private Vector2 SelectionCheckNoneTendendcySize=new Vector2(360.0f,400.0f);
  private float SelectionCheckLevelByOtherLength = 65.0f;
  private float SelectionCheckTendencySize = 80.0f;
  private float SelectionCheckInfoSpace = 25.0f;
  [Space(10)]
  [SerializeField] private GameObject SelectionElsePanel = null;
  [SerializeField] private Image SelectionElseIcon = null;
  [SerializeField] private TextMeshProUGUI SelectionElseDescription = null;
  [SerializeField] private PreviewSelectionTendency SelectionElseTendency = null;
  [SerializeField] private Vector2 SelectionElseNoneTendencySize = new Vector2(360.0f, 370.0f);
  [SerializeField] private Vector2 SelectionElseTendencySize = new Vector2(360.0f, 490.0f);
  [Space(10)]
  [SerializeField] private GameObject RewardStatusPanel = null;
  [SerializeField] private Image RewardStatusIcon = null;
  [SerializeField] private TextMeshProUGUI RewardStatusAmount = null;
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
    StatusType _currenttype = StatusType.HP;

    string _str = "";
    TextData _textdata = new TextData();
    bool _isincrease = true;
    bool _isup = true;

    _isincrease = true;
    int _genvalue = ((int)GameManager.Instance.MyGameData.GetHPGenModify(false));
    if (_genvalue.Equals(0)) _str = $"{GameManager.Instance.GetTextData(_currenttype,_isincrease).Name} {GameManager.Instance.GetTextData("nochange").Name}";
    else
    {
      if (_genvalue > 0)
      {
        _isup = true;
        _str = $"{GameManager.Instance.GetTextData(_currenttype, _isincrease, _isup, _genvalue).Name} {ColorText.PositiveColor("+" + _genvalue.ToString())}%";
      }//값이 양수면 긍정적인 효과
      else
      {
        _isup = false;
        _str = $"{GameManager.Instance.GetTextData(_currenttype, _isincrease, _isup, _genvalue).Name} {ColorText.NegativeColor("+" + _genvalue.ToString())}";
      }//값이 음수면 부정적인 효과
    }
    HPGenDescriptoin.text = _str;
    //회복 보정치 텍스트

    _isincrease = false;
      int _lossvalue = ((int)GameManager.Instance.MyGameData.GetHPLossModify(false));
    if (_lossvalue.Equals(0)) _str = $"{GameManager.Instance.GetTextData(_currenttype, _isincrease).Name} {GameManager.Instance.GetTextData("nochange").Name}";
    else
    {
      if (_lossvalue > 0)
      {
        _isup = true;

        _str = $"{GameManager.Instance.GetTextData(_currenttype, _isincrease, _isup, _lossvalue).Name} {ColorText.NegativeColor(_lossvalue.ToString())}%";
      }//값이 양수면 부정적인 효과
      else
      {
        _isup = false;
        _str = $"{GameManager.Instance.GetTextData(_currenttype, _isincrease, _isup, _lossvalue).Name} {ColorText.PositiveColor(_lossvalue.ToString())}";
      }//값이 음수면 긍정적인 효과
    }
    //소모 보정치 텍스트
    HPDecreaseDescription.text = _str;

    HPDescription.text = GameManager.Instance.GetTextData(_currenttype).Description;
    HPDecreaseDescription.text = _str;
    HpSubDescription.text = GameManager.Instance.GetTextData(_currenttype).SelectionSubDescription;
    CurrentPreview = HPPreview.GetComponent<RectTransform>();
 
    IEnumerator _cor = null;
    _cor = fadepreview(HPPreview, true);
    StartCoroutine(_cor);
  }//체력 설명, 증감량 표기 후 열기
  public void OpenSanityPreview()
  {
    StatusType _currenttype = StatusType.Sanity;
    string _str = "";
    TextData _textdata = new TextData();
    bool _isincrease = true;
    bool _isup = true;

    _isincrease = true;
    int _genvalue = ((int)GameManager.Instance.MyGameData.GetSanityGenModify(false));
    if (_genvalue.Equals(0)) _str = $"{GameManager.Instance.GetTextData(_currenttype, _isincrease).Name} {GameManager.Instance.GetTextData("nochange").Name}";
    else
    {
      if (_genvalue > 0)
      {
        _isup = true;
        _str = $"{GameManager.Instance.GetTextData(_currenttype, _isincrease, _isup, _genvalue).Name} {ColorText.PositiveColor("+" + _genvalue.ToString())}%";
      }//값이 양수면 긍정적인 효과
      else
      {
        _isup = false;
        _str = $"{GameManager.Instance.GetTextData(_currenttype, _isincrease, _isup, _genvalue).Name} {ColorText.NegativeColor("+" + _genvalue.ToString())}";
      }//값이 음수면 부정적인 효과
    }
    SanityGenDescriptoin.text = _str;

    for(int i = 0; i < 3; i++)
    {
      if (i < GameManager.Instance.MyGameData.MadnessCount) SanityMadnessIcons[i].sprite = GameManager.Instance.ImageHolder.MadnessActive;
      else SanityMadnessIcons[i].sprite = GameManager.Instance.ImageHolder.MadnessIdle;
    }

    _isincrease = false;
    int _lossvalue = ((int)GameManager.Instance.MyGameData.GetSanityLossModify(false));
    if (_lossvalue.Equals(0)) _str = $"{GameManager.Instance.GetTextData(_currenttype, _isincrease).Name} {GameManager.Instance.GetTextData("nochange").Name}";
    else
    {
      if (_lossvalue > 0)
      {
        _isup = true;
        _str = $"{GameManager.Instance.GetTextData(_currenttype, _isincrease, _isup, _lossvalue).Name} {ColorText.NegativeColor(_lossvalue.ToString())}%";
      }//값이 양수면 부정적인 효과
      else
      {
        _isup = false;
        _str = $"{GameManager.Instance.GetTextData(_currenttype, _isincrease, _isup, _lossvalue).Name} {ColorText.PositiveColor(_lossvalue.ToString())}";
      }//값이 음수면 긍정적인 효과
    }
    SanityDecreaseDescription.text = _str;

    SanityDescription.text = GameManager.Instance.GetTextData(_currenttype).Description;
    SanityDecreaseDescription.text = _str;
      SanitySubDescription.text = GameManager.Instance.GetTextData(_currenttype).SelectionSubDescription;
    CurrentPreview = SanityPreview.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(SanityPreview, true);
    StartCoroutine(_cor);
  }//정신력 설명,증감량 표기 후 열기
  public void OpenGoldPreview()
  {
    StatusType _currenttype = StatusType.Gold;
    string _str = "";
    TextData _textdata = new TextData();
    bool _isincrease = true;
    bool _isup = true;

    _isincrease = true;
    int _genvalue = ((int)GameManager.Instance.MyGameData.GetGoldGenModify(false));
    if (_genvalue.Equals(0)) _str = $"{GameManager.Instance.GetTextData(_currenttype, _isincrease).Name} {GameManager.Instance.GetTextData("nochange").Name}";
    else
    {
      if (_genvalue > 0)
      {
        _isup = true;
        _str = $"{GameManager.Instance.GetTextData(_currenttype, _isincrease, _isup, _genvalue).Name} {ColorText.PositiveColor("+"+_genvalue.ToString())}%";
      }//값이 양수면 긍정적인 효과
      else
      {
        _isup = false;
        _str = $"{GameManager.Instance.GetTextData(_currenttype, _isincrease, _isup, _genvalue).Name} {ColorText.NegativeColor("+" + _genvalue.ToString())}";
      }//값이 음수면 부정적인 효과
    }
    GoldGenDescriptoin.text = _str;


    _isincrease = false;
    int _lossvalue = ((int)GameManager.Instance.MyGameData.GetGoldPayModify(false));
    if (_lossvalue.Equals(0)) _str = $"{GameManager.Instance.GetTextData(_currenttype, _isincrease).Name} {GameManager.Instance.GetTextData("nochange").Name}";
    else
    {
      if (_lossvalue > 0)
      {
        _isup = true;
        _str = $"{GameManager.Instance.GetTextData(_currenttype, _isincrease, _isup, _lossvalue).Name} {ColorText.NegativeColor(_lossvalue.ToString())}%";
      }//값이 양수면 부정적인 효과
      else
      {
        _isup = false;
        _str = $"{GameManager.Instance.GetTextData(_currenttype, _isincrease, _isup, _lossvalue).Name} {ColorText.PositiveColor(_lossvalue.ToString())}";
      }//값이 음수면 긍정적인 효과
    }
    GoldDecreaseDescription.text = _str;

    GoldDescription.text = GameManager.Instance.GetTextData(_currenttype).Description;
    GoldDecreaseDescription.text = _str;
    GoldSubDescription.text = GameManager.Instance.GetTextData(_currenttype).SelectionSubDescription;

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
    _str=string.Format(GameManager.Instance.GetTextData("byskill").Name, _valuestr);
    ThemeLevelBySkill.text = _str;

    makecolorvalue(_onlyexp);
    _str =string.Format(GameManager.Instance.GetTextData("byexp").Name, _valuestr);
    ThemeLevelByExp.text = _str;

    makecolorvalue(_onlytendency);
    _str=string.Format(GameManager.Instance.GetTextData("bytendency").Name,_valuestr);
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
    _str =string.Format(GameManager.Instance.GetTextData("byownlevel").Name, _valuestr);
    SkillLevelBySkill.text = _str;

    makecolorvalue(_onlyexp);
    _str =string.Format(GameManager.Instance.GetTextData("byexp").Name, _valuestr);
    SkillLevelByExp.text = _str;


    if (_onlyplace != 0)
    {
      makecolorvalue(_onlyplace);
      _str = string.Format(GameManager.Instance.GetTextData("byplace").Name, _valuestr);
      SkillLevelByPlace.text = _str;
    }
    else SkillLevelByPlace.text = "";

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
    switch (_type)
    {
      case TendencyType.Head:
        _tendencyicon = GameManager.Instance.ImageHolder.GetTendencyIcon(_type,GameManager.Instance.MyGameData.GetTendencyLevel(_type));
        _textdata = GameManager.Instance.MyGameData.Tendency_Head.MyTextData;
        break;
      case TendencyType.Body:
        _tendencyicon = GameManager.Instance.ImageHolder.GetTendencyIcon(_type, GameManager.Instance.MyGameData.GetTendencyLevel(_type));
        _textdata = GameManager.Instance.MyGameData.Tendency_Body.MyTextData;
        break;
    }
    string _tendencyeffect = GameManager.Instance.MyGameData.GetTendencyEffectString_short(_type);

    _tendencyname = _textdata.Name;
    TendencyIcon.sprite = _tendencyicon;
    TendencyName.text = _tendencyname;
    TendencyDescription.text = _tendencyeffect;
    TendencySubDescription.text = _textdata.SelectionDescription;

    CurrentPreview = TendencyPreview.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(TendencyPreview, true);
    StartCoroutine(_cor);
  }
  public void OpenSelectionNonePreview(SelectionData _selection,TendencyType tendencytype,bool dir)
  {
    SelectionNoneText.text = _selection.SubDescription;
    SetRewardIcons(NoneRewardIcons, _selection.SelectionSuccesRewards);

    CurrentPreview = SelectionNonePanel.GetComponent<RectTransform>();

    switch (tendencytype)
    {
      case TendencyType.None:
        if (SelectionNoneTendency.gameObject.activeInHierarchy.Equals(true)) SelectionNoneTendency.gameObject.SetActive(false);
        SelectionNonePanel.GetComponent<RectTransform>().sizeDelta = SelectionNoneNoTendencySize;
        break;
      case TendencyType.Body:
        if (SelectionNoneTendency.gameObject.activeInHierarchy.Equals(false)) SelectionNoneTendency.gameObject.SetActive(true);
        SelectionNonePanel.GetComponent<RectTransform>().sizeDelta = SelectionNoneTendencySize;
        SelectionNoneTendency.Setup(GameManager.Instance.MyGameData.Tendency_Body,dir);
        break;
      case TendencyType.Head:
        if (SelectionNoneTendency.gameObject.activeInHierarchy.Equals(false)) SelectionNoneTendency.gameObject.SetActive(true);
        SelectionNonePanel.GetComponent<RectTransform>().sizeDelta = SelectionNoneTendencySize;
        SelectionNoneTendency.Setup(GameManager.Instance.MyGameData.Tendency_Head, dir);
        break;
    }

    IEnumerator _cor = null;
    _cor = fadepreview(SelectionNonePanel, true);
    StartCoroutine(_cor);
  }
  public void OpenSelectionPayPreview(SelectionData _selection, TendencyType tendencytype, bool dir)
  {
    PayDescription.text = _selection.SubDescription;
    SetRewardIcons(PayRewardIcons, _selection.SelectionSuccesRewards);
    Sprite _icon = null;
    int _target = 0;
    int _target_origin = 0;
    int _modify = 0;
    string _targetdescription = "";
    int _percent = -1;
    StatusType _status = StatusType.HP;
    switch (_selection.SelectionPayTarget)
    {
      case StatusType.HP:
        _status = StatusType.HP;
        _icon = GameManager.Instance.ImageHolder.HPDecreaseIcon;
        _target_origin = GameManager.Instance.MyGameData.PayHPValue_origin;
        _modify = (int)GameManager.Instance.MyGameData.GetHPLossModify(false);
        _target = GameManager.Instance.MyGameData.PayHPValue_modified;
        if (_modify.Equals(0)) _targetdescription = "";
        else if (_modify > 0)
        {
          _targetdescription = $"{GameManager.Instance.GetTextData("default").Name} {_target_origin}\n{GameManager.Instance.GetTextData(_status, false, true, _modify).Name} {ColorText.NegativeColor(_modify.ToString())}%";
        }//보정치가 0 이상이라면 부정적인것
        else
        {
          _targetdescription = $"{GameManager.Instance.GetTextData("default").Name} {_target_origin}\n{GameManager.Instance.GetTextData(_status, false, false, _modify).Name} {ColorText.PositiveColor(_modify.ToString())}%";
        }//보정치가 0 이하라면 긍정적인것
        break;//체력이라면 지불 기본값, 보정치, 최종값을 받아오고 보정치가 존재한다면 텍스트에 삽입

      case StatusType.Sanity:
        _status = StatusType.Sanity;
        _icon = GameManager.Instance.ImageHolder.SanityDecreaseIcon;
        _target_origin = GameManager.Instance.MyGameData.PaySanityValue_origin;
        _modify = (int)GameManager.Instance.MyGameData.GetSanityLossModify(false);
        _target = GameManager.Instance.MyGameData.PaySanityValue_modified;
        if (_modify.Equals(0)) _targetdescription = "";
        else if (_modify > 0)
        {
          _targetdescription = $"{GameManager.Instance.GetTextData("default").Name} {_target_origin}\n{GameManager.Instance.GetTextData(_status, false, true, _modify).Name} {ColorText.NegativeColor(_modify.ToString())}%";
        }//보정치가 0 이상이라면 부정적인것
        else
        {
          _targetdescription = $"{GameManager.Instance.GetTextData("default").Name} {_target_origin}\n{GameManager.Instance.GetTextData(_status, false, false, _modify).Name} {ColorText.PositiveColor(_modify.ToString())}%";
        }//보정치가 0 이하라면 긍정적인것
        break;//정신력이라면 지불 기본값,보정치,최종값을 받아오고 보정치가 존재한다면 텍스트에 삽입
      case StatusType.Gold:
        _status = StatusType.Gold;
        _icon = GameManager.Instance.ImageHolder.GoldDecreaseIcon;
        _target_origin = GameManager.Instance.MyGameData.PayGoldValue_origin;
        _modify = (int)GameManager.Instance.MyGameData.GetGoldPayModify(false);
        _target = GameManager.Instance.MyGameData.PayGoldValue_modified;
        if (_modify.Equals(0)) _targetdescription = "";
        else if (_modify > 0)
        {
          _targetdescription = $"{GameManager.Instance.GetTextData("default").Name} {_target_origin}\n{GameManager.Instance.GetTextData(_status, false, true, _modify).Name} {ColorText.NegativeColor(_modify.ToString())}%";
        }//보정치가 0 이상이라면 부정적인것
        else
        {
          _targetdescription = $"{GameManager.Instance.GetTextData("default").Name} {_target_origin}\n{GameManager.Instance.GetTextData(_status, false, false, _modify).Name} {ColorText.PositiveColor(_modify.ToString())}%";
        }//보정치가 0 이하라면 긍정적인것
        if (_target > GameManager.Instance.MyGameData.Gold) _percent = GameManager.Instance.MyGameData.CheckPercent_money(_target);
        break;//골드라면 지불,기본값,보정치,최종값을 받아오고 보정치가 존재한다면 텍스트에 삽입, 최종값이 보유값을 넘는다면 실패 확률 확인
    }

    PayIcon.sprite = _icon;
    PayTargetAmount.text = (-_target).ToString();

    CurrentPreview = SelectionPayPanel.GetComponent<RectTransform>();
    float _additionallength = 0;

    Vector2 _newsize = SelectionPayNoneTendencySize;
    if (_modify.Equals(0))//변동치 존재하면 변동치 텍스트 활성화
    {
      if (PayTargetDescription.gameObject.activeInHierarchy.Equals(true)) PayTargetDescription.gameObject.SetActive(false);
    }
    else
    {
      if (PayTargetDescription.gameObject.activeInHierarchy.Equals(false)) PayTargetDescription.gameObject.SetActive(true);
      PayTargetDescription.text = _targetdescription;
         _additionallength += SelectionPayDescriptionLength + SelectionPayInfoSpace;
  }

    if (_percent.Equals(-1))//(골드) 확률 검사해야 하면 확률 텍스트 활성화
    {
      if (PaySuccessPercent.gameObject.activeInHierarchy.Equals(true)) PaySuccessPercent.gameObject.SetActive(false);
    }
    else
    {
      if (PaySuccessPercent.gameObject.activeInHierarchy.Equals(false)) PaySuccessPercent.gameObject.SetActive(true);
      PaySuccessPercent.text = _percent.ToString() + "%";
         _additionallength += SelectionPayPerLength + SelectionPayInfoSpace;
    }

    switch (tendencytype)//성향 존재하는거면 그거 활성화
    {
      case TendencyType.None:
        if (SelectionPayTendendcy.gameObject.activeInHierarchy.Equals(true)) SelectionPayTendendcy.gameObject.SetActive(false);
        break;
      case TendencyType.Body:
        if (SelectionPayTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionPayTendendcy.gameObject.SetActive(true);
        SelectionPayTendendcy.Setup(GameManager.Instance.MyGameData.Tendency_Body, dir);
        _additionallength += SelectionPayTendendcyLength + SelectionPayInfoSpace;
        break;
      case TendencyType.Head:
        if (SelectionPayTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionPayTendendcy.gameObject.SetActive(true);
        SelectionPayTendendcy.Setup(GameManager.Instance.MyGameData.Tendency_Head, dir);
        _additionallength += SelectionPayTendendcyLength + SelectionPayInfoSpace;
        break;
    }
    if (_additionallength > 0) _additionallength += SelectionPayInfoSpace;

    _newsize = new Vector2(_newsize.x, _newsize.y + _additionallength);
    Debug.Log($"{SelectionPayNoneTendencySize}  {_additionallength}");

    SelectionPayPanel.GetComponent<RectTransform>().sizeDelta = _newsize;

    IEnumerator _cor = null;
    _cor = fadepreview(SelectionPayPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenSelectionCheckPreview_theme(SelectionData _selection, TendencyType tendencytype, bool dir)
  {
    CheckLevelByOtherSkills.text = "";
    CheckDescription.text = _selection.SubDescription;
    SetRewardIcons(CheckRewardIcons, _selection.SelectionSuccesRewards);
    Sprite _icon = null;
    int _currentlevel = 0;
    int  _byskill = 0, _byexp = 0, _bytendency = 0;
    int _targetlevel = GameManager.Instance.MyGameData.CheckThemeValue;
    int _percent = -1;
    ThemeType _themetype = _selection.SelectionCheckTheme;

    _icon = GameManager.Instance.ImageHolder.GetThemeIcon(_themetype);
    _byskill=GameManager.Instance.MyGameData.GetThemeLevelBySkill(_themetype);
    _byexp=GameManager.Instance.MyGameData.GetEffectThemeCount_Exp(_themetype);
    _bytendency=GameManager.Instance.MyGameData.GetThemeLevelByTendency(_themetype);

    _currentlevel = GameManager.Instance.MyGameData.GetThemeLevel(_themetype);
    _percent = GameManager.Instance.MyGameData.CheckPercent_themeorskill(_currentlevel, _targetlevel);

    SelectionCheckName.text = GameManager.Instance.GetTextData(_themetype).Name;
    CheckIcon_A.sprite = _icon;
    CheckIcon_B.transform.parent.gameObject.SetActive(false);
    CheckTargetLevel.text = $"{_currentlevel} / {_targetlevel}";
    CheckPercent.text = _percent.ToString() + "%";

    CurrentPreview = SelectionCheckPanel.GetComponent<RectTransform>();

    Vector2 _newsize = SelectionCheckNoneTendendcySize;
    float _addlength = 0.0f;
    switch (tendencytype)
    {
      case TendencyType.None:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(true)) SelectionCheckTendendcy.gameObject.SetActive(false);
        break;
      case TendencyType.Body:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionCheckTendendcy.gameObject.SetActive(true);
        SelectionCheckTendendcy.Setup(GameManager.Instance.MyGameData.Tendency_Body, dir);
        _addlength += SelectionCheckTendencySize + SelectionCheckInfoSpace;
        break;
      case TendencyType.Head:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionCheckTendendcy.gameObject.SetActive(true);
        SelectionCheckTendendcy.Setup(GameManager.Instance.MyGameData.Tendency_Head, dir);
        _addlength += SelectionCheckTendencySize + SelectionCheckInfoSpace;
        break;
    }
    if (_addlength > 0) _addlength += SelectionCheckInfoSpace;
    _newsize = new Vector2(_newsize.x, _newsize.y + _addlength);

    SelectionCheckPanel.GetComponent<RectTransform>().sizeDelta = _newsize;
    IEnumerator _cor = null;
    _cor = fadepreview(SelectionCheckPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenSelectionCheckPreview_skill(SelectionData _selection, TendencyType tendencytype, bool dir)
  {
    CheckDescription.text = _selection.SubDescription;
    SetRewardIcons(CheckRewardIcons, _selection.SelectionSuccesRewards);
    Sprite[] _icons = new Sprite[2];
    int _currentlevel = 0;
    int _byotherskills = 0;
    int _targetlevel = GameManager.Instance.MyGameData.CheckSkillValue;
    int _percent = 0;
    Skill _skill = GameManager.Instance.MyGameData.Skills[_selection.SelectionCheckSkill];
    GameManager.Instance.ImageHolder.GetSkillIcons(_skill.SkillType, ref _icons);

    _currentlevel = _skill.LevelForSkillCheck;
    _byotherskills = _skill.LevelByTheme;
    string _otherskilllevel = "";
    if (_byotherskills<=0) _otherskilllevel = "";
    else _otherskilllevel =string.Format(GameManager.Instance.GetTextData("byotherskills").Name, ColorText.PositiveColor(_otherskilllevel.ToString()));
    
    _percent = GameManager.Instance.MyGameData.CheckPercent_themeorskill(_currentlevel, _targetlevel);

    SelectionCheckName.text = GameManager.Instance.GetTextData(_skill.SkillType).Name;
    CheckIcon_A.sprite = _icons[0];
    if (CheckIcon_B.transform.parent.gameObject.activeSelf.Equals(false)) CheckIcon_B.transform.parent.gameObject.SetActive(true);
    CheckIcon_B.sprite = _icons[1];
    CheckLevelByOtherSkills.text = _otherskilllevel;
    CheckTargetLevel.text =$"{_currentlevel} / {_targetlevel}";
    CheckPercent.text = _percent.ToString() + "%";

    CurrentPreview = SelectionCheckPanel.GetComponent<RectTransform>();
    Vector2 _newsize = SelectionCheckNoneTendendcySize;
    float _addlength = 0.0f;

    if (_byotherskills > 0) _addlength += SelectionCheckLevelByOtherLength + SelectionCheckInfoSpace;
    switch (tendencytype)
    {
      case TendencyType.None:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(true)) SelectionCheckTendendcy.gameObject.SetActive(false);
        break;
      case TendencyType.Body:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionCheckTendendcy.gameObject.SetActive(true);
        SelectionCheckTendendcy.Setup(GameManager.Instance.MyGameData.Tendency_Body, dir);
        _addlength += SelectionCheckTendencySize + SelectionCheckInfoSpace;
        break;
      case TendencyType.Head:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionCheckTendendcy.gameObject.SetActive(true);
        SelectionCheckTendendcy.Setup(GameManager.Instance.MyGameData.Tendency_Head, dir);
        _addlength += SelectionCheckTendencySize + SelectionCheckInfoSpace; 
        break;
    }
    if (_addlength > 0.0f) _addlength += SelectionCheckInfoSpace;
    _newsize = new Vector2(_newsize.x, _newsize.y + _addlength);

    SelectionCheckPanel.GetComponent<RectTransform>().sizeDelta = _newsize;

    IEnumerator _cor = null;
    _cor = fadepreview(SelectionCheckPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenSelectionElsePreview(SelectionData _selection, TendencyType tendencytype, bool dir)
  {
    SetRewardIcons(CheckRewardIcons, _selection.SelectionSuccesRewards);
    Sprite _icon = null;
    if (_selection.ThisSelectionType.Equals(SelectionTargetType.Skill)) _icon = GameManager.Instance.ImageHolder.SkillSelectionIcon;
    else if (_selection.ThisSelectionType.Equals(SelectionTargetType.Exp)) _icon = GameManager.Instance.ImageHolder.ExpSelectionIcon;
    else _icon = GameManager.Instance.ImageHolder.TendencySelectionIcon;

    SelectionElseIcon.sprite = _icon;
    SelectionElseDescription.text = _selection.SubDescription;

    CurrentPreview=SelectionElsePanel.GetComponent<RectTransform>();

    switch (tendencytype)
    {
      case TendencyType.None:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(true)) SelectionCheckTendendcy.gameObject.SetActive(false);
        SelectionElsePanel.GetComponent<RectTransform>().sizeDelta = SelectionElseNoneTendencySize;
        break;
      case TendencyType.Body:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionCheckTendendcy.gameObject.SetActive(true);
        SelectionElsePanel.GetComponent<RectTransform>().sizeDelta = SelectionElseTendencySize;
        SelectionElseTendency.Setup(GameManager.Instance.MyGameData.Tendency_Body, dir);
        break;
      case TendencyType.Head:
        if (SelectionCheckTendendcy.gameObject.activeInHierarchy.Equals(false)) SelectionCheckTendendcy.gameObject.SetActive(true);
        SelectionElsePanel.GetComponent<RectTransform>().sizeDelta = SelectionElseTendencySize;
        SelectionElseTendency.Setup(GameManager.Instance.MyGameData.Tendency_Head, dir);
        break;
    }

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
    _icon = GameManager.Instance.ImageHolder.HPIcon;
    StatusType _statustype = StatusType.HP;
    _modify = (int)GameManager.Instance.MyGameData.GetHPGenModify(false);

    string _changedegree = "";
    _modifydescription = GameManager.Instance.GetTextData("default").Name + " " + GameManager.Instance.MyGameData.RewardHPValue_origin.ToString() + "\n";
    if (_modify.Equals(0.0f)) _changedegree =ColorText.NeutralColor( GameManager.Instance.GetTextData("nochange").Name);
    else if(_modify>0)_changedegree=ColorText.PositiveColor(_modify.ToString())+"%";
    else _changedegree=ColorText.NegativeColor(_modify.ToString()) + "%";

    _modifydescription+=$"{GameManager.Instance.GetTextData("modified").Name} {_changedegree}";

    RewardStatusIcon.sprite = _icon;
    RewardStatusAmount.text = _modifydescription+"\n\n+ " + _value.ToString();

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
    StatusType _statustype = StatusType.Sanity;
    _icon = GameManager.Instance.ImageHolder.SanityIcon;
    _modify = (int)GameManager.Instance.MyGameData.GetSanityGenModify(false);

    string _changedegree = "";
    _modifydescription = GameManager.Instance.GetTextData("default").Name + " " + GameManager.Instance.MyGameData.RewardSanityValue_origin.ToString() + "\n";
    if (_modify.Equals(0.0f)) _changedegree = ColorText.NeutralColor(GameManager.Instance.GetTextData("nochange").Name);
    else if (_modify > 0) _changedegree = ColorText.PositiveColor(_modify.ToString()) + "%";
    else _changedegree = ColorText.NegativeColor(_modify.ToString()) + "%";

    _modifydescription += $"{GameManager.Instance.GetTextData("modified").Name} {_changedegree}";


    RewardStatusIcon.sprite = _icon;
    RewardStatusAmount.text = _modifydescription + "\n\n+ " + _value.ToString();

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
    StatusType _statustype = StatusType.Gold;
    _icon = GameManager.Instance.ImageHolder.GoldIcon;
    _modify = (int)GameManager.Instance.MyGameData.GetGoldGenModify(false);

    string _changedegree = "";
    _modifydescription = GameManager.Instance.GetTextData("default").Name + " " + _value.ToString() + "\n";
    if (_modify.Equals(0.0f)) _changedegree = ColorText.NeutralColor(GameManager.Instance.GetTextData("nochange").Name);
    else if (_modify > 0) _changedegree = ColorText.PositiveColor(_modify.ToString()) + "%";
    else _changedegree = ColorText.NegativeColor(_modify.ToString()) + "%";

    _modifydescription += $"{GameManager.Instance.GetTextData("modified").Name} {_changedegree}";

    RewardStatusIcon.sprite = _icon;
    RewardStatusAmount.text = _modifydescription + "\n\n+ " + _value.ToString();

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
    string _description = $"{_textdata.Name}\n\n" +
      $"{string.Format(_textdata.Description,_turn,ConstValues.LongTermChangeCost.ToString())}";

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
    string _description = $"{_textdata.Name}\n\n" +
      $"{string.Format(_textdata.Description, _turn, ConstValues.LongTermChangeCost.ToString())}";

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
  public void OpenDisComfortPanel()
  {
     DiscomfortText.text = GameManager.Instance.GetTextData("discomfort").Description;

    CurrentPreview = DisComfortPanel.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(DisComfortPanel, true);
    StartCoroutine(_cor);
  }
  public void OpenPlacePanel(PlaceType place)
  {
    Sprite _placeicon = null;
    _placeicon = GameManager.Instance.ImageHolder.GetPlaceIcon(place);
    Sprite _themeicon = null;

    string _name = "", _effectdescription = "", _subdescription = "";
    TextData _textdata=GameManager.Instance.GetPlaceEffectTextData(place);
    _name = GameManager.Instance.GetTextData(place).Name;
    _subdescription = _textdata.Description;
    switch (place)
    {
      case PlaceType.Residence:
        if (PlaceThemeIcon.transform.parent.gameObject.activeInHierarchy.Equals(transform)) PlaceThemeIcon.transform.parent.gameObject.SetActive(false);
        _effectdescription = string.Format(_textdata.Name, GameManager.Instance.MyGameData.PlaceEffects[place], ((int)(ConstValues.PlaceEffect_residence * 100.0f)).ToString());
        break;
      case PlaceType.Marketplace:
        if (PlaceThemeIcon.transform.parent.gameObject.activeInHierarchy.Equals(transform)) PlaceThemeIcon.transform.parent.gameObject.SetActive(false);
        _effectdescription = string.Format(_textdata.Name, ConstValues.PlaceEffect_marketplace.ToString());
        break;
      case PlaceType.Temple:
        if (PlaceThemeIcon.transform.parent.gameObject.activeInHierarchy.Equals(transform)) PlaceThemeIcon.transform.parent.gameObject.SetActive(false);
        _effectdescription = _textdata.Name;
        break;
      case PlaceType.Library:
        ThemeType _theme = GameManager.Instance.MyGameData.CurrentSettlement.LibraryType;
        _themeicon = GameManager.Instance.ImageHolder.GetThemeIcon(_theme);
        _effectdescription = string.Format(_textdata.Name, GameManager.Instance.MyGameData.PlaceEffects[place], _themeicon, GameManager.Instance.GetTextData(_theme).Name);
      if(PlaceThemeIcon.transform.parent.gameObject.activeInHierarchy.Equals(false))PlaceThemeIcon.transform.parent.gameObject.SetActive(true);
        break;
      case PlaceType.Theater:
        if (PlaceThemeIcon.transform.parent.gameObject.activeInHierarchy.Equals(transform)) PlaceThemeIcon.transform.parent.gameObject.SetActive(false);
        _effectdescription = _textdata.Name;
        break;
      case PlaceType.Academy:
        if (PlaceThemeIcon.transform.parent.gameObject.activeInHierarchy.Equals(transform)) PlaceThemeIcon.transform.parent.gameObject.SetActive(false);
        _effectdescription = string.Format(_textdata.Name, GameManager.Instance.MyGameData.PlaceEffects[place], ConstValues.PlaceEffect_acardemy.ToString());
        break;
    }
    PlaceName.text = _name;
    PlaceIcon.sprite = _placeicon;
    PlaceThemeIcon.sprite = _themeicon;
    PlaceTurn.text = GameManager.Instance.MyGameData.PlaceEffects[place].ToString();
    PlaceDescription.text = _effectdescription;
    PlaceSubDescription.text = _subdescription;

    CurrentPreview = PlacePanel.GetComponent<RectTransform>();

    IEnumerator _cor = null;
    _cor = fadepreview(PlacePanel, true);
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
