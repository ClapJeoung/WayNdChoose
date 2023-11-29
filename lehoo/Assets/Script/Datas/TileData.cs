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
  /// <summary>
  /// 0:가려짐 1:본적있음 2:현재 시야
  /// </summary>
  public int Fogstate = 0;
  public void SetFog(int value)
  {
    if (Fogstate == value) return;

    Fogstate = value;
    if (value == 2) ButtonScript.SetReveal();
    else if (value == 1) ButtonScript.SetVisible();
  }

  public Settlement TileSettle = null;

  public TileObjScript ButtonScript = null;

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
      int _movepoint = ConstValues.Supply_Default;
      if (BottomEnvir == BottomEnvirType.Sea) _movepoint += ConstValues.Supply_Sea;
      if (TopEnvir == TopEnvirType.Mountain) _movepoint += ConstValues.Supply_Moutain;
      if (TopEnvir == TopEnvirType.Forest) _movepoint += ConstValues.Supply_Forest;
      if (BottomEnvir == BottomEnvirType.River || BottomEnvir == BottomEnvirType.RiverBeach) _movepoint += ConstValues.Supply_River;

      return _movepoint;
    }
  }
}
