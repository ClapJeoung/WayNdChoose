using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum UIMoveDir { Horizontal, Vertical }
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
  [SerializeField] private Transform MyCanvas = null;
  [SerializeField] private UI_dialogue MyDialogue = null;
  [SerializeField] private UI_Reward MyUIReward = null;
  [SerializeField] private UI_trait MyUITrait = null;
  [SerializeField] private UI_EventSuggest MyEvnetSuggest = null;
  [SerializeField] private UI_QuestSuggest MyQuestSuggent = null;
  [SerializeField] private RectTransform HpRect = null;
  [SerializeField] private RectTransform SanityRect = null;
  [SerializeField] private RectTransform GoldRect = null;
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
  public PreviewManager PreviewManager = null;
  [Space(10)]
  [SerializeField] private TextMeshProUGUI YearText = null;
  public void UpdateYearText() => YearText.text = GameManager.Instance.MyGameData.Year.ToString();
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
      if (_value < 0) { StartCoroutine(lossstatus(_value, HpRect.position)); HPLossParticle.Play(); }
      else if(_value > 0){ StartCoroutine(genstatus(_value, HpRect.position));HPGenParticle.Play(); }
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
      if (_value < 0) { StartCoroutine(lossstatus(_value, SanityRect.position)); SanityLossParticle.Play();}
      else if(_value>0) { StartCoroutine(genstatus(_value, SanityRect.position)); SanityGenParticle.Play(); }
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
      if (_value < 0) { StartCoroutine(lossstatus(_value, GoldRect.position)); GoldLossParticle.Play(); }
      else if (_value > 0) { StartCoroutine(genstatus(_value, GoldRect.position)); GoldGenParticle.Play(); }
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
  [SerializeField] private Image RationalIcon = null;
  [SerializeField] private Image PhysicalIcon = null;
  [SerializeField] private Image MentalIcon = null;
  [SerializeField] private Image MaterialIcon = null;
  [SerializeField] private Transform TraitHolder= null;
  public void UpdateTraitIcon()
  {
    List<Trait> _alltraits = GameManager.Instance.MyGameData.Traits;
    List<string> _currenticons=new List<string>();
    for (int i = 0; i < TraitHolder.childCount; i++) _currenticons.Add(TraitHolder.GetChild(i).name);
    foreach(var _trait in _alltraits)
    {
      if (+_currenticons.Count>0&&_currenticons.Contains(_trait.ID)) continue;
      System.Type[] _buttontype = new System.Type[] 
      { typeof(RectTransform), typeof(Image), typeof(Button), typeof(CanvasRenderer),typeof(PreviewInteractive),typeof(TraitIconButton) };
      Debug.Log(_trait.ID);
      GameObject _newicon = new GameObject(_trait.ID, _buttontype);
      Image _image = _newicon.GetComponent<Image>();
      _image.sprite = _trait.Icon;
      Button _button = _newicon.GetComponent<Button>();
      _button.image = _image;
      TraitIconButton _iconbutton= _newicon.GetComponent<TraitIconButton>();
      _iconbutton.MyUITrait = MyUITrait;
      _iconbutton.MyTrait = _trait;
      _button.onClick.AddListener(_iconbutton.Onclick);
      PreviewInteractive _previewinteractive = _newicon.GetComponent<PreviewInteractive>();
      _previewinteractive.PanelType = PreviewPanelType.Trait;
      _previewinteractive.MyTrait = _trait;
      _newicon.transform.SetParent(TraitHolder);
      _newicon.GetComponent<RectTransform>().localScale = Vector3.one;
    }
  }
  public void UpdateTendencyIcon()
  {
    TendencyType _tendency = TendencyType.Rational;
    int _level = GameManager.Instance.MyGameData.GetTendencyLevel(_tendency);
    RationalIcon.sprite = GameManager.Instance.ImageHolder.GetTendencyIcon(_tendency, _level);

    _tendency = TendencyType.Physical; _level = GameManager.Instance.MyGameData.GetTendencyLevel(_tendency);
    PhysicalIcon.sprite = GameManager.Instance.ImageHolder.GetTendencyIcon(_tendency, _level);
 
    _tendency = TendencyType.Mental; _level = GameManager.Instance.MyGameData.GetTendencyLevel(_tendency);
    MentalIcon.sprite = GameManager.Instance.ImageHolder.GetTendencyIcon(_tendency, _level);
 
    _tendency = TendencyType.Material; _level = GameManager.Instance.MyGameData.GetTendencyLevel(_tendency);
    MaterialIcon.sprite = GameManager.Instance.ImageHolder.GetTendencyIcon(_tendency, _level);
  }
  [SerializeField] private Image[] LongTermIcon = new Image[2];
  public void UpdateExpLongTermIcon()
  {
    for(int i=0; i < LongTermIcon.Length; i++)
    {
      if (GameManager.Instance.MyGameData.LongTermEXP[i] == null) LongTermIcon[i].sprite = GameManager.Instance.ImageHolder.EmptyExpIcon;
      else LongTermIcon[i].sprite = GameManager.Instance.ImageHolder.ExistExpIcon;
    }
  }
  [SerializeField] private Image[] ShortTermIcon = new Image[4];
  public void UpdateExpShortTermIcon()
  {
    for (int i = 0; i < ShortTermIcon.Length; i++)
    {
      if (GameManager.Instance.MyGameData.ShortTermEXP[i] == null) ShortTermIcon[i].sprite = GameManager.Instance.ImageHolder.EmptyExpIcon;
      else ShortTermIcon[i].sprite = GameManager.Instance.ImageHolder.ExistExpIcon;
    }
  }
  private UI_default LastUI = null;
  public void UpdateAllUI()
  {
    UpdateYearText();
    UpdateTurnIcon();
    UpdateHPText();
    UpdateSanityText();
    UpdateGoldText();
    UpdateTraitIcon();
    UpdateExpLongTermIcon();
    UpdateExpShortTermIcon();
    UpdateTendencyIcon();
  }
  private void Update()
  {
    if (Input.GetMouseButtonDown(1) && IsWorking == false) StartCoroutine(CloseAllUI());
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
   public IEnumerator OpenUI(RectTransform _rect,CanvasGroup _group,UIMoveDir _dir,bool _islarge)
  {
    if(LastUI!=null) _rect.transform.SetSiblingIndex(LastUI.transform.GetSiblingIndex() + 1);
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
      _rect.anchoredPosition=Vector2.Lerp(_startpos, _endpos, Mathf.Pow(_time/ _targetime, 0.5f));
      _group.alpha = Mathf.Lerp(MoveInAlpha, 1.0f, Mathf.Pow(_time / _targetime, 0.5f));
      _time += Time.deltaTime;
      yield return null;
    }
        _rect.anchoredPosition = _endpos;
    _group.blocksRaycasts = true;
    _group.alpha = 1.0f;
    _group.interactable = true;

    if(LastUI!=null) LastUI.CloseUI();
    if (_rect == MyQuestSuggent.MyRect || _rect == MyEvnetSuggest.MyRect) yield break;
    LastUI = _rect.GetComponent<UI_default>();
  }
    public IEnumerator OpenUI(CanvasGroup _group,bool _islarge)
  {
    if (_group.gameObject.activeSelf == false) _group.gameObject.SetActive(true);

    _group.alpha = MoveInAlpha;
    _group.blocksRaycasts = false;
    _group.interactable = false;

    float _time = 0.0f;
    float _tagettime = _islarge ? LargePanelFadeTime : SmallPanelFadeTime;
    while (_time < _tagettime)
    {
      _group.alpha = Mathf.Lerp(MoveInAlpha, 1.0f, Mathf.Pow(_time / _tagettime, 0.5f));
      _time += Time.deltaTime;
      yield return null;
    }
    _group.blocksRaycasts = true;
    _group.alpha = 1.0f;
    _group.interactable = true;

  }
    public IEnumerator CloseAllUI()
  {
    if (LastUI != null)
    {
      LastUI.CloseUI();
      LastUI = null;
      yield return null;
    }
    yield return null;
  }
    public IEnumerator CloseUI(RectTransform _rect, CanvasGroup _group, UIMoveDir _dir)
  {
        LastUI = null;
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
      _rect.anchoredPosition = Vector2.Lerp(_startpos, _endpos, Mathf.Pow(_time / LargePanelFadeTime, 0.5f));
      _group.alpha = Mathf.Lerp(1.0f, 0.0f, Mathf.Pow(_time / LargePanelFadeTime, 0.5f));
      _time += Time.deltaTime;
      yield return null;
    }
    _group.alpha = 0.0f;
  }
    public IEnumerator CloseUI(CanvasGroup _group,bool _islarge)
  {
        LastUI = null;
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
    public IEnumerator ChangeAlpha(Image _img, float _targetalpha)
  {
    float _startalpha = _targetalpha==1.0f ? 0.0f : 1.0f;
    float _endalpha = _targetalpha==1.0f ? 1.0f : 0.0f;
    float _time = 0.0f;
    Color _color = Color.white; 
    float _alpha = _startalpha;
    _color.a = _alpha;
    _img.color = _color;
    while (_time < IllustFadeTime)
    {
      _alpha = Mathf.Lerp(_startalpha, _endalpha, Mathf.Pow(_time / IllustFadeTime, 0.6f));
      _color.a = _alpha;
      _img.color = _color;
      _time += Time.deltaTime;
      yield return null;
    }
    _alpha = _endalpha;
    _color.a = _alpha;
    _img.color= _color;
  }
    public IEnumerator ChangeAlpha(CanvasGroup _group, float _targetalpha,bool _islarge)
  {
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
      _alpha = Mathf.Lerp(_startalpha, _endalpha, Mathf.Pow(_time / _targettime, 0.6f));
      _group.alpha = _alpha;
      _time += Time.deltaTime;
      yield return null;
    }
    _alpha = _endalpha;
    _group.alpha = _alpha;
    _group.interactable = true;
    _group.blocksRaycasts = true;
  }
    public IEnumerator ChangeAlpha(TextMeshProUGUI _tmp, float _targetalpha)
  {
    float _startalpha = _targetalpha == 1.0f ? 0.0f : 1.0f;
    float _endalpha = _targetalpha == 1.0f ? 1.0f : 0.0f;
    float _time = 0.0f;
    Color _color = Color.white;
    float _alpha = _startalpha;
    _color.a = _alpha;
    _tmp.color = _color;
    float _targettime=_targetalpha==1.0f?TextFadeInTime:TextFadeOutTime;
    while (_time < _targettime)
    {
      _alpha = Mathf.Lerp(_startalpha, _endalpha, Mathf.Pow(_time / _targettime, 0.6f));
      _color.a = _alpha;
      _tmp.color = _color;
      _time += Time.deltaTime;
      yield return null;
    }
    _alpha = _endalpha;
    _color.a = _alpha;
    _tmp.color = _color;
  }

  public void UpdateMap_SettlePanel(Settlement _settle) => MyMap.UpdatePanel(_settle);
  public void UpdateMap_SettleIcons(List<Settlement> _avail) => MyMap.UpdateIcons(_avail);
  public void UpdateMap_AddSettle(string _name, SettlementIcon _icon) => MyMap.AddSettleIcon(_name, _icon);
  public void UpdateMap_SetPlayerPos(Settlement _settle)=>MyMap.SetPlayerPos(_settle);
  public void OpenSuggestUI() => MyEvnetSuggest.OpenSuggest();
  public void OpenBadExpUI(Experience _badexp) => MyUIReward.OpenRewardExpPanel_penalty(_badexp);
  public void OpenDialogue() =>MyDialogue.SetEventDialogue();
  public void OpenSuccessDialogue(SuccessData _success) => MyDialogue.SetEventDialogue(_success);
  public void OpenFailDialogue(FailureData _fail) => MyDialogue.SetEventDialogue(_fail);

  public void OpenQuestDialogue() => MyQuestSuggent.OpenQuestSuggestUI();
  public void ResetEventPanels()
  {
    MyDialogue.ResetPanel();
    MyEvnetSuggest.CloseSuggestPanel();
    MyQuestSuggent.CloseQuestUI();
  }//이벤트 패널,리스트 패널,퀘스트 패널을 처음 상태로 초기화(맵 이동할 때 마다 호출)
}
