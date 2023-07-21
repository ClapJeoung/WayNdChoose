using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum BottomEnvirType
{
 NULL, Land,River,Sea,Source, Beach,RiverBeach
}
public enum TopEnvirType {NULL, Forest,Mountain,Highland }
public enum LandMarkType { NULL,Town,City,Castle}
public enum HexDir { TopRight,Right,BottomRight,BottomLeft,Left,TopLeft}
public class TileData
{
  public Vector2Int Coordinate = Vector2Int.zero;
  public BottomEnvirType BottomEnvir = BottomEnvirType.NULL;
  public TopEnvirType TopEnvir = TopEnvirType.NULL;
  public int Rotate = 0;
  public LandMarkType LandMark = LandMarkType.NULL;
  public Settlement TileSettle = null;
  public TileSpriteType TopEnvirSprite = TileSpriteType.NULL;
  public TileSpriteType BottomEnvirSprite= TileSpriteType.Sea;
}

public class maptext : MonoBehaviour
{
   public Tilemap Tilemap_bottom, Tilemap_top;
   public TilePrefabs MyTiles;
  [Space(10)]
  [SerializeField] private Transform SettlerHolder = null;
  [SerializeField] private Transform TileHolder = null;
  [SerializeField] private Sprite Townsprite, Citysprite, Castlesprite;
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
            bool _townriver = false, _townforest = false, _townhighland = false, _townmountain = false, _townsea = false;
            bool _cityriver = false, _cityforest = false, _cityhighland = false, _citymountain = false, _citysea = false;
            bool _castleriver = false, _castleforest = false, _castlehighland = false, _castlemountain = false;

            foreach (var _town in _data.Towns)
            {
                if (_town.IsRiver) _townriver = true;
                if (_town.IsForest) _townforest = true;
                if (_town.IsHighland) _townhighland = true;
                if (_town.IsMountain) _townmountain = true;
                if (_town.IsSea) _townsea = true;
            }
            foreach (var _city in _data.Cities)
            {
                if (_city.IsRiver) _cityriver = true;
                if (_city.IsForest) _cityforest = true;
                if (_city.IsHighland) _cityhighland = true;
                if (_city.IsMountain) _citymountain = true;
                if (_city.IsSea) _citysea = true;
            }
      var _castle = _data.Castles;
      if (_castle.IsRiver) _castleriver = true;
      if (_castle.IsForest) _castleforest = true;
      if (_castle.IsHighland) _castlehighland = true;
      if (_castle.IsMountain) _castlemountain = true;

      if (!_townriver||!_townforest||!_townhighland||!_townmountain||!_townsea||
               !_cityriver || !_cityforest || !_cityhighland || !_citymountain || !_citysea ||
               !_castleriver || !_castleforest || !_castlehighland || !_castlemountain )
            {
                string _str = "";
                if (!_townriver) _str += "���� ��  ";
                if (!_townforest) _str += "���� ��  ";
                if (!_townhighland) _str += "���� ���  ";
                if (!_townmountain) _str += "���� ��  ";
                if (!_townsea) _str += "���� �ٴ�  ";
                if (!_cityriver) _str += "���� ��  ";
                if (!_cityforest) _str += "���� ��  ";
                if (!_cityhighland) _str += "���� ���  ";
                if (!_citymountain) _str += "���� ��  ";
                if (!_citysea) _str += "���� �ٴ�  ";
                if (!_castleriver) _str += "��ä ��  ";
                if (!_castleforest) _str += "��ä ��  ";
                if (!_castlehighland) _str += "��ä ���  ";
                if (!_castlemountain) _str += "��ä ��  ";

            //    Debug.Log(_index+"�� ��    "+ _str + "����");
                yield return null;
                continue;
            }
      if (_data.Towns.Count != 4) { yield return null;continue; }
      if (_data.Cities.Count != 2) { yield return null; continue; }
      if (_data.Castles==null) {  yield return null; continue; }

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
    List<TileData> _lands=_NewMapData.GetEnvirTiles(new List<BottomEnvirType> { BottomEnvirType.Land });
    foreach(var _land in _lands)
    {
      List<TileData> _around = _NewMapData.GetAroundTile(_land, 1);
      foreach(var _tile in _around)
      {
        if (_tile.BottomEnvir == BottomEnvirType.Sea)
        {
          _NewMapData.Tile(_tile.Coordinate).BottomEnvir = BottomEnvirType.Land;
          _NewMapData.Tile(_tile.Coordinate).BottomEnvirSprite = TileSpriteType.Land;
        }
      }
    }
    #endregion
    //     Debug.Log("�ٴ� ���� �Ϸ�");
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

    while (_newmountain.Count < 9)
    {
      LoopCount++;
      if (LoopCount > 1000) { Debug.Log("�� ���� �� ���ѷ��� "+_newmountain.Count); return null; }

      int _index = _newmountain.Count / 3;
      List<TileData> _lines = _NewMapData.GetDirLines(_NewMapData.CenterTile, (HexDir)_mountaindirs[_index]);
      Vector2Int _mountain_0=Vector2Int.zero,_mountain_1 = Vector2Int.zero, _mountain_2=Vector2Int.zero;
      int _mountaindir_1 = 2, _mountaindir_2 = 3;
      while (true)
      {
        LoopCount++;
        if (LoopCount > 1000) { Debug.Log("�� ���� �� ���ѷ���");return null; }

        _mountain_0 = _lines[Random.Range((int)((float)_lines.Count * 0.3f), (int)((float)_lines.Count * 0.9f))].Coordinate
          +new Vector2Int(Random.Range(-1,2), Random.Range(-1, 2));
        if (checkformountain(_NewMapData.Tile(_mountain_0)) == false)
        {
          continue;
        }

        _mountain_1 = _NewMapData.GetNextTile(_mountain_0, (HexDir)_mountaindir_1).Coordinate;
        if (checkformountain(_NewMapData.Tile(_mountain_1)) == false)
        {
          continue;
        }


        _mountain_2 = _NewMapData.GetNextTile(_mountain_0, (HexDir)_mountaindir_2).Coordinate;
        if (checkformountain(_NewMapData.Tile(_mountain_2)) == false)
        {
          continue;
        }


        break;
      }

      _newmountain.Add(_mountain_0);
      _newmountain.Add(_mountain_1);
      _newmountain.Add(_mountain_2);

      bool checkformountain(TileData _tile)
      {
        List<TileData> _aroundtiles = _NewMapData.GetAroundTile(_tile, 1);

        if (_tile.BottomEnvir != BottomEnvirType.Land)
        {
          return false;
        }

        foreach(TileData _othertile in _aroundtiles)
        {
          if (_newmountain.Contains(_othertile.Coordinate) == true)
          {
            return false;
          }
        }

        return true;
      }//�ֺ� 1Ÿ�Ͽ� ���� ��ĥ ��� X
    }
    foreach (var _coor in _newmountain)
    {
      _NewMapData.Tile(_coor).TopEnvir = TopEnvirType.Mountain;
      _NewMapData.Tile(_coor).TopEnvirSprite = TileSpriteType.Mountain;
    }
    #endregion

    //     Debug.Log("�� ���� �Ϸ�");

    #region ��
    LoopCount = 0;

    RiverData[] _riverdatas = new RiverData[3] { new RiverData(), new RiverData(), new RiverData() };
    for (int i=0; i < _riverdatas.Length; i++)
    {
      List<int> _sourcetype = MixRandom(new List<int> { 0, 1, 2 });         //0,1,2(�߿��� ��ġ �ڵ�)
      for(int j=0; j< _sourcetype.Count; j++)
      {
        bool _iscomplete = false;
        List<int> _sourcedirs=new List<int>();                              //�� �� �ٱ� ����(0~5)
        switch (_sourcetype[j])
        {
          case 0: _sourcedirs = MixRandom( new List<int> { 0, 1 });break;
          case 1: _sourcedirs = MixRandom(new List<int> { 2, 3 }); break;
          case 2: _sourcedirs = MixRandom(new List<int> { 4, 5 }); break;
        }
        for(int k = 0; k < _sourcedirs.Count; k++)
        {
          Vector2Int _sourcepos = Vector2Int.zero;
          switch (_sourcetype[j])
          {
            case 0: _sourcepos = _NewMapData.GetNextCoor(_newmountain[i * 3],(HexDir) 1);break;
            case 1: _sourcepos = _NewMapData.GetNextCoor(_newmountain[i * 3+1], (HexDir)3); break;
            case 2: _sourcepos = _NewMapData.GetNextCoor(_newmountain[i * 3+2], (HexDir)5); break;
          }

          RiverData _riverdata = new RiverData();
          _riverdata.SourceType = _sourcetype[j];
          _riverdata.RiverCoors.Add(_sourcepos);
          _riverdata.RiverDirs.Add(_sourcedirs[k]);
          int _maindir = _sourcedirs[k];
          int _curvecount = 0;
          List<Vector2Int> _failcoors=new List<Vector2Int>();

          Debug.Log($"{i + 1}��° �� �߿��� {_sourcepos}���� {_maindir}�������� ����");
          while (true)
          {
            LoopCount++;
            if (LoopCount > 1000) { Debug.Log("�� ���� �� ���ѷ���"); return null; }

            if (_riverdata.RiverCoors.Count == 0) break;

            int _finishdir = 10;                                            //(0~5)
            List<int> _targetdirs = MixRandom(new List<int> { -1, 0, 1 });  //-1,0,1�� ��������
            List<int> _enabledirs=new List<int>();                          //Ȯ���� �����(-1,0,1)
            Vector2Int _currentcoor = _riverdata.RiverCoors[_riverdata.RiverCoors.Count - 1];  //���ư� Ÿ�� �߽���
            for (int l=0; l< _targetdirs.Count; l++)
            {

              int _nextdir = modifydir(_maindir, _targetdirs[l]);                //���� ���õ� ����(0~5)
              TileData _nexttile = _NewMapData.GetNextTile(_currentcoor, (HexDir)_nextdir);

              if (_curvecount == -2 && _targetdirs[l] == -1) continue;
              if (_curvecount == 2 && _targetdirs[l] == 1) continue;//������ Ŀ�� ����
              if (_failcoors.Contains(_nexttile.Coordinate)) continue;
              if (_nexttile.TopEnvir == TopEnvirType.Mountain || _nexttile.TopEnvir == TopEnvirType.Highland) continue;//��,��� �ȵ�
              if(i>0&& _riverdatas[0].RiverCoors.Contains(_nexttile.Coordinate)) continue;
              if (i>1 && _riverdatas[1].RiverCoors.Contains(_nexttile.Coordinate)) continue;

              _enabledirs.Add(_targetdirs[l]);
              if (_nexttile.BottomEnvir == BottomEnvirType.Beach) _finishdir = _nextdir;
            }

            if (_enabledirs.Count == 0)
            {
              Debug.Log($"{i + 1}��° �� [{_riverdata.RiverCoors.Count+1}]{_currentcoor} ����, {_riverdata.RiverCoors.Count}������ ȸ��");
              _failcoors.Add(_currentcoor);
              _riverdata.RiverCoors.Remove(_currentcoor);
              _riverdata.RiverDirs.Remove(_riverdata.RiverDirs[_riverdata.RiverDirs.Count - 1]);
              continue;
            }//enabledir ������ 0�� ��� ���� ���� �Ұ� Ÿ���̹Ƿ� ��ǥ,������ 1���� ���� (���� Ÿ�� failcoor)

            if (_finishdir != 10)
            {
              if (_riverdata.RiverCoors.Count < 6)
              {
                Debug.Log($"{i + 1}��° �� [{_riverdata.RiverCoors.Count + 1}]->({_finishdir})->[{_riverdata.RiverCoors.Count + 2}]{_NewMapData.GetNextCoor(_currentcoor, (HexDir)_finishdir)} ���� ������ �غ����� ���� {_riverdata.RiverCoors.Count+1}�� Ÿ�� �ٽ� �˻�");
                _failcoors.Add(_NewMapData.GetNextCoor(_currentcoor, (HexDir)_finishdir));
                continue;
              }//�غ��� ã�Ҵµ� �� ������ ä���� ���� ���(�ش� Ÿ�� failcoor)
              else
              {
                Debug.Log($"{i + 1}��° �� [{_riverdata.RiverCoors.Count + 1}]{_currentcoor} ->({_finishdir})-> [{_riverdata.RiverCoors.Count + 2}]{_NewMapData.GetNextCoor(_currentcoor, (HexDir)_finishdir)} �� ����");
                _riverdata.RiverCoors.Add(_NewMapData.GetNextCoor(_currentcoor, (HexDir)_finishdir));
                _riverdata.RiverDirs.Add(_finishdir);
                _iscomplete = true;
                break;
              }//�غ��� ã�Ұ� �� �ּ� ������ ����()
            }

            int _selectdir = 0;
            switch(_enabledirs[Random.Range(0, _enabledirs.Count - 1)])
            {
              case -1:
                _selectdir = modifydir(_maindir, -1);
                _curvecount--;
                break;
              case 0:
                _selectdir=modifydir(_maindir, 0);
                break;
              case 1:
                _selectdir = modifydir(_maindir, 1);
                _curvecount++;
                break;
            }//enabledir ������ 1 �̻��� ��� ������ 1��
            Vector2Int _nextcoor=_NewMapData.GetNextCoor(_currentcoor,(HexDir)_selectdir);

            Debug.Log($"{i + 1}��° �� [{_riverdata.RiverCoors.Count + 1}]{_currentcoor} ->({_selectdir})-> [{_riverdata.RiverCoors.Count + 2}]{_nextcoor} ����");
            _riverdata.RiverCoors.Add(_nextcoor);
            _riverdata.RiverDirs.Add(_selectdir);
          }

          if (_riverdata.RiverCoors.Count < 6) continue;

          if (_iscomplete) { _riverdatas[i] = _riverdata; break; }
        }
        if (_iscomplete) continue;
      }

      int modifydir(int maindir, int modify)
      {
        int _temp = maindir + modify;
        return _temp < 0 ? _temp + 6 : _temp > 6 ? _temp - 6 : _temp;
      }
    }

    foreach (var _river in _riverdatas)
    {
      for(int j = 0; j < _river.RiverCoors.Count; j++)
      {
        if (j == 0)
        {
          TileData _source = _NewMapData.Tile(_river.RiverCoors[j]);
          _source.Rotate = _river.SourceType * 2;
          _source.BottomEnvirSprite = MyTiles.GetSource(RotateDir(_river.RiverDirs[0],_source.Rotate));

        }//�߿���
        else if (j == _river.RiverCoors.Count - 1)
        {
          TileData _riverbeach=_NewMapData.Tile(_river.RiverCoors[j]);
          List<int> _seadirs = new List<int>();
          for (int k= 0; k < 6; k++)
          {
            if (_NewMapData.GetNextTile(_riverbeach, (HexDir)k).BottomEnvir == BottomEnvirType.Sea) _seadirs.Add(k);
          }

          for (int k = 0; k < 6; k++)
          {
            if (MaxDirIndex() == _seadirs.Count)
            {
              _riverbeach.BottomEnvirSprite = MyTiles.GetRiverBeach(_seadirs.Count,RotateDir(_river.RiverDirs[j-1],-k+3));
              _riverbeach.Rotate = k;
              break;
            }
            for(int l=0;l<_seadirs.Count; l++)
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
          TileData _rivertile = _NewMapData.Tile(_river.RiverCoors[j]);
          int[] _combineddirs=new int[2] { RotateDir(_river.RiverDirs[j - 1], 3), _river.RiverDirs[j] };
          for(int k = 0; k < 6; k++)
          {
            int _min=_combineddirs[0]<_combineddirs[1]?_combineddirs[0]:_combineddirs[1];
            int _max = _combineddirs[0] > _combineddirs[1] ? _combineddirs[0] : _combineddirs[1];

            if (_min == 0 && _max <= 3)
            {
              _rivertile.Rotate = k;
              _rivertile.BottomEnvirSprite = MyTiles.GetRiver(_max);
            }

            _combineddirs[0]= RotateDir(_combineddirs[0], -1);
            _combineddirs[1]=RotateDir(_combineddirs[1], -1);
          }
        }//�ٱ�
      }
    }

    #endregion


    GameManager.Instance.MyGameData.MyMapData = _NewMapData;
    MakeTilemap();
    return null;

    //    Debug.Log("�� ���� �Ϸ�");

    #region ���
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
    #endregion
    //     Debug.Log("��� ���� �Ϸ�");

    #region ��
    LoopCount = 0;

    int _forestmaxcount = Mathf.CeilToInt((float)ConstValues.LandSize * (float)ConstValues.LandSize * ConstValues.Ratio_forest);    //�� �ִ� ����
    int _forestcountaver = _forestmaxcount / 4; //�� ���� ��� �ִ밳��
    List<Vector2Int> _forests = new List<Vector2Int>();         //�� �Ϸ�� �� ��ǥ��

    List<Vector2Int> _forestbundle_origin=new List<Vector2Int>();
    int _forestmodifycount = 0;
    for(int i = 1; i < 100; i++)
    {
      _forestbundle_origin = _NewMapData.GetAroundCoor(Vector2Int.zero,i);
      if (_forestbundle_origin.Count > _forestcountaver)
      {
        _forestmodifycount = _forestbundle_origin.Count - _forestcountaver;
        break;
      }
    }
                                                                 //  Debug.Log($"�� �ִ� ���� : {_forestmaxcount}  �� ���� ��� ���� : {_forestcountaver}");
    while (_forests.Count < _forestmaxcount)
    {
      LoopCount++;
      if (LoopCount > 1000) { Debug.Log("�� ���� �� ���ѷ���"); return null; }

      List<Vector2Int> _forestbundle=new List<Vector2Int>();
      foreach (var _coor in _forestbundle_origin) _forestbundle.Add(_coor);
      List<Vector2Int> _deletebundles=new List<Vector2Int>();

      for(int i = 0; i < _forestmodifycount; i++)
      {
        Vector2Int _temp= _forestbundle_origin[Random.Range(0, _forestbundle_origin.Count)];
        if (_deletebundles.Contains(_temp))
        {
          i--;
          continue;
        }
      }
      foreach(var _deletecoor in _deletebundles)_forestbundle.Remove(_deletecoor);

      List<TileData> _emptylandtiles = _NewMapData.GetEnvirTiles
        (new List<BottomEnvirType> { BottomEnvirType.Land }, new List<TopEnvirType> { TopEnvirType.Mountain, TopEnvirType.Highland,TopEnvirType.Forest },1);
      foreach (var _tiles in _deletebundles) _forestbundle.Remove(_tiles);
      //�̰Ŏ� ���� ������ �뼺�뼺 �� �ϼ�
      
      Vector2Int _startcoor=_emptylandtiles[Random.Range(0,_emptylandtiles.Count)].Coordinate;
      foreach(var _tile in _forestbundle)
      {
        Vector2Int _targetcoor = _startcoor + _tile;
        TileData _targettile = _NewMapData.TileDatas[_targetcoor.x, _targetcoor.y];
        if (_targettile.BottomEnvir != BottomEnvirType.Land && _targettile.TopEnvir == TopEnvirType.NULL)
        {
          _forests.Add(_targetcoor);
          _NewMapData.Tile(_targetcoor).TopEnvir = TopEnvirType.Forest;
          _NewMapData.Tile(_targetcoor).TopEnvirSprite =
            (_NewMapData.Tile(_targetcoor).BottomEnvir == BottomEnvirType.River ||
            _NewMapData.Tile(_targetcoor).BottomEnvir == BottomEnvirType.Source ||
            _NewMapData.Tile(_targetcoor).BottomEnvir == BottomEnvirType.RiverBeach) ? TileSpriteType.RiverForest : TileSpriteType.Forest;
        }
      }

    }//�� ���� ä��� ������ �ݺ�
     //  Debug.Log($"�� �ִ� ���� : {_forestmaxcount}  ���� �� ���� : {_forests.Count}");

    #endregion
    //   Debug.Log("�� ���� �Ϸ�");

    #region ��ä
    LoopCount = 0;

    Settlement Castle = new Settlement(SettlementType.Castle);
    List<TileData> _castletiles = new List<TileData>();
    List<TileData> _castlestartrange = _NewMapData.GetAroundTile(new Vector2Int(ConstValues.MapSize / 2 + ConstValues.MapSize % 2, ConstValues.MapSize / 2 + ConstValues.MapSize % 2), 1);
    HexDir _castledir_1=HexDir.BottomRight, _castledir_2=HexDir.BottomLeft;

    while (_castletiles.Count != 3)
    {
      LoopCount++;
      if (LoopCount > 1000) { Debug.Log("��ä ���� �� ���ѷ���"); return null; }

      Vector2Int _starcoor = _castlestartrange[Random.Range(0, _castlestartrange.Count - 1)].Coordinate+new Vector2Int(Random.Range(-1,2), Random.Range(-1, 2));
     
      TileData _starttile = _NewMapData.TileDatas[_starcoor.x, _starcoor.y];
      if (CastleCheck(_starttile) == false) continue;

      TileData _secondtile = _NewMapData.GetNextTile(_starttile, _castledir_1);
      if (CastleCheck(_secondtile) == false) continue;

      TileData _thirdtile = _NewMapData.GetNextTile(_starttile, _castledir_2);
      if (CastleCheck(_thirdtile) == false) continue;

      _NewMapData.TileDatas[_starttile.Coordinate.x, _starttile.Coordinate.y].TileSettle = Castle;
      _NewMapData.TileDatas[_secondtile.Coordinate.x, _secondtile.Coordinate.y].TileSettle = Castle;
      _NewMapData.TileDatas[_thirdtile.Coordinate.x, _thirdtile.Coordinate.y].TileSettle = Castle;
      _castletiles.Add(_starttile);
      _castletiles.Add(_secondtile);
      _castletiles.Add(_thirdtile);
      _NewMapData.TileDatas[_starttile.Coordinate.x, _starttile.Coordinate.y].LandMark = LandMarkType.Castle;
      _NewMapData.TileDatas[_secondtile.Coordinate.x, _secondtile.Coordinate.y].LandMark = LandMarkType.Castle;
      _NewMapData.TileDatas[_thirdtile.Coordinate.x, _thirdtile.Coordinate.y].LandMark = LandMarkType.Castle;
      bool CastleCheck(TileData tile)
      {
        if (tile.BottomEnvir == BottomEnvirType.Sea) return false;
        if (tile.TopEnvir == TopEnvirType.Mountain) return false;
        if (tile.TileSettle != null) return false;
        return true;
      }
    }
    for(int i = 0; i < _castletiles.Count; i++)
    {
      Castle.Tiles.Add(_castletiles[i]);
    }

    #endregion
    //   Debug.Log("��ä ���� �Ϸ�");

    #region ����
    LoopCount = 0;

    List<TileData> _citytiles=new List<TileData>();
    List<Settlement> Cities=new List<Settlement>();
    Cities.Add(new Settlement(SettlementType.City));Cities.Add(new Settlement(SettlementType.City));
    List<HexDir> _citydirs=new List<HexDir>();
    List<HexDir> _towndirs = new List<HexDir>() { (HexDir)0, (HexDir)1, (HexDir)2, (HexDir)3, (HexDir)4, (HexDir)5 };
    switch (Random.Range(0, 3))
    {
      case 0:
        _citydirs =new List<HexDir>{ (HexDir)0,(HexDir)3};
        break;
      case 1:
        _citydirs = new List<HexDir> { (HexDir)1, (HexDir)4 };
        break;
      case 2:
        _citydirs = new List<HexDir> { (HexDir)2, (HexDir)5 };
        break;
    }
    foreach (HexDir _hexdir in _citydirs) _towndirs.Remove(_hexdir);
    _towndirs.Remove(_towndirs[Random.Range(0, _towndirs.Count)]);

    while (_citytiles.Count != 2*2)
    {
      LoopCount++;
      if (LoopCount > 1000) { Debug.Log("���� ���� �� ���ѷ���"); return null; }

      List<TileData> _lines = _NewMapData.GetDirLines(_NewMapData.CenterTile, _citydirs[_citytiles.Count / 2]);

      Vector2Int _selectcoor = _lines[Random.Range((int)(_lines.Count * 0.4f), (int)(_lines.Count * 1.0f))].Coordinate+new Vector2Int(Random.Range(-1,2), Random.Range(-1, 2));
      TileData _firsttile = _NewMapData.TileDatas[_selectcoor.x, _selectcoor.y];
      if(citycheck(_firsttile)==false)continue;

      int _loopcount = 0;
      TileData _secondtile = null;
      while (_loopcount < 6)
      {
        _secondtile = _NewMapData.GetNextTile(_firsttile, (HexDir)Random.Range(0, 6));
        if (citycheck(_secondtile) == true) break;
        _loopcount++;
      }
      if (_loopcount == 6) continue;

      _NewMapData.TileDatas[_firsttile.Coordinate.x, _firsttile.Coordinate.y].TileSettle = Cities[_citytiles.Count / 2];
      _NewMapData.TileDatas[_secondtile.Coordinate.x, _secondtile.Coordinate.y].TileSettle = Cities[_citytiles.Count / 2];
      _NewMapData.Tile(_firsttile.Coordinate).LandMark = LandMarkType.City;
      _NewMapData.Tile(_secondtile.Coordinate).LandMark = LandMarkType.Castle;
      _citytiles.Add(_firsttile);
      _citytiles.Add(_secondtile);

      bool citycheck(TileData tile)
      {
        if (tile.TopEnvir == TopEnvirType.Mountain) return false;
        if (tile.TileSettle != null) return false;
        if(_citytiles.Contains(tile)) return false;

        List<TileData> _aroundtiles = _NewMapData.GetAroundTile(tile, 2);
        foreach (var _tile in _aroundtiles) if (_tile.TileSettle != null) return false;

        return true;
      }
    }
    for(int i=0;i<_citytiles.Count; i++)
    {
      Cities[i/2].Tiles.Add(_citytiles[i]);
    }
    #endregion
    //      Debug.Log("���� ���� �Ϸ�");

    #region ����
    LoopCount = 0;

    List<Settlement> Towns = new List<Settlement> { new Settlement(SettlementType.Town), new Settlement(SettlementType.Town), new Settlement(SettlementType.Town) };
    List<TileData> _towntiles= new List<TileData>();
    while (_towntiles.Count < 3)
    {
      LoopCount++;
      if (LoopCount > 1000) { Debug.Log("���� ���� �� ���ѷ���"); return null; }

      List<TileData> _lines = _NewMapData.GetDirLines(_NewMapData.CenterTile, _towndirs[_towntiles.Count]);

      Vector2Int _selectcoor = _lines[Random.Range((int)(_lines.Count * 0.4f), (int)(_lines.Count * 1.0f))].Coordinate + new Vector2Int(Random.Range(-2, 3), Random.Range(-1, 2));
      TileData _towntile = _NewMapData.TileDatas[_selectcoor.x, _selectcoor.y];

      if (_towntile.TopEnvir == TopEnvirType.Mountain) continue;
      if (_towntile.TileSettle != null) continue;
      if (_towntiles.Contains(_towntile)) continue;
      List<TileData> _aroundtile = _NewMapData.GetAroundTile(_towntile, 2);
      bool _breakable = false;
      foreach(var _tile in _aroundtile)
      {
        if (_towntiles.Contains(_tile)||_tile.TileSettle!=null) { _breakable = true;break; }
      }
      if (_breakable) continue;

      _NewMapData.TileDatas[_towntile.Coordinate.x, _towntile.Coordinate.y].TileSettle = Towns[_towntiles.Count];
      _NewMapData.Tile(_towntile.Coordinate).LandMark = LandMarkType.Town;
      _towntiles.Add(_towntile);
    }
    for(int i = 0; i < _towntiles.Count; i++)
    {
      Towns[i].Tiles.Add(_towntiles[i]);
    }
    #endregion
    //    Debug.Log("���� ���� �Ϸ�");


    #region ������ ����

    List<Settlement> _finishedsettles=new List<Settlement>();
    List<Settlement> CastleTemp = new List<Settlement>() { Castle };
    SetSettle(ref CastleTemp, ref _finishedsettles);
    SetSettle(ref Cities,ref _finishedsettles);
    SetSettle(ref Towns, ref _finishedsettles);

    _NewMapData.AllSettles.Add(Castle);
    _NewMapData.AllSettles.Add(Cities[0]);
    _NewMapData.AllSettles.Add(Cities[1]);
    _NewMapData.AllSettles.Add(Towns[0]);
    _NewMapData.AllSettles.Add(Towns[1]);
    _NewMapData.AllSettles.Add(Towns[2]);

    #endregion
    void SetSettle(ref List<Settlement> newsettles,ref List<Settlement> completesettles)
    {
      for (int i = 0; i < newsettles.Count; i++)
      {
        Settlement newsettle = newsettles[i];
        List<TileData> _aroundtiles_1 = _NewMapData.GetAroundTile(newsettle.Tiles, 1);
        List<TileData> _aroundtiles_2 = _NewMapData.GetAroundTile(newsettle.Tiles, 1);

        newsettle.IsRiver = (CheckEnvir(_aroundtiles_1, BottomEnvirType.River) ||
          CheckEnvir(_aroundtiles_1, BottomEnvirType.Source) ||
          CheckEnvir(_aroundtiles_1, BottomEnvirType.RiverBeach));
        newsettle.IsMountain = CheckEnvir(_aroundtiles_2, TopEnvirType.Mountain);
        newsettle.IsSea = CheckEnvir(_aroundtiles_2, BottomEnvirType.Sea) || CheckEnvir(_aroundtiles_2, BottomEnvirType.Beach) || CheckEnvir(_aroundtiles_2, BottomEnvirType.RiverBeach);
        newsettle.IsHighland = CheckEnvir(_aroundtiles_1, TopEnvirType.Highland);
        newsettle.IsForest = CheckEnvir(_aroundtiles_1, TopEnvirType.Forest);

        string[] _namearray;
        if (newsettle.Type == SettlementType.Town) { _namearray = SettlementName.TownNams; }
        else if (newsettle.Type == SettlementType.City) { _namearray = SettlementName.CityNames; }
        else { _namearray = SettlementName.CastleNames; }

        int _infoindex = Random.Range(0, _namearray.Length);
        while (true)
        {
          _infoindex = Random.Range(0, _namearray.Length);

          foreach (var _othersettle in completesettles)
            if (_othersettle.Type == newsettle.Type && _othersettle.Index == _infoindex) continue;

          break;
        }
        newsettle.Index = _infoindex;
        completesettles.Add(newsettle);
      }
    }

    return _NewMapData;
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
    origindir += modify;
    if (origindir < 0) origindir += 6;
    else if (origindir > 5) origindir -= 6;
    return origindir;
  }
  public void MakeTilemap()
    {
    Vector3 _cellsize = new Vector3(75, 75);
    Debug.Log("���� ���� ����~");
        //Ÿ�Ϸ� ����ȭ
        for (int i = 0; i < ConstValues.MapSize; i++)
        {
      for (int j = 0; j < ConstValues.MapSize; j++)
      {
        Vector3Int _newpos = new Vector3Int(j, i, 0);

        TileSpriteType _sprtype = TileSpriteType.NULL;
        Vector3 _pos = Vector3.zero;
        int _rotate = 0;

        _sprtype = GameManager.Instance.MyGameData.MyMapData.TileDatas[j, i].BottomEnvirSprite;
        _pos = Tilemap_bottom.CellToWorld(_newpos);
        _rotate = GameManager.Instance.MyGameData.MyMapData.TileDatas[j, i].Rotate;

        maketile(MyTiles.GetTile(_sprtype),_pos,_rotate,new Vector2Int(j,i));

        _sprtype = GameManager.Instance.MyGameData.MyMapData.TileDatas[j, i].TopEnvirSprite;
        _rotate = 0;

        if(_sprtype!=TileSpriteType.NULL) maketile(MyTiles.GetTile(_sprtype), _pos, _rotate,new Vector2Int(j, i));
      }
        }
    //�� ���� �������� ��ư���� ����°�
    return;

    Vector3 _zeropos = Vector3.zero;
    Vector3 _buttonpos = Vector3.zero;
        for(int i = 0; i < GameJsonData.TownCount; i++)
    {
      _buttonpos = Vector3.zero;
      List<Vector3> townpos = new List<Vector3>();
      townpos.Add(Tilemap_top.CellToWorld((Vector3Int)GameManager.Instance.MyGameData.MyMapData.Towns[i].Tiles[0].Coordinate));
      foreach (Vector3 pos in townpos) _buttonpos += pos;
      //��ġ(Rect�� ��ȯ) ����ְ�
      List<GameObject> _images=new List<GameObject>();
      foreach(Vector3 _pos in townpos)
      {
        GameObject _temp = new GameObject("lehoo", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
        RectTransform _rect = _temp.GetComponent<RectTransform>();
        _rect.localScale = Vector3.one;
        _rect.anchoredPosition = new Vector3(_pos.x, _pos.y, 0);
        _rect.sizeDelta = _cellsize;
        _temp.GetComponent<Image>().sprite = Townsprite;
        _temp.transform.SetParent(SettlerHolder);
        _rect.anchoredPosition3D=new Vector3(_rect.anchoredPosition.x, _rect.anchoredPosition.y, 0);
        _images.Add(_temp);
      }//�̹��� ����� ��ġ,��������Ʈ �ֱ�
      GameObject _button =new GameObject("lehoo", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(SettlementIcon) });
      //��ư ������Ʈ
      _button.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(_buttonpos.x,_buttonpos.y,0.0f);
            _button.transform.SetParent(SettlerHolder);
      _button.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(_button.GetComponent<RectTransform>().anchoredPosition3D.x, _button.GetComponent<RectTransform>().anchoredPosition3D.y, 0.0f);
      for (int j = 0; j < _images.Count; j++)
      {
        _images[j].transform.SetParent(_button.transform, true);
        _images[j].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(_images[j].GetComponent<RectTransform>().anchoredPosition.x, _images[j].GetComponent<RectTransform>().anchoredPosition.y, 0.0f);
        _images[j].transform.localScale = Vector3.one;
        _images[j].AddComponent<Outline>().effectDistance = new Vector2(1.5f, -1.5f);
      }
      GameObject _questicon = new GameObject("questicon", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
      _questicon.GetComponent<Image>().enabled = false;
      _questicon.transform.SetParent(_button.transform);
      _questicon.transform.localScale = Vector3.one;
      _questicon.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
      _questicon.GetComponent<RectTransform>().sizeDelta=Vector3.one*45.0f;
      _button.AddComponent<CanvasGroup>().alpha = 0.3f;
      _button.GetComponent<SettlementIcon>().Setup(GameManager.Instance.MyGameData.MyMapData.Towns[i],_questicon.GetComponent<Image>());
    //��ư ��ũ��Ʈ�� �� �߽ɺ� ������Ʈ ����� �ֱٰܳ�
    }

        _zeropos = Vector3.zero;
    for (int i = 0; i < GameJsonData.CityCount; i++)
    {
      _buttonpos = Vector3.zero;
      List<Vector3> citypos = new List<Vector3>();
      citypos.Add(Tilemap_top.CellToWorld((Vector3Int) GameManager.Instance.MyGameData.MyMapData.Cities[i * 2].Tiles[0].Coordinate) );
      citypos.Add(Tilemap_top.CellToWorld((Vector3Int)GameManager.Instance.MyGameData.MyMapData.Cities[i * 2].Tiles[1].Coordinate));
      foreach (Vector3 pos in citypos) _buttonpos += pos;
            _buttonpos /= 2;
            //��ġ(Rect�� ��ȯ) ����ְ�
            List<GameObject> _images = new List<GameObject>();
      foreach (Vector3 _pos in citypos)
      {
        GameObject _temp = new GameObject("lehoo", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
        RectTransform _rect = _temp.GetComponent<RectTransform>();
        _rect.localScale = Vector3.one;
        _rect.anchoredPosition = new Vector3(_pos.x, _pos.y, 0);
        _rect.sizeDelta = _cellsize;
        _temp.GetComponent<Image>().sprite = Citysprite;
        _temp.transform.SetParent(SettlerHolder);
        _rect.anchoredPosition3D = new Vector3(_rect.anchoredPosition.x, _rect.anchoredPosition.y, 0);
        _images.Add(_temp);
                _zeropos += _pos;
            }//�̹��� ����� ��ġ,��������Ʈ �ֱ�

      GameObject _button = new GameObject("lehoo", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(SettlementIcon) });
      //��ư ������Ʈ
      _button.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(_buttonpos.x, _buttonpos.y, 0.0f);
      _button.transform.SetParent(SettlerHolder);
      _button.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(_button.GetComponent<RectTransform>().anchoredPosition3D.x, _button.GetComponent<RectTransform>().anchoredPosition3D.y, 0.0f);
      for (int j = 0; j < _images.Count; j++)
      {
                //     Debug.Log($"�� ��ġ : {_images[j].GetComponent<RectTransform>().anchoredPosition}");
                Vector3 _newpos = _images[j].GetComponent<RectTransform>().anchoredPosition - _button.GetComponent<RectTransform>().anchoredPosition;
        _images[j].transform.SetParent(_button.transform, true);
         //       Debug.Log($"�θ� ���� ��ġ : {_images[j].GetComponent<RectTransform>().anchoredPosition}");
                _images[j].transform.localScale = Vector3.one;
                _images[j].GetComponent<RectTransform>().anchoredPosition3D=new Vector3(_newpos.x,_newpos.y,0.0f);
        _images[j].AddComponent<Outline>().effectDistance = new Vector2(1.5f, -1.5f);
      }
      GameObject _questicon = new GameObject("questicon", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
      _questicon.GetComponent<Image>().enabled = false;
      _questicon.transform.SetParent(_button.transform);
      _questicon.transform.localScale = Vector3.one;
      _questicon.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
      _questicon.GetComponent<RectTransform>().sizeDelta = Vector3.one * 45.0f;
      _button.AddComponent<CanvasGroup>().alpha = 0.3f;
      _button.GetComponent<SettlementIcon>().Setup(GameManager.Instance.MyGameData.MyMapData.Cities[i], _questicon.GetComponent<Image>());
      //��ư ��ũ��Ʈ�� �� �߽ɺ� ������Ʈ ����� �ֱٰܳ�
    }

        _zeropos = Vector3.zero;
    _buttonpos = Vector3.zero;
    List<Vector3> castlepos = new List<Vector3>();
    castlepos.Add(Tilemap_top.CellToWorld((Vector3Int)GameManager.Instance.MyGameData.MyMapData.Castles.Tiles[0].Coordinate));
    castlepos.Add(Tilemap_top.CellToWorld((Vector3Int)GameManager.Instance.MyGameData.MyMapData.Castles.Tiles[1].Coordinate));
    castlepos.Add(Tilemap_top.CellToWorld((Vector3Int)GameManager.Instance.MyGameData.MyMapData.Castles.Tiles[2].Coordinate));
    foreach (Vector3 pos in castlepos) _buttonpos += pos;
    _buttonpos /= 3;
    //��ġ(Rect�� ��ȯ) ����ְ�
    List<GameObject> _castleimages = new List<GameObject>();
    foreach (Vector3 _pos in castlepos)
    {
      GameObject _temp = new GameObject("lehoo", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
      RectTransform _rect = _temp.GetComponent<RectTransform>();
      _rect.localScale = Vector3.one;
      _rect.anchoredPosition3D = new Vector3(_pos.x, _pos.y, 0);
      _rect.sizeDelta = _cellsize;
      _temp.GetComponent<Image>().sprite = Castlesprite;
      _temp.transform.SetParent(SettlerHolder);
      _rect.anchoredPosition3D = new Vector3(_rect.anchoredPosition.x, _rect.anchoredPosition.y, 0);
      _castleimages.Add(_temp);
      _zeropos += _pos;
    }//�̹��� ����� ��ġ,��������Ʈ �ֱ�

    GameObject _castlebutton = new GameObject("lehoo", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(SettlementIcon) });
    //��ư ������Ʈ
    _castlebutton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(_buttonpos.x, _buttonpos.y, 0.0f);
    _castlebutton.transform.SetParent(SettlerHolder);
    _castlebutton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(_castlebutton.GetComponent<RectTransform>().anchoredPosition3D.x, _castlebutton.GetComponent<RectTransform>().anchoredPosition3D.y, 0.0f);
    for (int j = 0; j < _castleimages.Count; j++)
    {
      Vector3 _newpos = _castleimages[j].GetComponent<RectTransform>().anchoredPosition - _castlebutton.GetComponent<RectTransform>().anchoredPosition;
      _castleimages[j].transform.SetParent(_castlebutton.transform, true);
      _castleimages[j].transform.localScale = Vector3.one;
      _castleimages[j].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(_newpos.x, _newpos.y, 0.0f);
      _castleimages[j].AddComponent<Outline>().effectDistance = new Vector2(1.5f, -1.5f);
    }
    GameObject _castlequesticon = new GameObject("questicon", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
    _castlequesticon.GetComponent<Image>().enabled = false;
    _castlequesticon.transform.SetParent(_castlebutton.transform);
    _castlequesticon.transform.localScale = Vector3.one;
    _castlequesticon.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
    _castlequesticon.GetComponent<RectTransform>().sizeDelta = Vector3.one * 45.0f;
    _castlebutton.AddComponent<CanvasGroup>().alpha = 0.3f;
    _castlebutton.GetComponent<SettlementIcon>().Setup(GameManager.Instance.MyGameData.MyMapData.Castles, _castlequesticon.GetComponent<Image>());
    //��ư ��ũ��Ʈ�� �� �߽ɺ� ������Ʈ ����� �ֱٰܳ�



    void maketile(Sprite spr, Vector3 pos, int rot,Vector2Int coordinate)
    {
      Vector3 _newpos = pos;
      string _name = $"{coordinate.x},{coordinate.y}  {GameManager.Instance.MyGameData.MyMapData.Tile(coordinate).BottomEnvir},{GameManager.Instance.MyGameData.MyMapData.Tile(coordinate).TopEnvir}";
      GameObject _tile = new GameObject(_name, new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
      _tile.GetComponent<Image>().sprite = spr;
      _tile.transform.rotation = Quaternion.Euler(new Vector3(0,0,-60.0f*rot));
      RectTransform _rect = _tile.GetComponent<RectTransform>();
      _rect.anchoredPosition3D =new Vector3(_newpos.x,_newpos.y,0.0f);
      _rect.sizeDelta = _cellsize;
      _tile.transform.SetParent(TileHolder);
      _rect.anchoredPosition3D = new Vector3(_rect.anchoredPosition3D.x, _rect.anchoredPosition3D.y, 0.0f);
      _tile.transform.localScale = Vector3.one;
    }

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
  public int SourceType = 0;                                            //0(��) 1(�ϴ�) 2(��)
  public List<Vector2Int> RiverCoors= new List<Vector2Int>();           //��� �� ��ǥ
  public List<int> RiverDirs=new List<int>();                           //��� �� ���� ����(���� ����)
}