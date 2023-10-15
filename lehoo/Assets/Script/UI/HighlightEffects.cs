using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HighlightEffectEnum { HP, Sanity, Gold, Movepoint,Skill, Madness, Exp}

public class HighlightEffects : MonoBehaviour
{
  public float EffectTime = 2.5f;
  [SerializeField] private List<CanvasGroup> HighlightList = new List<CanvasGroup>();
  private CanvasGroup GetEffect(HighlightEffectEnum effect)
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
    }
    return null;
  }
  public void SetHighlights(List<HighlightEffectEnum> effects)
  {
    foreach (HighlightEffectEnum effect in effects)
    {
      GetEffect(effect).alpha = 1.0f;
    }
  }
  public void TurnOff()
  {
    foreach(var  effect in HighlightList)if(effect.alpha==1.0f)effect.alpha = 0.0f;
  }
  public void HighlightAnimation(HighlightEffectEnum type)
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(GetEffect(type), 0.0f, EffectTime));
  }
}
