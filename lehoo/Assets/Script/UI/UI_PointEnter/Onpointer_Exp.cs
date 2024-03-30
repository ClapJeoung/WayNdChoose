using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Onpointer_Exp : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
  [SerializeField] private UI_RewardExp UpperUI = null;
  [SerializeField] private int Index = 0;

  public void OnPointerEnter(PointerEventData eventData)
  {
    UpperUI.OnpointerExp(Index);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    UpperUI.ExitPointerExp(Index);
  }
}
