using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 시작 성공(일반) 실패(일반) 성공,실패(성향 좌측) 성공,실패(성향 우측)
/// </summary>
public enum EventPhaseTypeEnum { Beginning,Selecting,Success,Fail,LSuccess,LFail,RSuccess,RFail}
[CreateAssetMenu(menuName ="ImageHolder")]
public class ImageHolder : ScriptableObject
{
  public List<Sprite> GameoverIllusts = new List<Sprite>();
  public Sprite GetGameoverIllust(string name)
  {
    foreach(var illust in GameoverIllusts)
    {
      if (string.Compare(illust.name, name, true) == 0) return illust;
    }
    Debug.Log(name + " 이란 이름의 게임 오버 일러스트가 없는 레후");
    return DefaultIllust;
  }

  public Sprite MovePointIcon_Enable = null;
  public Sprite MovePointIcon_Lack = null;
  public Sprite VillageIcon_black = null;
  public Sprite VillageIcon_white = null;
  public List<Sprite> VillageIllust=new List<Sprite>();                 //마을 일러스트
  public Sprite TownIcon_black = null;
  public Sprite TownIcon_white = null;
  public List<Sprite> TownIllust=new List<Sprite>();                 //도시 일러스트
  public Sprite CityIcon_black = null;
  public Sprite CityIcon_white = null;
  public List<Sprite> CityIllust=new List<Sprite>();               //성채 일러스트
  [Space(10)]
  public Sprite SkillIllust_Conversation = null;
  public Sprite SkillIllust_Force = null, SkillIllust_Wild = null, SkillIllust_Intelligence = null;
  [Space(10)]
  public Sprite[] TendencyIcon_Body = new Sprite[5];
  public Sprite[] TendencyIllust_Body=new Sprite[5];
  public Sprite[] TendencyIcon_Head=new Sprite[5];
  public Sprite[] TendencyIllust_Head = new Sprite[5];
  public Sprite StatusIcon(StatusType status)
  {
    if (status == StatusType.HP) return HPIcon;
    else if (status == StatusType.Sanity) return SanityIcon;
    else return GoldIcon;
  }
  public Sprite GetTendencyIcon(TendencyTypeEnum type,int level)
  {
    if(type.Equals(TendencyTypeEnum.Body))return TendencyIcon_Body[level+2];
    else return TendencyIcon_Head[level+2];
  }
  public Sprite GettendencyIllust(TendencyTypeEnum type,int level)
  {
    if (type.Equals(TendencyTypeEnum.Body)) return TendencyIllust_Body[level + 2];
    else return TendencyIllust_Head[level + 2];
  }
  [Space(10)]
  public Sprite ThemeIcon_Conversation = null;
  public Sprite ThemeIcon_Force = null;
  public Sprite ThemeIcon_Wild = null;
  public Sprite ThemeIcon_Intelligence = null;
  public Sprite GetSkillIcon(SkillType _type)
  {
    switch (_type)
    {
      case SkillType.Conversation: return ThemeIcon_Conversation;
      case SkillType.Force: return ThemeIcon_Force;
      case SkillType.Wild: return ThemeIcon_Wild;
      default: return ThemeIcon_Intelligence;
    }
  }
  [Space(10)]
 // public Sprite TendencySelectionIcon = null;
 // public Sprite ExpSelectionIcon = null;
 // public Sprite SkillSelectionIcon = null;
  public Sprite ResidenceIcon = null;
  public Sprite MarketPlaceIcon = null;
  public Sprite TempleIcon = null;
  public Sprite LibraryIcon = null;
//  public Sprite TheaterIcon = null;
//  public Sprite AcademyIcon = null;
  public Sprite GetPlaceIcon(SectorType placetype)
  {
    switch (placetype)
    {
      case SectorType.Residence:return ResidenceIcon;
      case SectorType.Temple: return TempleIcon;
      case SectorType.Marketplace:return MarketPlaceIcon;
      case SectorType.Library:return LibraryIcon;
  //    case SectorType.Theater:return TheaterIcon;
    //  default:return AcademyIcon;
    }
    return null;
  }
  [Space(10)]

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
  public List<Sprite> EndingIllusts=new List<Sprite>();//엔딩 일러스트
  public Sprite GetEndingIllust(string index)
  {
    foreach (var _temp in EndingIllusts)
      if (_temp.name.Equals(index)) return _temp;

    return DefaultIllust;
  }
  [Space(10)]
  public Sprite QuestIcon_Cult = null;
  public Sprite QuestIcon_Hideout_Idle = null;
  public Sprite QuestIcon_Hideout_Finish = null;
  public Sprite QuestIcon_Wolf = null;
  public Sprite QuestIcon_Ritual_Idle = null;
  public Sprite QuestIcon_Ritual_Finish = null;
    public List<Sprite> QuestIllust_Wolf = new List<Sprite>();          //늑대 관련 퀘스트 일러스트
    public Sprite GetQuestIllust(QuestType type,string id)
    {
        List<Sprite> _targetlist = new List<Sprite>();
        switch (type)
        {
            case QuestType.Cult:_targetlist = QuestIllust_Wolf;break;
        }
        foreach(var _illust in _targetlist)
        {
            if (string.Compare(id, _illust.name, true).Equals(0)) return _illust;
        }
        Debug.Log($"{id} 이미지 없음");
        return DefaultIllust;
    }
    public Sprite EmptyLongExpIcon = null;
    public Sprite EmptyShortExpIcon = null;
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
        if (dir.Equals(false)) return Arrow_Active_rational;
        else return Arrow_Active_physical;
      case TendencyTypeEnum.Head:
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
  public Sprite SelectionBackground(TendencyTypeEnum tendencytype, bool dir)
  {
    switch (tendencytype)
    {
      case TendencyTypeEnum.Body:
        if (dir.Equals(false)) return SelectionBackground_rational;
        else return SelectionBackground_physical;
      case TendencyTypeEnum.Head:
        if (dir.Equals(false)) return SelectionBackground_mental;
        else return SelectionBackground_material;
      default: return SelectionBackground_none;
    }
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
  public Sprite GetSkillIllust(SkillType _type)
  {
    switch (_type)
    {
      case SkillType.Conversation: return SkillIllust_Conversation;
      case SkillType.Force: return SkillIllust_Force;
      case SkillType.Wild: return SkillIllust_Wild;
      default: return SkillIllust_Intelligence;
    }
  }

    public Sprite GetVillageSprite(string _name)
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
  }//성채 이름에 해당하는 일러스트 가져오기

    public List<EventIllustHolder> GetEventIllusts(string eventid,int length)
    {
    string _name = eventid;
    _name += "_Illust";
    List<Sprite> _illusts = new List<Sprite>();
    foreach (Sprite _spr in EventIllust)
    {
      if (_spr.name.Contains(_name, System.StringComparison.InvariantCultureIgnoreCase) == true)
      {
        _illusts.Add(_spr);
      }
    }
    List<EventIllustHolder> _holders= new List<EventIllustHolder>();
    for(int i = 0; i < length; i++)
    {
      List<Sprite> _listtemp=new List<Sprite>();
      foreach(Sprite _spr in _illusts) if(_spr.name.Contains(i.ToString()))_listtemp.Add(_spr);

      _holders.Add(new EventIllustHolder(_listtemp));
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
    for(int i=0;i<illusts.Count;i++)
    {
      if (illusts[i].name.Contains("spring")) SpringIllust = illusts[i];
      else if (illusts[i].name.Contains("summer")) SummerIllust = illusts[i];
      else if (illusts[i].name.Contains("winter")) AutumnIllust = illusts[i];
      else if (illusts[i].name.Contains("autumn")) WinterIllust = illusts[i];
      else IdleIllust = illusts[i];
    }
  }
  public Sprite CurrentIllust
  {
    get
    {
      switch (GameManager.Instance.MyGameData.Turn)
      {
        case 0:
          if (SpringIllust != null) return SpringIllust;
          break;
        case 1:
          if (SummerIllust != null) return SummerIllust;
          break;
        case 2:
          if (AutumnIllust != null) return AutumnIllust;
          break;
        case 4:
          if (WinterIllust != null) return WinterIllust;
          break;
      }
      return IdleIllust;
    }
  }
}
