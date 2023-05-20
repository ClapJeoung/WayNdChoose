using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using static UnityEditor.Progress;

public class Settlement
{
  public ThemeType LibraryType = ThemeType.Conversation;
    public int IllustIndex = 0;
  public SettlementType Type;
  public int NameIndex;
  public string OriginName = "";
  private TextData NameTextData = null;
  private string name;
  public string Name { 
    get
    {
      if (NameTextData == null)
      {
      }
      return NameTextData.Name;
    }
  }
  public void Setup()
  {
    switch (Type)
    {
      case SettlementType.Town:
        OriginName = SettlementName.TownNams[NameIndex];
        NameTextData = GameManager.Instance.GetTextData(OriginName);
        SpringIllust = GameManager.Instance.ImageHolder.GetTownSprite($"{OriginName}_spring");
        SummerIllust = GameManager.Instance.ImageHolder.GetTownSprite($"{OriginName}_summer");
        FallIllust = GameManager.Instance.ImageHolder.GetTownSprite($"{OriginName}autumn");
        WinterIllust = GameManager.Instance.ImageHolder.GetTownSprite($"{OriginName}_winter"); 
        break;
      case SettlementType.City:
        OriginName = SettlementName.CityNames[NameIndex];
        NameTextData = GameManager.Instance.GetTextData(OriginName);
        SpringIllust = GameManager.Instance.ImageHolder.GetCitySprite($"{OriginName}_spring");
        SummerIllust = GameManager.Instance.ImageHolder.GetCitySprite($"{OriginName}_summer");
        FallIllust = GameManager.Instance.ImageHolder.GetCitySprite($"{OriginName}autumn");
        WinterIllust = GameManager.Instance.ImageHolder.GetCitySprite($"{OriginName}_winter"); break;
      case SettlementType.Castle:
        OriginName = SettlementName.CastleNames[NameIndex]; 
        NameTextData = GameManager.Instance.GetTextData(OriginName);
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

  public List<Vector3Int> Pose=new List<Vector3Int>();//일반 타일맵 기준

    public int Wealth = 0;      //부
    public int Faith = 0;       //신앙
    public int Culture = 0;     //문화
    public int Science = 0;     //과학
  public Vector3 VectorPos
  {
    get
    {
      Vector3Int _pos = Vector3Int.zero;
      foreach (var asdf in Pose) _pos += asdf;
      return _pos / Pose.Count;
    }
  }
  private TargetTileEventData tiledata = null;
  public TargetTileEventData TileData
  {
    get { 
      if(tiledata == null)
      {
        tiledata= new TargetTileEventData();
        tiledata.SettlementType = Type;
        tiledata.PlaceData.Add(PlaceType.Residence, Wealth);
        tiledata.PlaceData.Add(PlaceType.Marketplace, Wealth);
        tiledata.PlaceData.Add(PlaceType.Temple, Faith);
        switch (Type)
        {
          case SettlementType.Town:
            break;
          case SettlementType.City:
            tiledata.PlaceData.Add(PlaceType.Library, Culture);
            break;
          case SettlementType.Castle:
            tiledata.PlaceData.Add(PlaceType.Theater, Culture);
            tiledata.PlaceData.Add(PlaceType.Academy, Science);
            break;
        }
        tiledata.EnvironmentType.Add(EnvironmentType.River);
        if (IsRiver) tiledata.EnvironmentType.Add(EnvironmentType.River);
        if (IsForest) tiledata.EnvironmentType.Add(EnvironmentType.Forest);
        if (IsHighland) tiledata.EnvironmentType.Add(EnvironmentType.Highland);
        if (IsMountain) tiledata.EnvironmentType.Add(EnvironmentType.Mountain);
        if (IsSea) tiledata.EnvironmentType.Add(EnvironmentType.Sea);
        tiledata.Season = GameManager.Instance.MyGameData.Turn + 1;

      }
      return tiledata; 
    }
  }
  public bool CheckAbleEvent(EventDataDefulat _event)
  {
    if (_event.SettlementType.Equals(SettlementType.Outer)) return false;
    //바깥 이벤트일 경우 X
    if (!_event.SettlementType.Equals(SettlementType.None))
      if (_event.SettlementType.Equals(TileData.SettlementType)) return false;
    //전역 정착지 이벤트가 아닐 때 정착지 타입이 맞지 않으면 X
    if (_event.TileCheckType == 1)
    {
      if (!TileData.EnvironmentType.Contains(_event.EnvironmentType)) return false;
    }//환경 요구일 때 요구하는 환경이 하나도 없을 경우 X
    else if(_event.TileCheckType == 2)
    {
      if (!TileData.PlaceData[_event.PlaceType].Equals(_event.PlaceLevel)) return false;
    }//레벨 요구일 떄 요구하는 장소 레벨이 맞지 않을 경우 X
    return true;
  }
    public List<PlaceType> AvailabePlaces
    {
        get
        {
            if (availabeplaces.Count.Equals(0))
            {
                List<PlaceType> _places = new List<PlaceType>();

                _places.Add(PlaceType.Residence);
                _places.Add(PlaceType.Marketplace);
                _places.Add(PlaceType.Temple);
                switch (Type)
                {
                    case SettlementType.Town:
                        break;
                    case SettlementType.City:
                        _places.Add(PlaceType.Library);
                        break;
                    case SettlementType.Castle:
                        _places.Add(PlaceType.Theater);
                        _places.Add(PlaceType.Academy);
                        break;
                }
                availabeplaces = _places;
            }
            return availabeplaces;
        }
    }
    private List<PlaceType> availabeplaces = new List<PlaceType>();
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
  public int[] Town_NameIndex,City_NameIndex, Castle_NameIndex;
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
      _town.NameIndex = Town_NameIndex[i];
      _town.Type = SettlementType.Town;
      _town.IsRiver = Isriver_town[i];
      _town.IsForest= Isforest_town[i];
      _town.IsMountain= Ismountain_town[i];
      _town.IsHighland= Ismine_town[i];
      _town.IsSea = Issea_town[i];

      _town.Pose.Add(Town_Pos[i]);

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
      _city.NameIndex = City_NameIndex[i];
      _city.Type = SettlementType.City;
      _city.IsRiver = Isriver_city[i];
      _city.IsForest = Isforest_city[i];
      _city.IsMountain = Ismountain_city[i];
      _city.IsHighland = Ismine_city[i];
      _city.IsSea = Issea_city[i];

      _city.Pose.Add(City_Pos[i*2]);
      _city.Pose.Add(City_Pos[i * 2+1]);

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
      _castle.NameIndex = Castle_NameIndex[i];
      _castle.Type = SettlementType.Castle;
      _castle.IsRiver = Isriver_castle[i];
      _castle.IsForest = Isforest_castle[i];
      _castle.IsMountain = Ismountain_castle[i];
      _castle.IsHighland = Ismine_castle[i];
      _castle.IsSea = Issea_castle[i];

      _castle.Pose.Add(Castle_Pos[i * 3]);
      _castle.Pose.Add(Castle_Pos[i * 3 + 1]);
      _castle.Pose.Add(Castle_Pos[i * 3 + 2]);

      _castle.Wealth = Wealth_castle[i];
      _castle.Faith = Faith_castle[i];
      _castle.Culture = Culture_castle[i];
      _castle.Science = Science_castle[i];
            _castle.IllustIndex = Castle_Index[i];

            _mapdata.Castles.Add( _castle);
      _mapdata.AllSettles.Add( _castle);
    }

    foreach (var _settle in _mapdata.AllSettles) _settle.Setup();
    return _mapdata;
  }
  public void UpdateSaveData(MapData _data)
  {
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
    Vector2 _originpos = _origin.VectorPos;

    List<float> _distance=new List<float>();
    List<Settlement> _settlement=new List<Settlement>();
    foreach (Settlement _settle in AllSettles)
    {
      if (_settle == _origin) continue;

      float _dis=Vector2.Distance(_settle.VectorPos, _originpos);
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

