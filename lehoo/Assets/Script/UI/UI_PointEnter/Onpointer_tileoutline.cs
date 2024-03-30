using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Onpointer_tileoutline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  [SerializeField] private UI_map MyMapUI = null;
  [SerializeField] private TileData MyTile = null;
  public void Setup(UI_map map,TileData tile) { MyMapUI = map; MyTile = tile; }
  public void OnPointerEnter(PointerEventData eventData)
  {
    if (!MyTile.Interactable) return;
    MyMapUI.PointerEnterTile(MyTile);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if (!MyTile.Interactable ) return;
    MyMapUI.ExitTile();
  }
}
