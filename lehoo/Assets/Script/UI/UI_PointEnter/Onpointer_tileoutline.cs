using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Onpointer_tileoutline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  public UI_map MyMapUI = null;
  public TileData MyTile = null;
  public void OnPointerEnter(PointerEventData eventData)
  {
    MyMapUI.SetOutline_Idle(MyTile);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    MyMapUI.DisableOutline_Idle();
  }
}
