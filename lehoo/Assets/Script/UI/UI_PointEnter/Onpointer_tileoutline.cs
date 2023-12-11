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
    if (!MyTile.Interactable) return;
    if (!MyTile.ButtonScript.Button.interactable) return;
    MyMapUI.PointerEnterTile(MyTile);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if (!MyTile.Interactable) return;
    if (!MyTile.ButtonScript.Button.interactable) return;
    MyMapUI.ExitTile();
  }
}
