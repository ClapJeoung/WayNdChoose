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
  public Sprite UnknownHP = null;
  public Sprite UnknownSanity = null;
  public Sprite UnknownGold = null;
  public Sprite UnknownTrait = null;
  public Sprite UnknownTheme = null;
  public Sprite UnknownExp = null;
  [Space(10)]
  public Sprite ThemeIllust_Conversation = null;
  public Sprite ThemeIllust_Force = null, ThemeIllust_Wild = null, ThemeIllust_Intelligence = null;
    [Space(10)]
  public List<Sprite> SkillIllust_Speech = new List<Sprite>();       
  public List<Sprite> SkillIllust_Threat = new List<Sprite>();
  public List<Sprite> SkillIllust_Deception = new List<Sprite>();
  public List<Sprite> SkillIllust_Logic = new List<Sprite>();
  public List<Sprite> SkillIllust_Martialarts = new List<Sprite>();
  public List<Sprite> SkillIllust_Bow = new List<Sprite>();
  public List<Sprite> SkillIllust_Somatology = new List<Sprite>();
  public List<Sprite> SkillIllust_Survivable = new List<Sprite>();
  public List<Sprite> SkillIllust_Biology = new List<Sprite>();
  public List<Sprite> SkillIllust_Knowledge = new List<Sprite>();
  [Space(10)]
  public Sprite[] TendencyIcon_Rational=new Sprite[7];
  public Sprite[] TendencyIcon_Physical = new Sprite[7], TendencyIcon_Mental = new Sprite[7], TendencyIcon_Material = new Sprite[7];
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
  [Space(10)]
  public List<Sprite> ResidenceIllust = null;
  public List<Sprite> MarketPlaceIllust = null;
  public List<Sprite> TempleIllust = null;
  public List<Sprite> LibraryIllust = null;
  public List<Sprite> TheaterIllust = null;
  public List<Sprite> AcademyIllust = null;
  public Sprite GetPlaceIllust(PlaceType _placetype)
  {
    List<Sprite> _list = new List<Sprite>();
    switch (_placetype)
    {
      case PlaceType.Residence: _list = ResidenceIllust;break;
      case PlaceType.Marketplace: _list = MarketPlaceIllust; break;
      case PlaceType.Temple: _list = TempleIllust; break;
      case PlaceType.Library: _list = LibraryIllust; break;
      case PlaceType.Theater: _list = TheaterIllust; break;
      case PlaceType.Academy: _list = AcademyIllust; break;
    }
    if (_list.Count.Equals(0)) return DefaultIllust;
    return _list[Random.Range(0, _list.Count)];
  }
  [Space(10)]
    public List<Sprite> EventIllust = new List<Sprite>();              //모든 이벤트 일러스트
  public List<Sprite> EXPIllust = new List<Sprite>();                  //모든 경험 일러스트
  public Sprite NoGoldIllust = null;
  public Sprite EmptyExpIllust = null;
  public Sprite EmptyShortExpIcon = null;
  public Sprite EmptyLongExpIcon = null;
  public List<Sprite> TraitIcons=new List<Sprite>();
  public List<Sprite> TraitIllust = new List<Sprite>();                //모든 특성 일러스트
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
    public Sprite NothingQuestIllust = null;
  [Space(10)]
  public Sprite HPIcon = null;
  public Sprite SanityIcon = null;
  public Sprite GoldIcon = null;
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

  public Sprite GetSkillSprite(SkillName _skillname,int _level)
  {
    List<Sprite> _temp =new List<Sprite>();
    switch (_skillname)
    {
      case SkillName.Biology:_temp = SkillIllust_Biology;break;
      case SkillName.Bow: _temp = SkillIllust_Bow; break;
      case SkillName.Deception: _temp = SkillIllust_Deception; break;
      case SkillName.Knowledge: _temp = SkillIllust_Knowledge; break;
      case SkillName.Logic: _temp = SkillIllust_Logic; break;
      case SkillName.Martialarts: _temp = SkillIllust_Martialarts; break;
      case SkillName.Somatology: _temp = SkillIllust_Somatology; break;
      case SkillName.Speech: _temp = SkillIllust_Speech; break;
      case SkillName.Survivable: _temp = SkillIllust_Survivable; break;
      case SkillName.Threat: _temp = SkillIllust_Threat; break;
    }
    if (_temp.Count < _level) return DefaultIllust;
    return _temp[_level - 1];
  }
  public Sprite[] GetEventStartIllust(string _illustid)
  {
    //계절 적용된 id
    Sprite[] _illusts = new Sprite[4];
    for(int i=0;i< _illusts.Length; i++)
    {
     string _temp = _illustid;
      _illusts[i] = DefaultIllust;
      foreach(Sprite _spr in EventIllust)
      {
        if (_spr.name.Equals(_temp))
        {
          _illusts[i] = _spr;
          break;
        }
      }
    }
    return _illusts;
  }//ID로 이벤트 일러스트 가져오기
  public Sprite[] GetEventFailIllusts(string _illustid, string _index)
  {
    //계절 적용된 id
    Sprite[] _illusts = new Sprite[4];
    for (int i = 0; i < _illusts.Length; i++)
    {
      string _temp = _illustid  + "_" + _index + "_fail";
      foreach (Sprite _spr in EventIllust)
      {
        if (_spr.name.Equals(_temp))
        {
          _illusts[i] = _spr;
          break;
        }
      }

      if (_illusts[i] == null)
      {
        _temp = _illustid + "_" + _index + "_fail";
        foreach (Sprite _spr in EventIllust)
        {
          if (_spr.name.Equals(_temp))
          {
            _illusts[i] = _spr;
            break;
          }
        }
        if (_illusts[i] == null) _illusts[i] = DefaultIllust;
      }

    }
    return _illusts;
  }
  public Sprite[] GetEventSuccessIllusts(string _illustid, string _index)
  {
    //계절 적용된 id
    Sprite[] _illusts = new Sprite[4];
    for (int i = 0; i < _illusts.Length; i++)
    {
      string _temp = _illustid + "_" + _index + "_success";
      foreach (Sprite _spr in EventIllust)
      {
        if (_spr.name.Equals(_temp))
        {
          _illusts[i] = _spr;
          break;
        }
      }
        if (_illusts[i] == null) _illusts[i] = DefaultIllust;
    }
    return _illusts;
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
  public Sprite GetTraitIcon(string _illustid)
  {
    Sprite _targetsprite = DefaultIcon;
    foreach (Sprite _spr in TraitIcons)
    {
      if (_spr.name.Equals(_illustid))
      {
        _targetsprite = _spr;
        break;
      }
    }
    return _targetsprite;
  }
  public Sprite GetTraitIllust(string _illustid)
  {
    Sprite _targetsprite = DefaultIllust;
    foreach (Sprite _spr in TraitIllust)
    {
      if (_spr.name.Equals(_illustid))
      {
        _targetsprite = _spr;
        break;
      }
    }
    return _targetsprite;
  }//ID로 특성 일러스트 가져오기
  public Sprite GetTendencyIcon(TendencyType _type,int _level)
  {
    int _index = 0;
    switch (_type)
    {
      case TendencyType.Rational:
        _index = _level * -1;
        return TendencyIcon_Rational[_index+3];
      case TendencyType.Physical:
        _index = _level * +1;
        return TendencyIcon_Physical[_index + 3];
      case TendencyType.Mental:
        _index = _level * -1;
        return TendencyIcon_Mental[_index + 3];
      case TendencyType.Material:
        _index = _level * +1;
        return TendencyIcon_Material[_index + 3];
      default: return DefaultIcon ;
    }
  }
  public Sprite GetTendencyIllust(TendencyType _type)
  {
    switch (_type)
    {
      case TendencyType.Rational: return TendencyIllust_Rational;
      case TendencyType.Physical: return TendencyIllust_Physical;
      case TendencyType.Mental: return TendencyIllust_Mental;
      case TendencyType.Material: return TendencyIllust_Material;
      default: return DefaultIllust;
    }
  }

}
