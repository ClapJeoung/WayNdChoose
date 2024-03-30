using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Onpointer_sector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  [SerializeField] private SectorTypeEnum MySector = SectorTypeEnum.NULL;
  [SerializeField] private UI_dialogue SettlementUI = null;
  public void OnPointerEnter(PointerEventData eventData)
  {
    SettlementUI.OnPointerSector(MySector);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    SettlementUI.OutPointerSector();
  }
}
