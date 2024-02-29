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
    if (!MyTile.Interactable||MyTile.Fogstate!=2) return;
    MyMapUI.PointerEnterTile(MyTile);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if (!MyTile.Interactable || MyTile.Fogstate != 2) return;
    MyMapUI.ExitTile();
  }
}
