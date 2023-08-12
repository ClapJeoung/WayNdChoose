using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BottomEnvirType
{
 NULL, Land,River,Sea,Source, Beach,RiverBeach
}
public enum TopEnvirType {NULL, Forest,Mountain,Highland }
public enum LandscapeType { Outer,Settlement}
public enum HexDir { TopRight,Right,BottomRight,BottomLeft,Left,TopLeft}
public class TileData
{
  public Vector2Int Coordinate = Vector2Int.zero;
  public BottomEnvirType BottomEnvir = BottomEnvirType.NULL;
  public TopEnvirType TopEnvir = TopEnvirType.NULL;
  public int Rotate = 0;
  public LandscapeType LandScape = LandscapeType.Outer;
  public Settlement TileSettle = null;
  public TileSpriteType TopEnvirSprite = TileSpriteType.NULL;
  public TileSpriteType BottomEnvirSprite= TileSpriteType.Sea;
  public RectTransform Rect = null;
  public TileButtonScript ButtonScript = null;

  public bool Interactable
  {
    get
    {
      if (TopEnvir == TopEnvirType.Mountain || BottomEnvir == BottomEnvirType.Sea) return false;
      return true;
    }
  }
}
