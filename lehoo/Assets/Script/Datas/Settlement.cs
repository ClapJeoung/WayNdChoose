using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class Settlement
{
  public ThemeType LibraryType = ThemeType.Conversation;
  
  public List<PlaceType> EnablePlaces=new List<PlaceType>();
  public SettlementType Type;
  public int InfoIndex;
  public string OriginName
  {
    get
    {
      switch (Type)
      {
        case SettlementType.Town:
          return SettlementName.TownNams[InfoIndex];
        case SettlementType.City:
          return SettlementName.CityNames[InfoIndex];
        default:
          return SettlementName.CastleNames[InfoIndex];
      }
    }
  }
  private TextData NameTextData = null;
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
        NameTextData = GameManager.Instance.GetTextData(OriginName);
        SpringIllust = GameManager.Instance.ImageHolder.GetTownSprite($"{OriginName}_spring");
        SummerIllust = GameManager.Instance.ImageHolder.GetTownSprite($"{OriginName}_summer");
        FallIllust = GameManager.Instance.ImageHolder.GetTownSprite($"{OriginName}autumn");
        WinterIllust = GameManager.Instance.ImageHolder.GetTownSprite($"{OriginName}_winter"); 
        break;
      case SettlementType.City:
        NameTextData = GameManager.Instance.GetTextData(OriginName);
        SpringIllust = GameManager.Instance.ImageHolder.GetCitySprite($"{OriginName}_spring");
        SummerIllust = GameManager.Instance.ImageHolder.GetCitySprite($"{OriginName}_summer");
        FallIllust = GameManager.Instance.ImageHolder.GetCitySprite($"{OriginName}autumn");
        WinterIllust = GameManager.Instance.ImageHolder.GetCitySprite($"{OriginName}_winter"); break;
      case SettlementType.Castle:
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

  public bool IsForest = false;//�ֺ� 1ĭ�� �� ����
  public bool IsRiver=false;//�ֺ� 2ĭ�� �� ����
  public bool IsHighland = false;  //�ֺ� 1ĭ�� ��� ����
  public bool IsMountain = false;//�ֺ� 2ĭ�� �� ����
  public bool IsSea = false;    //�ֺ� 1ĭ�� �ٴ� ����

  public List<Vector3Int> Pose=new List<Vector3Int>();//�Ϲ� Ÿ�ϸ� ����
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
        tiledata.PlaceList.Add(PlaceType.Residence);
        tiledata.PlaceList.Add(PlaceType.Marketplace);
        tiledata.PlaceList.Add(PlaceType.Temple);
        switch (Type)
        {
          case SettlementType.Town:
            break;
          case SettlementType.City:
            tiledata.PlaceList.Add(PlaceType.Library);
            break;
          case SettlementType.Castle:
            tiledata.PlaceList.Add(PlaceType.Theater);
            tiledata.PlaceList.Add(PlaceType.Academy);
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
    //�ٱ� �̺�Ʈ�� ��� X
    if (!_event.SettlementType.Equals(SettlementType.None))
      if (_event.SettlementType.Equals(TileData.SettlementType)) return false;
    //���� ������ �̺�Ʈ�� �ƴ� �� ������ Ÿ���� ���� ������ X
    if (_event.EnvironmentType != EnvironmentType.NULL)
    {
      if(!TileData.EnvironmentType.Contains(_event.EnvironmentType))
      return false;
    }
    //ȯ���� ���� ������ X

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
      if (GameManager.Instance.EventHolder.IsFollowEventEnable(TileData, _targetplce)) _followplaces.Add(_targetplce);
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
        if (GameManager.Instance.EventHolder.IsFollowEventEnable(TileData, _targetplce)) _followplaces.Add(_targetplce);
        else _normalplaces.Add(_targetplce);
        _count= ConstValues.CityPlaceCount;

        break;
      case SettlementType.Castle:
        PlaceType __targetplce = PlaceType.Theater;
        if (GameManager.Instance.EventHolder.IsFollowEventEnable(TileData, __targetplce)) _followplaces.Add(__targetplce);
        else _normalplaces.Add(__targetplce);

        PlaceType ___targetplce = PlaceType.Academy;
        if (GameManager.Instance.EventHolder.IsFollowEventEnable(TileData, ___targetplce)) _followplaces.Add(___targetplce);
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
    }//���� ������ ��Ұ� ��ǥ���� ������ �� �� ������ ������ ��ȯ
    else if (_followplaces.Count == _count)
    {
      _enableplaces = _followplaces;
    }//���� ������ ��Ұ� ��ǥ ������ �����ϸ� �״�� ��ȯ
    else
    {
      var _temp = _normalplaces.OrderBy(x => _rnd.Next()).ToList();
      for (int i = 0; i < _count - _followplaces.Count; i++)
      {
        _followplaces.Add(_temp[i]);
      }
      _followplaces.Sort();
      _enableplaces = _followplaces;
    }//���� ������ ��Ұ� ��ǥġ�� �� �����ϸ� �Ϲ� ��� �� �������� ������ ��ȯ

    EnablePlaces = _enableplaces;
  }
}
public class MapData
{
  public int[,] MapCode_Bottom;
  public int[,] MapCode_Top;
  public List<Settlement> Towns = new List<Settlement>();
  public List<Settlement> Cities =new List<Settlement>();
  public Settlement Castles = null;
  public List<Settlement> AllSettles = new List<Settlement>();
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
    Vector2 _originpos = _origin.VectorPos;

    List<float> _distance=new List<float>();
    List<Settlement> _settlement=new List<Settlement>();
    foreach (Settlement _settle in AllSettles)
    {
      if (_settle == _origin) continue;

      float _dis=Vector2.Distance(_settle.VectorPos, _originpos);
      //  Debug.Log($"_originpos ���� _settle.VectorPos()������ �Ÿ� : {_dis}");
      _distance.Add(_dis);
      _settlement.Add(_settle);
    }//��� �������κ��� �Ÿ�,�������� ��� ��ųʸ� ����

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
  }//_origin �������κ��� ���� ����� 3��
}//���� �󿡼� ���� ���� ���ϰ� ��ȯ�� ���� ������
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

