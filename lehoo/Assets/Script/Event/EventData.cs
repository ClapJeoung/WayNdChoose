using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using static UnityEditor.Progress;
using System;
using Unity.Mathematics;

public class EventHolder
{

  public List<EventData> AllNormalEvents = new List<EventData>();
  public List<FollowEventData> AllFollowEvents = new List<FollowEventData>();
  public EventDataDefulat IsEventExist(string id)
  {
    for(int i = 0; i < AllNormalEvents.Count; i++)
    {
      if (string.Compare(AllNormalEvents[i].ID,id,false)==0) return AllNormalEvents[i];
    }
    for (int i = 0; i < AllFollowEvents.Count; i++)
    {
      if (string.Compare(AllFollowEvents[i].ID, id, false) == 0) return AllFollowEvents[i];
    }
    return null;
  }
  public QuestHolder_Cult Quest_Cult = null;
  private EventData defaultevent_outer = null;
  private EventData defaultevent_settlement = null;
  public EventData DefaultEvent_Outer
  {
    get
    {
      if (defaultevent_outer == null)
      {
        EventJsonData _json = new EventJsonData();
        _json.ID = "Event_NoMoreEventOuter";
        _json.PlaceInfo = "0@0@0";
        _json.Season = "";
        _json.Selection_Type = "1";
        _json.Selection_Target = "1@2";
        _json.Selection_Info = "2@1,3";
        _json.Failure_Penalty = "1@1";
        _json.Failure_Penalty_info = "1@2";
        _json.Reward_Target = "0@3";
        _json.Reward_Info = "Exp_Test@0";
        defaultevent_outer = ReturnEventDataDefault<EventData>(_json);
      }
      return defaultevent_outer;
    }
  }
  public EventData DefaultEvent_Settlement
  {
    get
    {
      if (defaultevent_settlement == null)
      {
        EventJsonData _json = new EventJsonData();
        _json.ID = "Event_NoMoreEventSettlement";
        _json.PlaceInfo = "4@0@0";
        _json.Season = "";
        _json.Selection_Type = "2";
        _json.Selection_Target = "0@1";
        _json.Selection_Info = "1@2";
        _json.Failure_Penalty = "0@1";
        _json.Failure_Penalty_info = "0@0";
        _json.Reward_Target = "2@3";
        _json.Reward_Info = "0@0";
        defaultevent_settlement = ReturnEventDataDefault<EventData>(_json);
      }
      return defaultevent_settlement;
    }
  }
  public Quest GetQuest(QuestType type)
  {
    switch (type)
    {
      case QuestType.Cult: return Quest_Cult;
    }
    return null;
  }
  public T ReturnEventDataDefault<T>(EventJsonData _data) where T : EventDataDefulat ,new()
  {
    T Data = new T();

    Data.ID = _data.ID;

    if (_data.Season != "")
    {
      string[] _seasons = _data.Season.Split('@');
      for (int i = 0; i < _seasons.Length; i++) Data.EnableSeasons.Add(int.Parse(_seasons[i]));
    }

    string[] _placeinfos = _data.PlaceInfo.Split('@');
    Data.AppearSpace = (EventAppearType)int.Parse(_placeinfos[0]);
    Data.EnvironmentType = (EnvironmentType)int.Parse(_placeinfos[1]);
    if(Data.AppearSpace!=EventAppearType.Outer)  Data.Sector = (SectorTypeEnum)int.Parse(_placeinfos[2]);

    if (_data.Selection_Type != "")
    {
      Data.Selection_type = (SelectionTypeEnum)int.Parse(_data.Selection_Type);
      switch (Data.Selection_type)//단일,이성육체,정신물질,기타 등등 분류별로 선택지,실패,성공 데이터 만들기
      {
        case SelectionTypeEnum.Single://단일 선택지
          Data.SelectionDatas = new SelectionData[1];
          Data.SelectionDatas[0] = new SelectionData(Data);

          if (_data.Selection_Target != "")
          {
            Data.SelectionDatas[0].ThisSelectionType = (SelectionTargetType)int.Parse(_data.Selection_Target);
            switch (Data.SelectionDatas[0].ThisSelectionType)
            {
              case SelectionTargetType.Pay: //지불
                Data.SelectionDatas[0].SelectionPayTarget = (StatusTypeEnum)int.Parse(_data.Selection_Info);
                break;
              case SelectionTargetType.Check_Single: //기술(단일)
                Data.SelectionDatas[0].SelectionCheckSkill.Add((SkillTypeEnum)int.Parse(_data.Selection_Info));
                break;
              case SelectionTargetType.Check_Multy: //기술(복수)
                string[] _temp = _data.Selection_Info.Split(',');
                for (int i = 0; i < _temp.Length; i++) Data.SelectionDatas[0].SelectionCheckSkill.Add((SkillTypeEnum)int.Parse(_temp[i]));
                break;
            }
            if (Data.SelectionDatas[0].ThisSelectionType.Equals(SelectionTargetType.Check_Single) ||
              Data.SelectionDatas[0].ThisSelectionType.Equals(SelectionTargetType.Check_Multy))
            {
              Data.SelectionDatas[0].FailureData = new FailureData(Data);
              Data.SelectionDatas[0].FailureData.Panelty_target = (PenaltyTarget)int.Parse(_data.Failure_Penalty);
              switch (Data.SelectionDatas[0].FailureData.Panelty_target)
              {
                case PenaltyTarget.None: break;
                case PenaltyTarget.Status: Data.SelectionDatas[0].FailureData.Loss_target = (StatusTypeEnum)int.Parse(_data.Failure_Penalty_info); break;
                case PenaltyTarget.EXP: Data.SelectionDatas[0].FailureData.ExpID = _data.Failure_Penalty_info; break;
              }
            }
            else if (Data.SelectionDatas[0].ThisSelectionType.Equals(SelectionTargetType.Pay) && Data.SelectionDatas[0].SelectionPayTarget.Equals(StatusTypeEnum.Gold))
            {
             Data.SelectionDatas[0].FailureData = GameManager.Instance.GoldFailData;
            }
            Data.SelectionDatas[0].SuccessData = new SuccessData(Data, TendencyTypeEnum.None,0);
            Data.SelectionDatas[0].SuccessData.Reward_Type = (RewardTypeEnum)int.Parse(_data.Reward_Target);
            switch (Data.SelectionDatas[0].SuccessData.Reward_Type)
            {
              case RewardTypeEnum.Experience: Data.SelectionDatas[0].SuccessData.Reward_EXPID = _data.Reward_Info; break;
              case RewardTypeEnum.Status:Data.SelectionDatas[0].SuccessData.Reward_StatusType=(StatusTypeEnum)int.Parse(_data.Reward_Info); break;
              case RewardTypeEnum.Skill: Data.SelectionDatas[0].SuccessData.Reward_SkillType = (SkillTypeEnum)int.Parse(_data.Reward_Info); break;
            }
          }

          break;
        case SelectionTypeEnum.Body://이성육체
          maketendencydata(TendencyTypeEnum.Body);
          break;
        case SelectionTypeEnum.Head://정신물질
          maketendencydata(TendencyTypeEnum.Head);
          break;
      }
      void maketendencydata(TendencyTypeEnum tendencytype)
      {
        Data.SelectionDatas = new SelectionData[2];

        if (_data.Selection_Target != "")
        {
          for (int i = 0; i < Data.SelectionDatas.Length; i++)
          {
            Data.SelectionDatas[i] = new SelectionData(Data,tendencytype, i);

            Data.SelectionDatas[i].ThisSelectionType = (SelectionTargetType)int.Parse(_data.Selection_Target.Split('@')[i]);
            switch (Data.SelectionDatas[i].ThisSelectionType)
            {
              case SelectionTargetType.Pay: //지불
                Data.SelectionDatas[i].SelectionPayTarget = (StatusTypeEnum)int.Parse(_data.Selection_Info.Split('@')[i]);
                break;
              case SelectionTargetType.Check_Single: //기술(단일)
                Data.SelectionDatas[i].SelectionCheckSkill.Add((SkillTypeEnum)int.Parse(_data.Selection_Info.Split('@')[i]));
                break;
              case SelectionTargetType.Check_Multy: //기술(복수)
                string[] _temp = _data.Selection_Info.Split('@')[i].Split(',');
                for (int j = 0; j < _temp.Length; j++) Data.SelectionDatas[i].SelectionCheckSkill.Add((SkillTypeEnum)int.Parse(_temp[j]));
                break;
            }

            if (Data.SelectionDatas[i].ThisSelectionType.Equals(SelectionTargetType.Check_Single) ||
              Data.SelectionDatas[i].ThisSelectionType.Equals(SelectionTargetType.Check_Multy))
            {
              Data.SelectionDatas[i].FailureData = new FailureData(Data,tendencytype, i);
              Data.SelectionDatas[i].FailureData.Panelty_target = (PenaltyTarget)int.Parse(_data.Failure_Penalty.Split('@')[i]);
              switch (Data.SelectionDatas[i].FailureData.Panelty_target)
              {
                case PenaltyTarget.None: break;
                case PenaltyTarget.Status: Data.SelectionDatas[i].FailureData.Loss_target = (StatusTypeEnum)int.Parse(_data.Failure_Penalty_info.Split('@')[i]); break;
                case PenaltyTarget.EXP: Data.SelectionDatas[i].FailureData.ExpID = _data.Failure_Penalty_info.Split('@')[i]; break;
              }
            }
            else if (Data.SelectionDatas[i].ThisSelectionType.Equals(SelectionTargetType.Pay) && Data.SelectionDatas[i].SelectionPayTarget.Equals(StatusTypeEnum.Gold))
            {
              Data.SelectionDatas[i].FailureData = GameManager.Instance.GoldFailData;
            }
            Data.SelectionDatas[i].SuccessData = new SuccessData(Data,tendencytype, i);
            Data.SelectionDatas[i].SuccessData.Reward_Type = (RewardTypeEnum)int.Parse(_data.Reward_Target.Split('@')[i]);
            switch (Data.SelectionDatas[i].SuccessData.Reward_Type)
            {
              case RewardTypeEnum.Experience: Data.SelectionDatas[i].SuccessData.Reward_EXPID = _data.Reward_Info.Split('@')[i]; break;
              case RewardTypeEnum.Status: Data.SelectionDatas[i].SuccessData.Reward_StatusType = (StatusTypeEnum)int.Parse(_data.Reward_Info.Split('@')[i]); break;
              case RewardTypeEnum.Skill: Data.SelectionDatas[i].SuccessData.Reward_SkillType = (SkillTypeEnum)int.Parse(_data.Reward_Info.Split('@')[i]); break;
            }

          }
        }

      }
    }

    return Data;
  }
  public void ConvertData_Normal(EventJsonData _data)
  {
    EventData Data = ReturnEventDataDefault<EventData>(_data);
      AllNormalEvents.Add(Data);
  }

  public void ConvertData_Follow(EventJsonData _data)
  {
    FollowEventData Data = ReturnEventDataDefault<FollowEventData>(_data);

    int _temp = 0;
    string[] _followdatas = _data.EventInfo.Split('@');
    FollowTypeEnum _followtype = int.TryParse(_followdatas[1], out _temp) == true ? FollowTypeEnum.Skill : FollowTypeEnum.Event;
    string _followtarget = _followdatas[1];

    Data.FollowType = _followtype; //선행 대상 이벤트/기술
    Data.FollowTarget = _followtarget;         //선행 대상- 이벤트 Id   기술 0,1,2,3

    switch (Data.FollowType)
    {
      case FollowTypeEnum.Event:
        Data.FollowTendency = int.Parse(_followdatas[2]);
        Data.FollowTargetSuccess = int.Parse(_followdatas[3]);
        break;
      case FollowTypeEnum.Skill:
        Data.FollowTargetLevel = int.Parse(_followdatas[2]);
        break;
    }
    if (_data.EndingID != "")
    {
      FollowEndingData _endingdata = new FollowEndingData();
      _endingdata.ID = $"ending_{_data.ID.Split("_")[1]}";
      _endingdata.Illust = GameManager.Instance.ImageHolder.GetEndingIllust(_endingdata.ID);
      _endingdata.Name = GameManager.Instance.GetTextData(Data.ID+"_NAME");
      _endingdata.Description = GameManager.Instance.GetTextData(Data.ID+"_DESCRIPTION");
      Data.EndingData = _endingdata;
    }
    AllFollowEvents.Add(Data);
  }
  public void ConvertData_Quest(EventJsonData _data)
  {
    switch ((QuestType)int.Parse(_data.EventInfo.Split('@')[1]))
    {
      case QuestType.Cult:
        ConvertData_Quest_cult(_data);
        break;
    }
    
  }//퀘스트 디자인 기획 끝나고 추가해야함
  public void ConvertData_Quest_cult(EventJsonData jsondata)
  {
    QuestEventData_Wolf eventdata = ReturnEventDataDefault<QuestEventData_Wolf>(jsondata);
    eventdata.Type = QuestType.Cult;
    switch (int.Parse(jsondata.EventInfo.Split('@')[2]))
    {
      case 0:
        Quest_Cult.Events_Cult_0to30.Add(eventdata);
        break;
      case 1:
        Quest_Cult.Events_Cult_30to60.Add(eventdata);
        break;
      case 2:
        Quest_Cult.Events_Cult_60to100.Add(eventdata);
        break;
    }
  }
  /// <summary>
  /// 0:없음 1:정신 2:육체 3:감정 4:물질
  /// </summary>
  /// <typeparam name="t"></typeparam>
  /// <param name="eventdata"></param>
  /// <param name="success"></param>
  /// <param name="tendency"></param>
  public void RemoveEvent(EventDataDefulat eventdata,bool success,int tendency)
  {
    if (GameManager.Instance.MyGameData.SuccessEvent_All.Contains(eventdata.ID) || GameManager.Instance.MyGameData.FailEvent_All.Contains(eventdata.ID)) return;

    if (eventdata.GetType() == typeof(QuestEventData_Wolf))
    {
      if (success) GameManager.Instance.MyGameData.SuccessEvent_All.Add(eventdata.ID);
      else GameManager.Instance.MyGameData.FailEvent_All.Add(eventdata.ID);
    }
    else
    {
      if (success == true)
      {
        switch (tendency)
        {
          case 0:
            GameManager.Instance.MyGameData.SuccessEvent_None.Add(eventdata.ID);
            break;
          case 1:
            GameManager.Instance.MyGameData.SuccessEvent_Rational.Add(eventdata.ID);
            break;
          case 2:
            GameManager.Instance.MyGameData.SuccessEvent_Physical.Add(eventdata.ID);
            break;
          case 3:
            GameManager.Instance.MyGameData.SuccessEvent_Mental.Add(eventdata.ID);
            break;
          case 4:
            GameManager.Instance.MyGameData.SuccessEvent_Material.Add(eventdata.ID);
            break;
        }
        GameManager.Instance.MyGameData.SuccessEvent_All.Add(eventdata.ID);
      }
      else
      {
        switch (tendency)
        {
          case 0:
            GameManager.Instance.MyGameData.FailEvent_None.Add(eventdata.ID);
            break;
          case 1:
            GameManager.Instance.MyGameData.FailEvent_Rational.Add(eventdata.ID);
            break;
          case 2:
            GameManager.Instance.MyGameData.FailEvent_Physical.Add(eventdata.ID);
            break;
          case 3:
            GameManager.Instance.MyGameData.FailEvent_Mental.Add(eventdata.ID);
            break;
          case 4:
            GameManager.Instance.MyGameData.FailEvent_Material.Add(eventdata.ID);
            break;
        }
        GameManager.Instance.MyGameData.FailEvent_All.Add(eventdata.ID);
      }
    }
  }
  /// <summary>
  /// 외부 이벤트 반환
  /// </summary>
  /// <param name="envir"></param>
  /// <returns></returns>
  public EventDataDefulat ReturnOutsideEvent(List<EnvironmentType> envirs)
  {
    List<EventDataDefulat> _allevents = new List<EventDataDefulat>();

    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        foreach (var _questevent in Quest_Cult.GetAvailableEvents())
        {
          if (_questevent.AppearSpace != EventAppearType.Outer) continue;
          _allevents.Add(_questevent);
        }
        break;
    }
    foreach (var _follow in AllFollowEvents)
    {
      if (GameManager.Instance.MyGameData.IsAbleEvent(_follow.ID)) continue;
      if (_follow.IsRightSeason == false) continue;
      if (_follow.AppearSpace!=EventAppearType.Outer) continue;

      switch (_follow.FollowType)
      {
        case FollowTypeEnum.Event:  //이벤트 연계일 경우 
          List<List<string>> _checktarget = new List<List<string>>();
          switch (_follow.FollowTargetSuccess)
          {
            case 0:
              switch (_follow.FollowTendency)
              {
                case 0: 
                  _checktarget.Add (GameManager.Instance.MyGameData.SuccessEvent_None);
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_Rational);
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_Mental);
                  break;
                case 1: 
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_Physical);
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_Material); break;
                case 2: 
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_All); break;
              }
              break;
            case 2:
              switch (_follow.FollowTendency)
              {
                case 0:
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_None);
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_Rational);
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_Mental);
                  break;
                case 1:
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_Physical);
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_Material); break;
                case 2:
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_All); break;
              }
              break;
            case 3:
              switch (_follow.FollowTendency)
              {
                case 0:
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_None);
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_Rational);
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_Mental);
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_None);
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_Rational);
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_Mental);
                  break;
                case 1:
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_Physical);
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_Material);
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_Physical);
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_Material); break;
                case 2:
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_All); 
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_All); break;
              }
              break;
          }
          foreach (var _list in _checktarget)
            if(_list.Contains(_follow.ID))
            _allevents.Add(_follow);
          break;
    /*    case FollowTypeEnum.EXP://경험 연계일 경우 현재 보유한 경험 ID랑 맞는지 확인
          if (_follow.FollowTarget.Equals(GameManager.Instance.MyGameData.LongTermEXP.ID)) _allevents.Add(_follow);
          foreach (var _data in GameManager.Instance.MyGameData.ShortTermEXP)
            if (_follow.FollowTarget.Equals(_data.ID)) _allevents.Add(_follow);*/
          break;
        case FollowTypeEnum.Skill://테마 연계일 경우 현재 테마의 레벨이 기준 이상인지 확인
          int _targetlevel = 0;
          SkillTypeEnum _type = SkillTypeEnum.Conversation; ;
          switch (_follow.FollowTarget)
          {
            case "0"://대화 테마
              _type = SkillTypeEnum.Conversation; break;
            case "1"://무력 테마
              _type = SkillTypeEnum.Force; break;
            case "2"://생존 테마
              _type = SkillTypeEnum.Wild; break;
            case "3"://학식 테마
              _type = SkillTypeEnum.Intelligence; break;
          }
          _targetlevel = GameManager.Instance.MyGameData.GetSkill(_type).Level;
          if (_follow.FollowTargetLevel <= _targetlevel) _allevents.Add(_follow);
          break;
      }
    }
    foreach (var _event in AllNormalEvents)
    {
      if (!GameManager.Instance.MyGameData.IsAbleEvent(_event.ID)) continue;
      if (_event.IsRightSeason == false) continue;
      if (_event.AppearSpace!=EventAppearType.Outer) continue;

      _allevents.Add(_event);
    }//해당 장소의 적합한 일반 이벤트 리스트

    List<EventDataDefulat> _envirevents = new List<EventDataDefulat>();
    List<EventDataDefulat> _noenvirevents = new List<EventDataDefulat>();
    for (int i = 0; i < _allevents.Count; i++)
    {
      if (envirs.Contains(_allevents[i].EnvironmentType)) _envirevents.Add(_allevents[i]);
      else if (_allevents[i].EnvironmentType == EnvironmentType.NULL) _noenvirevents.Add(_allevents[i]);
    }
    _allevents.Clear();
    if (_envirevents.Count > 0 && _noenvirevents.Count > 0)
    {
      if (UnityEngine.Random.Range(0, ConstValues.EventPer_Envir + ConstValues.EventPer_NoEnvir) < ConstValues.EventPer_Envir)
        foreach (var _event in _envirevents) _allevents.Add(_event);
      else
        foreach (var _event in _noenvirevents) _allevents.Add(_event);
    }
    else if (_envirevents.Count == 0 && _noenvirevents.Count > 0)
    {
      foreach (var _event in _noenvirevents) _allevents.Add(_event);
    }
    else if (_envirevents.Count > 0 && _noenvirevents.Count == 0)
    {
      foreach (var _event in _envirevents) _allevents.Add(_event);
    }

    List<EventDataDefulat> _questevents = new List<EventDataDefulat>();
    List<EventDataDefulat> _followevents = new List<EventDataDefulat>();
    List<EventDataDefulat> _normalevents = new List<EventDataDefulat>();
    foreach (var _event in _allevents)
    {
      if (_event.GetType() == typeof(QuestEventData_Wolf)) _questevents.Add(_event);
      else if (_event.GetType() == typeof(FollowEventData)) _followevents.Add(_event);
      else if (_event.GetType() == typeof(EventData)) _normalevents.Add(_event);
    }
    _allevents.Clear();
    Dictionary<List<EventDataDefulat>, int> _dic = new Dictionary<List<EventDataDefulat>, int>();
    if(_questevents.Count>0) _dic.Add(_questevents, ConstValues.EventPer_Quest);
    if(_followevents.Count>0) _dic.Add(_followevents, ConstValues.EventPer_Follow);
    if(_normalevents.Count>0) _dic.Add(_normalevents, ConstValues.EventPer_Normal);
    var _result = GetListByRatio(_dic);

    if (_result == null || _result.Count == 0)
    {
      return DefaultEvent_Outer;
    }
    else
    {
      return _result[UnityEngine.Random.Range(0, _result.Count)];
    }
  }
  /// <summary>
  /// 정착지의 장소 이벤트 반환
  /// </summary>
  /// <param name="settletype"></param>
  /// <param name="placetype"></param>
  /// <param name="placelevel"></param>
  /// <param name="envir"></param>
  /// <returns></returns>
  public EventDataDefulat ReturnPlaceEvent(SettlementType settletype, SectorTypeEnum sectortype, List<EnvironmentType> envirs)
  {
    List<EventDataDefulat> _allevents= new List<EventDataDefulat>();

    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        foreach(var _questevent in Quest_Cult.GetAvailableEvents())
        {
          if (_questevent.RightSpace(settletype) == false) continue;
          _allevents.Add(_questevent);
        }
        break;
    }
    foreach (var _follow in AllFollowEvents)
    {
      if (GameManager.Instance.MyGameData.IsAbleEvent(_follow.ID)) continue;
      if (_follow.IsRightSeason == false) continue;
      if (_follow.RightSpace(settletype) == false) continue;

      switch (_follow.FollowType)
      {
        case FollowTypeEnum.Event:  //이벤트 연계일 경우 
          List<List<string>> _checktarget = new List<List<string>>();
          switch (_follow.FollowTargetSuccess)
          {
            case 0:
              switch (_follow.FollowTendency)
              {
                case 0:
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_None);
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_Rational);
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_Mental);
                  break;
                case 1:
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_Physical);
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_Material); break;
                case 2:
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_All); break;
              }
              break;
            case 2:
              switch (_follow.FollowTendency)
              {
                case 0:
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_None);
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_Rational);
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_Mental);
                  break;
                case 1:
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_Physical);
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_Material); break;
                case 2:
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_All); break;
              }
              break;
            case 3:
              switch (_follow.FollowTendency)
              {
                case 0:
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_None);
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_Rational);
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_Mental);
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_None);
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_Rational);
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_Mental);
                  break;
                case 1:
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_Physical);
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_Material);
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_Physical);
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_Material); break;
                case 2:
                  _checktarget.Add(GameManager.Instance.MyGameData.FailEvent_All);
                  _checktarget.Add(GameManager.Instance.MyGameData.SuccessEvent_All); break;
              }
              break;
          }
          foreach (var _list in _checktarget)
            if (_list.Contains(_follow.ID))
              _allevents.Add(_follow);
          break;
        case FollowTypeEnum.Skill://테마 연계일 경우 현재 테마의 레벨이 기준 이상인지 확인
          int _targetlevel = 0;
          SkillTypeEnum _type = SkillTypeEnum.Conversation; ;
          switch (_follow.FollowTarget)
          {
            case "0"://대화 테마
              _type = SkillTypeEnum.Conversation; break;
            case "1"://무력 테마
              _type = SkillTypeEnum.Force; break;
            case "2"://생존 테마
              _type = SkillTypeEnum.Wild; break;
            case "3"://학식 테마
              _type = SkillTypeEnum.Intelligence; break;
          }
          _targetlevel = GameManager.Instance.MyGameData.GetSkill(_type).Level;
          if (_follow.FollowTargetLevel <= _targetlevel) _allevents.Add(_follow);
          break;
      }
    }
    foreach (var _event in AllNormalEvents)
    {
      if (GameManager.Instance.MyGameData.IsAbleEvent(_event.ID)) continue;
      if (_event.IsRightSeason == false) continue;
      if (_event.RightSpace(settletype) == false) continue;

      _allevents.Add(_event);
    }//해당 장소의 적합한 일반 이벤트 리스트

    List<EventDataDefulat> _envirevents= new List<EventDataDefulat>();
    List<EventDataDefulat> _noenvirevents = new List<EventDataDefulat>();
    for(int i = 0; i < _allevents.Count; i++)
    {
      if (envirs.Contains(_allevents[i].EnvironmentType)) _envirevents.Add(_allevents[i]);
      else if (_allevents[i].EnvironmentType == EnvironmentType.NULL) _noenvirevents.Add(_allevents[i]);
    }
    _allevents.Clear();
    if (_envirevents.Count > 0 && _noenvirevents.Count > 0)
    {
      if(UnityEngine.Random.Range(0,ConstValues.EventPer_Envir+ConstValues.EventPer_NoEnvir)<ConstValues.EventPer_Envir)
        foreach (var _event in _envirevents) _allevents.Add(_event);
      else
        foreach (var _event in _noenvirevents) _allevents.Add(_event);
    }
    else if (_envirevents.Count == 0 && _noenvirevents.Count > 0)
    {
      foreach(var _event in _noenvirevents)_allevents.Add(_event);
    }
    else if (_envirevents.Count > 0 && _noenvirevents.Count == 0)
    {
      foreach (var _event in _envirevents) _allevents.Add(_event);
    }

    List<EventDataDefulat> _sectorevents = new List<EventDataDefulat>();
    List<EventDataDefulat> _notsectorevents= new List<EventDataDefulat>();
    foreach(var _event in _allevents)
    {
      if(sectortype==_event.Sector)_sectorevents.Add(_event);
      else _notsectorevents.Add(_event);
    }
    if (_sectorevents.Count > 0 && _notsectorevents.Count > 0)
    {
      if (UnityEngine.Random.Range(0, ConstValues.EventPer_Sector + ConstValues.EventPer_NoSector) < ConstValues.EventPer_Sector)
        foreach (var _event in _sectorevents) _allevents.Add(_event);
      else
        foreach (var _event in _notsectorevents) _allevents.Add(_event);
    }
    else if (_sectorevents.Count == 0 && _notsectorevents.Count > 0)
    {
      foreach (var _event in _notsectorevents) _allevents.Add(_event);
    }
    else if (_sectorevents.Count > 0 && _notsectorevents.Count == 0)
    {
      foreach (var _event in _sectorevents) _allevents.Add(_event);
    }

    List<EventDataDefulat> _questevents= new List<EventDataDefulat>();
    List<EventDataDefulat> _followevents= new List<EventDataDefulat>();
    List<EventDataDefulat> _normalevents=new List<EventDataDefulat>();
    foreach(var _event in _allevents)
    {
      if (_event.GetType() == typeof(QuestEventData_Wolf)) _questevents.Add(_event);
      else if(_event.GetType()==typeof(FollowEventData))_followevents.Add(_event);
      else if(_event.GetType()==typeof(EventData))_normalevents.Add(_event);
    }
    _allevents.Clear();
    Dictionary<List<EventDataDefulat>, int> _dic = new Dictionary<List<EventDataDefulat>, int>();
    _dic.Add(_questevents, ConstValues.EventPer_Quest);
    if (_questevents.Count > 0) _dic.Add(_questevents, ConstValues.EventPer_Quest);
    if (_followevents.Count > 0) _dic.Add(_followevents, ConstValues.EventPer_Follow);
    if (_normalevents.Count > 0) _dic.Add(_normalevents, ConstValues.EventPer_Normal);
    var _result = GetListByRatio(_dic);

    if (_result == null|| _result.Count == 0)
    {
      return DefaultEvent_Settlement;
    }
    else
      return _result[UnityEngine.Random.Range(0,_result.Count)];
  }
  private List<T> GetListByRatio<T>(Dictionary<List<T>,int> listAndvalue)
  {
    List<List<T>> _availabelists= new List<List<T>>();
    List<int> _availablevalues= new List<int>();
    int _max = 0;
    foreach(var dic in listAndvalue)
    {
      if (dic.Key.Count == 0) continue;

      _availabelists.Add(dic.Key);
      _availablevalues.Add(dic.Value);
      _max += dic.Value;
    }
    _max = UnityEngine.Random.Range(0, _max);
    int _sum = 0;
    for(int i = 0; i < _availabelists.Count; i++)
    {
      _sum += _availablevalues[i];
      if (_max < _sum) return _availabelists[i];
    }
    if (_availabelists.Count > 0)
      return _availabelists[_availabelists.Count];
    else return null;
  }
}
public class TileInfoData
{
  public LandmarkType Landmark = LandmarkType.Outer;
  public Settlement Settlement=null; //정착지 타입
  public List<EnvironmentType> EnvirList = new List<EnvironmentType>();//주위 환경 타입
  
}
#region 이벤트 정보에 쓰는 배열들
public enum FollowTypeEnum { Event,Skill}
public enum SettlementType {Village,Town,City,Outer}
public enum EventAppearType { Outer, Village, Town, City, Settlement}
public enum SectorTypeEnum {NULL, Residence, Temple,Marketplace, Library,Theater,Academy}
public enum EnvironmentType { NULL, River,Forest,Mountain,Sea,Beach,Land,RiverBeach, Highland }
public enum SelectionTypeEnum { Single,Body, Head,Tendency,Experience }// (Vertical)Body : 좌 이성 우 육체    (Horizontal)Head : 좌 정신 우 물질    
public enum PenaltyTarget { None,Status,EXP }
public enum RewardTypeEnum {None, Status,Skill, Experience }
public enum EventSequence { Progress,Clear}//Suggest: 3개 제시하는 단계  Progress: 선택지 버튼 눌러야 하는 단계  Clear: 보상 수령해야 하는 단계
#endregion  
public class EventDataDefulat
{
  /// <summary>
  /// "Event_이름"
  /// </summary>
  public string ID = "";
  public int BeginningLength
  {
    get
    {
      return BeginningDescriptions.Count;
    }
  }
  public string Name
  {
    get
    {
      string _str = GameManager.Instance.GetTextData(ID + "_Name");
      return WNCText.GetSeasonText(_str);
    }
  }
  public List<EventIllustHolder> BeginningIllusts
  {
    get
    {
     return  GameManager.Instance.ImageHolder.GetEventIllusts(ID,"_Beginning",BeginningLength );
    }
  }
  private List<string> beginningdescriptions=new List<string>();
  public List<string> BeginningDescriptions
  {
    get
    {
      if (beginningdescriptions.Count == 0)
      {
        string _str = GameManager.Instance.GetTextData(ID + "_Descriptions");
        List<string> _temp = _str.Split('@').ToList();
        for (int i = 0; i < _temp.Count; i++)
        {
          beginningdescriptions.Add(WNCText.GetSeasonText(_temp[i]));
        }
      }
      return beginningdescriptions;
    }
  }
  public List<int> EnableSeasons = new List<int>();  //0개면 사계절 분화 없음
  public int Season
  {
    get
    {
      if (EnableSeasons.Contains(4)) return 4;
      return GameManager.Instance.MyGameData.Turn;
    }
  }
  public bool IsRightSeason
  {
    get
    {
      if (EnableSeasons.Count == 0) return true;
      else
      {
        if (EnableSeasons.Contains(GameManager.Instance.MyGameData.Turn - 1)) return true;
      }
      return false;
    }
  }
  public EventAppearType AppearSpace;
    public bool RightSpace(SettlementType currentsettle)
    {
        switch (AppearSpace)
        {
            case EventAppearType.Outer:
                return false;
            case EventAppearType.Village:
                if (currentsettle == SettlementType.Village) return true;
                break;
            case EventAppearType.Town:
                if (currentsettle == SettlementType.Town) return true;
                break;
            case EventAppearType.City:
                if (currentsettle == SettlementType.City) return true;
                break;
            case EventAppearType.Settlement:
                if (currentsettle == SettlementType.Outer) return false;
                return true;
        }
        return false;
    }

  public SectorTypeEnum Sector = SectorTypeEnum.NULL;
  public EnvironmentType EnvironmentType = EnvironmentType.NULL;

  public SelectionTypeEnum Selection_type;
  public SelectionData[] SelectionDatas;
}
public class SelectionData
{
  private EventDataDefulat MyEvent = null;
  public TendencyTypeEnum Tendencytype = TendencyTypeEnum.None;
  /// <summary>
  /// 0,1
  /// </summary>
  private int Index = 0;
  /// <summary>
  /// 성향 없는 경우
  /// </summary>
  /// <param name="myevent"></param>
  public SelectionData(EventDataDefulat myevent)
  {
    MyEvent = myevent;
    Tendencytype = TendencyTypeEnum.None;
    Index = 0;
  }
  public SelectionData(EventDataDefulat myevent,TendencyTypeEnum tendencytype, int index)
  {
    MyEvent = myevent;Index = index; Tendencytype = tendencytype;
  }
  private string ID { get { return MyEvent.ID; } }
  public string Name
  {
    get { return WNCText.GetSeasonText(GameManager.Instance.GetTextData(ID + "_Selecting").Split('@')[Index]); }
  }
/*  public string SubDescription
  {
    get { return WNCText.GetSeasonText(GameManager.Instance.GetTextData(ID + "_Selecting_Subdescriptions").Split('@')[Index]); }
  }
*/
  public SelectionTargetType ThisSelectionType = SelectionTargetType.Pay;

  public StatusTypeEnum SelectionPayTarget = StatusTypeEnum.HP;                       //Pay일때 사용
  public List<SkillTypeEnum> SelectionCheckSkill = new List<SkillTypeEnum>();         //Check_Single,Check_Multy일때 사용


  public SuccessData SuccessData = null;
  public FailureData FailureData = null;
}    

public class FailureData
{
  private EventDataDefulat MyEvent = null;
  public int Index = 0;
  public TendencyTypeEnum Tendencytype = TendencyTypeEnum.None;
  /// <summary>
  /// 성향 없는 경우
  /// </summary>
  /// <param name="myevent"></param>
  public FailureData(EventDataDefulat myevent)
  {
    MyEvent = myevent; Index = 0; Tendencytype = TendencyTypeEnum.None;
  }
  public FailureData(EventDataDefulat myevent,TendencyTypeEnum tendencytype, int index)
  {
    MyEvent = myevent; Index = index; Tendencytype = tendencytype;
  }
  private string OriginID { get { return MyEvent.ID; } }
  private string TypeID { get { return "_" + (Tendencytype == TendencyTypeEnum.None ? "" : Index == 0 ? "L" : "R") + "Fail"; } }
  private List<string> descriptions=new List<string>();
  public List<string> Descriptions
  {
    get
    {
      if (descriptions.Count == 0)
      {
        List<string> _temp = GameManager.Instance.GetTextData(OriginID+TypeID+"_Descriptions").Split('@').ToList();
        for (int i = 0; i < _temp.Count; i++)
        {
          descriptions.Add(WNCText.GetSeasonText(_temp[i]));
        }
      }
      return descriptions;
    }
  }
  public List<EventIllustHolder> Illusts
  {
    get
    {
      return GameManager.Instance.ImageHolder.GetEventIllusts(OriginID, TypeID, Descriptions.Count);
    }
  }
  public PenaltyTarget Panelty_target;
  public StatusTypeEnum Loss_target= StatusTypeEnum.HP;
  public string ExpID;
}
public class GoldFailData : FailureData
{
  public GoldFailData() : base(null,TendencyTypeEnum.None, -1) { }
  public string Description = "";
  public Sprite Illust = null;
}
public enum SelectionTargetType { None,Pay, Check_Single,Check_Multy}//선택지 개별 내용
public enum StatusTypeEnum { HP,Sanity,Gold}
public class SuccessData
{
  public TendencyTypeEnum Tendencytype = TendencyTypeEnum.None;
  private EventDataDefulat MyEvent = null;
  public int Index = 0;
  /// <summary>
  /// 성향 없는 경우
  /// </summary>
  /// <param name="myevent"></param>
  public SuccessData(EventDataDefulat myevent,TendencyTypeEnum tendencytype, int index)
  {
    MyEvent = myevent; Index = index; Tendencytype = tendencytype;
  }
  private string OriginID { get { return MyEvent.ID; } }
  private string TypeID { get { return "_" + (Tendencytype == TendencyTypeEnum.None ? "" : Index == 0 ? "L" : "R") + "Success"; } }
  private List<string> descriptions= new List<string>();
  public List<string> Descriptions
  {
    get
    {
      if (descriptions.Count == 0)
      {
        List<string> _temp = GameManager.Instance.GetTextData(OriginID+TypeID + "_Descriptions").Split('@').ToList();
        for (int i = 0; i < _temp.Count; i++)
        {
          descriptions.Add(WNCText.GetSeasonText(_temp[i]));
        }
      }
      return descriptions;
    }
  }
  public List<EventIllustHolder> Illusts
  {
    get
    {
      return GameManager.Instance.ImageHolder.GetEventIllusts(OriginID,TypeID, Descriptions.Count);
    }
  }
  public RewardTypeEnum Reward_Type;

  public StatusTypeEnum Reward_StatusType;
  public SkillTypeEnum Reward_SkillType;
  public string Reward_EXPID;
}

public class EventData:EventDataDefulat
{
}//기본 이벤트
public class FollowEventData:EventDataDefulat
{
  public FollowTypeEnum FollowType = 0;
  public string FollowTarget = "";
  public int FollowTargetLevel = 0;
  /// <summary>
  /// 0:성공 1:실패 2:노상관
  /// </summary>
  public int FollowTargetSuccess = 0;
  /// <summary>
  /// 0:왼쪽 1:오른쪽 2:노상관
  /// </summary>
  public int FollowTendency = 0; 

  public FollowEndingData EndingData = null;
}//연계 이벤트
public class FollowEndingData
{
  public string ID = "";
  public Sprite Illust = null;
  public string Name = "";
  public string Description = "";
}
public class QuestEventData_Wolf : EventDataDefulat
{
  public QuestType Type;
  public QuestEnum_Cult EventType;
}
public enum QuestType { Cult}
public enum QuestEnum_Cult { None,Prologue,Starting, Public, Sabbat,Ritual}
public class Quest
{
  public string OriginID="";
  public string QuestName { get { return GameManager.Instance.GetTextData(OriginID + "_Name_Text"); } }
  public string QuestDescription { get { return GameManager.Instance.GetTextData(OriginID + "_PreDescription_Text"); } }
    public Sprite QuestIllust { get { return GameManager.Instance.ImageHolder.Quest_Cult_MainIllust; } }
  public QuestType Type = QuestType.Cult;
  public Quest(string id,QuestType type)
  {
    OriginID = id;
    Type = type;
  }
}
public class QuestHolder_Cult:Quest
{
  public QuestHolder_Cult(string id, QuestType type) : base(id, type)
  {
    OriginID = id;
    Type = type;
  }
  #region 프롤로그 관련
  public Sprite Prologue_0_Illust { get { return GameManager.Instance.ImageHolder.GetCultIllust(GameManager.Instance.ImageHolder.Cult_Prologue, OriginID + "_Prologue_0_Illust"); } }
  public string Prologue_0_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_0_Description"); } }

  public Sprite Prologue_1_Illust { get { return GameManager.Instance.ImageHolder.GetCultIllust(GameManager.Instance.ImageHolder.Cult_Prologue, OriginID + "_Prologue_1_Illust"); } }
  public string Prologue_1_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_1_Description"); } }
  public string Prologue_1_Selection_0 { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_1_Selection_0"); } }
  public string Prologue_1_Selection_1 { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_1_Selection_1"); } }
 
  public Sprite Prologue_2_0_Illust { get { return GameManager.Instance.ImageHolder.GetCultIllust(GameManager.Instance.ImageHolder.Cult_Prologue, OriginID + "_Prologue_2_0_Illust"); } }
  public string Prologue_2_0_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_2_0_Description"); } }

  public Sprite Prologue_2_1_Illust { get { return GameManager.Instance.ImageHolder.GetCultIllust(GameManager.Instance.ImageHolder.Cult_Prologue, OriginID + "_Prologue_2_1_Illust"); } }
  public string Prologue_2_1_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_2_1_Description"); } }
 
  public Sprite Prologue_3_Illust { get { return GameManager.Instance.ImageHolder.GetCultIllust(GameManager.Instance.ImageHolder.Cult_Prologue, OriginID + "_Prologue_3_Illust"); } }
  public string Prologue_3_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_3_Description"); } }
  public string Prologue_3_Selection_0 { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_3_Selection_0"); } }
  public string Prologue_3_Selection_1 { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_3_Selection_1"); } }

  public Sprite Prologue_4_0_Illust { get { return GameManager.Instance.ImageHolder.GetCultIllust(GameManager.Instance.ImageHolder.Cult_Prologue, OriginID + "_Prologue_4_0_Illust"); } }
  public string Prologue_4_0_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_4_0_Description"); } }
  public Sprite Prologue_4_1_Illust { get { return GameManager.Instance.ImageHolder.GetCultIllust(GameManager.Instance.ImageHolder.Cult_Prologue, OriginID + "_Prologue_4_1_Illust"); } }
  public string Prologue_4_1_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_4_1_Description"); } }

  public Sprite Prologue_5_Illust { get { return GameManager.Instance.ImageHolder.GetCultIllust(GameManager.Instance.ImageHolder.Cult_Prologue, OriginID + "_Prologue_5_Illust"); } }
  public string Prologue_5_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_5_Description"); } }
  public Sprite Prologue_6_Illust { get { return GameManager.Instance.ImageHolder.GetCultIllust(GameManager.Instance.ImageHolder.Cult_Prologue, OriginID + "_Prologue_6_Illust"); } }
  public string Prologue_6_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_6_Description"); } }
  public Sprite Prologue_7_Illust { get { return GameManager.Instance.ImageHolder.GetCultIllust(GameManager.Instance.ImageHolder.Cult_Prologue, OriginID + "_Prologue_7_Illust"); } }
  public string Prologue_7_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_7_Description"); } }
  public Sprite Prologue_8_Illust { get { return GameManager.Instance.ImageHolder.GetCultIllust(GameManager.Instance.ImageHolder.Cult_Prologue, OriginID + "_Prologue_8_Illust"); } }
  public string Prologue_8_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_8_Description"); } }
  #endregion

  public List<QuestEventData_Wolf> Events_Cult_0to30 = new List<QuestEventData_Wolf>(); 
  public List<QuestEventData_Wolf> Events_Cult_30to60 = new List<QuestEventData_Wolf>();
  public List<QuestEventData_Wolf> Events_Cult_60to100 = new List<QuestEventData_Wolf>();

  /// <summary>
  /// Phase에 맞춰 사용 가능한 이벤트들을 반환(0,1,2)
  /// </summary>
  /// <returns></returns>
  public List<EventDataDefulat> GetAvailableEvents()
  {
    List<List<QuestEventData_Wolf>> _availablelists = new List<List<QuestEventData_Wolf>>();

    _availablelists.Add(Events_Cult_0to30);
    if (GameManager.Instance.MyGameData.Quest_Cult_Phase>0)
      _availablelists.Add(Events_Cult_30to60);
    if (GameManager.Instance.MyGameData.Quest_Cult_Phase > 1)
      _availablelists.Add(Events_Cult_60to100);

    List<EventDataDefulat> _availableevents=new List<EventDataDefulat>();
    foreach(var list in _availablelists)
      foreach(var _event in list) _availableevents.Add(_event);

    return _availableevents;
  }

  public Tuple<Sprite,string,string> GetSettlementData
  {
    get
    {
      int _count = GameManager.Instance.ImageHolder.Cult_Settlement.Count / 4;
      int _index=UnityEngine.Random.Range(0,_count);
      if (GameManager.Instance.MyGameData.Cult_Progress_SettlementEventIndex.Count < _count)
      while(!GameManager.Instance.MyGameData.Cult_Progress_SettlementEventIndex.Contains(_index)) _index = UnityEngine.Random.Range(0, _count);

      Sprite _illust = GameManager.Instance.ImageHolder.Cult_Settlement[_index * 4 + GameManager.Instance.MyGameData.Turn];
      string _filename = _illust.name.Split('_')[0];
      return new Tuple<Sprite, string, string>(
        _illust,
        WNCText.GetSeasonText(GameManager.Instance.GetTextData(_filename+"_description")),
        GameManager.Instance.GetTextData(_filename+"_selecting"));
    }
  }
  public Tuple<Sprite, string, string> GetSabbatData
  {
    get
    {
      int _count = GameManager.Instance.ImageHolder.Cult_Sabbat.Count / 4;
      int _index = UnityEngine.Random.Range(0, _count);
      if (GameManager.Instance.MyGameData.Cult_Progress_SabbatEventIndex.Count < _count)
        while (!GameManager.Instance.MyGameData.Cult_Progress_SabbatEventIndex.Contains(_index)) _index = UnityEngine.Random.Range(0, _count);

      Sprite _illust = GameManager.Instance.ImageHolder.Cult_Sabbat[_index * 4 + GameManager.Instance.MyGameData.Turn];
      string _filename = _illust.name.Split('_')[0];
      return new Tuple<Sprite, string, string>(
        _illust,
        WNCText.GetSeasonText(GameManager.Instance.GetTextData(_filename + "_description")),
        GameManager.Instance.GetTextData(_filename + "_selecting"));
    }
  }
  public Tuple<Sprite, string, string> GetRitualData
  {
    get
    {
      int _count = GameManager.Instance.ImageHolder.Cult_Ritual.Count / 4;
      int _index = UnityEngine.Random.Range(0, _count);
      if (GameManager.Instance.MyGameData.Cult_Progress_RitualEventIndex.Count < _count)
        while (!GameManager.Instance.MyGameData.Cult_Progress_RitualEventIndex.Contains(_index)) _index = UnityEngine.Random.Range(0, _count);

      Sprite _illust = GameManager.Instance.ImageHolder.Cult_Ritual[_index * 4 + GameManager.Instance.MyGameData.Turn];
      string _filename = _illust.name.Split('_')[0];
      return new Tuple<Sprite, string, string>(
        _illust,
        WNCText.GetSeasonText(GameManager.Instance.GetTextData(_filename + "_description")),
        GameManager.Instance.GetTextData(_filename + "_selecting"));
    }
  }
  public Tuple<Sprite,string> GetPhaseUpgradeData
  {
    get
    {
      List<Sprite> _list = new List<Sprite>();
      string _description = "";
      switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
      {
        case 1:
          _list = GameManager.Instance.ImageHolder.Cult_30;
          _description = GameManager.Instance.GetTextData("Cult_ProgressUpgrade_30");
          break;
        case 2:
          _list = GameManager.Instance.ImageHolder.Cult_60;
          _description = GameManager.Instance.GetTextData("Cult_ProgressUpgrade_60");
          break;
        case 3:
          _list = GameManager.Instance.ImageHolder.Cult_100;
          _description = GameManager.Instance.GetTextData("Cult_ProgressUpgrade_100");
          break;
      }

      return new Tuple<Sprite, string>(_list[UnityEngine.Random.Range(0, _list.Count)], _description);
    }
  }

}
[System.Serializable]
public class EventJsonData
{
  public string ID = "";              //ID

  public string EventInfo = "";
  //0(혹은 공백) : 일반 이벤트                               0
  //1:연계 이벤트                                           1@(A)@(B)@(C)
  //A:이벤트ID                               기술인덱스
  //B:이벤트 성향(0:왼쪽,1:오른쪽,2:노상관)    기술 레벨n
  //C:이벤트 결과(0:성공,1:실패,  2:노상관)

  //2:퀘스트                                                2@(퀘스트번호)@(0/1)
  public string PlaceInfo = "";          //0,1,2,3
  public string Season ;              //전역,봄,여름,가을,겨울

  public string Selection_Type;           //0.단일 1.이성+육체 2.정신+물질 
  public string Selection_Target;           //0.지불 1.기술(1) 2.기술(2)
  public string Selection_Info;             //0:체,정,골   1:기술1개  2:기술@기술

  public string Failure_Penalty;            //없음, 손실
  public string Failure_Penalty_info;       //0:X   1:체력,정신력,돈

  public string Reward_Target;              //0:경험 1:체력 2:정신력 3:돈  4:기술               5:X
  public string Reward_Info;                //  ID                         대화,무력,생존,학식

  public string EndingID = "";
}


