using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Security.Cryptography;
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
  public bool IsEvent = false;
  public bool IsResource = false;
  public int ResourceType
  {
    get
    {
      int _id = 0;
      if (TopEnvir == TopEnvirType.Mountain) _id = 4;
      else if ((BottomEnvir == BottomEnvirType.River || BottomEnvir == BottomEnvirType.RiverBeach) && TopEnvir == TopEnvirType.Forest) _id = 3;
      else if (BottomEnvir == BottomEnvirType.River || BottomEnvir == BottomEnvirType.RiverBeach) _id = 2;
      else if (TopEnvir == TopEnvirType.Forest) _id = 1;
      else _id = 0;
      return _id;
    }
  }
  public Vector2Int Coordinate = Vector2Int.zero;
  private HexGrid hexgrid = null;
  public HexGrid HexGrid
  {
    get 
    {
      if (hexgrid is null) hexgrid = new HexGrid(Coordinate);
      return hexgrid;
    }
  }
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
    if (value == 2) { ButtonScript.SetReveal(); 
      if(BottomEnvir!=BottomEnvirType.Sea)
      ButtonScript.Rect.GetComponent<Onpointer_tileoutline>().enabled = true; }
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
  public int RequireSupply
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
  public EnvironmentType RandomEnvir
  {
    get
    {
      List<EnvironmentType> _allenvirs= new List<EnvironmentType>();
      switch (BottomEnvir)
      {
        case BottomEnvirType.Land: _allenvirs.Add(EnvironmentType.Land);
          break;
        case BottomEnvirType.River: _allenvirs.Add(EnvironmentType.River);
          break;
        case BottomEnvirType.Beach: _allenvirs.Add(EnvironmentType.Beach); _allenvirs.Add(EnvironmentType.Sea);
          break;
        case BottomEnvirType.RiverBeach:
          _allenvirs.Add(EnvironmentType.River); _allenvirs.Add(EnvironmentType.Sea);
          break;
      }
      switch (TopEnvir)
      {
        case TopEnvirType.NULL:
          break;
        case TopEnvirType.Forest: 
          if (_allenvirs.Contains(EnvironmentType.Land)) _allenvirs.Remove(EnvironmentType.Land);
          _allenvirs.Add(EnvironmentType.Forest);
          break;
        case TopEnvirType.Mountain:
          if (_allenvirs.Contains(EnvironmentType.Land)) _allenvirs.Remove(EnvironmentType.Land);
          _allenvirs.Add(EnvironmentType.Mountain);
          break;
        case TopEnvirType.Highland:
          break;
      }
      return _allenvirs[Random.Range(0,_allenvirs.Count - 1)];
    }
  }
}
