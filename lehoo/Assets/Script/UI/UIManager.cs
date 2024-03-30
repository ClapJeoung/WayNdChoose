using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
  public AudioManager AudioManager = null;
  private static UIManager instance;
  public static UIManager Instance
  {
    get { return instance; }
  }
  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    StartCoroutine(ChangeAlpha(CurtainGroup, 0.0f, 2.5f));
  }
  public void ResetGame(string text,bool isending)
  {
    AudioManager.SetoffBGM();
    IsWorking = true;
    CurtainText.text=text;
    StartCoroutine(resetgame(isending));
  }
  private IEnumerator resetgame(bool isending)
  {
    yield return StartCoroutine(ChangeAlpha(CurtainGroup, 1.0f, 1.2f));
    if(isending) yield return new WaitForSeconds(3.0f);
    UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
  public AnimationCurve UIPanelOpenCurve = null;
  public AnimationCurve UIPanelCLoseCurve = null;
  public AnimationCurve CharacterMoveCurve = null;
  [Space(10)]
  public Transform MyCanvas = null;
  [SerializeField] private CanvasGroup CenterGroup = null;
  [SerializeField] private CanvasGroup CurtainGroup = null;
  [SerializeField] private TextMeshProUGUI CurtainText = null;
  public UI_Main MainUi = null;
  public UI_dialogue DialogueUI = null;
  public UI_RewardExp ExpRewardUI = null;
  public UI_QuestWolf CultUI = null;
  public UI_Mad MadUI = null;
  public SidePanel_Quest_Cult SidePanelCultUI = null;
  public UI_map MapUI = null;
  public UI_Ending EndingUI = null;
  public UI_Tutorial TutorialUI = null;
  public HighlightEffects HighlightManager = null;
  public ExpDragPreview ExpDragPreview = null;
  public MouseScript Mouse = null;
  public bool IsWorking = false;
  public PreviewManager PreviewManager = null;
  [SerializeField] private ImageSwapScript EnvirBackground = null;
  [SerializeField] private DebugScript DebugUI = null;
  public MapButton MapButton = null;
  public SettleButton SettleButton = null;
  [SerializeField] private AnimationCurve FadeAnimationCurve = null;
  [HideInInspector] public bool EnvirBackgroundEnable = false;
  public void UpdateBackground(EnvironmentType envir)
  {
    EnvirBackgroundEnable = true;
    Sprite _newbackground = GameManager.Instance.ImageHolder.GetEnvirBackground(envir);
    EnvirBackground.Next( _newbackground,1.0f);
  }
  public void UpdateBackground(SettlementType type)
  {
    EnvirBackgroundEnable = true;
    Sprite _newbackground = GameManager.Instance.ImageHolder.GetSettlementBackground(type,GameManager.Instance.MyGameData.Turn);
    EnvirBackground.Next(_newbackground, 1.0f);
  }
  public void OffBackground()
  {
    EnvirBackgroundEnable = false;
    EnvirBackground.Next(GameManager.Instance.ImageHolder.Transparent, 1.0f);
  }
  [Space(10)]
  [SerializeField] private TextMeshProUGUI YearText = null;
  public void UpdateYearText()
  {
    YearText.text= GameManager.Instance.MyGameData.Year.ToString();
  }
  [SerializeField] private Image SpringIcon = null, SummerIcon = null, FallIcon = null, WinterIcon = null;
  public void UpdateTurnIcon()
  {
    int _season = GameManager.Instance.MyGameData.Turn;
    SpringIcon.sprite = GameManager.Instance.ImageHolder.SpringIcon_deactive;
    SummerIcon.sprite = GameManager.Instance.ImageHolder.SummerIcon_deactive;
    FallIcon.sprite = GameManager.Instance.ImageHolder.FallIcon_deactive;
    WinterIcon.sprite = GameManager.Instance.ImageHolder.WinterIcon_deactive;
    switch (_season)
    {
      case 0:SpringIcon.sprite = GameManager.Instance.ImageHolder.SpringIcon_active;break;
      case 1:SummerIcon.sprite= GameManager.Instance.ImageHolder.SummerIcon_active;break;
      case 2:FallIcon.sprite= GameManager.Instance.ImageHolder.FallIcon_active;break;
      case 3:WinterIcon.sprite= GameManager.Instance.ImageHolder.WinterIcon_active;break;
    }
  }
  [Space(10)]
  [SerializeField] private float ExpGainTime = 0.5f;
  [SerializeField] private float ExpLossTime = 0.4f;
  [SerializeField] private AnimationCurve ExpAnimationCurve=new AnimationCurve();
  public IEnumerator ExpGainCount(RectTransform rect)
  {
    float _time = 0.0f;
    Vector3 _currentscale = Vector3.one;

    while (_time < ExpGainTime)
    {
      _currentscale = Vector3.Lerp(Vector3.one, Vector3.one * 1.5f, ExpAnimationCurve.Evaluate(_time / ExpGainTime));
        rect.localScale = _currentscale;
      _time += Time.deltaTime;
      yield return null;
    }
      rect.localScale = Vector3.one;
  }
  private IEnumerator ExpLossCount(RectTransform rect)
  {
    float _time = 0.0f;
    float _lossscale = 1.2f;
    Vector3 _currentscale = Vector3.one * _lossscale;
    while (_time < ExpLossTime)
    {
      _currentscale = Vector3.Lerp(Vector3.one, Vector3.one * _lossscale, ExpAnimationCurve.Evaluate(_time / ExpLossTime));
        rect.localScale = _currentscale;
      _time += Time.deltaTime;
      yield return null;
    }
      rect.localScale = Vector3.one;
  }
  /// <summary>
  /// 일반
  /// </summary>
  /// <param name="tmp"></param>
  /// <param name="current"></param>
  /// <param name="target"></param>
  /// <returns></returns>
  private float CountChangeTime_status = 0.5f;
  public IEnumerator ChangeCount(TextMeshProUGUI tmp, float current,float target)
  {
    int _count = (int)current;
    float _time = 0.0f, _targettime = CountChangeTime_status;
    while (_time < _targettime)
    {
      _count = Mathf.FloorToInt(Mathf.Lerp(current, target, _time / _targettime));
      tmp.text=_count.ToString();

      _time += Time.deltaTime;
      yield return null;
    }
    tmp.text = ((int)target).ToString();
  }
  /// <summary>
  /// 경험 카운트
  /// </summary>
  /// <param name="tmp"></param>
  /// <param name="current"></param>
  /// <param name="target"></param>
  /// <param name="exp"></param>
  /// <returns></returns>
  public IEnumerator ChangeCount(TextMeshProUGUI tmp, float current, float target,Experience exp)
  {
    int _count = (int)current;
    float _time = 0.0f, _targettime = CountChangeTime_status;
    while (_time < _targettime)
    {
      _count = Mathf.FloorToInt(Mathf.Lerp(current, target, _time / _targettime));
      tmp.text = _count.ToString();

      _time += Time.deltaTime;
      yield return null;
    }
    tmp.text = exp==null?"": ((int)target).ToString();
  }
  /// <summary>
  /// 선택지
  /// </summary>
  /// <param name="tmp"></param>
  /// <param name="current"></param>
  /// <param name="current"></param>
  /// <param name="colorfunc"></param>
  /// <returns></returns>
  public IEnumerator ChangeCount(TextMeshProUGUI tmp, float current,int require, Func<string,float,string> colorfunc)
  {
    int _startcount = 0;

    if (tmp.text.Contains('<'))
    {
      int _nextindix_0 = tmp.text.IndexOf('>');
      int _nextindex_1 = tmp.text.LastIndexOf('<');
      _startcount = int.Parse(tmp.text.Substring(_nextindix_0+1, _nextindex_1 - _nextindix_0-1)); //text의 시작 값
    }
    //target: text가 바뀔 값
    float _time = 0.0f, _targettime = 0.1f;
    while (_time < _targettime)
    {
      float _current = Mathf.FloorToInt(Mathf.Lerp(_startcount, current, _time / _targettime));
      tmp.text = colorfunc(Mathf.FloorToInt(_current).ToString(), _current / require);

      _time += Time.deltaTime;
      yield return null;
    }
    tmp.text = colorfunc(current.ToString(), current / require);
  }

  public IEnumerator ChangeCount(TextMeshProUGUI tmp, float current, float target, Func<int, string> _coloraction)
  {
    int _count = (int)current;
    float _time = 0.0f, _targettime = CountChangeTime_status;
    while (_time < _targettime)
    {
      _count = Mathf.FloorToInt(Mathf.Lerp(current, target, _time / _targettime));
      tmp.text = _coloraction!=null? _coloraction(_count):_count.ToString();

      _time += Time.deltaTime;
      yield return null;
    }
    tmp.text = _coloraction != null ? _coloraction((int)target) : ((int)target).ToString();
  }
  public  int StatusIconSize_min = 25, StatusIconSize_max = 75;
  [SerializeField] private RectTransform HPUIRect = null;
  [SerializeField] private TextMeshProUGUI HPText = null;
  private int lasthp = -1;
  public void UpdateHPText(int _last)
  {
    if (!lasthp.Equals(-1))
    {
      int _changedvalue = GameManager.Instance.MyGameData.HP - lasthp;
       if(_changedvalue!=0)
       StartCoroutine(statuschangedtexteffect(
         (_changedvalue>0?"<sprite=0>":"<sprite=6>")+ (_changedvalue>0?WNCText.GetHPColor("+"+_changedvalue):WNCText.GetHPColor(_changedvalue)),
         HPUIRect, _changedvalue>0));

      if (lasthp != GameManager.Instance.MyGameData.HP)
      {
        if (lasthp < GameManager.Instance.MyGameData.HP)
        {

        }
        else
        {
          AudioManager.PlaySFX(15, "status");
        }
      }
    }

    HPIcon.rectTransform.sizeDelta = Vector2.one * Mathf.Lerp( StatusIconSize_min, StatusIconSize_max, GameManager.Instance.MyGameData.HP / 100.0f);
    StartCoroutine(ChangeCount(HPText, _last,GameManager.Instance.MyGameData.HP));
  //  Debug.Log("체력 수치 업데이트");

    lasthp = GameManager.Instance.MyGameData.HP;
    UpdateHPIcon();
  }
  public void UpdateHPIcon()
  {
    HPIcon.sprite = GameManager.Instance.MyGameData.MadnessSafe ?
      GameManager.Instance.ImageHolder.HPIcon : GameManager.Instance.ImageHolder.HPBroken;
  }
  [SerializeField] private RectTransform SanityUIRect = null;
  [SerializeField] private TextMeshProUGUI SanityText = null;
  private int lastsanity = -1;
  public void UpdateSanityText(int _last)
  {
    if (!lastsanity.Equals(-1))
    {
      int _changedvalue = GameManager.Instance.MyGameData.Sanity - lastsanity;
      if(_changedvalue!=0)
        StartCoroutine(statuschangedtexteffect(
          (_changedvalue > 0 ? "<sprite=11>" : "<sprite=17>") + (_changedvalue > 0 ? WNCText.GetSanityColor("+" + _changedvalue) : WNCText.GetSanityColor(_changedvalue)),
          SanityUIRect, _changedvalue > 0));

      if (lastsanity != GameManager.Instance.MyGameData.Sanity)
      {
        if (lastsanity < GameManager.Instance.MyGameData.Sanity)
        {
          AudioManager.PlaySFX(16, "status");
        }
        else
        {
          AudioManager.PlaySFX(17, "status");
        }
      }
    }

    SanityIconRect.sizeDelta = Vector2.one * Mathf.Lerp(StatusIconSize_min, StatusIconSize_max, GameManager.Instance.MyGameData.Sanity / 100.0f);
    StartCoroutine(ChangeCount(SanityText, _last, GameManager.Instance.MyGameData.Sanity,
      GameManager.Instance.MyGameData.Sanity>100?WNCText.GetMaxSanityColor:null));

    lastsanity = GameManager.Instance.MyGameData.Sanity;
  }
  [SerializeField] private RectTransform GoldUIRect = null;
  [SerializeField] private TextMeshProUGUI GoldText = null;
  private int lastgold = -1;
  public void UpdateGoldText(int _last)
  {
    if (!lastgold.Equals(-1))
    {
      int _changedvalue = GameManager.Instance.MyGameData.Gold - lastgold;
      if (_changedvalue != 0)
        StartCoroutine(statuschangedtexteffect(
         ( _changedvalue > 0 ? "<sprite=22>" : "<sprite=28>") + (_changedvalue > 0 ? WNCText.GetGoldColor("+" + _changedvalue) : WNCText.GetGoldColor(_changedvalue)),
          GoldUIRect, _changedvalue > 0));

      if (lastgold != GameManager.Instance.MyGameData.Gold)
      {
        if (lastgold < GameManager.Instance.MyGameData.Gold)
        {
          AudioManager.PlaySFX(18, "status");
        }
        else
        {
        }
      }
    }

    StartCoroutine(ChangeCount(GoldText, _last, GameManager.Instance.MyGameData.Gold));
    lastgold = GameManager.Instance.MyGameData.Gold;
  }
  public int SupplyIconMinCount = -8, SupplyIconMaxCount = 30;
  [SerializeField] private RectTransform SupplyUIRect = null;
  [SerializeField] private Image Supply_Icon = null;
  [SerializeField] private TextMeshProUGUI SupplyText = null;
  private int lastsupply = -1;
  public void UpdateSupplyText(int _last)
  {
    if (lastsupply != -1)
    {
      int _changedvalue = GameManager.Instance.MyGameData.Supply - lastsupply;
      if (_changedvalue != 0)
        StartCoroutine(statuschangedtexteffect(
        (  _changedvalue > 0 ? "<sprite=100>" : "<sprite=101>") + (_changedvalue > 0 ? WNCText.GetSupplyColor("+" + _changedvalue) : WNCText.GetSupplyColor(_changedvalue)),
          SupplyUIRect, _changedvalue > 0));
      
      if (lastsupply != GameManager.Instance.MyGameData.Supply)
      {
        if (lastsupply < GameManager.Instance.MyGameData.Supply)
        {
        }
        else
        {
        }
      }
    }

    Supply_Icon.sprite = GameManager.Instance.MyGameData.Supply >0 ? GameManager.Instance.ImageHolder.Supply_Enable : GameManager.Instance.ImageHolder.Supply_Lack;
    if (lastsupply == 0 && GameManager.Instance.MyGameData.Supply == 0) return;

    MovepointIconRect.sizeDelta = Vector2.one * Mathf.Lerp(StatusIconSize_min, StatusIconSize_max,
      (GameManager.Instance.MyGameData.Supply - SupplyIconMinCount) / (float)(SupplyIconMaxCount-SupplyIconMinCount));
    StartCoroutine(ChangeCount(SupplyText, _last, GameManager.Instance.MyGameData.Supply));

    lastsupply = GameManager.Instance.MyGameData.Supply;
  }
  [Space(10)]
  [SerializeField] private float IconMoveTime_using = 1.0f;
  [SerializeField] private float IconMoveTime_gain = 1.0f;
  [SerializeField] private AnimationCurve IconUsingCurve = new AnimationCurve();
  [SerializeField] private AnimationCurve IconGainCurve=new AnimationCurve();
  [SerializeField] private AnimationCurve IconUsingScaleCurve=new AnimationCurve();
  [SerializeField] private List<RectTransform> IconRectList= new List<RectTransform>();
  [SerializeField] private Image HPIcon = null;
  [SerializeField] private RectTransform SanityIconRect = null;
  [SerializeField] private RectTransform GoldIconRect = null;
  [SerializeField] private RectTransform MovepointIconRect = null;
  [SerializeField] private AnimationCurve SkillIconGainCurve = new AnimationCurve();
  [SerializeField] private float SkillIconGainTime = 0.6f;
  public RectTransform ConversationIconRect = null;
  [SerializeField] private RectTransform ForceIconRect = null;
  [SerializeField] private RectTransform WildIconRect = null;
  [SerializeField] private RectTransform IntelligenceIconRect = null;
  [SerializeField] private TextMeshProUGUI ForceMadCountText = null;
  [SerializeField] private TextMeshProUGUI WildMadCountText = null;
  [SerializeField] private float MadCountOpenTime = 0.5f;
  [SerializeField] private float MadCountWaitTime= 1.5f;
  [SerializeField] private float MadCountCloseTime = 0.5f;
  [SerializeField] private AnimationCurve MadCountAnimationCurve = null;
  public void SetForceMadCount()
  {
    ForceMadCountText.text =
      GameManager.Instance.MyGameData.TotalRestCount % GameManager.Instance.Status.MadnessEffect_Force == GameManager.Instance.Status.MadnessEffect_Force - 1 ?
      WNCText.GetMadnessColor((GameManager.Instance.MyGameData.TotalRestCount % GameManager.Instance.Status.MadnessEffect_Force + 1).ToString() + "/" + GameManager.Instance.Status.MadnessEffect_Force.ToString()) :
      ((GameManager.Instance.MyGameData.TotalRestCount % GameManager.Instance.Status.MadnessEffect_Force + 1).ToString() + "/" + GameManager.Instance.Status.MadnessEffect_Force.ToString());
    StartCoroutine(madcounttext(ForceMadCountText.rectTransform));
  }
  public void SetWildMadCount()
  {
    WildMadCountText.text =
      GameManager.Instance.MyGameData.TotalMoveCount % GameManager.Instance.Status.MadnessEffect_Wild_temporary == GameManager.Instance.Status.MadnessEffect_Wild_temporary - 1 ?
      WNCText.GetMadnessColor((GameManager.Instance.MyGameData.TotalMoveCount % GameManager.Instance.Status.MadnessEffect_Wild_temporary + 1).ToString() + "/" + GameManager.Instance.Status.MadnessEffect_Wild_temporary.ToString()) :
      ((GameManager.Instance.MyGameData.TotalMoveCount % GameManager.Instance.Status.MadnessEffect_Wild_temporary + 1).ToString() + "/" + GameManager.Instance.Status.MadnessEffect_Wild_temporary.ToString());
    StartCoroutine(madcounttext(WildMadCountText.rectTransform));
  }
  private IEnumerator madcounttext(RectTransform rect)
  {
    float _time = 0.0f, _targettime = MadCountOpenTime;
    while (_time < _targettime)
    {
      rect.localScale = Vector3.one * MadCountAnimationCurve.Evaluate(_time / _targettime);
      _time += Time.deltaTime; yield return null;
    }
    rect.localScale = Vector3.one;
    yield return new WaitForSeconds(MadCountWaitTime);
    _time = 0.0f; _targettime = MadCountCloseTime;
    while (_time < _targettime)
    {
      rect.localScale = Vector3.one *(1.0f- MadCountAnimationCurve.Evaluate(_time / _targettime));
      _time += Time.deltaTime; yield return null;
    }
    rect.localScale = Vector3.zero;
  }
  private List<int> ActiveIconIndexList= new List<int>();
  [SerializeField] private RectTransform CultSidepanelOpenpos = null;
  /// <summary>
  /// 체,정,골
  /// </summary>
  /// <param name="type"></param>
  /// <param name="rect"></param>
  /// <param name="isusing"></param>
  public IEnumerator SetIconEffect(bool isusing,StatusTypeEnum status,RectTransform rect)
  {
    Vector2 _startpos=Vector2.zero, _endpos=Vector2.zero;
    Sprite _icon = null;
    switch(status)
    {
      case StatusTypeEnum.HP: 
        _icon = isusing ? GameManager.Instance.ImageHolder.HPDecreaseIcon : GameManager.Instance.ImageHolder.HPIcon;
        _startpos = isusing ? HPIcon.rectTransform.position : rect.position;
        _endpos=isusing?rect.position : HPIcon.rectTransform.position;
        break;
      case StatusTypeEnum.Sanity:
        _icon = isusing ? GameManager.Instance.ImageHolder.SanityDecreaseIcon : GameManager.Instance.ImageHolder.SanityIcon;
        _startpos = isusing ? SanityIconRect.position : rect.position;
        _endpos = isusing ? rect.position : SanityIconRect.position;
        break;
      case StatusTypeEnum.Gold:
        _icon = isusing ? GameManager.Instance.ImageHolder.GoldDecreaseIcon : GameManager.Instance.ImageHolder.GoldIcon;
        _startpos = isusing ? GoldIconRect.position : rect.position;
        _endpos = isusing ? rect.position : GoldIconRect.position;
        break;
    }
    int _index = 0;
    RectTransform _iconrect = null;
    for(int i=0;i<IconRectList.Count;i++)
    {
      _index = i;
      if (ActiveIconIndexList.Contains(i)) continue;

      _iconrect = IconRectList[_index];
      _iconrect.GetComponent<Image>().sprite = _icon;
      _iconrect.localScale = Vector3.one;
      ActiveIconIndexList.Add(_index);
      break;
    }
    
    float _time = 0.0f;
    AnimationCurve _curve = isusing ? IconUsingCurve : IconGainCurve;
    float _targettime = isusing ? IconMoveTime_using : IconMoveTime_gain;
    while (_time < _targettime)
    {

      _iconrect.position = Vector3.Lerp(_startpos, _endpos, _curve.Evaluate(_time / _targettime));
      _iconrect.anchoredPosition3D = new Vector3(_iconrect.anchoredPosition3D.x, _iconrect.anchoredPosition3D.y, 0.0f);
      if(isusing)_iconrect.localScale=Vector3.one*(1.0f-IconUsingScaleCurve.Evaluate(_time/ _targettime));

      _time += Time.deltaTime;
      yield return null;
    }
    _iconrect.anchoredPosition = Vector2.one * 3000.0f;
    ActiveIconIndexList.Remove(_index);
  }
  
  public IEnumerator SetIconEffect(SkillTypeEnum skilltype)
  {
    RectTransform _targetrect = null;
    switch (skilltype)
    {
      case SkillTypeEnum.Conversation:
        _targetrect = ConversationIconRect;
        break;
      case SkillTypeEnum.Force:
        _targetrect = ForceIconRect;
        break;
      case SkillTypeEnum.Wild:
        _targetrect = WildIconRect;
        break;
      case SkillTypeEnum.Intelligence:
        _targetrect = IntelligenceIconRect;
        break;
    }
    float _time = 0.0f, _targettime = SkillIconGainTime;
    while (_time < _targettime)
    {
      _targetrect.localScale=Vector3.one*Mathf.Lerp(1.0f,GameManager.Instance.Status.StatusHighlightSize,SkillIconGainCurve.Evaluate(_time/ _targettime));
      _time += Time.deltaTime;
      yield return null;
    }
    _targetrect.localScale = Vector3.one;
  }
  /// <summary>
  /// 성향
  /// </summary>
  /// <param name="tendencytype"></param>
  /// <param name="dir"></param>
  /// <param name="rect"></param>
  /// <returns></returns>
  public IEnumerator SetIconEffect(TendencyTypeEnum tendencytype,bool dir,RectTransform rect)
  {
    Vector2 _startpos = rect.position, _endpos = Vector2.zero;
    int _level = 0;
    Sprite _icon = GameManager.Instance.ImageHolder.GetTendencyIcon(tendencytype, dir ? -1 : 1);

    switch (tendencytype)
    {
      case TendencyTypeEnum.Body:
        _level = GameManager.Instance.MyGameData.Tendency_Body.Level;
        break;
      case TendencyTypeEnum.Head:
        _level = GameManager.Instance.MyGameData.Tendency_Head.Level;
        break;
    }
    int _targetlevel = 0;
    switch (_level)
    {
      case -2:
        _targetlevel = dir ? -2 : -1;
            break;
      case -1:
        _targetlevel = dir ? -2 : 1;
        break;
      case 1:
        _targetlevel = dir ? -1 : 2;
        break;
      case 2:
        _targetlevel = dir ? 1 : 2;
        break;
    }
    _endpos = GetTendencyRectPos(tendencytype, _targetlevel).position;

    int _index = 0;
    RectTransform _iconrect = null;
    for (int i = 0; i < IconRectList.Count; i++)
    {
      _index = i;
      if (ActiveIconIndexList.Contains(i)) continue;

      _iconrect = IconRectList[_index];
      _iconrect.GetComponent<Image>().sprite = _icon;
      _iconrect.localScale = Vector3.one;
      ActiveIconIndexList.Add(_index);
      break;
    }

    float _time = 0.0f;
    AnimationCurve _curve =IconGainCurve;
    while (_time < IconMoveTime_using)
    {

      _iconrect.position = Vector3.Lerp(_startpos, _endpos, _curve.Evaluate(_time / IconMoveTime_using));
      _iconrect.anchoredPosition3D = new Vector3(_iconrect.anchoredPosition3D.x, _iconrect.anchoredPosition3D.y, 0.0f);

      _time += Time.deltaTime;
      yield return null;
    }
    _iconrect.anchoredPosition = Vector2.one * 3000.0f;
    ActiveIconIndexList.Remove(_index);
  }
  public IEnumerator SetIconEffect_movepoint_gain(RectTransform startrect,int count)
  {
    Vector2 _startpos = startrect.position;
    Vector2 _endpos = MovepointIconRect.position;
    Sprite _icon = GameManager.Instance.ImageHolder.Supply_Enable;
    for(int i=0;i< count;i++)
    {
      int _index = 0;
      RectTransform _rect = null;
      for(int j=0;j<IconRectList.Count;j++)
      {
        _index = j;
        if (ActiveIconIndexList.Contains(j)) continue;

        _rect = IconRectList[_index];
        _rect.GetComponent<Image>().sprite = _icon;
        _rect.localScale = Vector3.one;
        ActiveIconIndexList.Add(_index);
        break;
      }
      StartCoroutine(iconmove_movepoint(_index, _rect, _startpos, _endpos, IconUsingCurve));
      yield return new WaitForSeconds(0.08f);
    }
  }
  public void SetIconEffect_movepoint_using(RectTransform rect,StatusTypeEnum status)
  {
    Vector2 _startpos = 
      status==StatusTypeEnum.HP?      MovepointIconRect.position:
      status==StatusTypeEnum.Sanity?  SanityIconRect.position:
                                      GoldIconRect.position,
      _endpos = rect.position;
    Sprite _icon = GameManager.Instance.ImageHolder.Supply_Enable;

    int _index = 0;
    _icon = status==StatusTypeEnum.HP ? GameManager.Instance.ImageHolder.Supply_Enable :
      status == StatusTypeEnum.Sanity ? GameManager.Instance.ImageHolder.SanityIcon :
                                        GameManager.Instance.ImageHolder.GoldIcon;
    RectTransform _targetrect = null;
    for (int i = 0; i < IconRectList.Count; i++)
    {
      _index = i;
      if (ActiveIconIndexList.Contains(i)) continue;

      _targetrect = IconRectList[_index];
      _targetrect.GetComponent<Image>().sprite = _icon;
      _targetrect.localScale = Vector3.one;
      ActiveIconIndexList.Add(_index);
      break;
    }

    StartCoroutine(iconmove_movepoint(_index, _targetrect, _startpos, _endpos, IconUsingCurve));
  }
  public IEnumerator SetIconEffect_movepoint_ritualfail(RectTransform endrect,int count)
  {
    Vector2 _startpos = MovepointIconRect.position, _endpos = endrect.position;
    Sprite _icon = GameManager.Instance.ImageHolder.Supply_Enable;

    for (int j = 0; j < count; j++)
    {
      int _index = 0;
      RectTransform _targetrect = null;
      for (int i = 0; i < IconRectList.Count; i++)
      {
        _index = i;
        if (ActiveIconIndexList.Contains(i)) continue;

        _targetrect = IconRectList[_index];
        _targetrect.GetComponent<Image>().sprite = _icon;
        _targetrect.localScale = Vector3.one;
        ActiveIconIndexList.Add(_index);
        break;
      }

      StartCoroutine(iconmove_movepoint(_index, _targetrect, _startpos, _endpos, IconUsingCurve));
      yield return new WaitForSeconds(0.15f);
    }
  }
  private IEnumerator iconmove_movepoint(int index,RectTransform rect ,Vector2 startpos,Vector2 endpos,AnimationCurve curve)
  {
    AudioManager.PlaySFX(30,"movecost");
    float _time = 0.0f;
    float _targettime =  IconMoveTime_using;
    while (_time < _targettime)
    {
      rect.position = Vector3.Lerp(startpos, endpos, curve.Evaluate(_time / _targettime));
      rect.anchoredPosition3D = new Vector3(rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, 0.0f);
      rect.localScale = Vector3.one * (1.0f - IconUsingScaleCurve.Evaluate(_time / _targettime));

      _time += Time.deltaTime;
      yield return null;
    }
    rect.anchoredPosition = Vector2.one * 3000.0f;
    ActiveIconIndexList.Remove(index);

  }
  /// <summary>
  /// 불쾌(정착지)
  /// </summary>
  /// <param name="count"></param>
  /// <param name="buttonrect"></param>
  /// <param name="discomfortrect"></param>
  /// <returns></returns>
  public IEnumerator SetIconEffect_Discomfort(int count, RectTransform buttonrect,RectTransform discomfortrect)
  {
    Vector2 _startpos = buttonrect.position, _endpos = discomfortrect.position;
    Sprite _icon = GameManager.Instance.ImageHolder.DisComfort;

    for (int j = 0; j < count; j++)
    {
      int _index = 0;
      RectTransform _targetrect = null;
      for (int i = 0; i < IconRectList.Count; i++)
      {
        _index = i;
        if (ActiveIconIndexList.Contains(i)) continue;

        _targetrect = IconRectList[_index];
        _targetrect.GetComponent<Image>().sprite = _icon;
        _targetrect.localScale = Vector3.one;
        ActiveIconIndexList.Add(_index);
        break;
      }
      if (j != count - 1)
      {
        StartCoroutine(iconmove_discomfort(_index, _targetrect, _startpos, _endpos));
        yield return new WaitForSeconds(0.1f);
      }
      else
      {
        yield return StartCoroutine(iconmove_discomfort(_index, _targetrect, _startpos, _endpos));
      }
    }
  }
  private IEnumerator iconmove_discomfort(int index, RectTransform rect, Vector2 startpos, Vector2 endpos)
  {
    float _time = 0.0f;
    AnimationCurve _curve = IconGainCurve;
    while (_time < IconMoveTime_gain)
    {
      rect.position = Vector3.Lerp(startpos, endpos, _curve.Evaluate(_time / IconMoveTime_gain));
      rect.anchoredPosition3D = new Vector3(rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, 0.0f);

      _time += Time.deltaTime;
      yield return null;
    }
    rect.anchoredPosition = Vector2.one * 3000.0f;
    ActiveIconIndexList.Remove(index);

  }
  [SerializeField] private RectTransform SliderIconRect = null;

  public void CultEventProgressIconMove(Sprite icon, RectTransform startrect) => StartCoroutine(culteventprogressiconmove(icon, startrect));
  private IEnumerator culteventprogressiconmove(Sprite icon,RectTransform startrect)
  {
    int _index = 0;
    RectTransform _targetrect = null;
    for (int i = 0; i < IconRectList.Count; i++)
    {
      _index = i;
      if (ActiveIconIndexList.Contains(i)) continue;

      _targetrect = IconRectList[_index];
      _targetrect.GetComponent<Image>().sprite = icon;
      _targetrect.localScale = Vector3.one;
      ActiveIconIndexList.Add(_index);
      break;
    }
    float _time = 0.0f;
    AnimationCurve _curve = IconUsingCurve;
    Vector2 _startpos = startrect.position, _endpos = SliderIconRect.position;
    while (_time < IconMoveTime_using)
    {
      _targetrect.position = Vector3.Lerp(_startpos, _endpos, _curve.Evaluate(_time / IconMoveTime_using));
      _targetrect.anchoredPosition3D = new Vector3(_targetrect.anchoredPosition3D.x, _targetrect.anchoredPosition3D.y, 0.0f);
      _targetrect.localScale = Vector3.one * (1.0f - IconUsingScaleCurve.Evaluate(_time / IconMoveTime_using));
      _time += Time.deltaTime;
      yield return null;
    }
    _targetrect.anchoredPosition = Vector2.one * 3000.0f;
    ActiveIconIndexList.Remove(_index);

    SidePanelCultUI.UpdateProgressValue();
  }

  [Space(10)]
  [SerializeField] private RectTransform StatusRect = null;
  [SerializeField] private float StatusTextMovetime_gain = 1.0f;
  [SerializeField] private float StatusTextMovetime_loss = 1.0f;
  [SerializeField] private Vector3 StatusTextEffectPos_gain_top = new Vector3(0.0f, 50.0f);
  [SerializeField] private Vector3 StatusTextEffectPos_gain_bottom = new Vector3(0.0f, 30.0f);
  [SerializeField] private Vector3 StatusTextEffectPos_loss_top = new Vector3(0.0f, -50.0f);
  [SerializeField] private Vector3 StatusTextEffectPos_loss_bottom = new Vector3(0.0f, -100.0f);
  [SerializeField] private GameObject StatusTextPrefab = null;
  [SerializeField] private AnimationCurve StatusTextEffectCurve = new AnimationCurve();
  private IEnumerator statuschangedtexteffect(string value,RectTransform targetrect, bool isgain)
  {
    float _time = 0.0f, _targettime = isgain?StatusTextMovetime_gain:StatusTextMovetime_loss;
    GameObject _prefab=Instantiate(StatusTextPrefab,StatusRect);
    _prefab.GetComponent<TextMeshProUGUI>().text = value;
    RectTransform _rect = _prefab.GetComponent<RectTransform>();

    Vector3 _startpos = targetrect.anchoredPosition3D + (isgain ? StatusTextEffectPos_gain_bottom : StatusTextEffectPos_loss_top);
    Vector3 _endpos = targetrect.anchoredPosition3D + (isgain ? StatusTextEffectPos_gain_top: StatusTextEffectPos_loss_bottom);
    while (_time < _targettime)
    {
      _rect.anchoredPosition = Vector3.Lerp(_startpos, _endpos, StatusTextEffectCurve.Evaluate(_time / _targettime));
      _time += Time.deltaTime;
      yield return null;
    }
    _rect.anchoredPosition = _endpos;
    yield return StartCoroutine(ChangeAlpha(_prefab.GetComponent<CanvasGroup>(),0.0f,0.2f));
    Destroy(_prefab);
  }
  [Space(5)]
  private int conversationlevel = -1;
  private int forcelevel = -1;
  private int wildlevel = -1;
  private int intelligencelevel = -1;
  [SerializeField] private TextMeshProUGUI ConversationLevel = null;
  [SerializeField] private TextMeshProUGUI ForceLevel = null;
  [SerializeField] private TextMeshProUGUI WildLevel = null;
  [SerializeField] private TextMeshProUGUI IntelligenceLevel = null;
  [SerializeField] private CanvasGroup ConversationEffectGroup = null;
  [SerializeField] private CanvasGroup ForceEffectGroup = null;
  [SerializeField] private CanvasGroup WildEffectGroup = null;
  [SerializeField] private CanvasGroup IntelligenceEffectGroup = null;
  [SerializeField] private Color MadnessColor = new Color();
  [SerializeField] private Color IdleColor = Color.white;
  public void UpdateSkillLevel()
  {
    ConversationLevel.text = GameManager.Instance.MyGameData.Madness_Conversation ?
      WNCText.GetMadnessColor(GameManager.Instance.MyGameData.Skill_Conversation.Level) :
      WNCText.UIIdleColor(GameManager.Instance.MyGameData.Skill_Conversation.Level);
    ConversationIconRect.transform.GetComponent<Image>().color = GameManager.Instance.MyGameData.Madness_Conversation ?
      MadnessColor : IdleColor;

    if (conversationlevel != -1)
    {
      if (conversationlevel != GameManager.Instance.MyGameData.Skill_Conversation.Level)
      {
        StartCoroutine(ChangeAlpha(ConversationEffectGroup, 0.0f, ExpGainTime));
        conversationlevel = GameManager.Instance.MyGameData.Skill_Conversation.Level;
      }
    }

    ForceLevel.text = GameManager.Instance.MyGameData.Madness_Force ?
      WNCText.GetMadnessColor(GameManager.Instance.MyGameData.Skill_Force.Level) :
      WNCText.UIIdleColor(GameManager.Instance.MyGameData.Skill_Force.Level);
    ForceIconRect.transform.GetComponent<Image>().color = GameManager.Instance.MyGameData.Madness_Force ?
   MadnessColor : IdleColor;
    if (forcelevel != -1)
    {
      if (forcelevel != GameManager.Instance.MyGameData.Skill_Force.Level)
      {
        StartCoroutine(ChangeAlpha(ForceEffectGroup, 0.0f, ExpGainTime));
        forcelevel = GameManager.Instance.MyGameData.Skill_Force.Level;
      }
    }

    WildLevel.text = GameManager.Instance.MyGameData.Madness_Wild ?
      WNCText.GetMadnessColor(GameManager.Instance.MyGameData.Skill_Wild.Level) :
      WNCText.UIIdleColor(GameManager.Instance.MyGameData.Skill_Wild.Level);
    WildIconRect.transform.GetComponent<Image>().color = GameManager.Instance.MyGameData.Madness_Wild ?
    MadnessColor : IdleColor;
    if (wildlevel != -1)
    {
      if (wildlevel != GameManager.Instance.MyGameData.Skill_Wild.Level)
      {
        StartCoroutine(ChangeAlpha(WildEffectGroup, 0.0f, ExpGainTime));
        wildlevel = GameManager.Instance.MyGameData.Skill_Wild.Level;
      }
    }

    IntelligenceLevel.text = GameManager.Instance.MyGameData.Madness_Intelligence ?
     WNCText.GetMadnessColor(GameManager.Instance.MyGameData.Skill_Intelligence.Level) :
     WNCText.UIIdleColor(GameManager.Instance.MyGameData.Skill_Intelligence.Level);
    IntelligenceIconRect.transform.GetComponent<Image>().color = GameManager.Instance.MyGameData.Madness_Intelligence ?
    MadnessColor : IdleColor;
    if (intelligencelevel != -1)
    {
      if (intelligencelevel != GameManager.Instance.MyGameData.Skill_Intelligence.Level)
      {
        StartCoroutine(ChangeAlpha(IntelligenceEffectGroup, 0.0f, ExpGainTime));
        intelligencelevel = GameManager.Instance.MyGameData.Skill_Intelligence.Level;
      }
    }
  }

  [Space(5)]
  [SerializeField] private RectTransform TendencyBodyRect = null;
  [SerializeField] private Image TendencyBodyIcon = null;
  [SerializeField] private CanvasGroup TendencyBodyBackground = null;
  [SerializeField] private RectTransform TendencyHeadRect = null;
  [SerializeField] private CanvasGroup TendencyHeadBackbround = null;
  [SerializeField] private Image TendencyHeadIcon = null;
  [SerializeField] private RectTransform TendencyPos_body_m2 = null;
  [SerializeField] private RectTransform TendencyPos_body_m1 = null;
  [SerializeField] private RectTransform TendencyPos_body_p1 = null;
  [SerializeField] private RectTransform TendencyPos_body_p2 = null;
  [SerializeField] private RectTransform TendencyPos_head_m2 = null;
  [SerializeField] private RectTransform TendencyPos_head_m1 = null;
  [SerializeField] private RectTransform TendencyPos_head_p1 = null;
  [SerializeField] private RectTransform TendencyPos_head_p2 = null;
  private RectTransform GetTendencyRectPos(TendencyTypeEnum type, int level)
  {
    switch (level)
    {
      case -2:return type==TendencyTypeEnum.Body?TendencyPos_body_m2:TendencyPos_head_m2;
      case -1:return type == TendencyTypeEnum.Body ? TendencyPos_body_m1 : TendencyPos_head_m1;
      case 1:return type == TendencyTypeEnum.Body ? TendencyPos_body_p1 : TendencyPos_head_p1;
      case 2: return type == TendencyTypeEnum.Body ? TendencyPos_body_p2 : TendencyPos_head_p2;
    }
    return null;
  }
  public void UpdateTendencyIcon()
  {
    if (GameManager.Instance.MyGameData.Tendency_Body.Level != 0 && TendencyBodyBackground.alpha==0.0f)
    {
      StartCoroutine(ChangeAlpha(TendencyBodyBackground, 1.0f, 0.5f));
    }
    if (GameManager.Instance.MyGameData.Tendency_Head.Level != 0 && TendencyHeadBackbround.alpha == 0.0f)
    {
      StartCoroutine(ChangeAlpha(TendencyHeadBackbround, 1.0f, 0.5f));
    }

    Vector2 _bodypos = GameManager.Instance.MyGameData.Tendency_Body.Level != 0 ?
      GetTendencyRectPos(TendencyTypeEnum.Body, GameManager.Instance.MyGameData.Tendency_Body.Level).anchoredPosition :
      Vector2.zero;
    Vector2 _headpos = GameManager.Instance.MyGameData.Tendency_Head.Level!=0?
      GetTendencyRectPos(TendencyTypeEnum.Head, GameManager.Instance.MyGameData.Tendency_Head.Level).anchoredPosition:
      Vector2.zero;
    TendencyBodyIcon.sprite = GameManager.Instance.ImageHolder.GetTendencyIcon(TendencyTypeEnum.Body, GameManager.Instance.MyGameData.Tendency_Body.Level);
    TendencyHeadIcon.sprite = GameManager.Instance.ImageHolder.GetTendencyIcon(TendencyTypeEnum.Head, GameManager.Instance.MyGameData.Tendency_Head.Level);

    StartCoroutine(movetendencyrect(TendencyBodyRect,  _bodypos));
    StartCoroutine(movetendencyrect(TendencyHeadRect, _headpos));
  }
  private IEnumerator movetendencyrect(RectTransform titlerect,Vector2 targetpos)
  {
    if (titlerect.anchoredPosition == targetpos) yield break;

    Vector2 _originpos_title = titlerect.anchoredPosition;
    Vector2 _targetpos_title = targetpos;
    float _time = 0.0f, _targettime = 0.4f;

    while (_time < _targettime)
    {
      titlerect.anchoredPosition = Vector2.Lerp(_originpos_title, _targetpos_title,IconGainCurve.Evaluate(_time / _targettime));

      _time += Time.deltaTime; yield return null;
    }

    titlerect.anchoredPosition = _targetpos_title;
  }
  [SerializeField] private Button LongExpButton = null;
  [SerializeField] private RectTransform LongExpCover = null;
  [SerializeField] private TextMeshProUGUI LongExpTurn = null;
  [SerializeField] private CanvasGroup LongMad = null;
  [SerializeField] private CanvasGroup ExpUse_Long_Group = null;
  [SerializeField] private Image ExpUse_Long_img = null;
  [SerializeField] private TextMeshProUGUI ExpUse_Long_text = null;
  [SerializeField] private CanvasGroup Exp_Long_Group = null;
  private bool LongExpActive = false;
  [SerializeField] private Button ShortExpButton_A = null;
  [SerializeField] private RectTransform ShortExpCover_A = null;
  [SerializeField] private TextMeshProUGUI ShortExpTurn_A= null;
  [SerializeField] private CanvasGroup ShortMad_A = null;
  [SerializeField] private CanvasGroup ExpUse_Short_A_Group = null;
  [SerializeField] private Image ExpUse_Short_A_img = null;
  [SerializeField] private TextMeshProUGUI ExpUse_Short_A_text = null;
  [SerializeField] private CanvasGroup Exp_Short_A_Group = null;
  private bool ShortExpAActive = false;
  [SerializeField] private Button ShortExpButton_B = null;
  [SerializeField] private RectTransform ShortExpCover_B = null;
  [SerializeField] private TextMeshProUGUI ShortExpTurn_B = null;
  [SerializeField] private CanvasGroup ShortMad_B = null;
  [SerializeField] private CanvasGroup ExpUse_Short_B_Group = null;
  [SerializeField] private Image ExpUse_Short_B_img = null;
  [SerializeField] private TextMeshProUGUI ExpUse_Short_B_text = null;
  [SerializeField] private CanvasGroup Exp_Short_B_Group = null;
  private bool ShortExpBActive = false;
  [SerializeField] private Color ExpUseRefuseColor = new Color();
  [SerializeField] private float UsedTextWarningSize = 1.4f;
  [SerializeField] private float UsedTextWarningTime = 0.5f;
  [SerializeField] private float ExpUsingTime = 0.6f;
  private float ExpGroupAlpha_disable = 0.25f;
  private float ExpGroupAlpha_enable = 1.0f;
  public void SetExpUse(List<SelectionData> datas)
  {
    List<EffectType> _effects= new List<EffectType>();
    foreach(var _data in datas)
    {
      switch (_data.ThisSelectionType)
      {
        case SelectionTargetType.None:
          break;
        case SelectionTargetType.Pay:
          /*
          switch (_data.SelectionPayTarget)
          {
            case StatusTypeEnum.HP:
              _effects.Add(EffectType.HPLoss);
              break;
            case StatusTypeEnum.Sanity:
              _effects.Add(EffectType.SanityLoss);
              break;
            case StatusTypeEnum.Gold:
              break;
          }
          */
          break;
        case SelectionTargetType.Check_Single:
          switch (_data.SelectionCheckSkill[0])
          {
            case SkillTypeEnum.Conversation:
              _effects.Add(EffectType.Conversation);
              break;
            case SkillTypeEnum.Force:
              _effects.Add(EffectType.Force);
              break;
            case SkillTypeEnum.Wild:
              _effects.Add(EffectType.Wild);
              break;
            case SkillTypeEnum.Intelligence:
              _effects.Add(EffectType.Intelligence);
              break;
          }
          break;
        case SelectionTargetType.Check_Multy:
          foreach(var _skill in _data.SelectionCheckSkill)
          {
            switch (_skill)
            {
              case SkillTypeEnum.Conversation:
                _effects.Add(EffectType.Conversation);
                break;
              case SkillTypeEnum.Force:
                _effects.Add(EffectType.Force);
                break;
              case SkillTypeEnum.Wild:
                _effects.Add(EffectType.Wild);
                break;
              case SkillTypeEnum.Intelligence:
                _effects.Add(EffectType.Intelligence);
                break;
            }
          }
          break;
      }
    }

    foreach(var _effect in _effects)
    {
      if (GameManager.Instance.MyGameData.LongExp != null &&
        GameManager.Instance.MyGameData.LongExp.Duration > 1&&
        GameManager.Instance.MyGameData.LongExp.Effects.Contains(_effect) &&
        Exp_Long_Group.alpha == ExpGroupAlpha_disable)
      {
        Exp_Long_Group.alpha = ExpGroupAlpha_enable;
        LongExpButton.interactable = true;
      }
      if (GameManager.Instance.MyGameData.ShortExp_A != null &&
        GameManager.Instance.MyGameData.ShortExp_A.Duration > 1 &&
     GameManager.Instance.MyGameData.ShortExp_A.Effects.Contains(_effect) &&
     Exp_Short_A_Group.alpha == ExpGroupAlpha_disable)
      {
        Exp_Short_A_Group.alpha = ExpGroupAlpha_enable;
        ShortExpButton_A.interactable = true;
      }
      if (GameManager.Instance.MyGameData.ShortExp_B != null &&
        GameManager.Instance.MyGameData.ShortExp_B.Duration > 1 &&
        GameManager.Instance.MyGameData.ShortExp_B.Effects.Contains(_effect) &&
        Exp_Short_B_Group.alpha == ExpGroupAlpha_disable)
      {
        Exp_Short_B_Group.alpha = ExpGroupAlpha_enable;
        ShortExpButton_B.interactable = true;
      }
    }
  }
  public void SetExpUnuse()
  {
    Exp_Long_Group.alpha = ExpGroupAlpha_disable;
    Exp_Short_A_Group.alpha = ExpGroupAlpha_disable;
    Exp_Short_B_Group.alpha = ExpGroupAlpha_disable;
  }
  public void UseExp(bool dir)
  {
    bool _anyexpused = false;
    if (GameManager.Instance.MyGameData.LongExp != null)
    {
      Experience _long = GameManager.Instance.MyGameData.LongExp;
      if (dir&&DialogueUI.ExpUsageDic_L.ContainsKey(_long))
      {
        StartCoroutine(useexp(ExpUse_Long_Group, LongExpButton.GetComponent<RectTransform>(),_long, DialogueUI.ExpUsageDic_L[_long]));
        _anyexpused = true;
      }
      else if (!dir&&DialogueUI.ExpUsageDic_R.ContainsKey(_long))
      {
        StartCoroutine(useexp(ExpUse_Long_Group, LongExpButton.GetComponent<RectTransform>(), _long, DialogueUI.ExpUsageDic_R[_long]));
        _anyexpused = true;
      }
      else if (ExpUse_Long_Group.alpha == 1.0f)
      {
        StartCoroutine(ChangeAlpha(ExpUse_Long_Group, 0.0f, 0.4f));
      }
    }
    if (GameManager.Instance.MyGameData.ShortExp_A != null)
    {
      Experience _exp = GameManager.Instance.MyGameData.ShortExp_A;
      if (dir && DialogueUI.ExpUsageDic_L.ContainsKey(_exp))
      {
        StartCoroutine(useexp(ExpUse_Short_A_Group, ShortExpButton_A.GetComponent<RectTransform>(), _exp, DialogueUI.ExpUsageDic_L[_exp]));
        _anyexpused = true;
      }
      else if (!dir && DialogueUI.ExpUsageDic_R.ContainsKey(_exp))
      {
        StartCoroutine(useexp(ExpUse_Short_A_Group, ShortExpButton_A.GetComponent<RectTransform>(), _exp, DialogueUI.ExpUsageDic_R[_exp]));
        _anyexpused = true;
      }
      else if (ExpUse_Short_A_Group.alpha == 1.0f)
      {
        StartCoroutine(ChangeAlpha(ExpUse_Short_A_Group, 0.0f, 0.4f));
      }
    }
    if (GameManager.Instance.MyGameData.ShortExp_B != null)
    {
      Experience _exp = GameManager.Instance.MyGameData.ShortExp_B;
      if (dir && DialogueUI.ExpUsageDic_L.ContainsKey(_exp))
      {
        StartCoroutine(useexp(ExpUse_Short_B_Group, ShortExpButton_B.GetComponent<RectTransform>(), _exp, DialogueUI.ExpUsageDic_L[_exp]));
        _anyexpused = true;
      }
      else if (!dir && DialogueUI.ExpUsageDic_R.ContainsKey(_exp))
      {
        StartCoroutine(useexp(ExpUse_Short_B_Group, ShortExpButton_B.GetComponent<RectTransform>(), _exp, DialogueUI.ExpUsageDic_R[_exp]));
        _anyexpused = true;
      }
      else if (ExpUse_Short_B_Group.alpha == 1.0f)
      {
        StartCoroutine(ChangeAlpha(ExpUse_Short_B_Group, 0.0f, 0.4f));
      }
    }
    if (_anyexpused)
    {
      StartCoroutine(doitlater());
    }
  }
  private IEnumerator doitlater()
  {
    yield return new WaitForSeconds(ExpUsingTime);
    UpdateExpPanel();
  }
  private IEnumerator useexp(CanvasGroup group,RectTransform targetrect,Experience exp,int duration)
  {
    RectTransform _rect = group.GetComponent<RectTransform>();
    Vector2 _startpos = _rect.anchoredPosition, _endpos = targetrect.anchoredPosition;
    float _time = 0.0f, _targettime = ExpUsingTime;
    while(_time< _targettime)
    {
      _rect.anchoredPosition=Vector2.Lerp(_startpos,_endpos,_time/_targettime);
      group.alpha=Mathf.Lerp(1.0f,0.0f,_time/_targettime);
      _time += Time.deltaTime; yield return null;
    }
    group.alpha = 0.0f;
    _rect.anchoredPosition = _startpos;
    exp.Duration -= duration;
  }
  public void UpdateExpButton(bool isactive)
  {
    LongExpButton.interactable = GameManager.Instance.MyGameData.LongExp != null ? GameManager.Instance.MyGameData.LongExp.Duration > 1 ? isactive : false:false ;
    ShortExpButton_A.interactable = GameManager.Instance.MyGameData.ShortExp_A != null ? GameManager.Instance.MyGameData.ShortExp_A.Duration>1?isactive : false:false;
    ShortExpButton_B.interactable = GameManager.Instance.MyGameData.ShortExp_B != null ? GameManager.Instance.MyGameData.ShortExp_B.Duration>1? isactive : false:false;
  }
  public void ExpUsingWarning(Experience exp)
  {
    TextMeshProUGUI _targettext =
      GameManager.Instance.MyGameData.LongExp == exp ? ExpUse_Long_text :
      GameManager.Instance.MyGameData.ShortExp_A == exp ? ExpUse_Short_A_text : ExpUse_Short_B_text;
    StartCoroutine(expusingwarning(_targettext));
  }
  private IEnumerator expusingwarning(TextMeshProUGUI tmp)
  {
    float _time = 0.0f;
    while(_time < UsedTextWarningTime)
    {
      tmp.rectTransform.localScale = Vector3.one * Mathf.Lerp(UsedTextWarningSize, 1.0f, _time / UsedTextWarningTime);
      tmp.color=Color.Lerp(ExpUseRefuseColor,Color.white, _time / UsedTextWarningTime);
      _time += Time.deltaTime; yield return null;
    }
    tmp.rectTransform.localScale = Vector3.one;
    tmp.color = Color.white;
  }
  public Vector2 ExpCoverUpPos = new Vector2(0.0f, 48.0f);
  /// <summary>
  /// long A B
  /// </summary>
  /// <param name="index"></param>
  public void UpdateExpMad(int index)
  {
    switch (index)
    {
      case 0:
        StartCoroutine(ChangeAlpha(LongMad, 0.0f, 1.0f));
        break;
      case 1:
        StartCoroutine(ChangeAlpha(ShortMad_A, 0.0f, 1.0f));
        break;
      case 2:
        StartCoroutine(ChangeAlpha(ShortMad_B, 0.0f, 1.0f));
        break;
    }
  }
  private IEnumerator longexpcoroutine = null;
  private IEnumerator shortexpAcoroutine = null;
  private IEnumerator shortexpBcoroutine = null;
  public void UpdateExpPanel()
  {
    bool _starteffect = false;
    bool _turnchanged = false;

    if (GameManager.Instance.MyGameData.LongExp == null)
    {
      LongExpTurn.text = "";

      if (LongExpActive == true)
      {
        StartCoroutine(moverect(LongExpCover, ExpCoverUpPos, Vector2.zero, 0.4f, UIPanelCLoseCurve));
        changecount(longexpcoroutine, LongExpTurn, GameManager.Instance.MyGameData.LongExp);
        _starteffect = true;
        AudioManager.PlaySFX(21);
      }
      LongExpActive = false;
    }
    else
    {
      if (LongExpActive == false)
      {
        StartCoroutine(moverect(LongExpCover, Vector2.zero, ExpCoverUpPos, 0.4f, UIPanelOpenCurve));
        _starteffect = true;
        StartCoroutine(ExpGainCount(LongExpTurn.rectTransform));
        changecount(longexpcoroutine,LongExpTurn, GameManager.Instance.MyGameData.LongExp);
        AudioManager.PlaySFX(20);
      }
      else
      {
        _turnchanged = int.Parse(LongExpTurn.text) != GameManager.Instance.MyGameData.LongExp.Duration;
        if (_turnchanged)
        {
          StartCoroutine(ExpLossCount(LongExpTurn.rectTransform));
          changecount(longexpcoroutine, LongExpTurn, GameManager.Instance.MyGameData.LongExp);
        }
      }
      LongExpActive = true;
    }

    if (GameManager.Instance.MyGameData.ShortExp_A == null)
    {
      ShortExpTurn_A.text = "";

      if (ShortExpAActive == true)
      {
        StartCoroutine(moverect(ShortExpCover_A, ExpCoverUpPos, Vector2.zero, 0.4f, UIPanelCLoseCurve));
        changecount(shortexpAcoroutine, ShortExpTurn_A, GameManager.Instance.MyGameData.ShortExp_A);
        _starteffect = true;
        AudioManager.PlaySFX(21);
      }
      ShortExpAActive = false;
    }
    else
    {
      if (ShortExpAActive == false)
      {
        StartCoroutine(moverect(ShortExpCover_A, Vector2.zero, ExpCoverUpPos, 0.4f, UIPanelOpenCurve));
        _starteffect = true;
        StartCoroutine(ExpGainCount(ShortExpTurn_A.rectTransform));
        changecount(shortexpAcoroutine, ShortExpTurn_A, GameManager.Instance.MyGameData.ShortExp_A);
        AudioManager.PlaySFX(20);
      }
      else
      {
        _turnchanged = int.Parse(ShortExpTurn_A.text) != GameManager.Instance.MyGameData.ShortExp_A.Duration;
        if (_turnchanged)
        {
          changecount(shortexpAcoroutine, ShortExpTurn_A, GameManager.Instance.MyGameData.ShortExp_A);
          StartCoroutine(ExpLossCount(ShortExpTurn_A.rectTransform));
        }
      }
      ShortExpAActive = true;
    }
    if (GameManager.Instance.MyGameData.ShortExp_B == null)
    {
      ShortExpTurn_B.text = "";

      if (ShortExpBActive == true) 
      {
        StartCoroutine(moverect(ShortExpCover_B, ExpCoverUpPos, Vector2.zero, 0.4f, UIPanelCLoseCurve));
        changecount(shortexpBcoroutine, ShortExpTurn_B, GameManager.Instance.MyGameData.ShortExp_B);
        _starteffect = true;
        AudioManager.PlaySFX(21);
      }

      ShortExpBActive = false;
    }
    else
    {
      if (ShortExpBActive == false)
      {
        StartCoroutine(moverect(ShortExpCover_B, Vector2.zero, ExpCoverUpPos, 0.4f, UIPanelOpenCurve));
        _starteffect = true;
        StartCoroutine(ExpGainCount(ShortExpTurn_B.rectTransform));
        changecount(shortexpBcoroutine, ShortExpTurn_B, GameManager.Instance.MyGameData.ShortExp_B);
        AudioManager.PlaySFX(20);
      }
      else
      {
        _turnchanged = int.Parse(ShortExpTurn_B.text) != GameManager.Instance.MyGameData.ShortExp_B.Duration;
        if (_turnchanged)
        {
          StartCoroutine(ExpLossCount(ShortExpTurn_B.rectTransform));
          changecount(shortexpBcoroutine, ShortExpTurn_B, GameManager.Instance.MyGameData.ShortExp_B);
        }
      }

      ShortExpBActive = true;
    }
    if (_starteffect) HighlightManager.HighlightAnimation(HighlightEffectEnum.Exp);

    UpdateHPIcon();

    void changecount(IEnumerator coroutine, TextMeshProUGUI tmp,Experience exp)
    {
      if (coroutine == null)
      {
        coroutine = ChangeCount(tmp
          , tmp.text != "" ? int.Parse(tmp.text) : 1
          , exp==null?0: exp.Duration
          , exp);
        StartCoroutine(coroutine);
      }
      else
      {
        StopCoroutine(coroutine);
        coroutine = ChangeCount(tmp
          , tmp.text != "" ? int.Parse(tmp.text) : 1
          , exp == null ? 0 : exp.Duration
          , exp);
        StartCoroutine(coroutine);
      }
    }
  }
  public void UpdateExpUse()
  {
    if (GameManager.Instance.MyGameData.LongExp == null)
    {
      if (ExpUse_Long_Group.alpha==1.0f)
      {
        ExpUse_Long_text.text = "";
        StartCoroutine(ChangeAlpha(ExpUse_Long_Group, 0.0f, 0.2f));
      }
    }
    else
    {
      Experience _long = GameManager.Instance.MyGameData.LongExp;
      if (DialogueUI.ExpUsageDic_L.ContainsKey(_long))
      {
        ExpUse_Long_img.sprite = GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Single ?
          GameManager.Instance.ImageHolder.SelectionBackground_none :
          GameManager.Instance.ImageHolder.SelectionBackground(
            GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Body ? TendencyTypeEnum.Body:TendencyTypeEnum.Head, true);

        if (ExpUse_Long_Group.alpha == 0.0f)
        {
          StartCoroutine(ChangeAlpha(ExpUse_Long_Group, 1.0f, 0.2f));
        }
        ExpUse_Long_text.text = ((int)-1.0f * DialogueUI.ExpUsageDic_L[_long]).ToString();
      }
      else if (DialogueUI.ExpUsageDic_R.ContainsKey(_long))
      {
        ExpUse_Long_img.sprite = GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Single ?
          GameManager.Instance.ImageHolder.SelectionBackground_none :
          GameManager.Instance.ImageHolder.SelectionBackground(
            GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Body ? TendencyTypeEnum.Body : TendencyTypeEnum.Head, false);

        if (ExpUse_Long_Group.alpha == 0.0f)
        {
          StartCoroutine(ChangeAlpha(ExpUse_Long_Group, 1.0f, 0.2f));
        }
        ExpUse_Long_text.text = ((int)-1.0f * DialogueUI.ExpUsageDic_R[_long]).ToString();
      }
      else
      {
        if (ExpUse_Long_Group.alpha == 1.0f)
        {
          ExpUse_Long_text.text = "";
          StartCoroutine(ChangeAlpha(ExpUse_Long_Group, 0.0f, 0.2f));
        }
      }

    }
    if (GameManager.Instance.MyGameData.ShortExp_A == null)
    {
      if (ExpUse_Short_A_Group.alpha == 1.0f)
      {
        ExpUse_Short_A_text.text = "";
        StartCoroutine(ChangeAlpha(ExpUse_Short_A_Group, 0.0f, 0.2f));
      }
    }
    else
    {
      Experience _Short = GameManager.Instance.MyGameData.ShortExp_A;
      if (DialogueUI.ExpUsageDic_L.ContainsKey(_Short))
      {
        ExpUse_Short_A_img.sprite = GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Single ?
          GameManager.Instance.ImageHolder.SelectionBackground_none :
          GameManager.Instance.ImageHolder.SelectionBackground(
            GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Body ? TendencyTypeEnum.Body : TendencyTypeEnum.Head, true);

        if (ExpUse_Short_A_Group.alpha == 0.0f)
        {
          StartCoroutine(ChangeAlpha(ExpUse_Short_A_Group, 1.0f, 0.2f));
        }
        ExpUse_Short_A_text.text = ((int)-1.0f * DialogueUI.ExpUsageDic_L[_Short]).ToString();
      }
      else if (DialogueUI.ExpUsageDic_R.ContainsKey(_Short))
      {
        ExpUse_Short_A_img.sprite = GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Single ?
          GameManager.Instance.ImageHolder.SelectionBackground_none :
          GameManager.Instance.ImageHolder.SelectionBackground(
            GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Body ? TendencyTypeEnum.Body : TendencyTypeEnum.Head, false);

        if (ExpUse_Short_A_Group.alpha == 0.0f)
        {
          StartCoroutine(ChangeAlpha(ExpUse_Short_A_Group, 1.0f, 0.2f));
        }
        ExpUse_Short_A_text.text = ((int)-1.0f * DialogueUI.ExpUsageDic_R[_Short]).ToString();
      }
      else
      {
        if (ExpUse_Short_A_Group.alpha == 1.0f)
        {
          ExpUse_Short_A_text.text = "";
          StartCoroutine(ChangeAlpha(ExpUse_Short_A_Group, 0.0f, 0.2f));
        }
      }
    }
    if (GameManager.Instance.MyGameData.ShortExp_B == null)
    {
      if (ExpUse_Short_B_Group.alpha == 1.0f)
      {
        ExpUse_Short_B_text.text = "";
        StartCoroutine(ChangeAlpha(ExpUse_Short_B_Group, 0.0f, 0.2f));
      }
    }
    else
    {
      Experience _Short = GameManager.Instance.MyGameData.ShortExp_B;
      if (DialogueUI.ExpUsageDic_L.ContainsKey(_Short))
      {
        ExpUse_Short_B_img.sprite = GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Single ?
          GameManager.Instance.ImageHolder.SelectionBackground_none :
          GameManager.Instance.ImageHolder.SelectionBackground(
            GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Body ? TendencyTypeEnum.Body : TendencyTypeEnum.Head, true);

        if (ExpUse_Short_B_Group.alpha == 0.0f)
        {
          StartCoroutine(ChangeAlpha(ExpUse_Short_B_Group, 1.0f, 0.2f));
        }
        ExpUse_Short_B_text.text = ((int)-1.0f * DialogueUI.ExpUsageDic_L[_Short]).ToString();
      }
      else if (DialogueUI.ExpUsageDic_R.ContainsKey(_Short))
      {
        ExpUse_Short_B_img.sprite = GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Single ?
          GameManager.Instance.ImageHolder.SelectionBackground_none :
          GameManager.Instance.ImageHolder.SelectionBackground(
            GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Body ? TendencyTypeEnum.Body : TendencyTypeEnum.Head, false);

        if (ExpUse_Short_B_Group.alpha == 0.0f)
        {
          StartCoroutine(ChangeAlpha(ExpUse_Short_B_Group, 1.0f, 0.2f));
        }
        ExpUse_Short_B_text.text = ((int)-1.0f * DialogueUI.ExpUsageDic_R[_Short]).ToString();
      }
      else
      {
        if (ExpUse_Short_B_Group.alpha == 1.0f)
        {
          ExpUse_Short_B_text.text = "";
          StartCoroutine(ChangeAlpha(ExpUse_Short_B_Group, 0.0f, 0.2f));
        }
      }
    }
  }
  public void UpdateAllUI()
  {
    UpdateYearText();
    UpdateTurnIcon();
    UpdateHPText(12);
    UpdateSanityText(12);
    UpdateGoldText(12);
    UpdateSupplyText(12);
    UpdateExpPanel();
    UpdateTendencyIcon();
    UpdateSkillLevel();
    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        SidePanelCultUI.UpdateUI();
        SidePanelCultUI.UpdateProgressValue();
        break;
    }
  }
  private void Update()
  {
#if UNITY_EDITOR
    if (Input.GetKeyDown(KeyCode.F10))
    {
      if(DebugUI.gameObject.activeInHierarchy==true)DebugUI.gameObject.SetActive(false);
      else
      {
        DebugUI.gameObject.SetActive(true);
        DebugUI.UpdateValues();
      }
    }
#endif
  }
    private Queue<IEnumerator> UIAnimationQueue = new Queue<IEnumerator>();
    public void AddUIQueue(IEnumerator _anim)
    {
        UIAnimationQueue.Enqueue(_anim);
        if (UIAnimationQueue.Count.Equals(1))
        {
            StartCoroutine(playanimation());
        }
    }
    private IEnumerator playanimation()
    {
        if (IsWorking) yield break;
        IsWorking = true;

        while (UIAnimationQueue.Count > 0)
        {
            yield return StartCoroutine(UIAnimationQueue.Dequeue());
            yield return null;
        }

        IsWorking = false;
        yield return null;
    }
  public IEnumerator ChangeAlpha(CanvasGroup _group, float _targetalpha, float targettime)
  {
    LayoutRebuilder.ForceRebuildLayoutImmediate(_group.transform as RectTransform);

    float _startalpha = _targetalpha == 1.0f ? 0.0f : 1.0f;
    float _endalpha = _targetalpha == 1.0f ? 1.0f : 0.0f;
    float _time = 0.0f;
    float _targettime = targettime;
    float _alpha = _startalpha;
    _group.alpha = _alpha;
    _group.interactable = false;
    _group.blocksRaycasts = false;
    while (_time < _targettime)
    {
      _alpha = Mathf.Lerp(_startalpha, _endalpha, FadeAnimationCurve.Evaluate(_time/_targettime));
      _group.alpha = _alpha;
      _time += Time.deltaTime;
      yield return null;
    }
    _alpha = _endalpha;
    _group.alpha = _alpha;
    if (_targetalpha.Equals(1.0f))
    {
      _group.interactable = true;
      _group.blocksRaycasts = true;
    }
  }
  public IEnumerator ChangeAlpha(CanvasGroup _group,float _startalpha, float _targetalpha, float targettime)
  {
    LayoutRebuilder.ForceRebuildLayoutImmediate(_group.transform as RectTransform);

    float _endalpha = _targetalpha == 1.0f ? 1.0f : 0.0f;
    float _time = 0.0f;
    float _targettime = targettime;
    float _alpha = _startalpha;
    _group.alpha = _alpha;
    _group.interactable = false;
    _group.blocksRaycasts = false;
    while (_time < _targettime)
    {
      _alpha = Mathf.Lerp(_startalpha, _endalpha, FadeAnimationCurve.Evaluate(_time / _targettime));
      _group.alpha = _alpha;
      _time += Time.deltaTime;
      yield return null;
    }
    _alpha = _endalpha;
    _group.alpha = _alpha;
    if (_targetalpha.Equals(1.0f))
    {
      _group.interactable = true;
      _group.blocksRaycasts = true;
    }
  }
  public IEnumerator ChangeAlpha(CanvasGroup _group, float _targetalpha, float targettime,AnimationCurve curve)
  {
    LayoutRebuilder.ForceRebuildLayoutImmediate(_group.transform as RectTransform);

    float _startalpha = _targetalpha == 1.0f ? 0.0f : 1.0f;
    float _endalpha = _targetalpha == 1.0f ? 1.0f : 0.0f;
    float _time = 0.0f;
    float _targettime = targettime;
    float _alpha = _startalpha;
    _group.alpha = _alpha;
    _group.interactable = false;
    _group.blocksRaycasts = false;
    while (_time < _targettime)
    {
      _alpha = Mathf.Lerp(_startalpha, _endalpha, curve.Evaluate(_time / _targettime));
      _group.alpha = _alpha;
      _time += Time.deltaTime;
      yield return null;
    }
    _alpha = _endalpha;
    _group.alpha = _alpha;
    if (_targetalpha.Equals(1.0f))
    {
      _group.interactable = true;
      _group.blocksRaycasts = true;
    }
  }
  public void UpdateMap_SetPlayerPos(Vector2 coordinate)=>MapUI.SetPlayerPos(coordinate);
  public void UpdateMap_SetPlayerPos() => MapUI.SetPlayerPos(GameManager.Instance.MyGameData.Coordinate);
  public void OpenDialogue_Event(bool dir)
  {
    //야외에서 바로 이벤트로 진입하는 경우는 UiMap에서 지도 닫는 메소드를 이미 실행한 상태

    AddUIQueue(DialogueUI.OpenEventUI(dir));
  }//야외에서 이벤트 실행하는 경우, 정착지 진입 직후 퀘스트 실행하는 경우, 정착지에서 장소 클릭해 이벤트 실행하는 경우
  public void GetMad() => MadUI.OpenUI();
  [SerializeField] private AnimationCurve ScrollCurve = null;
  [SerializeField] private float ScrollTime = 0.6f;
  public IEnumerator updatescrollbar(Scrollbar scrollbar)
  {
    yield return new WaitForSeconds(0.05f);
    float _originvalue = scrollbar.value;
    float _time = 0.0f;
    while ( _time < ScrollTime)
    {
      scrollbar.value = Mathf.Lerp(_originvalue, 0.0f,ScrollCurve.Evaluate(_time/ ScrollTime));
      _time += Time.deltaTime;
      yield return null;

    }
    scrollbar.value = 0.0f;
  }

  [SerializeField] private Transform InfoPanelHolder = null;
  [SerializeField] private GameObject InfoPanelPrefab = null;
  public void SetInfoPanel(string text)
  {
    Instantiate(InfoPanelPrefab,InfoPanelHolder).GetComponent<InfoPanel>().Setup(text);
  }
  #region 메인-게임 전환
  [Space(20)]
  [SerializeField] private List<PanelRectEditor> TitlePanels=new List<PanelRectEditor>();
  [SerializeField] private float SceneAnimationTitleMoveTime = 0.3f;
  [SerializeField] private float TitleWaitTime = 0.4f;
  [SerializeField] private float ObjWaitTime = 0.2f;
  [SerializeField] private AnimationCurve SceneAnimationCurve = null;
  public IEnumerator opengamescene()
  {
    var _titlewait = new WaitForSeconds(TitleWaitTime);
    var _objwait = new WaitForSeconds(ObjWaitTime);
    for (int i = 0; i < TitlePanels.Count; i++)
    {
      StartCoroutine(moverect(TitlePanels[i].Rect, TitlePanels[i].OutisdePos, TitlePanels[i].InsidePos, SceneAnimationTitleMoveTime, SceneAnimationCurve));
      yield return _titlewait;
    }
    yield return new WaitForSeconds(1.0f);
  }
  public IEnumerator moverect(RectTransform rect, Vector2 startpos, Vector2 endpos, float targettime, bool isopen)
  {
    LayoutRebuilder.ForceRebuildLayoutImmediate(rect.transform as RectTransform);

    AnimationCurve _targetcurve = isopen ? UIPanelOpenCurve : UIPanelCLoseCurve;

    float _time = 0.0f, _targettime = targettime;
    while (_time < _targettime)
    {
      rect.anchoredPosition = Vector2.Lerp(startpos, endpos, _targetcurve.Evaluate(_time / _targettime));
      _time += Time.deltaTime;
      yield return null;
    }
    rect.anchoredPosition = endpos;
  }
  public IEnumerator moverect(RectTransform rect, Vector2 startpos, Vector2 endpos, float targettime, AnimationCurve curve)
  {
    LayoutRebuilder.ForceRebuildLayoutImmediate(rect.transform as RectTransform);

    float _time = 0.0f, _targettime = targettime;
    while (_time < _targettime)
    {
      rect.anchoredPosition = Vector2.Lerp(startpos, endpos, curve.Evaluate(_time / _targettime));
      _time += Time.deltaTime;
      yield return null;
    }
    rect.anchoredPosition = endpos;
  }
  #endregion
  #region 게임-엔딩 전환
  public void OpenDead(Sprite illust,string description)
  {
    StopAllCoroutines();
    UIAnimationQueue.Clear();
    IsWorking = false;
    StartCoroutine(opendead(illust,description));
  }
  private IEnumerator opendead(Sprite illsut,string description)
  {
    AudioManager.StopWalking();
    GameManager.Instance.DeleteSaveData();
    EndingUI.IsDead = true;

    yield return StartCoroutine(ChangeAlpha(CenterGroup, 0.0f, 3.0f));
    EndingUI.OpenUI_Dead(illsut,description);
  }
  public void OpenEnding(EndingDatas data)
  {
    GameManager.Instance.AddEnding(data.ID);
    StopAllCoroutines();
    GameManager.Instance.DeleteSaveData();
    AudioManager.StopWalking();
    UIAnimationQueue.Clear();
    IsWorking = false;
    StartCoroutine(openending(data));
  }
  private IEnumerator openending(EndingDatas data)
  {
   yield return StartCoroutine(ChangeAlpha(CenterGroup, 0.0f, 3.0f));
    EndingUI.OpenUI_Ending(data);
  }
  #endregion
}
public static class WNCText
{
  /*
  public static string GetAsLengthColor(int value,int length)
  {
    switch (GameManager.Instance.MyGameData.GetMoveRangeType(length))
    {
      case RangeEnum.Low:
        return $"<#2A7935>{value}</color>";
      case RangeEnum.Middle:
        return $"<#BAC623>{value}</color>";
      default: return $"<#BC4915>{value}</color>";
    }
  }
  */
  public static string UIIdleColor(int value)
  {
    return $"<#D4D4D4>{value}</color>";
  }
  public static string UIIdleColor(string str)
  {
    return $"<#D4D4D4>{str}</color>";
  }
  public static string GetMadnessColor(int value)
  {
    return $"<#A959B0>{value}</color>";
  }
  public static string GetMadnessColor(string str)
  {
    return $"<#A959B0>{str}</color>";
  }
  public static string GetSomethingColor(string str)
  {
    return $"<#CFAC7A>{str}</color>";
  }
  public static string GetSomethingColor(int str)
  {
    return $"<#CFAC7A>{str}</color>";
  }
  public static string SetSize(int size,string str)
  {
    return $"<size={size}>{str}</size>";
  }
  public static string GetSubdescriptionColor(string str)
  {
    return $"<i><#989898>{str}</color></i>";
  }
  public static string GetDiscomfortColor(string str)
  {
    return $"<#427573>{str}</color>";
  }
  public static string GetDiscomfortColor(int value)
  {
    return $"<#427573>{value}</color>";
  }
  public static string GetSupplyColor(string str)
  {
    return $"<#6BA260>{str}</color>";
  }
  public static string GetSupplyColor(int value)
  {
    return $"<#6BA260>{value}</color>";
  }
  public static string GetHPColor(string str)
  {
    return $"<#A8616A>{str}</color>";
  }
  public static string GetHPColor(int value)
  {
    return $"<#A8616A>{value.ToString()}</color>"; 
  }
  public static string GetSanityColor(string str)
  {
    return $"<#886D8F>{str}</color>";
  }
  public static string GetSanityColor(int value)
  {
    return $"<#886D8F>{value.ToString()}</color>";
  }
  public static string GetMaxSanityColor(string str)
  {
    return $"<#8A1BD4>{str}</color>";
  }
  public static string GetMaxSanityColor(int value)
  {
    return $"<#8A1BD4>{value.ToString()}</color>";
  }

  public static string GetGoldColor(string str)
  {
    return $"<#AD9D63>{str}</color>";
  }
  public static string GetGoldColor(int value)
  {
    return $"<#AD9D63>{value.ToString()}</color>";
  }
  private static Color SuccessColor = new Color(0.4f, 1.0f, 0.45f, 1.0f);
  private static Color FailColor = new Color(1.0f, 0.4f, 0.45f, 1.0f);
  public static string PercentageColor(string origin,float percent)
  {
    float _glength = MathF.Abs(SuccessColor.g - FailColor.g);
    float _rlength = MathF.Abs(SuccessColor.r - FailColor.r);

    float _ratio =Mathf.Clamp(percent, 0.0f,1.0f);
    float _r_ratio = _rlength/(_rlength + _glength);
    float _g_ratio = _glength / (_rlength + _glength);
    Color _color =new Color(
      _ratio<_g_ratio?FailColor.r:Mathf.Lerp(FailColor.r,SuccessColor.r,(_ratio-_g_ratio)/_r_ratio),
      _ratio<_g_ratio?Mathf.Lerp(FailColor.g,SuccessColor.g,_ratio/_g_ratio):SuccessColor.g,
      SuccessColor.b,1.0f);

    string _html = ColorUtility.ToHtmlStringRGB(_color);
 //   Debug.Log($"{value}/{max} = {value / max} -> {_html}");
    return $"<#{_html}>{origin}</color>";
  }
  public static string PositiveColor(string str)
  {
    return "<#A5D9A5>" + str+"</color>";
  }
  public static string NegativeColor(string str)
  {
    return "<#5A0809>" + str + "</color>";
  }
  public static string NeutralColor(string str)
  {
    return "<#D8D8D8>" + str + "</color>";
  }
  public static string GetSeasonText(string str)
  {
    List<int> _startindex = new List<int>(), _endindex = new List<int>();
    for (int i = 0; i < str.Length; i++)
    {
      if (str[i].Equals('[')) _startindex.Add(i);
      else if (str[i].Equals(']')) _endindex.Add(i);
    }
    if (_startindex.Count.Equals(0)) return str;

    List<string> _strsegs = new List<string>();
    for (int i = 0; i < _startindex.Count; i++)
    {
      int _start = _startindex[i] + 1;
      int _length = _endindex[i] - _start;
      _strsegs.Add(str.Substring(_start, _length));
    }
    string _newstr = str;
    int _currentseason = GameManager.Instance.MyGameData.Turn;
    for (int i = 0; i < _strsegs.Count; i++)
    {
      _newstr= _newstr.Replace("[" + _strsegs[i] + "]", _strsegs[i].Split(",")[_currentseason]);
    }

    return _newstr;
  }

}