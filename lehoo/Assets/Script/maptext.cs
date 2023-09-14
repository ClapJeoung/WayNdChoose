using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;

public class maptext : MonoBehaviour
{
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
      MapData _data = MakeMap();
            bool _villageriver = false, _villageforest = false, _villagehighland = false, _villagemountain = false, _villagesea = false;
            bool _townriver = false, _townforest = false, _townhighland = false, _townmountain = false, _townsea = false;
            bool _cityriver = false, _cityforest = false, _cityhighland = false, _citymountain = false;

            foreach (var _village in _data.Villages)
            {
                if (_village.IsRiver) _villageriver = true;
                if (_village.IsForest) _villageforest = true;
                if (_village.IsHighland) _villagehighland = true;
                if (_village.IsMountain) _villagemountain = true;
                if (_village.IsSea) _villagesea = true;
            }
            foreach (var _town in _data.Towns)
            {
                if (_town.IsRiver) _townriver = true;
                if (_town.IsForest) _townforest = true;
                if (_town.IsHighland) _townhighland = true;
                if (_town.IsMountain) _townmountain = true;
                if (_town.IsSea) _townsea = true;
            }
      var _city = _data.City;
      if (_city.IsRiver) _cityriver = true;
      if (_city.IsForest) _cityforest = true;
      if (_city.IsHighland) _cityhighland = true;
      if (_city.IsMountain) _citymountain = true;

      if (!_villageriver||!_villageforest||!_villagemountain||!_villagesea||
               !_townriver || !_townforest  || !_townmountain || !_townsea ||
               !_cityriver || !_cityforest  || !_citymountain )
            {
                string _str = "";
                if (!_villageriver) _str += "���� ��  ";
                if (!_villageforest) _str += "���� ��  ";
              //  if (!_villagehighland) _str += "���� ���  ";
                if (!_villagemountain) _str += "���� ��  ";
                if (!_villagesea) _str += "���� �ٴ�  ";
                if (!_townriver) _str += "���� ��  ";
                if (!_townforest) _str += "���� ��  ";
              //  if (!_townhighland) _str += "���� ���  ";
                if (!_townmountain) _str += "���� ��  ";
                if (!_townsea) _str += "���� �ٴ�  ";
                if (!_cityriver) _str += "��ä ��  ";
                if (!_cityforest) _str += "��ä ��  ";
             //   if (!_cityhighland) _str += "��ä ���  ";
                if (!_citymountain) _str += "��ä ��  ";

                Debug.Log(_index+"�� ��    "+ _str + "����");
                yield return null;
                continue;
            }
      if (_data.Villages.Count != 3) { yield return null;continue; }
      if (_data.Towns.Count != 2) { yield return null; continue; }
      if (_data.City==null) {  yield return null; continue; }

      Debug.Log($"{_index}��° �� ����\n");
            GameManager.Instance.MyGameData.MyMapData = _data;
            break;
        }
    }
  /// <summary>
  /// range ������ŭ�� Ÿ�� ����(range �ּ� 0)
  /// </summary>
  /// <param name="range"></param>
  /// <returns></returns>
  public MapData MakeMap()
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

    #region �� �����
    //������ �߽�
    List<Vector2Int> _firstlandcoors = _NewMapData.GetAroundCoor(new List<Vector2Int> { _NewMapData.CenterTile.Coordinate }, ConstValues.LandSize / 2 + ConstValues.LandSize % 2);
    foreach (var _landcoor in _firstlandcoors)
    {
      _NewMapData.Tile(_landcoor).BottomEnvir = BottomEnvirType.Land;
      _NewMapData.Tile(_landcoor).BottomEnvirSprite = TileSpriteType.Land;
    }
    #endregion

    #region �ٴ�,�ؾȼ�

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
    //������ ������ �´�� �ִ� �ٴ� Ÿ�ϵ� ��������
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
            _beaches[i].Rotate = j;
          }

          _seadirs[0] = RotateDir(_seadirs[0], -1);
          _seadirs[1]=RotateDir(_seadirs[1], -1);
        }
      }//�ٴ� ������ 2����, ���� ���� ���� ������ ��� beach_n�� �ƴ� beach_middle�� �Ҵ�
      else
      {
        for (int j = 0; j < 6; j++)
        {
          if (MaxDirIndex() == _seadirs.Count - 1)
          {
            _beaches[i].BottomEnvirSprite = MyTiles.GetBeachTile(_seadirs.Count);
            _beaches[i].Rotate = j;
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
    //     Debug.Log("�ٴ� ���� �Ϸ�");

    #region ��
    LoopCount = 0;

    RiverData[] _riverdatas = new RiverData[6];
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

      //  Debug.Log($"{i + 1}��° �� �߿��� {_sourcepos}���� {_maindir}�������� ����");
      while (true)
      {
        LoopCount++;
        if (LoopCount > 1000) { Debug.Log("�� ���� �� ���ѷ���"); return null; }

        if (_riverdata.RiverDirs.Count == 0) break;

        int _finishdir = 10;                                            //(0~5)
        List<int> _targetdirs = MixRandom(new List<int> { -1, 0, 1 });  //-1,0,1�� ��������
        List<int> _enabledirs = new List<int>();                          //Ȯ���� �����(-1,0,1)
        Vector2Int _currentcoor = _riverdata.RiverCoors[_riverdata.RiverCoors.Count - 1];  //���ư� Ÿ�� �߽���
        for (int k = 0; k < _targetdirs.Count; k++)
        {

          int _nextdir = RotateDir(_maindir, _targetdirs[k]);                //���� ���õ� ����(0~5)
          TileData _nexttile = _NewMapData.GetNextTile(_currentcoor, (HexDir)_nextdir);

          if (_nexttile.Coordinate.x == 0 || _nexttile.Coordinate.y == 0 || _nexttile.Coordinate.x == ConstValues.MapSize - 1 || _nexttile.Coordinate.y == ConstValues.MapSize - 1)
          {
            _enabledirs.Add(_targetdirs[k]);
            _finishdir = _nextdir;
          }
          if (_curvecount == -3 && _targetdirs[k] == -1) continue;
          if (_curvecount == 3 && _targetdirs[k] == 1) continue;//������ Ŀ�� ����
          if (_failcoors.Contains(_nexttile.Coordinate)) continue;
          bool _overlaped = false;
          for (int m = 0; m < i; m++)
          {
            if (_riverdatas[m].RiverCoors.Contains(_nexttile.Coordinate)) { _overlaped = true; break; }
          }
          if (_overlaped) continue;
          _enabledirs.Add(_targetdirs[k]);
          if (_nexttile.BottomEnvir == BottomEnvirType.Beach) _finishdir = _nextdir;
        }

        if (_enabledirs.Count == 0)
        {
          //   Debug.Log($"{i + 1}��° �� [{_riverdata.RiverCoors.Count+1}]{_currentcoor} ����, {_riverdata.RiverCoors.Count}������ ȸ��");
          _failcoors.Add(_currentcoor);
          _riverdata.RiverCoors.Remove(_currentcoor);
          _riverdata.RiverDirs.Remove(_riverdata.RiverDirs[_riverdata.RiverDirs.Count - 1]);
          continue;
        }//enabledir ������ 0�� ��� ���� ���� �Ұ� Ÿ���̹Ƿ� ��ǥ,������ 1���� ���� (���� Ÿ�� failcoor)

        if (_finishdir != 10)
        {
          if (_riverdata.RiverCoors.Count < ConstValues.MinRiverCount)
          {
            //   Debug.Log($"{i + 1}��° �� [{_riverdata.RiverCoors.Count + 1}]{_currentcoor}->({_finishdir})->[{_riverdata.RiverCoors.Count + 2}]{_NewMapData.GetNextCoor(_currentcoor, (HexDir)_finishdir)} ���� ������ �غ����� ���� {_riverdata.RiverCoors.Count+1}�� Ÿ�� �ٽ� �˻�");
            _failcoors.Add(_NewMapData.GetNextCoor(_currentcoor, (HexDir)_finishdir));
            continue;
          }//�غ��� ã�Ҵµ� �� ������ ä���� ���� ���(�ش� Ÿ�� failcoor)
          else
          {
            //   Debug.Log($"{i + 1}��° �� [{_riverdata.RiverCoors.Count + 1}]{_currentcoor} ->({_finishdir})-> [{_riverdata.RiverCoors.Count + 2}]{_NewMapData.GetNextCoor(_currentcoor, (HexDir)_finishdir)} �� ����");
            _riverdata.RiverCoors.Add(_NewMapData.GetNextCoor(_currentcoor, (HexDir)_finishdir));
            _riverdata.RiverDirs.Add(_finishdir);
            _iscomplete = true;
            break;
          }//�غ��� ã�Ұ� �� �ּ� ������ ����()
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
        }//enabledir ������ 1 �̻��� ��� ������ 1��
        Vector2Int _nextcoor = _NewMapData.GetNextCoor(_currentcoor, (HexDir)_selectdir);

        //  Debug.Log($"{i + 1}��° �� [{_riverdata.RiverCoors.Count + 1}]{_currentcoor} ->({_selectdir})-> [{_riverdata.RiverCoors.Count + 2}]{_nextcoor} ����");
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
          _NewMapData.Tile(_riverdatas[i].RiverCoors[j]).Rotate = RotateDir(i, 3);

        }//�߿���
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
              _riverbeach.Rotate = k + 1;
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

        }//�غ�
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
              _rivertile.Rotate = k;
              _rivertile.BottomEnvirSprite = MyTiles.GetRiver(_max);
            }

            _combineddirs[0] = RotateDir(_combineddirs[0], -1);
            _combineddirs[1] = RotateDir(_combineddirs[1], -1);
          }
        }//�ٱ�
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
    //    Debug.Log("�� ���� �Ϸ�");

    #region �縷
    /*
      Vector3Int _desertorigin = Vector3Int.zero;                //�縷 ������
      List<int> _lessdir = new List<int>(); //���� ���� ����
      if (_seacount == 2)                                        //�ٴ� 2���̶�� �������̿� ����
      {
          if (_seaside[1] == true) { _desertorigin.x = 0; _lessdir.Add(1); }
          else if (_seaside[3] == true) { _desertorigin.x = DefaultSize - 1; _lessdir.Add(3); }
          if (_seaside[0] == true) { _desertorigin.y = 0; _lessdir.Add(0); }
          else if (_seaside[2] == true) { _desertorigin.y = DefaultSize - 1; _lessdir.Add(2); }
      }
      else                                                       //�ٴ� 3���̶�� ������ �Ѱ�� ���ڶ��� ����
          for (int i = 0; i < 4; i++)
          {
              if (_seaside[i] == true) continue;
              if (i == 0) { _lessdir.Add(2); _desertorigin = new Vector3Int(6, 12, 0); break; }
              else if (i == 1) { _lessdir.Add(3); _desertorigin = new Vector3Int(12, 6, 0); break; }
              else if (i == 2) { _lessdir.Add(0); _desertorigin = new Vector3Int(6, 0, 0); break; }
              else if (i == 3) { _lessdir.Add(1); _desertorigin = new Vector3Int(0, 6, 0); break; }
          }

      MapData_b[_desertorigin.x, _desertorigin.y] = MapCode.b_desert;           //�縷 �������� �縷 Ÿ�� ��ġ

      int _desertcount_max = Mathf.CeilToInt((float)DefaultSize * (float)DefaultSize * Ratio_desert);   //�縷 �ִ� ����(����)
      List<Vector3Int> _desertpos = new List<Vector3Int>();       //��ü �縷 ��ġ
      List<Vector3Int> _newdesertpos = new List<Vector3Int>();    //�ֱ� ���� ���� �縷 ��ġ
      _desertpos.Add(_desertorigin); _newdesertpos.Add(_desertorigin); //���� �縷 ����Ʈ, �ֱ� �縷 ����Ʈ�� �縷 ���� �Է�
      while (true)    //�ߴ� ������ ���ο�
      {
          List<Vector3Int> _newdesert_temp = new List<Vector3Int>();   //�߰��Ǳ� ���� �縷 ���
          List<int> _dirpool = new List<int>();
          for (int i = 0; i < 6; i++)
          {
              _dirpool.Add(i);
              _dirpool.Add(i);
              _dirpool.Add(i);
              if (_lessdir.Contains(i)) continue;
              _dirpool.Add(i);
          }
          for (int i = 0; i < _newdesertpos.Count; i++)   //���� ���� �縷 �ϳ��� Ȯ�� ����
          {
              int[] _dir = new int[3] { -1, -1, -1 }; //������ ���� ������ ����
              int _index = 0, _temp = 0;
              while (_index < 2)                        //���� ���� �縷���� ���ư� ���� 3�� ����
              {
                  _temp = _dirpool[Random.Range(0, _dirpool.Count)];
                  if (_dir.Contains(_temp)) continue;     //��ġ�� �����̸� �ٽ�
                  _dir[_index] = _temp;
                  _index++;
              }                                       //������ ���� 3�� �Ϸ�
              if (_desertpos.Count == 1)//ù �縷 ������ ����
              {
                  if (_seacount == 2)
                  {
                      if (_seaside[0] == true)
                      {
                          if (_seaside[1] == true)    //��, ������
                          {
                              _dir[0] = 0; _dir[1] = 1; _dir[2] = 3;
                          }
                          else if (_seaside[3] == true)//��, ����
                          {
                              _dir[0] = 0; _dir[1] = 4; _dir[2] = 5;
                          }
                      }
                      else if (_seaside[2] == true)
                      {
                          if (_seaside[1] == true)    //�Ʒ�, ������
                          {
                              _dir[0] = 0; _dir[1] = 1; _dir[2] = 2;
                          }
                          else if (_seaside[3] == true)//�Ʒ�, ����
                          {
                              _dir[0] = 2; _dir[1] = 3; _dir[2] = 4;
                          }
                      }
                  }
                  else                                                       //�ٴ� 3���̶�� ������ �Ѱ�� ���ڶ��� ����
                  {
                      if (_seaside[0] == false) { _dir[0] = 4; _dir[1] = 2; _dir[2] = 3; }
                      else if (_seaside[1] == false) { _dir[0] = 3; _dir[1] = 4; _dir[2] = 5; }
                      else if (_seaside[2] == false) { _dir[0] = 0; _dir[1] = 4; _dir[2] = 5; }
                      else if (_seaside[3] == false) { _dir[0] = 0; _dir[1] = 1; _dir[2] = 2; }
                  }

              }
              for (int j = 0; j < _dir.Length; j++)    //�ش� ���� 3���� �縷 3�� ����(������ ����)
              {
                  Vector3Int _newpos = GetNextPos(_newdesertpos[i], _dir[j]); //���� ���� Ÿ�Ͽ��� 1ĭ ���ư� ��ǥ
                  if (!CheckInside(_newpos)) continue;                 //���� ����� ���
                  if (MapData_b[_newpos.x, _newpos.y] == MapCode.b_sea) continue; //�� ��ǥ�� �ٴٰ� �ְų�
                  if (_desertpos.Contains(_newpos)) continue;         //�� ��ǥ�� �縷�� �ְų�
                  if (_newdesert_temp.Contains(_newpos)) continue;    //�� ��ǥ�� �縷(����)�� ������ ���
                  _newdesert_temp.Add(_newpos);                       //�� �ɸ��°� ������ �縷(����)�� �߰�
                                                                      //���� ���� ������ŭ �ݺ�
              }
              //�縷 Ÿ�� �ϳ��κ��� ������� �縷(�ִ� 3��) �Է� �Ϸ�
          }
          //    Debug.Log($"{_desertpos.Count + _newdesert_temp.Count}     {_desertcount_max}");
          if (_desertpos.Count + _newdesert_temp.Count >= _desertcount_max)
          {
              break;
          }
          //���� �縷 + ���� �縷 ������ �ִ� �縷 ���� �ʰ��ϸ� ����
          if (_newdesert_temp.Count == 0) break;
          //�ű� ���� �縷�� 0����� ����

          _newdesertpos.Clear();  //�ű� �縷 ����Ʈ �ʱ�ȭ
          for (int i = 0; i < _newdesert_temp.Count; i++)
          {
              _desertpos.Add(_newdesert_temp[i]); _newdesertpos.Add(_newdesert_temp[i]);
          }
          //��� ���� ���ζ�� �縷 ���� ��ǥ�� �縷 Ȯ�� ����Ʈ�� ����
          //�ű� �縷 ����Ʈ�� ���ο� �����ͷ� ������
      }

      for(int i = 0; i < _desertpos.Count; i++)
      {
          int _code = MapData_b[_desertpos[i].x, _desertpos[i].y];
          if (_code == MapCode.b_beach) MapData_b[_desertpos[i].x, _desertpos[i].y] = MapCode.b_desert_beach;
          else MapData_b[_desertpos[i].x, _desertpos[i].y] = MapCode.b_desert;
      }//�ش� ��ġ �縷/�縷 �غ����� ��ȯ
    */
    #endregion
    //   Debug.Log("�縷 ���� �Ϸ�");  
    #region ��
    LoopCount = 0;

    List<Vector2Int> _newmountain = new List<Vector2Int>(); //�� ��ġ ����Ʈ
    int[] _mountaindirs = Random.Range(0, 2) == 0 ? new int[3] { 1, 3, 5 } : new int[3] { 0, 2, 4 };

    while (_newmountain.Count != 9)
    {
      LoopCount++;
      if (LoopCount > 1000) { Debug.Log("�� ���� �� ���ѷ��� "+_newmountain.Count); return null; }

      int _index = _newmountain.Count / 3;
      List<TileData> _lines= _NewMapData.GetDirLines(_NewMapData.CenterTile, (HexDir)_mountaindirs[_index]);
      int _mintileindex = 2, _maxtileindex = _lines.Count - 1;
      Vector2Int _mountain_0=Vector2Int.zero,_mountain_1 = Vector2Int.zero, _mountain_2=Vector2Int.zero;
      int _mountaindir_1 = 2, _mountaindir_2 = 3;
      while (true)
      {
        LoopCount++;
        if (LoopCount > 1000) { Debug.Log("�� ���� �� ���ѷ���");return null; }

        _mountain_0 = _lines[Random.Range(_mintileindex, _maxtileindex)].Coordinate
          +new Vector2Int(Random.Range(-1,2), Random.Range(-1, 2));
        _mountain_1 = _NewMapData.GetNextTile(_mountain_0, (HexDir)_mountaindir_1).Coordinate;
        _mountain_2 = _NewMapData.GetNextTile(_mountain_0, (HexDir)_mountaindir_2).Coordinate;

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

    //     Debug.Log("�� ���� �Ϸ�");

    #region ���
    /*
    LoopCount = 0;

    int _highlandmaxcount = Mathf.CeilToInt((float)ConstValues.LandSize * (float)ConstValues.LandSize * ConstValues.Ratio_highland);//��� �ִ� ����
    int _maxaver = _highlandmaxcount / 4;        //��� �ٱ� ��� �ִ� ����  +-2 �����Ұ�
    List<Vector2Int> _highlands = new List<Vector2Int>();   //��� ���(�縷 ������� ��ȯ ��)
    while (_highlands.Count < _highlandmaxcount)
    {
      LoopCount++;
      if (LoopCount > 1000) { Debug.Log("��� ���� �� ���ѷ���"); return null; }

      List<Vector2Int> _currenthigh = new List<Vector2Int>(); //���� ��� ��ǥ ����Ʈ
      int _currentmax = _maxaver + Random.Range(-2, 3);       //���� ��� �ٱ� �ִ� ����
      if (_highlandmaxcount - _highlands.Count < _currentmax) _currentmax = _highlandmaxcount - _highlands.Count;
      //�̹� �ִ밪���� ��ģ ���� ������ ����� ��

      List<int> _dirlist = new List<int>();                   //���� ��� ���� ����Ʈ
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
      _dirlist.Add(_dir);                                     //���� ���� ���� ���� �Ϸ�

      List<int> _faildir = new List<int>();//�� ĭ���� ������ ����(6�� �����ϸ� �ٱ� ��)
      Vector2Int _currentpos = new Vector2Int(Random.Range(0, ConstValues.MapSize), Random.Range(0, ConstValues.MapSize));   //���� ��� ��ġ
      Vector2Int _nextpos = new Vector2Int();                 //���ư� ��� ��ġ
      while (true)
      {
        LoopCount++;
        if (LoopCount > 1000) { Debug.Log("��� ���� �� ���ѷ���"); return null; }

        if (_faildir.Count == 6) break;                 //�� Ÿ�Ͽ��� 6���� ���� �����ϰų�
        if (_currenthigh.Count >= _currentmax) break;   //��ǥ ���� �޼��ϰų�

        int _currentdir = _dirlist[Random.Range(0, _dirlist.Count)];//������ ���� ����
        _nextpos = GetNextPos(_currentpos, _currentdir);            //Ÿ�� ���ư���
        if (!highcheck(_nextpos))
        {
          if (!_faildir.Contains(_currentdir)) _faildir.Add(_currentdir); //���� ���� ����Ʈ�� �ֱ�(������)
          continue;
        }                                //�� �ȿ� �ִ���, ���̳� ����̶� ��ġ����

        if (_faildir.Count > 0) _faildir.Clear();                      //�����ϸ� ���� ī��Ʈ �ʱ�ȭ

        _currenthigh.Add(_nextpos);                                 //���� ��� ����Ʈ�� �߰�
        _currentpos = _nextpos;                                     //���� ��ǥ ���� ��ǥ�� ����

        Vector2Int _aroundpos = new Vector2Int();
        for (int i = 0; i < 6; i++)
        {
          _aroundpos = GetNextPos(_currentpos, i);
          if (_NewMapData.TileDatas[_aroundpos.x, _aroundpos.y].TopEnvir==TopEnvirType.Mountain) break;
          //�ֺ� 1ĭ�� ���� ������ ������ �ٷ� ����
        }//�ֺ� 1ĭ �� �˻�
      }//��� �ٱ� �������

      foreach (Vector2Int _pos in _currenthigh)
      {
        _highlands.Add(_pos);                                       //��� ��ü ����Ʈ�� �߰�
        _NewMapData.TileDatas[_pos.x,_pos.y].TopEnvir=TopEnvirType.Highland;
        _NewMapData.Tile(_pos).TopEnvirSprite = TileSpriteType.HighLand;
      }//Ÿ�� ���� �Է�

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
      }//�߿���,��,�ٴ�,���,���̸� False �ƴϸ� True
    }
    */
    #endregion
    //     Debug.Log("��� ���� �Ϸ�");

    #region ��
    LoopCount = 0;

    int _forestmaxcount = Mathf.CeilToInt((float)ConstValues.LandSize * (float)ConstValues.LandSize * ConstValues.Ratio_forest);    //�� �ִ� ����
    int _forestcountaver = _forestmaxcount / 4; //�� ���� ��� �ִ밳��
    List<Vector2Int> _forests = new List<Vector2Int>();         //�� �Ϸ�� �� ��ǥ�� ������ ����Ʈ

    List<Vector2Int> _forestbundle_origin=new List<Vector2Int>();//(10,10)�� �߽����� ���������� ����
    _forestbundle_origin = _NewMapData.GetAroundCoor(Vector2Int.one * 10, 5);

    int _forestmodifycount = (_forestbundle_origin.Count / 3) * 2;

    List<TileData> _emptylandtiles = _NewMapData.GetEnvirTiles
    (new List<BottomEnvirType> { BottomEnvirType.Land, BottomEnvirType.Source, BottomEnvirType.River }, new List<TopEnvirType> { TopEnvirType.Mountain, TopEnvirType.Highland, TopEnvirType.Forest }, 1);

    while (_forests.Count < _forestmaxcount)
    {
      LoopCount++;
      if (LoopCount > 1000) { Debug.Log("�� ���� �� ���ѷ���"); return null; }

      List<Vector2Int> _forestbundle=new List<Vector2Int>();
      foreach (var _coor in _forestbundle_origin) _forestbundle.Add(_coor);//origin(10,10)�� ������ _forestbundle(10,10)

      List<Vector2Int> _deletebundles=new List<Vector2Int>();              //�� �������� ���� ��ġ(10,10)
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
      //�̰Ŏ� ���� ������ �뼺�뼺 �� �ϼ�((10,10)����)
      
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

    }//�� ���� ä��� ������ �ݺ�
     //  Debug.Log($"�� �ִ� ���� : {_forestmaxcount}  ���� �� ���� : {_forests.Count}");

    #endregion
    //   Debug.Log("�� ���� �Ϸ�");
    #region ����
    LoopCount = 0;

    Settlement City = new Settlement(SettlementType.City);
    List<TileData> _citytiles = new List<TileData>();
    List<TileData> _citystartrange = _NewMapData.GetAroundTile(new Vector2Int(ConstValues.MapSize / 2 + ConstValues.MapSize % 2, ConstValues.MapSize / 2 + ConstValues.MapSize % 2), 1);
    HexDir _citydir_1=HexDir.BottomRight, _citydir_2=HexDir.BottomLeft;

    while (_citytiles.Count != 3)
    {
      LoopCount++;
      if (LoopCount > 1000) { Debug.Log("���� ���� �� ���ѷ���"); return null; }

      Vector2Int _starcoor = _citystartrange[Random.Range(0, _citystartrange.Count - 1)].Coordinate+new Vector2Int(Random.Range(-1,2), Random.Range(-1, 2));
     
      TileData _starttile = _NewMapData.TileDatas[_starcoor.x, _starcoor.y];
      if (cityCheck(_starttile) == false) continue;

      TileData _secondtile = _NewMapData.GetNextTile(_starttile, _citydir_1);
      if (cityCheck(_secondtile) == false) continue;

      TileData _thirdtile = _NewMapData.GetNextTile(_starttile, _citydir_2);
      if (cityCheck(_thirdtile) == false) continue;

      _NewMapData.TileDatas[_starttile.Coordinate.x, _starttile.Coordinate.y].TileSettle = City;
      _NewMapData.TileDatas[_secondtile.Coordinate.x, _secondtile.Coordinate.y].TileSettle = City;
      _NewMapData.TileDatas[_thirdtile.Coordinate.x, _thirdtile.Coordinate.y].TileSettle = City;
      _citytiles.Add(_starttile);
      _citytiles.Add(_secondtile);
      _citytiles.Add(_thirdtile);
      _NewMapData.TileDatas[_starttile.Coordinate.x, _starttile.Coordinate.y].Landmark = LandmarkType.City;
      _NewMapData.TileDatas[_secondtile.Coordinate.x, _secondtile.Coordinate.y].Landmark = LandmarkType.City;
      _NewMapData.TileDatas[_thirdtile.Coordinate.x, _thirdtile.Coordinate.y].Landmark = LandmarkType.City;
      bool cityCheck(TileData tile)
      {
        if (tile.BottomEnvir == BottomEnvirType.Sea) return false;
        if (tile.TopEnvir == TopEnvirType.Mountain) return false;
        if (tile.TileSettle != null) return false;
        return true;
      }
    }
    for(int i = 0; i < _citytiles.Count; i++)
    {
      City.Tiles.Add(_citytiles[i]);
    }

    #endregion
    //   Debug.Log("��ä ���� �Ϸ�");

    #region ����
    LoopCount = 0;

    List<TileData> _towntiles=new List<TileData>();
    List<Settlement> Towns=new List<Settlement>();
    Towns.Add(new Settlement(SettlementType.Town)); Towns.Add(new Settlement(SettlementType.Town));
    List<HexDir> _towndirs=new List<HexDir>();
    List<HexDir> _villagedirs = new List<HexDir>() { (HexDir)0, (HexDir)1, (HexDir)2, (HexDir)3, (HexDir)4, (HexDir)5 };
    switch (Random.Range(0, 3))
    {
      case 0:
        _towndirs =new List<HexDir>{ (HexDir)0,(HexDir)3};
        break;
      case 1:
        _towndirs = new List<HexDir> { (HexDir)1, (HexDir)4 };
        break;
      case 2:
        _towndirs = new List<HexDir> { (HexDir)2, (HexDir)5 };
        break;
    }
    foreach (HexDir _hexdir in _towndirs) _villagedirs.Remove(_hexdir);
    _villagedirs.Remove(_villagedirs[Random.Range(0, _villagedirs.Count)]);
    List<List<TileData>> _enablelines = new List<List<TileData>>();
    _enablelines.Add(_NewMapData.GetDirLines(_NewMapData.CenterTile, _towndirs[0]));
    _enablelines.Add(_NewMapData.GetDirLines(_NewMapData.CenterTile, _towndirs[1]));
    List<TileData> _disabletiles=new List<TileData>();

    while (_towntiles.Count != 2*2)
    {
      LoopCount++;
      if (LoopCount > 1000) { Debug.Log("���� ���� �� ���ѷ���"); return null; }
      int _index = _towntiles.Count / 2;

      Vector2Int _selectcoor = _enablelines[_index][Random.Range(1, _enablelines.Count - 1)].Coordinate+new Vector2Int(Random.Range(-1,2), Random.Range(-1, 2));
      TileData _firsttile = _NewMapData.TileDatas[_selectcoor.x, _selectcoor.y];
      if(towncheck(_firsttile)==false)continue;

      int _loopcount = 0;
      TileData _secondtile = null;
      while (_loopcount < 6)
      {
        _secondtile = _NewMapData.GetNextTile(_firsttile, (HexDir)Random.Range(0, 6));
        if (towncheck(_secondtile) == true) break;
        _loopcount++;
      }
      if (_loopcount == 6) continue;

      _NewMapData.TileDatas[_firsttile.Coordinate.x, _firsttile.Coordinate.y].TileSettle = Towns[_towntiles.Count / 2];
      _NewMapData.TileDatas[_secondtile.Coordinate.x, _secondtile.Coordinate.y].TileSettle = Towns[_towntiles.Count / 2];
      _NewMapData.Tile(_firsttile.Coordinate).Landmark = LandmarkType.Town;
      _NewMapData.Tile(_secondtile.Coordinate).Landmark = LandmarkType.Town;
      _towntiles.Add(_firsttile);
      _towntiles.Add(_secondtile);

      bool towncheck(TileData tile)
      {
        if (tile.TopEnvir == TopEnvirType.Mountain) return false;
        if (tile.BottomEnvir == BottomEnvirType.Sea) return false;
        if (tile.TileSettle != null) return false;
        if(_towntiles.Contains(tile)) return false;

        List<TileData> _aroundtiles = _NewMapData.GetAroundTile(tile, 1);
        foreach (var _tile in _aroundtiles) if (_tile.TileSettle != null) return false;

        return true;
      }
    }
    for(int i=0;i<_towntiles.Count; i++)
    {
      Towns[i/2].Tiles.Add(_towntiles[i]);
    }
    #endregion
    //      Debug.Log("���� ���� �Ϸ�");

    #region �̶�
    LoopCount = 0;

    List<Settlement> Villages = new List<Settlement> { new Settlement(SettlementType.Town), new Settlement(SettlementType.Town), new Settlement(SettlementType.Town) };
    List<TileData> _villagetiles= new List<TileData>();
    while (_villagetiles.Count < 3)
    {
      LoopCount++;
      if (LoopCount > 1000) { Debug.Log("�̶� ���� �� ���ѷ���"); return null; }

      List<TileData> _lines = _NewMapData.GetDirLines(_NewMapData.CenterTile, _villagedirs[_villagetiles.Count]);

      Vector2Int _selectcoor = _lines[Random.Range(1, _lines.Count-1)].Coordinate + new Vector2Int(Random.Range(-2, 3), Random.Range(-1, 2));
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
      if (_breakable) continue;//���� 2ĭ�� �ٸ� �������� ������ X

      _NewMapData.TileDatas[_villagetile.Coordinate.x, _villagetile.Coordinate.y].TileSettle = Villages[_villagetiles.Count];
      _NewMapData.Tile(_villagetile.Coordinate).Landmark = LandmarkType.Village;
      _villagetiles.Add(_villagetile);
    }

    for(int i = 0; i < _villagetiles.Count; i++)
    {
      Villages[i].Tiles.Add(_villagetiles[i]);
    }
    #endregion
    //    Debug.Log("���� ���� �Ϸ�");


    #region ������ ����

    List<Settlement> _citylisttemp = new List<Settlement>() { City };
    SetSettle(ref _citylisttemp);
    SetSettle(ref Towns);
    SetSettle(ref Villages);

    _NewMapData.City = _citylisttemp[0];
    _NewMapData.Towns = Towns;
    _NewMapData.Villages=Villages;

    _NewMapData.AllSettles.Add(_citylisttemp[0]);
    _NewMapData.AllSettles.Add(Towns[0]);
    _NewMapData.AllSettles.Add(Towns[1]);
    _NewMapData.AllSettles.Add(Villages[0]);
    _NewMapData.AllSettles.Add(Villages[1]);
    _NewMapData.AllSettles.Add(Villages[2]);

    #endregion

    void SetSettle(ref List<Settlement> newsettles)
    {
      for (int i = 0; i < newsettles.Count; i++)
      {
        List<TileData> _aroundtiles_1 = _NewMapData.GetAroundTile(newsettles[i].Tiles, 1);
        List<TileData> _aroundtiles_2 = _NewMapData.GetAroundTile(newsettles[i].Tiles, 2);

        newsettles[i].IsRiver = CheckEnvir(_aroundtiles_1, BottomEnvirType.River) ||
          CheckEnvir(_aroundtiles_1, BottomEnvirType.Source) ||
          CheckEnvir(_aroundtiles_1, BottomEnvirType.RiverBeach);
        newsettles[i].IsMountain = CheckEnvir(_aroundtiles_2, TopEnvirType.Mountain);
        newsettles[i].IsSea = CheckEnvir(_aroundtiles_2, BottomEnvirType.Sea) || CheckEnvir(_aroundtiles_2, BottomEnvirType.Beach) || CheckEnvir(_aroundtiles_2, BottomEnvirType.RiverBeach);
        newsettles[i].IsHighland = CheckEnvir(_aroundtiles_1, TopEnvirType.Highland);
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

    bool _villageriver = false, _villageforest = false, _villagehighland = false, _villagemountain = false, _villagesea = false;
    bool _townriver = false, _townforest = false, _townhighland = false, _townmountain = false, _townsea = false;
    bool _cityriver = false, _cityforest = false, _cityhighland = false, _citymountain = false;

    foreach (var _village in _NewMapData.Villages)
    {
      if (_village.IsRiver) _villageriver = true;
      if (_village.IsForest) _villageforest = true;
      if (_village.IsHighland) _villagehighland = true;
      if (_village.IsMountain) _villagemountain = true;
      if (_village.IsSea) _villagesea = true;
    }
    foreach (var _town in _NewMapData.Towns)
    {
      if (_town.IsRiver) _townriver = true;
      if (_town.IsForest) _townforest = true;
      if (_town.IsHighland) _townhighland = true;
      if (_town.IsMountain) _townmountain = true;
      if (_town.IsSea) _townsea = true;
    }
    var _city = _NewMapData.City;
    if (_city.IsRiver) _cityriver = true;
    if (_city.IsForest) _cityforest = true;
    if (_city.IsHighland) _cityhighland = true;
    if (_city.IsMountain) _citymountain = true;

    if (!_villageriver || !_villageforest || !_villagemountain || !_villagesea ||
             !_townriver || !_townforest || !_townmountain || !_townsea ||
             !_cityriver || !_cityforest || !_citymountain)
    {
      _str = "";
      if (!_villageriver) _str += "���� ��  ";
      if (!_villageforest) _str += "���� ��  ";
      //  if (!_villagehighland) _str += "���� ���  ";
      if (!_villagemountain) _str += "���� ��  ";
      if (!_villagesea) _str += "���� �ٴ�  ";
      if (!_townriver) _str += "���� ��  ";
      if (!_townforest) _str += "���� ��  ";
      //  if (!_townhighland) _str += "���� ���  ";
      if (!_townmountain) _str += "���� ��  ";
      if (!_townsea) _str += "���� �ٴ�  ";
      if (!_cityriver) _str += "��ä ��  ";
      if (!_cityforest) _str += "��ä ��  ";
      //   if (!_cityhighland) _str += "��ä ���  ";
      if (!_citymountain) _str += "��ä ��  ";

      Debug.Log("�� ��    " + _str + "����");
    }

    /* Debug.Log($"��ä\n" +
       $"{_NewMapData.Castles.Tiles[0].Coordinate} {_NewMapData.Castles.Tiles[1].Coordinate} {_NewMapData.Castles.Tiles[2].Coordinate}\n\n" +
       $"����\n" +
       $"{_NewMapData.Cities[0].Tiles[0].Coordinate} {_NewMapData.Cities[0].Tiles[1].Coordinate}\n" +
       $"{_NewMapData.Cities[1].Tiles[0].Coordinate} {_NewMapData.Cities[1].Tiles[1].Coordinate}\n\n" +
       $"����\n" +
       $"{_NewMapData.Towns[0].Tiles[0].Coordinate}\n" +
       $"{_NewMapData.Towns[1].Tiles[0].Coordinate}\n" +
       $"{_NewMapData.Towns[0].Tiles[0].Coordinate}");*/

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
    if (_temp == 6) Debug.Log($"{origindir} {modify} ��í�ƾƾ�");
    return _temp;
  }
  public void MakeTilemap()
  {
    Vector3 _cellsize = new Vector3(190 * 0.7f, 190 * 0.7f); Debug.Log("���� ���� ����~");
    //Ÿ�Ϸ� ����ȭ
    for (int i = 0; i < ConstValues.MapSize; i++)
    {
      for (int j = 0; j < ConstValues.MapSize; j++)
      {
        Vector3Int _coordinate = new Vector3Int(j, i, 0);

        Vector3 _pos = Tilemap_bottom.CellToWorld(_coordinate);
        int _rotate = GameManager.Instance.MyGameData.MyMapData.TileDatas[j, i].Rotate;

        string _bottomname = $"{j},{i} {GameManager.Instance.MyGameData.MyMapData.Tile(_coordinate).BottomEnvir}";
        Sprite _bottomspr = MyTiles.GetTile(GameManager.Instance.MyGameData.MyMapData.TileDatas[j, i].BottomEnvirSprite);
        GameObject _bottomtile = new GameObject(_bottomname, new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
        _bottomtile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -60.0f * _rotate));
        _bottomtile.transform.SetParent(TileHolder_bottomenvir);
        Image _bottomimage = _bottomtile.GetComponent<Image>();
        _bottomimage.sprite = _bottomspr;
        RectTransform _bottomrect = _bottomtile.GetComponent<RectTransform>();
        _bottomrect.sizeDelta = _cellsize;
        _bottomrect.position = new Vector3(_pos.x, _pos.y, _bottomrect.position.z);
        _bottomrect.transform.localScale = Vector3.one;
        _bottomrect.anchoredPosition3D = new Vector3(_bottomrect.anchoredPosition3D.x, _bottomrect.anchoredPosition3D.y, 0.0f);
        GameManager.Instance.MyGameData.MyMapData.Tile(_coordinate).Rect = _bottomrect;
        Outline _bottomoutline = _bottomtile.AddComponent<Outline>();
        _bottomoutline.effectColor = Color.black;
        _bottomoutline.effectDistance = Vector2.one * 2.0f;
        Button _button = _bottomtile.AddComponent<Button>();
        Navigation _nav = new Navigation();
        _nav.mode = Navigation.Mode.None;
        _button.navigation = _nav;
        _bottomoutline.enabled = false;
        _bottomtile.AddComponent(typeof(TileButtonScript));
        _bottomimage.raycastTarget = GameManager.Instance.MyGameData.MyMapData.Tile(_coordinate).Interactable;
        _button.interactable = false;

        string _topname = $"{j},{i} {GameManager.Instance.MyGameData.MyMapData.Tile(_coordinate).TopEnvir}";
        Sprite _topespr = MyTiles.GetTile(GameManager.Instance.MyGameData.MyMapData.TileDatas[j, i].TopEnvirSprite);
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
        Sprite _landmarkspr = MyTiles.GetTile(GameManager.Instance.MyGameData.MyMapData.TileDatas[j, i].landmarkSprite);
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
        _buttonscript.OutLine = _bottomoutline;
        _buttonscript.Button = _button;
        _buttonscript.SelectHolder = MapUIScript.SelectTileHolder;
        _buttonscript.OriginHolder = TileHolder_bottomenvir;
        _buttonscript.MapUI = MapUIScript;
        _buttonscript.TileData = GameManager.Instance.MyGameData.MyMapData.Tile(_coordinate);
        _buttonscript.BottomImage = _bottomimage;
        _buttonscript.TopImage = _topimage;
        _buttonscript.LandmarkImage= _landmarkimage;
        _button.onClick.AddListener(() => _buttonscript.Clicked());

        GameManager.Instance.MyGameData.MyMapData.Tile(_coordinate).ButtonScript = _buttonscript;
      }
    }
    //�� ���� �������� ��ư���� ����°�

    List<TileData> _settlementrectlist= new List<TileData>();
    Vector2 _settlementpos = Vector2.zero;

    for(int i = 0; i < GameManager.Instance.MyGameData.MyMapData.Villages.Count; i++)
    {
      Settlement _village = GameManager.Instance.MyGameData.MyMapData.Villages[i];
      _settlementpos = _village.Tiles[0].Rect.anchoredPosition;

      string _villagename = _village.OriginName;
      GameObject _villageholder = new GameObject(_villagename, new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(CanvasGroup) });
      _villageholder.transform.SetParent(TileHolder_landmark);
      _villageholder.GetComponent<RectTransform>().anchoredPosition = _settlementpos;
      _villageholder.GetComponent<CanvasGroup>().blocksRaycasts = false;

      _village.Tiles[0].ButtonScript.LandmarkImage.transform.SetParent(_villageholder.transform);
      MapUIScript.VillageIcons.Add(_villageholder);
    }

    _settlementrectlist.Clear();
    _settlementpos = Vector2.zero;
    for (int i = 0; i < GameManager.Instance.MyGameData.MyMapData.Towns.Count; i++)
    {
      Settlement _town = GameManager.Instance.MyGameData.MyMapData.Towns[i];
      for(int j = 0; j < _town.Tiles.Count; j++)
      {
        _settlementrectlist.Add(_town.Tiles[j]);
        _settlementpos += _town.Tiles[j].Rect.anchoredPosition;
      }
      _settlementpos /= 2.0f;

      string _townname = _town.OriginName;
      GameObject _townholder = new GameObject(_townname, new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer),typeof(CanvasGroup) });
      _townholder.transform.SetParent(TileHolder_landmark);
      _townholder.GetComponent<RectTransform>().anchoredPosition = _settlementpos;
      _townholder.GetComponent<CanvasGroup>().blocksRaycasts = false;

      for (int j = 0; j < _settlementrectlist.Count; j++)
      {
        _settlementrectlist[j].ButtonScript.LandmarkImage.transform.SetParent(_townholder.transform);
      }
      MapUIScript.TownIcons.Add(_townholder);
    }

    _settlementrectlist.Clear();
    _settlementpos = Vector2.zero;
    Settlement _city = GameManager.Instance.MyGameData.MyMapData.City;
    for (int i = 0; i < _city.Tiles.Count; i++)
    {
      _settlementrectlist.Add(_city.Tiles[i]);
      _settlementpos += _city.Tiles[i].Rect.anchoredPosition;
    }
    _settlementpos /= 3.0f;

    string _cityname = _city.OriginName;
    GameObject _cityholder = new GameObject(_cityname, new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(CanvasGroup) });
    _cityholder.transform.SetParent(TileHolder_landmark);
    _cityholder.GetComponent<RectTransform>().anchoredPosition = _settlementpos;
    _cityholder.GetComponent<CanvasGroup>().blocksRaycasts = false;

    for (int j = 0; j < _settlementrectlist.Count; j++)
    {
      _settlementrectlist[j].ButtonScript.LandmarkImage.transform.SetParent(_cityholder.transform);
    }
    MapUIScript.CityIcon=_cityholder;

  }

  /// <summary>
  /// startpos���� dir(0~6) ���� 1ĭ Ÿ�� ��ǥ ��ȯ
  /// </summary>
  /// <param name="startpos"></param>
  /// <param name="dir"></param>
  /// <returns></returns>
  public Vector2Int GetNextPos(Vector2Int startpos,int dir)
    {
        bool _iseven = startpos.y % 2 == 0;  //startpos�� y�� ¦������ �Ǻ�
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
    /// <summary>
    /// check ��Ͽ��� pos�� ���� 1ĭ�� ��ġ�°� ������ false
    /// </summary>
    /// <param name="check"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
}
public class RiverData
{
  public int RiverIndex = 0;                                            //�ð���� 0~5
  public List<Vector2Int> RiverCoors= new List<Vector2Int>();           //��� �� ��ǥ
  public List<int> RiverDirs=new List<int>();                           //��� �� ���� ����(���� ����)
}