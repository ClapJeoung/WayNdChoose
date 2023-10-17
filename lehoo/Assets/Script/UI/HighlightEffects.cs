using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
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
        GetHighlight(info.CallType).SetEffect(info.Skilltype);
      }
      else
      {
        GetHighlight(info.CallType).SetEffect(info.Value);
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
}
[System.Serializable]
public class HighlightHolder
{
  public HighlightEffectEnum Type;
  public CanvasGroup Group = null;
  public TextMeshProUGUI Text = null;
  private int OriginValue = 100;
  public Image Conversation_Frame = null;
  public Image Conversation_Icon = null;
  public Image Force_Frame = null;
  public Image Force_Icon = null;
  public Image Wild_Frame = null;
  public Image Wild_Icon = null;
  public Image Intelligence_Frame = null;
  public Image Intelligence_Icon = null;
  public Sprite Frame_black = null;
  public Sprite Frame_white = null;
  public void SetEffect(int value)
  {
    Group.alpha = 1.0f;

    if (OriginValue == 100 || value == 0) return;
    OriginValue = int.Parse(Text.text);
    string _valuetext =OriginValue+ value > 0 ? $"+{value}" : $"{value}";
    Text.text = _valuetext;
  }
  public void SetEffect(List<SkillTypeEnum> skill)
  {
    Sprite _spr = null;
    foreach(var type in skill)
    {
      _spr = GameManager.Instance.ImageHolder.GetSkillIcon(type, false);
      switch (type)
      {
        case SkillTypeEnum.Conversation:
          Conversation_Frame.sprite = Frame_white;
          Conversation_Icon.sprite = _spr;
          break;
        case SkillTypeEnum.Force:
          Force_Frame.sprite = Frame_white;
          Force_Icon.sprite = _spr;
          break;
        case SkillTypeEnum.Wild:
          Wild_Frame.sprite = Frame_white;
          Wild_Icon.sprite = _spr;
          break;
        case SkillTypeEnum.Intelligence:
          Intelligence_Frame.sprite = Frame_white;
          Intelligence_Icon.sprite = _spr;
          break;
      }
    }
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
            Conversation_Frame.sprite = Frame_black;
            Conversation_Icon.sprite = _spr;
            break;
          case SkillTypeEnum.Force:
            Force_Frame.sprite = Frame_black;
            Force_Icon.sprite = _spr;
            break;
          case SkillTypeEnum.Wild:
            Wild_Frame.sprite = Frame_black;
            Wild_Icon.sprite = _spr;
            break;
          case SkillTypeEnum.Intelligence:
            Intelligence_Frame.sprite = Frame_black;
            Intelligence_Icon.sprite = _spr;
            break;
        }
      }
    }

    if (OriginValue == 100) return;
    Text.text=OriginValue.ToString();
  }
}
