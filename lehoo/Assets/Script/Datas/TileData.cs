using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BottomEnvirType
{
 NULL, Land,River,Sea,Source, Beach,RiverBeach
}
public enum TopEnvirType {NULL, Forest,Mountain,Highland }
public enum LandmarkType { Outer,Village,Town,City,Ritual}
public enum HexDir { TopRight,Right,BottomRight,BottomLeft,Left,TopLeft}
public class TileData
{
  public Vector2Int Coordinate = Vector2Int.zero;
  public BottomEnvirType BottomEnvir = BottomEnvirType.NULL;
  public TopEnvirType TopEnvir = TopEnvirType.NULL;
  public int Rotation = 0;
  public LandmarkType Landmark = LandmarkType.Outer;
  public Settlement TileSettle = null;
  public TileSpriteType TopEnvirSprite = TileSpriteType.NULL;
  public TileSpriteType BottomEnvirSprite= TileSpriteType.Sea;
  public TileSpriteType landmarkSprite
  {
    get 
    {
      switch (Landmark)
      {
        case LandmarkType.Village:return TileSpriteType.Village;
          case LandmarkType.Town:return TileSpriteType.Town;
          case LandmarkType.City:return TileSpriteType.City;
          case LandmarkType.Ritual:return TileSpriteType.Ritual;
 //       case LandmarkType.RitualProgress:return TileSpriteType.RitualProgress;
      }
      return TileSpriteType.NULL;
    }
  }
  public RectTransform Rect = null;
  public TileButtonScript ButtonScript = null;

  public bool Interactable
  {
    get
    {
      if (BottomEnvir == BottomEnvirType.Sea) return false;
      return true;
    }
  }
}
