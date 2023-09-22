using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Onpointer_restbuttons : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
  public UI_Settlement SettlementUI = null;
  public StatusType MyStatusType = StatusType.HP;

  public void OnPointerEnter(PointerEventData data)
  {
    SettlementUI.OnPointerRestType(MyStatusType);
  }
  public void OnPointerExit(PointerEventData data)
  {
    SettlementUI.OnExitRestType(MyStatusType);
  }
}
