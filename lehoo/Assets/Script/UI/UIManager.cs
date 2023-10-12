using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

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
    StartCoroutine(ChangeAlpha(CurtainGroup, 0.0f, 2.5f));
  }
  public void ResetGame(string text)
  {
    IsWorking = true;
    CurtainText.text=text;
    StartCoroutine(resetgame());
  }
  private IEnumerator resetgame()
  {
    yield return StartCoroutine(ChangeAlpha(CurtainGroup, 1.0f, 1.2f));
    yield return new WaitForSeconds(3.0f);
    UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
  public AnimationCurve UIPanelOpenCurve = null;
  public AnimationCurve UIPanelCLoseCurve = null;
  public AnimationCurve CharacterMoveCurve = null;
  [Space(10)]
  public Transform MyCanvas = null;
  public CanvasGroup CenterGroup = null;
  public CanvasGroup CurtainGroup = null;
  public TextMeshProUGUI CurtainText = null;
  public UI_Main MainUI = null;
  public UI_dialogue MyDialogue = null;
  public UI_RewardExp ExpRewardUI = null;
  public UI_Settlement MySettleUI = null;
  public UI_QuestWolf QuestUI_Cult = null;
  public UI_Mad MyMadPanel = null;
  public SidePanel_Quest_Cult QuestSidePanel_Cult = null;
  public UI_map MyMap = null;
  public UI_Tendency MyTendencyPanelUI = null;
  public UI_skill_info MySkillUIPanelUI = null;
  public UI_Expereince_info MyExpPanelUI = null;
  public UI_Ending EndingUI = null;
  public bool IsWorking = false;
  public PreviewManager PreviewManager = null;
  [SerializeField] private Image EnvirBackground = null;
  [SerializeField] private CanvasGroup EnvirGroup = null;
  [SerializeField] private float EnvirChangeTime = 1.5f;
  [SerializeField] private DebugScript DebugUI = null;
  public MapButton MapButton = null;
  public SettleButton SettleButton = null;
  [SerializeField] private AnimationCurve FadeAnimationCurve = null;
  public void UpdateBackground(EnvironmentType envir)
  {
    Sprite _newbackground = GameManager.Instance.ImageHolder.GetEnvirBackground(envir);
    EnvirBackground.sprite = _newbackground;
    StartCoroutine(ChangeAlpha(EnvirGroup,1.0f,EnvirChangeTime));
  }
  public void OffBackground()
  {
    EnvirBackground.sprite = GameManager.Instance.ImageHolder.Transparent;
    StartCoroutine(ChangeAlpha(EnvirGroup, 0.0f, EnvirChangeTime));
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
  [Space(10)]
  public float StatusEffectTime = 1.5f;
  [SerializeField] private TextMeshProUGUI HPText = null;
  [SerializeField] private Image HPEffect_Image = null;
  [SerializeField] private CanvasGroup HPEffect_Group = null;
  [SerializeField] private Color HPUpColor= Color.white;
  [SerializeField] private Color HPDownColor= Color.white;
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

    Color _color = lasthp < GameManager.Instance.MyGameData.HP ? HPUpColor : HPDownColor;
    HPEffect_Image.color = _color;
    StartCoroutine(ChangeAlpha(HPEffect_Group, 0.0f, StatusEffectTime));

    lasthp = GameManager.Instance.MyGameData.HP;
  }
  [SerializeField] private TextMeshProUGUI SanityText_current = null;
 // [SerializeField] private TextMeshProUGUI SanityText_max = null;
  private int lastsanity = -1;
  [SerializeField] private Image SanityEffect_Image = null;
  [SerializeField] private CanvasGroup SanityEffect_Group = null;
  [SerializeField] private Color SanityUpColor = Color.white;
  [SerializeField] private Color SanityDownColor = Color.white;
  public void UpdateSanityText()
  {
    if (!lastsanity.Equals(-1))
    {
      int _changedvalue = GameManager.Instance.MyGameData.Sanity - lastsanity;
      if (_changedvalue != 0)
        StartCoroutine(statuschangedtexteffect(WNCText.GetSanityColor(_changedvalue), SanityText_current.rectTransform));
    }
    SanityText_current.text = GameManager.Instance.MyGameData.Sanity.ToString();
   // SanityText_max.text = GameManager.Instance.MyGameData.MaxSanity.ToString();
    Debug.Log("정신력, 최대 정신력 수치 업데이트");

    Color _color = lastsanity < GameManager.Instance.MyGameData.Sanity ? SanityUpColor : SanityDownColor;
    SanityEffect_Image.color = _color;
    StartCoroutine(ChangeAlpha(SanityEffect_Group, 0.0f, StatusEffectTime));
    
    lastsanity = GameManager.Instance.MyGameData.Sanity;
  }
  [SerializeField] private TextMeshProUGUI GoldText = null;
  private int lastgold = -1;
  [SerializeField] private Image GoldEffect_Image = null;
  [SerializeField] private CanvasGroup GoldEffect_Group = null;
  [SerializeField] private Color GoldUpColor = Color.white;
  [SerializeField] private Color GoldDownColor = Color.white;
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

    Color _color = lastgold < GameManager.Instance.MyGameData.Gold ? GoldUpColor : GoldDownColor;
    GoldEffect_Image.color = _color;
    StartCoroutine(ChangeAlpha(GoldEffect_Group, 0.0f, StatusEffectTime));

    lastgold = GameManager.Instance.MyGameData.Gold;
  }
  [SerializeField] private Image MovePoint_Icon = null;
  [SerializeField] private TextMeshProUGUI MovePointText = null;
  private int lastmovepoint = -1;
  [SerializeField] private Image MovepointEffect_Image = null;
  [SerializeField] private CanvasGroup MovepointEffect_Group = null;
  [SerializeField] private Color MovepointUpColor = Color.white;
  [SerializeField] private Color MovepointDownColor = Color.white;
  public void UpdateMovePointText()
  {
    if (lastmovepoint != -1)
    {
      int _changedvalue = GameManager.Instance.MyGameData.MovePoint- lastmovepoint;
      if (_changedvalue != 0)
        StartCoroutine(statuschangedtexteffect(WNCText.GetMovepointColor(_changedvalue), MovePointText.rectTransform));
    }
    MovePoint_Icon.sprite = GameManager.Instance.MyGameData.MovePoint >0 ? GameManager.Instance.ImageHolder.MovePointIcon_Enable : GameManager.Instance.ImageHolder.MovePointIcon_Lack;
    MovePointText.text = GameManager.Instance.MyGameData.MovePoint.ToString();
    Debug.Log("이동력 수치 업데이트");

    Color _color = lastmovepoint < GameManager.Instance.MyGameData.MovePoint ? MovepointUpColor : MovepointDownColor;
    MovepointEffect_Image.color = _color;
    StartCoroutine(ChangeAlpha(MovepointEffect_Group, 0.0f, StatusEffectTime));
    
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
  [SerializeField] private TextMeshProUGUI ConversationLevel = null;
  [SerializeField] private TextMeshProUGUI ForceLevel = null;
  [SerializeField] private TextMeshProUGUI WildLevel = null;
  [SerializeField] private TextMeshProUGUI IntelligenceLevel = null;
  public void UpdateSkillLevel()
  {
    ConversationLevel.text = GameManager.Instance.MyGameData.Madness_Conversation ?
      WNCText.GetMadnessSkillColor(GameManager.Instance.MyGameData.Skill_Conversation.Level) :
      WNCText.UIIdleColor(GameManager.Instance.MyGameData.Skill_Conversation.Level);
    ForceLevel.text = GameManager.Instance.MyGameData.Madness_Force ?
      WNCText.GetMadnessSkillColor(GameManager.Instance.MyGameData.Skill_Force.Level) :
      WNCText.UIIdleColor(GameManager.Instance.MyGameData.Skill_Force.Level);
    WildLevel.text = GameManager.Instance.MyGameData.Madness_Wild ?
      WNCText.GetMadnessSkillColor(GameManager.Instance.MyGameData.Skill_Wild.Level) :
      WNCText.UIIdleColor(GameManager.Instance.MyGameData.Skill_Wild.Level);
    IntelligenceLevel.text = GameManager.Instance.MyGameData.Madness_Intelligence ?
     WNCText.GetMadnessSkillColor(GameManager.Instance.MyGameData.Skill_Intelligence.Level) :
     WNCText.UIIdleColor(GameManager.Instance.MyGameData.Skill_Intelligence.Level);
  }

  [Space(5)]
  [SerializeField] private RectTransform TendencyBodyRect = null;
  [SerializeField] private Image TendencyBodyIcon = null;
  [SerializeField] private RectTransform TendencyHeadRect = null;
  [SerializeField] private Image TendencyHeadIcon = null;
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
    TendencyBodyIcon.sprite = GameManager.Instance.ImageHolder.GetTendencyIcon(TendencyTypeEnum.Body, GameManager.Instance.MyGameData.Tendency_Body.Level);
    switch (GameManager.Instance.MyGameData.Tendency_Head.Level)
    {
      case -2: _headpos = TendencyPos_m2; break;
      case -1: _headpos = TendencyPos_m1; break;
      case 0: _headpos = new Vector2(0.0f, 0.0f);break;
      case 1: _headpos = TendencyPos_p1; break;
      case 2: _headpos = TendencyPos_p2; break;
    }
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
      titlerect.anchoredPosition = Vector2.Lerp(_originpos_title, _targetpos_title, _time / _targettime);

      _time += Time.deltaTime; yield return null;
    }

    titlerect.anchoredPosition = _targetpos_title;
  }
  [SerializeField] private RectTransform LongExpCover = null;
  [SerializeField] private TextMeshProUGUI LongExpTurn = null;
  private bool LongExpActive = false;
 // [SerializeField] private TextMeshProUGUI LongExpEffect = null;
  [SerializeField] private RectTransform ShortExpCover_A = null;
  [SerializeField] private TextMeshProUGUI ShortExpTurn_A= null;
  private bool ShortExpAActive = false;
  //[SerializeField] private TextMeshProUGUI ShortExpEffect_A = null;
  [SerializeField] private RectTransform ShortExpCover_B = null;
  [SerializeField] private TextMeshProUGUI ShortExpTurn_B = null;
  private bool ShortExpBActive = false;
  public Vector2 ExpCoverUpPos = new Vector2(0.0f, 81.6f);
 // [SerializeField] private TextMeshProUGUI ShortExpEffect_B = null;
  public void UpdateExpPael()
  {
    if (GameManager.Instance.MyGameData.LongExp == null)
    {
      LongExpTurn.text = "";

      if (LongExpActive == true) StartCoroutine(moverect(LongExpCover, ExpCoverUpPos, Vector2.zero, 0.4f, UIPanelCLoseCurve));
      LongExpActive = false;
    }
    else
    {
      LongExpTurn.text = GameManager.Instance.MyGameData.LongExp.Duration.ToString();

      if (LongExpActive == false) StartCoroutine(moverect(LongExpCover,Vector2.zero, ExpCoverUpPos, 0.4f, UIPanelOpenCurve));
      LongExpActive = true;
    }

    if (GameManager.Instance.MyGameData.ShortExp_A == null)
    {
      ShortExpTurn_A.text = "";

      if (ShortExpAActive == true) StartCoroutine(moverect(ShortExpCover_A, ExpCoverUpPos, Vector2.zero, 0.4f, UIPanelCLoseCurve));
      ShortExpAActive = false;
    }
    else
    {
      ShortExpTurn_A.text = GameManager.Instance.MyGameData.ShortExp_A.Duration.ToString();

      if (ShortExpAActive == false) StartCoroutine(moverect(ShortExpCover_A, Vector2.zero, ExpCoverUpPos, 0.4f, UIPanelOpenCurve));
      ShortExpAActive = true;
    }
    if (GameManager.Instance.MyGameData.ShortExp_B == null)
    {
      ShortExpTurn_B.text = "";

      if (ShortExpBActive == true) StartCoroutine(moverect(ShortExpCover_B, ExpCoverUpPos, Vector2.zero, 0.4f, UIPanelCLoseCurve));
      ShortExpBActive = false;
    }
    else
    {
      ShortExpTurn_B.text = GameManager.Instance.MyGameData.ShortExp_B.Duration.ToString();

      if (ShortExpBActive == false) StartCoroutine(moverect(ShortExpCover_B, Vector2.zero, ExpCoverUpPos, 0.4f, UIPanelOpenCurve));
      ShortExpBActive = true;
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
    UpdateExpPael();
    UpdateTendencyIcon();
    UpdateSkillLevel();
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
  #region 게임-엔딩 전환

  public void OpenEnding(Tuple<List<Sprite>, List<string>, string, string> data)
  {
    StopAllCoroutines();
    StartCoroutine(openending(data));
  }
  private IEnumerator openending(Tuple<List<Sprite>, List<string>, string, string> data)
  {
    StartCoroutine(ChangeAlpha(CenterGroup, 0.0f, 3.0f));

    var _titlewait = new WaitForSeconds(TitleWaitTime);
    var _objwait = new WaitForSeconds(ObjWaitTime);

    for (int i = TitlePanels.Count - 1; i > 3; i--)
    {
      StartCoroutine(moverect(TitlePanels[i].Rect, TitlePanels[i].InsidePos, TitlePanels[i].OutisdePos, SceneAnimationObjMoveTime, SceneAnimationCurve));
      yield return _objwait;
    }
    for (int i = 3; i > -1; i--)
    {
      StartCoroutine(moverect(TitlePanels[i].Rect, TitlePanels[i].InsidePos, TitlePanels[i].OutisdePos, SceneAnimationTitleMoveTime, SceneAnimationCurve));
      yield return _titlewait;
    }

    EndingUI.OpenUI_Ending(data);
  }
  #endregion
}
public static class WNCText
{
  public static string UIIdleColor(int value)
  {
    return $"<#D4D4D4>{value}</color>";
  }
  public static string UIIdleColor(string str)
  {
    return $"<#D4D4D4>{str}</color>";
  }
  public static string GetMadnessSkillColor(int value)
  {
    return $"<#A959B0>{value}</color>";
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
    return $"<#DA81F5>{str}</color>";
  }
  public static string GetDiscomfortColor(int value)
  {
    return $"<#DA81F5>{value}</color>";
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