using OpenCvSharp;
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
  [SerializeField] private AnimationCurve ExpandCurve = null;
  public IEnumerator ExpandRect(RectTransform rect,float targetsize, float targettime)
  {
    float _time = 0.0f;
    while (_time < targettime)
    {
      rect.localScale = Vector3.Lerp(Vector3.one, Vector3.one * targetsize, ExpandCurve.Evaluate(_time / targettime));
      _time += Time.deltaTime;
      yield return null;
    }
    rect.localScale = Vector3.one;
    yield return null;
  }
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
  public UI_Skill SkillUI = null;
  public UI_Exp ExpUI = null;
  public UI_Tendency TendencyUI = null;
  public UI_Status StatusUI = null;
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
      case 0:
        SpringIcon.sprite = GameManager.Instance.ImageHolder.SpringIcon_active;
        if(!SpringIcon.gameObject.activeSelf) SpringIcon.gameObject.SetActive(true);
        if (SummerIcon.gameObject.activeSelf) SummerIcon.gameObject.SetActive(false); 
        if (FallIcon.gameObject.activeSelf) FallIcon.gameObject.SetActive(false); 
        if (WinterIcon.gameObject.activeSelf) WinterIcon.gameObject.SetActive(false); 
        break;
      case 1:
        SpringIcon.sprite = GameManager.Instance.ImageHolder.SpringIcon_deactive;
        if (!SpringIcon.gameObject.activeSelf) SpringIcon.gameObject.SetActive(true);
        SummerIcon.sprite = GameManager.Instance.ImageHolder.SummerIcon_active;
        if (!SummerIcon.gameObject.activeSelf) SummerIcon.gameObject.SetActive(true);
        if (FallIcon.gameObject.activeSelf) FallIcon.gameObject.SetActive(false);
        if (WinterIcon.gameObject.activeSelf) WinterIcon.gameObject.SetActive(false);
        break;
      case 2:
        SpringIcon.sprite = GameManager.Instance.ImageHolder.SpringIcon_deactive;
        if (!SpringIcon.gameObject.activeSelf) SpringIcon.gameObject.SetActive(true);
        SummerIcon.sprite = GameManager.Instance.ImageHolder.SummerIcon_deactive;
        if (!SummerIcon.gameObject.activeSelf) SummerIcon.gameObject.SetActive(true);
        FallIcon.sprite = GameManager.Instance.ImageHolder.FallIcon_active;
        if (!FallIcon.gameObject.activeSelf) FallIcon.gameObject.SetActive(true);
        if (WinterIcon.gameObject.activeSelf) WinterIcon.gameObject.SetActive(false);
        break;
      case 3:
        SpringIcon.sprite = GameManager.Instance.ImageHolder.SpringIcon_deactive;
        if (!SpringIcon.gameObject.activeSelf) SpringIcon.gameObject.SetActive(true);
        SummerIcon.sprite = GameManager.Instance.ImageHolder.SummerIcon_deactive;
        if (!SummerIcon.gameObject.activeSelf) SummerIcon.gameObject.SetActive(true);
        FallIcon.sprite = GameManager.Instance.ImageHolder.FallIcon_deactive;
        if (!FallIcon.gameObject.activeSelf) FallIcon.gameObject.SetActive(true);
        WinterIcon.sprite = GameManager.Instance.ImageHolder.WinterIcon_active;
        if (!WinterIcon.gameObject.activeSelf) WinterIcon.gameObject.SetActive(true);
        break;
    }
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
  [Space(10)]
  [SerializeField] private float IconMoveTime_using = 1.0f;
  [SerializeField] private float IconMoveTime_gain = 1.0f;
  [SerializeField] private AnimationCurve IconUsingCurve = new AnimationCurve();
  [SerializeField] private AnimationCurve IconUsingScaleCurve=new AnimationCurve();
  [SerializeField] private List<RectTransform> IconRectList= new List<RectTransform>();
  private List<int> ActiveIconIndexList= new List<int>();
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

    SidePanelCultUI.UpdateProgressSlider();
  }

  public void UpdateAllUI()
  {
    UpdateYearText();
    UpdateTurnIcon();
    StatusUI.UpdateHPText(12);
    StatusUI.UpdateSanityText(12);
    StatusUI.UpdateGoldText(12);
    StatusUI.UpdateSupplyText(12);
    ExpUI.UpdateExpPanel();
    TendencyUI.UpdateTendencyIcon();
    SkillUI.UpdateSkillLevel();
    SkillUI.SetProgres();
    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        SidePanelCultUI.UpdateUI();
        SidePanelCultUI.UpdateProgressSlider();
        break;
    }
  }
  private void Update()
  {
#if UNITY_EDITOR
    if (Input.GetKeyDown(KeyCode.Home))
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
  [HideInInspector] public float InfoLength = 0.0f;
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
    GameManager.Instance.AddEndingProgress(data.ID);
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