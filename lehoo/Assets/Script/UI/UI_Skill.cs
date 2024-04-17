using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Skill : MonoBehaviour
{
  [SerializeField] private float SkillIconGainTime = 0.6f;
  [Space(5)]
  public RectTransform ConversationIconRect = null;
  [SerializeField] private TextMeshProUGUI ConversationLevel = null;
  [SerializeField] private CanvasGroup ConversationEffectGroup = null;
  [SerializeField] private CanvasGroup ConvLevelUp = null;
  [Space(5)]
  [SerializeField] private RectTransform ForceIconRect = null;
  [SerializeField] private TextMeshProUGUI ForceLevel = null;
  [SerializeField] private CanvasGroup ForceEffectGroup = null;
  [SerializeField] private CanvasGroup ForceLevelUp = null;
  [Space(5)]
  [SerializeField] private RectTransform WildIconRect = null;
  [SerializeField] private TextMeshProUGUI WildLevel = null;
  [SerializeField] private CanvasGroup WildEffectGroup = null;
  [SerializeField] private CanvasGroup WildLevelUp = null;
  [Space(5)]
  [SerializeField] private TextMeshProUGUI IntelligenceLevel = null;
  [SerializeField] private RectTransform IntelligenceIconRect = null;
  [SerializeField] private CanvasGroup IntelligenceEffectGroup = null;
  [SerializeField] private CanvasGroup IntelLevelUp = null;
  [Space(5)]
  [SerializeField] private TextMeshProUGUI ForceMadCountText = null;
  [SerializeField] private TextMeshProUGUI WildMadCountText = null;
  [SerializeField] private float MadCountOpenTime = 0.5f;
  [SerializeField] private float MadCountWaitTime = 1.0f;
  [SerializeField] private float MadCountCloseTime = 0.5f;
  [SerializeField] private AnimationCurve MadCountAnimationCurve = null;
  [SerializeField] private float LevelUpSize = 1.35f;
  [SerializeField] private float LevelUpTime = 0.7f;
  public void SetForceMadCount()
  {
    ForceMadCountText.text =
      GameManager.Instance.MyGameData.TotalRestCount % GameManager.Instance.Status.MadnessEffect_Force == GameManager.Instance.Status.MadnessEffect_Force - 1 ?
      WNCText.GetMadnessColor((GameManager.Instance.MyGameData.TotalRestCount % GameManager.Instance.Status.MadnessEffect_Force + 1).ToString() + "/" + GameManager.Instance.Status.MadnessEffect_Force.ToString()) :
      ((GameManager.Instance.MyGameData.TotalRestCount % GameManager.Instance.Status.MadnessEffect_Force + 1).ToString() + "/" + GameManager.Instance.Status.MadnessEffect_Force.ToString());
    if (madcoroutine_force == null)
    {
      madcoroutine_force = madcounttext(ForceMadCountText.rectTransform);
      StartCoroutine(madcoroutine_force);
    }
    else
    {
      StopCoroutine(madcoroutine_force);
      madcoroutine_force = madcounttext(ForceMadCountText.rectTransform);
      StartCoroutine(madcoroutine_force);
    }
  }
  public void SetWildMadCount()
  {
    WildMadCountText.text =
      GameManager.Instance.MyGameData.TotalMoveCount % GameManager.Instance.Status.MadnessEffect_Wild_temporary == GameManager.Instance.Status.MadnessEffect_Wild_temporary - 1 ?
      WNCText.GetMadnessColor((GameManager.Instance.MyGameData.TotalMoveCount % GameManager.Instance.Status.MadnessEffect_Wild_temporary + 1).ToString() + "/" + GameManager.Instance.Status.MadnessEffect_Wild_temporary.ToString()) :
      ((GameManager.Instance.MyGameData.TotalMoveCount % GameManager.Instance.Status.MadnessEffect_Wild_temporary + 1).ToString() + "/" + GameManager.Instance.Status.MadnessEffect_Wild_temporary.ToString());
    if (madcoroutine_wild == null)
    {
      madcoroutine_wild = madcounttext(WildMadCountText.rectTransform);
      StartCoroutine(madcoroutine_wild);
    }
    else
    {
      StopCoroutine(madcoroutine_wild);
      madcoroutine_wild = madcounttext(WildMadCountText.rectTransform);
      StartCoroutine(madcoroutine_wild);
    }
  }
  private IEnumerator madcoroutine_force = null;
  private IEnumerator madcoroutine_wild = null;
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
      rect.localScale = Vector3.one * (1.0f - MadCountAnimationCurve.Evaluate(_time / _targettime));
      _time += Time.deltaTime; yield return null;
    }
    rect.localScale = Vector3.zero;
  }
  private int conversationlevel = -1;
  private int forcelevel = -1;
  private int wildlevel = -1;
  private int intelligencelevel = -1;
  [SerializeField] private Color MadnessColor = new Color();
  [SerializeField] private Color IdleColor = Color.white;
  [SerializeField] private float SkillGainTime = 0.5f;
  [Space(10)]
  [SerializeField] private TextMeshProUGUI ProgressText = null;
  [SerializeField] private List<Image> ProgressIcons = null;
  private float ProgressDisableAlpha = 0.6f;
  [SerializeField] private CanvasGroup LevelUpEffect = null;
  private IEnumerator levelupcoroutine = null;
  [SerializeField] private float LevelUpBlinkTime = 0.6f;
  private bool IsLevelup = false;
  [SerializeField] private float ProgressExpandSize = 1.7f;
  [SerializeField] private float ProgressExpandTime = 1.2f;
  [SerializeField] private float ProgressExpandWaitTime = 0.05f;
  [SerializeField] private float ProgressExpandTerm = 0.3f;

  private void Start()
  {
    ProgressText.text = GameManager.Instance.GetTextData("ForSkillProgress");
  }
  public void SetProgres()
  {
    int _count = GameManager.Instance.MyGameData.SkillProgressRequire;
    for(int i = 0; i < ProgressIcons.Count; i++)
    {
      if (i < _count)
      {
        if(i < GameManager.Instance.MyGameData.SkillProgress)
        { //활성화
          if (ProgressIcons[i].GetComponent<CanvasGroup>().alpha == ProgressDisableAlpha)
          {
            ProgressIcons[i].GetComponent<CanvasGroup>().alpha = 1.0f;
            ProgressIcons[i].sprite = GameManager.Instance.ImageHolder.SkillProgress_Full;

            if(i!=GameManager.Instance.MyGameData.SkillProgressRequire-1) StartCoroutine(UIManager.Instance.ExpandRect(ProgressIcons[i].rectTransform, ProgressExpandSize, ProgressExpandTime));
          }
        }
        else
        {//비활성화
          if (ProgressIcons[i].GetComponent<CanvasGroup>().alpha == 1.0f)
          {
            ProgressIcons[i].GetComponent<CanvasGroup>().alpha = ProgressDisableAlpha;
            ProgressIcons[i].sprite = GameManager.Instance.ImageHolder.SkillProgress_Empty;
          }
        }
   
        if (!ProgressIcons[i].gameObject.activeSelf) ProgressIcons[i].gameObject.SetActive(true);
      }
      else
      {
        if (ProgressIcons[i].gameObject.activeSelf) ProgressIcons[i].gameObject.SetActive(false);
      }

    }
    LayoutRebuilder.ForceRebuildLayoutImmediate(ProgressIcons[0].transform.parent.transform as RectTransform);
    LayoutRebuilder.ForceRebuildLayoutImmediate(ProgressIcons[0].transform.parent.transform.parent.transform as RectTransform);
    if (GameManager.Instance.MyGameData.SkillProgress >= GameManager.Instance.MyGameData.SkillProgressRequire)
    {
      IsLevelup = true;
      if (levelupcoroutine == null)
      {
        levelupcoroutine = Blink();
        StartCoroutine(levelupcoroutine);
        StartCoroutine(EnableLevelup());
      }
      else
      {

      }

      if (ConvLevelUp.alpha==0.0f)
      {
        StartCoroutine(UIManager.Instance.ChangeAlpha(ConvLevelUp,1.0f,0.1f));
        StartCoroutine(UIManager.Instance.ChangeAlpha(ForceLevelUp, 1.0f, 0.1f));
        StartCoroutine(UIManager.Instance.ChangeAlpha(WildLevelUp, 1.0f, 0.1f));
        StartCoroutine(UIManager.Instance.ChangeAlpha(IntelLevelUp, 1.0f, 0.1f));
      }
    }
  }
  private IEnumerator Blink()
  {
    float _time = 0.0f;
    LevelUpEffect.alpha = 1.0f;

    while (IsLevelup)
    {
      while (_time < LevelUpBlinkTime)
      {
        LevelUpEffect.alpha = Mathf.Lerp(1.0f, 0.0f, _time / LevelUpBlinkTime);
        _time += Time.deltaTime;
        yield return null;
      }
      _time = 0.0f;
      yield return null;
    }
    LevelUpEffect.alpha = 0.0f;
  }
  private IEnumerator EnableLevelup()
  {
    foreach(var _icon in ProgressIcons)
    {
      if (_icon.gameObject.activeSelf)
      {
        StartCoroutine(expandprogressicon(_icon.rectTransform));
        yield return new WaitForSeconds(ProgressExpandTerm);
      }
      yield return null;
    }

  }
  private IEnumerator expandprogressicon(RectTransform rect)
  {
    WaitForSeconds _wait = new WaitForSeconds(ProgressExpandWaitTime);
    while (IsLevelup)
    {
      yield return StartCoroutine(UIManager.Instance.ExpandRect(rect, ProgressExpandSize, ProgressExpandTime));
      yield return _wait;
    }
    yield return null;
  }
  public void LevelUp(int index)
  {
    if (!IsLevelup) return;
    IsLevelup = false;
    switch (index)
    {
      case 0:
        GameManager.Instance.MyGameData.Skill_Conversation.LevelByDefault++;
        StartCoroutine(UIManager.Instance.ExpandRect(ConversationIconRect, LevelUpSize,LevelUpTime));
        UIManager.Instance.SidePanelCultUI.UpdateProgressText();
        break;
      case 1: 
        GameManager.Instance.MyGameData.Skill_Force.LevelByDefault++;
        StartCoroutine(UIManager.Instance.ExpandRect(ForceIconRect, LevelUpSize, LevelUpTime));
        break;
      case 2: 
        GameManager.Instance.MyGameData.Skill_Wild.LevelByDefault++;
        StartCoroutine(UIManager.Instance.ExpandRect(WildIconRect, LevelUpSize, LevelUpTime));
        break;
      case 3: 
        GameManager.Instance.MyGameData.Skill_Intelligence.LevelByDefault++;
        StartCoroutine(UIManager.Instance.ExpandRect(IntelligenceIconRect, LevelUpSize, LevelUpTime));
        break;
    }

    StartCoroutine(UIManager.Instance.ChangeAlpha(ConvLevelUp, 0.0f, 0.1f));
    StartCoroutine(UIManager.Instance.ChangeAlpha(ForceLevelUp, 0.0f, 0.1f));
    StartCoroutine(UIManager.Instance.ChangeAlpha(WildLevelUp, 0.0f, 0.1f));
    StartCoroutine(UIManager.Instance.ChangeAlpha(IntelLevelUp, 0.0f, 0.1f));

    int _sum = 0;
    int _minusvalue = -1;
    for (int i = 1; i < GameManager.Instance.Status.SkillProgress_Max; i++)
    {
      _sum += i;
      if (GameManager.Instance.MyGameData.SkillLevelupCount < _sum)
      {
        _minusvalue = i;
        break;
      }
    }
    if (_minusvalue == -1) _minusvalue = GameManager.Instance.Status.SkillProgress_Max;

    GameManager.Instance.MyGameData.SkillLevelupCount++;
    GameManager.Instance.MyGameData.SkillProgress -= _minusvalue;
    levelupcoroutine = null;
  }
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
        StartCoroutine(UIManager.Instance.ChangeAlpha(ConversationEffectGroup, 0.0f, SkillGainTime));
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
        StartCoroutine(UIManager.Instance.ChangeAlpha(ForceEffectGroup, 0.0f, SkillGainTime));
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
        StartCoroutine(UIManager.Instance.ChangeAlpha(WildEffectGroup, 0.0f, SkillGainTime));
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
        StartCoroutine(UIManager.Instance.ChangeAlpha(IntelligenceEffectGroup, 0.0f, SkillGainTime));
        intelligencelevel = GameManager.Instance.MyGameData.Skill_Intelligence.Level;
      }
    }
  }

}
