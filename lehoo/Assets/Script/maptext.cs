using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class maptext : MonoBehaviour
{
  [ContextMenu("거리 받기 테스트")]
  public void GetLength()
  {
    HexGrid Hexgrid = new HexGrid(AxisGrid);
    string _str=$"({AxisGrid.x},{AxisGrid.y}) -> ({Hexgrid.q},{Hexgrid.r},{Hexgrid.s}) -> ";
    foreach (var dir in Hexgrid.GetDir)
      _str += (int)dir + " ";
    print(_str);
  }
  public Vector2Int AxisGrid=new Vector2Int();


  [SerializeField] private UI_map MapUIScript = null;
  public Tilemap Tilemap_bottom, Tilemap_top;
   public TilePrefabs MyTiles;
  [Space(10)]
  [SerializeField] private Transform TileHolder_bottomenvir = null;
  [SerializeField] private Transform TileHolder_topenvir = null;
  [SerializeField] private Transform TileHolder_landmark = null;
  [SerializeField] private Sprite Villagesprite, Townsprite, Citysprite;
    private void Start()
    {
        Vector3Int _pos = new Vector3Int(5, 5,0);
        Matrix4x4 _trans = Matrix4x4.Translate(Vector3.zero) * Matrix4x4.Rotate(Quaternion.Euler(new Vector3(0, 0, -60.0f)));
        TileChangeData _data = new TileChangeData
        {
            position = _pos,
 //           tile = TileSprite.GetRiver(0,1,ref _asdf),
            color = Color.white,
            transform = _trans
        };

        //  StartCoroutine(_simul());
        //  StartCoroutine(_simul_fatal());
     //   StartCoroutine(makeperfectmap());
    }
  public List<T> MixRandom<T>(List<T> target)
  {
    List<T> _rnd = new List<T>();
    while (_rnd.Count != target.Count)
    {
      T _temp = target[Random.Range(0, target.Count)];
      if (_rnd.Contains(_temp)) continue;
      _rnd.Add(_temp);
    }
    return _rnd;
  }

  public void MakePerfectMap()
  {
    StartCoroutine(makeperfectmap());
  }
  private IEnumerator makeperfectmap()
  {
    int _index = 0;
    while (true)
    {
      _index++;
      MapData _map = CreateMap();
      if (_map == null) continue;
      bool _villageriver = false, _villageforest = false, _villagemountain = false, _villagesea = false;
      bool _townriver = false, _townforest = false,  _townmountain = false, _townsea = false;
      bool _cityriver = false, _cityforest = false, _citymountain = false, _citysea = false;

      foreach (var _village in _map.Villages)
      {
        if (_village.IsRiver) _villageriver = true;
        if (_village.IsForest) _villageforest = true;
    //    if (_village.IsHighland) _villagehighland = true;
        if (_village.IsMountain) _villagemountain = true;
        if (_village.IsSea) _villagesea = true;
      }
      var _town = _map.Town;
      
        if (_town.IsRiver) _townriver = true;
        if (_town.IsForest) _townforest = true;
       // if (_town.IsHighland) _townhighland = true;
        if (_town.IsMountain) _townmountain = true;
        if (_town.IsSea) _townsea = true;
      
      var _city = _map.City;
      if (_city.IsRiver) _cityriver = true;
      if (_city.IsForest) _cityforest = true;
    //  if (_city.IsHighland) _cityhighland = true;
      if (_city.IsMountain) _citymountain = true;
      if (_city.IsSea) _citysea = true;

      if (!_villageforest && !_townforest && !_cityforest) { Debug.Log(_index+"번 맵 숲 하나도 없음"); continue; }
      if (!_villagesea && !_townsea && !_citysea) { Debug.Log(_index + "번 맵 바다 하나도 없음"); continue; }
      if (!_villagemountain && !_townmountain && !_citymountain) { Debug.Log(_index + "번 맵 산 하나도 없음"); continue; }
      if (!_villageriver && !_townriver && !_cityriver) { Debug.Log(_index + "번 맵 강 하나도 없음"); continue; }
      if (_map.Villages.Count != 2|| _map.Town == null|| _map.Town == null) { yield return null; continue; }

      Debug.Log($"{_index}번째 맵 성공\n");
      GameManager.Instance.MyGameData.MyMapData = _map;
      break;
    }
  }
  /// <summary>
  /// range 범위만큼의 타일 개수(range 최소 0)
  /// </summary>
  /// <param name="range"></param>
  /// <returns></returns>
  public MapData CreateMap()
  {
    int LoopCount = 0;

    MapData _NewMapData = new MapData();
    _NewMapData.TileDatas = new TileData[ConstValues.MapSize, ConstValues.MapSize];
    for (int i = 0; i < ConstValues.MapSize; i++)
      for (int j = 0; j < ConstValues.MapSize; j++)
      {
        _NewMapData.TileDatas[j, i] = new TileData();
        _NewMapData.TileDatas[j, i].Coordinate = new Vector2Int(j, i);
        _NewMapData.TileDatas[j, i].BottomEnvir = BottomEnvirType.Sea;
      }

    #region 땅 만들기
    //지도의 중심
    List<Vector2Int> _firstlandcoors = _NewMapData.GetAroundCoor(new List<Vector2Int> { _NewMapData.CenterTile.Coordinate }, ConstValues.LandRadius);
    foreach (var _landcoor in _firstlandcoors)
    {
      _NewMapData.Tile(_landcoor).BottomEnvir = BottomEnvirType.Land;
      _NewMapData.Tile(_landcoor).BottomEnvirSprite = TileSpriteType.Land;
    }
    #endregion

    #region 바다,해안선

    List<TileData> _seatiles=_NewMapData.GetEnvirTiles(new List<BottomEnvirType> { BottomEnvirType.Sea});
    List<TileData> _outerbeachtiles = new List<TileData>();
    
    foreach (var _sea in _seatiles)
    {
      List<TileData> _around_1 = _NewMapData.GetAroundTile(_sea, 1);
      List<TileData> _around_2 = _NewMapData.GetAroundTile(_sea,2);
      _around_1.Remove(_sea);
      _around_2.Remove(_sea);

      foreach (var _targettile in _around_1)
        if (_targettile.BottomEnvir == BottomEnvirType.Land)
        {
          if (Random.Range(0, 10) < 7)
          {
            _outerbeachtiles.Add(_sea);
          }
          break;
        }
      foreach(var _targettile in _around_2)
        if (_targettile.BottomEnvir == BottomEnvirType.Land)
        {
          if (Random.Range(0, 10) < 4)
          {
            _outerbeachtiles.Add(_sea);
          }
          break;
        }
    }
    //주위에 육지랑 맞닿아 있는 바다 타일들 가져오기
    foreach (var _tile in _outerbeachtiles)
    {
      _NewMapData.Tile(_tile.Coordinate).BottomEnvir = BottomEnvirType.Land;
      _NewMapData.Tile(_tile.Coordinate).BottomEnvirSprite = TileSpriteType.Land;
    }

    List<TileData> _poundtiles=new List<TileData>();
    List<TileData> _disablebeaches=new List<TileData>();
    while (true)
    {
      _disablebeaches.Clear();
      _poundtiles.Clear();

      for (int i = 0; i < _outerbeachtiles.Count; i++)
      {
        List<TileData> _around = _NewMapData.GetAroundTile(_outerbeachtiles[i], 1);
        _around.Remove(_outerbeachtiles[i]);

        int _seacount = 0;
        foreach (var _tile in _around) if (_tile.BottomEnvir == BottomEnvirType.Sea) _seacount++;
        if (_seacount > 4) _disablebeaches.Add(_outerbeachtiles[i]);
      }
      foreach (var _tile in _disablebeaches)
      {
        _NewMapData.Tile(_tile.Coordinate).BottomEnvir = BottomEnvirType.Sea;
        _NewMapData.Tile(_tile.Coordinate).BottomEnvirSprite = TileSpriteType.Sea;
        _outerbeachtiles.Remove(_tile);
      }

      List<TileData> _currentseas = _NewMapData.GetEnvirTiles(new List<BottomEnvirType> { BottomEnvirType.Sea });
      foreach(var _sea in _currentseas)
      {
        List<TileData> _around = _NewMapData.GetAroundTile(_sea, 1);
        _around.Remove(_sea);

        if (_around.Count < 6) continue;

        int _landcount = 0;
        foreach(var _tile in _around)if(_tile.BottomEnvir == BottomEnvirType.Land) _landcount++;

        if (_landcount ==6) _poundtiles.Add(_sea);
      }
      foreach(var _tile in _poundtiles)
      {
        _NewMapData.Tile(_tile.Coordinate).BottomEnvir = BottomEnvirType.Land;
        _NewMapData.Tile(_tile.Coordinate).BottomEnvirSprite= TileSpriteType.Land;
      }

      if (_disablebeaches.Count == 0&&_poundtiles.Count==0) break;

    }

        _seatiles = _NewMapData.GetEnvirTiles(new List<BottomEnvirType> { BottomEnvirType.Sea });
        List<TileData> _pureseatiles = new List<TileData>();
        List<TileData> _detectcentertiles = new List<TileData>();
        List<TileData> _detecttargettiles = new List<TileData>();
        _pureseatiles.Add(_NewMapData.Tile(Vector2Int.zero));
        _detectcentertiles.Add(_NewMapData.Tile(Vector2Int.zero));

        while (_detectcentertiles.Count > 0)
        {
            _detecttargettiles.Clear();

            foreach(var _center in _detectcentertiles)
            {
                for(int i = 0; i < 6; i++)
                {
                    TileData _target = _NewMapData.GetNextTile(_center, (HexDir)i);

                    if (_target.BottomEnvir != BottomEnvirType.Sea) continue;
                    if (_pureseatiles.Contains(_target) || _detectcentertiles.Contains(_target) || _detecttargettiles.Contains(_target)) continue;

                    _detecttargettiles.Add(_target);
                }
            }

            _detectcentertiles.Clear();
            foreach (var _newsea in _detecttargettiles)
            {
                _pureseatiles.Add(_newsea);
                _detectcentertiles.Add(_newsea);
            }
        }
        foreach(var seatile in _seatiles)
        {
            if (_pureseatiles.Contains(seatile)) continue;
            _NewMapData.TileDatas[seatile.Coordinate.x, seatile.Coordinate.y].BottomEnvir = BottomEnvirType.Land;
            _NewMapData.TileDatas[seatile.Coordinate.x, seatile.Coordinate.y].BottomEnvirSprite = TileSpriteType.Land;
        }

    for(int i=0;i<ConstValues.MapSize; i++)
      for(int j=0;j<ConstValues.MapSize;j++)
      {
        if (_NewMapData.Tile(new Vector2Int(j, i)).BottomEnvir == BottomEnvirType.Sea) continue;

        List<TileData> _around = _NewMapData.GetAroundTile(new Vector2Int(j,i), 1);
        _around.Remove(_NewMapData.Tile(new Vector2Int(j, i)));

        int _seacount = 0;
        foreach (var _tile in _around) if (_tile.BottomEnvir == BottomEnvirType.Sea) _seacount++;
        if (_seacount > 0)
        {
          _NewMapData.TileDatas[j, i].BottomEnvir = BottomEnvirType.Beach;
        }
      }

    TileData _centersea = _NewMapData.CenterTile;
    _centersea.BottomEnvir = BottomEnvirType.Sea;
    _centersea.BottomEnvirSprite = TileSpriteType.Sea;

    List<TileData> _beaches = _NewMapData.GetEnvirTiles(new List<BottomEnvirType> { BottomEnvirType.Beach });
    for(int i = 0; i < _beaches.Count; i++)
    {
      List<int> _seadirs=new List<int>();
      for(int j = 0; j < 6; j++)
      {
        if(_NewMapData.GetNextTile(_beaches[i],(HexDir)j).BottomEnvir==BottomEnvirType.Sea)_seadirs.Add(j);
      }

      if (_seadirs.Count == 2 && RotateDir(_seadirs[0], 3) == _seadirs[1])
      {
        _beaches[i].BottomEnvirSprite = TileSpriteType.Beach_middle;
        for(int j = 0; j < 6; j++)
        {
          if((_seadirs[0]==0&&_seadirs[1]==3)|| (_seadirs[0] == 3 && _seadirs[1] == 0))
          {
            _beaches[i].Rotation = j;
          }

          _seadirs[0] = RotateDir(_seadirs[0], -1);
          _seadirs[1]=RotateDir(_seadirs[1], -1);
        }
      }//바다 개수가 2개고, 서로 마주 보는 형태일 경우 beach_n이 아닌 beach_middle로 할당
      else
      {
        for (int j = 0; j < 6; j++)
        {
          if (MaxDirIndex() == _seadirs.Count - 1)
          {
            _beaches[i].BottomEnvirSprite = MyTiles.GetBeachTile(_seadirs.Count);
            _beaches[i].Rotation = j;
            break;
          }
          for (int k = 0; k < _seadirs.Count; k++)
            _seadirs[k] = RotateDir(_seadirs[k], -1);
        }
      }

      int MaxDirIndex()
      {
        int _max = 0;
        foreach (var _dir in _seadirs) if (_max < _dir) _max = _dir;
        return _max;
      }
    }
    #endregion
    //     Debug.Log("바다 생성 완료");

    #region 강
    LoopCount = 0;

    RiverData[] _riverdatas = new RiverData[6];
    int _riverdiradd = UnityEngine.Random.Range(0, 1);
    for (int i = 0; i < _riverdatas.Length; i++)
    {
      bool _iscomplete = false;

      RiverData _riverdata = new RiverData();
      Vector2Int _sourcecoor = _NewMapData.GetNextCoor(_NewMapData.CenterTile, (HexDir)i);
      _riverdata.RiverIndex = i;
      _riverdata.RiverCoors.Add(_sourcecoor);
      _riverdata.RiverDirs.Add(i);
      _riverdata.RiverCoors.Add(_NewMapData.GetNextCoor(_sourcecoor, (HexDir)i));
      int _maindir = i;
      int _curvecount = 0;
      List<Vector2Int> _failcoors = new List<Vector2Int>();

      //  Debug.Log($"{i + 1}번째 강 발원지 {_sourcepos}에서 {_maindir}방향으로 시작");
      while (true)
      {
        LoopCount++;
        if (LoopCount > 1000) { Debug.Log("강 생성 중 무한루프"); return null; }

        if (_riverdata.RiverDirs.Count == 0) { i--; break; }

        int _finishdir = 10;                                            //(0~5)
        List<int> _targetdirs = MixRandom(new List<int> { -1, 0, 1 });  //-1,0,1이 무작위로
        List<int> _enabledirs = new List<int>();                          //확보한 방향들(-1,0,1)
        Vector2Int _currentcoor = _riverdata.RiverCoors[_riverdata.RiverCoors.Count - 1];  //나아갈 타일 중심지
        for (int k = 0; k < _targetdirs.Count; k++)
        {

          int _nextdir = RotateDir(_maindir, _targetdirs[k]);                //현재 선택된 방향(0~5)
          TileData _nexttile = _NewMapData.GetNextTile(_currentcoor, (HexDir)_nextdir);

          if (_nexttile.Coordinate.x == 0 || _nexttile.Coordinate.y == 0 || _nexttile.Coordinate.x == ConstValues.MapSize - 1 || _nexttile.Coordinate.y == ConstValues.MapSize - 1)
          {
            _enabledirs.Add(_targetdirs[k]);
            _finishdir = _nextdir;
          }
          if (_curvecount == -3 && _targetdirs[k] == -1) continue;
          if (_curvecount == 3 && _targetdirs[k] == 1) continue;//과도한 커브 금지
          if (_failcoors.Contains(_nexttile.Coordinate)) continue;
          bool _overlaped = false;
          for (int m = 0; m < i; m++) //이전 강 데이터들 중 이미 목표 타일을 가지고 있는 강은 없는지 확인
          {
            try
            {
              if (_riverdatas[m].RiverCoors.Contains(_nexttile.Coordinate))
              {
                _overlaped = true;
                break;
              }
            }
            catch (System.Exception e)
            {
              Debug.Log(e);
              throw;
            }
          }
          if (_overlaped) continue;
          _enabledirs.Add(_targetdirs[k]);
          if (_nexttile.BottomEnvir == BottomEnvirType.Beach) _finishdir = _nextdir;
        }

        if (_enabledirs.Count == 0)
        {
          //   Debug.Log($"{i + 1}번째 강 [{_riverdata.RiverCoors.Count+1}]{_currentcoor} 제명, {_riverdata.RiverCoors.Count}번으로 회귀");
          _failcoors.Add(_currentcoor);
          _riverdata.RiverCoors.Remove(_currentcoor);
          _riverdata.RiverDirs.Remove(_riverdata.RiverDirs[_riverdata.RiverDirs.Count - 1]);
          continue;
        }//enabledir 개수가 0일 경우 여긴 진행 불가 타일이므로 좌표,방향을 1개씩 제거 (현재 타일 failcoor)

        if (_finishdir != 10)
        {
          if (_riverdata.RiverCoors.Count < ConstValues.MinRiverCount)
          {
            //   Debug.Log($"{i + 1}번째 강 [{_riverdata.RiverCoors.Count + 1}]{_currentcoor}->({_finishdir})->[{_riverdata.RiverCoors.Count + 2}]{_NewMapData.GetNextCoor(_currentcoor, (HexDir)_finishdir)} 기준 미충족 해변으로 제명 {_riverdata.RiverCoors.Count+1}번 타일 다시 검사");
            _failcoors.Add(_NewMapData.GetNextCoor(_currentcoor, (HexDir)_finishdir));
            continue;
          }//해변을 찾았는데 강 개수를 채우지 못한 경우(해당 타일 failcoor)
          else
          {
            //   Debug.Log($"{i + 1}번째 강 [{_riverdata.RiverCoors.Count + 1}]{_currentcoor} ->({_finishdir})-> [{_riverdata.RiverCoors.Count + 2}]{_NewMapData.GetNextCoor(_currentcoor, (HexDir)_finishdir)} 강 종료");
            _riverdata.RiverCoors.Add(_NewMapData.GetNextCoor(_currentcoor, (HexDir)_finishdir));
            _riverdata.RiverDirs.Add(_finishdir);
            _iscomplete = true;
            break;
          }//해변을 찾았고 강 최소 개수도 만족()
        }

        int _selectdir = 0;
        switch (_enabledirs[Random.Range(0, _enabledirs.Count - 1)])
        {
          case -1:
            _selectdir = RotateDir(_maindir, -1);
            _curvecount--;
            break;
          case 0:
            _selectdir = RotateDir(_maindir, 0);
            break;
          case 1:
            _selectdir = RotateDir(_maindir, 1);
            _curvecount++;
            break;
        }//enabledir 개수가 1 이상일 경우 무작위 1택
        Vector2Int _nextcoor = _NewMapData.GetNextCoor(_currentcoor, (HexDir)_selectdir);

        //  Debug.Log($"{i + 1}번째 강 [{_riverdata.RiverCoors.Count + 1}]{_currentcoor} ->({_selectdir})-> [{_riverdata.RiverCoors.Count + 2}]{_nextcoor} 성공");
        _riverdata.RiverCoors.Add(_nextcoor);
        _riverdata.RiverDirs.Add(_selectdir);
      }

      if (_iscomplete) { _riverdatas[i] = _riverdata; }
    }

    for (int i = 0; i < _riverdatas.Length; i++)
    {
      for (int j = 0; j < _riverdatas[i].RiverCoors.Count; j++)
      {
        if (j == 0 || j == _riverdatas[i].RiverCoors.Count)
        {
          _NewMapData.Tile(_riverdatas[i].RiverCoors[j]).BottomEnvir = BottomEnvirType.RiverBeach;
          _NewMapData.Tile(_riverdatas[i].RiverCoors[j]).BottomEnvirSprite = MyTiles.GetRiverBeach(1, 3);
          _NewMapData.Tile(_riverdatas[i].RiverCoors[j]).Rotation = RotateDir(i, 3);

        }//발원지
        else if (j == _riverdatas[i].RiverCoors.Count - 1)
        {
          TileData _riverbeach = _NewMapData.Tile(_riverdatas[i].RiverCoors[j]);
          List<int> _seadirs = new List<int>();
          for (int k = 0; k < 6; k++)
          {
            if (_NewMapData.GetNextTile(_riverbeach, (HexDir)k).BottomEnvir == BottomEnvirType.Sea) _seadirs.Add(k);
          }

          int _lastdir = RotateDir(_riverdatas[i].RiverDirs[_riverdatas[i].RiverDirs.Count - 1], 3);
          for (int k = 0; k < 6; k++)
          {
            if (MaxDirIndex() == _seadirs.Count)
            {
              _riverbeach.BottomEnvir = BottomEnvirType.RiverBeach;
              _riverbeach.BottomEnvirSprite = MyTiles.GetRiverBeach(_seadirs.Count, RotateDir(_lastdir, -k) - 1);
              _riverbeach.Rotation = k + 1;
              break;
            }
            for (int l = 0; l < _seadirs.Count; l++)
              _seadirs[l] = RotateDir(_seadirs[l], -1);
          }

          int MaxDirIndex()
          {
            int _max = 0;
            foreach (var _dir in _seadirs) if (_max < _dir) _max = _dir;
            return _max;
          }

        }//해변
        else
        {
          TileData _rivertile = _NewMapData.Tile(_riverdatas[i].RiverCoors[j]);
          int[] _combineddirs = new int[2] { RotateDir(_riverdatas[i].RiverDirs[j - 1], 3), _riverdatas[i].RiverDirs[j] };
          for (int k = 0; k < 6; k++)
          {
            int _min = _combineddirs[0] < _combineddirs[1] ? _combineddirs[0] : _combineddirs[1];
            int _max = _combineddirs[0] < _combineddirs[1] ? _combineddirs[1] : _combineddirs[0];

            if (_min == 0 && _max <= 3)
            {
              _rivertile.BottomEnvir = BottomEnvirType.River;
              _rivertile.Rotation = k;
              _rivertile.BottomEnvirSprite = MyTiles.GetRiver(_max);
            }

            _combineddirs[0] = RotateDir(_combineddirs[0], -1);
            _combineddirs[1] = RotateDir(_combineddirs[1], -1);
          }
        }//줄기
      }
    }
    string _str = "";
    foreach (var _river in _riverdatas)
    {
      for (int i = 0; i < _river.RiverCoors.Count; i++)
      {
        if (i == _river.RiverCoors.Count - 1)
        {
          _str += $"{_river.RiverCoors[i]}";
        }
        else
        {
          _str += $"{_river.RiverCoors[i]}->[{_river.RiverDirs[i]}]->";
        }
      }
      _str += "\n";
    }
    //  Debug.Log(_str);
    #endregion
    //    Debug.Log("강 생성 완료");

    #region 사막
    /*
      Vector3Int _desertorigin = Vector3Int.zero;                //사막 시작점
      List<int> _lessdir = new List<int>(); //순위 낮은 방향
      if (_seacount == 2)                                        //바다 2면이라면 구석탱이에 배정
      {
          if (_seaside[1] == true) { _desertorigin.x = 0; _lessdir.Add(1); }
          else if (_seaside[3] == true) { _desertorigin.x = DefaultSize - 1; _lessdir.Add(3); }
          if (_seaside[0] == true) { _desertorigin.y = 0; _lessdir.Add(0); }
          else if (_seaside[2] == true) { _desertorigin.y = DefaultSize - 1; _lessdir.Add(2); }
      }
      else                                                       //바다 3면이라면 적당한 한가운데 끝자락에 생성
          for (int i = 0; i < 4; i++)
          {
              if (_seaside[i] == true) continue;
              if (i == 0) { _lessdir.Add(2); _desertorigin = new Vector3Int(6, 12, 0); break; }
              else if (i == 1) { _lessdir.Add(3); _desertorigin = new Vector3Int(12, 6, 0); break; }
              else if (i == 2) { _lessdir.Add(0); _desertorigin = new Vector3Int(6, 0, 0); break; }
              else if (i == 3) { _lessdir.Add(1); _desertorigin = new Vector3Int(0, 6, 0); break; }
          }

      MapData_b[_desertorigin.x, _desertorigin.y] = MapCode.b_desert;           //사막 시작점에 사막 타일 배치

      int _desertcount_max = Mathf.CeilToInt((float)DefaultSize * (float)DefaultSize * Ratio_desert);   //사막 최대 개수(정수)
      List<Vector3Int> _desertpos = new List<Vector3Int>();       //전체 사막 위치
      List<Vector3Int> _newdesertpos = new List<Vector3Int>();    //최근 새로 생긴 사막 위치
      _desertpos.Add(_desertorigin); _newdesertpos.Add(_desertorigin); //현재 사막 리스트, 최근 사막 리스트에 사막 원점 입력
      while (true)    //중단 조건은 내부에
      {
          List<Vector3Int> _newdesert_temp = new List<Vector3Int>();   //추가되기 직전 사막 목록
          List<int> _dirpool = new List<int>();
          for (int i = 0; i < 6; i++)
          {
              _dirpool.Add(i);
              _dirpool.Add(i);
              _dirpool.Add(i);
              if (_lessdir.Contains(i)) continue;
              _dirpool.Add(i);
          }
          for (int i = 0; i < _newdesertpos.Count; i++)   //새로 생긴 사막 하나씩 확장 시작
          {
              int[] _dir = new int[3] { -1, -1, -1 }; //무작위 방향 저장할 변수
              int _index = 0, _temp = 0;
              while (_index < 2)                        //새로 생긴 사막에서 나아갈 방향 3개 저장
              {
                  _temp = _dirpool[Random.Range(0, _dirpool.Count)];
                  if (_dir.Contains(_temp)) continue;     //겹치는 방향이면 다시
                  _dir[_index] = _temp;
                  _index++;
              }                                       //무작위 방향 3개 완료
              if (_desertpos.Count == 1)//첫 사막 방향은 고정
              {
                  if (_seacount == 2)
                  {
                      if (_seaside[0] == true)
                      {
                          if (_seaside[1] == true)    //위, 오른쪽
                          {
                              _dir[0] = 0; _dir[1] = 1; _dir[2] = 3;
                          }
                          else if (_seaside[3] == true)//위, 왼쪽
                          {
                              _dir[0] = 0; _dir[1] = 4; _dir[2] = 5;
                          }
                      }
                      else if (_seaside[2] == true)
                      {
                          if (_seaside[1] == true)    //아래, 오른쪽
                          {
                              _dir[0] = 0; _dir[1] = 1; _dir[2] = 2;
                          }
                          else if (_seaside[3] == true)//아래, 왼쪽
                          {
                              _dir[0] = 2; _dir[1] = 3; _dir[2] = 4;
                          }
                      }
                  }
                  else                                                       //바다 3면이라면 적당한 한가운데 끝자락에 생성
                  {
                      if (_seaside[0] == false) { _dir[0] = 4; _dir[1] = 2; _dir[2] = 3; }
                      else if (_seaside[1] == false) { _dir[0] = 3; _dir[1] = 4; _dir[2] = 5; }
                      else if (_seaside[2] == false) { _dir[0] = 0; _dir[1] = 4; _dir[2] = 5; }
                      else if (_seaside[3] == false) { _dir[0] = 0; _dir[1] = 1; _dir[2] = 2; }
                  }

              }
              for (int j = 0; j < _dir.Length; j++)    //해당 방향 3개에 사막 3개 배정(막히면 안함)
              {
                  Vector3Int _newpos = GetNextPos(_newdesertpos[i], _dir[j]); //현재 씨앗 타일에서 1칸 나아간 좌표
                  if (!CheckInside(_newpos)) continue;                 //범위 벗어나면 취소
                  if (MapData_b[_newpos.x, _newpos.y] == MapCode.b_sea) continue; //그 좌표에 바다가 있거나
                  if (_desertpos.Contains(_newpos)) continue;         //그 좌표에 사막이 있거나
                  if (_newdesert_temp.Contains(_newpos)) continue;    //그 좌표에 사막(예정)이 있으면 취소
                  _newdesert_temp.Add(_newpos);                       //뭐 걸리는거 없으면 사막(예정)에 추가
                                                                      //이후 방향 개수만큼 반복
              }
              //사막 타일 하나로부터 뻗어나가는 사막(최대 3개) 입력 완료
          }
          //    Debug.Log($"{_desertpos.Count + _newdesert_temp.Count}     {_desertcount_max}");
          if (_desertpos.Count + _newdesert_temp.Count >= _desertcount_max)
          {
              break;
          }
          //현재 사막 + 예정 사막 개수가 최대 사막 개수 초과하면 종료
          if (_newdesert_temp.Count == 0) break;
          //신규 예정 사막이 0개라면 종료

          _newdesertpos.Clear();  //신규 사막 리스트 초기화
          for (int i = 0; i < _newdesert_temp.Count; i++)
          {
              _desertpos.Add(_newdesert_temp[i]); _newdesertpos.Add(_newdesert_temp[i]);
          }
          //허용 범위 내부라면 사막 예정 좌표를 사막 확정 리스트에 대입
          //신규 사막 리스트도 새로운 데이터로 물갈이
      }

      for(int i = 0; i < _desertpos.Count; i++)
      {
          int _code = MapData_b[_desertpos[i].x, _desertpos[i].y];
          if (_code == MapCode.b_beach) MapData_b[_desertpos[i].x, _desertpos[i].y] = MapCode.b_desert_beach;
          else MapData_b[_desertpos[i].x, _desertpos[i].y] = MapCode.b_desert;
      }//해당 위치 사막/사막 해변으로 변환
    */
    #endregion
    //   Debug.Log("사막 생성 완료");  
    #region 산
    LoopCount = 0;

    List<Vector2Int> _newmountain = new List<Vector2Int>(); //산 위치 리스트
    int[] _mountaindirs = Random.Range(0, 2) == 0 ? new int[3] { 1, 3, 5 } : new int[3] { 0, 2, 4 };

    while (_newmountain.Count != 9)
    {
      LoopCount++;
      if (LoopCount > 1000) { Debug.Log("산 생성 중 무한루프 "+_newmountain.Count); return null; }

      int _index = _newmountain.Count / 3;
      List<TileData> _lines= _NewMapData.GetDirLines(_NewMapData.CenterTile, (HexDir)_mountaindirs[_index]);
      int _mintileindex = 1, _maxtileindex = _lines.Count - 3;
      Vector2Int _mountain_0=Vector2Int.zero,_mountain_1 = Vector2Int.zero, _mountain_2=Vector2Int.zero;
      int _mountaindir_1 = 0, _mountaindir_2 = 0;
      while (true)
      {
        LoopCount++;
        if (LoopCount > 1000) { Debug.Log("산 생성 중 무한루프");return null; }

        _mountain_0 = _lines[Random.Range(_mintileindex, _maxtileindex)].Coordinate
          +new Vector2Int(Random.Range(-1,2), Random.Range(-1, 2));
        _mountaindir_1 = Random.Range(0, 6);
        _mountaindir_2 = RotateDir(_mountaindir_1, Random.Range(-1, 2));
        _mountain_1 = _NewMapData.GetNextTile(_mountain_0, (HexDir)_mountaindir_1).Coordinate;
        _mountain_2 = _NewMapData.GetNextTile(_mountain_1, (HexDir)_mountaindir_2).Coordinate;

        List<TileData> _mountaintemps =_NewMapData.GetAroundTile(new List<TileData> { _NewMapData.Tile(_mountain_0), _NewMapData.Tile(_mountain_1), _NewMapData.Tile(_mountain_2) },1) ;

        bool _enable = true;
        foreach(var _tile in _mountaintemps)
        {
          if (_newmountain.Contains(_tile.Coordinate)) { _enable = false; break; }
          if(_tile.BottomEnvir==BottomEnvirType.Sea) { _enable = false; break; }
        }
        if (!_enable) continue;

        break;
      }

      _newmountain.Add(_mountain_0);
      _newmountain.Add(_mountain_1);
      _newmountain.Add(_mountain_2);

    }
    foreach (var _coor in _newmountain)
    {
      _NewMapData.Tile(_coor).TopEnvir = TopEnvirType.Mountain;
      _NewMapData.Tile(_coor).TopEnvirSprite = TileSpriteType.Mountain;
    }
    #endregion

    //     Debug.Log("산 생성 완료");

    #region 고원
    /*
    LoopCount = 0;

    int _highlandmaxcount = Mathf.CeilToInt((float)ConstValues.LandSize * (float)ConstValues.LandSize * ConstValues.Ratio_highland);//고원 최대 개수
    int _maxaver = _highlandmaxcount / 4;        //고원 줄기 평균 최대 개수  +-2 보정할것
    List<Vector2Int> _highlands = new List<Vector2Int>();   //고원 목록(사막 고원으로 변환 전)
    while (_highlands.Count < _highlandmaxcount)
    {
      LoopCount++;
      if (LoopCount > 1000) { Debug.Log("고원 생성 중 무한루프"); return null; }

      List<Vector2Int> _currenthigh = new List<Vector2Int>(); //현재 고원 좌표 리스트
      int _currentmax = _maxaver + Random.Range(-2, 3);       //현재 고원 줄기 최대 길이
      if (_highlandmaxcount - _highlands.Count < _currentmax) _currentmax = _highlandmaxcount - _highlands.Count;
      //이번 최대값까지 합친 값이 범위에 벗어나면 컷

      List<int> _dirlist = new List<int>();                   //현재 고원 방향 리스트
                                                              //4  3  3  2  2  2
      int _dir = Random.Range(0, 6);
      for (int i = 0; i < 6; i++) { _dirlist.Add(i); _dirlist.Add(i); }
      switch (_dir)
      {
        case 0: _dirlist.Add(0); _dirlist.Add(5); _dirlist.Add(1); break;
        case 1: _dirlist.Add(1); _dirlist.Add(0); _dirlist.Add(2); break;
        case 2: _dirlist.Add(2); _dirlist.Add(1); _dirlist.Add(3); break;
        case 3: _dirlist.Add(3); _dirlist.Add(2); _dirlist.Add(4); break;
        case 4: _dirlist.Add(4); _dirlist.Add(3); _dirlist.Add(5); break;
        case 5: _dirlist.Add(5); _dirlist.Add(4); _dirlist.Add(0); break;
      }
      _dirlist.Add(_dir);                                     //방향 선택 배율 조정 완료

      List<int> _faildir = new List<int>();//한 칸에서 실패한 방향(6번 실패하면 줄기 끝)
      Vector2Int _currentpos = new Vector2Int(Random.Range(0, ConstValues.MapSize), Random.Range(0, ConstValues.MapSize));   //현재 고원 위치
      Vector2Int _nextpos = new Vector2Int();                 //나아갈 고원 위치
      while (true)
      {
        LoopCount++;
        if (LoopCount > 1000) { Debug.Log("고원 생성 중 무한루프"); return null; }

        if (_faildir.Count == 6) break;                 //한 타일에서 6방향 전부 실패하거나
        if (_currenthigh.Count >= _currentmax) break;   //목표 개수 달성하거나

        int _currentdir = _dirlist[Random.Range(0, _dirlist.Count)];//무작위 방향 선택
        _nextpos = GetNextPos(_currentpos, _currentdir);            //타일 나아가기
        if (!highcheck(_nextpos))
        {
          if (!_faildir.Contains(_currentdir)) _faildir.Add(_currentdir); //실패 방향 리스트에 넣기(없으면)
          continue;
        }                                //맵 안에 있는지, 물이나 고원이랑 겹치는지

        if (_faildir.Count > 0) _faildir.Clear();                      //성공하면 실패 카운트 초기화

        _currenthigh.Add(_nextpos);                                 //현재 고원 리스트에 추가
        _currentpos = _nextpos;                                     //현재 좌표 다음 좌표로 변경

        Vector2Int _aroundpos = new Vector2Int();
        for (int i = 0; i < 6; i++)
        {
          _aroundpos = GetNextPos(_currentpos, i);
          if (_NewMapData.TileDatas[_aroundpos.x, _aroundpos.y].TopEnvir==TopEnvirType.Mountain) break;
          //주변 1칸에 산이 인접해 있으면 바로 종료
        }//주변 1칸 산 검사
      }//고원 줄기 뻗어나가기

      foreach (Vector2Int _pos in _currenthigh)
      {
        _highlands.Add(_pos);                                       //고원 전체 리스트에 추가
        _NewMapData.TileDatas[_pos.x,_pos.y].TopEnvir=TopEnvirType.Highland;
        _NewMapData.Tile(_pos).TopEnvirSprite = TileSpriteType.HighLand;
      }//타일 값에 입력

      bool highcheck(Vector2Int _pos)
      {
        TileData _tile = _NewMapData.TileDatas[_pos.x, _pos.y];
        if (_tile.BottomEnvir == BottomEnvirType.Source ||
          _tile.BottomEnvir == BottomEnvirType.Sea ||
          _tile.BottomEnvir == BottomEnvirType.Beach ||
          _tile.BottomEnvir == BottomEnvirType.RiverBeach ||
          _tile.BottomEnvir == BottomEnvirType.River ||
          _tile.TopEnvir == TopEnvirType.Mountain ||
          _tile.TopEnvir == TopEnvirType.Highland) return false;
        return true;
      }//발원지,강,바다,고원,산이면 False 아니면 True
    }
    */
    #endregion
    //     Debug.Log("고원 생성 완료");

    #region 숲
    LoopCount = 0;

    int _forestmaxcount = Mathf.CeilToInt(ConstValues.LandRadius * ConstValues.LandRadius*4.0f * ConstValues.Ratio_forest);    //숲 최대 개수
    List<TileData> _landtiles = _NewMapData.GetEnvirTiles
      (new List<BottomEnvirType> { BottomEnvirType.Land, BottomEnvirType.River, BottomEnvirType.RiverBeach },
      new List<TopEnvirType> { TopEnvirType.Mountain},1);
    List<TileData> _temp= new List<TileData>();
    foreach(var _land in _landtiles)if(!_land.Interactable)_temp.Add(_land);
    foreach (var _delete in _temp) _landtiles.Remove(_delete);
    List<int> _indexes= new List<int>();
    for(int i = 0; i < _landtiles.Count; i++)
    {
      int _length = _NewMapData.GetLength(_NewMapData.CenterTile.Coordinate, _landtiles[i].Coordinate).Count;
      int _count = _length <= (ConstValues.LandRadius / 2+1)?_length:(Mathf.Clamp( -_length+ (ConstValues.LandRadius),0,100));
      _count = _count - _landtiles[i].MovePoint < 1 ? 1 : _count - _landtiles[i].MovePoint;
      for (int j = 0; j < _count; j++)
      {
        _indexes.Add(i);
      }
    }
    _temp.Clear();
    TileData _selecttile = null;
    while(_temp.Count< _forestmaxcount)
    {
      _selecttile = _landtiles[_indexes[Random.Range(0,_indexes.Count)]];
      if (_temp.Contains(_selecttile)) continue;
      _temp.Add(_selecttile);
    }
    for (int i = 0; i < _temp.Count; i++)
    {
      _temp[i].TopEnvir = TopEnvirType.Forest;
      _temp[i].TopEnvirSprite = (_temp[i].BottomEnvir == BottomEnvirType.Source || _temp[i].BottomEnvir == BottomEnvirType.River) ?
        TileSpriteType.RiverForest : TileSpriteType.Forest;

    }

    /*
    int _forestcountaver = _forestmaxcount / 4; //숲 더미 평균 최대개수
    List<Vector2Int> _forests = new List<Vector2Int>();         //편성 완료된 숲 좌표들 보관할 리스트

    List<Vector2Int> _forestbundle_origin=new List<Vector2Int>();//(10,10)을 중심으로 퍼져나가는 모양새
    _forestbundle_origin = _NewMapData.GetAroundCoor(Vector2Int.one * 10, 5);

    int _forestmodifycount = (_forestbundle_origin.Count / 3) * 2;

    List<TileData> _emptylandtiles = _NewMapData.GetEnvirTiles
    (new List<BottomEnvirType> { BottomEnvirType.Land, BottomEnvirType.Source, BottomEnvirType.River }, new List<TopEnvirType> { TopEnvirType.Mountain, TopEnvirType.Highland, TopEnvirType.Forest }, 1);

    while (_forests.Count < _forestmaxcount)
    {
      LoopCount++;
      if (LoopCount > 1000) { Debug.Log("숲 생성 중 무한루프"); return null; }

      List<Vector2Int> _forestbundle=new List<Vector2Int>();
      foreach (var _coor in _forestbundle_origin) _forestbundle.Add(_coor);//origin(10,10)을 복사한 _forestbundle(10,10)

      List<Vector2Int> _deletebundles=new List<Vector2Int>();              //빈 구멍으로 만들 위치(10,10)
      for(int i = 0; i < _forestmodifycount; i++)
      {
        Vector2Int _deletetarget= _forestbundle[Random.Range(0, _forestbundle.Count)];
        if (_deletebundles.Contains(_deletetarget))
        {
          i--;
          continue;
        }
        _deletebundles.Add(_deletetarget);
      }
      foreach(var _deletecoor in _deletebundles)_forestbundle.Remove(_deletecoor);

      foreach (var _tiles in _deletebundles) _forestbundle.Remove(_tiles);
      //이거롷 개수 충족한 듬성듬성 숲 완성((10,10)기준)
      
      Vector2Int _startcoor=_emptylandtiles[Random.Range(0,_emptylandtiles.Count)].Coordinate;
      foreach(var _modify in _forestbundle)
      {
        Vector2Int _targetcoor = _startcoor + _modify-Vector2Int.one*10;
        if (_targetcoor.x < 0 || _targetcoor.y < 0 || _targetcoor.x > ConstValues.MapSize - 1 || _targetcoor.y > ConstValues.MapSize - 1) continue;
        TileData _targettile = _NewMapData.Tile(_targetcoor);

        if (_forests.Contains(_targetcoor)) continue;
        if (_targettile.BottomEnvir==BottomEnvirType.Sea||_targettile.BottomEnvir==BottomEnvirType.Beach) continue;
        if (_targettile.TopEnvir == TopEnvirType.Forest || _targettile.TopEnvir == TopEnvirType.Mountain) continue;

        _targettile.TopEnvir = TopEnvirType.Forest;
        _targettile.TopEnvirSprite = (_targettile.BottomEnvir == BottomEnvirType.Source || _targettile.BottomEnvir == BottomEnvirType.River) ?
          TileSpriteType.RiverForest : TileSpriteType.Forest;

        _forests.Add(_targetcoor);
        _emptylandtiles.Remove(_NewMapData.Tile(_targetcoor));
      }

    }//숲 개수 채우기 전까지 반복
     //  Debug.Log($"숲 최대 개수 : {_forestmaxcount}  현재 숲 개수 : {_forests.Count}");
    *///예전 숲 생성기
    #endregion
    //   Debug.Log("숲 생성 완료");
    #region 도시
    LoopCount = 0;

    Settlement City = new Settlement(SettlementType.City);
    TileData _citytile = null;
    HexDir _cityhexdir = (HexDir)Random.Range(0, 6);
    List<TileData> _citycreatetiles = _NewMapData.GetDirLines(_NewMapData.CenterTile, _cityhexdir);

    while (true)
    {
      LoopCount++;
      if (LoopCount > 1000) { Debug.Log("도시 생성 중 무한루프"); return null; }

      Vector2Int _starcoor = _citycreatetiles[Random.Range(3, _citycreatetiles.Count - 2)].Coordinate+new Vector2Int(Random.Range(-1,2), Random.Range(-1, 2));
     
      _citytile = _NewMapData.TileDatas[_starcoor.x, _starcoor.y];
      if (cityCheck(_citytile) == false) continue;

      _NewMapData.TileDatas[_citytile.Coordinate.x, _citytile.Coordinate.y].TileSettle = City;
      _NewMapData.TileDatas[_citytile.Coordinate.x, _citytile.Coordinate.y].Landmark = LandmarkType.City;
      City.Tile = _citytile;
      break;
      bool cityCheck(TileData tile)
      {
        if (tile.BottomEnvir == BottomEnvirType.Sea) return false;
        if (tile.TopEnvir == TopEnvirType.Mountain) return false;
        if (tile.TileSettle != null) return false;
        return true;
      }
    }

    #endregion
    //   Debug.Log("성채 생성 완료");

    #region 마을
    LoopCount = 0;

    TileData _towntile = null;
    Settlement Town = new Settlement(SettlementType.Town);
    HexDir _towndir = RotateDir(_cityhexdir, 3);
    List<HexDir> _villagedirs = new List<HexDir>() { (HexDir)0, (HexDir)1, (HexDir)2, (HexDir)3, (HexDir)4, (HexDir)5 };
    _villagedirs.Remove(_towndir); _villagedirs.Remove(_towndir);
    List<List<TileData>> _enablelines = new List<List<TileData>>();
    _enablelines.Add(_NewMapData.GetDirLines(_NewMapData.CenterTile, _towndir));
    List<TileData> _disabletiles=new List<TileData>();

    while (true)
    {
      LoopCount++;
      if (LoopCount > 1000) { Debug.Log("마을 생성 중 무한루프"); return null; }
      int _index = 0;

      Vector2Int _selectcoor = _enablelines[_index][Random.Range(3, _enablelines.Count - 2)].Coordinate+new Vector2Int(Random.Range(-1,2), Random.Range(-1, 2));
      _towntile = _NewMapData.TileDatas[_selectcoor.x, _selectcoor.y];
      if(towncheck(_towntile) ==false)continue;


      _NewMapData.TileDatas[_towntile.Coordinate.x, _towntile.Coordinate.y].TileSettle = Town;
      _NewMapData.Tile(_towntile.Coordinate).Landmark = LandmarkType.Town;
      Town.Tile = _towntile;
      break;

      bool towncheck(TileData tile)
      {
        if (tile.TopEnvir == TopEnvirType.Mountain) return false;
        if (tile.BottomEnvir == BottomEnvirType.Sea) return false;
        if (tile.TileSettle != null) return false;

        List<TileData> _aroundtiles = _NewMapData.GetAroundTile(tile, 1);
        foreach (var _tile in _aroundtiles) if (_tile.TileSettle != null) return false;

        return true;
      }
    }
    #endregion
    //      Debug.Log("도시 생성 완료");

    #region 촌락
    LoopCount = 0;

    List<Settlement> Villages = new List<Settlement> {  new Settlement(SettlementType.Village), new Settlement(SettlementType.Village) };
    List<TileData> _villagetiles= new List<TileData>();
    HexDir _firstvillagedir = _villagedirs[Random.Range(0, _villagedirs.Count)];
    while (_villagetiles.Count !=2)
    {
      LoopCount++;
      if (LoopCount > 1000) { Debug.Log("촌락 생성 중 무한루프"); return null; }

      HexDir _villagedir = _villagetiles.Count == 0 ? _firstvillagedir : RotateDir(_firstvillagedir, 3);
      List <TileData> _lines = _NewMapData.GetDirLines(_NewMapData.CenterTile, _villagedir);

      Vector2Int _selectcoor = _lines[Random.Range(3, _lines.Count-2)].Coordinate + new Vector2Int(Random.Range(-2, 3), Random.Range(-1, 2));
      TileData _villagetile = _NewMapData.Tile(_selectcoor);

      if (_villagetile.TopEnvir == TopEnvirType.Mountain) continue;
      if (_villagetile.TileSettle != null) continue;
      if (_villagetiles.Contains(_villagetile)) continue;
      if (_villagetile.BottomEnvir == BottomEnvirType.Sea) continue;

      List<TileData> _aroundtile = _NewMapData.GetAroundTile(_villagetile, 2);
      bool _breakable = false;
      foreach(var _tile in _aroundtile)
      {
        if (_villagetiles.Contains(_tile)||_tile.TileSettle!=null) { _breakable = true;break; }
      }
      if (_breakable) continue;//주위 2칸에 다른 정착지가 있으면 X

      _NewMapData.TileDatas[_villagetile.Coordinate.x, _villagetile.Coordinate.y].TileSettle = Villages[_villagetiles.Count];
      _NewMapData.Tile(_villagetile.Coordinate).Landmark = LandmarkType.Village;
      _villagetiles.Add(_villagetile);
    }

    for(int i = 0; i < _villagetiles.Count; i++)
    {
      Villages[i].Tile=_villagetiles[i];
    }
    #endregion
    //    Debug.Log("마을 생성 완료");


    #region 정착지 정보

    List<Settlement> _citylisttemp = new List<Settlement>() { City };
    SetSettle(ref _citylisttemp);
    List<Settlement> _townlisttemp = new List<Settlement>() { Town };
    SetSettle(ref _townlisttemp);
    SetSettle(ref Villages);

    _NewMapData.City = _citylisttemp[0];
    _NewMapData.Town = Town;
    _NewMapData.Villages=Villages;

    _NewMapData.AllSettles.Add(_citylisttemp[0]);
    _NewMapData.AllSettles.Add(_townlisttemp[0]);
    _NewMapData.AllSettles.Add(Villages[0]);
    _NewMapData.AllSettles.Add(Villages[1]);

    #endregion

    void SetSettle(ref List<Settlement> newsettles)
    {
      for (int i = 0; i < newsettles.Count; i++)
      {
        List<TileData> _aroundtiles_1 = _NewMapData.GetAroundTile(newsettles[i].Tile, 1);
        List<TileData> _aroundtiles_2 = _NewMapData.GetAroundTile(newsettles[i].Tile, 2);

        newsettles[i].IsRiver = CheckEnvir(_aroundtiles_1, BottomEnvirType.River) ||
          CheckEnvir(_aroundtiles_1, BottomEnvirType.Source) ||
          CheckEnvir(_aroundtiles_1, BottomEnvirType.RiverBeach);
        newsettles[i].IsMountain = CheckEnvir(_aroundtiles_1, TopEnvirType.Mountain);
        newsettles[i].IsSea = CheckEnvir(_aroundtiles_1, BottomEnvirType.Sea) || CheckEnvir(_aroundtiles_1, BottomEnvirType.Beach) || CheckEnvir(_aroundtiles_2, BottomEnvirType.RiverBeach);
    //    newsettles[i].IsHighland = CheckEnvir(_aroundtiles_1, TopEnvirType.Highland);
        newsettles[i].IsForest = CheckEnvir(_aroundtiles_1, TopEnvirType.Forest);


        string[] _namearray;
        if (newsettles[i].SettlementType == SettlementType.Village) { _namearray = SettlementName.VillageNames; }
        else if (newsettles[i].SettlementType == SettlementType.Town) { _namearray = SettlementName.TownNames; }
        else { _namearray = SettlementName.CityNames; }

        int _infoindex = Random.Range(0, _namearray.Length);
        while (true)
        {
          _infoindex = Random.Range(0, _namearray.Length);

          foreach (var _othersettle in newsettles)
            if (_othersettle.Index == _infoindex) continue;

          break;
        }
        newsettles[i].Index = _infoindex;
      }
    }

    return _NewMapData;


    GameManager.Instance.MyGameData.MyMapData = _NewMapData;
    MakeTilemap();
    return null;

  }
  bool CheckEnvir(List<TileData> tiles, BottomEnvirType envir)
  {
    foreach (var _tile in tiles)
    {
      if (_tile.BottomEnvir == envir) return true;
    }
    return false;
  }
  bool CheckEnvir(List<TileData> tiles, TopEnvirType envir)
  {
    foreach (var _tile in tiles)
    {
      if (_tile.TopEnvir == envir) return true;
    }
    return false;
  }
  int RotateDir(int origindir,int modify)
  {
    int _temp= origindir + modify;
    if (_temp < 0) _temp += 6;
    else if (_temp > 5) _temp -= 6;
    if (_temp == 6) Debug.Log($"{origindir} {modify} 테챠아아앗");
    return _temp;
  }
  HexDir RotateDir(HexDir origindir, int modify)
  {
    int _temp = (int)origindir + modify;
    if (_temp < 0) _temp += 6;
    else if (_temp > 5) _temp -= 6;
    if (_temp == 6) Debug.Log($"{origindir} {modify} 테챠아아앗");
    return (HexDir)_temp;
  }
  public IEnumerator MakeTilemap()
  {
    //190*0.7f
    Vector3 _cellsize = new Vector3(100.0f,100.0f); Debug.Log("맵을 만든 레후~");
    //타일로 구현화
    for (int i = 0; i < ConstValues.MapSize; i++)
    {
      for (int j = 0; j < ConstValues.MapSize; j++)
      {
        Vector3Int _coordinate = new Vector3Int(j, i, 0);

        Vector3 _pos = Tilemap_bottom.CellToWorld(_coordinate);
        TileData _currenttile = GameManager.Instance.MyGameData.MyMapData.TileDatas[j, i];
        int _rotate = _currenttile.Rotation;

        string _bottomname = $"{j},{i} {GameManager.Instance.MyGameData.MyMapData.Tile(_coordinate).BottomEnvir}";
        Sprite _bottomspr = MyTiles.GetTile(_currenttile.BottomEnvirSprite);
        GameObject _bottomtile = new GameObject(_bottomname, new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
        _bottomtile.transform.SetParent(TileHolder_bottomenvir);
        _bottomtile.AddComponent<Onpointer_tileoutline>().MyMapUI = MapUIScript;
        _bottomtile.GetComponent<Onpointer_tileoutline>().MyTile = GameManager.Instance.MyGameData.MyMapData.Tile(_coordinate);
        _bottomtile.GetComponent<Image>().sprite = GameManager.Instance.ImageHolder.Transparent;
        RectTransform _bottomrect = _bottomtile.GetComponent<RectTransform>();
        _bottomrect.sizeDelta = _cellsize;
        _bottomrect.position = new Vector3(_pos.x, _pos.y, _bottomrect.position.z);
        _bottomrect.transform.localScale = Vector3.one;
        _bottomrect.anchoredPosition3D = new Vector3(_bottomrect.anchoredPosition3D.x, _bottomrect.anchoredPosition3D.y, 0.0f);

        GameObject _bottomimageobj = new GameObject("_image", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
        Image _bottomimage = _bottomimageobj.GetComponent<Image>();
        _bottomimageobj.transform.SetParent(_bottomtile.transform);
        _bottomimage.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -60.0f * _rotate));
        _bottomimage.sprite = _bottomspr;
        _bottomimage.raycastTarget = false;
        RectTransform _bottomimagerect = _bottomimageobj.GetComponent<RectTransform>();
        _bottomimagerect.sizeDelta = _cellsize;
        _bottomimagerect.anchoredPosition = Vector3.zero;
        _bottomimagerect.transform.localScale = Vector3.one;
    //    Outline _bottomoutline = _bottomtile.AddComponent<Outline>();
    //    _bottomoutline.effectColor = Color.black;
    //    _bottomoutline.effectDistance = Vector2.one * 2.0f;
        Button _button = _bottomtile.AddComponent<Button>();
        Navigation _nav = new Navigation();
        _nav.mode = Navigation.Mode.None;
        _button.navigation = _nav;
    //    _bottomoutline.enabled = false;
        _bottomtile.AddComponent(typeof(TileButtonScript));
        _bottomtile.GetComponent<Image>().raycastTarget = GameManager.Instance.MyGameData.MyMapData.Tile(_coordinate).Interactable;
        _button.interactable = false;
        var _buttoncolor = _button.colors;
        _buttoncolor.disabledColor = Color.white;
        _button.colors = _buttoncolor;

        string _topname = $"{j},{i} {GameManager.Instance.MyGameData.MyMapData.Tile(_coordinate).TopEnvir}";
        Sprite _topespr = MyTiles.GetTile(_currenttile.TopEnvirSprite);
        GameObject _toptile = new GameObject(_topname, new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
        _toptile.transform.SetParent(TileHolder_topenvir);
        RectTransform _toprect = _toptile.GetComponent<RectTransform>();
        _toprect.sizeDelta = _cellsize;
        _toprect.position = new Vector3(_pos.x, _pos.y, _toprect.position.z);
        _toprect.transform.localScale = Vector3.one;
        _toprect.anchoredPosition3D = new Vector3(_toprect.anchoredPosition3D.x, _toprect.anchoredPosition3D.y, 0.0f);
        Image _topimage = _toptile.GetComponent<Image>();
        _topimage.raycastTarget = false;
        _topimage.sprite = _topespr;

        string _landmarkname = $"{j},{i} {GameManager.Instance.MyGameData.MyMapData.Tile(_coordinate).Landmark}";
        Sprite _landmarkspr = MyTiles.GetTile(_currenttile.landmarkSprite);
        GameObject _landmarktile= new GameObject(_landmarkname, new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
        _landmarktile.transform.SetParent(TileHolder_landmark);
        RectTransform _landmarkrect = _landmarktile.GetComponent<RectTransform>();
        _landmarkrect.sizeDelta = _cellsize;
        _landmarkrect.position = new Vector3(_pos.x, _pos.y, _landmarkrect.position.z);
        _landmarkrect.transform.localScale = Vector3.one;
        _landmarkrect.anchoredPosition3D = new Vector3(_landmarkrect.anchoredPosition3D.x, _landmarkrect.anchoredPosition3D.y, 0.0f);
        Image _landmarkimage = _landmarktile.GetComponent<Image>();
        _landmarkimage.raycastTarget = false;
        _landmarkimage.sprite = _landmarkspr;


        TileButtonScript _buttonscript = _bottomtile.GetComponent<TileButtonScript>();
        _buttonscript.Rect = _bottomrect;
       // _buttonscript.OutLine = _bottomoutline;
        _buttonscript.Button = _button;
        //_buttonscript.SelectHolder = MapUIScript.SelectTileHolder;
       // _buttonscript.OriginHolder = TileHolder_bottomenvir;
        _buttonscript.MapUI = MapUIScript;
        _buttonscript.TileData = GameManager.Instance.MyGameData.MyMapData.Tile(_coordinate);
        _buttonscript.BottomImage = _bottomimage;
        _buttonscript.TopImage = _topimage;
        _buttonscript.LandmarkImage= _landmarkimage;
        _buttonscript.OnPointer = _bottomtile.GetComponent<Onpointer_tileoutline>();
        _button.onClick.AddListener(() => _buttonscript.Clicked());
        PreviewInteractive _tilepreview = _buttonscript.Rect.transform.AddComponent<PreviewInteractive>();
        _tilepreview.PanelType = PreviewPanelType.TileInfo;
        _tilepreview.MyTileData = _currenttile;
        _buttonscript.Preview= _tilepreview;
        _tilepreview.enabled = false;
        GameObject _previewpos_bottom = new GameObject("_posholder", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer) });
        _tilepreview.OtherRect = _previewpos_bottom.GetComponent<RectTransform>();
        _previewpos_bottom.transform.localScale = Vector3.zero;
        _previewpos_bottom.transform.parent = _bottomtile.transform;
        _previewpos_bottom.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f,-_cellsize.y / 2.0f);
        GameObject _previewpos_top = new GameObject("_posholder", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer) });
        _tilepreview.OtherRect_other = _previewpos_top.GetComponent<RectTransform>();
        _previewpos_top.transform.localScale = Vector3.zero;
        _previewpos_top.transform.parent = _bottomtile.transform;
        _previewpos_top.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, _cellsize.y / 2.0f);

        GameManager.Instance.MyGameData.MyMapData.Tile(_coordinate).ButtonScript = _buttonscript;
      }
    }
    //이 밑은 정착지를 버튼으로 만드는거

    List<TileData> _settlementrectlist= new List<TileData>();
    Vector2 _settlementpos = Vector2.zero;
    Vector2 _outlinesize = Vector2.one * 135.0f;
    Color _outlinecolor = new Color(0.5f, 0.0f, 1.0f, 1.0f);
    for (int i = 0; i < GameManager.Instance.MyGameData.MyMapData.Villages.Count; i++)
    {
      Settlement _village = GameManager.Instance.MyGameData.MyMapData.Villages[i];
      _settlementpos = _village.Tile.ButtonScript.Rect.anchoredPosition;

      string _villagename = _village.OriginName;
      GameObject _villageholder = new GameObject(_villagename, new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(CanvasGroup) });
      _villageholder.transform.SetParent(TileHolder_landmark);
      _villageholder.GetComponent<RectTransform>().anchoredPosition = _settlementpos;
      _villageholder.GetComponent<CanvasGroup>().blocksRaycasts = false;
      _villageholder.transform.localScale = Vector3.one;

      _village.Tile.ButtonScript.LandmarkImage.transform.SetParent(_villageholder.transform);
      _village.Tile.ButtonScript.Rect.localScale = Vector3.one;

      MapUIScript.VillageIcons.Add(_villageholder);

      GameObject _villageoutline=new GameObject(_villagename+"_outline",new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(CanvasGroup) });
      Image _villageoutlineimage = _villageoutline.GetComponent<Image>();
      _villageoutlineimage.sprite = GameManager.Instance.ImageHolder.VillageIcon_outline;
      _villageoutlineimage.raycastTarget = false;
      _villageoutlineimage.color = _outlinecolor;
      CanvasGroup _villageoutlinegroup = _villageoutline.GetComponent<CanvasGroup>();
      _villageoutlinegroup.alpha = 0.0f;
      _villageoutlinegroup.interactable = false;
      _villageoutlinegroup.blocksRaycasts = false;
      _villageoutline.transform.SetParent(_villageholder.transform);
      _villageoutline.transform.SetAsFirstSibling();
      _villageoutline.GetComponent<RectTransform>().sizeDelta = _outlinesize;
      _villageoutlineimage.rectTransform.anchoredPosition = Vector3.zero;
      _villageoutline.transform.localScale= Vector3.one;
      _village.Tile.ButtonScript.DiscomfortOutline = _villageoutlinegroup;
    }

    _settlementrectlist.Clear();
      Settlement _town = GameManager.Instance.MyGameData.MyMapData.Town;
    _settlementrectlist.Add(_town.Tile);
    _settlementpos = _town.Tile.ButtonScript.Rect.anchoredPosition;

      string _townname = _town.OriginName;
      GameObject _townholder = new GameObject(_townname, new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer),typeof(CanvasGroup) });
      _townholder.transform.SetParent(TileHolder_landmark);
      _townholder.GetComponent<RectTransform>().anchoredPosition = _settlementpos;
      _townholder.GetComponent<CanvasGroup>().blocksRaycasts = false;
    _townholder.transform.localScale = Vector3.one;

    for (int j = 0; j < _settlementrectlist.Count; j++)
      {
        _settlementrectlist[j].ButtonScript.LandmarkImage.transform.SetParent(_townholder.transform);
      _town.Tile.ButtonScript.Rect.localScale = Vector3.one;
    }
    MapUIScript.TownIcon=(_townholder);
    GameObject _townoutline = new GameObject(_townname + "_outline", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(CanvasGroup) });
    Image _townoutlineimage = _townoutline.GetComponent<Image>();
    _townoutlineimage.sprite = GameManager.Instance.ImageHolder.TownIcon_outline;
    _townoutlineimage.raycastTarget = false;
    _townoutlineimage.color = _outlinecolor;
    CanvasGroup _townoutlinegroup = _townoutline.GetComponent<CanvasGroup>();
    _townoutlinegroup.alpha = 0.0f;
    _townoutlinegroup.interactable = false;
    _townoutlinegroup.blocksRaycasts = false;
    _townoutline.transform.SetParent(_townholder.transform);
    _townoutline.transform.SetAsFirstSibling();
    _townoutline.GetComponent<RectTransform>().sizeDelta = _outlinesize;
    _townoutlineimage.rectTransform.anchoredPosition = Vector3.zero;
    _townoutline.transform.localScale = Vector3.one;
    _town.Tile.ButtonScript.DiscomfortOutline = _townoutlinegroup;

    _settlementrectlist.Clear();
    Settlement _city = GameManager.Instance.MyGameData.MyMapData.City;
    _settlementrectlist.Add(_city.Tile);
    _settlementpos = _city.Tile.ButtonScript.Rect.anchoredPosition;

    string _cityname = _city.OriginName;
    GameObject _cityholder = new GameObject(_cityname, new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(CanvasGroup) });
    _cityholder.transform.SetParent(TileHolder_landmark);
    _cityholder.GetComponent<RectTransform>().anchoredPosition = _settlementpos;
    _cityholder.GetComponent<CanvasGroup>().blocksRaycasts = false;
    _cityholder.transform.localScale = Vector3.one;

    for (int j = 0; j < _settlementrectlist.Count; j++)
    {
      _settlementrectlist[j].ButtonScript.LandmarkImage.transform.SetParent(_cityholder.transform);
      _city.Tile.ButtonScript.Rect.localScale = Vector3.one;
    }
    MapUIScript.CityIcon=_cityholder;

    GameObject _cityoutline = new GameObject(_cityname + "_outline", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(CanvasGroup) });
    Image _cityoutlineimage = _cityoutline.GetComponent<Image>();
    _cityoutlineimage.sprite = GameManager.Instance.ImageHolder.CityIcon_outline;
    _cityoutlineimage.raycastTarget = false;
    _cityoutlineimage.color = _outlinecolor;
    CanvasGroup _cityoutlinegroup = _cityoutline.GetComponent<CanvasGroup>();
    _cityoutlinegroup.alpha = 0.0f;
    _cityoutlinegroup.interactable = false;
    _cityoutlinegroup.blocksRaycasts = false;
    _cityoutline.transform.SetParent(_cityholder.transform);
    _cityoutline.transform.SetAsFirstSibling();
    _cityoutline.GetComponent<RectTransform>().sizeDelta = _outlinesize;
    _cityoutlineimage.rectTransform.anchoredPosition= Vector3.zero;
    _cityoutline.transform.localScale = Vector3.one;
    _city.Tile.ButtonScript.DiscomfortOutline = _cityoutlinegroup;

    yield return null;
  }

  /// <summary>
  /// startpos에서 dir(0~6) 방향 1칸 타일 좌표 반환
  /// </summary>
  /// <param name="startpos"></param>
  /// <param name="dir"></param>
  /// <returns></returns>
  public Vector2Int GetNextPos(Vector2Int startpos,int dir)
    {
        bool _iseven = startpos.y % 2 == 0;  //startpos의 y가 짝수인지 판별
        Vector2Int _modify=new Vector2Int();
        switch (dir)
        {
            case 0: _modify = _iseven ? new Vector2Int(0, 1) : new Vector2Int(1, 1); break;
            case 1: _modify = new Vector2Int(1, 0); break;
            case 2: _modify = _iseven ? new Vector2Int(0, -1) : new Vector2Int(1, -1); break;
            case 3: _modify = _iseven ? new Vector2Int(-1, -1) : new Vector2Int(0, -1); break;
            case 4: _modify = new Vector2Int(-1, 0); break;
            case 5: _modify = _iseven ? new Vector2Int(-1, 1) : new Vector2Int(0, 1); break;
        }
        return startpos + _modify;
    }
}
public class RiverData
{
  public int RiverIndex = 0;                                            //시계방향 0~5
  public List<Vector2Int> RiverCoors= new List<Vector2Int>();           //모든 강 좌표
  public List<int> RiverDirs=new List<int>();                           //모든 강 진행 방향(절대 방향)
}

[System.Serializable]
public class HexGrid
{
  public int q = 0, r = 0, s = 0;
  public HexGrid()
  {
    q = 0;
    r = 0;
    s = 0;
  }
  public HexGrid(int q, int r, int s)
  {
    this.q = q;
    this.r = r;
    this.s = s;
  }
  public HexGrid(Vector2Int vector2)
  {
    q = vector2.x - (vector2.y - (vector2.y &1)) / 2;
    r = vector2.y;
    s = -q - r;
  }
  public void Clear()
  {
    q = 0; r = 0; s = 0;
  }
  public HexGrid AddHexdir(HexDir hexdir)
  {
    switch ((int)hexdir)
    {
      case 0:
        return new HexGrid(q, r + 1, s - 1);
      case 1:
        return new HexGrid(q + 1, r, s - 1);
      case 2:
        return new HexGrid(q + 1, r - 1, s);
      case 3:
        return new HexGrid(q, r - 1, s + 1);
      case 4:
        return new HexGrid(q - 1, r, s + 1);
      case 5:
        return new HexGrid(q - 1, r + 1, s);
    }
    return null;
  }
  public static HexGrid operator +(HexGrid h0, HexGrid h1)
  {
    return new HexGrid(h0.q + h1.q, h0.r + h1.r, h0.s + h1.s);
  }
  public static HexGrid operator -(HexGrid h0, HexGrid h1)
  {
    return new HexGrid(h0.q - h1.q, h0.r - h1.r, h0.s - h1.s);
  }
  public List<HexDir> GetDir
  {
    get
    {
      string _str = $"({q},{r},{s}) ->";

      int _loopcount = 0;
      List<HexDir> _list = new List<HexDir>();
      HexGrid _current = new HexGrid(q, r, s);
      HexGrid _temp = new HexGrid();
      int _value = 100;
      int _sign = 0;
      HexGrid _dirgrid = new HexGrid();

      while (_value != 0)
      {
        _loopcount++;
        if (_loopcount > 100) { Debug.Log("테챠앗!!!"); return null; }

        _value = _current.q / 2;
        if (_value != 0)
        {
          _sign = (int)Mathf.Sign(_value);
          for (int i = 0; i < Mathf.Abs(_value); i++)
          {
            _dirgrid.q = 2 * _sign;
            _dirgrid.r = -1 * _sign;
            _dirgrid.s = -1 * _sign;

            _current -= _dirgrid;

            foreach (var _hex in GetDir(_dirgrid))
              _list.Add(_hex);
          }
        }
          _value = _current.r / 2;
          if (_value != 0)
          {
            _sign = (int)Mathf.Sign(_value);
            for (int i = 0; i < Mathf.Abs(_value); i++)
            {
              _dirgrid.q = -1 * _sign;
              _dirgrid.r = 2 * _sign;
              _dirgrid.s = -1 * _sign;

              _current -= _dirgrid;

              foreach (var _hex in GetDir(_dirgrid))
                _list.Add(_hex);
            }

            _value = _current.s / 2;
            if (_value != 0)
            {
              _sign = (int)Mathf.Sign(_value);
              for (int i = 0; i < Mathf.Abs(_value); i++)
              {
                _dirgrid.q = -1 * _sign;
                _dirgrid.r = -1 * _sign;
                _dirgrid.s = 2 * _sign;

                _current -= _dirgrid;

                foreach (var _hex in GetDir(_dirgrid))
                  _list.Add(_hex);
              }

            }
          }

        }

      foreach (var hex in GetDir(_current))
      {
        _list.Add(hex);
      }
      updatedirstr();
      bool _isoverlap = true;
      while (_isoverlap == true)
      {
        _isoverlap = false;

        if (_list.Contains((HexDir)0) && _list.Contains((HexDir)3))
        {
          _list.Remove((HexDir)0);
          _list.Remove((HexDir)3);
          _isoverlap = true;

          _str += "   0+3 중복 제거";
          updatedirstr();
        }
        if (_list.Contains((HexDir)1) && _list.Contains((HexDir)4))
        {
          _list.Remove((HexDir)1);
          _list.Remove((HexDir)4);
          _isoverlap = true;
        
          _str += "   1+3 중복 제거";
          updatedirstr();
        }
        if (_list.Contains((HexDir)2) && _list.Contains((HexDir)5))
        {
          _list.Remove((HexDir)2);
          _list.Remove((HexDir)5);
          _isoverlap = true;
         
          _str += "   2+5 중복 제거";
          updatedirstr();
        }

        if (_list.Contains((HexDir)1) && _list.Contains((HexDir)5))
        {
          _list.Remove((HexDir)1);
          _list.Remove((HexDir)5);

          _list.Add((HexDir)0);
          _isoverlap = true;

          _str += "   5+1 => 0";
          updatedirstr();
        }
        if (_list.Contains((HexDir)0) && _list.Contains((HexDir)2))
        {
          _list.Remove((HexDir)0);
          _list.Remove((HexDir)2);

          _list.Add((HexDir)1);
          _isoverlap = true;

          _str += "   0+2 => 1";
          updatedirstr();
        }
        if (_list.Contains((HexDir)1) && _list.Contains((HexDir)3))
        {
          _list.Remove((HexDir)1);
          _list.Remove((HexDir)3);

          _list.Add((HexDir)2);
          _isoverlap = true;

          _str += "   1+3 => 2";
          updatedirstr();
        }
        if (_list.Contains((HexDir)2) && _list.Contains((HexDir)4))
        {
          _list.Remove((HexDir)2);
          _list.Remove((HexDir)4);

          _list.Add((HexDir)3);
          _isoverlap = true;

          _str += "   2+4 => 3";
          updatedirstr();
        }
        if (_list.Contains((HexDir)3) && _list.Contains((HexDir)5))
        {
          _list.Remove((HexDir)3);
          _list.Remove((HexDir)5);

          _list.Add((HexDir)4);
          _isoverlap = true;

          _str += "   3+5 => 4";
          updatedirstr();
        }
        if (_list.Contains((HexDir)0) && _list.Contains((HexDir)4))
        {
          _list.Remove((HexDir)0);
          _list.Remove((HexDir)4);

          _list.Add((HexDir)5);
          _isoverlap = true;

          _str += "   4+0 => 5";
          updatedirstr();
        }
      }

   //   Debug.Log(_str);
      return _list;

      //최소단위 받아서 계산
      List<HexDir> GetDir(HexGrid grid)
      {
        List<HexDir> _temp = new List<HexDir>();

        if (grid.q == 2) { _temp.Add((HexDir)1); _temp.Add((HexDir)2); }
        else if (grid.q == -2) { _temp.Add((HexDir)4); _temp.Add((HexDir)5); }
        else if (grid.r == 2) { _temp.Add((HexDir)0); _temp.Add((HexDir)5); }
        else if (grid.r == -2) { _temp.Add((HexDir)2); _temp.Add((HexDir)3); }
        else if (grid.s == 2) { _temp.Add((HexDir)3); _temp.Add((HexDir)4); }
        else if (grid.s == -2) { _temp.Add((HexDir)0); _temp.Add((HexDir)1); }
        else if (grid.q == 0 && grid.r == 1 && grid.s == -1) _temp.Add((HexDir)0);
        else if (grid.q == 1 && grid.r == 0 && grid.s == -1) _temp.Add((HexDir)1);
        else if (grid.q == 1 && grid.r == -1 && grid.s == 0) _temp.Add((HexDir)2);
        else if (grid.q == 0 && grid.r == -1 && grid.s == 1) _temp.Add((HexDir)3);
        else if (grid.q == -1 && grid.r == 0 && grid.s == 1) _temp.Add((HexDir)4);
        else if (grid.q == -1 && grid.r == 1 && grid.s == 0) _temp.Add((HexDir)5);

        return _temp;
      }
      void updatedirstr()
      {
        _str += "\n(";
        foreach(var hex in _list)
        {
          _str += $"{(int)hex},";
        }
        _str += ")";
      }
    }
  }
}