using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Onpointer_study : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  [SerializeField] private UI_RewardExp RewardUI = null;
  [SerializeField] private CanvasGroup DefaultGroup = null;
  public void OnPointerEnter(PointerEventData eventData)
  {
    if (!DefaultGroup.interactable) return;
    RewardUI.OnpointerStudy();
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if (!DefaultGroup.interactable) return;
    RewardUI.ExitPointerStudy();
  }

}
