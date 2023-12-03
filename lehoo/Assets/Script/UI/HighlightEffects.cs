using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum HighlightEffectEnum { HP, Sanity, Gold, Movepoint,Skill, Madness, Exp,Rational,Physical,Mental,Material}

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
      case HighlightEffectEnum.Movepoint:
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
      if (info.CallType == HighlightEffectEnum.Skill)
      {
        GetHighlight(info.CallType).SetEffect_Skill(info.Skilltype);
      }
      else
      {
        GetHighlight(info.CallType).SetEffect();
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
    StartCoroutine(UIManager.Instance.ChangeAlpha(GetHighlight(type).Group, 0.0f, EffectTime));
  }
  /// <summary>
  /// 광기 하이라이트
  /// </summary>
  /// <param name="type"></param>
  /// <param name="skilltype"></param>
  public void HighlightAnimation(HighlightEffectEnum type,SkillTypeEnum skilltype)
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(GetHighlight(type).Group, 0.0f, EffectTime));
    CanvasGroup _group = GetHighlight(type).GetSkillGroup_Madness(skilltype);
    if(_group!=null) StartCoroutine(UIManager.Instance.ChangeAlpha(_group, 0.0f, EffectTime));
  }

}
[System.Serializable]
public class HighlightHolder
{
  public HighlightEffectEnum Type;
  public CanvasGroup Group = null;
  public TextMeshProUGUI Text = null;
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
    Sprite _spr = null;
    foreach(var type in skill)
    {
      _spr = GameManager.Instance.ImageHolder.GetSkillIcon(type, false);
      switch (type)
      {
        case SkillTypeEnum.Conversation:
          ConversationEffect.alpha = 1.0f;
          break;
        case SkillTypeEnum.Force:
          ForceEffect.alpha = 1.0f;
          break;
        case SkillTypeEnum.Wild:
          WildEffect.alpha = 1.0f;
          break;
        case SkillTypeEnum.Intelligence:
          IntelligenceEffect.alpha = 1.0f;
          break;
      }
    }
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

    if (Type == HighlightEffectEnum.Skill)
    {
      Sprite _spr = null;
      SkillTypeEnum _skilltype = SkillTypeEnum.Conversation;
      for(int i=0;i<4;i++)
      {
        _skilltype = (SkillTypeEnum)i;
        _spr = GameManager.Instance.ImageHolder.GetSkillIcon(_skilltype, false);
        switch (_skilltype)
        {
          case SkillTypeEnum.Conversation:
            ConversationEffect.alpha = 0.0f;
            break;
          case SkillTypeEnum.Force:
            ForceEffect.alpha = 0.0f;
            break;
          case SkillTypeEnum.Wild:
            WildEffect.alpha = 0.0f;
            break;
          case SkillTypeEnum.Intelligence:
            IntelligenceEffect.alpha = 0.0f;
            break;
        }
      }
    }
  }
}
