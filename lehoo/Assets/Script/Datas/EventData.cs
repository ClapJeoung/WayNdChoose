using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Unity.Mathematics;

public class EventHolder
{

  public List<EventData> AllEvent = new List<EventData>();
  public EventData IsEventExist(string id)
  {
    for(int i = 0; i < AllEvent.Count; i++)
    {
      if (string.Compare(AllEvent[i].ID,id,false)==0) return AllEvent[i];
    }
    return null;
  }
  public QuestHolder_Cult Quest_Cult = null;
  public EventData GetEvent(string id)
  {
    foreach(EventData e in AllEvent)
      if(e.ID== id) return e;
    return null;
  }

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
        _json.Selection_Type = "1";
        _json.Selection_Target = "2@3";
        _json.Selection_Info = "2@1,3";
        _json.Failure_Penalty = "1@1";
        _json.Failure_Penalty_info = "1@2";
        _json.Reward_Target = "3@0";
        _json.Reward_Info = "Exp_Test@0";
        defaultevent_outer = ReturnEventDataDefault(_json);
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
        _json.Selection_Type = "2";
        _json.Selection_Target = "0@1";
        _json.Selection_Info = "1@2";
        _json.Failure_Penalty = "0@1";
        _json.Failure_Penalty_info = "0@0";
        _json.Reward_Target = "2@2";
        _json.Reward_Info = "0@0";
        defaultevent_settlement = ReturnEventDataDefault(_json);
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
  public EventData ReturnEventDataDefault(EventJsonData _data) 
  {
    EventData Data=new EventData();
    Data.ID = _data.ID;

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
              Data.SelectionDatas[0].ThisSelectionType.Equals(SelectionTargetType.Check_Multy)||
                Data.SelectionDatas[0].ThisSelectionType.Equals(SelectionTargetType.Pay) && Data.SelectionDatas[0].SelectionPayTarget.Equals(StatusTypeEnum.Gold))
            {
              Data.SelectionDatas[0].FailData = new FailData(Data);
              Data.SelectionDatas[0].FailData.Penelty_target = (PenaltyTarget)int.Parse(_data.Failure_Penalty);
              switch (Data.SelectionDatas[0].FailData.Penelty_target)
              {
                case PenaltyTarget.None: break;
                case PenaltyTarget.Status: Data.SelectionDatas[0].FailData.StatusType = (StatusTypeEnum)int.Parse(_data.Failure_Penalty_info); break;
                case PenaltyTarget.EXP: Data.SelectionDatas[0].FailData.ExpID = _data.Failure_Penalty_info; break;
              }
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

            Data.SelectionDatas[i].StopEvent = _data.EventLine != "" && _data.EventLine.Split('@').Length > 1 ?
              int.Parse(_data.EventLine.Split('@')[1]) == i : false;

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
              Data.SelectionDatas[i].ThisSelectionType.Equals(SelectionTargetType.Check_Multy)||
                              Data.SelectionDatas[0].ThisSelectionType.Equals(SelectionTargetType.Pay) && Data.SelectionDatas[0].SelectionPayTarget.Equals(StatusTypeEnum.Gold))

            {
              Data.SelectionDatas[i].FailData = new FailData(Data,tendencytype, i);
              Data.SelectionDatas[i].FailData.Penelty_target = (PenaltyTarget)int.Parse(_data.Failure_Penalty.Split('@')[i]);
              switch (Data.SelectionDatas[i].FailData.Penelty_target)
              {
                case PenaltyTarget.None: break;
                case PenaltyTarget.Status: Data.SelectionDatas[i].FailData.StatusType = (StatusTypeEnum)int.Parse(_data.Failure_Penalty_info.Split('@')[i]); break;
                case PenaltyTarget.EXP: Data.SelectionDatas[i].FailData.ExpID = _data.Failure_Penalty_info.Split('@')[i]; break;
              }
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

    Data.EventLine = _data.EventLine.Split('@')[0];
    return Data;
  }
  public void ConvertData_Normal(EventJsonData _data)
  {
    EventData Data = ReturnEventDataDefault(_data);
      AllEvent.Add(Data);
  }
  public void ConvertData_Follow(EventJsonData _data)
  {
    EventData Data = ReturnEventDataDefault(_data);
    Data.EventType = EventTypeEnum.Follow;

    string[] _followdatas = _data.EventInfo.Split('@');
    FollowTypeEnum _followtype = _followdatas.Length<3 ? FollowTypeEnum.Exp : FollowTypeEnum.Event;
    string _followtarget = _followdatas[1];

    Data.FollowType = _followtype; //선행 대상 이벤트/기술
    Data.FollowID = _followtarget;         //선행 대상- 이벤트 Id   기술 0,1,2,3

    switch (Data.FollowType)
    {
      case FollowTypeEnum.Event:
        Data.FollowTendency = int.Parse(_followdatas[2]);
        Data.FollowTargetSuccess = int.Parse(_followdatas[3]);
        break;
      case FollowTypeEnum.Exp:
        break;
    }
    Data.EndingID = _data.EndingID;

    AllEvent.Add(Data);
  }
  public void ConvertData_Quest(EventJsonData _data)
  {
    switch ((QuestType)int.Parse(_data.EventInfo.Split('@')[1]))
    {
      case QuestType.Cult:
        ConvertData_Quest_cult(_data);
        break;
    }

  }
  public void ConvertData_Quest_cult(EventJsonData jsondata)
  {
    EventData eventdata = ReturnEventDataDefault(jsondata);
    eventdata.EventType = EventTypeEnum.Cult;
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
    AllEvent.Add(eventdata);
  }
  /// <summary>
  /// 0:없음 1:정신 2:육체 3:감정 4:물질
  /// </summary>
  /// <typeparam name="t"></typeparam>
  /// <param name="eventdata"></param>
  /// <param name="success"></param>
  /// <param name="tendency"></param>
  public void RemoveEvent(EventData eventdata,bool success,int tendency)
  {
    if (GameManager.Instance.MyGameData.SuccessEvent_All.Contains(eventdata.ID) || GameManager.Instance.MyGameData.FailEvent_All.Contains(eventdata.ID)) return;

    if (eventdata.EventType==EventTypeEnum.Cult)
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
  public EventData ReturnOutsideEvent(List<EnvironmentType> envirs)
  {
    List<EventData> _disableevents=new List<EventData>();
    List<EventData> _ableevents = new List<EventData>();

    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        foreach (var _questevent in Quest_Cult.GetAvailableEvents())
        {
          if (_questevent.AppearSpace != EventAppearType.Outer) continue;
          if (_questevent.EnvironmentType != EnvironmentType.NULL)
          {
            switch (_questevent.EnvironmentType)
            {
              case EnvironmentType.Sea:
                if (!envirs.Contains(EnvironmentType.Beach) &&
                  !envirs.Contains(EnvironmentType.RiverBeach)) continue;
                break;
              default:
                if (!envirs.Contains(_questevent.EnvironmentType)) continue;
                break;
            }
          }

          if (GameManager.Instance.MyGameData.IsAbleEvent(_questevent.ID) == false) { _disableevents.Add(_questevent); }
          else { _ableevents.Add(_questevent); }
        }
        break;
    }
    foreach (var _event in AllEvent)
    {
      if (_event.AppearSpace!=EventAppearType.Outer) continue;
      if (GameManager.Instance.MyGameData.CurrentEventLine != "" &&
        GameManager.Instance.MyGameData.CurrentEventLine != _event.EventLine!) continue;
      if (_event.EnvironmentType != EnvironmentType.NULL)
      {
        switch (_event.EnvironmentType)
        {
          case EnvironmentType.Sea:
            if (!envirs.Contains(EnvironmentType.Beach) &&
              !envirs.Contains(EnvironmentType.RiverBeach)) continue;
            break;
          default:
            if (!envirs.Contains(_event.EnvironmentType)) continue;
            break;
        }
      }

      switch (_event.EventType)
      {
        case EventTypeEnum.Default:
          if (GameManager.Instance.MyGameData.IsAbleEvent(_event.ID) == false) { _disableevents.Add(_event); }
          else { _ableevents.Add(_event); }
          break;
        case EventTypeEnum.Follow:
          switch (_event.FollowType)
          {
            case FollowTypeEnum.Event:  //이벤트 연계일 경우 
            //  if (_event.ID == "EV_Wheat")
             //   Debug.Log("레후");
              List<List<string>> _checktarget = new List<List<string>>();
              switch (_event.FollowTargetSuccess)
              {
                case 0:
                  switch (_event.FollowTendency)
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
                case 1:
                  switch (_event.FollowTendency)
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
                case 2:
                  switch (_event.FollowTendency)
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
                if (_list.Contains(_event.FollowID))
                {
                  if (GameManager.Instance.MyGameData.IsAbleEvent(_event.ID) == false) { _disableevents.Add(_event); }
                  else { _ableevents.Add(_event); }
                  break;
                }
              break;
            case FollowTypeEnum.Exp://테마 연계일 경우 현재 테마의 레벨이 기준 이상인지 확인
              if ((GameManager.Instance.MyGameData.LongExp!=null&&GameManager.Instance.MyGameData.LongExp.ID==_event.FollowID)
                || (GameManager.Instance.MyGameData.ShortExp_A != null && GameManager.Instance.MyGameData.ShortExp_A.ID == _event.FollowID)
                || (GameManager.Instance.MyGameData.ShortExp_B != null && GameManager.Instance.MyGameData.ShortExp_B.ID == _event.FollowID))
                if (GameManager.Instance.MyGameData.IsAbleEvent(_event.ID) == false) { _disableevents.Add(_event); }
                else { _ableevents.Add(_event); }
              break;
          }
          break;
      }
    }

    List<EventData> _targetevents = _ableevents.Count > 0 ? _ableevents : _disableevents;

    List<string> _eventlist= new List<string>();
    for(int i = 0; i < _targetevents.Count; i++)
    {
      int _count = 0;
      EventData _event = _targetevents[i];
      switch(_event.EventType)
      {
        case EventTypeEnum.Default: _count += ConstValues.EventPer_Normal;break;
        case EventTypeEnum.Follow: _count += _event.FollowType==FollowTypeEnum.Event?ConstValues.EventPer_Follow_Ev:ConstValues.EventPer_Follow_Ex;break;
        case EventTypeEnum.Cult: _count += ConstValues.EventPer_Quest;break;
      }

      if (envirs.Contains(_event.EnvironmentType)) _count *= ConstValues.EventPer_Envir;
      else _count *= ConstValues.EventPer_NoEnvir;

      for (int j = 0; j < _count; j++) _eventlist.Add(_event.ID);
    }


    string _resultid = _eventlist[UnityEngine.Random.Range(0, _eventlist.Count)];
    foreach (EventData _event in _targetevents)
      if (_event.ID == _resultid) return _event;

    Debug.Log("여기까지 올 리가 없는디");
    return null;
  }
  /// <summary>
  /// 정착지의 장소 이벤트 반환
  /// </summary>
  /// <param name="settletype"></param>
  /// <param name="placetype"></param>
  /// <param name="placelevel"></param>
  /// <param name="envir"></param>
  /// <returns></returns>
  public EventData ReturnSectorEvent(SettlementType settletype, SectorTypeEnum sectortype, List<EnvironmentType> envirs)
  {
    List<EventData> _disableevents = new List<EventData>();
    List<EventData> _ableevents = new List<EventData>();

    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        foreach (var _questevent in Quest_Cult.GetAvailableEvents())
        {
          if (_questevent.RightSpace(settletype,_questevent.Sector) == false) continue;
          if (_questevent.EnvironmentType != EnvironmentType.NULL)
          {
            switch (_questevent.EnvironmentType)
            {
              case EnvironmentType.Sea:
                if (!envirs.Contains(EnvironmentType.Beach) &&
                  !envirs.Contains(EnvironmentType.RiverBeach)) continue;
                break;
              default:
                if(!envirs.Contains(_questevent.EnvironmentType)) continue; 
                break;
            }
          }
          if (_questevent.Sector != SectorTypeEnum.NULL)
          {
            switch (_questevent.Sector)
            {
              case SectorTypeEnum.Residence:
                if (settletype != SettlementType.Village) continue;
                break;
              case SectorTypeEnum.Temple:
                if(settletype==SettlementType.City) continue;
                break;
              case SectorTypeEnum.Marketplace:
                if (settletype == SettlementType.Village) continue;
                break;
              case SectorTypeEnum.Library:
                if (settletype != SettlementType.City) continue;
                break;
            }
          }

          if (GameManager.Instance.MyGameData.IsAbleEvent(_questevent.ID) == false) { _disableevents.Add(_questevent); }
          else { _ableevents.Add(_questevent); }
        }
        break;
    }
    foreach (var _event in AllEvent)
    {
      if (GameManager.Instance.MyGameData.IsAbleEvent(_event.ID)==false) continue;
      if (GameManager.Instance.MyGameData.CurrentEventLine != "" &&
   GameManager.Instance.MyGameData.CurrentEventLine != _event.EventLine!) continue;
      if (_event.RightSpace(settletype, _event.Sector) == false) continue;
      if (_event.EnvironmentType != EnvironmentType.NULL)
      {
        switch (_event.EnvironmentType)
        {
          case EnvironmentType.Sea:
            if (!envirs.Contains(EnvironmentType.Beach) &&
              !envirs.Contains(EnvironmentType.RiverBeach)) continue;
            break;
          default:
            if (!envirs.Contains(_event.EnvironmentType)) continue;
            break;
        }
      }
      if (_event.Sector != SectorTypeEnum.NULL)
      {
        switch (_event.Sector)
        {
          case SectorTypeEnum.Residence:
            if (settletype != SettlementType.Village) continue;
            break;
          case SectorTypeEnum.Temple:
            if (settletype == SettlementType.City) continue;
            break;
          case SectorTypeEnum.Marketplace:
            if (settletype == SettlementType.Village) continue;
            break;
          case SectorTypeEnum.Library:
            if (settletype != SettlementType.City) continue;
            break;
        }
      }

      switch (_event.EventType)
      {
        case EventTypeEnum.Default:
          if (GameManager.Instance.MyGameData.IsAbleEvent(_event.ID) == false) { _disableevents.Add(_event); }
          else { _ableevents.Add(_event); }
          break;
        case EventTypeEnum.Follow:
          switch (_event.FollowType)
          {
            case FollowTypeEnum.Event:  //이벤트 연계일 경우 
              List<List<string>> _checktarget = new List<List<string>>();
              switch (_event.FollowTargetSuccess)
              {
                case 0:
                  switch (_event.FollowTendency)
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
                case 1:
                  switch (_event.FollowTendency)
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
                case 2:
                  switch (_event.FollowTendency)
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
                if (_list.Contains(_event.FollowID))
                {
                  if (GameManager.Instance.MyGameData.IsAbleEvent(_event.ID) == false) { _disableevents.Add(_event); }
                  else { _ableevents.Add(_event); }
                  break;
                }
              break;
            case FollowTypeEnum.Exp://테마 연계일 경우 현재 테마의 레벨이 기준 이상인지 확인
              if ((GameManager.Instance.MyGameData.LongExp != null && GameManager.Instance.MyGameData.LongExp.ID == _event.FollowID)
                || (GameManager.Instance.MyGameData.ShortExp_A != null && GameManager.Instance.MyGameData.ShortExp_A.ID == _event.FollowID)
                || (GameManager.Instance.MyGameData.ShortExp_B != null && GameManager.Instance.MyGameData.ShortExp_B.ID == _event.FollowID))
                if (GameManager.Instance.MyGameData.IsAbleEvent(_event.ID) == false) { _disableevents.Add(_event); }
                else { _ableevents.Add(_event); }
              break;
          }
          break;
      }
    }


    List<EventData> _targetevents = _ableevents.Count > 0 ? _ableevents : _disableevents;
    
    List<string> _eventlist = new List<string>();
    for (int i = 0; i < _targetevents.Count; i++)
    {
      int _count = 0;
      EventData _event = _targetevents[i];
      switch (_event.EventType)
      {
        case EventTypeEnum.Default: _count += ConstValues.EventPer_Normal; break;
        case EventTypeEnum.Follow: _count += _event.FollowType == FollowTypeEnum.Event ? ConstValues.EventPer_Follow_Ev : ConstValues.EventPer_Follow_Ex; break;
        case EventTypeEnum.Cult: _count += ConstValues.EventPer_Quest; break;
      }

      if (envirs.Contains(_event.EnvironmentType)) _count *= ConstValues.EventPer_Envir;
      else _count *= ConstValues.EventPer_NoEnvir;

      if (_event.Sector == sectortype) _count *= ConstValues.EventPer_Sector;
      else _count *= ConstValues.EventPer_NoSector;


      for (int j = 0; j < _count; j++) _eventlist.Add(_event.ID);
    }

    string _resultid = _eventlist[UnityEngine.Random.Range(0, _eventlist.Count)];
    foreach (EventData _event in _targetevents)
      if (_event.ID == _resultid) return _event;

    Debug.Log("여기까지 올 리가 없는디");
    return null;
  }
}
public class TileInfoData
{
  public LandmarkType Landmark = LandmarkType.Outer;
  public Settlement Settlement=null; //정착지 타입
  public List<EnvironmentType> EnvirList = new List<EnvironmentType>();//주위 환경 타입
  
}
#region 이벤트 정보에 쓰는 배열들
public enum FollowTypeEnum { Event,Exp}
public enum SettlementType {Village,Town,City,Outer}
public enum EventAppearType { Outer, Village, Town, City, Settlement}
public enum SectorTypeEnum {NULL, Residence, Temple,Marketplace, Library}
//,Theater,Academy
public enum EnvironmentType { NULL, River,Forest,Mountain,Sea,Beach,Land,RiverBeach, Highland }
public enum SelectionTypeEnum { Single,Body, Head}// (Vertical)Body : 좌 이성 우 육체    (Horizontal)Head : 좌 정신 우 물질    
public enum PenaltyTarget { None,Status,EXP }
public enum RewardTypeEnum {None, Status,Skill, Experience }
public enum EventSequence { Progress,Clear}
public enum EventTypeEnum { Default,Follow,Cult}
#endregion  
public class EventData
{
  public EventTypeEnum EventType = EventTypeEnum.Default;

  public FollowTypeEnum FollowType = 0;
  public string FollowID = "";
  /// <summary>
  /// 0:성공 1:실패 2:노상관
  /// </summary>
  public int FollowTargetSuccess = 0;
  /// <summary>
  /// 0:왼쪽 1:오른쪽 2:노상관
  /// </summary>
  public int FollowTendency = 0;

  public string EndingID = "";
  public string EventLine = "";

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
     return  GameManager.Instance.ImageHolder.GetEventIllusts(ID,"Beginning",BeginningLength );
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
  public EventAppearType AppearSpace;
  public bool RightSpace(SettlementType currentsettle,SectorTypeEnum eventsector)
  {
    switch (AppearSpace)
    {
      case EventAppearType.Outer:
        return false;
      case EventAppearType.Village:
        if (currentsettle == SettlementType.Village)
        {
          if (eventsector == SectorTypeEnum.Marketplace || eventsector == SectorTypeEnum.Library) return false;
          return true;
        }
        break;
      case EventAppearType.Town:
        if (currentsettle == SettlementType.Town)
        {
          if (eventsector == SectorTypeEnum.Residence || eventsector == SectorTypeEnum.Library) return false;
          return true;
        }
        break;
      case EventAppearType.City:
        if (currentsettle == SettlementType.City)
        {
          if (eventsector == SectorTypeEnum.Residence || eventsector == SectorTypeEnum.Temple) return false;
          return true;
        }
        break;
      case EventAppearType.Settlement:
        if (currentsettle == SettlementType.Outer)
        {
          return false;
        }
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
  public bool StopEvent = false;

  private EventData MyEvent = null;
  public TendencyTypeEnum Tendencytype = TendencyTypeEnum.None;
  /// <summary>
  /// 0,1
  /// </summary>
  public int Index = 0;
  /// <summary>
  /// 성향 없는 경우
  /// </summary>
  /// <param name="myevent"></param>
  public SelectionData(EventData myevent)
  {
    MyEvent = myevent;
    Tendencytype = TendencyTypeEnum.None;
    Index = 0;
  }
  public SelectionData(EventData myevent,TendencyTypeEnum tendencytype, int index)
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
  public FailData FailData = null;
}    

public class FailData
{
  private EventData MyEvent = null;
  public int Index = 0;
  public TendencyTypeEnum Tendencytype = TendencyTypeEnum.None;
  /// <summary>
  /// 성향 없는 경우
  /// </summary>
  /// <param name="myevent"></param>
  public FailData(EventData myevent)
  {
    MyEvent = myevent; Index = 0; Tendencytype = TendencyTypeEnum.None;
  }
  public FailData(EventData myevent,TendencyTypeEnum tendencytype, int index)
  {
    MyEvent = myevent; Index = index; Tendencytype = tendencytype;
  }
  private string OriginID { get { return MyEvent.ID; } }
  private string TypeID { get { return  (Tendencytype == TendencyTypeEnum.None ? "" : Index == 0 ? "L" : "R") + "Fail"; } }
  private List<string> descriptions=new List<string>();
  public List<string> Descriptions
  {
    get
    {
      if (descriptions.Count == 0)
      {
        List<string> _temp = GameManager.Instance.GetTextData(OriginID+"_"+TypeID+"_Descriptions").Split('@').ToList();
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
  public PenaltyTarget Penelty_target;
  public StatusTypeEnum StatusType= StatusTypeEnum.HP;
  public string ExpID;
}
public class GoldFailData : FailData
{
  public GoldFailData() : base(null,TendencyTypeEnum.None, -1) { }
  public string Description = "";
  public Sprite Illust = null;
}
public enum SelectionTargetType { None,Pay, Check_Single,Check_Multy}//선택지 개별 내용
public enum StatusTypeEnum { HP,Sanity,Gold,Supply}
public class SuccessData
{
  public TendencyTypeEnum Tendencytype = TendencyTypeEnum.None;
  private EventData MyEvent = null;
  public int Index = 0;
  /// <summary>
  /// 성향 없는 경우
  /// </summary>
  /// <param name="myevent"></param>
  public SuccessData(EventData myevent,TendencyTypeEnum tendencytype, int index)
  {
    MyEvent = myevent; Index = index; Tendencytype = tendencytype;
  }
  private string OriginID { get { return MyEvent.ID; } }
  private string TypeID { get { return  (Tendencytype == TendencyTypeEnum.None ? "" : Index == 0 ? "L" : "R") + "Success"; } }
  private List<string> descriptions= new List<string>();
  public List<string> Descriptions
  {
    get
    {
      if (descriptions.Count == 0)
      {
        List<string> _temp = GameManager.Instance.GetTextData(OriginID + "_" + TypeID + "_Descriptions").Split('@').ToList();
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

  public List<EventData> Events_Cult_0to30 = new List<EventData>(); 
  public List<EventData> Events_Cult_30to60 = new List<EventData>();
  public List<EventData> Events_Cult_60to100 = new List<EventData>();

  /// <summary>
  /// Phase에 맞춰 사용 가능한 이벤트들을 반환(0,1,2)
  /// </summary>
  /// <returns></returns>
  public List<EventData> GetAvailableEvents()
  {
    List<List<EventData>> _availablelists = new List<List<EventData>>();

    _availablelists.Add(Events_Cult_0to30);
    if (GameManager.Instance.MyGameData.Quest_Cult_Phase>0)
      _availablelists.Add(Events_Cult_30to60);
    if (GameManager.Instance.MyGameData.Quest_Cult_Phase > 1)
      _availablelists.Add(Events_Cult_60to100);

    List<EventData> _availableevents=new List<EventData>();
    foreach(var list in _availablelists)
      foreach(var _event in list) _availableevents.Add(_event);

    return _availableevents;
  }

  public Tuple<Sprite,string> GetSettlementData(SettlementType type)
  {
    return new Tuple<Sprite, string>
      (
      GameManager.Instance.ImageHolder.GetCultSettlementIllust(type),
      WNCText.GetSeasonText(GameManager.Instance.GetTextData("Cult_"+type.ToString()+"_description"))      );
  }
  public Tuple<Sprite, string> GetSabbatData
  {
    get
    {
      int _count = GameManager.Instance.ImageHolder.Cult_Sabbat.Count;
      int _index = UnityEngine.Random.Range(0, _count);

      if (GameManager.Instance.MyGameData.Cult_Progress_SabbatEventIndex.Count == _count)
      {
        return new Tuple<Sprite, string>
          (
          GameManager.Instance.ImageHolder.Cult_Sabbat[0],
          WNCText.GetSeasonText(GameManager.Instance.GetTextData("Cult_Sabbat_0_description")));
      }


      while (GameManager.Instance.MyGameData.Cult_Progress_SabbatEventIndex.Contains(_index)) _index = UnityEngine.Random.Range(0, _count);

      Sprite _illust = GameManager.Instance.ImageHolder.Cult_Sabbat[_index];
      string _filename = "Cult_Sabbat_" + _index.ToString();
      GameManager.Instance.MyGameData.Cult_Progress_SabbatEventIndex.Add(_index);
      return new Tuple<Sprite, string>(
        _illust,
        WNCText.GetSeasonText(GameManager.Instance.GetTextData(_filename + "_description")));    }
  }
  public Tuple<Sprite, string> GetRitualData
  {
    get
    {
      int _count = GameManager.Instance.ImageHolder.Cult_Ritual.Count;
      int _index = UnityEngine.Random.Range(0, _count);

      if (GameManager.Instance.MyGameData.Cult_Progress_RitualEventIndex.Count == _count)
      {
        return new Tuple<Sprite, string>
          (
          GameManager.Instance.ImageHolder.Cult_Ritual[0],
          WNCText.GetSeasonText(GameManager.Instance.GetTextData("Cult_Ritual_0_description"))          );
      }

      while (GameManager.Instance.MyGameData.Cult_Progress_RitualEventIndex.Contains(_index)) _index = UnityEngine.Random.Range(0, _count);

      Sprite _illust = GameManager.Instance.ImageHolder.Cult_Ritual[_index ];
      string _filename = "Cult_Ritual_" + _index.ToString();
      GameManager.Instance.MyGameData.Cult_Progress_RitualEventIndex.Add(_index);
      return new Tuple<Sprite, string>(
        _illust,
        WNCText.GetSeasonText(GameManager.Instance.GetTextData(_filename + "_description")));
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

  public string Selection_Type;           //0.단일 1.이성+육체 2.정신+물질 
  public string Selection_Target;           //0.지불 1.기술(1) 2.기술(2)
  public string Selection_Info;             //0:체,정,골   1:기술1개  2:기술@기술

  public string Failure_Penalty;            //없음, 손실
  public string Failure_Penalty_info;       //0:X   1:체력,정신력,돈

  public string Reward_Target;              //없음, 스텟, 기술, 경험
  public string Reward_Info;                //  ID                         대화,무력,생존,학식

  public string EndingID = "";
  public string EventLine = "";
}


