using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public enum BottomEnvirType
{
 NULL, Land,River,Sea,Source, Beach,RiverBeach
}
public enum TopEnvirType {NULL, Forest,Mountain,Highland }
public enum LandmarkType { Outer,Village,Town,City,Ritual}
public enum HexDir { TopRight,Right,BottomRight,BottomLeft,Left,TopLeft}
public enum TileTypeEnum { Normal,Landmark}

[System.Serializable]
public class TileData
{
  private int resourcetype = -1;
  /// <summary>
  /// 자원 타입(0~5)
  /// </summary>
  public int ResourceType
  {
    get
    {
      if (resourcetype == -1)
      {
        if (TopEnvir == TopEnvirType.Mountain) resourcetype = 4;
        else if ((BottomEnvir == BottomEnvirType.River || BottomEnvir == BottomEnvirType.RiverBeach) && TopEnvir == TopEnvirType.Forest) resourcetype = 3;
        else if (BottomEnvir == BottomEnvirType.River || BottomEnvir == BottomEnvirType.RiverBeach) resourcetype = 2;
        else if (TopEnvir == TopEnvirType.Forest) resourcetype = 1;
        else resourcetype = 0;
      }
      return resourcetype;
    }
  }
  private int resourcecount = -1;
  /// <summary>
  /// 자원 개수
  /// </summary>
  public int ResourceCount
  {
    get
    {
      if(resourcecount == -1)
      {
        switch (ResourceType)
        {
          case 0:resourcecount = GameManager.Instance.Status.ResourceCount_Land;break;
          case 1: resourcecount = GameManager.Instance.Status.ResourceCount_Forest; break;
          case 2: resourcecount = GameManager.Instance.Status.ResourceCount_River; break;
          case 3: resourcecount = GameManager.Instance.Status.ResourceCount_Land; break;
          case 4: resourcecount = GameManager.Instance.Status.ResourceCount_Mountain; break;
        }
      }
      return resourcecount;
    }
  }
  private string resourcetext = "";
  /// <summary>
  /// 자원 텍스트 <sprite=123>...
  /// </summary>
  public string ResourceText
  {
    get
    {
      if (resourcetext == "")
      {
        string _text = "";
        switch (ResourceType)
        {
          case 0:_text = "<sprite=116>";break;
          case 1: _text = "<sprite=117>"; break;
          case 2: _text = "<sprite=118>"; break;
          case 3: _text = "<sprite=119>"; break;
          case 4: _text = "<sprite=120>"; break;
        }
        StringBuilder _sb= new StringBuilder();
        for(int i=0;i<ResourceCount;i++)
        {
          _sb.Append(_text);
        }
        resourcetext = _sb.ToString();
      }
      return resourcetext;
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
  public TileTypeEnum TileType = TileTypeEnum.Normal;
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
    if (value == 2)
    {
      ButtonScript.SetReveal();
      if (BottomEnvir != BottomEnvirType.Sea)
        ButtonScript.Rect.GetComponent<Onpointer_tileoutline>().enabled = true;
      if (TileSettle != null)
        ButtonScript.Preview.enabled = true;

      ButtonScript.BottomImage.enabled = true;
      if (ButtonScript.TopImage != null) ButtonScript.TopImage.enabled = true;
      if (ButtonScript.LandmarkImage != null) ButtonScript.LandmarkImage.enabled = true;
    }
    else if (value == 1)
    {
      ButtonScript.SetVisible();
      ButtonScript.BottomImage.enabled = true;
      if (ButtonScript.TopImage != null) ButtonScript.TopImage.enabled = true;
      if (ButtonScript.LandmarkImage != null) ButtonScript.LandmarkImage.enabled = true;
    }
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
      int _movepoint = GameManager.Instance.Status.Supply_Default;
      if (BottomEnvir == BottomEnvirType.Sea) _movepoint += GameManager.Instance.Status.Supply_Sea;
      if (TopEnvir == TopEnvirType.Mountain) _movepoint += GameManager.Instance.Status.Supply_Moutain;
      if (TopEnvir == TopEnvirType.Forest) _movepoint += GameManager.Instance.Status.Supply_Forest;
      if (BottomEnvir == BottomEnvirType.River || BottomEnvir == BottomEnvirType.RiverBeach) _movepoint += GameManager.Instance.Status.Supply_River;

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
