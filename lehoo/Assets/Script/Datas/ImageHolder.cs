using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 시작 성공(일반) 실패(일반) 성공,실패(성향 좌측) 성공,실패(성향 우측)
/// </summary>
public enum EventPhaseTypeEnum { Beginning,Selecting,Success,Fail,LSuccess,LFail,RSuccess,RFail}
[CreateAssetMenu(menuName ="ImageHolder")]
public class ImageHolder : ScriptableObject
{
  public List<Sprite> EventIllust = new List<Sprite>();
  public List<Sprite> EXPIllust = new List<Sprite>();
  public Sprite GetRandomMainIllust(Sprite lastillust)
  {
    Sprite _spr = EventIllust[Random.Range(0, EventIllust.Count)];
    while (_spr == lastillust) _spr = EventIllust[Random.Range(0, EventIllust.Count)];
    return _spr;
  }
  [Space(10)]
  public List<EndingDatas> EndingList = new List<EndingDatas>();//엔딩 일러스트
  public EndingDatas GetEndingData(string id)
  {
    foreach (var _temp in EndingList)
      if (_temp.ID == id) return _temp;

    return null;
  }
  [Space(20)]
  public Sprite GameOver_Idle = null;
  public Sprite GameOver_Conversation = null;
  public Sprite GameOver_Force = null;
  public Sprite GameOver_Wild = null;
  public Sprite GameOver_Intelligence = null;
  public Sprite GameOver_Madness = null;
  [Space(10)]
  public List<Sprite> Background_Village=new List<Sprite>();
  public List<Sprite> Village_spring = new List<Sprite>();
  public List<Sprite> Village_summer = new List<Sprite>();
  public List<Sprite> Village_autumn = new List<Sprite>();
  public List<Sprite> Village_winter = new List<Sprite>();
  public List<Sprite> Background_Town = new List<Sprite>();
  public List<Sprite> Town_spring= new List<Sprite>();
  public List<Sprite> Town_summer= new List<Sprite>();
  public List<Sprite> Town_autumn= new List<Sprite>();
  public List<Sprite> Town_winter=new List<Sprite>();
  public List<Sprite> Background_City = new List<Sprite>();
  public List<Sprite> City_spring = new List<Sprite>();
  public List<Sprite> City_summer = new List<Sprite>();
  public List<Sprite> City_autumn = new List<Sprite>();
  public List<Sprite> City_winter = new List<Sprite>();
  public Sprite GetSettlementIllust(SettlementType type,int season)
  {
    List<Sprite> _targetlist=new List<Sprite>();
    switch (type)
    {
      case SettlementType.Village:
        switch (season)
        {
          case 0:_targetlist = Village_spring; break;
          case 1:_targetlist = Village_summer;break;
          case 2:_targetlist = Village_autumn;break;
          case 3: _targetlist=Village_winter;break;
        }
        break;
      case SettlementType.Town:
        switch (season)
        {
          case 0: _targetlist = Town_spring; break;
          case 1: _targetlist = Town_summer; break;
          case 2: _targetlist = Town_autumn; break;
          case 3: _targetlist = Town_winter; break;
        }
        break;
      case SettlementType.City:
        switch (season)
        {
          case 0: _targetlist = City_spring; break;
          case 1: _targetlist = City_summer; break;
          case 2: _targetlist = City_autumn; break;
          case 3: _targetlist = City_winter; break;
        }
        break;
    }

    return _targetlist[Random.Range(0,_targetlist.Count)];
  }
  public Sprite GetSettlementBackground(SettlementType type,int season)
  {
    switch (type)
    {
      case SettlementType.Village:
        return Background_Village[season];
      case SettlementType.Town:
        return Background_Town[season];
      case SettlementType.City:
        return Background_City[season];
    }
    return null;
  }
  [Space(5)]
  public List<Sprite> Village_Residence_s0 = new List<Sprite>();
  public List<Sprite> Village_Residence_s1 = new List<Sprite>();
  public List<Sprite> Village_Residence_s2 = new List<Sprite>();
  public List<Sprite> Village_Residence_s3 = new List<Sprite>();
  public List<Sprite> Village_Temple_s0 = new List<Sprite>();
  public List<Sprite> Village_Temple_s1 = new List<Sprite>();
  public List<Sprite> Village_Temple_s2 = new List<Sprite>();
  public List<Sprite> Village_Temple_s3 = new List<Sprite>();
  public List<Sprite> Town_Temple_s0 = new List<Sprite>();
  public List<Sprite> Town_Temple_s1 = new List<Sprite>();
  public List<Sprite> Town_Temple_s2 = new List<Sprite>();
  public List<Sprite> Town_Temple_s3 = new List<Sprite>();
  public List<Sprite> Town_Market_s0 = new List<Sprite>();
  public List<Sprite> Town_Market_s1 = new List<Sprite>();
  public List<Sprite> Town_Market_s2 = new List<Sprite>();
  public List<Sprite> Town_Market_s3 = new List<Sprite>();
  public List<Sprite> City_Market_s0 = new List<Sprite>();
  public List<Sprite> City_Market_s1 = new List<Sprite>();
  public List<Sprite> City_Market_s2 = new List<Sprite>();
  public List<Sprite> City_Market_s3 = new List<Sprite>();
  public List<Sprite> City_Library_s0 = new List<Sprite>();
  public List<Sprite> City_Library_s1 = new List<Sprite>();
  public List<Sprite> City_Library_s2 = new List<Sprite>();
  public List<Sprite> City_Library_s3 = new List<Sprite>();
  public Sprite GetSectorIllust(SettlementType settlement, SectorTypeEnum sector,int seasen)
  {
    List<Sprite> _targetlist = new List<Sprite>();
    switch (settlement)
    {
      case SettlementType.Village:
        switch (sector)
        {
          case SectorTypeEnum.Residence:
            switch (seasen)
            {
              case 0:
                _targetlist = Village_Residence_s0;
                break;
              case 1:
                _targetlist = Village_Residence_s1;
                break;
              case 2:
                _targetlist = Village_Residence_s2;
                break;
              case 3:
                _targetlist = Village_Residence_s3;
                break;
            }
            break;
          case SectorTypeEnum.Temple:
            switch (seasen)
            {
              case 0:
                _targetlist = Village_Temple_s0;
                break;
              case 1:
                _targetlist = Village_Temple_s1;
                break;
              case 2:
                _targetlist = Village_Temple_s2;
                break;
              case 3:
                _targetlist = Village_Temple_s3;
                break;
            }
            break;
        }
        break;
      case SettlementType.Town:
        switch (sector)
        {
          case SectorTypeEnum.Temple:
            switch (seasen)
            {
              case 0:
                _targetlist = Town_Temple_s0;
                break;
              case 1:
                _targetlist = Town_Temple_s1;
                break;
              case 2:
                _targetlist = Town_Temple_s2;
                break;
              case 3:
                _targetlist = Town_Temple_s3;
                break;
            }
            break;
          case SectorTypeEnum.Marketplace:
            switch (seasen)
            {
              case 0:
                _targetlist = Town_Market_s0;
                break;
              case 1:
                _targetlist = Town_Market_s1;
                break;
              case 2:
                _targetlist = Town_Market_s2;
                break;
              case 3:
                _targetlist = Town_Market_s3;
                break;
            }
            break;
        }
        break;
      case SettlementType.City:
        switch (sector)
        {
          case SectorTypeEnum.Marketplace:
            switch (seasen)
            {
              case 0:
                _targetlist = City_Market_s0;
                break;
              case 1:
                _targetlist = City_Market_s1;
                break;
              case 2:
                _targetlist = City_Market_s2;
                break;
              case 3:
                _targetlist = City_Market_s3;
                break;
            }
            break;
          case SectorTypeEnum.Library:
            switch (seasen)
            {
              case 0:
                _targetlist = City_Library_s0;
                break;
              case 1:
                _targetlist = City_Library_s1;
                break;
              case 2:
                _targetlist = City_Library_s2;
                break;
              case 3:
                _targetlist = City_Library_s3;
                break;
            }
            break;
        }
        break;
    }
    return _targetlist[Random.Range(0, _targetlist.Count)];
  }

  public Sprite Supply_Enable = null;
  public Sprite Supply_Lack = null;
  public Sprite VillageIcon_black = null;
  public Sprite VillageIcon_white = null;
  public Sprite VillageIcon_outline = null;
  public Sprite TownIcon_black = null;
  public Sprite TownIcon_white = null;
  public Sprite TownIcon_outline = null;
  public Sprite CityIcon_black = null;
  public Sprite CityIcon_white = null;
  public Sprite CityIcon_outline = null;
  [Space(5)]
  public Sprite SkillIcon_Conversation_b = null;
  public Sprite SkillIcon_Force_b = null;
  public Sprite SkillIcon_Wild_b = null;
  public Sprite SkillIcon_Intelligence_b = null;
  public Sprite SkillIcon_Conversation_w = null;
  public Sprite SkillIcon_Force_w = null;
  public Sprite SkillIcon_Wild_w = null;
  public Sprite SkillIcon_Intelligence_w = null;
  /// <summary>
  /// 0흑 1백
  /// </summary>
  /// <param name="_type"></param>
  /// <param name="color"></param>
  /// <returns></returns>
  public Sprite GetSkillIcon(SkillTypeEnum _type,bool isblack)
  {
    switch (_type)
    {
      case SkillTypeEnum.Conversation: return isblack ? SkillIcon_Conversation_b : SkillIcon_Conversation_w;
      case SkillTypeEnum.Force: return isblack ? SkillIcon_Force_b : SkillIcon_Force_w;
      case SkillTypeEnum.Wild: return isblack ? SkillIcon_Wild_b : SkillIcon_Wild_w;
      default: return isblack ? SkillIcon_Intelligence_b : SkillIcon_Intelligence_w;
    }
  }
  [Space(10)]
  public List<Sprite> MadnessIluusts = new List<Sprite>();
  /// <summary>
  /// 대화 무력 자연 학식 체력
  /// </summary>
  /// <param name="index"></param>
  /// <returns></returns>
  public Sprite GetMadnessIllust(int index)
  {
    return MadnessIluusts[index];
  }
  [Space(10)]
  public Sprite[] TendencyIcon_Body = new Sprite[5];
 // public Sprite[] TendencyIllust_Body=new Sprite[5];
  public Sprite[] TendencyIcon_Head=new Sprite[5];
 // public Sprite[] TendencyIllust_Head = new Sprite[5];
  public Sprite StatusIcon(StatusTypeEnum status)
  {
    if (status == StatusTypeEnum.HP) return HPIcon;
    else if (status == StatusTypeEnum.Sanity) return SanityIcon;
    else return GoldIcon;
  }
  public Sprite GetTendencyIcon(TendencyTypeEnum type,int level)
  {
    if(type.Equals(TendencyTypeEnum.Body))return level==0?Transparent: TendencyIcon_Body[level+2];
    else return level==0?Transparent: TendencyIcon_Head[level+2];
  }
  /*public Sprite GettendencyIllust(TendencyTypeEnum type,int level)
  {
    if (type.Equals(TendencyTypeEnum.Body)) return TendencyIllust_Body[level + 2];
    else return TendencyIllust_Head[level + 2];
  }
  */
  [Space(10)]
  public Sprite UnknownSectorIcon = null;
  public Sprite ResidenceIcon_b = null, ResidenceIcon_w = null;
  public Sprite MarketPlaceIcon_b = null, MarketPlaceIcon_w = null;
  public Sprite TempleIcon_b = null, TempleIcon_w = null;
  public Sprite LibraryIcon_b = null, LibraryIcon_w = null;
  //public Sprite TheaterIcon = null;
  // public Sprite AcademyIcon = null;
  public Sprite GetSectorIcon(SectorTypeEnum placetype,bool isblack)
  {
    switch (placetype)
    {
      case SectorTypeEnum.Residence:return isblack? ResidenceIcon_b: ResidenceIcon_w;
      case SectorTypeEnum.Temple: return isblack?TempleIcon_b: TempleIcon_w;
      case SectorTypeEnum.Marketplace:return isblack?MarketPlaceIcon_b: MarketPlaceIcon_w;
      case SectorTypeEnum.Library:return isblack?LibraryIcon_b: LibraryIcon_w;
  //    case SectorType.Theater:return TheaterIcon;
    //  default:return AcademyIcon;
    }
    return null;
  }
  public Sprite DefaultIllust = null;                                 //빈 일러스트
  public Sprite DefaultIcon = null;
  public Sprite NoGoldIllust = null;
  public Sprite Transparent = null;
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
  public Sprite HPBroken = null;
  public Sprite SanityIcon = null;
  public Sprite SanityIncreaseIcon = null;
  public Sprite SanityDecreaseIcon = null;
  public Sprite GoldIcon = null;
  public Sprite GoldIncreaseIcon = null;
  public Sprite GoldDecreaseIcon = null;
  public Sprite UnknownExpRewardIcon = null;
  [Space(10)]
  public Sprite DisComfort = null;
  public Sprite MadnessIdle = null;
  public Sprite MadnessActive = null;
  [Space(10)]
  public Sprite Arrow_White = null;
  public Sprite Arrow_Empty = null;
  public Sprite Arrow_DeActive = null;
  public Sprite Arrow_Active_rational = null;
  public Sprite Arrow_Active_physical = null;
  public Sprite Arrow_Active_mental = null;
  public Sprite Arrow_Active_material = null;
  public Sprite Arrow_Active(TendencyTypeEnum tendencytype,bool dir)
  {
    switch (tendencytype)
    {
      case TendencyTypeEnum.Body:
        return dir ? Arrow_Active_rational : Arrow_Active_physical;
      case TendencyTypeEnum.Head:
        return dir ? Arrow_Active_mental : Arrow_Active_material;
      default: return DefaultIcon;
    }
  }
  [Space(10)]
  public Sprite SelectionBackground_none = null;
  public Sprite SelectionBackground_rational = null;
  public Sprite SelectionBackground_physical = null;
  public Sprite SelectionBackground_mental = null;
  public Sprite SelectionBackground_material = null;
  public Sprite SelectionBackground(TendencyTypeEnum tendencytype, bool dir)
  {
    switch (tendencytype)
    {
      case TendencyTypeEnum.Body:
        if (dir.Equals(true)) return SelectionBackground_rational;
        else return SelectionBackground_physical;
      case TendencyTypeEnum.Head:
        if (dir.Equals(true)) return SelectionBackground_mental;
        else return SelectionBackground_material;
      default: return SelectionBackground_none;
    }
  }
  public Sprite SelectionButtonImage_None_Idle = null;
  public Sprite SelectionButtonImage_None_Enter = null;
  public Sprite SelectionButtonImage_None_Clicked = null;
  public Sprite SelectionButtonImage_None_Disable = null;
  public Sprite SelectionButtonImage_BodyM_Idle = null;
  public Sprite SelectionButtonImage_BodyM_Enter = null;
  public Sprite SelectionButtonImage_BodyM_Clicked = null;
  public Sprite SelectionButtonImage_BodyM_Disable = null;
  public Sprite SelectionButtonImage_BodyP_Idle = null;
  public Sprite SelectionButtonImage_BodyP_Enter = null;
  public Sprite SelectionButtonImage_BodyP_Clicked = null;
  public Sprite SelectionButtonImage_BodyP_Disable = null;
  public Sprite SelectionButtonImage_HeadM_Idle = null;
  public Sprite SelectionButtonImage_HeadM_Enter = null;
  public Sprite SelectionButtonImage_HeadM_Disable = null;
  public Sprite SelectionButtonImage_HeadM_Clicked = null;
  public Sprite SelectionButtonImage_HeadP_Idle = null;
  public Sprite SelectionButtonImage_HeadP_Enter = null;
  public Sprite SelectionButtonImage_HeadP_Clicked = null;
  public Sprite SelectionButtonImage_HeadP_Disable = null;
  public SpriteState GetSelectionButtonBackground(TendencyTypeEnum tendencytype,bool dir)
  {
    SpriteState _state = new SpriteState();
    switch (tendencytype)
    {
      case TendencyTypeEnum.Body:
        if (dir.Equals(true))
        {
          _state.highlightedSprite = SelectionButtonImage_BodyM_Enter;
          _state.pressedSprite = SelectionButtonImage_BodyM_Clicked;
          _state.selectedSprite = SelectionButtonImage_BodyM_Idle;
          _state.disabledSprite = SelectionButtonImage_BodyM_Disable;
        }
        else
        {
          _state.highlightedSprite = SelectionButtonImage_BodyP_Enter;
          _state.pressedSprite = SelectionButtonImage_BodyP_Clicked;
          _state.selectedSprite = SelectionButtonImage_BodyP_Idle;
          _state.disabledSprite = SelectionButtonImage_BodyP_Disable;
        }
        break;
      case TendencyTypeEnum.Head:
        if (dir.Equals(true))
        {
          _state.highlightedSprite = SelectionButtonImage_HeadM_Enter;
          _state.pressedSprite = SelectionButtonImage_HeadM_Clicked;
          _state.selectedSprite = SelectionButtonImage_HeadM_Idle;
          _state.disabledSprite = SelectionButtonImage_HeadM_Disable;
        }
        else
        {
          _state.highlightedSprite = SelectionButtonImage_HeadP_Enter;
          _state.pressedSprite = SelectionButtonImage_HeadP_Clicked;
          _state.selectedSprite = SelectionButtonImage_HeadP_Idle;
          _state.disabledSprite = SelectionButtonImage_HeadP_Disable;
        }
        break;
      default:
        {
          _state.highlightedSprite = SelectionButtonImage_None_Enter;
          _state.pressedSprite = SelectionButtonImage_None_Clicked;
          _state.selectedSprite = SelectionButtonImage_None_Idle;
          _state.disabledSprite = SelectionButtonImage_None_Disable;
        }
        break;
    }
    return _state;
  }
  [Space(10)]
  public Sprite[] NullEnvirIllust=new Sprite[0];
  public Sprite[] RiverEnvirIllust=null;
  public Sprite[] ForestEnvirIllust = null;
  public Sprite[] HighlandEnvirIllust = null;
  public Sprite[] MountainEnvirIllust = null;
  public Sprite[] SeaEnvirIllust = null;
  public Sprite GetEnvirBackground(EnvironmentType envir)
  {
    switch (envir)
    {
      case EnvironmentType.River:
      case EnvironmentType.Sea:
      case EnvironmentType.RiverBeach:
        return RiverEnvirIllust[GameManager.Instance.MyGameData.Turn];
      case EnvironmentType.Forest:
        return ForestEnvirIllust[GameManager.Instance.MyGameData.Turn];
      case EnvironmentType.Highland:
        return HighlandEnvirIllust[GameManager.Instance.MyGameData.Turn];
      case EnvironmentType.Mountain:
        return MountainEnvirIllust[GameManager.Instance.MyGameData.Turn];
      default:
        return NullEnvirIllust[GameManager.Instance.MyGameData.Turn];
    }
  }
  [Space(10)]
  public Sprite UnknownTile = null;
  public Sprite UnknownEvent = null;
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
  public Sprite Resource_Berry_Icon = null;
  public Sprite Resource_Berry_Tile = null;
  public Sprite Resource_Wood_Icon = null;
  public Sprite Resource_Wood_Tile = null;
  public Sprite Resource_Fish_Icon = null;
  public Sprite Resource_Fish_Tile = null;
  public Sprite Resource_Swamp_Icon = null;
  public Sprite Resource_Swamp_Tile = null;
  public Sprite Resource_Stone_Icon = null;
  public Sprite Resource_Stone_Tile = null;
  public Sprite Resource_Chest = null;
  /// <summary>
  /// 베리 목재 생선 갈대 돌
  /// </summary>
  /// <param name="id"></param>
  /// <param name="isicon"></param>
  /// <returns></returns>
  public Sprite GetResourceSprite(int id,bool isicon)
  {
    switch (id)
    {
      case 0: return isicon ? Resource_Berry_Icon : Resource_Berry_Tile;
      case 1: return isicon? Resource_Wood_Icon : Resource_Wood_Tile;
      case 2: return isicon? Resource_Fish_Icon: Resource_Fish_Tile;
      case 3: return isicon ? Resource_Swamp_Icon : Resource_Swamp_Tile;
          case 4: return isicon ? Resource_Stone_Icon : Resource_Stone_Tile;
      case 5: return Resource_Chest;
    }
    return null;
  }
  public Sprite GetResourceSprite(TileData _tile, bool isicon)
  {
    switch (_tile.ResourceType)
    {
      case 0: return isicon ? Resource_Berry_Icon : Resource_Berry_Tile;
      case 1: return isicon ? Resource_Wood_Icon : Resource_Wood_Tile;
      case 2: return isicon ? Resource_Fish_Icon : Resource_Fish_Tile;
      case 3: return isicon ? Resource_Swamp_Icon : Resource_Swamp_Tile;
      case 4: return isicon ? Resource_Stone_Icon : Resource_Stone_Tile;
      case 5: return Resource_Chest;
    }
    return null;
  }
  public Sprite CampingTile = null;
  /*
             public Sprite GetSkillIllust(SkillTypeEnum _type)
             {
               switch (_type)
               {
                 case SkillTypeEnum.Conversation: return SkillIllust_Conversation;
                 case SkillTypeEnum.Force: return SkillIllust_Force;
                 case SkillTypeEnum.Wild: return SkillIllust_Wild;
                 default: return SkillIllust_Intelligence;
               }
             }
             */

  /*  public Sprite GetVillageSprite(string _name)
  {
    Sprite _targetsprite = DefaultIllust;
    foreach (Sprite _spr in VillageIllust)
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
  }//성채 이름에 해당하는 일러스트 가져오기 */

  public List<EventIllustHolder> GetEventIllusts(string originid,string typeid,int length)
    {
    List<Sprite> _illusts = new List<Sprite>();
    foreach (Sprite _spr in EventIllust)
    {
      if (_spr.name.Split('_')[1].Equals(originid.Split('_')[1], System.StringComparison.InvariantCultureIgnoreCase) == true)
      {
        if(_spr.name.Contains(typeid, System.StringComparison.InvariantCultureIgnoreCase)==true)
        _illusts.Add(_spr);
      }
    }
    List<EventIllustHolder> _holders= new List<EventIllustHolder>();
    if (length > 1)
    {
      string[] _typearray = null;
      for (int i = 0; i < length; i++)
      {
        List<Sprite> _listtemp = new List<Sprite>();
        foreach (Sprite _spr in _illusts)
        {
          _typearray = _spr.name.Split('_')[2].Split('@');  //3번째 문자열(Beginning0,RSuccess2,LSuccess1 이런거) 에서 타입 글자확인+숫자확인
          foreach(string _type in _typearray)
          {
            if (_type.Contains(typeid) && _type.Contains(i.ToString())) 
            { 
              _listtemp.Add(_spr);
              break;
            }
            else continue;
          }

        }
        _holders.Add(new EventIllustHolder(_listtemp));
      }
    }
    else
    {
      _holders.Add(new EventIllustHolder(_illusts));
    }

    return _holders;
  }//이벤트 시작 일러스트
  public Sprite GetEXPIllust(string _illustid)
  {
    Sprite _targetsprite = DefaultIllust;
    foreach (Sprite _spr in EXPIllust)
    {
      if (string.Compare(_spr.name,_illustid,true)==0)
      {
        _targetsprite = _spr;
        break;
      }
    }
    return _targetsprite;
  }//ID로 경험 일러스트 가져오기
  [Space(10)]
  public Sprite IconBackground_normal = null;
  public Sprite IconBackground_status = null;
  [Space(10)]
  public Sprite QuestIcon_Cult = null;
  public Sprite QuestIcon_Hideout_Idle = null;
  public Sprite QuestIcon_Hideout_Finish = null;
  public Sprite QuestIcon_Wolf = null;
  public Sprite QuestIcon_Ritual_Idle = null;
  public Sprite QuestIcon_Ritual_Finish = null;
  public Sprite Quest_Cult_MainIllust = null;
  public List<Sprite> Cult_Prologue = new List<Sprite>();          //컬트-프롤로그
  public Sprite Cult_Settlement_village = null;
  public Sprite Cult_Settlement_town = null;
  public Sprite Cult_Settlement_city = null;
  public Sprite GetCultSettlementIllust(SettlementType type)
  {
    switch (type)
    {
      case SettlementType.Village:return Cult_Settlement_village;
        case SettlementType.Town:return Cult_Settlement_town;
        case SettlementType.City:return Cult_Settlement_city;
    }
    return null;
  }
  public List<Sprite> Cult_Sabbat = new List<Sprite>();
  public List<Sprite> Cult_Ritual = new List<Sprite>();
}
public class EventIllustHolder
{
  private Sprite IdleIllust = null;
  private Sprite SpringIllust = null;
  private Sprite SummerIllust = null;
  private Sprite AutumnIllust = null;
  private Sprite WinterIllust = null;
  public EventIllustHolder(List<Sprite> illusts)
  {
    if (illusts.Count == 0)
    {
      IdleIllust = GameManager.Instance.ImageHolder.DefaultIllust;
    }
    else
    {
      string[] _namearray = null;
      for (int i = 0; i < illusts.Count; i++)
      {
        _namearray = illusts[i].name.Split('_');
        if (_namearray.Length < 4)
        {
          IdleIllust = illusts[i];
        }
        else
        {
          if (_namearray[3].Contains("s0")) { SpringIllust = illusts[i]; }
         else if (_namearray[3].Contains("s1")) { SummerIllust = illusts[i]; }
         else if (_namearray[3].Contains("s2")) { AutumnIllust = illusts[i];  }
         else if (_namearray[3].Contains("s3")) { WinterIllust = illusts[i];  }
        }
      }
    }
  }
  public Sprite CurrentIllust
  {
    get
    {
      Sprite _target = null;
      switch (GameManager.Instance.MyGameData.Turn)
      {
        case 0:
          if (SpringIllust != null) _target= SpringIllust;
          break;
        case 1:
          if (SummerIllust != null) _target = SummerIllust;
          break;
        case 2:
          if (AutumnIllust != null) _target = AutumnIllust;
          break;
        case 3:
          if (WinterIllust != null) _target = WinterIllust;
          break;
      }
      if (_target == null) _target = IdleIllust;
      return _target;
    }
  }
}
[System.Serializable]
public class EndingDatas
{
  public string ID = "";
  public string SelectName { get
    {
      return GameManager.Instance.GetTextData(ID + "_Name");
    } }
  public Sprite PreviewIcon = null;
  public string Preview_Name { get { return GameManager.Instance.GetTextData(ID + "_preview_name"); } }
  public string Preview_Opened { get { return GameManager.Instance.GetTextData(ID + "_preview_opened"); } }
 // public string Preview_Closed { get { return GameManager.Instance.GetTextData(ID + "_preview_closed"); } }

  public List<Sprite> Illusts;
  public string Refuse_Name
  {
    get { return GameManager.Instance.GetTextData(ID + "_Refuse_Name"); }
  }
  public string Refuse_Description
  {
    get { return GameManager.Instance.GetTextData(ID + "_Refuse_Description"); }
  }
  public Sprite RefuseIllust = null;
  public List<string> Descriptions
  {
    get
    {
      return GameManager.Instance.GetTextData(ID+"_Descriptions").Split('@').ToList();
    }
  }
  public string LastWord
  {
    get { return GameManager.Instance.GetTextData(ID + "_LastWord"); }
  }
}
