using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using System.Reflection;
using OpenCvSharp.Flann;
using OpenCvSharp;
using System.Runtime.CompilerServices;

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
  public CanvasGroup CenterGroup = null;
  public CanvasGroup CurtainGroup = null;
  public TextMeshProUGUI CurtainText = null;
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
  public void UpdateBackground(EnvironmentType envir)
  {
    Sprite _newbackground = GameManager.Instance.ImageHolder.GetEnvirBackground(envir);
    EnvirBackground.Next( _newbackground,1.0f);
  }
  public void OffBackground()
  {
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
  [SerializeField] private float StatusGainTime = 0.8f;
  [SerializeField] private float StatusGainSize = 1.4f;
  [SerializeField] private float StatusLossTime = 0.8f;
  [SerializeField] private AnimationCurve StatusGainCurve=new AnimationCurve();
  [SerializeField] private AnimationCurve StatusLossCurve=new AnimationCurve();
  private IEnumerator statusgainanimation(List<RectTransform> rectlist)
  {
    float _time = 0.0f;
    Vector3 _currentscale = Vector3.one;

    while (_time < StatusGainTime)
    {
      _currentscale= Vector3.Lerp(Vector3.one, Vector3.one * StatusGainSize, StatusGainCurve.Evaluate(_time / StatusGainTime));
      foreach(var rect in rectlist)
        rect.localScale = _currentscale;
      _time += Time.deltaTime;
      yield return null;
    }
    foreach (var rect in rectlist)
      rect.localScale = Vector3.one;
  }
  private IEnumerator statuslossanimation(List<RectTransform> rectlist,float losssscalesize)
  {
    float _time = 0.0f;
    Vector3 _currentscale = Vector3.one * losssscalesize;
    while (_time < StatusLossTime)
    {
      _currentscale = Vector3.Lerp(Vector3.one, Vector3.one * losssscalesize, StatusLossCurve.Evaluate(_time / StatusLossTime));
      foreach (var rect in rectlist)
        rect.localScale = _currentscale;
      _time += Time.deltaTime;
      yield return null;
    }
    foreach (var rect in rectlist)
      rect.localScale = Vector3.one;
  }
  public IEnumerator statusgainanimation(RectTransform rect)
  {
    float _time = 0.0f;
    Vector3 _currentscale = Vector3.one;

    while (_time < StatusGainTime)
    {
      _currentscale = Vector3.Lerp(Vector3.one, Vector3.one * StatusGainSize, StatusGainCurve.Evaluate(_time / StatusGainTime));
        rect.localScale = _currentscale;
      _time += Time.deltaTime;
      yield return null;
    }
      rect.localScale = Vector3.one;
  }
  private IEnumerator statuslossanimation(RectTransform rect,float losssscalesize)
  {
    float _time = 0.0f;
    Vector3 _currentscale = Vector3.one * losssscalesize;
    while (_time < StatusLossTime)
    {
      _currentscale = Vector3.Lerp(Vector3.one, Vector3.one * losssscalesize, StatusLossCurve.Evaluate(_time / StatusLossTime));
        rect.localScale = _currentscale;
      _time += Time.deltaTime;
      yield return null;
    }
      rect.localScale = Vector3.one;
  }
  [SerializeField] private AnimationCurve StatusEffectLossCurve = new AnimationCurve();
  [SerializeField] private TextMeshProUGUI HPText = null;
  private int lasthp = -1;
  public void UpdateHPText()
  {
    if (!lasthp.Equals(-1))
    {
      int _changedvalue = GameManager.Instance.MyGameData.HP - lasthp;
      // if(_changedvalue!=0)
      // StartCoroutine(statuschangedtexteffect(WNCText.GetHPColor(_changedvalue), HPText.rectTransform));
      if (lasthp < GameManager.Instance.MyGameData.HP) StartCoroutine(statusgainanimation(new List<RectTransform> { HPIcon.rectTransform, HPText.rectTransform }));
      else
      {
        StartCoroutine(statuslossanimation(new List<RectTransform> { HPIcon.rectTransform, HPText.rectTransform },
          Mathf.Lerp(ConstValues.StatusLossMinSacle,ConstValues.StatusLossMaxScale,(Mathf.Abs(_changedvalue)-ConstValues.StatusLoss_HP_Min)/ConstValues.StatusLoss_HP_Max)));
        AudioManager.PlaySFX(15,"status");
      }

      HighlightManager.HighlightAnimation(HighlightEffectEnum.HP);
    }


    HPIcon.rectTransform.sizeDelta = Vector2.one * Mathf.Lerp( ConstValues.StatusIconSize_min, ConstValues.StatusIconSize_max, GameManager.Instance.MyGameData.HP / 100.0f);
    HPText.text = GameManager.Instance.MyGameData.HP.ToString();
  //  Debug.Log("체력 수치 업데이트");

    lasthp = GameManager.Instance.MyGameData.HP;
    UpdateHPIcon();
  }
  public void UpdateHPIcon()
  {
    HPIcon.sprite = GameManager.Instance.MyGameData.MadnessSafe ?
      GameManager.Instance.ImageHolder.HPIcon : GameManager.Instance.ImageHolder.HPBroken;
  }
  [SerializeField] private TextMeshProUGUI SanityText_current = null;
 // [SerializeField] private TextMeshProUGUI SanityText_max = null;
  private int lastsanity = -1;
  public void UpdateSanityText()
  {
    if (!lastsanity.Equals(-1))
    {
      int _changedvalue = GameManager.Instance.MyGameData.Sanity - lastsanity;
      // if (_changedvalue != 0)
      //   StartCoroutine(statuschangedtexteffect(WNCText.GetSanityColor(_changedvalue), SanityText_current.rectTransform));
      if (lastsanity < GameManager.Instance.MyGameData.Sanity)
      {
        StartCoroutine(statusgainanimation(new List<RectTransform> { SanityIconRect, SanityText_current.rectTransform }));
        AudioManager.PlaySFX(16, "status");
      }
      else
      {
        StartCoroutine(statuslossanimation(new List<RectTransform> { SanityIconRect, SanityText_current.rectTransform },
          Mathf.Lerp(ConstValues.StatusLossMinSacle, ConstValues.StatusLossMaxScale, (Mathf.Abs(_changedvalue) - ConstValues.StatusLoss_Sanity_Min) / ConstValues.StatusLoss_Sanity_Max)));
        AudioManager.PlaySFX(17, "status");
      }

      HighlightManager.HighlightAnimation(HighlightEffectEnum.Sanity);
    }

    SanityIconRect.sizeDelta = Vector2.one * Mathf.Lerp(ConstValues.StatusIconSize_min, ConstValues.StatusIconSize_max, GameManager.Instance.MyGameData.Sanity / 100.0f);
    SanityText_current.text = GameManager.Instance.MyGameData.Sanity>100?
      WNCText.GetMaxSanityColor(GameManager.Instance.MyGameData.Sanity.ToString()): GameManager.Instance.MyGameData.Sanity.ToString();
   // SanityText_max.text = GameManager.Instance.MyGameData.MaxSanity.ToString();
 //   Debug.Log("정신력, 최대 정신력 수치 업데이트");


    lastsanity = GameManager.Instance.MyGameData.Sanity;
  }
  [SerializeField] private TextMeshProUGUI GoldText = null;
  private int lastgold = -1;
  public void UpdateGoldText()
  {
    if (!lastgold.Equals(-1))
    {
      int _changedvalue = GameManager.Instance.MyGameData.Gold - lastgold;
      //  if (_changedvalue != 0)
      //   StartCoroutine(statuschangedtexteffect(WNCText.GetGoldColor(_changedvalue), GoldText.rectTransform));
      if (lastgold < GameManager.Instance.MyGameData.Gold)
      {
        StartCoroutine(statusgainanimation(new List<RectTransform> { GoldIconRect, GoldText.rectTransform }));
        AudioManager.PlaySFX(18,"status");
      }
      else
      {
        StartCoroutine(statuslossanimation(new List<RectTransform> { GoldIconRect, GoldText.rectTransform },
          Mathf.Lerp(ConstValues.StatusLossMinSacle, ConstValues.StatusLossMaxScale, (Mathf.Abs(_changedvalue) - ConstValues.StatusLoss_Gold_Min) / ConstValues.StatusLoss_Gold_Max)));
      }

      HighlightManager.HighlightAnimation(HighlightEffectEnum.Gold);
    }

    GoldText.text = GameManager.Instance.MyGameData.Gold.ToString();
  //  Debug.Log("골드 수치 업데이트");


    lastgold = GameManager.Instance.MyGameData.Gold;
  }
  [SerializeField] private Image MovePoint_Icon = null;
  [SerializeField] private TextMeshProUGUI MovePointText = null;
  private int lastmovepoint = -1;
  public void UpdateMovePointText()
  {
    if (lastmovepoint != -1)
    {
      int _changedvalue = GameManager.Instance.MyGameData.Supply - lastmovepoint;
   //   if (_changedvalue != 0)
    //    StartCoroutine(statuschangedtexteffect(WNCText.GetMovepointColor(_changedvalue), MovePointText.rectTransform));
      if (lastmovepoint < GameManager.Instance.MyGameData.Supply) StartCoroutine(statusgainanimation(new List<RectTransform> { MovepointIconRect, MovePointText.rectTransform }));
      else StartCoroutine(statuslossanimation(new List<RectTransform> { MovepointIconRect, MovePointText.rectTransform},
        Mathf.Lerp(ConstValues.StatusLossMinSacle, ConstValues.StatusLossMaxScale, (Mathf.Abs(_changedvalue) - ConstValues.StatusLoss_MP_Min) / ConstValues.StatusLoss_MP_Max)));

      HighlightManager.HighlightAnimation(HighlightEffectEnum.Movepoint);
    }

    MovePoint_Icon.sprite = GameManager.Instance.MyGameData.Supply >0 ? GameManager.Instance.ImageHolder.MovePointIcon_Enable : GameManager.Instance.ImageHolder.MovePointIcon_Lack;

  //  Debug.Log("이동력 수치 업데이트");
    if (lastmovepoint == 0 && GameManager.Instance.MyGameData.Supply == 0) return;

    MovepointIconRect.sizeDelta = Vector2.one * Mathf.Lerp(ConstValues.StatusIconSize_min, ConstValues.StatusIconSize_max,
      (GameManager.Instance.MyGameData.Supply - ConstValues.SupplyIconMinCount) / (float)(ConstValues.SupplyIconMaxCount-ConstValues.SupplyIconMinCount));
    MovePointText.text = GameManager.Instance.MyGameData.Supply.ToString();

    lastmovepoint = GameManager.Instance.MyGameData.Supply;
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
      GameManager.Instance.MyGameData.TotalRestCount % ConstValues.MadnessEffect_Force == ConstValues.MadnessEffect_Force - 1 ?
      WNCText.GetMadnessColor((GameManager.Instance.MyGameData.TotalRestCount % ConstValues.MadnessEffect_Force + 1).ToString() + "/" + ConstValues.MadnessEffect_Force.ToString()) :
      ((GameManager.Instance.MyGameData.TotalRestCount % ConstValues.MadnessEffect_Force + 1).ToString() + "/" + ConstValues.MadnessEffect_Force.ToString());
    StartCoroutine(madcounttext(ForceMadCountText.rectTransform));
  }
  public void SetWildMadCount()
  {
    WildMadCountText.text =
      GameManager.Instance.MyGameData.TotalRestCount % ConstValues.MadnessEffect_Wild == ConstValues.MadnessEffect_Wild - 1 ?
      WNCText.GetMadnessColor((GameManager.Instance.MyGameData.TotalRestCount % ConstValues.MadnessEffect_Wild + 1).ToString() + "/" + ConstValues.MadnessEffect_Wild.ToString()) :
      ((GameManager.Instance.MyGameData.TotalRestCount % ConstValues.MadnessEffect_Wild + 1).ToString() + "/" + ConstValues.MadnessEffect_Wild.ToString());
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
  /// <summary>
  /// 기술
  /// </summary>
  /// <param name="isusing"></param>
  /// <param name="skilltype"></param>
  /// <param name="rect"></param>
  /// <returns></returns>
  public IEnumerator SetIconEffect(bool isusing, SkillTypeEnum skilltype, RectTransform rect)
  {
    Vector2 _startpos = Vector2.zero, _endpos = Vector2.zero;
    Sprite _icon = null;
    switch (skilltype)
    {
      case SkillTypeEnum.Conversation:
        _icon = GameManager.Instance.ImageHolder.SkillIcon_Conversation_w;
        _startpos = isusing ? ConversationIconRect.position : rect.position;
        _endpos = isusing ? rect.position : ConversationIconRect.position;
        break;
      case SkillTypeEnum.Force:
        _icon = GameManager.Instance.ImageHolder.SkillIcon_Force_w;
        _startpos = isusing ? ForceIconRect.position : rect.position;
        _endpos = isusing ? rect.position : ForceIconRect.position;
        break;
      case SkillTypeEnum.Wild:
        _icon = GameManager.Instance.ImageHolder.SkillIcon_Wild_w;
        _startpos = isusing ? WildIconRect.position : rect.position;
        _endpos = isusing ? rect.position : WildIconRect.position;
        break;
      case SkillTypeEnum.Intelligence:
        _icon = GameManager.Instance.ImageHolder.SkillIcon_Intelligence_w;
        _startpos = isusing ? IntelligenceIconRect.position : rect.position;
        _endpos = isusing ? rect.position : IntelligenceIconRect.position;
        break;
    }
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
    AnimationCurve _curve = isusing ? IconUsingCurve : IconGainCurve;
    float _targettime = isusing ? IconMoveTime_using : IconMoveTime_gain;
    while (_time < _targettime)
    {

      _iconrect.position = Vector3.Lerp(_startpos, _endpos, _curve.Evaluate(_time / _targettime));
      _iconrect.anchoredPosition3D = new Vector3(_iconrect.anchoredPosition3D.x, _iconrect.anchoredPosition3D.y, 0.0f);
     _iconrect.localScale = Vector3.one * (1.0f - IconUsingScaleCurve.Evaluate(_time / _targettime));

      _time += Time.deltaTime;
      yield return null;
    }
    _iconrect.anchoredPosition = Vector2.one * 3000.0f;
    ActiveIconIndexList.Remove(_index);

    GameManager.Instance.MyGameData.GetSkill(skilltype).LevelByDefault++;
    UIManager.Instance.AudioManager.PlaySFX(19);
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
    Sprite _icon = GameManager.Instance.ImageHolder.MovePointIcon_Enable;
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
  /// <summary>
  /// 이동력
  /// </summary>
  /// <param name="tendencytype"></param>
  /// <param name="dir"></param>
  /// <param name="rect"></param>
  /// <returns></returns>
  public IEnumerator SetIconEffect_movepoint_using(Dictionary<TileData,int> rectNcount,StatusTypeEnum status)
  {
    foreach(var _data in rectNcount)
    {
      Vector2 _startpos = MovepointIconRect.position, _endpos = _data.Key.ButtonScript.Rect.position;
      Sprite _icon = GameManager.Instance.ImageHolder.MovePointIcon_Enable;

      for (int j = 0; j < _data.Key.MovePoint; j++)
      {
        int _index = 0;
        _icon = j< _data.Value? GameManager.Instance.ImageHolder.MovePointIcon_Enable:
          status==StatusTypeEnum.Sanity?GameManager.Instance.ImageHolder.SanityIcon:GameManager.Instance.ImageHolder.GoldIcon;
        _startpos = j < _data.Value ? MovepointIconRect.position:
          status==StatusTypeEnum.Sanity?SanityIconRect.position:GoldIconRect.position;
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
        yield return new WaitForSeconds(0.08f);
      }
      yield return new WaitForSeconds(0.1f);
    }
  }
  public void SetRitualFail()
  {
    StartCoroutine(SetIconEffect_movepoint_ritualfail(CultSidepanelOpenpos,ConstValues.Quest_Cult_Ritual_PenaltySupply));
  }
  public IEnumerator SetIconEffect_movepoint_ritualfail(RectTransform endrect,int count)
  {
    Vector2 _startpos = MovepointIconRect.position, _endpos = endrect.position;
    Sprite _icon = GameManager.Instance.ImageHolder.MovePointIcon_Enable;

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
    AudioManager.PlaySFX(30);
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
        StartCoroutine(statusgainanimation(new List<RectTransform> { ConversationLevel.rectTransform, ConversationEffectGroup.transform as RectTransform}));
        StartCoroutine(ChangeAlpha(ConversationEffectGroup, 0.0f, StatusGainTime));
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
        StartCoroutine(statusgainanimation(new List<RectTransform> { ForceLevel.rectTransform, ForceEffectGroup.transform as RectTransform }));
        StartCoroutine(ChangeAlpha(ForceEffectGroup, 0.0f, StatusGainTime));
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
        StartCoroutine(statusgainanimation(new List<RectTransform> { WildLevel.rectTransform, WildEffectGroup.transform as RectTransform }));
        StartCoroutine(ChangeAlpha(WildEffectGroup, 0.0f, StatusGainTime));
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
        StartCoroutine(statusgainanimation(new List<RectTransform> { IntelligenceLevel.rectTransform, IntelligenceEffectGroup.transform as RectTransform }));
        StartCoroutine(ChangeAlpha(IntelligenceEffectGroup, 0.0f, StatusGainTime));
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
  private bool LongExpActive = false;
  [SerializeField] private Button ShortExpButton_A = null;
  [SerializeField] private RectTransform ShortExpCover_A = null;
  [SerializeField] private TextMeshProUGUI ShortExpTurn_A= null;
  [SerializeField] private CanvasGroup ShortMad_A = null;
  [SerializeField] private CanvasGroup ExpUse_Short_A_Group = null;
  [SerializeField] private Image ExpUse_Short_A_img = null;
  [SerializeField] private TextMeshProUGUI ExpUse_Short_A_text = null;
  private bool ShortExpAActive = false;
  [SerializeField] private Button ShortExpButton_B = null;
  [SerializeField] private RectTransform ShortExpCover_B = null;
  [SerializeField] private TextMeshProUGUI ShortExpTurn_B = null;
  [SerializeField] private CanvasGroup ShortMad_B = null;
  [SerializeField] private CanvasGroup ExpUse_Short_B_Group = null;
  [SerializeField] private Image ExpUse_Short_B_img = null;
  [SerializeField] private TextMeshProUGUI ExpUse_Short_B_text = null;
  private bool ShortExpBActive = false;
  [SerializeField] private Color ExpUseRefuseColor = new Color();
  [SerializeField] private float UsedTextWarningSize = 1.4f;
  [SerializeField] private float UsedTextWarningTime = 0.5f;
  [SerializeField] private float ExpUsingTime = 0.6f;
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
      Invoke("UpdateExpPael", ExpUsingTime);
    }
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
  public void UpdateExpMad()
  {
    if (GameManager.Instance.MyGameData.LongExp != null && GameManager.Instance.MyGameData.LongExp.Duration > (ConstValues.MadnessEffect_Intelligence-1))
      StartCoroutine(ChangeAlpha(LongMad, 0.0f, 1.0f));
    if (GameManager.Instance.MyGameData.ShortExp_A != null && GameManager.Instance.MyGameData.ShortExp_A.Duration > (ConstValues.MadnessEffect_Intelligence - 1))
      StartCoroutine(ChangeAlpha(ShortMad_A, 0.0f, 1.0f));
    if (GameManager.Instance.MyGameData.ShortExp_B != null && GameManager.Instance.MyGameData.ShortExp_B.Duration > (ConstValues.MadnessEffect_Intelligence - 1))
      StartCoroutine(ChangeAlpha(ShortMad_B, 0.0f, 1.0f));
  }
  public void UpdateExpPael()
  {
    bool _starteffect = false;
    bool _turnchanged = false;

    if (GameManager.Instance.MyGameData.LongExp == null)
    {
      LongExpTurn.text = "";

      if (LongExpActive == true)
      {
        StartCoroutine(moverect(LongExpCover, ExpCoverUpPos, Vector2.zero, 0.4f, UIPanelCLoseCurve));
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
        StartCoroutine(statusgainanimation(LongExpTurn.rectTransform));
        AudioManager.PlaySFX(20);
      }
      else
      {
        _turnchanged = int.Parse(LongExpTurn.text) != GameManager.Instance.MyGameData.LongExp.Duration;
        if (_turnchanged) StartCoroutine(statuslossanimation(LongExpTurn.rectTransform,1.2f));
      }
      LongExpTurn.text = GameManager.Instance.MyGameData.LongExp.Duration.ToString();
      LongExpActive = true;
    }

    if (GameManager.Instance.MyGameData.ShortExp_A == null)
    {
      ShortExpTurn_A.text = "";

      if (ShortExpAActive == true)
      {
        StartCoroutine(moverect(ShortExpCover_A, ExpCoverUpPos, Vector2.zero, 0.4f, UIPanelCLoseCurve));
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
        StartCoroutine(statusgainanimation(ShortExpTurn_A.rectTransform));
        AudioManager.PlaySFX(20);
      }
      else
      {
        _turnchanged = int.Parse(ShortExpTurn_A.text) != GameManager.Instance.MyGameData.ShortExp_A.Duration;
        if (_turnchanged)StartCoroutine(statuslossanimation(ShortExpTurn_A.rectTransform,1.2f));
      }
      ShortExpTurn_A.text = GameManager.Instance.MyGameData.ShortExp_A.Duration.ToString();
      ShortExpAActive = true;
    }
    if (GameManager.Instance.MyGameData.ShortExp_B == null)
    {
      ShortExpTurn_B.text = "";

      if (ShortExpBActive == true) 
      {
        StartCoroutine(moverect(ShortExpCover_B, ExpCoverUpPos, Vector2.zero, 0.4f, UIPanelCLoseCurve));
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
        StartCoroutine(statusgainanimation(ShortExpTurn_B.rectTransform));
        AudioManager.PlaySFX(20);
      }
      else
      {
        _turnchanged = int.Parse(ShortExpTurn_B.text) != GameManager.Instance.MyGameData.ShortExp_B.Duration;
        if (_turnchanged) StartCoroutine(statuslossanimation(ShortExpTurn_B.rectTransform,1.2f));
      }
      ShortExpTurn_B.text = GameManager.Instance.MyGameData.ShortExp_B.Duration.ToString();
      ShortExpBActive = true;
    }
    if (_starteffect) HighlightManager.HighlightAnimation(HighlightEffectEnum.Exp);

    UpdateHPIcon();
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
    UpdateHPText();
    UpdateSanityText();
    UpdateGoldText();
    UpdateMovePointText();
    UpdateExpPael();
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
    if (Input.GetKeyDown(KeyCode.F12))
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