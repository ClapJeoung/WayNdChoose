using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SidePanel_Quest_Cult : MonoBehaviour
{
  [SerializeField] private Slider ProgressSlider = null;
  public CanvasGroup SliderGroup = null;
  public CanvasGroup DefaultGroup = null;
  [SerializeField] private TextMeshProUGUI TurnText = null;
  [SerializeField] private float TurnTitleMoveTime = 1.5f;
  [SerializeField] private AnimationCurve TurnTileAnimationCurve = new AnimationCurve();
  [SerializeField] private RectTransform TurnTitleRect = null;
  [SerializeField] private TextMeshProUGUI ValueText = null;
  public Vector2 IconHidePos = new Vector2(-250.0f, 0.0f);
  public Vector2 IconOpenPos = new Vector2(-90.0f, 0.0f);
  public float OpenTime = 1.0f;
  public float CloseTime = 0.8f;
  public float WaitTime = 0.3f;
  [Space(20)]
  public CanvasGroup Village_Group = null;
  public CanvasGroup VillageIconEffect = null;
  [Space(5)]
  public CanvasGroup Town_Group = null;
  public CanvasGroup TownIconEffect = null;
  [Space(5)]
  public CanvasGroup City_Group = null;
  public CanvasGroup CityIconEffect = null;
  [Space(5)]
  public CanvasGroup Sabbat_Group = null;
  [SerializeField] private Image Sabbat_SectorIcon = null;
  [SerializeField] private Outline Sabbat_Effect = null;
  public void SetSabbatFail()
  {
    string _failtext = string.Format(GameManager.Instance.GetTextData("Cult_Sabbat_Fail"), WNCText.GetDiscomfortColor(GameManager.Instance.Status.Quest_Cult_Sabbat_PenaltyDiscomfort));
    UIManager.Instance.SetInfoPanel(_failtext);
  }
  public void SetRitualFail()
  {
    string _failtext = string.Format(GameManager.Instance.GetTextData("Cult_Sabbat_Fail"), WNCText.GetSupplyColor(GameManager.Instance.Status.Quest_Cult_Ritual_PenaltySupply));
    UIManager.Instance.SetInfoPanel(_failtext);
  }
  [Space(5)]
  public CanvasGroup Ritual_Group = null;
  public Image Ritual_Bottom = null;
  [SerializeField] private Image Ritual_Top = null;
  [SerializeField] private Outline Ritual_Effect = null;
  private IEnumerator OpenGroup(CanvasGroup group,float waittime)
  {
    if(waittime>0)yield return new WaitForSeconds(waittime);
    StartCoroutine(UIManager.Instance.moverect(group.transform as RectTransform, IconHidePos, IconOpenPos, OpenTime, UIManager.Instance.UIPanelOpenCurve));
  }
  private IEnumerator CloseGroup(CanvasGroup group, float waittime)
  {
    if (waittime > 0) yield return new WaitForSeconds(waittime);
    StartCoroutine(UIManager.Instance.moverect(group.transform as RectTransform, IconOpenPos, IconHidePos, CloseTime, UIManager.Instance.UIPanelCLoseCurve));
  }
  private int LastPhase = -1;
  private float LastProgress = -1;
  private int LastTurn = -1;
  public void UpdateUI()
  {
    int _progressvalue = 0;
    switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
    {
      case 0:
        if (LastPhase != 0)
        {
          StartCoroutine(OpenGroup(Village_Group, 0.0f));
        }
        if (GameManager.Instance.MyGameData.Tendency_Body.Level != 0 && DefaultGroup.alpha == 0.0f)
        {
          StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 0.5f));
          StartCoroutine(UIManager.Instance.ChangeAlpha(SliderGroup, 1.0f, 0.5f));
        }
        _progressvalue = GameManager.Instance.Status.Quest_Cult_Progress_Village+ GameManager.Instance.MyGameData.Skill_Conversation.Level/GameManager.Instance.Status.ConversationEffect_Level*GameManager.Instance.Status.ConversationEffect_Value;
        break;
      case 1:
        if (DefaultGroup.alpha == 0.0f)
        {
          StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 0.5f));
          StartCoroutine(UIManager.Instance.ChangeAlpha(SliderGroup, 1.0f, 0.5f));
        }

          _progressvalue = GameManager.Instance.Status.Quest_Cult_Progress_Town + GameManager.Instance.MyGameData.Skill_Conversation.Level/GameManager.Instance.Status.ConversationEffect_Level*GameManager.Instance.Status.ConversationEffect_Value;
        if (LastPhase != 1)
        {
          if (LastPhase != -1) StartCoroutine(CloseGroup(Village_Group, 0.0f));
          StartCoroutine(OpenGroup(Town_Group, LastPhase != -1 ? CloseTime + WaitTime : 0.0f)); ;
        }
        break;
      case 2:
        if (DefaultGroup.alpha == 0.0f)
        {
          StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 0.5f));
          StartCoroutine(UIManager.Instance.ChangeAlpha(SliderGroup, 1.0f, 0.5f));
        }

          _progressvalue = GameManager.Instance.Status.Quest_Cult_Progress_City + GameManager.Instance.MyGameData.Skill_Conversation.Level/GameManager.Instance.Status.ConversationEffect_Level*GameManager.Instance.Status.ConversationEffect_Value;

        if (LastPhase != 2)
        {
          if (LastPhase != -1) StartCoroutine(CloseGroup(Town_Group, 0.0f));
          StartCoroutine(OpenGroup(City_Group, LastPhase != -1 ? CloseTime + WaitTime : 0.0f));
        }
        break;
      case 3:
        if (DefaultGroup.alpha == 0.0f)
        {
          StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 0.5f));
          StartCoroutine(UIManager.Instance.ChangeAlpha(SliderGroup, 1.0f, 0.5f));
        }

          _progressvalue = GameManager.Instance.Status.Quest_Cult_Progress_Sabbat + GameManager.Instance.MyGameData.Skill_Conversation.Level/GameManager.Instance.Status.ConversationEffect_Level*GameManager.Instance.Status.ConversationEffect_Value;
        if (LastPhase != 3)
        {
          Sabbat_SectorIcon.sprite = GameManager.Instance.ImageHolder.GetSectorIcon(GameManager.Instance.MyGameData.Cult_SabbatSector,true);

          if (LastPhase == 2) StartCoroutine(CloseGroup(City_Group, 0.0f));
          else if (LastPhase == 4) StartCoroutine(CloseGroup(Ritual_Group, 0.0f));

          StartCoroutine(OpenGroup(Sabbat_Group, LastPhase != -1 ? CloseTime + WaitTime : 0.0f));
        }
        break;
      case 4:
        if (DefaultGroup.alpha == 0.0f)
        {
          StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 0.5f));
          StartCoroutine(UIManager.Instance.ChangeAlpha(SliderGroup, 1.0f, 0.5f));
        }

          _progressvalue = GameManager.Instance.Status.Quest_Cult_Progress_Ritual + GameManager.Instance.MyGameData.Skill_Conversation.Level/GameManager.Instance.Status.ConversationEffect_Level*GameManager.Instance.Status.ConversationEffect_Value;
        if (LastPhase != 4)
        {
          if (LastPhase != -1) StartCoroutine(CloseGroup(Sabbat_Group, 0.0f));

          Ritual_Bottom.sprite = GameManager.Instance.MyGameData.Cult_RitualTile.ButtonScript.BottomImage.sprite;
          Ritual_Bottom.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -60.0f * GameManager.Instance.MyGameData.Cult_RitualTile.Rotation));
          Ritual_Top.sprite = GameManager.Instance.MyGameData.Cult_RitualTile.ButtonScript.TopImage.sprite;

          StartCoroutine(OpenGroup(Ritual_Group, LastPhase != -1 ? CloseTime + WaitTime : 0.0f));
        }
        break;
    }

    LastPhase = GameManager.Instance.MyGameData.Quest_Cult_Phase;
    ValueText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_ProgressValue"), _progressvalue);
    TurnText.text = GameManager.Instance.MyGameData.Cult_CoolTime.ToString();
    if (LastTurn != -1 && LastTurn != GameManager.Instance.MyGameData.Cult_CoolTime)
    {
      StartCoroutine(spinturntitle());
    }
    LastTurn = GameManager.Instance.MyGameData.Cult_CoolTime;
  }
  private IEnumerator spinturntitle()
  {
    float _time = 0.0f;
    float _rotatevalue = (LastTurn - GameManager.Instance.MyGameData.Cult_CoolTime) * 360.0f;
    float _endtime = _rotatevalue == 360.0f ? TurnTitleMoveTime : Mathf.Abs(1.0f * (LastTurn - GameManager.Instance.MyGameData.Cult_CoolTime));
    while (_time < _endtime)
    {
      TurnTitleRect.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Mathf.Lerp(0.0f, _rotatevalue, TurnTileAnimationCurve.Evaluate(_time / TurnTitleMoveTime))));
      _time += Time.deltaTime; yield return null;
    }
    TurnTitleRect.rotation = Quaternion.Euler(Vector3.zero);
    yield return null;
  }
  public void UpdateProgressValue()
  {
    if (LastProgress != GameManager.Instance.MyGameData.Quest_Cult_Progress)
    {
      StartCoroutine(changeslidervalue());
      LastProgress = GameManager.Instance.MyGameData.Quest_Cult_Progress;
    }
  }
  public RectTransform HandleRect = null;
  public float HandleMaxScale = 1.4f;
  public AnimationCurve HandleScaleCurve = new AnimationCurve();
  private IEnumerator changeslidervalue()
  {
    float _current = ProgressSlider.value;
    float _target = GameManager.Instance.MyGameData.Quest_Cult_Progress;
    float _time = 0.0f, _targettime = 1.2f;
    Vector2 _originsize = Vector2.one, _maxsize = Vector2.one * HandleMaxScale;
    while( _time < _targettime )
    {
      ProgressSlider.value = Mathf.Lerp(_current, _target, _time / _targettime);
      HandleRect.localScale=Vector2.Lerp(_originsize,_maxsize,HandleScaleCurve.Evaluate(_time/_targettime));
      _time+=Time.deltaTime;
      yield return null;
    }
  }

  public void SetSabbatEffect(bool enable)
  {
    if(Sabbat_Effect.enabled!=enable) Sabbat_Effect.enabled= enable;
  }
  public void SetRitualEffect(bool enable)
  {
    if(Ritual_Effect.enabled!=enable) Ritual_Effect.enabled = enable;
  }
  public void SetSettlementEffect(SettlementType type,bool enable)
  {
    switch (type)
    {
      case SettlementType.Village:
        VillageIconEffect.alpha = enable?1.0f:0.0f;
        break;
      case SettlementType.Town:
        TownIconEffect.alpha = enable ? 1.0f : 0.0f;
        break;
      case SettlementType.City:
        CityIconEffect.alpha = enable ? 1.0f : 0.0f;
        break;
    }
  }
}
