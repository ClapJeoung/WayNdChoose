using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileButtonScript : MonoBehaviour
{
  public Outline OutLine = null;
  public int OriginIndex = -1;
  public Button Button = null;
  public Transform SelectHolder = null;
  public Transform OriginHolder = null;
  public UI_map MapUI = null;
  public TileData TileData = null;
  public Image MyImage = null;
  public Image TopEnvirImage = null;
  public void Clicked()
  {
    if (TileData.Coordinate == GameManager.Instance.MyGameData.Coordinate) return;

    if (transform.parent == OriginHolder)
    {
      MapUI.SelectTile(TileData,TileData.Rect.position);
      SelectTile();
    }
    else
    {
      MapUI.MoveMap();
    }
  }

  public void SelectTile()
  {
    if (OriginIndex == -1) OriginIndex = transform.GetSiblingIndex();
    transform.SetParent(SelectHolder);
    OutLine.enabled = true;
  }
  public void CancleTile()
  {
    transform.SetParent(OriginHolder);
    transform.SetSiblingIndex(OriginIndex);
    OutLine.enabled = false;
  }
}
