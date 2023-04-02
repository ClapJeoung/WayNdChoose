using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class EventHolder
{
    public List<EventData> AvailableNormalEvents=new List<EventData>();
  public void AddData_Normal(EventJsonData _data)
  {
    EventData Data = new EventData();
    Data.ID = _data.ID;
        Data.IllustID = _data.IllustID;
    Data.Name = _data.Name;
    Data.Description = _data.Description;
    Data.PreDescription = _data.PreDescription;
    Data.Selection_type = (SelectionType)_data.Selection_Type;

    Data.Selection_description = _data.Selection_Description.Split('@');

    string[] _temp = _data.Selection_Target.Split('@');
    Data.Selection_target = new CheckTarget[_temp.Length];
    for(int i = 0; i < _temp.Length; i++)Data.Selection_target[i]=(CheckTarget)int.Parse(_temp[i]);

        if (_data.Selection_Info != null)
        {
            _temp = _data.Selection_Info.Split('@');
            Data.Selection_info = new int[_temp.Length];
            for (int i = 0; i < _temp.Length; i++) Data.Selection_info[i] = int.Parse(_temp[i]);
        }

    if (_data.Failure_Description != null)
    {
      Data.Failure_description = _data.Failure_Description.Split('@');

      _temp = _data.Failure_Penalty.Split('@');
      Data.Failure_penalty = new PenaltyTarget[_temp.Length];
      for (int i = 0; i < _temp.Length; i++) Data.Failure_penalty[i] = (PenaltyTarget)int.Parse(_temp[i]);

      if(_data.Failure_Penalty_info!=null)
      Data.Failure_penalty_info = _data.Failure_Penalty_info.Split('@');
    }

    Data.Success_description = _data.Success_Description.Split('@');

    _temp = _data.Reward_Target.Split('@');
    Data.Reward_Target = new RewardTarget[_temp.Length];
    for (int i = 0; i < _temp.Length; i++) Data.Reward_Target[i] = (RewardTarget)int.Parse(_temp[i]);

    if(_data.Reward_Info!=null)
    Data.Reward_info= _data.Reward_Info.Split('@');

    _temp = _data.SubReward.Split('@');
    Data.SubReward_target =new int[_temp.Length];
    for(int i=0;i<_temp.Length;i++) Data.SubReward_target[i] = int.Parse(_temp[i]);

        AvailableNormalEvents.Add(Data);
  }
  public void RemoveEvent(string _ID)
  {
    foreach(var _data in AvailableNormalEvents)
        {
            if (_data.ID.Equals(_ID))
            {
                AvailableNormalEvents.Remove(_data);
                break;
            }
        }
  }
  public EventData ReturnEvent(EventBasicData _data)
  {
    List<EventData> _events = new List<EventData>();

    //레후~

    return _events[Random.Range(0, _events.Count)];
  }
}
public class EventJsonData
{
    public string ID = "";              //ID
    public string IllustID = "";        //일러스트 ID
  public string Name = "";              //이름
  public string PreDescription = "";    //미리보기 텍스트
    public int Settlement = 0;          //0,1,2,3
    public int Place = 0;               //0,1,2,3,4
    public int Place_Level = 0;          //0(전부) 1(낮) 2(중) 3(높)
    public int Season = 0;              //전역,봄,여름,가을,겨울
    public string Description = "";       //설명 텍스트
    public int Environment_Type = 0;         //전역,숲,강,언덕,산,바다

    public int Selection_Type;           //0.단일 1.이성+육체 2.정신+물질 3.성향 4.경험 5.기술
    public string Selection_Description = ""; //선택지 별 텍스트
  public string Selection_Target;           //0.무조건 1.지불 2.테마 3.기술
  public string Selection_Info;             //0:정보 없음  1:체력,정신력,돈
                                            //2:대화,무력,생존,정신
                                            //3: 0.설득 1.협박  2.기만  3.논리 4.격투 5.활술 6.인체 7.생존 8.생물 9.잡학

  public string Failure_Description = "";   //선택지 별 실패 텍스트
  public string Failure_Penalty;            //없음,손실,경험
  public string Failure_Penalty_info;       //(체력,정신력,돈),경험 ID

  public string Success_Description = "";   //선택지 별 성공 텍스트
  public string Reward_Target;              //경험,체력,정신력,돈,기술-테마,기술-개별,특성
  public string Reward_Info;                //경험 :ID  체력,정신력,돈:X  테마:대화,무력,생존,학식  개별기술:위 참조  특성:ID

  public string SubReward;                  //없음,돈,정신력,돈+정신력
}
public class FollowEventJsonData
{
    public string ID = "";              //ID
    public int FollowType = 0;              //이벤트,경험,특성,테마,기술
    public int FollowTarget = 0;            //해당 ID 혹은 0,1,2,3 혹은 0~9
    public int FollowResult = 0;            //이벤트일 경우 성공,실패
    public int FollowTendency = 0;          //이벤트일 경우 기타,이성,육체,정신,물질 선택지 여부
    public string Name = "";              //이름
    public string PreDescription = "";    //미리보기 텍스트
    public string Description = "";       //설명 텍스트
    public int Season = 0;              //전역,봄,여름,가을,겨울
    public int Settlement = 0;          //0,1,2,3
    public int Place = 0;               //0,1,2,3,4
    public int Place_Level = 0;          //0(전부) 1(낮) 2(중) 3(높)
    public int Environment_Type = 0;         //전역,숲,강,언덕,산,바다

    public int Selection_Type;           //0.단일 1.이성+육체 2.정신+물질 3.성향 4.경험 5.기술
    public string Selection_Description = ""; //선택지 별 텍스트
    public string Selection_Target;           //0.무조건 1.지불 2.테마 3.기술
    public string Selection_Info;             //0:정보 없음  1:체력,정신력,돈
                                              //2:대화,무력,생존,정신
                                              //3: 0.설득 1.협박  2.기만  3.논리 4.격투 5.활술 6.인체 7.생존 8.생물 9.잡학

    public string Failure_Description = "";   //선택지 별 실패 텍스트
    public string Failure_Penalty;            //없음,손실,경험
    public string Failure_Penalty_info;       //(체력,정신력,돈),경험 ID

    public string Success_Description = "";   //선택지 별 성공 텍스트
    public string Reward_Target;              //경험,체력,정신력,돈,기술-테마,기술-개별,특성
    public string Reward_Info;                //경험 :ID  체력,정신력,돈:X  테마:대화,무력,생존,학식  개별기술:위 참조  특성:ID

    public string SubReward;                  //없음,돈,정신력,돈+정신력
}
public class QuestEventDataJson
{
    public string QuestId = "";                 //퀘스트 ID
    public string ID = "";
    public int QuestIndex = 0;                   //자기 퀘스트에서 순서
    public string Name = "";              //이름
    public string PreDescription = "";    //미리보기 텍스트
    public string Description_first = "";       //설명 텍스트(최초)
    public string Description_after = "";       //설명 텍스트
    public int Settlement = 0;          //0(아무 정착지),1,2,3,4(외부)
    public int Place = 0;               //0,1,2,3,4

    public int Selection_Type;           //0.단일 1.이성+육체 2.정신+물질 3.성향 4.경험 5.기술
    public string Selection_Description = ""; //선택지 별 텍스트
    public string Selection_Target;           //0.무조건 1.지불 2.테마 3.기술
    public string Selection_Info;             //0:정보 없음  1:체력,정신력,돈
                                              //2:대화,무력,생존,정신
                                              //3: 0.설득 1.협박  2.기만  3.논리 4.격투 5.활술 6.인체 7.생존 8.생물 9.잡학

    public string Failure_Description = "";   //선택지 별 실패 텍스트
    public string Failure_Penalty;            //없음,손실,경험
    public string Failure_Penalty_info;       //(체력,정신력,돈),경험 ID

    public string Success_Description = "";   //선택지 별 성공 텍스트
    public string Reward_Target;              //경험,체력,정신력,돈,기술-테마,기술-개별,특성
    public string Reward_Info;                //경험 :ID  체력,정신력,돈:X  테마:대화,무력,생존,학식  개별기술:위 참조  특성:ID

    public string SubReward;                  //없음,돈,정신력,돈+정신력
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
  public EnvironmentType EnvironmentType;
  public int Season;
}
public enum SettlementType { Town,City,Castle,Outer}
public enum PlaceType { Residence,Marketplace,Temple,Library,Theater,Campus}
public enum EnvironmentType { None,River,Forest,Mine,Mountain,Sea }
public enum SelectionType { Single,Verticla, Horizontal,Tendency,Experience,Skill }//Horizontal : 좌 물질 우 정신     Vertical : 위 이성 아래 육체
public enum CheckTarget { None,Pay,Theme,Skill}
public enum PenaltyTarget { None,Status,EXP }
public enum RewardTarget { Experience,GoldAndExperience,Gold,HP,Sanity,Theme,Skill,Trait}
public enum EventSequence { Sugguest,Progress,Clear}//Suggest: 3개 제시하는 단계  Progress: 선택지 버튼 눌러야 하는 단계  Clear: 보상 수령해야 하는 단계
public class EventData  //기본적인 무작위 풀에서 나오는 이벤트
{
    public string ID = "";
    public string IllustID = "";
    public string Name = "";
    public string Description = "";
  public string PreDescription = "";

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
