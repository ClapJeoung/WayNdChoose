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
[System.Serializable]
public class TileData
{
  public Vector2Int Coordinate = Vector2Int.zero;
  public int Rotation = 0;
  public BottomEnvirType BottomEnvir = BottomEnvirType.NULL;
  public TopEnvirType TopEnvir = TopEnvirType.NULL;
  public LandmarkType Landmark = LandmarkType.Outer;
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

  public Settlement TileSettle = null;

  public TileButtonScript ButtonScript = null;

  public bool Interactable
  {
    get
    {
      if (BottomEnvir == BottomEnvirType.Sea) return false;
      return true;
    }
  }
  public int MovePoint
  {
    get
    {
      int _movepoint = 0;
      if (BottomEnvir == BottomEnvirType.Sea) _movepoint = ConstValues.MovePoint_Sea;
      else if (TopEnvir == TopEnvirType.Mountain) _movepoint = ConstValues.MovePoint_Moutain;
      else if (TopEnvir == TopEnvirType.Forest) _movepoint = ConstValues.MovePoint_Forest;
      else if (BottomEnvir == BottomEnvirType.River || BottomEnvir == BottomEnvirType.RiverBeach) _movepoint = ConstValues.MovePoint_River;
      else _movepoint = ConstValues.MovePoint_Default;

      if (Landmark == LandmarkType.Ritual) _movepoint += ConstValues.Quest_Cult_RitualMovepoint;

      return _movepoint;
    }
  }
}
