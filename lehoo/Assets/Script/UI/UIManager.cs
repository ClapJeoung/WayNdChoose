using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public enum UIMoveDir { Horizontal, Vertical }
public enum UIFadeMoveDir { Left, Right, Up, Down }
//등장 시 우->좌, 좌->우, 하->상, 상->하
//퇴장 시 왼쪽, 오른쪽, 위쪽, 아래쪽으로 이동
public class UIManager : MonoBehaviour
{
   public List<UI_default> AllUIDefault=new List<UI_default>();
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
  public Transform MyCanvas = null;
  public UI_Main MainUI = null;
  public UI_dialogue MyDialogue = null;
  public UI_RewardExp ExpRewardUI = null;
  public UI_Settlement MySettleUI = null;
  public UI_QuestWolf QuestUI_Cult = null;
  public UI_Mad MyMadPanel = null;
  public UI_FollowEnding MyFollowEnding = null;
  public UI_Gameover GameOverUI = null;
  public SidePanel_Quest_Wolf QuestSidePanel_Cult = null;
  public UI_map MyMap = null;
  public UI_Tendency MyTendencyPanelUI = null;
  public UI_skill_info MySkillUIPanelUI = null;
  public UI_Expereince_info MyExpPanelUI = null;
  public bool IsWorking = false;
  public float LargePanelMoveTime = 0.3f;
  public float LargePanelMoveDegree = 0.08f;
  public float LargePanelFadeTime = 0.8f;
  public float IllustFadeTime = 1.5f;
  public float SmallPanelFadeTime = 0.8f;
  public float TextFadeInTime = 0.7f;
  public float TextFadeOutTime = 0.4f;
  public float FadeWaitTime = 1.0f;
  public float PreviewFadeTime = 0.2f;
  public int IllustSoftness_start = 800, IllustSoftness_end = 50;
    public float FadeMoveDegree = 40.0f;
  public Vector2 RightSidePos = new Vector2(1260.0f, 0.0f);
  public Vector2 TopSidePos = new Vector2(0.0f, 900.0f);
  public PreviewManager PreviewManager = null;
  [SerializeField] private Image EnvirBackground = null;
  [SerializeField] private CanvasGroup EnvirGroup = null;
  [SerializeField] private float EnvirChangeTime = 1.5f;
  [SerializeField] private DebugScript DebugUI = null;
  private float EnvirBackgroundIdleAlpha = 0.7f;
  public MapButton MapButton = null;
  public SettleButton SettleButton = null;
  [SerializeField] private AnimationCurve FadeAnimationCurve = null;
  public void GameOver(GameOverTypeEnum gameovertype)
  {
    StopAllCoroutines();
    for(int i = 0; i < AllUIDefault.Count; i++)
    {
      if (AllUIDefault[i].IsOpen) AllUIDefault[i].CloseForGameover();
    }
    StartCoroutine(closegamescene());
    GameOverUI.OpenUI(gameovertype);
  }
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
  [SerializeField] private AnimationCurve StatusEffectLossCurve = new AnimationCurve();
  [SerializeField] private TextMeshProUGUI HPText = null;
  private int lasthp = -1;
  public void UpdateHPText()
  {
    if (!lasthp.Equals(-1))
    {
      int _changedvalue = GameManager.Instance.MyGameData.HP - lasthp;
      if(_changedvalue!=0)
      StartCoroutine(statuschangedtexteffect(WNCText.GetHPColor(_changedvalue), HPText.rectTransform));
    }
    HPText.text = GameManager.Instance.MyGameData.HP.ToString();
    Debug.Log("체력 수치 업데이트");
    lasthp = GameManager.Instance.MyGameData.HP;
  }
  [SerializeField] private TextMeshProUGUI SanityText_current = null;
  [SerializeField] private TextMeshProUGUI SanityText_max = null;
  private int lastsanity = -1;
  public void UpdateSanityText()
  {
    if (!lastsanity.Equals(-1))
    {
      int _changedvalue = GameManager.Instance.MyGameData.CurrentSanity - lastsanity;
      if (_changedvalue != 0)
        StartCoroutine(statuschangedtexteffect(WNCText.GetSanityColor(_changedvalue), SanityText_current.rectTransform));
    }
    SanityText_current.text = GameManager.Instance.MyGameData.CurrentSanity.ToString();
    SanityText_max.text = GameManager.Instance.MyGameData.MaxSanity.ToString();
    Debug.Log("정신력, 최대 정신력 수치 업데이트");
  lastsanity = GameManager.Instance.MyGameData.CurrentSanity;
  }
  [SerializeField] private TextMeshProUGUI GoldText = null;
  private int lastgold = -1;
  public void UpdateGoldText()
  {
    if (!lastgold.Equals(-1))
    {
      int _changedvalue = GameManager.Instance.MyGameData.Gold - lastgold;
      if (_changedvalue != 0)
        StartCoroutine(statuschangedtexteffect(WNCText.GetGoldColor(_changedvalue), GoldText.rectTransform));
    }
    GoldText.text = GameManager.Instance.MyGameData.Gold.ToString();
    Debug.Log("골드 수치 업데이트");
    lastgold = GameManager.Instance.MyGameData.Gold;
  }
  [SerializeField] private TextMeshProUGUI MovePointText = null;
  private int lastmovepoint = -1;
  public void UpdateMovePointText()
  {
    if (lastmovepoint != -1)
    {
      int _changedvalue = GameManager.Instance.MyGameData.MovePoint- lastmovepoint;
      if (_changedvalue != 0)
        StartCoroutine(statuschangedtexteffect(WNCText.GetMovepointColor(_changedvalue), MovePointText.rectTransform));
    }
    MovePointText.text = GameManager.Instance.MyGameData.MovePoint.ToString();
    Debug.Log("이동력 수치 업데이트");
    lastmovepoint = GameManager.Instance.MyGameData.MovePoint;
  }
  [Space(10)]
  [SerializeField] private float StatusTextMovetime = 1.0f;
  [SerializeField] private float LossStatusMoveSpace = 35.0f;
  [SerializeField] private GameObject StatusTextPrefab = null;
  private IEnumerator statuschangedtexteffect(string value,RectTransform targetrect)
  {
    float _time = 0.0f, _targettime = StatusTextMovetime;
    GameObject _prefab=Instantiate(StatusTextPrefab,MyCanvas);
    _prefab.GetComponent<TextMeshProUGUI>().text = value;
    RectTransform _rect = _prefab.GetComponent<RectTransform>();

    float _startingspace = 40.0f;
    _rect.position = targetrect.position;
    _rect.anchoredPosition3D = new Vector3(_rect.anchoredPosition.x, _rect.anchoredPosition.y-_startingspace, 0.0f);
    StartCoroutine(ChangeAlpha(_prefab.GetComponent<CanvasGroup>(), 1.0f, 0.3f));

    Vector3 _startpos = _rect.anchoredPosition;
    Vector3 _endpos = _rect.anchoredPosition + Vector2.down * LossStatusMoveSpace;
    while (_time < _targettime)
    {
      _rect.anchoredPosition = Vector3.Lerp(_startpos, _endpos, StatusEffectLossCurve.Evaluate(_time / _targettime));
      _time += Time.deltaTime;
      yield return null;
    }

    yield return StartCoroutine(ChangeAlpha(_prefab.GetComponent<CanvasGroup>(),0.0f,0.2f));
    Destroy(_prefab);
  }
  [Space(5)]
  private int ConverLevel = -1, ForceLevel = -1, WildLevel = -1, IntelLevel = -1;

  [Space(5)]
  [SerializeField] private RectTransform TendencyBodyRect = null;
  [SerializeField] private RectTransform TendencyBodyIcon = null;
  [SerializeField] private RectTransform TendencyHeadRect = null;
  [SerializeField] private RectTransform TendencyHeadIcon = null;
  private Vector2 TendencyPos_m2 = new Vector2(-98.0f, 0.0f);
  private Vector2 TendencyPos_m1 = new Vector2(-30.0f, 0.0f);
  private Vector2 TendencyPos_p1 = new Vector2(35.0f, 0.0f);
  private Vector2 TendencyPos_p2 = new Vector2(101.0f, 0.0f);
  public void UpdateTendencyIcon()
  {
    if (GameManager.Instance.MyGameData.Tendency_Body.Level!=0&&TendencyBodyIcon.gameObject.activeInHierarchy == false) TendencyBodyIcon.gameObject.SetActive(true);
    if (GameManager.Instance.MyGameData.Tendency_Head.Level!=0&&TendencyHeadIcon.gameObject.activeInHierarchy == false) TendencyHeadIcon.gameObject.SetActive(true);

    Vector2 _bodypos = Vector2.zero;
    Vector2 _headpos= Vector2.zero;
    switch (GameManager.Instance.MyGameData.Tendency_Body.Level)
    {
      case -2:_bodypos = TendencyPos_m2;break;
      case -1: _bodypos = TendencyPos_m1; break;
      case 0: _bodypos = new Vector2(0.0f, 0.0f); break;
      case 1: _bodypos = TendencyPos_p1; break;
      case 2: _bodypos = TendencyPos_p2; break;
    }
    switch (GameManager.Instance.MyGameData.Tendency_Head.Level)
    {
      case -2: _headpos = TendencyPos_m2; break;
      case -1: _headpos = TendencyPos_m1; break;
      case 0: _headpos = new Vector2(0.0f, 0.0f);break;
      case 1: _headpos = TendencyPos_p1; break;
      case 2: _headpos = TendencyPos_p2; break;
    }
    StartCoroutine(movetendencyrect(TendencyBodyRect, TendencyBodyIcon, _bodypos));
    StartCoroutine(movetendencyrect(TendencyHeadRect,TendencyHeadIcon, _headpos));
  }
  private IEnumerator movetendencyrect(RectTransform titlerect,RectTransform iconrect,Vector2 targetpos)
  {
    if (titlerect.anchoredPosition == targetpos) yield break;

    Vector2 _originpos_title = titlerect.anchoredPosition;
    Vector2 _originpos_icon= iconrect.anchoredPosition;
    Vector2 _targetpos_title = targetpos;
    Vector2 _targetpos_icon =_originpos_title- (_targetpos_title - _originpos_title);
    float _time = 0.0f, _targettime = 0.8f;

    while (_time < _targettime)
    {
      titlerect.anchoredPosition = Vector2.Lerp(_originpos_title, _targetpos_title, _time / _targettime);
      iconrect.anchoredPosition=Vector2.Lerp(_originpos_icon,_targetpos_icon,_time / _targettime);

      _time += Time.deltaTime; yield return null;
    }

    titlerect.anchoredPosition = _targetpos_title;
    iconrect.anchoredPosition = _targetpos_icon;
  }
  [SerializeField] private Image LongTermCover = null;
  [SerializeField] private TextMeshProUGUI LongTermTurn = null;
  public void UpdateExpLongTermIcon()
  {
    if (GameManager.Instance.MyGameData.LongTermEXP == null)
    {
      if(LongTermCover.enabled==false) LongTermCover.enabled = true;
      LongTermTurn.text = "";
    }
    else
    {
      if (LongTermCover.enabled == true) LongTermCover.enabled = false;

      LongTermTurn.text = GameManager.Instance.MyGameData.LongTermEXP.Duration.ToString();
    }
  }
  [SerializeField] private Image[] ShortTermCover = new Image[2];
  [SerializeField] private TextMeshProUGUI[] ShortTermTurn=new TextMeshProUGUI[2];
  public void UpdateExpShortTermIcon()
  {
    for (int i = 0; i < ShortTermCover.Length; i++)
    {
      if (GameManager.Instance.MyGameData.ShortTermEXP[i] == null)
      {
        ShortTermTurn[i].enabled = true;
      }
      else
      {
        if(ShortTermCover[i].enabled==true) ShortTermCover[i].enabled = false;

        ShortTermTurn[i].text = GameManager.Instance.MyGameData.ShortTermEXP[i].Duration.ToString();
      }
    }
  }
  public void UpdateAllUI()
  {
    UpdateYearText();
    UpdateTurnIcon();
    UpdateHPText();
    UpdateSanityText();
    UpdateGoldText();
    UpdateMovePointText();
    UpdateExpLongTermIcon();
    UpdateExpShortTermIcon();
    UpdateTendencyIcon();
  }
  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.F4))
    {
      if(DebugUI.gameObject.activeInHierarchy==true)DebugUI.gameObject.SetActive(false);
      else
      {
        DebugUI.gameObject.SetActive(true);
        DebugUI.UpdateValues();
      }
    }
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
  public void CloseOtherStatusPanels(UI_default currentui)
  {
    if(currentui as UI_skill_info != null)
    {
      if (MyExpPanelUI.IsOpen) MyExpPanelUI.CloseUI();
      if (MyTendencyPanelUI.IsOpen) MyTendencyPanelUI.CloseUI();
    }
    else if(currentui as UI_Expereince_info != null)
    {
      if (MySkillUIPanelUI.IsOpen) MySkillUIPanelUI.CloseUI();
      if (MyTendencyPanelUI.IsOpen) MyTendencyPanelUI.CloseUI();
    }
    else if(currentui as UI_Tendency != null)
    {
      if (MyExpPanelUI.IsOpen) MyExpPanelUI.CloseUI();
      if (MySkillUIPanelUI.IsOpen) MySkillUIPanelUI.CloseUI();
    }
  }
   public IEnumerator OpenUI(RectTransform _rect,CanvasGroup _group,UIMoveDir _dir,bool _islarge)
  {
    if (_rect.gameObject.activeSelf == false) _rect.gameObject.SetActive(true);
    Vector2 _size = _rect.sizeDelta;
    _group.alpha = 0.0f;
    Vector2 _startpos = new Vector2(_size.x * (_dir == UIMoveDir.Horizontal ? LargePanelMoveDegree : 0), _size.y * (_dir == UIMoveDir.Vertical ? LargePanelMoveDegree : 0));
    _rect.anchoredPosition = _startpos;
    Vector2 _endpos = Vector2.zero;
    float _time = 0.0f;
        float _targetime = _islarge ? LargePanelFadeTime : SmallPanelFadeTime;
    while (_time < _targetime)
    {
      _rect.anchoredPosition = Vector2.Lerp(_startpos,_endpos, UIPanelOpenCurve.Evaluate(_time/_targetime));

      _group.alpha = Mathf.Lerp(0.0f, 1.0f, Mathf.Pow(_time / _targetime, 0.5f));
      _time += Time.deltaTime;
      yield return null;
    }
        _rect.anchoredPosition = _endpos;
    _group.blocksRaycasts = true;
    _group.alpha = 1.0f;
    _group.interactable = true;
   
    if (_rect == MySettleUI.DefaultRect) yield break;
  }
  public IEnumerator OpenUI(RectTransform _rect,UIMoveDir _dir,float _movetime)
  {
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
    if ( _rect == MySettleUI.DefaultRect) yield break;
  }
  public IEnumerator OpenUI(RectTransform _rect,Vector2 originpos, Vector2 targetpos, float _movetime)
  {
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

    if (_rect == MySettleUI.DefaultRect) yield break;
  }
    public IEnumerator CloseUI(RectTransform _rect, CanvasGroup _group, UIMoveDir _dir)
  {
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
  }
  public IEnumerator CloseUI(CanvasGroup _group,bool _islarge)
  {
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
  }
  public IEnumerator CloseUI(RectTransform _rect, UIMoveDir _dir, float _movetime)
  {
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
  public IEnumerator CloseUI(RectTransform _rect,Vector2 originpos, Vector2 targetpos, float _movetime)
  {
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
  }
  public IEnumerator ChangeAlpha(Image _img, float _targetalpha)
  {
    LayoutRebuilder.ForceRebuildLayoutImmediate(_img.transform as RectTransform);

    float _startalpha = _targetalpha==1.0f ? 0.0f : 1.0f;
    float _endalpha = _targetalpha==1.0f ? 1.0f : 0.0f;
    float _time = 0.0f;
    Color _color = Color.white; 
    float _alpha = _startalpha;
    _color.a = _alpha;
    _img.color = _color;
    while (_time < IllustFadeTime)
    {
      _alpha = Mathf.Lerp(_startalpha, _endalpha, FadeAnimationCurve.Evaluate(_time / IllustFadeTime));
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
    LayoutRebuilder.ForceRebuildLayoutImmediate(_img.transform as RectTransform);

    float _startalpha = _targetalpha == 1.0f ? 0.0f : 1.0f;
    float _endalpha = _targetalpha == 1.0f ? 1.0f : 0.0f;
    float _time = 0.0f;
    Color _color = Color.white;
    float _alpha = _startalpha;
    _color.a = _alpha;
    _img.color = _color;
    while (_time < targettime)
    {
      _alpha = Mathf.Lerp(_startalpha, _endalpha, FadeAnimationCurve.Evaluate(_time / targettime));
      _color.a = _alpha;
      _img.color = _color;
      _time += Time.deltaTime;
      yield return null;
    }
    _alpha = _endalpha;
    _color.a = _alpha;
    _img.color = _color;
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
  public IEnumerator ChangeAlpha(CanvasGroup _group, float _targetalpha,bool _islarge)
  {
    LayoutRebuilder.ForceRebuildLayoutImmediate(_group.transform as RectTransform);

    float _startalpha = _targetalpha == 1.0f ? 0.0f : 1.0f;
    float _endalpha = _targetalpha == 1.0f ? 1.0f : 0.0f;
    float _time = 0.0f;
    float _targettime = _islarge ? LargePanelFadeTime : SmallPanelFadeTime;
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
  public IEnumerator ChangeAlpha(CanvasGroup _group, float _targetalpha, bool _islarge, UIFadeMoveDir _movedir)
  {
    LayoutRebuilder.ForceRebuildLayoutImmediate(_group.transform as RectTransform);

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

    float _startalpha = _targetalpha == 1.0f ? 0.0f : 1.0f;
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
      _alpha = Mathf.Lerp(_startalpha, _endalpha, FadeAnimationCurve.Evaluate(_time / _targettime));
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
  }
  public IEnumerator ChangeAlpha(TextMeshProUGUI _tmp, float _targetalpha,float targettime)
  {
    LayoutRebuilder.ForceRebuildLayoutImmediate(_tmp.transform as RectTransform);

    float _startalpha = _targetalpha == 1.0f ? 0.0f : 1.0f;
    float _endalpha = _targetalpha == 1.0f ? 1.0f : 0.0f;
    float _time = 0.0f;
    Color _color = Color.white;
    float _alpha = _startalpha;
    _color.a = _alpha;
    _tmp.color = _color;
    float _targettime = targettime;
    while (_time < _targettime)
    {
      _alpha = Mathf.Lerp(_startalpha, _endalpha, FadeAnimationCurve.Evaluate(_time / _targettime));
      _color.a = _alpha;
      _tmp.faceColor = _color;

      _time += Time.deltaTime;
      yield return null;
    }
    _alpha = _endalpha;
    _color.a = _alpha;
    _tmp.color = _color;
  }
  public void UpdateMap_SetPlayerPos(Vector2 coordinate)=>MyMap.SetPlayerPos(coordinate);
  public void UpdateMap_SetPlayerPos() => MyMap.SetPlayerPos(GameManager.Instance.MyGameData.Coordinate);
  public void CreateMap() => MyMap.MapCreater.MakeTilemap();
  public void OpenDialogue()
  {
    //야외에서 바로 이벤트로 진입하는 경우는 UiMap에서 지도 닫는 메소드를 이미 실행한 상태

    MyDialogue.OpenUI();
  }//야외에서 이벤트 실행하는 경우, 정착지 진입 직후 퀘스트 실행하는 경우, 정착지에서 장소 클릭해 이벤트 실행하는 경우
  public void ResetEventPanels()
  {
    if(MyDialogue.IsOpen) MyDialogue.CloseUI();
    if(MySettleUI.IsOpen) MySettleUI.CloseUI();
  }//이벤트 패널,리스트 패널,퀘스트 패널을 처음 상태로 초기화(맵 이동할 때 마다 호출)
    public void CloseSuggestPanel_normal() => MySettleUI.CloseUI();
  public void GetMad() => MyMadPanel.OpenUI();
  public void OpenEnding(FollowEndingData endingdata)
  {
    MyDialogue.CloseUI();
    MyFollowEnding.OpenEnding(endingdata);
  }

  #region 메인-게임 전환
  [SerializeField] private List<PanelRectEditor> TitlePanels=new List<PanelRectEditor>();
  [SerializeField] private float SceneAnimationTitleMoveTime = 0.3f;
  [SerializeField] private float SceneAnimationObjMoveTime = 0.1f;
  [SerializeField] private float TitleWaitTime = 0.4f;
  [SerializeField] private float ObjWaitTime = 0.2f;
  [SerializeField] private AnimationCurve SceneAnimationCurve = null;
  public IEnumerator opengamescene()
  {
    var _titlewait = new WaitForSeconds(TitleWaitTime);
    var _objwait = new WaitForSeconds(ObjWaitTime);
    PlayAudio(IntroOpenAudio);
    for (int i = 0; i < 4; i++)
    {
      StartCoroutine(moverect(TitlePanels[i].Rect, TitlePanels[i].OutisdePos, TitlePanels[i].InsidePos, SceneAnimationTitleMoveTime, SceneAnimationCurve));
      yield return _titlewait;
    }
    for(int i = 4; i < TitlePanels.Count; i++)
    {
      StartCoroutine(moverect(TitlePanels[i].Rect, TitlePanels[i].OutisdePos, TitlePanels[i].InsidePos, SceneAnimationObjMoveTime, SceneAnimationCurve));
      yield return _objwait;
    }
  }
  public IEnumerator moverect(RectTransform rect, Vector2 startpos, Vector2 endpos, float targettime, bool isopen)
  {
    LayoutRebuilder.ForceRebuildLayoutImmediate(rect.transform as RectTransform);

    AnimationCurve _targetcurve = isopen ? UIPanelOpenCurve : UIPanelCLoseCurve;
    AudioClip _clip = isopen ? PanelOpenAudio : PanelCloseAudio;
    PlayAudio(_clip);

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
  public IEnumerator closegamescene()
  {
    var _titlewait = new WaitForSeconds(TitleWaitTime);
    var _objwait = new WaitForSeconds(ObjWaitTime);

    for (int i = TitlePanels.Count-1; i >3; i++)
    {
      StartCoroutine(moverect(TitlePanels[i].Rect, TitlePanels[i].InsidePos, TitlePanels[i].OutisdePos, SceneAnimationObjMoveTime, SceneAnimationCurve));
      yield return _objwait;
    }
    for (int i = 3; i >-1; i++)
    {
      StartCoroutine(moverect(TitlePanels[i].Rect, TitlePanels[i].InsidePos, TitlePanels[i].OutisdePos, SceneAnimationTitleMoveTime, SceneAnimationCurve));
      yield return _titlewait;
    }
  }
  #endregion
  [Space(10)]
  [SerializeField] private AudioClip IntroOpenAudio = null;
  [SerializeField] private AudioClip PanelOpenAudio = null;
  [SerializeField] private AudioClip PanelCloseAudio = null;
  private List<AudioSource> audiosources = new List<AudioSource>();
  private List<AudioSource> AudioSources
  {
    get
    {
      if (audiosources.Count == 0)
      {
        audiosources=GetComponents<AudioSource>().ToList();
      }
      return audiosources;
    }
  }
  public void PlayAudio(AudioClip clip)
  {
    for(int i=0;i< AudioSources.Count; i++)
    {
      if (AudioSources[i].isPlaying) continue;
      AudioSources[i].clip = clip;
      AudioSources[i].Play();
    }
  }

}
public static class WNCText
{
  public static string GetSomethingColor(string str)
  {
    return $"<#F5DA81>{str}</color>";
  }
  public static string GetSomethingColor(int str)
  {
    return $"<#F5DA81>{str}</color>";
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
    return $"<#5F04B4>{str}</color>";
  }
  public static string GetDiscomfortColor(int value)
  {
    return $"<#5F04B4>{value}</color>";
  }
  public static string GetMovepointColor(string str)
  {
    return $"<#BEF781>{str}</color>";
  }
  public static string GetMovepointColor(int value)
  {
    return $"<#BEF781>{value}</color>";
  }
  public static string GetHPColor(string str)
  {
    return $"<#F78181>{str}</color>";
  }
  public static string GetHPColor(int value)
  {
    return $"<#F78181>{value.ToString()}</color>"; 
  }
  public static string GetSanityColor(string str)
  {
    return $"<#BE81F7>{str}</color>";
  }
  public static string GetSanityColor(int value)
  {
    return $"<#BE81F7>{value.ToString()}</color>";
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
    return $"<#F3F781>{str}</color>";
  }
  public static string GetGoldColor(int value)
  {
    return $"<#F3F781>{value.ToString()}</color>";
  }
  private static Color SuccessColor = new Color(0.8867924f, 5621194f, 0.3471876f, 1.0f);
  private static Color FailColor = new Color(0.5648571f, 0.8862745f, 0.3490196f, 1.0f);
  public static string PercentageColor(int percent)
  {
    string _html = ColorUtility.ToHtmlStringRGB(Color.Lerp(FailColor, SuccessColor, percent / 100.0f));
    return $"<#{_html}>{percent}%</color>";
  }
  public static string PositiveColor(string str)
  {
    return "<#64FE2E>"+str+"</color>";
  }
  public static string NegativeColor(string str)
  {
    return "<#FF4000>" + str + "</color>";
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