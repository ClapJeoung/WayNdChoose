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
  [SerializeField] private UI_map MapUIScript = null;
  public Tilemap Tilemap_bottom, Tilemap_top;
   public TilePrefabs MyTiles;
  [Space(10)]
  [SerializeField] private Transform TileHolder_bottomenvir = null;
  [SerializeField] private Transform TileHolder_topenvir = null;
  [SerializeField] private Transform TileHolder_EventMark = null;
  [SerializeField] private Transform TileHolder_landmark = null;
  [SerializeField] private Transform TileHolder_Fog = null;
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

  public IEnumerator makeperfectmap(bool realperfect)
  {
    int _index = 0;
    while (true)
    {
      _index++;
      if (_index > 100)
      {
        Debug.Log("테챠아");
        yield break;
      }
      MapData _map = CreateMap();
      if (_map == null) continue;
      yield return null;
      if (realperfect)
      {
        bool _anyriver = false, _anyforest = false, _anymountain = false;

        foreach (var _village in _map.Villages)
        {
          if (!_anyriver && _village.IsRiver) _anyriver = true;
          if (!_anyforest && _village.IsForest) _anyforest = true;
          if (!_anymountain && _village.IsMountain) _anymountain = true;
        }

        foreach (var _town in _map.Towns)
        {
          if (!_anyriver && _town.IsRiver) _anyriver = true;
          if (!_anyforest && _town.IsForest) _anyforest = true;
          if (!_anymountain && _town.IsMountain) _anymountain = true;
        }

        foreach (var _city in _map.Citys)
        {
          if (!_anyriver && _city.IsRiver) _anyriver = true;
          if (!_anyforest && _city.IsForest) _anyforest = true;
          if (!_anymountain && _city.IsMountain) _anymountain = true;
        }

        if (!_anyforest) { Debug.Log(_index + "번 맵 숲 하나도 없음"); continue; }
        if (!_anymountain) { Debug.Log(_index + "번 맵 산 하나도 없음"); continue; }
        if (!_anyriver) { Debug.Log(_index + "번 맵 강 하나도 없음"); continue; }
        if (_map.Villages.Count < 3 || _map.Towns.Count < 3 || _map.Citys.Count < 2) { yield return null; continue; }
      }
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
    List<Vector2Int> _firstlandcoors = _NewMapData.GetAroundCoor(_NewMapData.CenterTile.Coordinate, ConstValues.LandRadius);
    foreach (var _landcoor in _firstlandcoors)
    {
      _NewMapData.Tile(_landcoor).BottomEnvir = BottomEnvirType.Land;
      _NewMapData.Tile(_landcoor).BottomEnvirSprite = TileSpriteType.Land;
    }
    #endregion
    #region 바다,해안선
    List<TileData> _seatiles = _NewMapData.GetEnvirTiles(new List<BottomEnvirType> { BottomEnvirType.Sea });
    List<TileData> _outerbeachtiles = new List<TileData>();

    foreach (var _sea in _seatiles)
    {
      List<TileData> _around_1 = _NewMapData.GetAroundTile(_sea, 1);
      List<TileData> _around_2 = _NewMapData.GetAroundTile(_sea, 2);
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
      foreach (var _targettile in _around_2)
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

    List<TileData> _poundtiles = new List<TileData>();
    List<TileData> _disablebeaches = new List<TileData>();
    while (true)
    {
      LoopCount++;
      if (LoopCount > 1000) { Debug.Log("웅덩이 제거 중 무한루프"); return null; }

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
      foreach (var _sea in _currentseas)
      {
        List<TileData> _around = _NewMapData.GetAroundTile(_sea, 1);
        _around.Remove(_sea);

        if (_around.Count < 6) continue;

        int _landcount = 0;
        foreach (var _tile in _around) if (_tile.BottomEnvir == BottomEnvirType.Land) _landcount++;

        if (_landcount == 6) _poundtiles.Add(_sea);
      }
      foreach (var _tile in _poundtiles)
      {
        _NewMapData.Tile(_tile.Coordinate).BottomEnvir = BottomEnvirType.Land;
        _NewMapData.Tile(_tile.Coordinate).BottomEnvirSprite = TileSpriteType.Land;
      }

      if (_disablebeaches.Count == 0 && _poundtiles.Count == 0) break;

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

      foreach (var _center in _detectcentertiles)
      {
        for (int i = 0; i < 6; i++)
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
    foreach (var seatile in _seatiles)
    {
      if (_pureseatiles.Contains(seatile)) continue;
      _NewMapData.TileDatas[seatile.Coordinate.x, seatile.Coordinate.y].BottomEnvir = BottomEnvirType.Land;
      _NewMapData.TileDatas[seatile.Coordinate.x, seatile.Coordinate.y].BottomEnvirSprite = TileSpriteType.Land;
    }

    for (int i = 0; i < ConstValues.MapSize; i++)
      for (int j = 0; j < ConstValues.MapSize; j++)
      {
        if (_NewMapData.Tile(new Vector2Int(j, i)).BottomEnvir == BottomEnvirType.Sea) continue;

        List<TileData> _around = _NewMapData.GetAroundTile(new Vector2Int(j, i), 1);
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
    for (int i = 0; i < _beaches.Count; i++)
    {
      List<int> _seadirs = new List<int>();
      for (int j = 0; j < 6; j++)
      {
        if (_NewMapData.GetNextTile(_beaches[i], (HexDir)j).BottomEnvir == BottomEnvirType.Sea) _seadirs.Add(j);
      }

      if (_seadirs.Count == 2 && RotateDir(_seadirs[0], 3) == _seadirs[1])
      {
        _beaches[i].BottomEnvirSprite = TileSpriteType.Beach_middle;
        for (int j = 0; j < 6; j++)
        {
          if ((_seadirs[0] == 0 && _seadirs[1] == 3) || (_seadirs[0] == 3 && _seadirs[1] == 0))
          {
            _beaches[i].Rotation = j;
          }

          _seadirs[0] = RotateDir(_seadirs[0], -1);
          _seadirs[1] = RotateDir(_seadirs[1], -1);
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
      Vector2Int _sourcecoor = _NewMapData.GetNextCoor(_NewMapData.CenterTile, (HexDir)i,true);
      _riverdata.RiverIndex = i;
      _riverdata.RiverCoors.Add(_sourcecoor);
      _riverdata.RiverDirs.Add(i);
      _riverdata.RiverCoors.Add(_NewMapData.GetNextCoor(_sourcecoor, (HexDir)i,true));
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
            _failcoors.Add(_NewMapData.GetNextCoor(_currentcoor, (HexDir)_finishdir,true));
            continue;
          }//해변을 찾았는데 강 개수를 채우지 못한 경우(해당 타일 failcoor)
          else
          {
            //   Debug.Log($"{i + 1}번째 강 [{_riverdata.RiverCoors.Count + 1}]{_currentcoor} ->({_finishdir})-> [{_riverdata.RiverCoors.Count + 2}]{_NewMapData.GetNextCoor(_currentcoor, (HexDir)_finishdir)} 강 종료");
            _riverdata.RiverCoors.Add(_NewMapData.GetNextCoor(_currentcoor, (HexDir)_finishdir,true));
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
        Vector2Int _nextcoor = _NewMapData.GetNextCoor(_currentcoor, (HexDir)_selectdir,true);

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
              _riverbeach.BottomEnvirSprite = MyTiles.GetRiverBeach(_seadirs.Count, RotateDir(_lastdir, -k-1));
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

    #region 산
    LoopCount = 0;

    bool _mountaindone = false;
    List<TileData> _newmountain = new List<TileData>(); //산 위치 리스트
    System.Random _random = new System.Random();
    int[] _mountaindirs = new int[6] { 0, 1, 2, 3, 4, 5 };
    for(int i = 0; i < _mountaindirs.Length; i++)
    {
      _mountaindone = false;

      List<TileData> _lines = _NewMapData.GetDirLines(_NewMapData.CenterTile, (HexDir)_mountaindirs[i]);
      int _tilecount = Random.Range(Mathf.FloorToInt(_lines.Count * ConstValues.Mountain_length_min), Mathf.FloorToInt(_lines.Count * ConstValues.Mountain_length_max));
      Vector2Int _startcoordinate = _lines[_tilecount].Coordinate;
      for (int j = 0; j < _tilecount / 2; j++)
        _startcoordinate = _NewMapData.GetNextCoor(_startcoordinate, (HexDir)RotateDir(_mountaindirs[i], 2), true);
      List<TileData> _templist = _NewMapData.GetAroundTile(_startcoordinate, 1);
      List<TileData> _mountainstartlist = new List<TileData>(); //목표 중심 주위의 1칸 타일들
      foreach (var _tile in _templist)
      {
        if (_tile.BottomEnvir != BottomEnvirType.Land) continue;
        _mountainstartlist.Add(_tile);
      }
      _mountainstartlist = _mountainstartlist.OrderBy(_ => _random.Next()).ToList();

      for (int j = 0; j < _mountainstartlist.Count; j++)
      {
        List<int> _currentmountaindirs = _mountaindirs.OrderBy(_ => _random.Next()).ToList(); //0~5 무작위로 배열
        for(int k = 0; k < _currentmountaindirs.Count; k++)
        {
          List<List<TileData>> _mountaintilelists = new List<List<TileData>> { new List<TileData> { _mountainstartlist[0] } };
          for(int l = 0; l < ConstValues.Mountain_Count_max; l++)
          {
            List<TileData> _currentlinetiles= new List<TileData>();
            TileData _previewtile = _mountaintilelists[0][0];
            for (int n = 0; n < _mountaintilelists.Count; n++) _previewtile = _NewMapData.GetNextTile(_previewtile,(HexDir)_currentmountaindirs[k]);
            for(int n = -1; n < 2; n++)
            {
              TileData _targettile = _NewMapData.GetNextTile(_previewtile, (HexDir)RotateDir(_currentmountaindirs[k], n));
              if (_targettile.BottomEnvir != BottomEnvirType.Land || _newmountain.Contains(_targettile)) continue;
              _currentlinetiles.Add(_targettile);
            }
            _mountaintilelists.Add(_currentlinetiles);
          }
          int _ablecount = 0;
          foreach (var _list in _mountaintilelists) if (_list.Count > 0) _ablecount++;

          _mountaindone = _ablecount >= ConstValues.Mountain_Count_min;

          if (_mountaindone)
          {
            foreach (var _list in _mountaintilelists)
            {
              if (_list.Count == 0) continue;
              _newmountain.Add(_list[Random.Range(0, _list.Count)]);
            }
            break;
          }
        }
        if (_mountaindone) break;
      }
    }
    for(int i=0;i< _newmountain.Count;i++)
    {
      _newmountain[i].TopEnvir = TopEnvirType.Mountain;
      _newmountain[i].TopEnvirSprite = TileSpriteType.Mountain;
    }

    #endregion
    //     Debug.Log("산 생성 완료");

    #region 숲
    LoopCount = 0;

    int _forestmaxcount = Mathf.CeilToInt(ConstValues.LandRadius * ConstValues.LandRadius * 4.0f * ConstValues.Ratio_forest);    //숲 최대 개수
    List<TileData> _landtiles = _NewMapData.GetEnvirTiles
      (new List<BottomEnvirType> { BottomEnvirType.Land, BottomEnvirType.River, BottomEnvirType.RiverBeach },
      new List<TopEnvirType> { TopEnvirType.Mountain }, 1);
    List<TileData> _temp = new List<TileData>();
    foreach (var _land in _landtiles) if (!_land.Interactable) _temp.Add(_land);
    foreach (var _delete in _temp) _landtiles.Remove(_delete);
    List<int> _indexes = new List<int>();
    for (int i = 0; i < _landtiles.Count; i++)
    {
      int _length = _NewMapData.CenterTile.HexGrid.GetDistance(_landtiles[i]);
      int _count = _length <= (ConstValues.LandRadius / 2 + 1) ? _length : (Mathf.Clamp(-_length + (ConstValues.LandRadius), 0, 100));
      _count = _count - _landtiles[i].RequireSupply < 1 ? 1 : _count - _landtiles[i].RequireSupply;
      for (int j = 0; j < _count; j++)
      {
        _indexes.Add(i);
      }
    }
    _temp.Clear();
    TileData _selecttile = null;
    while (_temp.Count < _forestmaxcount)
    {
      _selecttile = _landtiles[_indexes[Random.Range(0, _indexes.Count)]];
      if (_temp.Contains(_selecttile)) continue;
      _temp.Add(_selecttile);
    }
    for (int i = 0; i < _temp.Count; i++)
    {
      _temp[i].TopEnvir = TopEnvirType.Forest;
      _temp[i].TopEnvirSprite = (_temp[i].BottomEnvir == BottomEnvirType.Source || _temp[i].BottomEnvir == BottomEnvirType.River) ?
        TileSpriteType.RiverForest : TileSpriteType.Forest;

    }

    #endregion
    //   Debug.Log("숲 생성 완료");
    #region 도시
    LoopCount = 0;
    int _settlementcompeleteeindex = 0;
    List<List<TileData>> _enablelines = new List<List<TileData>>();

    List<Settlement> City = new List<Settlement> { new Settlement(SettlementType.City), new Settlement(SettlementType.City) };
    TileData _citytile = null;
    List<HexDir> _cityhexdir = new List<HexDir>();
    switch (Random.Range(0, 3))
    {
      case 0:
        _cityhexdir.Add((HexDir)0);
        _cityhexdir.Add((HexDir)3);
        break;
      case 1:
        _cityhexdir.Add((HexDir)1);
        _cityhexdir.Add((HexDir)4);
        break;
      case 2:
        _cityhexdir.Add((HexDir)2);
        _cityhexdir.Add((HexDir)5);
        break;
    }
    _enablelines.Add(_NewMapData.GetDirLines(_NewMapData.CenterTile, _cityhexdir[0]));
    _enablelines.Add(_NewMapData.GetDirLines(_NewMapData.CenterTile, _cityhexdir[1]));

    while (_settlementcompeleteeindex < 2)
    {
      LoopCount++;
      if (LoopCount > 100) { Debug.Log("도시 생성 중 무한루프"); return null; }
      int _index = _settlementcompeleteeindex;

      int _lengthmin = Mathf.FloorToInt(_enablelines[_index].Count * ConstValues.SettlementLength_Town);
      int _lengthmax = Mathf.FloorToInt(_enablelines[_index].Count * ConstValues.SettlementLength_City);
      Vector2Int _starcoor = _enablelines[_index][Random.Range(_lengthmin, _lengthmax)].Coordinate + new Vector2Int(Random.Range(-1, 2), Random.Range(-1, 2));

      _citytile = _NewMapData.TileDatas[_starcoor.x, _starcoor.y];
      if (cityCheck(_citytile) == false) continue;

      _NewMapData.TileDatas[_citytile.Coordinate.x, _citytile.Coordinate.y].TileSettle = City[_index];
      _NewMapData.TileDatas[_citytile.Coordinate.x, _citytile.Coordinate.y].Landmark = LandmarkType.City;
      City[_index].Tile = _citytile;
      _settlementcompeleteeindex++;

      bool cityCheck(TileData tile)
      {
        if (tile.BottomEnvir !=BottomEnvirType.Land) return false;
        if (tile.TopEnvir !=TopEnvirType.NULL) return false;
        if (tile.TileSettle != null) return false;
        return true;
      }
    }

    #endregion
    //   Debug.Log("성채 생성 완료");

    #region 마을
    LoopCount = 0;
    _settlementcompeleteeindex = 0;
    _enablelines.Clear();

    TileData _towntile = null;
    List<Settlement> Town = new List<Settlement> { new Settlement(SettlementType.Town), new Settlement(SettlementType.Town), new Settlement(SettlementType.Town) };
    List<HexDir> _towndir = Random.Range(0, 2) == 0 ?
      new List<HexDir>() { (HexDir)0, (HexDir)2, (HexDir)4}:
      new List<HexDir>() { (HexDir)1, (HexDir)3, (HexDir)5 };
    List<HexDir> _villagedirs = new List<HexDir>();
    foreach(var _dir in _towndir)
      _villagedirs.Add(RotateDir(_dir,3));
    _enablelines.Add(_NewMapData.GetDirLines(_NewMapData.CenterTile, _towndir[0]));
    _enablelines.Add(_NewMapData.GetDirLines(_NewMapData.CenterTile, _towndir[1]));
    _enablelines.Add(_NewMapData.GetDirLines(_NewMapData.CenterTile, _towndir[2]));

    while (_settlementcompeleteeindex < 3)
    {
      LoopCount++;
      if (LoopCount > 100) { Debug.Log("마을 생성 중 무한루프"); return null; }
      int _index = _settlementcompeleteeindex;

      int _lengthmin = Mathf.FloorToInt(_enablelines[_index].Count * ConstValues.SettlementLength_Village);
      int _lengthmax = Mathf.FloorToInt(_enablelines[_index].Count * ConstValues.SettlementLength_Town); 
      
      Vector2Int _selectcoor = _enablelines[_index][Random.Range(_lengthmin, _lengthmax)].Coordinate+new Vector2Int(Random.Range(-1,2), Random.Range(-1, 2));
      _towntile = _NewMapData.TileDatas[_selectcoor.x, _selectcoor.y];
      if(towncheck(_towntile) ==false)continue;


      _NewMapData.TileDatas[_towntile.Coordinate.x, _towntile.Coordinate.y].TileSettle = Town[_index];
      _NewMapData.Tile(_towntile.Coordinate).Landmark = LandmarkType.Town;
      Town[_index].Tile = _towntile;
      _settlementcompeleteeindex++;

      bool towncheck(TileData tile)
      {
        if (tile.BottomEnvir != BottomEnvirType.Land) return false;
        if (tile.TopEnvir != TopEnvirType.NULL) return false;
        if (tile.TileSettle != null) return false;

        List<TileData> _aroundtiles = _NewMapData.GetAroundTile(tile, 3);
        foreach (var _tile in _aroundtiles) if (_tile.TileSettle != null) return false;

        return true;
      }
    }
    #endregion
    //      Debug.Log("도시 생성 완료");

    #region 촌락
    LoopCount = 0;
    _settlementcompeleteeindex = 0;
    _enablelines.Clear();

    List<Settlement> Villages = new List<Settlement> {  new Settlement(SettlementType.Village), new Settlement(SettlementType.Village), new Settlement(SettlementType.Village) };
    _enablelines.Add(_NewMapData.GetDirLines(_NewMapData.CenterTile, _villagedirs[0]));
    _enablelines.Add(_NewMapData.GetDirLines(_NewMapData.CenterTile, _villagedirs[1]));
    _enablelines.Add(_NewMapData.GetDirLines(_NewMapData.CenterTile, _villagedirs[2]));

    while (_settlementcompeleteeindex < 3)
    {
      LoopCount++;
      if (LoopCount > 100) { Debug.Log("촌락 생성 중 무한루프"); return null; }
      int _index = _settlementcompeleteeindex;

      int _lengthmin = Mathf.FloorToInt(_enablelines[_index].Count * ConstValues.SettlementLength_min);
      int _lengthmax = Mathf.FloorToInt(_enablelines[_index].Count * ConstValues.SettlementLength_Village);

      Vector2Int _selectcoor = _enablelines[_index][Random.Range(_lengthmin,_lengthmax)].Coordinate + new Vector2Int(Random.Range(-1, 2), Random.Range(-1, 2));
      TileData _villagetile = _NewMapData.Tile(_selectcoor);

      if (_villagetile.BottomEnvir != BottomEnvirType.Land) continue;
      if (_villagetile.TopEnvir != TopEnvirType.NULL) continue;
      if (_villagetile.BottomEnvir == BottomEnvirType.Sea) continue;

      List<TileData> _aroundtile = _NewMapData.GetAroundTile(_villagetile, 3);
      bool _breakable = false;
      foreach(var _tile in _aroundtile)
      {
        if (_tile.TileSettle!=null) { _breakable = true;break; }
      }
      if (_breakable) continue;

      _NewMapData.TileDatas[_villagetile.Coordinate.x, _villagetile.Coordinate.y].TileSettle = Villages[_index];
      _NewMapData.Tile(_villagetile.Coordinate).Landmark = LandmarkType.Village;
      Villages[_index].Tile = _villagetile;
      _settlementcompeleteeindex++;
    }

    #endregion
    //    Debug.Log("마을 생성 완료");


    #region 정착지 정보

    SetSettle(ref City);
    SetSettle(ref Town);
    SetSettle(ref Villages);

    for(int i = 0; i < City.Count; i++)
    {
      _NewMapData.Citys.Add(City[i]);
      _NewMapData.AllSettles.Add(City[i]);
    }
    for (int i = 0; i < Town.Count; i++)
    {
      _NewMapData.Towns.Add(Town[i]);
      _NewMapData.AllSettles.Add(Town[i]);
    }
    for (int i = 0; i < Villages.Count; i++)
    {
      _NewMapData.Villages.Add(Villages[i]);
      _NewMapData.AllSettles.Add(Villages[i]);
    }


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
    Vector3 _cellsize = Vector3.one*80.0f; Debug.Log("맵을 만든 레후~");
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
        GameObject _bottomtile = new GameObject(_bottomname, new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer),typeof(Image) });
        _bottomtile.tag = "Tile";
        _bottomtile.transform.SetParent(TileHolder_bottomenvir);
        _bottomtile.AddComponent<Onpointer_tileoutline>().MyMapUI = MapUIScript;
        _bottomtile.GetComponent<Onpointer_tileoutline>().MyTile = GameManager.Instance.MyGameData.MyMapData.Tile(_coordinate);
        _bottomtile.GetComponent<Image>().sprite = GameManager.Instance.ImageHolder.Transparent;
        _bottomtile.GetComponent<Image>().raycastTarget = false;
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
        Button _button = _bottomtile.AddComponent<Button>();
        Navigation _nav = new Navigation();
        _nav.mode = Navigation.Mode.None;
        _button.navigation = _nav;
        _bottomtile.AddComponent(typeof(TileObjScript));
        _bottomtile.GetComponent<Image>().raycastTarget = GameManager.Instance.MyGameData.MyMapData.Tile(_coordinate).Interactable;
        _button.interactable = _currenttile.Fogstate==2;
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

        string _fogname = $"{j},{i} Fog";
        GameObject _fogtile = new GameObject(_fogname, new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image),typeof(CanvasGroup) });
        _fogtile.transform.SetParent(TileHolder_Fog);
        RectTransform _fogrect = _fogtile.GetComponent<RectTransform>();
        _fogrect.sizeDelta = _cellsize;
        _fogrect.position = new Vector3(_pos.x, _pos.y, _fogrect.position.z);
        _fogrect.transform.localScale = Vector3.one;
        _fogrect.anchoredPosition3D = new Vector3(_fogrect.anchoredPosition3D.x, _fogrect.anchoredPosition3D.y, 0.0f);
        Image _fogimage = _fogtile.GetComponent<Image>();
        _fogimage.raycastTarget = false;
        _fogimage.sprite = MyTiles.Fog;

        string _eventname= $"{j},{i} EventMark";
        GameObject _eventmark = new GameObject(_eventname, new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
        _eventmark.transform.SetParent(TileHolder_EventMark);
        RectTransform _eventrect = _eventmark.GetComponent<RectTransform>();
        _eventrect.sizeDelta = _cellsize;
        _eventrect.position = new Vector3(_pos.x, _pos.y, _eventrect.position.z);
        _eventrect.transform.localScale = Vector3.one;
        _eventrect.anchoredPosition3D = new Vector3(_eventrect.anchoredPosition3D.x, _eventrect.anchoredPosition3D.y, 0.0f);
        Image _eventimage = _eventmark.GetComponent<Image>();
        _eventimage.raycastTarget = false;
        _eventimage.sprite = GameManager.Instance.ImageHolder.UnknownEvent;
        _eventimage.enabled = _currenttile.IsEvent;

        TileObjScript _tilescript = _bottomtile.GetComponent<TileObjScript>();
        _tilescript.Rect = _bottomrect;
        _tilescript.Button = _button;
        _tilescript.MapUI = MapUIScript;
        _tilescript.TileData = GameManager.Instance.MyGameData.MyMapData.Tile(_coordinate);
        _tilescript.BottomImage = _bottomimage;
        _tilescript.TopImage = _topimage;
        _tilescript.LandmarkImage= _landmarkimage;
        _tilescript.OnPointer = _bottomtile.GetComponent<Onpointer_tileoutline>();
        _button.onClick.AddListener(() => _tilescript.Clicked());
        PreviewInteractive _tilepreview = _tilescript.Rect.transform.AddComponent<PreviewInteractive>();
        _tilepreview.PanelType = PreviewPanelType.TileInfo;
        _tilepreview.MyTileData = _currenttile;
        _tilescript.Preview= _tilepreview;
        _tilepreview.enabled = true;
        GameObject _previewpos_bottom = new GameObject("_posholder", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer) });
        _tilepreview.OtherRect = _previewpos_bottom.GetComponent<RectTransform>();
        _previewpos_bottom.transform.localScale = Vector3.zero;
        _previewpos_bottom.transform.SetParent(_bottomtile.transform);
        _previewpos_bottom.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f,-_cellsize.y / 2.0f);
        GameObject _previewpos_top = new GameObject("_posholder", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer) });
        _tilepreview.OtherRect_other = _previewpos_top.GetComponent<RectTransform>();
        _previewpos_top.transform.localScale = Vector3.zero;
        _previewpos_top.transform.SetParent(_bottomtile.transform);
        _previewpos_top.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, _cellsize.y / 2.0f);
        _tilescript.FogGroup = _fogtile.GetComponent<CanvasGroup>();
        _tilescript.FogGroup.alpha = _currenttile.Fogstate == 0 ? 1.0f : _currenttile.Fogstate == 1 ? ConstValues.FogAlpha_visible : 0.0f;
        _tilescript.EventMarkImage = _eventimage;

        GameManager.Instance.MyGameData.MyMapData.Tile(_coordinate).ButtonScript = _tilescript;
      }
    }
    //이 밑은 정착지를 버튼으로 만드는거

    Vector2 _settlementpos = Vector2.zero;
    Vector2 _outlinesize = _cellsize * 1.2f;
    Color _outlinecolor = new Color(0.5f, 0.0f, 1.0f, 1.0f);
    for (int i = 0; i < GameManager.Instance.MyGameData.MyMapData.Villages.Count; i++)
    {
      Settlement _village = GameManager.Instance.MyGameData.MyMapData.Villages[i];
      _settlementpos = _village.Tile.ButtonScript.Rect.anchoredPosition;

      string _villagename = "village_" + i.ToString();
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

    for (int i = 0; i < GameManager.Instance.MyGameData.MyMapData.Towns.Count; i++)
    {
      Settlement _town = GameManager.Instance.MyGameData.MyMapData.Towns[i];
      _settlementpos = _town.Tile.ButtonScript.Rect.anchoredPosition;

      string _townname = "town_" + i.ToString();
      GameObject _townholder = new GameObject(_townname, new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(CanvasGroup) });
      _townholder.transform.SetParent(TileHolder_landmark);
      _townholder.GetComponent<RectTransform>().anchoredPosition = _settlementpos;
      _townholder.GetComponent<CanvasGroup>().blocksRaycasts = false;
      _townholder.transform.localScale = Vector3.one;

      _town.Tile.ButtonScript.LandmarkImage.transform.SetParent(_townholder.transform);
      _town.Tile.ButtonScript.Rect.localScale = Vector3.one;

      MapUIScript.TownIcons.Add(_townholder);

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
    }

    for (int i = 0; i < GameManager.Instance.MyGameData.MyMapData.Citys.Count; i++)
    {
      Settlement _city = GameManager.Instance.MyGameData.MyMapData.Citys[i];
      _settlementpos = _city.Tile.ButtonScript.Rect.anchoredPosition;

      string _cityname = "city_" + i.ToString();
      GameObject _cityholder = new GameObject(_cityname, new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(CanvasGroup) });
      _cityholder.transform.SetParent(TileHolder_landmark);
      _cityholder.GetComponent<RectTransform>().anchoredPosition = _settlementpos;
      _cityholder.GetComponent<CanvasGroup>().blocksRaycasts = false;
      _cityholder.transform.localScale = Vector3.one;

      _city.Tile.ButtonScript.LandmarkImage.transform.SetParent(_cityholder.transform);
      _city.Tile.ButtonScript.Rect.localScale = Vector3.one;

      MapUIScript.CityIcons.Add(_cityholder);

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
      _cityoutlineimage.rectTransform.anchoredPosition = Vector3.zero;
      _cityoutline.transform.localScale = Vector3.one;
      _city.Tile.ButtonScript.DiscomfortOutline = _cityoutlinegroup;
    }
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
    q = vector2.x - (vector2.y - (vector2.y & 1)) / 2;
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
  public override int GetHashCode()
  {
    return base.GetHashCode();
  }
  public override bool Equals(object obj)
  {
    return base.Equals(obj);
  }
  public static HexGrid operator +(HexGrid h0, HexGrid h1)
  {
    return new HexGrid(h0.q + h1.q, h0.r + h1.r, h0.s + h1.s);
  }
  public static HexGrid operator -(HexGrid h0, HexGrid h1)
  {
    return new HexGrid(h0.q - h1.q, h0.r - h1.r, h0.s - h1.s);
  }
  public static bool operator ==(HexGrid h0, HexGrid h1)
  {
    return h0.q.Equals(h1.q) && h0.r.Equals(h1.r) && h0.s.Equals(h1.s);
  }
  public static bool operator !=(HexGrid h0, HexGrid h1)
  {
    return h0.q.Equals(h1.q) || h0.r.Equals(h1.r) || h0.s.Equals(h1.s);
  }
  private List<HexDir> GetDirectRoute()
  {
    List<HexDir> _list = new List<HexDir>();
    HexGrid _current = new HexGrid(q, r, s);
    HexGrid _temp = new HexGrid();
    int _value = 100;
    int _sign = 0;
    HexGrid _dirgrid = new HexGrid();

    while (_value != 0)
    {
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
    bool _isoverlap = true;
    while (_isoverlap == true)
    {
      _isoverlap = false;

      if (_list.Contains((HexDir)0) && _list.Contains((HexDir)3))
      {
        _list.Remove((HexDir)0);
        _list.Remove((HexDir)3);
        _isoverlap = true;
      }
      if (_list.Contains((HexDir)1) && _list.Contains((HexDir)4))
      {
        _list.Remove((HexDir)1);
        _list.Remove((HexDir)4);
        _isoverlap = true;
      }
      if (_list.Contains((HexDir)2) && _list.Contains((HexDir)5))
      {
        _list.Remove((HexDir)2);
        _list.Remove((HexDir)5);
        _isoverlap = true;
      }

      if (_list.Contains((HexDir)1) && _list.Contains((HexDir)5))
      {
        _list.Remove((HexDir)1);
        _list.Remove((HexDir)5);

        _list.Add((HexDir)0);
        _isoverlap = true;
      }
      if (_list.Contains((HexDir)0) && _list.Contains((HexDir)2))
      {
        _list.Remove((HexDir)0);
        _list.Remove((HexDir)2);

        _list.Add((HexDir)1);
        _isoverlap = true;
      }
      if (_list.Contains((HexDir)1) && _list.Contains((HexDir)3))
      {
        _list.Remove((HexDir)1);
        _list.Remove((HexDir)3);

        _list.Add((HexDir)2);
        _isoverlap = true;
      }
      if (_list.Contains((HexDir)2) && _list.Contains((HexDir)4))
      {
        _list.Remove((HexDir)2);
        _list.Remove((HexDir)4);

        _list.Add((HexDir)3);
        _isoverlap = true;
      }
      if (_list.Contains((HexDir)3) && _list.Contains((HexDir)5))
      {
        _list.Remove((HexDir)3);
        _list.Remove((HexDir)5);

        _list.Add((HexDir)4);
        _isoverlap = true;
      }
      if (_list.Contains((HexDir)0) && _list.Contains((HexDir)4))
      {
        _list.Remove((HexDir)0);
        _list.Remove((HexDir)4);

        _list.Add((HexDir)5);
        _isoverlap = true;
      }
    }

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
  }
  public List<HexDir> GetDirectRoute(TileData starttile)
  {
    HexGrid _current = new HexGrid(q-starttile.HexGrid.q,r - starttile.HexGrid.r, s - starttile.HexGrid.s);
    return _current.GetDirectRoute();
  }
  public List<HexDir> GetDirectRoute(HexGrid starthex)
  {
    HexGrid _current = new HexGrid(q- starthex.q, r- starthex.r, s- starthex.s);
    return _current.GetDirectRoute();
  }
  public int GetDistance(HexGrid starthex)
  {
    HexGrid _newhex = new HexGrid(q, r, s) - starthex;
    return Mathf.Max(Mathf.Abs(_newhex.q), Mathf.Abs(_newhex.r), Mathf.Abs(_newhex.s));
  }
  public int GetDistance(TileData starttile)
  {
    HexGrid _newhex = new HexGrid(q, r, s) - starttile.HexGrid;
    return Mathf.Max(Mathf.Abs(_newhex.q), Mathf.Abs(_newhex.r), Mathf.Abs(_newhex.s));
  }
  public int GetDistance(Vector2Int startcoor)
  {
    HexGrid _newhex = new HexGrid(q, r, s) - new HexGrid(startcoor);
    return Mathf.Max(Mathf.Abs(_newhex.q), Mathf.Abs(_newhex.r), Mathf.Abs(_newhex.s));
  }
  public bool EnableLength(int length)
  {
    return q >= -length && q <= length && r >= -length && r <= length && s >= -length && s <= length;
  }
}