using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Onpointer_highlight : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
  private bool interactive = true;
  public bool Interactive
  {
    get { return interactive; }
    set
    {
      interactive = value;
      if (value == false) UIManager.Instance.HighlightManager.TurnOff();

    }
  }
  public List<HighlightEffectEnum> HighlightList=new List<HighlightEffectEnum> ();
  public void AddHighlight(HighlightEffectEnum highlightEffect)
  {
    if(!HighlightList.Contains(highlightEffect))HighlightList.Add(highlightEffect);
  }
  public void RemoveHighlight(HighlightEffectEnum highlightEffect)
  {
    if(HighlightList.Contains(highlightEffect))HighlightList.Remove(highlightEffect);
  }
  public void OnPointerEnter(PointerEventData eventData)
  {
    if (!Interactive) return;
    if (HighlightList.Count == 0) return;

    UIManager.Instance.HighlightManager.SetHighlights(HighlightList);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if (!Interactive) return;
    if (HighlightList.Count == 0) return;

    UIManager.Instance.HighlightManager.TurnOff();
  }

}
