using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum UIMoveDir { Horizontal, Vertical }
public enum UIFadeMoveDir { Left, Right, Up, Down }
//등장 시 우->좌, 좌->우, 하->상, 상->하
//퇴장 시 왼쪽, 오른쪽, 위쪽, 아래쪽으로 이동
public class UIManager : MonoBehaviour
{
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
    else Destroy(gameObject);
  }
  public AnimationCurve UIPanelOpenCurve = null;
  public AnimationCurve UIPanelCLoseCurve = null;
  public AnimationCurve CharacterMoveCurve = null;
  [Space(10)]
  [SerializeField] private Transform MyCanvas = null;
  [SerializeField] private UI_dialogue MyDialogue = null;
  [SerializeField] private UI_Reward MyUIReward = null;
  [SerializeField] private UI_EventSuggest MyEvnetSuggest = null;
  public UI_QuestSuggest MyQuestSuggent = null;
  [SerializeField] private UI_Mad MyMadPanel = null;
  [SerializeField] private UI_PlaceEffect MyPlaceEffect = null;
  public UI_FollowEnding MyFollowEnding = null;
  [SerializeField] private RectTransform HpTextRect = null;
  [SerializeField] private RectTransform SanityTextRect = null;
  [SerializeField] private RectTransform GoldTextRect = null;
  public UI_map MyMap = null;
  public bool IsWorking = false;
  public float LargePanelMoveTime = 0.3f;
  public float LargePanelMoveDegree = 0.08f;
  public float LargePanelFadeTime = 0.8f;
  public float IllustFadeTime = 1.5f;
  public float SmallPanelFadeTime = 0.8f;
  public float TextFadeInTime = 0.7f;
  public float TextFadeOutTime = 0.4f;
  public float FadeWaitTime = 1.0f;
  public float MoveInAlpha = 0.1f;
  public float PreviewFadeTime = 0.2f;
  public GameObject TextPreviewPrefab = null;
  public float LossTextMoveDegree = 1.0f;
  public float LossTextMoveTime = 1.5f;
  public float GenTextMoveDegree = 1.5f;
  public float GenTextMoveTime = 2.0f;
  public float CharacterWaitTime = 2.0f;
  public int IllustSoftness_start = 800, IllustSoftness_end = 50;
    public float FadeMoveDegree = 40.0f;
  public Vector2 RightSidePos = new Vector2(1260.0f, 0.0f);
  public Vector2 TopSidePos = new Vector2(0.0f, 900.0f);
  public PreviewManager PreviewManager = null;
  [SerializeField] private Image EnvirBackground = null;
  [SerializeField] private CanvasGroup EnvirGroup = null;
  [SerializeField] private float EnvirChangeTime = 1.5f;
  private float EnvirBackgroundIdleAlpha = 0.7f;
  public void UpdateBackground(EnvironmentType envir)
  {
    Sprite _newbackground = GameManager.Instance.ImageHolder.GetEnvirBackground(envir);
    StartCoroutine(changebackground(_newbackground));
  }
  private IEnumerator changebackground(Sprite newenvir)
  {
    float _time = 0.0f, _targettime = EnvirChangeTime / 2.0f;
    while (_time < _targettime)
    {
      EnvirGroup.alpha = Mathf.Lerp(EnvirBackgroundIdleAlpha,0.0f,_time / _targettime);
      _time += Time.deltaTime;
      yield return null;
    }
    EnvirGroup.alpha = 0.0f;
    EnvirBackground.sprite = newenvir;
    _time = 0.0f;
    while (_time < _targettime)
    {
      EnvirGroup.alpha = EnvirBackgroundIdleAlpha*(_time / _targettime);
      _time += Time.deltaTime;
      yield return null;
    }
    EnvirGroup.alpha = EnvirBackgroundIdleAlpha;
  }
  [Space(10)]
  [SerializeField] private TextMeshProUGUI YearText_a = null;//(시작 기준) 아래 텍스트
  [SerializeField] private TextMeshProUGUI YearText_b = null;//(시작 기준) 위 텍스트
  private bool CurrentYearTextCount = true;
  private float YearTextMoveUnit = 140.0f;
  public void UpdateYearText()
  {
    RectTransform _currentrect = CurrentYearTextCount ? YearText_a.rectTransform:YearText_b.rectTransform;
    RectTransform _targetrect = CurrentYearTextCount ? YearText_b.rectTransform : YearText_a.rectTransform;
    if (CurrentYearTextCount.Equals(true)) YearText_b.text = GameManager.Instance.MyGameData.Year.ToString();
    else YearText_a.text= GameManager.Instance.MyGameData.Year.ToString();

    StartCoroutine(moveyeartext(_currentrect,_targetrect));
  }
  private IEnumerator moveyeartext(RectTransform currentrect,RectTransform targetrect)
  {
    float _time = 0.0f, _targettime = 0.8f;
    Vector2 _toppos=new Vector2(0.0f,YearTextMoveUnit),_middlepos=Vector2.zero,_bottompos=new Vector2(0.0f,-YearTextMoveUnit);
    
    while (_time < _targettime)
    {
      currentrect.anchoredPosition = Vector2.Lerp(_middlepos, _bottompos, _time / _targettime);
      targetrect.anchoredPosition = Vector2.Lerp(_toppos, _bottompos, _time / _targettime);
      _time += Time.deltaTime;
      yield return null;
    }
    currentrect.anchoredPosition = _toppos;
    targetrect.anchoredPosition = _middlepos;

    CurrentYearTextCount = !CurrentYearTextCount;
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
  [SerializeField] private TextMeshProUGUI HPText = null;
  private int lasthp = -1;
  public void UpdateHPText()
  {
    if (!lasthp.Equals(-1))
    {
      int _value = GameManager.Instance.MyGameData.HP - lasthp;
      if (_value < 0) { StartCoroutine(lossstatus(_value, HpTextRect.position)); HPLossParticle.Play(); }
      else if(_value > 0){ StartCoroutine(genstatus(_value, HpTextRect.position));HPGenParticle.Play(); }
    }
    HPText.text = GameManager.Instance.MyGameData.HP.ToString();
    lasthp = GameManager.Instance.MyGameData.HP;
  }
  [SerializeField] private TextMeshProUGUI SanityText = null;
  private int lastsanity = -1;
  public void UpdateSanityText()
  {
    if (!lastsanity.Equals(-1))
    {
      int _value = GameManager.Instance.MyGameData.CurrentSanity - lastsanity;
      if (_value < 0) { StartCoroutine(lossstatus(_value, SanityTextRect.position)); SanityLossParticle.Play();}
      else if(_value>0) { StartCoroutine(genstatus(_value, SanityTextRect.position)); SanityGenParticle.Play(); }
    }
    SanityText.text = GameManager.Instance.MyGameData.CurrentSanity.ToString();
  lastsanity= GameManager.Instance.MyGameData.CurrentSanity;
  }
  [SerializeField] private TextMeshProUGUI GoldText = null;
  private int lastgold = -1;
  public void UpdateGoldText()
  {
    if (!lastgold.Equals(-1))
    {
      int _value = GameManager.Instance.MyGameData.Gold - lastgold;
      if (_value < 0) { StartCoroutine(lossstatus(_value, GoldTextRect.position)); GoldLossParticle.Play(); }
      else if (_value > 0) { StartCoroutine(genstatus(_value, GoldTextRect.position)); GoldGenParticle.Play(); }
    }
    GoldText.text = GameManager.Instance.MyGameData.Gold.ToString();
    lastgold= GameManager.Instance.MyGameData.Gold;
  }
  [Space(10)]
  [SerializeField] private ParticleSystem HPLossParticle = null;
  [SerializeField] private ParticleSystem SanityLossParticle = null;
  [SerializeField] private ParticleSystem GoldLossParticle = null;
  [SerializeField] private ParticleSystem HPGenParticle = null;
  [SerializeField] private ParticleSystem SanityGenParticle = null;
  [SerializeField] private ParticleSystem GoldGenParticle = null;
  
  private IEnumerator lossstatus(int _value,Vector3 _startpos)
  {
    if (_value == 0) yield break;
    _startpos.z = 0;
    float _dividevalue = 0.7f;
    float _time = 0.0f, _targettime = LossTextMoveTime;
    Vector3 _endpos = _startpos + Vector3.down * LossTextMoveDegree;
    Vector3 _currentpos = _startpos;
    Vector3 _middlepos = Vector3.Lerp(_startpos, _endpos, _dividevalue);
    GameObject _textimg = Instantiate(TextPreviewPrefab, MyCanvas);
    CanvasGroup _group=_textimg.GetComponent<CanvasGroup>();
    _textimg.transform.SetParent(MyCanvas);
    TextMeshProUGUI _tmp=_textimg.GetComponent<TextMeshProUGUI>();
    _tmp.text = _value.ToString();
    RectTransform _rect= _textimg.GetComponent<RectTransform>();
    _rect.position = new Vector3(_startpos.x, _startpos.y, 0.0f);
    _rect.localScale = Vector3.one;
    float _startalpha = 0.0f, _endalpha = 1.0f;
    float _currentalpha= _startalpha;
    float _firsttime = _targettime * _dividevalue;
    float _secondtime = _targettime - _firsttime;
    _startpos.z = MyCanvas.position.z;_endpos.z = MyCanvas.position.z;
    _middlepos.z = MyCanvas.position.z;
    while (_time < _firsttime)
    {
      _currentpos = Vector3.Lerp(_startpos, _middlepos, Mathf.Pow(_time / _firsttime, 0.5f));
      _currentalpha = Mathf.Lerp(_startalpha, _endalpha, Mathf.Pow(_time / _firsttime, 2.0f));
      _rect.position = _currentpos;
      _group.alpha=_currentalpha;
      _time+=Time.deltaTime;
      yield return null;
    }
    _time = 0.0f;
    _rect.position = _middlepos;
    _currentalpha = _endalpha;
    while (_time < _secondtime)
    {
      _currentpos =Vector3.Lerp(_middlepos, _endpos, Mathf.Pow(_time / _secondtime, 3.0f));
      _currentalpha = Mathf.Lerp(_endalpha, _startalpha, Mathf.Pow(_time / _secondtime, 0.5f));
      _rect.position = _currentpos;
      _group.alpha = _currentalpha;
      _time += Time.deltaTime;
      yield return null;
    }
    Destroy(_textimg);
  }
  private IEnumerator genstatus(int _value,Vector3 _endpos)
  {
    if (_value == 0) yield break;
    float _stopdegree = 0.4f;
    float _time = 0.0f, _endtime = GenTextMoveTime;
    _endpos.z = 0;
    Vector3 _startpos = _endpos + Vector3.down* GenTextMoveDegree;
    Vector3 _stoppos = Vector3.Lerp(_startpos, _endpos, _stopdegree);
    GameObject _obj = Instantiate(TextPreviewPrefab, MyCanvas);
    RectTransform _rect= _obj.GetComponent<RectTransform>();
    _rect.localScale = Vector3.one;
    CanvasGroup _group = _obj.GetComponent<CanvasGroup>();
    _rect.GetComponent<TextMeshProUGUI>().text = "+"+ _value.ToString();
    _rect.position = new Vector3(_startpos.x, _startpos.y, 0.0f);
    float _startalpha = 1.0f, _endalpha = 0.0f;
    _group.alpha = _startalpha;
    float _currentalpha = _startalpha;
    Vector3 _currentpos = _startpos;
    _startpos.z = MyCanvas.position.z; _stoppos.z = MyCanvas.position.z;
    while (_time < _endtime)
    {
      _currentalpha = Mathf.Lerp(_startalpha, _endalpha, Mathf.Pow(_time / _endtime, 6.0f));
      _currentpos = Vector3.Lerp(_startpos, _stoppos, Mathf.Pow(_time / _endtime, 0.4f));
      _group.alpha = _currentalpha;
      _rect.position = _currentpos;
      _time += Time.deltaTime;
      yield return null;
    }
    Destroy(_obj);
  }
  [Space(5)]
  [SerializeField] private CanvasGroup ConverEffect = null;
  [SerializeField] private CanvasGroup ForceEffect = null;
  [SerializeField] private CanvasGroup WildEffect = null;
  [SerializeField] private CanvasGroup IntelEffect = null;
  private int ConverLevel = -1, ForceLevel = -1, WildLevel = -1, IntelLevel = -1;
  public void UpdateTheme()
  {

  }
  public void UpdateTheme(SkillName skill)
  {
    ThemeType _theme_a = ThemeType.Conversation, _theme_b = ThemeType.Conversation;
    switch (skill)
    {
      case SkillName.Speech: _theme_a = ThemeType.Conversation;_theme_b = ThemeType.Conversation; break;
      case SkillName.Threat: _theme_a = ThemeType.Conversation; _theme_b = ThemeType.Force; break;
      case SkillName.Deception: _theme_a = ThemeType.Conversation; _theme_b = ThemeType.Wild; break;
      case SkillName.Logic: _theme_a = ThemeType.Conversation; _theme_b = ThemeType.Intelligence; break;
      case SkillName.Martialarts: _theme_a = ThemeType.Force; _theme_b = ThemeType.Force; break;
      case SkillName.Bow: _theme_a = ThemeType.Force; _theme_b = ThemeType.Wild; break;
      case SkillName.Somatology: _theme_a = ThemeType.Force; _theme_b = ThemeType.Intelligence; break;
      case SkillName.Survivable: _theme_a = ThemeType.Wild; _theme_b = ThemeType.Wild; break;
      case SkillName.Biology: _theme_a = ThemeType.Wild; _theme_b = ThemeType.Intelligence; break;
      case SkillName.Knowledge: _theme_a = ThemeType.Intelligence; _theme_b = ThemeType.Intelligence; break;
    }
    if (_theme_a == _theme_b) UpdateTheme(_theme_a);
    else
    {
      UpdateTheme(_theme_a);UpdateTheme(_theme_b);
    }
  }
  public void UpdateTheme(ThemeType theme)
  {
    switch (theme)
    {
      case ThemeType.Conversation:
        ConverEffect.alpha = 1.0f;
        StartCoroutine(ChangeAlpha(ConverEffect,0.0f,0.15f,false));
        break;
      case ThemeType.Force:
        ForceEffect.alpha = 1.0f;
        StartCoroutine(ChangeAlpha(ForceEffect, 0.0f, 0.15f, false));
        break;
      case ThemeType.Wild:
        WildEffect.alpha = 1.0f;
        StartCoroutine(ChangeAlpha(WildEffect, 0.0f, 0.15f, false));
        break;
      case ThemeType.Intelligence:
        IntelEffect.alpha = 1.0f;
        StartCoroutine(ChangeAlpha(IntelEffect, 0.0f, 0.15f, false));
        break;
    }
  }

  [Space(5)]
  [SerializeField] private RectTransform TendencyHeadRect = null;
  [SerializeField] private RectTransform TendencyHeadIcon = null;
  [SerializeField] private RectTransform TendencyBodyRect = null;
  [SerializeField] private RectTransform TendencyBodyIcon = null;
  private float TendendcyIconMoveUnit = 66.0f;

  public void UpdateTendencyIcon()
  {
    if(GameManager.Instance.MyGameData.Tendency_Body.ChangedDir!=0)
    {
      StartCoroutine(movetendencyrect( TendencyBodyRect, GameManager.Instance.MyGameData.Tendency_Body.ChangedDir));
      StartCoroutine(movetendencyicon(TendencyBodyIcon, GameManager.Instance.MyGameData.Tendency_Body.ChangedDir));
      GameManager.Instance.MyGameData.Tendency_Body.ChangedDir = 0;
    }

    if (GameManager.Instance.MyGameData.Tendency_Head.ChangedDir != 0)
    {
      StartCoroutine(movetendencyrect(TendencyHeadRect, GameManager.Instance.MyGameData.Tendency_Head.ChangedDir));
      StartCoroutine(movetendencyicon(TendencyHeadIcon, GameManager.Instance.MyGameData.Tendency_Head.ChangedDir));
      GameManager.Instance.MyGameData.Tendency_Head.ChangedDir = 0;
    }
  }
  private IEnumerator movetendencyrect(RectTransform rect,int degree)
  {
    Vector2 _originpos = rect.anchoredPosition;
    Vector2 _targetpos=new Vector2(_originpos.x+TendendcyIconMoveUnit*degree, _originpos.y);
    Vector2 _currentpos = _originpos;
    float _time = 0.0f, _targetime = 0.4f;
    while(_time< _targetime)
    {
      rect.anchoredPosition = _currentpos;
      _currentpos = Vector2.Lerp(_originpos, _targetpos, _time / _targetime);
      _time += Time.deltaTime;
      yield return null;
    }
    rect.anchoredPosition = _targetpos;
  }
  private IEnumerator movetendencyicon(RectTransform rect, int degree)
  {
    Vector2 _originpos = rect.anchoredPosition;
    Vector2 _targetpos = new Vector2(_originpos.x - TendendcyIconMoveUnit * degree, _originpos.y);
    Vector2 _currentpos = _originpos;
    float _time = 0.0f, _targetime = 0.4f;
    while (_time < _targetime)
    {
      rect.anchoredPosition = _currentpos;
      _currentpos = Vector2.Lerp(_originpos, _targetpos, _time / _targetime);
      _time += Time.deltaTime;
      yield return null;
    }
    rect.anchoredPosition = _targetpos;
  }
  [SerializeField] private Image[] LongTermIcon = new Image[2];
  [SerializeField] private TextMeshProUGUI[] LongTermTurn=new TextMeshProUGUI[2];
  public void UpdateExpLongTermIcon()
  {
    for(int i=0; i < LongTermIcon.Length; i++)
    {
      if (GameManager.Instance.MyGameData.LongTermEXP[i] == null)
      {
        LongTermIcon[i].enabled = true;
      }
      else
      {
        LongTermIcon[i].enabled = false;
        LongTermTurn[i].text = GameManager.Instance.MyGameData.LongTermEXP[i].Duration.ToString();
      }
    }
  }
  [SerializeField] private Image[] ShortTermIcon = new Image[4];
  [SerializeField] private TextMeshProUGUI[] ShortTermTurn=new TextMeshProUGUI[4];
  public void UpdateExpShortTermIcon()
  {
    for (int i = 0; i < ShortTermIcon.Length; i++)
    {
      if (GameManager.Instance.MyGameData.ShortTermEXP[i] == null)
      {
        ShortTermTurn[i].enabled = true;
      }
      else
      {
        ShortTermIcon[i].enabled = false;
        ShortTermTurn[i].text = GameManager.Instance.MyGameData.ShortTermEXP[i].Duration.ToString();
      }
    }
  }
  public CanvasGroup CurrentTopUI = null;
  public void UpdateAllUI()
  {
    UpdateYearText();
    UpdateTurnIcon();
    UpdateHPText();
    UpdateSanityText();
    UpdateGoldText();
    UpdateExpLongTermIcon();
    UpdateExpShortTermIcon();
    UpdateTendencyIcon();
  }
  private void Update()
  {
    if (Input.GetMouseButtonDown(1) && IsWorking == false) CloseCurrentTopUI();
    if (Input.GetKeyDown(KeyCode.W)) SanityLossParticle.Play();
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
  public IEnumerator OpenSoftness(RectMask2D _rectmask)
  {
    float _time = 0.0f, _endtime = SmallPanelFadeTime;
    float _startsoft=IllustSoftness_start,_endsoft=IllustSoftness_end;
    Vector2Int _soft = _rectmask.softness;
    while (_time < _endtime)
    {
      _soft.x = (int)(Mathf.Lerp(_startsoft, _endsoft, _time / _endtime));
      _rectmask.softness = _soft;
      _time += Time.deltaTime;
      yield return null;
    }
  }
   public IEnumerator OpenUI(RectTransform _rect,CanvasGroup _group,UIMoveDir _dir,bool _islarge,bool istopui)
  {
    if (istopui) CloseCurrentTopUI();

    if (_rect.gameObject.activeSelf == false) _rect.gameObject.SetActive(true);
    Vector2 _size = _rect.sizeDelta;
    _group.alpha = MoveInAlpha;
    Vector2 _startpos = new Vector2(_size.x * (_dir == UIMoveDir.Horizontal ? LargePanelMoveDegree : 0), _size.y * (_dir == UIMoveDir.Vertical ? LargePanelMoveDegree : 0));
    _rect.anchoredPosition = _startpos;
    Vector2 _endpos = Vector2.zero;
    float _time = 0.0f;
        float _targetime = _islarge ? LargePanelFadeTime : SmallPanelFadeTime;
    while (_time < _targetime)
    {
      _rect.anchoredPosition = Vector2.Lerp(_startpos,_endpos, UIPanelOpenCurve.Evaluate(_time/_targetime));

      _group.alpha = Mathf.Lerp(MoveInAlpha, 1.0f, Mathf.Pow(_time / _targetime, 0.5f));
      _time += Time.deltaTime;
      yield return null;
    }
        _rect.anchoredPosition = _endpos;
    _group.blocksRaycasts = true;
    _group.alpha = 1.0f;
    _group.interactable = true;
   
    if(istopui) CurrentTopUI = _group;

    if (_rect == MyQuestSuggent.MyRect || _rect == MyEvnetSuggest.MyRect) yield break;
  }
  public IEnumerator OpenUI(RectTransform _rect,UIMoveDir _dir,float _movetime, bool istopui)
  {
    if (istopui) CloseCurrentTopUI();

    if (_rect.gameObject.activeSelf == false) _rect.gameObject.SetActive(true);
    if (_rect.GetComponent<CanvasGroup>().alpha.Equals(0.0f))
    {
      _rect.GetComponent<CanvasGroup>().alpha = 1.0f;
      _rect.GetComponent<CanvasGroup>().interactable = true;
      _rect.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    Vector2 _size = _rect.sizeDelta;
    Vector2 _startpos = _dir.Equals(UIMoveDir.Horizontal) ? RightSidePos : TopSidePos;
    _rect.anchoredPosition = _startpos;
    Vector2 _endpos = Vector2.zero;
    float _time = 0.0f;
    float _targetime = _movetime;
    while (_time < _targetime)
    {
      _rect.anchoredPosition = Vector2.Lerp(_startpos, _endpos, UIPanelOpenCurve.Evaluate(_time / _targetime));
      _time += Time.deltaTime;
      yield return null;
    }
    _rect.anchoredPosition = _endpos;
    if (istopui) CurrentTopUI = _rect.GetComponent<CanvasGroup>() != null ? _rect.GetComponent<CanvasGroup>() : null;

    if (_rect == MyQuestSuggent.MyRect || _rect == MyEvnetSuggest.MyRect) yield break;
  }
  public IEnumerator OpenUI(RectTransform _rect,Vector2 originpos, Vector2 targetpos, float _movetime, bool istopui)
  {
    if (istopui) CloseCurrentTopUI();

    if (_rect.gameObject.activeSelf == false) _rect.gameObject.SetActive(true);
    if (_rect.GetComponent<CanvasGroup>().alpha.Equals(0.0f))
    {
      _rect.GetComponent<CanvasGroup>().alpha = 1.0f;
      _rect.GetComponent<CanvasGroup>().interactable = true;
      _rect.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    Vector2 _size = _rect.sizeDelta;
    Vector2 _startpos = _rect.anchoredPosition;
    _rect.anchoredPosition = _startpos;
    Vector2 _endpos = targetpos;
    float _time = 0.0f;
    float _targetime = _movetime;
    while (_time < _targetime)
    {
      _rect.anchoredPosition = Vector2.Lerp(_startpos, _endpos, UIPanelOpenCurve.Evaluate(_time / _targetime));
      _time += Time.deltaTime;
      yield return null;
    }
    _rect.anchoredPosition = _endpos;

    if (istopui) CurrentTopUI = _rect.GetComponent<CanvasGroup>() != null ? _rect.GetComponent<CanvasGroup>() : null;
    if (_rect == MyQuestSuggent.MyRect || _rect == MyEvnetSuggest.MyRect) yield break;
  }
  public void CloseCurrentTopUI() => StartCoroutine(closetopui());
  public IEnumerator closetopui()
  {
    if (CurrentTopUI != null)
    {

      if (CurrentTopUI.transform.parent.GetComponent<UI_default>() != null)
      {
        var _uidefault = CurrentTopUI.transform.parent.GetComponent<UI_default>();
        CurrentTopUI = null;
        _uidefault.CloseUI();
      }
      else
      {
        CurrentTopUI.alpha = 0.0f;
        CurrentTopUI.interactable = false;
        CurrentTopUI.blocksRaycasts = false;
        CurrentTopUI = null;
      }
      yield return null;
    }
    yield return null;
  }
    public IEnumerator CloseUI(RectTransform _rect, CanvasGroup _group, UIMoveDir _dir,bool istopui)
  {
    if (istopui) CurrentTopUI = null;

    _group.interactable = false;
    _group.blocksRaycasts = false;
    Vector2 _size = _rect.sizeDelta;
    _group.alpha = 1.0f;
    Vector2 _startpos = Vector2.zero;
    _rect.anchoredPosition = _startpos;
    Vector2 _endpos = new Vector2(_size.x * (_dir == UIMoveDir.Horizontal ? LargePanelMoveDegree : 0), _size.y * (_dir == UIMoveDir.Vertical ? LargePanelMoveDegree : 0));
    float _time = 0.0f;
    while (_time < LargePanelFadeTime)
    {
      _rect.anchoredPosition = Vector2.Lerp(_startpos, _endpos,UIPanelCLoseCurve.Evaluate(_time / LargePanelFadeTime));
      _group.alpha = Mathf.Lerp(1.0f, 0.0f, Mathf.Pow(_time / LargePanelFadeTime, 0.5f));
      _time += Time.deltaTime;
      yield return null;
    }
    _rect.anchoredPosition = _endpos;
    _group.alpha = 0.0f;

    if (istopui) CurrentTopUI = null;
  }
  public IEnumerator CloseUI(CanvasGroup _group,bool _islarge, bool istopui)
  {
    if (istopui) CurrentTopUI = null;

    _group.interactable = false;
    _group.blocksRaycasts = false;
    _group.alpha = 1.0f;
    float _time = 0.0f;
    float _targettime = _islarge ? LargePanelFadeTime : SmallPanelFadeTime;
    while (_time < _targettime)
    {
      _group.alpha = Mathf.Lerp(1.0f, 0.0f, Mathf.Pow(_time / _targettime, 0.5f));
      _time += Time.deltaTime;
      yield return null;
    }
    _group.alpha = 0.0f;

    if(istopui) CurrentTopUI = null;
  }
  public IEnumerator CloseUI(RectTransform _rect, UIMoveDir _dir, float _movetime, bool istopui)
  {
    if (istopui) CurrentTopUI = null;

    Vector2 _size = _rect.sizeDelta;
    Vector2 _startpos = _rect.anchoredPosition3D;
    Vector2 _endpos = _dir.Equals(UIMoveDir.Horizontal) ? RightSidePos : TopSidePos;
    float _time = 0.0f;
    float _targetime = _movetime;
    while (_time < _targetime)
    {
      _rect.anchoredPosition = Vector2.Lerp(_startpos, _endpos, UIPanelCLoseCurve.Evaluate(_time / LargePanelFadeTime));
      _time += Time.deltaTime;
      yield return null;
    }
    _rect.anchoredPosition = _endpos;

  }
  public IEnumerator CloseUI(RectTransform _rect,Vector2 originpos, Vector2 targetpos, float _movetime, bool istopui)
  {
    if (istopui) CloseCurrentTopUI();

    Vector2 _size = _rect.sizeDelta;
    Vector2 _startpos = _rect.anchoredPosition3D;
    Vector2 _endpos = targetpos;
    float _time = 0.0f;
    float _targetime = _movetime;
    while (_time < _targetime)
    {
      _rect.anchoredPosition = Vector2.Lerp(_startpos, _endpos, UIPanelCLoseCurve.Evaluate(_time / LargePanelFadeTime));
      _time += Time.deltaTime;
      yield return null;
    }
    _rect.anchoredPosition = _endpos;
    if (istopui) CurrentTopUI = null;

  }
  public IEnumerator ChangeAlpha(Image _img, float _targetalpha)
  {
    float _startalpha = _targetalpha==1.0f ? MoveInAlpha : 1.0f;
    float _endalpha = _targetalpha==1.0f ? 1.0f : 0.0f;
    float _time = 0.0f;
    Color _color = Color.white; 
    float _alpha = _startalpha;
    _color.a = _alpha;
    _img.color = _color;
    while (_time < IllustFadeTime)
    {
      _alpha = Mathf.Lerp(_startalpha, _endalpha, Mathf.Pow(_time / IllustFadeTime, 0.3f));
      _color.a = _alpha;
      _img.color = _color;
      _time += Time.deltaTime;
      yield return null;
    }
    _alpha = _endalpha;
    _color.a = _alpha;
    _img.color= _color;
  }
  public IEnumerator ChangeAlpha(Image _img, float _targetalpha,float targettime)
  {
    float _startalpha = _targetalpha == 1.0f ? MoveInAlpha : 1.0f;
    float _endalpha = _targetalpha == 1.0f ? 1.0f : 0.0f;
    float _time = 0.0f;
    Color _color = Color.white;
    float _alpha = _startalpha;
    _color.a = _alpha;
    _img.color = _color;
    while (_time < targettime)
    {
      _alpha = Mathf.Lerp(_startalpha, _endalpha, Mathf.Pow(_time / targettime, 0.3f));
      _color.a = _alpha;
      _img.color = _color;
      _time += Time.deltaTime;
      yield return null;
    }
    _alpha = _endalpha;
    _color.a = _alpha;
    _img.color = _color;
  }
  public IEnumerator ChangeAlpha(CanvasGroup _group, float _targetalpha, float targettime, bool istopui)
  {
    if (istopui&&_targetalpha.Equals(1.0f)) CloseCurrentTopUI();

    float _startalpha = _targetalpha == 1.0f ? MoveInAlpha : 1.0f;
    float _endalpha = _targetalpha == 1.0f ? 1.0f : 0.0f;
    float _time = 0.0f;
    float _targettime = targettime;
    float _alpha = _startalpha;
    _group.alpha = _alpha;
    _group.interactable = false;
    _group.blocksRaycasts = false;
    while (_time < _targettime)
    {
      _alpha = Mathf.Lerp(_startalpha, _endalpha, Mathf.Pow(_time / _targettime, 0.3f));
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

    if (istopui) CurrentTopUI = _targetalpha.Equals(1.0f) ? _group : null;
  }
  public IEnumerator ChangeAlpha(CanvasGroup _group, float _targetalpha,bool _islarge, bool istopui)
  {
    if (istopui) CloseCurrentTopUI();
    float _startalpha = _targetalpha == 1.0f ? MoveInAlpha : 1.0f;
    float _endalpha = _targetalpha == 1.0f ? 1.0f : 0.0f;
    float _time = 0.0f;
    float _targettime = _islarge ? LargePanelFadeTime : SmallPanelFadeTime;
    float _alpha = _startalpha;
    _group.alpha = _alpha;
    _group.interactable = false;
    _group.blocksRaycasts = false;
    while (_time < _targettime)
    {
      _alpha = Mathf.Lerp(_startalpha, _endalpha, Mathf.Pow(_time / _targettime, 0.3f));
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
    if (istopui) CurrentTopUI = _targetalpha.Equals(1.0f) ? _group : null;
  }
  public IEnumerator ChangeAlpha(CanvasGroup _group, float _targetalpha, bool _islarge, UIFadeMoveDir _movedir, bool istopui)
  {
    if (istopui) CloseCurrentTopUI();
    Vector2 _dir = Vector2.zero;
    switch (_movedir)
    {
      case UIFadeMoveDir.Left: _dir = Vector2.left; break;
      case UIFadeMoveDir.Right: _dir = Vector2.right; break;
      case UIFadeMoveDir.Up: _dir = Vector2.up; break;
      case UIFadeMoveDir.Down: _dir = Vector2.down; break;
    }
    RectTransform _rect = _group.transform.GetComponent<RectTransform>();
    Vector2 _originpos = _rect.anchoredPosition3D;
    Vector2 _startpos = _targetalpha.Equals(1.0f) ? _originpos - _dir * FadeMoveDegree : _originpos;
    Vector2 _endpos = _targetalpha.Equals(1.0f) ? _originpos : _originpos + _dir * FadeMoveDegree;
    Vector2 _currentpos = _startpos;
    //movedir 설정했으면 해당 방향대로 목표, 종료 위치 설정

    float _startalpha = _targetalpha == 1.0f ? MoveInAlpha : 1.0f;
    float _endalpha = _targetalpha == 1.0f ? 1.0f : 0.0f;
    float _time = 0.0f;
    float _targettime = _islarge ? LargePanelFadeTime : SmallPanelFadeTime;
    float _alpha = _startalpha;
    _group.alpha = _alpha;
    _group.interactable = false;
    _group.blocksRaycasts = false;
    _rect.anchoredPosition3D = _currentpos;
    AnimationCurve _curve = _targetalpha.Equals(1.0f) ? UIPanelOpenCurve : UIPanelCLoseCurve;
    while (_time < _targettime)
    {
      _currentpos = Vector2.Lerp(_startpos, _endpos, _curve.Evaluate(_time / _targettime));
      _rect.anchoredPosition3D = _currentpos;
      _alpha = Mathf.Lerp(_startalpha, _endalpha, Mathf.Pow(_time / _targettime, 0.7f));
      _group.alpha = _alpha;
      _time += Time.deltaTime;
      yield return null;
    }
    _alpha = _endalpha;
    _group.alpha = _alpha;
    _rect.anchoredPosition3D = _originpos;
    if (_targetalpha.Equals(1.0f))
    {
      _group.interactable = true;
      _group.blocksRaycasts = true;
    }
    if (istopui) CurrentTopUI = _targetalpha.Equals(1.0f) ? _group : null;
  }
  public IEnumerator ChangeAlpha(TextMeshProUGUI _tmp, float _targetalpha,float targettime)
  {
    float _startalpha = _targetalpha == 1.0f ? MoveInAlpha : 1.0f;
    float _endalpha = _targetalpha == 1.0f ? 1.0f : 0.0f;
    float _time = 0.0f;
    Color _color = Color.white;
    float _alpha = _startalpha;
    _color.a = _alpha;
    _tmp.color = _color;
    float _targettime = targettime;
    while (_time < _targettime)
    {
      _alpha = Mathf.Lerp(_startalpha, _endalpha, Mathf.Pow(_time / _targettime, 0.3f));
      _color.a = _alpha;
      _tmp.faceColor = _color;

      _time += Time.deltaTime;
      yield return null;
    }
    _alpha = _endalpha;
    _color.a = _alpha;
    _tmp.color = _color;
  }

  public void UpdateMap_SettleIcons(List<Settlement> _avail) => MyMap.UpdateIcons(_avail);
  public void UpdateMap_AddSettle(SettlementIcon _icon) => MyMap.AddSettleIcon(_icon);
  public void UpdateMap_SetPlayerPos(Settlement _settle)=>MyMap.SetPlayerPos(_settle);
  public void UpdateMap_SetPlayerPos() => MyMap.SetPlayerPos(GameManager.Instance.MyGameData.CurrentPos);
  public void CreateMap() => MyMap.MapCreater.MakeTilemap(GameManager.Instance.MyMapSaveData);
  public void OpenSuggestUI() => MyEvnetSuggest.OpenSuggest();
  public void OpenBadExpUI(Experience _badexp) => MyUIReward.OpenRewardExpPanel_penalty(_badexp);
    public void OpenDialogue()
    {
        //야외에서 바로 이벤트로 진입하는 경우는 UiMap에서 지도 닫는 메소드를 이미 실행한 상태

        //정착지에서 이벤트 선택하는 경우도 이미 EventSugest에서 닫기 메소드를 실행한 상태

        MyDialogue.SetEventDialogue();
    }//야외에서 이벤트 실행하는 경우, 정착지 진입 직후 퀘스트 실행하는 경우, 정착지에서 장소 클릭해 이벤트 실행하는 경우
  public void OpenSuccessDialogue(SuccessData _success) => MyDialogue.SetEventDialogue(_success);
  public void OpenFailDialogue(FailureData _fail) => MyDialogue.SetEventDialogue(_fail);

  public void OpenQuestDialogue() => MyQuestSuggent.OpenQuestSuggestUI();
  public void ResetEventPanels()
  {
    MyDialogue.ClosePanel_quick();
    MyEvnetSuggest.CloseSuggestPanel_quick();
   if(MyQuestSuggent.IsActivePanel) MyQuestSuggent.CloseQuestUI();
  }//이벤트 패널,리스트 패널,퀘스트 패널을 처음 상태로 초기화(맵 이동할 때 마다 호출)
    public void CloseSuggestPanel_normal() => MyEvnetSuggest.CloseSuggestPanel_normal();
  public void GetMad(Experience mad) => MyMadPanel.OpenUI(mad);
  public void UpdatePlaceEffect() => MyPlaceEffect.UpdatePlace();
  public void OpenEnding(FollowEndingData endingdata)
  {
    MyDialogue.CloseUI();
    MyFollowEnding.OpenEnding(endingdata);
  }

  #region 메인-게임 전환
  [Space(20)]
  [SerializeField] private RectTransform TopTitleRect = null;
  [SerializeField] private Vector2 TopTitle_ClosePos = Vector2.zero;
  [SerializeField] private Vector2 TopTitle_OpenPos = Vector2.zero;
  [Space(3)]
  [SerializeField] private RectTransform RightTitleRect = null;
  [SerializeField] private Vector2 RightTitle_ClosePos = Vector2.zero;
  [SerializeField] private Vector2 RightTitle_OpenPos = Vector2.zero;
  [Space(3)]
  [SerializeField] private RectTransform BottomTitleRect = null;
  [SerializeField] private Vector2 BottomTitle_ClosePos = Vector2.zero;
  [SerializeField] private Vector2 BottomTitle_OpenPos = Vector2.zero;
  [Space(3)]
  [SerializeField] private RectTransform LeftTitleRect = null;
  [SerializeField] private Vector2 LeftTitle_ClosePos = Vector2.zero;
  [SerializeField] private Vector2 LeftTitle_OpenPos = Vector2.zero;
  [Space(3)]
  [SerializeField] private RectTransform YearRect = null;
  [SerializeField] private Vector2 Year_ClosePos = Vector2.zero;
  [SerializeField] private Vector2 Year_OpenPos = Vector2.zero;
  [Space(3)]
  [SerializeField] private RectTransform TurnRect = null;
  [SerializeField] private Vector2 Turn_ClosePos = Vector2.zero;
  [SerializeField] private Vector2 Turn_OpenPos = Vector2.zero;
  [Space(3)]
  [SerializeField] private RectTransform HPRect = null;
  [SerializeField] private Vector2 HP_ClosePos = Vector2.zero;
  [SerializeField] private Vector2 HP_OpenPos = Vector2.zero;
  [Space(3)]
  [SerializeField] private RectTransform SanityRect = null;
  [SerializeField] private Vector2 Sanity_ClosePos = Vector2.zero;
  [SerializeField] private Vector2 Sanity_OpenPos = Vector2.zero;
  [Space(3)]
  [SerializeField] private RectTransform GoldRect = null;
  [SerializeField] private Vector2 Gold_ClosePos = Vector2.zero;
  [SerializeField] private Vector2 Gold_OpenPos = Vector2.zero;
  [Space(3)]
  [SerializeField] private RectTransform QuestRect = null;
  [SerializeField] private Vector2 Quest_ClosePos = Vector2.zero;
  [SerializeField] private Vector2 Quest_OpenPos = Vector2.zero;
  [Space(3)]
  [SerializeField] private RectTransform OptionRect = null;
  [SerializeField] private Vector2 Option_ClosePos = Vector2.zero;
  [SerializeField] private Vector2 Option_OpenPos = Vector2.zero;
  [Space(3)]
  [SerializeField] private RectTransform PlaceRect = null;
  [SerializeField] private Vector2 Place_ClosePos = Vector2.zero;
  [SerializeField] private Vector2 Place_OpenPos = Vector2.zero;
  [Space(3)]
  [SerializeField] private RectTransform SkillRect = null;
  [SerializeField] private Vector2 Skill_ClosePos = Vector2.zero;
  [SerializeField] private Vector2 Skill_OpenPos = Vector2.zero;
  [Space(3)]
  [SerializeField] private RectTransform ExpRect = null;
  [SerializeField] private Vector2 Exp_ClosePos = Vector2.zero;
  [SerializeField] private Vector2 Exp_OpenPos = Vector2.zero;
  [Space(3)]
  [SerializeField] private RectTransform TendencyRect = null;
  [SerializeField] private Vector2 Tendency_ClosePos = Vector2.zero;
  [SerializeField] private Vector2 Tendency_OpenPos = Vector2.zero;
  [Space(10)]
  [SerializeField] private float SceneAnimationTitleMoveTime = 0.3f;
  [SerializeField] private float SceneAnimationObjMoveTime = 0.1f;
  [SerializeField] private float TitleWaitTime = 0.2f;
  [SerializeField] private float ObjWaitTime = 0.08f;
  [SerializeField] private AnimationCurve SceneAnimationCurve = null;
  public IEnumerator opengamescene()
  {
    var _titlewait = new WaitForSeconds(TitleWaitTime);
    var _objwait = new WaitForSeconds(ObjWaitTime);

    StartCoroutine(moverect(TopTitleRect, TopTitle_ClosePos, TopTitle_OpenPos, SceneAnimationTitleMoveTime, SceneAnimationCurve));
    yield return _titlewait;
    StartCoroutine(moverect(RightTitleRect, RightTitle_ClosePos, RightTitle_OpenPos, SceneAnimationTitleMoveTime, SceneAnimationCurve));
    yield return _titlewait;
    StartCoroutine(moverect(BottomTitleRect, BottomTitle_ClosePos, BottomTitle_OpenPos, SceneAnimationTitleMoveTime, SceneAnimationCurve));
    yield return _titlewait;
    StartCoroutine(moverect(LeftTitleRect, LeftTitle_ClosePos, LeftTitle_OpenPos, SceneAnimationTitleMoveTime, SceneAnimationCurve));
    yield return _titlewait;
    StartCoroutine(moverect(YearRect, Year_ClosePos, Year_OpenPos, SceneAnimationObjMoveTime, SceneAnimationCurve));
    yield return _objwait;
    StartCoroutine(moverect(TurnRect, Turn_ClosePos, Turn_OpenPos, SceneAnimationObjMoveTime, SceneAnimationCurve));
    yield return _objwait;
    StartCoroutine(moverect(HPRect, HP_ClosePos, HP_OpenPos, SceneAnimationObjMoveTime, SceneAnimationCurve));
    yield return _objwait;
    StartCoroutine(moverect(SanityRect, Sanity_ClosePos, Sanity_OpenPos, SceneAnimationObjMoveTime, SceneAnimationCurve));
    yield return _objwait;
    StartCoroutine(moverect(GoldRect, Gold_ClosePos, Gold_OpenPos, SceneAnimationObjMoveTime, SceneAnimationCurve));
    yield return _objwait;
    StartCoroutine(moverect(QuestRect, Quest_ClosePos, Quest_OpenPos, SceneAnimationObjMoveTime, SceneAnimationCurve));
    yield return _objwait;
    StartCoroutine(moverect(OptionRect, Option_ClosePos, Option_OpenPos, SceneAnimationObjMoveTime, SceneAnimationCurve));
    yield return _objwait;
    StartCoroutine(moverect(PlaceRect, Place_ClosePos, Place_OpenPos, SceneAnimationObjMoveTime, SceneAnimationCurve));
    yield return _objwait;
    StartCoroutine(moverect(SkillRect, Skill_ClosePos, Skill_OpenPos, SceneAnimationObjMoveTime, SceneAnimationCurve));
    yield return _objwait;
    StartCoroutine(moverect(ExpRect, Exp_ClosePos, Exp_OpenPos, SceneAnimationObjMoveTime, SceneAnimationCurve));
    yield return _objwait;
    yield return StartCoroutine(moverect(TendencyRect, Tendency_ClosePos, Tendency_OpenPos, SceneAnimationObjMoveTime, SceneAnimationCurve));
  }
  public IEnumerator moverect(RectTransform rect, Vector2 startpos, Vector2 endpos, float targettime, AnimationCurve targetcurve)
  {
    float _time = 0.0f, _targettime = targettime;
    while (_time < _targettime)
    {
      rect.anchoredPosition = Vector2.Lerp(startpos, endpos, targetcurve.Evaluate(_time / _targettime));
      _time += Time.deltaTime;
      yield return null;
    }
    rect.anchoredPosition = endpos;
  }
  #endregion

}
public static class ColorText
{
  private static Color SuccessColor = new Color(0.8867924f, 5621194f, 0.3471876f, 1.0f);
  private static Color FailColor = new Color(0.5648571f, 0.8862745f, 0.3490196f, 1.0f);
  public static string PercentageColor(int percent)
  {
    string _html = ColorUtility.ToHtmlStringRGB(Color.Lerp(FailColor, SuccessColor, percent / 100.0f));
    return $"<#{_html}>{percent}</color>";
  }
  public static string PositiveColor(string str)
  {
    return "<#64FE2E>"+str+"</color>";
  }
  public static string NegativeColor(string str)
  {
    return "<#FF0000>" + str + "</color>";
  }
  public static string NeutralColor(string str)
  {
    return "<#D8D8D8>" + str + "</color>";
  }
}