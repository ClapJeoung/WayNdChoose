using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHolder
{
  public EventHolder_settle Town =new EventHolder_settle();
  public EventHolder_settle City =new EventHolder_settle();
  public EventHolder_settle Castle =new EventHolder_settle();
  //정착지->장소->장소 등급->난이도->계절->환경
  public EventHolder_outer Outer =new EventHolder_outer();
  //외부  ->              ->난이도->계절->환경
  public Dictionary<string,EventData> AllEvents=new Dictionary<string,EventData>();
  public void AddData(string _index, EventJsonData _data)
  {
    EventData Data = new EventData();
    Data.Index = _index;
    Data.Name = _data.Name;
    Data.Description = _data.Description;
    Data.Selection_type = (SelectionType)_data.Selection_Type;
    Data.Selection_description = _data.Selection_Description.Split('@');
    string[] _temp = _data.Selection_Target.Split('@');
    Data.Selection_target = new CheckTarget[_temp.Length];
    for(int i = 0; i < _temp.Length; i++)Data.Selection_target[i]=(CheckTarget)int.Parse(_temp[i]);
    _temp = _data.Selection_Info.Split('@');
    Data.Selection_info=new int[_temp.Length];
    for (int i = 0; i < _temp.Length; i++) Data.Selection_info[i] = int.Parse(_temp[i]);

    Data.Failure_description=_data.Failure_Description.Split('@');
    _temp = _data.Failure_Penalty.Split('@');
    Data.Failure_penalty=new PenaltyTarget[_temp.Length];
    for(int i = 0; i < _temp.Length; i++) Data.Failure_penalty[i]=(PenaltyTarget)int.Parse(_temp[i]);
    _temp = _data.Failure_Penalty_info.Split('@');
    for (int i = 0; i < _temp.Length; i++) Data.Failure_penalty_info[i] = _temp[i];

    Data.Success_description = _data.Success_Description.Split('@');

    _temp = _data.Reward_Target.Split('@');
    Data.Reward_Target = new RewardTarget[_temp.Length];
    for (int i = 0; i < _temp.Length; i++) Data.Reward_Target[i] = (RewardTarget)int.Parse(_temp[i]);
    Data.Reward_info= _data.Reward_Info.Split('@');

    _temp = _data.SubReward.Split('@');
    Data.SubReward_target =new int[_temp.Length];
    for(int i=0;i<_temp.Length;i++) Data.SubReward_target[i] = int.Parse(_temp[i]);

    if (_data.Settlement == 0) Town.AddData(Data, _data);
    else if(_data.Settlement == 1) City.AddData(Data, _data);
    else if (_data.Settlement == 2) Castle.AddData(Data, _data);
    else Outer.AddData(Data, _data);

    AllEvents.Add(_index, Data);
  }
  public void RemoveEvent(string _index)
  {
    AllEvents.Remove(_index);
  }
  public EventData ReturnEvent(EventBasicData _data)
  {
    List<EventData> _events = new List<EventData>();

    if(_data.SettlementType==SettlementType.Town)_events=Town.ReturnEvent(_data);
    else if(_data.SettlementType==SettlementType.City)_events=City.ReturnEvent(_data);
    else if(_data.SettlementType ==SettlementType.Castle) _events=Castle.ReturnEvent(_data);
    else _events=Outer.ReturnEvent(_data);

    return _events[Random.Range(0, _events.Count)];
  }
}
public class EventJsonData
{
  public const string SplitChar = "@";
  public string Name = "";
  public string Description = "";
  public int Season = 0;
  public int Settlement = 0;          //0,1,2,3
  public int Place = 0;               //0,1,2,3,4
  public int Place_Level = 0;          //0(전부) 1(낮) 2(중) 3(높)
  public int Environment_Type = 0;         //0,1,2,3,4,5

  public int Selection_Type;           //0,1,2,3,4,5
  public string Selection_Description = "";
  public string Selection_Target;
  public string Selection_Info;
  public int Selection_Difficult;

  public string Failure_Description = "";
  public string Failure_Penalty;
  public string Failure_Penalty_info;

  public string Success_Description = "";
  public string Reward_Target;
  public string Reward_Info;
  public int Reward_Difficult;

  public string SubReward;
}
public class EventHolder_settle
{
  public EventHolder_place Residence=new EventHolder_place();
  public EventHolder_place Marketplace=new EventHolder_place();
  public EventHolder_place Temple=new EventHolder_place();
  public EventHolder_place Library=new EventHolder_place();
  public EventHolder_place Theater=new EventHolder_place();
  public EventHolder_place Campus=new EventHolder_place();
  public void AddData(EventData _data,EventJsonData _json)
  {
    if (_json.Settlement == 0) Residence.AddData(_data, _json);
    else if (_json.Settlement == 1) Marketplace.AddData(_data, _json);
    else if (_json.Settlement == 2) Temple.AddData(_data, _json);
    else if (_json.Settlement == 3)
    {
      if (_json.Settlement == 1) Library.AddData(_data, _json);
      else Theater.AddData(_data, _json);
    }
    else Campus.AddData(_data, _json);
  }
  public void RemoveEvent(EventData _data)
  {
    Residence.RemoveEvent(_data);
    Marketplace.RemoveEvent(_data);
    Temple.RemoveEvent(_data);
    Library.RemoveEvent(_data);
    Theater.RemoveEvent(_data);
    Campus.RemoveEvent(_data);
  }
  public List<EventData> ReturnEvent(EventBasicData _data)
  {
    List<EventData> _lehoo = null;
    if (_data.PlaceType == PlaceType.Residence) _lehoo = Residence.ReturnEvent(_data);
    else if (_data.PlaceType == PlaceType.Marketplace) _lehoo = Marketplace.ReturnEvent(_data);
    else if (_data.PlaceType == PlaceType.Temple) _lehoo = Temple.ReturnEvent(_data);
    else if (_data.PlaceType == PlaceType.Library) _lehoo = Library.ReturnEvent(_data);
    else if (_data.PlaceType == PlaceType.Theater) _lehoo = Theater.ReturnEvent(_data);
    else _lehoo = Campus.ReturnEvent(_data);
    return _lehoo;
  }
}
public class EventHolder_outer
{
  public EventHolder_season Whenever = new EventHolder_season();
  public EventHolder_season Spring = new EventHolder_season();
  public EventHolder_season Summer = new EventHolder_season();
  public EventHolder_season Fall = new EventHolder_season();
  public EventHolder_season Winter = new EventHolder_season();
  public void AddData(EventData _data, EventJsonData _json)
  {
    if (_json.Season == 0) Whenever.AddData(_data, _json);
    else if (_json.Season == 1) Spring.AddData(_data, _json);
    else if (_json.Season == 2) Summer.AddData(_data, _json);
    else if (_json.Season == 3) Fall.AddData(_data, _json);
    else Winter.AddData(_data, _json);
  }
  public void RemoveEvent(EventData _data)
  {
    Whenever.RemoveEvent(_data);
    Spring.RemoveEvent(_data);
    Summer.RemoveEvent(_data);
    Fall.RemoveEvent(_data);
    Winter.RemoveEvent(_data);
  }
  public List<EventData> ReturnEvent(EventBasicData _data)
  {
    List<EventData> _lehoo = null;
    if (_data.Season == 0) _lehoo = Whenever.ReturnEvent(_data);
    else if (_data.Season == 1) _lehoo = Spring.ReturnEvent(_data);
    else if (_data.Season == 2) _lehoo = Summer.ReturnEvent(_data);
    else if (_data.Season == 3) _lehoo = Fall.ReturnEvent(_data);
    else _lehoo = Winter.ReturnEvent(_data);
    return _lehoo;
  }
}

public class EventHolder_place
{
  public EventHolder_placelevel Whatever=new EventHolder_placelevel();
  public EventHolder_placelevel Low=new EventHolder_placelevel();
  public EventHolder_placelevel Middle=new EventHolder_placelevel();
  public EventHolder_placelevel High=new EventHolder_placelevel();
  public void AddData(EventData _data, EventJsonData _json)
  {
    if (_json.Place_Level == 0) Whatever.AddData(_data, _json);
    else if(_json.Place_Level==1)Low.AddData(_data, _json);
    else if(_json.Place_Level==2)Middle.AddData(_data, _json);
    else High.AddData(_data, _json);
  }
  public void RemoveEvent(EventData _data)
  {
    Whatever.RemoveEvent(_data);
    Low.RemoveEvent(_data);
    Middle.RemoveEvent(_data);
    High.RemoveEvent(_data);
  }
  public List<EventData> ReturnEvent(EventBasicData _data)
  {
    List<EventData> _lehoo = null;
    if (_data.PlaceLevel == 0) _lehoo = Whatever.ReturnEvent(_data);
    else if (_data.PlaceLevel == 1) _lehoo = Low.ReturnEvent(_data);
    else if (_data.PlaceLevel == 2) Middle.ReturnEvent(_data);
    else _lehoo = High.ReturnEvent(_data);
    return _lehoo;
  }
}
public class EventHolder_placelevel
{
  public EventHolder_difficulty Chang=new EventHolder_difficulty();
  public EventHolder_difficulty Middle=new EventHolder_difficulty();
  public EventHolder_difficulty Hye=new EventHolder_difficulty();
  public void AddData(EventData _data, EventJsonData _json)
  {
    int _diff = _json.Reward_Difficult - _json.Selection_Difficult;
    if (_diff < 0) Hye.AddData(_data, _json);
    else if (_diff == 0) Middle.AddData(_data, _json);
    else Chang.AddData(_data, _json);
  }
  public void RemoveEvent(EventData _data)
  {
    Chang.RemoveEvent(_data);
    Middle.RemoveEvent(_data);
    Hye.RemoveEvent(_data);
  }
  public List<EventData> ReturnEvent(EventBasicData _data)
  {
    List<EventData> _lehoo = null;
    if(_data.EventLevel==EventLevel.Chang)_lehoo=Chang.ReturnEvent(_data);
    else if(_data.EventLevel==EventLevel.Middle)_lehoo=Middle.ReturnEvent(_data);
    else _lehoo=Hye.ReturnEvent(_data);
    return _lehoo;
  }
}
public class EventHolder_difficulty
{
  public EventHolder_season Whenever=new EventHolder_season();
  public EventHolder_season Spring=new EventHolder_season();
  public EventHolder_season Summer=new EventHolder_season();
  public EventHolder_season Fall=new EventHolder_season();
  public EventHolder_season Winter=new EventHolder_season();
  public void AddData(EventData _data, EventJsonData _json)
  {
    if(_json.Season==0)Whenever.AddData(_data, _json);
    else if(_json.Season==1)Spring.AddData(_data, _json);
    else if(_json.Season==2)Summer.AddData(_data, _json);
    else if(_json.Season==3)Fall.AddData(_data, _json);
    else Winter.AddData(_data, _json);
  }
  public void RemoveEvent(EventData _data)
  {
    Whenever.RemoveEvent(_data);
    Spring.RemoveEvent(_data);
    Summer.RemoveEvent(_data);
    Fall.RemoveEvent(_data);
    Winter.RemoveEvent(_data);
  }
  public List<EventData> ReturnEvent(EventBasicData _data)
  {
    List<EventData> _lehoo = null;
    if(_data.Season==0)_lehoo=Whenever.ReturnEvent(_data);
    else if(_data.Season==1)_lehoo=Spring.ReturnEvent(_data);
    else if (_data.Season == 2) _lehoo = Summer.ReturnEvent(_data);
    else if (_data.Season == 3) _lehoo = Fall.ReturnEvent(_data);
    else _lehoo= Winter.ReturnEvent(_data);
    return _lehoo;
  }
}
public class EventHolder_season
{
  public EventHolder_Envir Whererever = new EventHolder_Envir();
  public EventHolder_Envir Forest = new EventHolder_Envir();
  public EventHolder_Envir River = new EventHolder_Envir();
  public EventHolder_Envir Highland = new EventHolder_Envir();
  public EventHolder_Envir Mountain = new EventHolder_Envir();
  public EventHolder_Envir Sea = new EventHolder_Envir();
  public void AddData(EventData _data, EventJsonData _json)
  {
    if (_json.Environment_Type == 0) Whererever.AddData( _data);
    else if(_json.Environment_Type==1)Forest.AddData(_data);
    else if(_json.Environment_Type==2)River.AddData(_data);
    else if(_json.Environment_Type==3)Highland.AddData(_data);
    else if(_json.Environment_Type==4)Mountain.AddData(_data);
    else Sea.AddData(_data);
  }
  public void RemoveEvent(EventData _data)
  {
    Whererever.RemoveEvent(_data);
    Forest.RemoveEvent(_data);
    River.RemoveEvent(_data);
    Highland.RemoveEvent(_data);
    Mountain.RemoveEvent(_data);
    Sea.RemoveEvent(_data);
  }
  public List<EventData> ReturnEvent(EventBasicData _data)
  {
    List<EventData> _lehoo = null;
    if (_data.EnvironmentType == EnvironmentType.None) _lehoo = Whererever.ReturnEvent();
    else if( _data.EnvironmentType==EnvironmentType.Forest) _lehoo = Forest.ReturnEvent();
    else if (_data.EnvironmentType == EnvironmentType.River) _lehoo = River.ReturnEvent();
    else if (_data.EnvironmentType == EnvironmentType.Mine) _lehoo = Highland.ReturnEvent();
    else if (_data.EnvironmentType == EnvironmentType.Mountain) _lehoo = Mountain.ReturnEvent();
    else _lehoo=Sea.ReturnEvent();
    return _lehoo;
  }
}
public class EventHolder_Envir
{

  public List<EventData> Data = new List<EventData>();
  public void AddData(EventData _data)
  {
    if (!Data.Contains(_data)) Data.Add(_data);
  }
  public void RemoveEvent(EventData _data)
  {
    if(Data.Contains(_data))
    Data.Remove(_data);
  }
  public List<EventData> ReturnEvent()
  {
    return Data;
  }
}
public class EventBasicData
{
  public SettlementType SettlementType;
  public PlaceType PlaceType;
  public int PlaceLevel;
  public EventLevel EventLevel;
  public EnvironmentType EnvironmentType;
  public int Season;
}
public enum EventLevel { Chang, Middle, Hye }
public enum SettlementType { Town,City,Castle,Outer}
public enum PlaceType { Residence,Marketplace,Temple,Library,Theater,Campus}
public enum EnvironmentType { None,River,Forest,Mine,Mountain,Sea }
public enum SelectionType { Single,Verticla, Horizontal,Tendency,Experience,Skill }//Horizontal : 좌 물질 우 정신     Vertical : 위 이성 아래 육체
public enum CheckTarget { None,Pay,Theme,Skill}
public enum PenaltyTarget { None,Status,EXP }
public enum RewardTarget { Experience,GoldAndExperience,Gold,HP,Sanity,Theme,Skill,Trait}
public class EventData  //기본적인 무작위 풀에서 나오는 이벤트
{
    public string Index = "";
    public string Name = "";
    public string Description = "";

    public SelectionType Selection_type;
    public string[] Selection_description;
    public CheckTarget[] Selection_target;
    public int[] Selection_info;

    public string[] Failure_description;
    public PenaltyTarget[] Failure_penalty;
    public string[] Failure_penalty_info;

    public string[] Success_description;

  public RewardTarget[] Reward_Target;
  public string[] Reward_info;

  public int[] SubReward_target;
}
