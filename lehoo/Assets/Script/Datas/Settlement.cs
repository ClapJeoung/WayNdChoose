using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEngine.UIElements;
using Newtonsoft.Json;

public class Settlement
{
  public int Index = -1;
  public int Discomfort = 0;
  public bool IsForest = false;//주변 1칸에 숲 여부
  public bool IsRiver = false;//주변 1칸에 강 여부
  public bool IsHighland = false;  //주변 1칸에 언덕 여부
  public bool IsMountain = false;//주변 1칸에 산 여부
  public bool IsSea = false;    //주변 1칸에 바다 여부

  public TileData Tile = null;

  public Settlement(SettlementType settletype)
  {
    SettlementType = settletype;
  }
  private List<SectorTypeEnum> sectors=new List<SectorTypeEnum>();
  public List<SectorTypeEnum> Sectors
  {
    get
    {
      if (sectors.Count == 0)
      {
        sectors = new List<SectorTypeEnum>();
        switch (SettlementType)
        {
          case SettlementType.Village:
            sectors.Add(SectorTypeEnum.Residence);
            sectors.Add(SectorTypeEnum.Temple);
            break;
          case SettlementType.Town:
            sectors.Add(SectorTypeEnum.Temple);
            sectors.Add(SectorTypeEnum.Marketplace);
            break;
          case SettlementType.City:
            sectors.Add(SectorTypeEnum.Marketplace);
            sectors.Add(SectorTypeEnum.Library);
            break;
        }
      }
      return sectors;
    }
  }
  public SettlementType SettlementType;
  /*
  public string OriginName
  {
    get
    {
      switch (SettlementType)
      {
        case SettlementType.Village:
          return SettlementName.VillageNames[Index];
        case SettlementType.Town:
          return SettlementName.TownNames[Index];
        default:
          return SettlementName.CityNames[Index];
      }
    }
  }
  public string Name { 
    get
    {
      return GameManager.Instance.GetTextData(OriginName);
    }
  }
  */
  public Vector3 Position
  {
    get
    {
      return new Vector3(Tile.Coordinate.x, Tile.Coordinate.y,0.0f);
    }
  }
  public GameObject HolderObject
  {
    get
    {
      return Tile.ButtonScript.Rect.transform.parent.gameObject;
    }
  }
  private TileInfoData tileinfodata = null;
  public TileInfoData TileInfoData
  {
    get { 
      if(tileinfodata == null)
      {
        tileinfodata = new TileInfoData();
        tileinfodata.Landmark = LandmarkType.Outer;
        switch (SettlementType)
        {
          case SettlementType.Village:tileinfodata.Landmark = LandmarkType.Village;break;
          case SettlementType.Town:tileinfodata.Landmark = LandmarkType.Town;break;
          case SettlementType.City: tileinfodata.Landmark= LandmarkType.City;break;
        }
        tileinfodata.Settlement = this;
        if (IsForest) tileinfodata.EnvirList.Add(EnvironmentType.Forest);
        if (IsRiver) tileinfodata.EnvirList.Add(EnvironmentType.River);
        if (IsMountain) tileinfodata.EnvirList.Add(EnvironmentType.Mountain);
        if (IsSea) tileinfodata.EnvirList.Add(EnvironmentType.Sea);
      }
      return tileinfodata; 
    }
  }
}
public class MapData
{
  public void SetEventTiles()
  {
    foreach (var _eventtile in EventTiles)
    {
      _eventtile.IsEvent = false;
      _eventtile.ButtonScript.EventMarkImage.enabled = false;
    }

    List<EnvironmentType> _priorenvirs = new List<EnvironmentType>();
    foreach (var _event in GameManager.Instance.EventHolder.AllEvent)
    {
      if (!GameManager.Instance.MyGameData.IsAbleEvent(_event.ID)) continue;
      if (_event.EnvironmentType == EnvironmentType.Land) continue;
      if (!GameManager.Instance.MyGameData.IsEventTypeEnable(_event, true)) continue;

      if (!_priorenvirs.Contains(_event.EnvironmentType)) _priorenvirs.Add(_event.EnvironmentType);
    }

    List<TileData> _neweventtiles = new List<TileData>();

    int _worldeventcount = 0;
    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:

        TileData _culttarget = null;
        int _min = 100;
        switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
        {
          case 0:
            foreach (var _target in Villages)
            {
              int _distance = _target.Tile.HexGrid.GetDistance(GameManager.Instance.MyGameData.CurrentTile);
              if (_distance < _min)
              {
                _min = _distance;
                _culttarget = _target.Tile;
              }
            }
            break;
          case 1:
            foreach (var _target in Towns)
            {
              int _distance = _target.Tile.HexGrid.GetDistance(GameManager.Instance.MyGameData.CurrentTile);
              if (_distance < _min)
              {
                _min = _distance;
                _culttarget = _target.Tile;
              }
            }
            break;
          case 2:
            foreach (var _target in Citys)
            {
              int _distance = _target.Tile.HexGrid.GetDistance(GameManager.Instance.MyGameData.CurrentTile);
              if (_distance < _min)
              {
                _min = _distance;
                _culttarget = _target.Tile;
              }
            }
            break;
          case 3:
            switch (GameManager.Instance.MyGameData.Cult_SabbatSector)
            {
              case SectorTypeEnum.Residence:
                foreach (var _target in Villages)
                {
                  int _distance = _target.Tile.HexGrid.GetDistance(GameManager.Instance.MyGameData.CurrentTile);
                  if (_distance < _min)
                  {
                    _min = _distance;
                    _culttarget = _target.Tile;
                  }
                }
                break;
              case SectorTypeEnum.Temple:
                foreach (var _target in Villages)
                {
                  int _distance = _target.Tile.HexGrid.GetDistance(GameManager.Instance.MyGameData.CurrentTile);
                  if (_distance < _min)
                  {
                    _min = _distance;
                    _culttarget = _target.Tile;
                  }
                }
                foreach (var _target in Towns)
                {
                  int _distance = _target.Tile.HexGrid.GetDistance(GameManager.Instance.MyGameData.CurrentTile);
                  if (_distance < _min)
                  {
                    _min = _distance;
                    _culttarget = _target.Tile;
                  }
                }
                break;
              case SectorTypeEnum.Marketplace:
                foreach (var _target in Towns)
                {
                  int _distance = _target.Tile.HexGrid.GetDistance(GameManager.Instance.MyGameData.CurrentTile);
                  if (_distance < _min)
                  {
                    _min = _distance;
                    _culttarget = _target.Tile;
                  }
                }
                foreach (var _target in Citys)
                {
                  int _distance = _target.Tile.HexGrid.GetDistance(GameManager.Instance.MyGameData.CurrentTile);
                  if (_distance < _min)
                  {
                    _min = _distance;
                    _culttarget = _target.Tile;
                  }
                }
                break;
              case SectorTypeEnum.Library:
                foreach (var _target in Citys)
                {
                  int _distance = _target.Tile.HexGrid.GetDistance(GameManager.Instance.MyGameData.CurrentTile);
                  if (_distance < _min)
                  {
                    _min = _distance;
                    _culttarget = _target.Tile;
                  }
                }
                break;
            }
            break;
          case 4:
            _culttarget = GameManager.Instance.MyGameData.Cult_RitualTile;
            break;
        }

        List<TileData> _enablerange = GetAroundTile(GameManager.Instance.MyGameData.CurrentTile, ConstValues.WorldEventRange_max);
        List<TileData> _minrangetiles = GetAroundTile(GameManager.Instance.MyGameData.CurrentTile, ConstValues.WorldEventRange_min - 1);
        foreach (var _removetile in _minrangetiles)
          _enablerange.Remove(_removetile);
        //주위 3칸~5칸

        _neweventtiles.Add(GetRandomEventTile(_culttarget));


        if (GameManager.Instance.MyGameData.Quest_Cult_Progress < ConstValues.WorldEventPhase_1_Cult)
          _worldeventcount = ConstValues.WorldEventCount_0;
        if (GameManager.Instance.MyGameData.Quest_Cult_Progress < ConstValues.WorldEventPhase_2_Cult)
          _worldeventcount = ConstValues.WorldEventCount_1;
        else _worldeventcount = ConstValues.WorldEventCount_2;

        if (_worldeventcount == 0)
        {

        }
        else
        {
          var _allsettlements = AllSettles.OrderBy(settle => settle.Tile.HexGrid.GetDistance(GameManager.Instance.MyGameData.CurrentTile)).ToList();
          List<TileData> _worldeventtargets = new List<TileData>();
          int _indextemp = 0;
          while (_worldeventtargets.Count < _worldeventcount+1)
          {
            if (_allsettlements[_indextemp].Tile == _culttarget)
            {
              _indextemp++;
              continue;
            }

            _worldeventtargets.Add(_allsettlements[_indextemp].Tile);
            _indextemp++;
          }
          foreach (var _target in _worldeventtargets)
            _neweventtiles.Add(GetRandomEventTile(_target));
        }

        EventTiles.Clear();
        foreach (var _tile in _neweventtiles)
        {
          EventTiles.Add(_tile);
          _tile.IsEvent = true;
          _tile.ButtonScript.EventMarkImage.enabled = true;
        }
        break;


        TileData GetRandomEventTile(TileData targetcentertile)
        {
          List<TileData> _targettiles = new List<TileData>();
          if (targetcentertile.HexGrid.GetDistance(GameManager.Instance.MyGameData.CurrentTile) < 3)
          {
            List<TileData> _targetaround = GetAroundTile(targetcentertile, ConstValues.CultEventRange_Target);
            _targetaround.Remove(targetcentertile);
            _targetaround.Remove(GameManager.Instance.MyGameData.CurrentTile);
            foreach (var _tile in _targetaround)
            {
              if (!_enablerange.Contains(_tile) ||
                !_tile.Interactable ||
                _tile.Landmark != LandmarkType.Outer ||
                EventTiles.Contains(_tile) ||
                _tile.Fogstate == 0) continue;
              bool _isoverlap = false;
              foreach (var _preeventtile in _neweventtiles)
                foreach (var _aroundtile in GetAroundTile(_preeventtile, 1))
                {
                  if (_tile.Coordinate == _aroundtile.Coordinate)
                  {
                    _isoverlap = true;
                    break;
                  }
                  if (_isoverlap) break;
                }
              if (_isoverlap) continue;

              _targettiles.Add(_tile);
            }
          }
          else
          {
            List<TileData> _route = new List<TileData>();
            _route.Add(GameManager.Instance.MyGameData.CurrentTile);

            foreach (var _dir in targetcentertile.HexGrid.GetRoute(GameManager.Instance.MyGameData.CurrentTile))
              _route.Add(GetNextTile(_route[_route.Count - 1], _dir));

            _route = GetAroundTile(_route, 2);
            _route.Remove(GameManager.Instance.MyGameData.CurrentTile);
            foreach (var _tile in _route)
            {
              if (!_enablerange.Contains(_tile) || !_tile.Interactable || _tile.TileSettle != null || EventTiles.Contains(_tile) || _tile.Fogstate == 0) continue;
              _targettiles.Add(_tile);
            }
          }

          int _fog0count = 3, _envircount = 10, _envirdefaultcount = 1, _fogdefaultcount = 0;
          List<int> _indexes = new List<int>();
          for (int i = 0; i < _targettiles.Count; i++)
          {
            int _count = 0;
            List<TileData> _aroundtile = GetAroundTile(_targettiles[i], 1);
            _aroundtile.Remove(_targettiles[i]);
            foreach (var _tile in _aroundtile)
              _count += _tile.Fogstate == 0 ? _fog0count : _fogdefaultcount;
            switch (_targettiles[i].BottomEnvir)
            {
              case BottomEnvirType.NULL:
              case BottomEnvirType.Land:
              case BottomEnvirType.Sea:
              case BottomEnvirType.Source:
                break;
              case BottomEnvirType.River:
              case BottomEnvirType.RiverBeach:
                _count += _priorenvirs.Contains(EnvironmentType.River) ? _envircount : _envirdefaultcount;
                break;
              case BottomEnvirType.Beach:
                _count += _priorenvirs.Contains(EnvironmentType.Beach) ? _envircount : _envirdefaultcount;
                break;
            }
            switch (_targettiles[i].TopEnvir)
            {
              case TopEnvirType.NULL:
                break;
              case TopEnvirType.Forest:
                _count += _priorenvirs.Contains(EnvironmentType.Forest) ? _envircount : _envirdefaultcount;
                break;
              case TopEnvirType.Mountain:
                _count += _priorenvirs.Contains(EnvironmentType.Mountain) ? _envircount : _envirdefaultcount;
                break;
              case TopEnvirType.Highland:
                _count += _priorenvirs.Contains(EnvironmentType.Highland) ? _envircount : _envirdefaultcount;
                break;
            }
            for (int j = 0; j < _count; j++) _indexes.Add(i);
          }

          return _targettiles[_indexes[Random.Range(0, _indexes.Count)]];
        }
    }

  }
  public static int GetMinLength(TileData starttile, List<Settlement> settlements)
  {
    int _min = 100;
    foreach (var settlement in settlements)
    {
      int _length = starttile.HexGrid.GetDistance(settlement.Tile.HexGrid);
      if(_length<_min) _min = _length;
    }
    return _min;
  }
  public static int GetMaxLength(TileData starttile, List<Settlement> settlements)
  {
    int _max = -1;
    foreach (var settlement in settlements)
    {
      int _length = starttile.HexGrid.GetDistance(settlement.Tile.HexGrid);
      if (_length > _max) _max = _length;
    }
    return _max;
  }
  public Vector2Int GetNextCoor(Vector2Int coor,HexDir dir,bool limitgrid)
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
    if (!limitgrid) return _temp;

    if (_temp.x < 0) _temp.x = 0;
    if(_temp.y < 0) _temp.y = 0;
    if (_temp.x >= ConstValues.MapSize) _temp.x = ConstValues.MapSize - 1;
    if(_temp.y>=ConstValues.MapSize)_temp.y=ConstValues.MapSize - 1;
    return _temp;
  }
  public Vector2Int GetNextCoor(TileData tile, HexDir dir,bool limitgrid)
  {
    return GetNextCoor(tile.Coordinate, dir, limitgrid);
  }
  public TileData GetNextTile(Vector2Int coor,HexDir dir)
  {
    var _coor=GetNextCoor(coor,dir,true);
    return TileDatas[_coor.x,_coor.y];
  }
  public TileData GetNextTile(TileData tile, HexDir dir)
  {
    var _coor = GetNextCoor(tile.Coordinate, dir,true);
    return TileDatas[_coor.x, _coor.y];
  }
  public List<TileData> GetAroundTile(TileData tile, int range)
  {
    List<Vector2Int> _coors=new List<Vector2Int>();
    try
    {
     _coors   = GetAroundCoor(tile.Coordinate, range);
    }
    catch(System.Exception e)
    {
      Debug.Log(tile+" "+ range);
    }
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
  public List<TileData> GetAroundTile(Vector2 coor, int range)
  {
    List<Vector2Int> _coors = GetAroundCoor(new Vector2Int((int)coor.x,(int)coor.y), range);
    List<TileData> _tiles = new List<TileData>();
    foreach (var _coor in _coors) _tiles.Add(TileDatas[_coor.x, _coor.y]);
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
  /// <summary>
  /// HexDir로 경로 방향 방식으로 모아둔 것들
  /// </summary>
  private Dictionary<int, List<List<HexDir>>> AroundCoors = new Dictionary<int, List<List<HexDir>>>();
  public List<Vector2Int> GetAroundCoor(Vector2Int coor, int range)
  {
    if (!AroundCoors.ContainsKey(range))
    {
      List<HexGrid> _rangegrids=new List<HexGrid>();
      for(int q=range*-1; q<range+1;q++)
        for(int r=range*-1;r<range+1;r++)
          for(int s=range*-1;s<range+1;s++)
          {
            if(q==0&&r==0&&s==0) continue;
            if (q + r + s != 0) continue;
            _rangegrids.Add(new HexGrid(q,r,s));
          }
      HexGrid _zero=new HexGrid(0,0,0);
      List<List<HexDir>> _dirs=new List<List<HexDir>>();
      foreach (var _hexgrid in _rangegrids)
        _dirs.Add(_hexgrid.GetRoute(_zero));
      AroundCoors.Add(range, _dirs);
    }

    Vector2Int _temp = coor;
    List<Vector2Int> _coors = new List<Vector2Int>() { coor };
    foreach (var _offset in AroundCoors[range])
    {
      _temp = coor; 

      foreach (var _dir in _offset)
        _temp = GetNextCoor(_temp, _dir, true);

      if (_temp.x < 0|| _temp.y < 0|| _temp.x >= ConstValues.MapSize|| _temp.y >= ConstValues.MapSize) continue;

      _coors.Add(_temp);
    }
    return _coors;
  }
  public List<Vector2Int> GetAroundCoor(List<Vector2Int> coors,int range)
  {
    if (!AroundCoors.ContainsKey(range))
    {
      List<HexGrid> _rangegrids = new List<HexGrid>();
      for (int q = range * -1; q < range + 1; q++)
        for (int r = range * -1; r < range + 1; r++)
          for (int s = range * -1; s < range + 1; s++)
          {
            if (q == 0 && r == 0 && s == 0) continue;
            if (q + r + s != 0) continue;
            _rangegrids.Add(new HexGrid(q, r, s));
          }
      HexGrid _zero = new HexGrid(0, 0, 0);
      List<List<HexDir>> _dirs = new List<List<HexDir>>();
      foreach (var _hexgrid in _rangegrids)
        _dirs.Add(_hexgrid.GetRoute(_zero));
      AroundCoors.Add(range, _dirs);
    }


    List<Vector2Int> _result = new List<Vector2Int>();
    Vector2Int _temp = Vector2Int.zero;

    foreach (var _coordinate in coors)
    {
      foreach (var _dirs in AroundCoors[range])
      {
        _temp = _coordinate;
        foreach (var _dir in _dirs) _temp = GetNextCoor(_temp, _dir, true);

        if (_temp.x < 0 || _temp.y < 0 || _temp.x >= ConstValues.MapSize || _temp.y >= ConstValues.MapSize) continue;

        if (!_result.Contains(_temp)) _result.Add(_temp);
      }
    }

    return _result;
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
      if (_lines.Count > 2 && 
        ((_next.BottomEnvir==BottomEnvirType.Sea)||
        _next.Coordinate.x==0||_next.Coordinate.y==0||_next.Coordinate.x==ConstValues.MapSize-1|| _next.Coordinate.y == ConstValues.MapSize - 1)) break;

      _lines.Add(_next);
    }

    return _lines;
  }
  public TileData CenterTile
  {
    get
    {
      return TileDatas[ConstValues.MapSize / 2 + ConstValues.MapSize % 2-1, ConstValues.MapSize / 2 + ConstValues.MapSize % 2-1];
    }
  }
  public List<Settlement> GetCloseSettles(Settlement _origin,int _count)
  {
    Vector2 _originpos = _origin.Position;

    List<float> _distance=new List<float>();
    List<Settlement> _settlement=new List<Settlement>();
    foreach (Settlement _settle in AllSettles)
    {
      if (_settle == _origin) continue;

      float _dis=Vector2.Distance(_settle.Position, _originpos);
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
    _data.Landmark = _centertile.Landmark;
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

    return _data;
  }//월드 기준 타일 1개 좌표 하나 받아서 그 주위 2칸의 산 바다, 나머지 1타일


  public List<TileData> EventTiles = new List<TileData>();
  public TileData[,] TileDatas;
  public List<Settlement> Villages = new List<Settlement>();
  public List<Settlement> Towns = new List<Settlement>();
  public List<Settlement> Citys = new List<Settlement>();
  public List<Settlement> AllSettles = new List<Settlement>();

}//게임 상에서 꺼내 쓰기 편리하게 변환한 지도 데이터
public static class SettlementName
{
  public static string[] VillageNames =
  {
    "alion","amberfort","arthurstone","ashbrook","brigelis","calibria","eldermist","elvendom","evergreenvale","galania","ironcliff","raia","rappland","sellier","silberia","sunridge","synodia","windmere"
  };
  public static string[] TownNames =
  {
    "albace","avalomia","ellsworth","emael","iberton","lumia","niverdale","solaria","sylveria","valtar"
  };
  public static string[] CityNames =
  {
    "aberstead","arcadimia","catrua","lugrea","mabrel","meridina","penbrite","solanum","tryon","udrium"
  };
}

