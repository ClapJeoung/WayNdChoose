using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEngine.UIElements;
using Newtonsoft.Json;
using Unity.VisualScripting;

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

  public List<HexDir> GetRoute(TileData starttile,TileData endtile)
  {
    if(endtile.HexGrid.GetDistance(starttile)==1) return endtile.HexGrid.GetDirectRoute(starttile);

    List<HexDir> _originhexdir = endtile.HexGrid.GetDirectRoute(starttile);
    List<TileData> _origintiles = new List<TileData>();//시작 빼고 end까지
    bool _isminimun = true;
    for(int i = 0; i < _originhexdir.Count; i++)
    {
      TileData _temptile = GetNextTile(_origintiles.Count == 0 ? starttile : _origintiles[_origintiles.Count - 1], _originhexdir[i]);
      _origintiles.Add(_temptile);
      if (i<_originhexdir.Count-1&& _temptile.RequireSupply > 1) _isminimun = false;
    }
    if (_isminimun) 
    { 
      //Debug.Log("이미 최소 경로인 레후~"); 
      return endtile.HexGrid.GetDirectRoute(starttile); 
    }

    int _hexdirdif = 0;
    int[] _altersum = new int[2];
    bool[] _altersuccess=new bool[2];
    int _currentsupply = 0;
    int _index = 0;
    TileData _starttile = null, _targettile = null;
    List<int> _skipindex=new List<int>();
    int _loopcount = 0;
    while (_index < _origintiles.Count-1)
    {
      _loopcount++;
      if (_loopcount > 1000) 
      {
        Debug.Log("테챠앗"); 
        return null; 
      }
      if (_skipindex.Contains(_index)) { _index++; continue; }
      _altersum[0] = 0;
      _altersum[1] = 0;
      _altersuccess[0] = true;
      _altersuccess[1] = true;
      _starttile = _index == 0 ? starttile : _origintiles[_index-1];
      TileData _middletile = _origintiles[_index];
      _targettile = _origintiles[_index + 1];

      if (_middletile.RequireSupply > 1 || !_middletile.Interactable || _middletile.Fogstate != 2)
      {
        if (!_middletile.Interactable || _middletile.Fogstate != 2)
        {
          _currentsupply = 0;
        }
        else
        {
          _currentsupply = _middletile.RequireSupply;
        }
        _hexdirdif = (int)_originhexdir[_index + 1] - (int)_originhexdir[_index];
        var _alterroute = getalterroute(_originhexdir[_index], _hexdirdif);
        for(int i=0; i < _alterroute.Length; i++)
        {
          TileData _temptile = null;
          for (int j = 0; j < _alterroute[i].Count-1; j++)
          {
            _temptile = GetNextTile(j==0?_starttile:_temptile, _alterroute[i][j]);
            if (!_temptile.Interactable   //targettile이 이동 불가하거나
              || _temptile.Fogstate != 2  //targettile 안개에 막혀있거나
              ||(_currentsupply>0&&_altersum[i] + _temptile.RequireSupply + _temptile.HexGrid.GetDistance(_targettile)-1 >= _currentsupply))
            {                             //기존 루트가 이동 가능하고, 기존 루트보다 비싸질 경우엔
              _altersuccess[i] = false;   //이 대체 루트 폐기
              break;
            }
           _altersum[i] += _temptile.RequireSupply;
          }
        }
        int _resultid = 0;
         if (_altersuccess[0] && !_altersuccess[1])  //0번 루트만 이동 가능
        {
          if (_currentsupply == 0) _resultid = 0;   //0번 루트로 교체
          else if (_altersum[0] < _currentsupply) _resultid = 0;//0번 루트로 교체
          else _resultid = 2;//실패
        }
        else if (!_altersuccess[0] && _altersuccess[1])//1번 루트만 이동 가능
        {
          if (_currentsupply == 0) _resultid = 1;   //1번 루트로 교체
          else if (_altersum[1] < _currentsupply) _resultid = 1;//1번 루트로 교체
          else _resultid = 2;//실패
        }
        else if (!_altersuccess[0] && !_altersuccess[1])//0번, 1번 둘 다 이동 불가능
        {
          _resultid = 2;//실패
        }
        else if (_altersuccess[0] && _altersuccess[1]) //0번, 1번 둘 다 성공
        {
          if (_altersum[0] < _altersum[1]) _resultid = 0;
          else _resultid = 1;
        }
        switch (_resultid)
        {
          case 0: //0번 루트 삽입
            _origintiles.RemoveAt(_index);
            _originhexdir.RemoveAt(_index + 1);
            _originhexdir.RemoveAt(_index);
            for(int i = 0; i < _alterroute[0].Count; i++)
            {
              if(i<_alterroute[0].Count-1) _origintiles.Insert(_index + i, GetNextTile(_index+i==0?starttile: _origintiles[_index-1 + i], _alterroute[0][i]));
              _originhexdir.Insert(_index + i, _alterroute[0][i]);
            }
            for (int i = 0; i < _skipindex.Count; i++) if(i>_index) _skipindex[i] += _alterroute[0].Count-1;
            _index = -1;
            break;
          case 1: //1번 루트 삽입
            _origintiles.RemoveAt(_index);
            _originhexdir.RemoveAt(_index+1);
            _originhexdir.RemoveAt(_index);
            for (int i = 0; i < _alterroute[1].Count; i++)
            {
              if (i < _alterroute[1].Count - 1) _origintiles.Insert(_index + i, GetNextTile(_index+i == 0 ? starttile : _origintiles[_index-1 + i], _alterroute[1][i]));
              _originhexdir.Insert(_index + i, _alterroute[1][i]);
            }
            for (int i = 0; i < _skipindex.Count; i++) if (i > _index) _skipindex[i] += _alterroute[1].Count - 1;
            _index = -1;
            break;
          case 2: //포기하고 실패 인덱스에 추가
            _skipindex.Add(_index);
            break;
        }

        if (_resultid != 2)
        {
          for (int i = 0; i < _originhexdir.Count - 1; i++)
          {
            if (rotatedir((int)_originhexdir[i], 2) == (int)_originhexdir[i+1])
            {
              if (_skipindex.Contains(i)) _skipindex.Remove(i);
              for(int j = 0; j < _skipindex.Count; j++) if (_skipindex[j] > i) _skipindex[j]--;

              _origintiles.RemoveAt(i);
              _originhexdir[i] = (HexDir)rotatedir((int)_originhexdir[i], 1);
              _originhexdir.RemoveAt(i + 1);

              i = -1;
              continue;
            }
            else if (rotatedir((int)_originhexdir[i], -2) == (int)_originhexdir[i + 1])
            {
              if (_skipindex.Contains(i)) _skipindex.Remove(i);
              for (int j = 0; j < _skipindex.Count; j++) if (_skipindex[j] > i) _skipindex[j]--;

              _origintiles.RemoveAt(i);
              _originhexdir[i] = (HexDir)rotatedir((int)_originhexdir[i], -1);
              _originhexdir.RemoveAt(i + 1);

              i = -1;
              continue;
            }
          }
        }
      }
      _index++;
    }
   // Debug.Log(_loopcount);
    return _originhexdir;

    List<HexDir>[] getalterroute(HexDir startdir, int dir)
    {
      List<HexDir>[] _modifieddirs = new List<HexDir>[2];
      switch (dir)
      {
        case -1:
        case 5:
          _modifieddirs[0]=new List<HexDir> { (HexDir)rotatedir((int)startdir,-1), (HexDir)rotatedir((int)startdir, 0) };
          _modifieddirs[1]=new List<HexDir> { 
            (HexDir)rotatedir((int)startdir, 1)
          ,(HexDir)rotatedir((int)startdir,0)
          ,(HexDir)rotatedir((int)startdir,-1)
          ,(HexDir)rotatedir((int)startdir,-2)};
          break;
        case 0:
          _modifieddirs[0]=new List<HexDir> { (HexDir)rotatedir((int)startdir, -1), (HexDir)rotatedir((int)startdir, 0), (HexDir)rotatedir((int)startdir, 1)};
          _modifieddirs[1]=new List<HexDir> { (HexDir)rotatedir((int)startdir, 1) , (HexDir)rotatedir((int)startdir, 0) , (HexDir)rotatedir((int)startdir, -1) };
          break;
        case 1:
        case -5:
          _modifieddirs[0]=new List<HexDir> {
          (HexDir)rotatedir((int)startdir,-1),
          (HexDir)rotatedir((int)startdir, 0),
          (HexDir)rotatedir((int)startdir, 1),
          (HexDir)rotatedir((int)startdir, 2)};
          _modifieddirs[1]=new List<HexDir> { (HexDir)rotatedir((int)startdir, 1) , (HexDir)rotatedir((int)startdir, 0) };
          break;
      }
      return _modifieddirs;
    }
    int rotatedir(int dir,int value) { return (dir + value) < 0 ? (dir + value + 6) : (dir + value) > 5 ? (dir + value - 6) : (dir + value); }
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
    if (_temp.x >= GameManager.Instance.Status.MapSize) _temp.x = GameManager.Instance.Status.MapSize - 1;
    if(_temp.y>=GameManager.Instance.Status.MapSize)_temp.y=GameManager.Instance.Status.MapSize - 1;
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
        _dirs.Add(_hexgrid.GetDirectRoute(_zero));
      AroundCoors.Add(range, _dirs);
    }

    Vector2Int _temp = coor;
    List<Vector2Int> _coors = new List<Vector2Int>() { coor };
    foreach (var _offset in AroundCoors[range])
    {
      _temp = coor; 

      foreach (var _dir in _offset)
        _temp = GetNextCoor(_temp, _dir, true);

      if (_temp.x < 0|| _temp.y < 0|| _temp.x >= GameManager.Instance.Status.MapSize|| _temp.y >= GameManager.Instance.Status.MapSize) continue;

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
        _dirs.Add(_hexgrid.GetDirectRoute(_zero));
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

        if (_temp.x < 0 || _temp.y < 0 || _temp.x >= GameManager.Instance.Status.MapSize || _temp.y >= GameManager.Instance.Status.MapSize) continue;

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
        _next.Coordinate.x==0||_next.Coordinate.y==0||_next.Coordinate.x==GameManager.Instance.Status.MapSize-1|| _next.Coordinate.y == GameManager.Instance.Status.MapSize - 1)) break;

      _lines.Add(_next);
    }

    return _lines;
  }
  public TileData CenterTile
  {
    get
    {
      return TileDatas[GameManager.Instance.Status.MapSize / 2 + GameManager.Instance.Status.MapSize % 2-1, GameManager.Instance.Status.MapSize / 2 + GameManager.Instance.Status.MapSize % 2-1];
    }
  }
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

