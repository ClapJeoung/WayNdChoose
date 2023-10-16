using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 시작 성공(일반) 실패(일반) 성공,실패(성향 좌측) 성공,실패(성향 우측)
/// </summary>
public enum EventPhaseTypeEnum { Beginning,Selecting,Success,Fail,LSuccess,LFail,RSuccess,RFail}
[CreateAssetMenu(menuName ="ImageHolder")]
public class ImageHolder : ScriptableObject
{
  public List<Sprite> GameoverIllusts = new List<Sprite>();

  public Sprite MovePointIcon_Enable = null;
  public Sprite MovePointIcon_Lack = null;
  public Sprite VillageIcon_black = null;
  public Sprite VillageIcon_white = null;
 // public List<Sprite> VillageIllust=new List<Sprite>();                 //마을 일러스트
  public Sprite TownIcon_black = null;
  public Sprite TownIcon_white = null;
 // public List<Sprite> TownIllust=new List<Sprite>();                 //도시 일러스트
  public Sprite CityIcon_black = null;
  public Sprite CityIcon_white = null;
  //public List<Sprite> CityIllust=new List<Sprite>();               //성채 일러스트
  [Space(10)]
  public Sprite SkillIllust_Conversation = null;
  public Sprite SkillIllust_Force = null, SkillIllust_Wild = null, SkillIllust_Intelligence = null;
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
 /* public Sprite TendencySelectionIcon = null;
  public Sprite ExpSelectionIcon = null;
  public Sprite SkillSelectionIcon = null;
  public Sprite ResidenceIcon = null;
  public Sprite MarketPlaceIcon = null;
  public Sprite TempleIcon = null;
  public Sprite LibraryIcon = null;
  public Sprite TheaterIcon = null;
  public Sprite AcademyIcon = null;
  public Sprite GetPlaceIcon(SectorTypeEnum placetype)
  {
    switch (placetype)
    {
      case SectorTypeEnum.Residence:return ResidenceIcon;
      case SectorTypeEnum.Temple: return TempleIcon;
      case SectorTypeEnum.Marketplace:return MarketPlaceIcon;
      case SectorTypeEnum.Library:return LibraryIcon;
  //    case SectorType.Theater:return TheaterIcon;
    //  default:return AcademyIcon;
    }
    return null;
  }
  [Space(10)]
 */
    public List<Sprite> EventIllust = new List<Sprite>();              //모든 이벤트 일러스트
  public List<Sprite> EXPIllust = new List<Sprite>();                  //모든 경험 일러스트
  public Sprite GetRandomMainIllust(Sprite lastillust)
  {
    Sprite _spr = Random.Range(0, 2).Equals(0) ? EventIllust[Random.Range(0, EventIllust.Count)] : EXPIllust[Random.Range(0, EXPIllust.Count)];
    while (_spr == lastillust) _spr = Random.Range(0, 2).Equals(0) ? EventIllust[Random.Range(0, EventIllust.Count)] : EXPIllust[Random.Range(0, EXPIllust.Count)];
    return _spr;
  }//대문에 넣을 일러스트 무작위로 전달
  [Space(5)]
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
  public Sprite SelectionButtonImage_BodyM_Idle = null;
  public Sprite SelectionButtonImage_BodyM_Enter = null;
  public Sprite SelectionButtonImage_BodyM_Clicked = null;
  public Sprite SelectionButtonImage_BodyP_Idle = null;
  public Sprite SelectionButtonImage_BodyP_Enter = null;
  public Sprite SelectionButtonImage_BodyP_Clicked = null;
  public Sprite SelectionButtonImage_HeadM_Idle = null;
  public Sprite SelectionButtonImage_HeadM_Enter = null;
  public Sprite SelectionButtonImage_HeadM_Clicked = null;
  public Sprite SelectionButtonImage_HeadP_Idle = null;
  public Sprite SelectionButtonImage_HeadP_Enter = null;
  public Sprite SelectionButtonImage_HeadP_Clicked = null;
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
          _state.disabledSprite = SelectionButtonImage_BodyM_Idle;
        }
        else
        {
          _state.highlightedSprite = SelectionButtonImage_BodyP_Enter;
          _state.pressedSprite = SelectionButtonImage_BodyP_Clicked;
          _state.selectedSprite = SelectionButtonImage_BodyP_Idle;
          _state.disabledSprite = SelectionButtonImage_BodyP_Idle;
        }
        break;
      case TendencyTypeEnum.Head:
        if (dir.Equals(true))
        {
          _state.highlightedSprite = SelectionButtonImage_HeadM_Enter;
          _state.pressedSprite = SelectionButtonImage_HeadM_Clicked;
          _state.selectedSprite = SelectionButtonImage_HeadM_Idle;
          _state.disabledSprite = SelectionButtonImage_HeadM_Idle;
        }
        else
        {
          _state.highlightedSprite = SelectionButtonImage_HeadP_Enter;
          _state.pressedSprite = SelectionButtonImage_HeadP_Clicked;
          _state.selectedSprite = SelectionButtonImage_HeadP_Idle;
          _state.disabledSprite = SelectionButtonImage_HeadP_Idle;
        }
        break;
      default:
        {
          _state.highlightedSprite = SelectionButtonImage_None_Enter;
          _state.pressedSprite = SelectionButtonImage_None_Clicked;
          _state.selectedSprite = SelectionButtonImage_None_Idle;
          _state.disabledSprite = SelectionButtonImage_None_Idle;
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
  [Space(10)]
  public Sprite UnknownTile = null;
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
      if (_spr.name.Contains(originid, System.StringComparison.InvariantCultureIgnoreCase) == true)
      {
        if(_spr.name.Contains(typeid, System.StringComparison.InvariantCultureIgnoreCase)==true)
        _illusts.Add(_spr);
      }
    }
    List<EventIllustHolder> _holders= new List<EventIllustHolder>();
    if (length > 1)
    {
      for (int i = 0; i < length; i++)
      {
        List<Sprite> _listtemp = new List<Sprite>();
        foreach (Sprite _spr in _illusts) if (_spr.name.Contains(i.ToString())) _listtemp.Add(_spr);

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
  public List<EndingIllusts> EndingIllustList = new List<EndingIllusts>();//엔딩 일러스트
  public EndingIllusts GetEndingIllust(string id)
  {
    foreach (var _temp in EndingIllustList)
      if (_temp.ID== id) return _temp;

    return null;
  }
  [Space(10)]
  public Sprite QuestIcon_Cult = null;
  public Sprite QuestIcon_Hideout_Idle = null;
  public Sprite QuestIcon_Hideout_Finish = null;
  public Sprite QuestIcon_Wolf = null;
  public Sprite QuestIcon_Ritual_Idle = null;
  public Sprite QuestIcon_Ritual_Finish = null;
  public Sprite Quest_Cult_MainIllust = null;
  public List<Sprite> Cult_Prologue = new List<Sprite>();          //컬트-프롤로그
  public List<Sprite> Cult_ToPhase1 = new List<Sprite>();
  public List<Sprite> Cult_ToPhase2 = new List<Sprite>();
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
  public List<Sprite> CultEnding_Illusts = new List<Sprite>();
  public Sprite CultEnding_Rational = null, CultEnding_Physical = null,
    CultEnding_Mental = null, CultEnding_Material = null;
  public Sprite CultEnding_Last = null;
  public Sprite GetCultIllust(List<Sprite> targetlist, string id)
  {
    foreach (var _illust in targetlist)
    {
      if (string.Compare(id, _illust.name, true).Equals(0)) return _illust;
    }
    Debug.Log($"{id} 이미지 없음");
    return DefaultIllust;
  }

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
      bool _noneseason = true;
      for (int i = 0; i < illusts.Count; i++)
      {
        if (illusts[i].name.Contains("spring")) { SpringIllust = illusts[i]; _noneseason = false; }
        if (illusts[i].name.Contains("summer")){ SummerIllust = illusts[i]; _noneseason = false; }
        if (illusts[i].name.Contains("winter")){ AutumnIllust = illusts[i]; _noneseason = false; }
          if (illusts[i].name.Contains("autumn")){ WinterIllust = illusts[i]; _noneseason = false; }
            if(_noneseason==true) IdleIllust = illusts[i];
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
        case 4:
          if (WinterIllust != null) _target = WinterIllust;
          break;
      }
      if (_target == null) _target = IdleIllust;
      return _target;
    }
  }
}
[System.Serializable]
public class EndingIllusts
{
  public string ID = "";
  public string Name { get
    {
      return GameManager.Instance.GetTextData(ID + "_Name");
    } }
  public List<Sprite> Illusts;
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
