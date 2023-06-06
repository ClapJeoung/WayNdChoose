using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ImageHolder")]

public class ImageHolder : ScriptableObject
{
  public List<Sprite> TownIllust=new List<Sprite>();                 //마을 일러스트
  public List<Sprite> CityIllust=new List<Sprite>();                 //도시 일러스트
  public List<Sprite> CastleIllust=new List<Sprite>();               //성채 일러스트
  [Space(10)]
  public Sprite UnknownTheme = null;
  public Sprite UnknownExp = null;
  [Space(10)]
  public Sprite ThemeIllust_Conversation = null;
  public Sprite ThemeIllust_Force = null, ThemeIllust_Wild = null, ThemeIllust_Intelligence = null;
  [Space(10)]
  public Sprite[] TendencyIcon_Body = new Sprite[5];
  public Sprite[] TendencyIllust_Body=new Sprite[5];
  public Sprite[] TendencyIcon_Head=new Sprite[5];
  public Sprite[] TendencyIllust_Head = new Sprite[5];
  public Sprite GetTendencyIcon(TendencyType type,int level)
  {
    if(type.Equals(TendencyType.Body))return TendencyIcon_Body[level+2];
    else return TendencyIcon_Head[level+2];
  }
  public Sprite GettendencyIllust(TendencyType type,int level)
  {
    if (type.Equals(TendencyType.Body)) return TendencyIllust_Body[level + 2];
    else return TendencyIllust_Head[level + 2];
  }
  [Space(10)]
  public Sprite TendencyIllust_Rational = null;
  public Sprite TendencyIllust_Physical = null, TendencyIllust_Mental = null, TendencyIllust_Material = null;
  [Space(10)]
  public Sprite ThemeIcon_Conversation = null;
  public Sprite ThemeIcon_Force = null;
  public Sprite ThemeIcon_Wild = null;
  public Sprite ThemeIcon_Intelligence = null;
  [Space(10)]
  public Sprite TendencySelectionIcon = null;
  public Sprite ExpSelectionIcon = null;
  public Sprite SkillSelectionIcon = null;
    public Sprite NoneIllust = null;
  public Sprite ResidenceIcon = null;
  public Sprite MarketPlaceIcon = null;
  public Sprite TempleIcon = null;
  public Sprite LibraryIcon = null;
  public Sprite TheaterIcon = null;
  public Sprite AcademyIcon = null;
  public Sprite GetPlaceIcon(PlaceType placetype)
  {
    switch (placetype)
    {
      case PlaceType.Residence:return ResidenceIcon;
      case PlaceType.Marketplace:return MarketPlaceIcon;
      case PlaceType.Temple:return TempleIcon;
      case PlaceType.Library:return LibraryIcon;
      case PlaceType.Theater:return TheaterIcon;
      default:return AcademyIcon;
    }
  }
  [Space(10)]
    public List<Sprite> EventIllust = new List<Sprite>();              //모든 이벤트 일러스트
  public List<Sprite> EXPIllust = new List<Sprite>();                  //모든 경험 일러스트
  public List<Sprite> EndingIllusts=new List<Sprite>();//엔딩 일러스트
  public Sprite GetEndingIllust(string index)
  {
    foreach (var _temp in EndingIllusts)
      if (_temp.name.Equals(index)) return _temp;

    return DefaultIllust;
  }
  public Sprite GetRandomMainIllust(Sprite lastillust)
  {
    Sprite _spr=Random.Range(0,2).Equals(0)?EventIllust[Random.Range(0,EventIllust.Count)]:EXPIllust[Random.Range(0,EXPIllust.Count)];
    while(_spr==lastillust) _spr = Random.Range(0, 2).Equals(0) ? EventIllust[Random.Range(0, EventIllust.Count)] : EXPIllust[Random.Range(0, EXPIllust.Count)];
    return _spr;
  }//대문에 넣을 일러스트 무작위로 전달
  public Sprite NoGoldIllust = null;
    public Sprite EmptyLongExpIcon = null;
    public Sprite EmptyShortExpIcon = null;
  public Sprite DefaultIllust = null;                                 //빈 일러스트
  public Sprite DefaultIcon = null;
    [Space(10)]
    public Sprite SpringIcon_active = null;
  public Sprite SpringIcon_deactive = null;
  public Sprite SummerIcon_active = null;
  public Sprite SummerIcon_deactive = null;
  public Sprite FallIcon_active = null;
  public Sprite FallIcon_deactive = null;
  public Sprite WinterIcon_active = null;
  public Sprite WinterIcon_deactive = null;
  [Space(10)]
  public Sprite HPIcon = null;
  public Sprite HPIncreaseIcon = null;
  public Sprite HPDecreaseIcon = null;
  public Sprite SanityIcon = null;
  public Sprite SanityIncreaseIcon = null;
  public Sprite SanityDecreaseIcon = null;
  public Sprite GoldIcon = null;
  public Sprite GoldIncreaseIcon = null;
  public Sprite GoldDecreaseIcon = null;
  [Space(10)]
  public Sprite Quest_risig = null;
  public Sprite Quest_climax = null;
  public Sprite Quest_fall = null;
  [Space(10)]
  public Sprite DisComfort = null;
  public Sprite MadnessIdle = null;
  public Sprite MadnessActive = null;
  public Sprite Arrow_DeActive = null;
  public Sprite Arrow_Active_rational = null;
  public Sprite Arrow_Active_physical = null;
  public Sprite Arrow_Active_mental = null;
  public Sprite Arrow_Active_material = null;
  public Sprite Arrow_Active(TendencyType tendencytype,bool dir)
  {
    switch (tendencytype)
    {
      case TendencyType.Body:
        if (dir.Equals(false)) return Arrow_Active_rational;
        else return Arrow_Active_physical;
      case TendencyType.Head:
        if (dir.Equals(false)) return Arrow_Active_mental;
        else return Arrow_Active_material;
      default: return DefaultIcon;
    }
  }
  [Space(10)]
  public Sprite SelectionBackground_none = null;
  public Sprite SelectionBackground_rational = null;
  public Sprite SelectionBackground_physical = null;
  public Sprite SelectionBackground_mental = null;
  public Sprite SelectionBackground_material = null;
  [Space(10)]
  public Sprite[] NullEnvirIllust=new Sprite[0];
  public Sprite[] RiverEnvirIllust=null;
  public Sprite[] ForestEnvirIllust = null;
  public Sprite[] HighlandEnvirIllust = null;
  public Sprite[] MountainEnvirIllust = null;
  public Sprite[] SeaEnvirIllust = null;
  [Space(10)]
  public Sprite RiverTile = null;
  public Sprite ForestTile=null,HighlandTile=null,MountainTile=null,SeaTile=null;
  public Sprite GetEnvirTile(EnvironmentType envir)
  {
    switch (envir)
    {
      case EnvironmentType.River: return RiverTile;
      case EnvironmentType.Forest: return ForestTile;
      case EnvironmentType.Highland: return HighlandTile;
      case EnvironmentType.Mountain: return MountainTile;
      case EnvironmentType.Sea: return SeaTile;
      default: return DefaultIcon;
    }
  }
  public Sprite GetEnvirBackground(EnvironmentType envir)
  {
    switch (envir)
    {
      case EnvironmentType.River:
        return RiverEnvirIllust[GameManager.Instance.MyGameData.Turn];
      case EnvironmentType.Forest:
        return ForestEnvirIllust[GameManager.Instance.MyGameData.Turn];
      case EnvironmentType.Highland:
        return HighlandEnvirIllust[GameManager.Instance.MyGameData.Turn];
      case EnvironmentType.Mountain:
        return MountainEnvirIllust[GameManager.Instance.MyGameData.Turn];
      case EnvironmentType.Sea:
        return SeaEnvirIllust[GameManager.Instance.MyGameData.Turn];
      default:
        return NullEnvirIllust[GameManager.Instance.MyGameData.Turn];
    }
  }
  public Sprite SelectionBackground (TendencyType tendencytype,bool dir)
  {
    switch (tendencytype)
    {
      case TendencyType.Body:
        if (dir.Equals(false)) return SelectionBackground_rational;
        else return SelectionBackground_physical;
      case TendencyType.Head:
        if (dir.Equals(false)) return SelectionBackground_mental;
        else return SelectionBackground_material;
      default:return SelectionBackground_none;
    }
  }
  public Sprite GetThemeIcon(ThemeType _type)
  {
    switch (_type)
    {
      case ThemeType.Conversation:return ThemeIcon_Conversation;
      case ThemeType.Force:return ThemeIcon_Force;
        case ThemeType.Wild:return ThemeIcon_Wild;
      default:return ThemeIcon_Intelligence;
    }
  }
  public Sprite GetThemeIllust(ThemeType _type)
  {
    switch (_type)
    {
      case ThemeType.Conversation: return ThemeIllust_Conversation;
      case ThemeType.Force: return ThemeIllust_Force;
      case ThemeType.Wild: return ThemeIllust_Wild;
      default: return ThemeIllust_Intelligence;
    }
  }

  public void GetSkillIcons(SkillName _skill,ref Sprite[] _sprites)
  {
    switch (_skill)
    {
      case SkillName.Speech: _sprites[0] = ThemeIcon_Conversation;_sprites[1] = ThemeIcon_Conversation;break;
      case SkillName.Threat: _sprites[0] = ThemeIcon_Conversation; _sprites[1] = ThemeIcon_Force; break;
      case SkillName.Deception : _sprites[0] = ThemeIcon_Conversation; _sprites[1] = ThemeIcon_Wild; break;
      case SkillName.Logic : _sprites[0] = ThemeIcon_Conversation; _sprites[1] = ThemeIcon_Intelligence; break;
      case SkillName.Martialarts : _sprites[0] = ThemeIcon_Force; _sprites[1] = ThemeIcon_Force; break;
      case SkillName.Bow : _sprites[0] = ThemeIcon_Force; _sprites[1] = ThemeIcon_Wild; break;
      case SkillName.Somatology: _sprites[0] = ThemeIcon_Force; _sprites[1] = ThemeIcon_Intelligence; break;
      case SkillName.Survivable : _sprites[0] = ThemeIcon_Wild; _sprites[1] = ThemeIcon_Wild; break;
      case SkillName.Biology : _sprites[0] = ThemeIcon_Wild; _sprites[1] = ThemeIcon_Intelligence; break;
      case SkillName.Knowledge : _sprites[0] = ThemeIcon_Intelligence; _sprites[1] = ThemeIcon_Intelligence; break;
    }
  }
    public Sprite GetTownSprite(string _name)
  {
    Sprite _targetsprite = DefaultIllust;
    foreach (Sprite _spr in TownIllust)
    {
      if (_spr.name.Equals(_name))
      {
        _targetsprite = _spr;
        break;
      }
    }
    return _targetsprite;
  }//마을 이름에 해당하는 일러스트 가져오기
  public Sprite GetCitySprite(string _name)
  {
    Sprite _targetsprite = DefaultIllust;
    foreach (Sprite _spr in CityIllust)
    {
      if (_spr.name.Equals(_name))
      {
        _targetsprite = _spr;
        break;
      }
    }
    return _targetsprite;
  }//도시 이름에 해당하는 일러스트 가져오기
  public Sprite GetCastleSprite(string _name)
  {
    Sprite _targetsprite = DefaultIllust;
    foreach (Sprite _spr in CastleIllust)
    {
      if (_spr.name.Equals(_name))
      {
        _targetsprite = _spr;
        break;
      }
    }
    return _targetsprite;
  }//성채 이름에 해당하는 일러스트 가져오기

  public Sprite GetEventStartIllust(string _illustid)
  {
    //계절 적용된 id
    foreach (Sprite _spr in EventIllust)
    {
      if (_spr.name.Equals(_illustid))
      {
        return _spr;
      }
    }
    Debug.Log(_illustid);
      return DefaultIllust;
  }//ID로 이벤트 일러스트 가져오기(계절별)
  public Sprite GetEventFailIllusts(string _illustid, string _index)
  {
    //계절 적용된 id
    string _temp = _illustid + "_" + _index + "_fail";
    foreach (Sprite _spr in EventIllust)
    {
      if (_spr.name.Equals(_temp))
      {
        return _spr;
      }
    }
    return DefaultIllust;
  }
  public Sprite GetEventSuccessIllusts(string _illustid, string _index)
  {
    //계절 적용된 id
    string _temp = _illustid + "_" + _index + "_success";
    foreach (Sprite _spr in EventIllust)
    {
      if (_spr.name.Equals(_temp))
      {
        return _spr;
      }
    }


    return DefaultIllust;
  }

  public Sprite GetEXPIllust(string _illustid)
  {
    Sprite _targetsprite = DefaultIllust;
    foreach (Sprite _spr in EXPIllust)
    {
      if (_spr.name.Equals(_illustid))
      {
        _targetsprite = _spr;
        break;
      }
    }
    return _targetsprite;
  }//ID로 경험 일러스트 가져오기

}
