using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
public class Settlement
{
    public int IllustIndex = 0;
  public string Name;

  public bool IsRiver=false;//주변 2칸에 강 여부
  public bool IsForest = false;//주변 1칸에 숲 여부
  public bool IsMine = false;  //주변 1칸에 언덕 여부
  public bool IsMountain = false;//주변 2칸에 산 여부
  public bool IsSea = false;    //주변 1칸에 바다 여부

  public List<Vector3Int> Pose=new List<Vector3Int>();
  public bool IsOpen = false;

    public int Wealth = 0;      //부
    public int Faith = 0;       //신앙
    public int Culture = 0;     //문화
    public int Science = 0;     //과학
  public Vector3Int VectorPos()
  {
    Vector3Int _pos = Vector3Int.zero;
    foreach (var asdf in Pose) _pos += asdf;
    return _pos / Pose.Count;
  }
}
public class MapSaveData
{
    public int Size = 0;
    public int[] BottomMapCode;
    public int[] BottomTileCode;
    public int[] TopMapCode;
    public int[] TopTileCode;
    public int[] RotCode;
    public Vector3Int[] Town_Pos;
    public Vector3Int[] City_Pos;
    public Vector3Int[] Castle_Pos;
  public string[] Town_Names,City_Names, Castle_Names;
    public int[] Town_Index, City_Index, Castle_Index;
  public bool[] Town_Open;
  public bool[] City_Open;
  public bool[] Castle_Open;
    public int TownCount, CityCount, CastleCount;
    public int[]   Wealth_town, Faith_town, Culture_town, Science_town;
  public bool[] Isriver_town, Isforest_town, Ismine_town, Ismountain_town,Issea_town;
  public int[] Wealth_city, Faith_city, Culture_city, Science_city;
  public bool[] Isriver_city, Isforest_city, Ismine_city, Ismountain_city,Issea_city;
  public int[]  Wealth_castle, Faith_castle, Culture_castle, Science_castle;
  public bool[] Isriver_castle, Isforest_castle, Ismine_castle, Ismountain_castle,Issea_castle;

  public MapData ConvertToMapData()
  {
    MapData _mapdata = new MapData();
    _mapdata.MapCode_Bottom = new int[Size, Size];
    _mapdata.MapCode_Top = new int[Size, Size];
    for(int i=0;i<Size;i++)
      for(int j=0;j<Size;j++)
      {
        _mapdata.MapCode_Bottom[j, i] = BottomMapCode[i * Size + j];
        _mapdata.MapCode_Top[j, i] = TopMapCode[i * Size + j];
      }
    for(int i = 0; i < TownCount; i++)
    {
      Settlement _town = new Settlement();
      _town.Name = Town_Names[i];
      _town.IsRiver = Isriver_town[i];
      _town.IsForest= Isforest_town[i];
      _town.IsMountain= Ismountain_town[i];
      _town.IsMine= Ismine_town[i];
      _town.IsSea = Issea_town[i];

      _town.Pose.Add(Town_Pos[i]);
      _town.IsOpen = Town_Open[i];

      _town.Wealth= Wealth_town[i];
      _town.Faith= Faith_town[i];
      _town.Culture= Culture_town[i];
      _town.Science= Science_town[i];

            _town.IllustIndex = Town_Index[i];

      _mapdata.Towns.Add( _town);
      _mapdata.AllSettles.Add( _town);
    }
    for (int i = 0; i < CityCount; i++)
    {
      Settlement _city = new Settlement();
      _city.Name = City_Names[i];
      _city.IsRiver = Isriver_city[i];
      _city.IsForest = Isforest_city[i];
      _city.IsMountain = Ismountain_city[i];
      _city.IsMine = Ismine_city[i];
      _city.IsSea = Issea_city[i];

      _city.Pose.Add(City_Pos[i*2]);
      _city.Pose.Add(City_Pos[i * 2+1]);
      _city.IsOpen = City_Open[i];

      _city.Wealth = Wealth_city[i];
      _city.Faith = Faith_city[i];
      _city.Culture = Culture_city[i];
      _city.Science = Science_city[i];
            _city.IllustIndex = City_Index[i];

            _mapdata.Cities.Add( _city);
      _mapdata.AllSettles.Add( _city);
    }
    for (int i = 0; i < CastleCount; i++)
    {
      Settlement _castle = new Settlement();
      _castle.Name = Castle_Names[i];
      _castle.IsRiver = Isriver_castle[i];
      _castle.IsForest = Isforest_castle[i];
      _castle.IsMountain = Ismountain_castle[i];
      _castle.IsMine = Ismine_castle[i];
      _castle.IsSea = Issea_castle[i];

      _castle.Pose.Add(Castle_Pos[i * 3]);
      _castle.Pose.Add(Castle_Pos[i * 3 + 1]);
      _castle.Pose.Add(Castle_Pos[i * 3 + 2]);
      _castle.IsOpen = Castle_Open[i];

      _castle.Wealth = Wealth_castle[i];
      _castle.Faith = Faith_castle[i];
      _castle.Culture = Culture_castle[i];
      _castle.Science = Science_castle[i];
            _castle.IllustIndex = Castle_Index[i];

            _mapdata.Castles.Add( _castle);
      _mapdata.AllSettles.Add( _castle);
    }

    return _mapdata;
  }
  public void UpdateSaveData(MapData _data)
  {
    int i = 0;
    foreach(var data in _data.Towns)
    {
      Town_Open[i] = data.IsOpen;
      i++;
    }
    i = 0;
    foreach (var data in _data.Cities)
    {
      City_Open[i] = data.IsOpen;
      i++;
    }
    i = 0;
    foreach (var data in _data.Castles)
    {
      Castle_Open[i] = data.IsOpen;
      i++;
    }
  }
}
public class MapData
{
  public int[,] MapCode_Bottom;
  public int[,] MapCode_Top;
  public List<Settlement> Towns = new List<Settlement>();
  public List<Settlement> Cities =new List<Settlement>();
  public List<Settlement> Castles =new List<Settlement>();
  public List<Settlement> AllSettles = new List<Settlement>();
  public List<Settlement> GetCloseSettles(Settlement _origin,int _count)
  {
    Vector3Int _originpos = _origin.VectorPos();

    List<float> _distance=new List<float>();
    List<Settlement> _settlement=new List<Settlement>();
    foreach (Settlement _settle in AllSettles)
    {
      if (_settle == _origin) continue;

      float _dis=Vector3Int.Distance(_settle.VectorPos(), _originpos);
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
}
public static class SettlementName
{
  public static string[] TownNams =
  {
    "마을0","마을1","마을2","마을3","마을4","마을5","마을6","마을7","마을8","마을9","마을10","마을11","마을12"
  };
  public static string[] CityNames =
  {
    "도시0","도시1","도시2","도시3","도시4","도시5","도시6","도시7","도시8","도시9","도시10","도시11","도시12"
  };
  public static string[] CastleNames =
  {
    "성채0","성채1","성채2","성채3","성채4","성채5","성채6","성채7","성채8","성채9","성채10","성채11","성채12"
  };
}

