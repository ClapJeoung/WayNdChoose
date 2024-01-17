using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum HighlightEffectEnum { HP, Sanity, Gold, Supply,Skill, Madness, Exp,Rational,Physical,Mental,Material}

public class HighlightEffects : MonoBehaviour
{
  public float EffectTime = 2.5f;
  [SerializeField] private List<HighlightHolder> HighlightList = new List<HighlightHolder>();
  private HighlightHolder GetHighlight(HighlightEffectEnum effect)
  {
    switch (effect)
    {
      case HighlightEffectEnum.HP:
        return HighlightList[0];
      case HighlightEffectEnum.Sanity:
        return HighlightList[1];
      case HighlightEffectEnum.Gold:
        return HighlightList[2];
      case HighlightEffectEnum.Supply:
        return HighlightList[3];
      case HighlightEffectEnum.Skill:
        return HighlightList[4];
      case HighlightEffectEnum.Madness:
        return HighlightList[5];
      case HighlightEffectEnum.Exp:
        return HighlightList[6];
      case HighlightEffectEnum.Rational:
        return HighlightList[7];
      case HighlightEffectEnum.Physical:
        return HighlightList[8];
      case HighlightEffectEnum.Mental:
        return HighlightList[9];
      case HighlightEffectEnum.Material:
        return HighlightList[10];
    }
    return null;
  }
  public void SetHighlights(List<HighlightCallInfo> callinfo)
  {
    foreach (HighlightCallInfo info in callinfo)
    {
      switch (info.CallType)
      {
        case HighlightEffectEnum.HP:
        case HighlightEffectEnum.Sanity:
        case HighlightEffectEnum.Gold:
        case HighlightEffectEnum.Supply:
          GetHighlight(info.CallType).IconRect.localScale = Vector3.one * ConstValues.StatusHighlightSize;
          break;
        case HighlightEffectEnum.Skill:
          GetHighlight(info.CallType).SetEffect_Skill(info.Skilltype);
          break;
        default:
          GetHighlight(info.CallType).SetEffect();
          break;
      }
    }
  }
  public void TurnOff()
  {
    foreach (var effect in HighlightList)
    {
      effect.Reset();
    }
  }
  public void HighlightAnimation(HighlightEffectEnum type)
  {
    switch (type)
    {
      case HighlightEffectEnum.HP:
      case HighlightEffectEnum.Sanity:
      case HighlightEffectEnum.Gold:
      case HighlightEffectEnum.Supply:
      case HighlightEffectEnum.Skill:
        GetHighlight(type).IconRect.localScale = Vector3.one * ConstValues.StatusHighlightSize;
        break;
      default:
        StartCoroutine(UIManager.Instance.ChangeAlpha(GetHighlight(type).Group, 0.0f, EffectTime));
        break;
    }
  }
  public void Highlight_Skill(SkillTypeEnum skill)
  {
    GetHighlight(HighlightEffectEnum.Skill).GetIcon_Skill(skill).localScale=Vector3.one * ConstValues.StatusHighlightSize;
  }
  public void Highlight_Skill(List<SkillTypeEnum> skills)
  {
    foreach (var _icon in GetHighlight(HighlightEffectEnum.Skill).GetIcon_Skill(skills))
      _icon.localScale = Vector3.one * ConstValues.StatusHighlightSize;
  }
  /// <summary>
  /// 광기 하이라이트
  /// </summary>
  /// <param name="type"></param>
  /// <param name="skilltype"></param>
  public void Highlight_Madness(SkillTypeEnum skilltype)
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(GetHighlight(HighlightEffectEnum.Skill).Group, 0.0f, EffectTime));
    CanvasGroup _group = GetHighlight(HighlightEffectEnum.Skill).GetSkillGroup_Madness(skilltype);
    if(_group!=null) StartCoroutine(UIManager.Instance.ChangeAlpha(_group, 0.0f, EffectTime));
  }

}
[System.Serializable]
public class HighlightHolder
{
  public HighlightEffectEnum Type;
  public RectTransform IconRect = null;
  public CanvasGroup Group = null;
  public TextMeshProUGUI Text = null;
  public RectTransform ConvIcon = null;
  public RectTransform ForceIcon = null;
  public RectTransform WildIcon = null;
  public RectTransform IntelIcon = null;
  public CanvasGroup ConversationEffect = null;
  public CanvasGroup ForceEffect = null;
  public CanvasGroup WildEffect = null;
  public CanvasGroup IntelligenceEffect = null;
  public void SetEffect()
  {
    Group.alpha = 1.0f;
  }
  public void SetEffect_Skill(List<SkillTypeEnum> skill)
  {
    foreach(var type in skill)
    {
      switch (type)
      {
        case SkillTypeEnum.Conversation:
          ConvIcon.localScale = Vector3.one * ConstValues.StatusHighlightSize;
          break;
        case SkillTypeEnum.Force:
          ForceIcon.localScale = Vector3.one * ConstValues.StatusHighlightSize;
          break;
        case SkillTypeEnum.Wild:
          WildIcon.localScale = Vector3.one * ConstValues.StatusHighlightSize;
          break;
        case SkillTypeEnum.Intelligence:
          IntelIcon.localScale = Vector3.one * ConstValues.StatusHighlightSize;
          break;
      }
    }
  }
  public RectTransform GetIcon_Skill(SkillTypeEnum skill)
  {
    switch (skill)
    {
      case SkillTypeEnum.Conversation:
        return ConvIcon;
      case SkillTypeEnum.Force:
        return ForceIcon;
      case SkillTypeEnum.Wild:
        return WildIcon;
      case SkillTypeEnum.Intelligence:
        return IntelIcon;
      default:
        return null;
    }
  }
  public List<RectTransform> GetIcon_Skill(List<SkillTypeEnum> skills)
  {
    List<RectTransform> _icons= new List<RectTransform>();
    foreach (SkillTypeEnum skill in skills)
    {
      switch (skill)
      {
        case SkillTypeEnum.Conversation:
          _icons.Add(ConvIcon);
          break;
        case SkillTypeEnum.Force:
          _icons.Add(ForceIcon);
          break;
        case SkillTypeEnum.Wild:
          _icons.Add(WildIcon);
          break;
        case SkillTypeEnum.Intelligence:
          _icons.Add(IntelIcon);
          break;
      }
    }
    return _icons;
  }
  public CanvasGroup GetSkillGroup_Madness(SkillTypeEnum skilltype)
  {
    CanvasGroup _group = null;
    switch (skilltype)
    {
      case SkillTypeEnum.Conversation:
        _group = ConversationEffect;
        break;
      case SkillTypeEnum.Force:
        _group = ForceEffect;
        break;
      case SkillTypeEnum.Wild:
        _group = WildEffect;
        break;
      case SkillTypeEnum.Intelligence:
        _group = IntelligenceEffect;
        break;
    }
    return _group;
  }
  public void Reset()
  {
    if(Group.alpha!=0.0f) Group.alpha = 0.0f;
    if (IconRect != null) IconRect.localScale = Vector3.one;

    if (Type == HighlightEffectEnum.Skill)
    {
      ConvIcon.localScale = Vector3.one;
      ForceIcon.localScale = Vector3.one;
      WildIcon.localScale = Vector3.one;
      IntelIcon.localScale = Vector3.one;
      ConversationEffect.alpha = 0.0f;
      ForceEffect.alpha = 0.0f;
      WildEffect.alpha = 0.0f;
      IntelligenceEffect.alpha = 0.0f;
    }
  }
}
