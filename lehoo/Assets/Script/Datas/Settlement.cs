using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class Settlement
{
  public SkillType LibraryType = SkillType.Conversation;
  public Settlement(SettlementType settletype)
  {
    Type = settletype;
  }
  public List<PlaceType> EnablePlaces=new List<PlaceType>();//제시될 장소들
  public SettlementType Type;
  public int Index=-1;
  public string OriginName
  {
    get
    {
      switch (Type)
      {
        case SettlementType.Town:
          return SettlementName.TownNams[Index];
        case SettlementType.City:
          return SettlementName.CityNames[Index];
        default:
          return SettlementName.CastleNames[Index];
      }
    }
  }
  public int Discomfort = 0;
  public void AddDiscomfort()
  {
    switch (Type)
    {
      case SettlementType.Town:Discomfort += ConstValues.TownDiscomfortGrowth;return;
      case SettlementType.City:Discomfort += ConstValues.CityDiscomfortGrowth; return;
      default: Discomfort += ConstValues.CastleDiscomfortGrowth; return;
    }
  }
  private string name;
  public string Name { 
    get
    {
      return GameManager.Instance.GetTextData(OriginName);
    }
  }
  public void Setup()
  {
    switch (Type)
    {
      case SettlementType.Town:
        SpringIllust = GameManager.Instance.ImageHolder.GetTownSprite($"{OriginName}_spring");
        SummerIllust = GameManager.Instance.ImageHolder.GetTownSprite($"{OriginName}_summer");
        FallIllust = GameManager.Instance.ImageHolder.GetTownSprite($"{OriginName}autumn");
        WinterIllust = GameManager.Instance.ImageHolder.GetTownSprite($"{OriginName}_winter"); 
        break;
      case SettlementType.City:
        SpringIllust = GameManager.Instance.ImageHolder.GetCitySprite($"{OriginName}_spring");
        SummerIllust = GameManager.Instance.ImageHolder.GetCitySprite($"{OriginName}_summer");
        FallIllust = GameManager.Instance.ImageHolder.GetCitySprite($"{OriginName}autumn");
        WinterIllust = GameManager.Instance.ImageHolder.GetCitySprite($"{OriginName}_winter"); break;
      case SettlementType.Castle:
        SpringIllust = GameManager.Instance.ImageHolder.GetCastleSprite($"{OriginName}_spring");
        SummerIllust = GameManager.Instance.ImageHolder.GetCastleSprite($"{OriginName}_summer");
        FallIllust = GameManager.Instance.ImageHolder.GetCastleSprite($"{OriginName}autumn");
        WinterIllust = GameManager.Instance.ImageHolder.GetCastleSprite($"{OriginName}_winter"); break;
    }
  }
  public Sprite SpringIllust = null, SummerIllust = null, FallIllust = null, WinterIllust = null;
  public Sprite Illust { get
    {
      switch (GameManager.Instance.MyGameData.Turn)
      {
        case 0:
          return SpringIllust;
        case 1:return SummerIllust;
        case 2:return FallIllust;
        case 3:return WinterIllust;
        default:return GameManager.Instance.ImageHolder.DefaultIllust;
      }
    } }

  public bool IsForest = false;//주변 1칸에 숲 여부
  public bool IsRiver=false;//주변 2칸에 강 여부
  public bool IsHighland = false;  //주변 1칸에 언덕 여부
  public bool IsMountain = false;//주변 2칸에 산 여부
  public bool IsSea = false;    //주변 1칸에 바다 여부

  public List<TileData> Tiles=new List<TileData>();//일반 타일맵 기준
  public Vector3 Coordinate
  {
    get
    {
      Vector2Int _pos = Vector2Int.zero;
      foreach (var asdf in Tiles) _pos += asdf.Coordinate;
      return new Vector3(_pos.x/Tiles.Count,_pos.y/Tiles.Count);
    }
  }
  public List<RectTransform> TileIcons=new List<RectTransform>();
  private TileInfoData tileinfodata = null;
  public TileInfoData TileInfoData
  {
    get { 
      if(tileinfodata == null)
      {
        tileinfodata = new TileInfoData();
        tileinfodata.LandScape = LandscapeType.Settlement;
        tileinfodata.Settlement = this;
        if (IsForest) tileinfodata.EnvirList.Add(EnvironmentType.Forest);
        if (IsRiver) tileinfodata.EnvirList.Add(EnvironmentType.River);
        if (IsMountain) tileinfodata.EnvirList.Add(EnvironmentType.Mountain);
        if (IsSea) tileinfodata.EnvirList.Add(EnvironmentType.Sea);
      }
      return tileinfodata; 
    }
  }
  public bool CheckAbleEvent(EventDataDefulat _event)
  {

    switch (_event.AppearSpace)
    {
      case EventAppearType.Outer: return false;
      case EventAppearType.Town:
        if (Type != SettlementType.Town) return false;
        break;
      case EventAppearType.City:
        if (Type != SettlementType.Castle) return false;
        break;
      case EventAppearType.Castle:
        if (Type != SettlementType.Castle) return false;
        break;
      case EventAppearType.Settlement:
        break;
    }

    if (_event.EnvironmentType != EnvironmentType.NULL)
    {
      if(!tileinfodata.EnvirList.Contains(_event.EnvironmentType))
      return false;
    }
    //환경이 맞지 않으면 X

    return true;
  }

  public void SetAvailablePlaces()
  {
    List<PlaceType> _followplaces = new List<PlaceType>();
    List<PlaceType> _normalplaces=new List<PlaceType>();
    List<PlaceType> _enableplaces=new List<PlaceType>();


    for(int i = 0; i < 3; i++)
    {
      PlaceType _targetplce = (PlaceType)i;
      if (GameManager.Instance.EventHolder.IsFollowEventEnable(TileInfoData, _targetplce)) _followplaces.Add(_targetplce);
      else _normalplaces.Add(_targetplce);
    }

    System.Random _rnd=new System.Random();
    int _count = 0;
    switch (Type)
    {
      case SettlementType.Town:
        _count = ConstValues.TownPlaceCount;

        break;
      case SettlementType.City:
        PlaceType _targetplce = PlaceType.Library;
        if (GameManager.Instance.EventHolder.IsFollowEventEnable(TileInfoData, _targetplce)) _followplaces.Add(_targetplce);
        else _normalplaces.Add(_targetplce);
        _count= ConstValues.CityPlaceCount;

        break;
      case SettlementType.Castle:
        PlaceType __targetplce = PlaceType.Theater;
        if (GameManager.Instance.EventHolder.IsFollowEventEnable(TileInfoData, __targetplce)) _followplaces.Add(__targetplce);
        else _normalplaces.Add(__targetplce);

        PlaceType ___targetplce = PlaceType.Academy;
        if (GameManager.Instance.EventHolder.IsFollowEventEnable(TileInfoData, ___targetplce)) _followplaces.Add(___targetplce);
        else _normalplaces.Add(___targetplce);
        _count = ConstValues.CastlePlaceCount;

        break;
    }

    if (_followplaces.Count > _count)
    {
      var _temp = _followplaces.OrderBy(x => _rnd.Next()).ToList();
      for (int i = 0; i < _count; i++)
        _enableplaces.Add(_temp[i]);
      _enableplaces.Sort();
    }//연계 가능한 장소가 목표보다 많으면 그 중 무작위 선택해 반환
    else if (_followplaces.Count == _count)
    {
      _enableplaces = _followplaces;
    }//연계 가능한 장소가 목표 개수랑 동일하면 그대로 반환
    else
    {
      var _temp = _normalplaces.OrderBy(x => _rnd.Next()).ToList();
      for (int i = 0; i < _count - _followplaces.Count; i++)
      {
        _followplaces.Add(_temp[i]);
      }
      _followplaces.Sort();
      _enableplaces = _followplaces;
    }//연계 가능한 장소가 목표치에 못 도달하면 일반 장소 중 무작위로 선택해 반환

    EnablePlaces = _enableplaces;
  }
}
public class MapData
{
  public int CircleHexCount(int range)
  {
    List<Vector2Int> _center=new List<Vector2Int>();
    _center.Add(Vector2Int.zero);
    var _coors = GetAroundCoor(_center, range);
    return _coors.Count;
  }
  public Vector2Int GetNextCoor(Vector2Int coor,HexDir dir)
  {
    int  _y=coor.y%2;
    Vector2Int _mod = Vector2Int.zero;
    if ( _y.Equals(0))
    {
      switch (dir)
      {
        case HexDir.TopRight:
          _mod = new Vector2Int(0, 1);break;
        case HexDir.Right:
          _mod = new Vector2Int(1, 0); break;
        case HexDir.BottomRight:
          _mod = new Vector2Int(0, -1); break;
        case HexDir.BottomLeft:
          _mod = new Vector2Int(-1, -1); break;
        case HexDir.Left:
          _mod = new Vector2Int(-1,0); break;
        case HexDir.TopLeft:
          _mod = new Vector2Int(-1, 1); break;
      }
    }
    else
    {
      switch (dir)
      {
        case HexDir.TopRight:
          _mod = new Vector2Int(1, 1); break;
        case HexDir.Right:
          _mod = new Vector2Int(1, 0); break;
        case HexDir.BottomRight:
          _mod = new Vector2Int(1, -1); break;
        case HexDir.BottomLeft:
          _mod = new Vector2Int(0, -1); break;
        case HexDir.Left:
          _mod = new Vector2Int(-1, 0); break;
        case HexDir.TopLeft:
          _mod = new Vector2Int(0, 1); break;
      }
    }
    Vector2Int _temp = coor + _mod;
    if (_temp.x < 0) _temp.x = 0;
    if(_temp.y < 0) _temp.y = 0;
    if (_temp.x >= ConstValues.MapSize) _temp.x = ConstValues.MapSize - 1;
    if(_temp.y>=ConstValues.MapSize)_temp.y=ConstValues.MapSize - 1;
    return _temp;
  }
  public Vector2Int GetNextCoor(TileData tile, HexDir dir)
  {
    return GetNextCoor(tile.Coordinate, dir);
  }
  public TileData GetNextTile(Vector2Int coor,HexDir dir)
  {
    var _coor=GetNextCoor(coor,dir);
    return TileDatas[_coor.x,_coor.y];
  }
  public TileData GetNextTile(TileData tile, HexDir dir)
  {
    var _coor = GetNextCoor(tile.Coordinate, dir);
    return TileDatas[_coor.x, _coor.y];
  }
  public List<TileData> GetAroundTile(TileData tile, int range)
  {
    List<Vector2Int> _coors = GetAroundCoor(tile.Coordinate, range);
    List<TileData> _tiles = new List<TileData>();
    foreach (var _coor in _coors) _tiles.Add(TileDatas[_coor.x, _coor.y]);
    return _tiles;
  }
  public List<TileData> GetAroundTile(Vector2Int coor, int range)
  {
    List<Vector2Int> _coors = GetAroundCoor( coor , range);
    List<TileData> _tiles = new List<TileData>();
    foreach (var _coor in _coors) _tiles.Add(TileDatas[_coor.x, _coor.y]);
    return _tiles;
  }
  public List<TileData> GetAroundTile(List<Vector2Int> coors,int range)
  {
    List<Vector2Int> _coors = GetAroundCoor(coors, range);
    List<TileData> _tiles = new List<TileData>();
    foreach(var _coor in _coors)_tiles.Add(TileDatas[_coor.x,_coor.y]);
    return _tiles;
  }
  public List<TileData> GetAroundTile(List<TileData> tiles, int range)
  {
    List<Vector2Int> _coors = new List<Vector2Int>();
    foreach (var _tile in tiles) _coors.Add(_tile.Coordinate);
    List<Vector2Int> _aroundcoors = GetAroundCoor(_coors, range);
    List<TileData> _tiles = new List<TileData>();
    foreach (var _coor in _aroundcoors) _tiles.Add(TileDatas[_coor.x, _coor.y]);
    return _tiles;
  }
  public List<Vector2Int> GetAroundCoor(Vector2Int coor, int range)
  {
    List<Vector2Int> _targetcoors =new List<Vector2Int> { coor };

    for (int i = 0; i < range; i++)
    {
      List<Vector2Int> _temp = new List<Vector2Int>();

      foreach (var _coor in _targetcoors)
        for (int j = 0; j < 6; j++) _temp.Add(GetNextCoor(_coor, (HexDir)j));

      foreach (var _coor in _temp)
        if (!_targetcoors.Contains(_coor)) _targetcoors.Add(_coor);
    }
    return _targetcoors;
  }
  public List<Vector2Int> GetAroundCoor(List<Vector2Int> coor,int range)
  {
    List<Vector2Int> _targetcoors = new List<Vector2Int>();
    foreach (var _coor in coor) _targetcoors.Add(_coor);

    for (int i = 0; i < range; i++)
    {
      List<Vector2Int> _temp = new List<Vector2Int>();

      foreach (var _coor in _targetcoors)
        for (int j = 0; j < 6; j++) _temp.Add(GetNextCoor(_coor, (HexDir)j));

      foreach (var _coor in _temp)
        if (!_targetcoors.Contains(_coor)) _targetcoors.Add(_coor);
    }
    return _targetcoors;
  }
  public TileData Tile(Vector2Int coor) { return TileDatas[coor.x,coor.y]; }
  public TileData Tile(Vector3 coor) { return TileDatas[(int)coor.x, (int)coor.y]; }
  public List<TileData> GetEnvirTiles(List<BottomEnvirType> includebottom)
  {
    List<TileData> _result = new List<TileData>();
    foreach (var _tile in TileDatas)
      if (includebottom.Contains(_tile.BottomEnvir)) _result.Add(_tile);
    return _result;
  }
  public List<TileData> GetEnvirTiles(List<TopEnvirType> includetop)
  {
    List<TileData> _result = new List<TileData>();
    foreach (var _tile in TileDatas)
      if (includetop.Contains(_tile.TopEnvir)) _result.Add(_tile);
    return _result;
  }
  /// <summary>
  /// (포함,포함) (포함,제외) (제외,포함) (제외,제외)
  /// </summary>
  /// <param name="bottoms"></param>
  /// <param name="tops"></param>
  /// <param name="type"></param>
  /// <returns></returns>
  public List<TileData> GetEnvirTiles(List<BottomEnvirType> bottoms,List<TopEnvirType> tops,int type)
  {
    List<TileData> _result= new List<TileData>();
    foreach (var _tile in TileDatas)
    {
      switch (type)
      {
        case 0:
          if (bottoms.Contains(_tile.BottomEnvir) && tops.Contains(_tile.TopEnvir)) _result.Add(_tile);
          break;
        case 1:
          if (bottoms.Contains(_tile.BottomEnvir) && !tops.Contains(_tile.TopEnvir)) _result.Add(_tile);
          break;
        case 2:
          if (!bottoms.Contains(_tile.BottomEnvir) && tops.Contains(_tile.TopEnvir)) _result.Add(_tile);
          break;
        case 3:
          if (!bottoms.Contains(_tile.BottomEnvir) && !tops.Contains(_tile.TopEnvir)) _result.Add(_tile);
          break;
      }
    }
    return _result;
  }
  public List<TileData> GetDirLines(TileData targetcenter, HexDir dir)
  {
    List<TileData> _lines= new List<TileData>();
    _lines.Add(targetcenter);
    while (true)
    {
      TileData _next = GetNextTile(_lines[_lines.Count - 1], dir);
      _lines.Add(_next);
      if (_lines.Count>3&&( _next.BottomEnvir == BottomEnvirType.Beach || _next.BottomEnvir == BottomEnvirType.RiverBeach))
      {
        break;
      }
    }

    return _lines;
  }
  public TileData CenterTile
  {
    get
    {
      return TileDatas[ConstValues.MapSize / 2 + ConstValues.MapSize % 2, ConstValues.MapSize / 2 + ConstValues.MapSize % 2];
    }
  }
  public Settlement GetSettle(string originname)
  {
    for(int i = 0; i < AllSettles.Count; i++)
    {
      if (AllSettles[i].OriginName.Equals(originname)) return AllSettles[i];
    }
    return null;
  }
  public List<Settlement> GetCloseSettles(Settlement _origin,int _count)
  {
    Vector2 _originpos = _origin.Coordinate;

    List<float> _distance=new List<float>();
    List<Settlement> _settlement=new List<Settlement>();
    foreach (Settlement _settle in AllSettles)
    {
      if (_settle == _origin) continue;

      float _dis=Vector2.Distance(_settle.Coordinate, _originpos);
      //  Debug.Log($"_originpos 에서 _settle.VectorPos()까지의 거리 : {_dis}");
      _distance.Add(_dis);
      _settlement.Add(_settle);
    }//모든 정착지로부터 거리,정착지가 담긴 딕셔너리 생성

    List<Settlement> _closest = new List<Settlement>();
    for (int i = 0; i < _count; i++)
    {
      float _min = _distance.Min();
      int _index = _distance.IndexOf(_min);
      _closest.Add(_settlement[_index]);

      _distance.RemoveAt(_index); 
      _settlement.RemoveAt(_index);
    }


    return _closest;
  }//_origin 정착지로부터 제일 가까운 3개
  public TileInfoData GetTileData(Vector2 _tilepos)
  {
    Vector2Int _tileintpos = new Vector2Int(Mathf.RoundToInt(_tilepos.x), Mathf.RoundToInt(_tilepos.y));
    TileData _centertile = Tile(_tileintpos);
    List<TileData> _tiles_1 = GetAroundTile(_tileintpos, 1);
    List<TileData> _tiles_2 = GetAroundTile(_tileintpos, 2);
    _tiles_1.Add(_centertile);
    _tiles_2.Add(_centertile);
    TileInfoData _data = new TileInfoData();
    _data.LandScape = _centertile.LandScape;
    if(_centertile.TileSettle!=null)_data.Settlement= _centertile.TileSettle;
    //월드 좌표 -> 타일 좌표

    EnvironmentType _centerbottomenvir = EnvironmentType.NULL;
    switch (_centertile.BottomEnvir)
    {
      case BottomEnvirType.Land: _centerbottomenvir = EnvironmentType.Land; break;
      case BottomEnvirType.River: _centerbottomenvir = EnvironmentType.River; break;
      case BottomEnvirType.Sea: _centerbottomenvir = EnvironmentType.Sea; break;
      case BottomEnvirType.Beach: _centerbottomenvir = EnvironmentType.Beach; break;
      case BottomEnvirType.RiverBeach: _centerbottomenvir = EnvironmentType.RiverBeach; break;
    }
    EnvironmentType _centertopenvir = EnvironmentType.NULL;
    switch (_centertile.TopEnvir)
    {
      case TopEnvirType.Forest: _centertopenvir = EnvironmentType.Forest; break;
      case TopEnvirType.Mountain: _centertopenvir = EnvironmentType.Mountain; break;
      case TopEnvirType.Highland: _centertopenvir = EnvironmentType.Highland; break;
    }
    if(!_data.EnvirList.Contains(_centerbottomenvir))_data.EnvirList.Add(_centerbottomenvir);
    if (!_data.EnvirList.Contains(_centertopenvir)) _data.EnvirList.Add(_centertopenvir);

    foreach(var tile_1 in _tiles_1)
    {
      EnvironmentType _range1bottomenvir = EnvironmentType.NULL;
      switch (tile_1.BottomEnvir)
      {
        case BottomEnvirType.Land: _range1bottomenvir = EnvironmentType.Land; break;
        case BottomEnvirType.River: _range1bottomenvir = EnvironmentType.River; break;
        case BottomEnvirType.Sea: _range1bottomenvir = EnvironmentType.Sea; break;
        case BottomEnvirType.Beach: _range1bottomenvir = EnvironmentType.Beach; break;
        case BottomEnvirType.RiverBeach: _range1bottomenvir = EnvironmentType.RiverBeach; break;
      }
      EnvironmentType _range1topenvir = EnvironmentType.NULL;
      switch (tile_1.TopEnvir)
      {
        case TopEnvirType.Forest: _range1topenvir = EnvironmentType.Forest; break;
        case TopEnvirType.Mountain: _range1topenvir = EnvironmentType.Mountain; break;
        case TopEnvirType.Highland: _range1topenvir = EnvironmentType.Highland; break;
      }
      if (!_data.EnvirList.Contains(_range1bottomenvir)) _data.EnvirList.Add(_range1bottomenvir);
      if (!_data.EnvirList.Contains(_range1topenvir)) _data.EnvirList.Add(_range1topenvir);
    }

    foreach (var tile_2 in _tiles_2)
    {
      EnvironmentType _range2bottomenvir = EnvironmentType.NULL;
      switch (tile_2.BottomEnvir)
      {
        case BottomEnvirType.Sea: _range2bottomenvir = EnvironmentType.Sea; break;
        case BottomEnvirType.Beach: _range2bottomenvir = EnvironmentType.Beach; break;
        case BottomEnvirType.RiverBeach: _range2bottomenvir = EnvironmentType.RiverBeach; break;
      }
      EnvironmentType _range2topenvir = EnvironmentType.NULL;
      switch (tile_2.TopEnvir)
      {
        case TopEnvirType.Mountain: _range2topenvir = EnvironmentType.Mountain; break;
      }
      if (!_data.EnvirList.Contains(_range2bottomenvir)) _data.EnvirList.Add(_range2bottomenvir);
      if (!_data.EnvirList.Contains(_range2topenvir)) _data.EnvirList.Add(_range2topenvir);
    }


    return _data;
  }//월드 기준 타일 1개 좌표 하나 받아서 그 주위 2칸의 산 바다, 나머지 1타일


  public TileData[,] TileDatas;
  public List<Settlement> Towns = new List<Settlement>();
  public List<Settlement> Cities = new List<Settlement>();
  public Settlement Castles = null;
  public List<Settlement> AllSettles = new List<Settlement>();

}//게임 상에서 꺼내 쓰기 편리하게 변환한 지도 데이터
public static class SettlementName
{
  public static string[] TownNams =
  {
    "alion","amberfort","arthurstone","ashbrook","brigelis","calibria","eldermist","elvendom","evergreenvale","galania","ironcliff","raia","rappland","sellier","silberia","sunridge","synodia","windmere"
  };
  public static string[] CityNames =
  {
    "albace","avalomia","ellsworth","emael","iberton","lumia","niverdale","solaria","sylveria","valtar"
  };
  public static string[] CastleNames =
  {
    "aberstead","arcadimia","catrua","lugrea","mabrel","meridina","penbrite","solanum","tryon","udrium"
  };
}

