using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using static UnityEditor.Progress;

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
    Data.Sector = (SectorType)int.Parse(_placeinfos[1]);
    Data.EnvironmentType = (EnvironmentType)int.Parse(_placeinfos[2]);

    if (_data.Selection_Type != "")
    {
      Data.Selection_type = (SelectionType)int.Parse(_data.Selection_Type);
      switch (Data.Selection_type)//����,�̼���ü,���Ź���,��Ÿ ��� �з����� ������,����,���� ������ �����
      {
        case SelectionType.Single://���� ������
          Data.SelectionDatas = new SelectionData[1];
          Data.SelectionDatas[0] = new SelectionData(Data);

          if (_data.Selection_Target != "")
          {
            Data.SelectionDatas[0].ThisSelectionType = (SelectionTargetType)int.Parse(_data.Selection_Target);
            switch (Data.SelectionDatas[0].ThisSelectionType)
            {
              case SelectionTargetType.Pay: //����
                Data.SelectionDatas[0].SelectionPayTarget = (StatusType)int.Parse(_data.Selection_Info);
                break;
              case SelectionTargetType.Check_Single: //���(����)
                Data.SelectionDatas[0].SelectionCheckSkill.Add((SkillType)int.Parse(_data.Selection_Info));
                break;
              case SelectionTargetType.Check_Multy: //���(����)
                string[] _temp = _data.Selection_Info.Split(',');
                for (int i = 0; i < _temp.Length; i++) Data.SelectionDatas[0].SelectionCheckSkill.Add((SkillType)int.Parse(_temp[i]));
                break;
            }
            if (Data.SelectionDatas[0].ThisSelectionType.Equals(SelectionTargetType.Check_Single) ||
              Data.SelectionDatas[0].ThisSelectionType.Equals(SelectionTargetType.Check_Multy))
            {
              Data.FailureDatas = new FailureData[1];
              Data.FailureDatas[0] = new FailureData(Data);
              Data.FailureDatas[0].Panelty_target = (PenaltyTarget)int.Parse(_data.Failure_Penalty);
              switch (Data.FailureDatas[0].Panelty_target)
              {
                case PenaltyTarget.None: break;
                case PenaltyTarget.Status: Data.FailureDatas[0].Loss_target = (StatusType)int.Parse(_data.Failure_Penalty_info); break;
                case PenaltyTarget.EXP: Data.FailureDatas[0].ExpID = _data.Failure_Penalty_info; break;
              }
            }
            else if (Data.SelectionDatas[0].ThisSelectionType.Equals(SelectionTargetType.Pay) && Data.SelectionDatas[0].SelectionPayTarget.Equals(StatusType.Gold))
            {
              Data.FailureDatas = new FailureData[1]; Data.FailureDatas[0] = GameManager.Instance.GoldFailData;
            }
            Data.SuccessDatas = new SuccessData[1];
            Data.SuccessDatas[0] = new SuccessData(Data, TendencyTypeEnum.None,0);
            Data.SuccessDatas[0].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target);
            switch (Data.SuccessDatas[0].Reward_Target)
            {
              case RewardTarget.Experience: Data.SuccessDatas[0].Reward_EXPID = _data.Reward_Info; break;
              case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;
              case RewardTarget.Skill: Data.SuccessDatas[0].Reward_SkillType = (SkillType)int.Parse(_data.Reward_Info); break;
            }

            Data.SelectionDatas[0].SelectionSuccesReward=Data.SuccessDatas[0].Reward_Target;
            if (Data.SuccessDatas[0].Reward_Target == RewardTarget.Skill)
              Data.SelectionDatas[0].SelectionRewardSkillType = Data.SuccessDatas[0].Reward_SkillType;
          }

          break;
        case SelectionType.Body://�̼���ü
          maketendencydata(TendencyTypeEnum.Body);
          break;
        case SelectionType.Head://���Ź���
          maketendencydata(TendencyTypeEnum.Head);
          break;
      }
      void maketendencydata(TendencyTypeEnum tendencytype)
      {
        Data.SelectionDatas = new SelectionData[2];
        Data.FailureDatas = new FailureData[2];
        Data.SuccessDatas = new SuccessData[2];

        if (_data.Selection_Target != "")
        {
          for (int i = 0; i < Data.SelectionDatas.Length; i++)
          {
            Data.SelectionDatas[i] = new SelectionData(Data,tendencytype, i);

            Data.SelectionDatas[i].ThisSelectionType = (SelectionTargetType)int.Parse(_data.Selection_Target.Split('@')[i]);
            switch (Data.SelectionDatas[i].ThisSelectionType)
            {
              case SelectionTargetType.Pay: //����
                Data.SelectionDatas[i].SelectionPayTarget = (StatusType)int.Parse(_data.Selection_Info.Split('@')[i]);
                break;
              case SelectionTargetType.Check_Single: //���(����)
                Data.SelectionDatas[i].SelectionCheckSkill.Add((SkillType)int.Parse(_data.Selection_Info.Split('@')[i]));
                break;
              case SelectionTargetType.Check_Multy: //���(����)
                string[] _temp = _data.Selection_Info.Split('@')[i].Split(',');
                for (int j = 0; j < _temp.Length; j++) Data.SelectionDatas[i].SelectionCheckSkill.Add((SkillType)int.Parse(_temp[j]));
                break;
            }

            if (Data.SelectionDatas[i].ThisSelectionType.Equals(SelectionTargetType.Check_Single) ||
              Data.SelectionDatas[i].ThisSelectionType.Equals(SelectionTargetType.Check_Multy))
            {
              Data.FailureDatas[i] = new FailureData(Data,tendencytype, i);
              Data.FailureDatas[i].Panelty_target = (PenaltyTarget)int.Parse(_data.Failure_Penalty.Split('@')[i]);
              switch (Data.FailureDatas[i].Panelty_target)
              {
                case PenaltyTarget.None: break;
                case PenaltyTarget.Status: Data.FailureDatas[i].Loss_target = (StatusType)int.Parse(_data.Failure_Penalty_info.Split('@')[i]); break;
                case PenaltyTarget.EXP: Data.FailureDatas[i].ExpID = _data.Failure_Penalty_info.Split('@')[i]; break;
              }
            }
            else if (Data.SelectionDatas[i].ThisSelectionType.Equals(SelectionTargetType.Pay) && Data.SelectionDatas[i].SelectionPayTarget.Equals(StatusType.Gold))
            {
              Data.FailureDatas[i] = GameManager.Instance.GoldFailData;
            }
            Data.SuccessDatas[i] = new SuccessData(Data,tendencytype, i);
            Data.SuccessDatas[i].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target.Split('@')[i]);
            switch (Data.SuccessDatas[i].Reward_Target)
            {
              case RewardTarget.Experience: Data.SuccessDatas[i].Reward_EXPID = _data.Reward_Info.Split('@')[i]; break;
              case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;
              case RewardTarget.Skill: Data.SuccessDatas[i].Reward_SkillType = (SkillType)int.Parse(_data.Reward_Info.Split('@')[i]); break;
            }

            Data.SelectionDatas[i].SelectionSuccesReward=(Data.SuccessDatas[i].Reward_Target);
            if (Data.SuccessDatas[i].Reward_Target == RewardTarget.Skill)
              Data.SelectionDatas[i].SelectionRewardSkillType = Data.SuccessDatas[i].Reward_SkillType;
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

  public void ConvertData_Follow(FollowEventJsonData _data)
  {
    FollowEventData Data = ReturnEventDataDefault<FollowEventData>(_data);

    Data.FollowType = (FollowType)int.Parse(_data.FollowType); //���� ��� �̺�Ʈ,����,Ư��,�׸�,���
    Data.FollowTarget = _data.FollowTarget;         //���� ���- �̺�Ʈ,����,Ư���̸� Id   �׸��� 0,1,2,3  ����̸� 0~9
    if (Data.FollowType == FollowType.Event)
    {
      Data.FollowTargetSuccess = int.Parse(_data.FollowTargetSuccess) == 0 ? true : false;//���� ����� �̺�Ʈ�� ��� ���� Ȥ�� ����
      Data.FollowTendency = int.Parse(_data.FollowTendency);                              //���� ����� �̺�Ʈ�� ��� ������ ����
    }
    else if (Data.FollowType.Equals(FollowType.Skill))
      Data.FollowTargetLevel = int.Parse(_data.FollowTargetSuccess);

    if (_data.EndingName != "")
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
  public void ConvertData_Quest(QuestEventDataJson _data)
  {
    switch ((QuestType) int.Parse(_data.QuestType))
    {
      case QuestType.Cult:
        ConvertData_Quest_cult(_data);
        break;
    }
    
  }//����Ʈ ������ ��ȹ ������ �߰��ؾ���
  public void ConvertData_Quest_cult(QuestEventDataJson jsondata)
  {
    QuestEventData_Wolf eventdata = ReturnEventDataDefault<QuestEventData_Wolf>(jsondata);
    eventdata.Type = QuestType.Cult;
    switch (int.Parse(jsondata.QuestEventType))
    {
      case 0:
        Quest_Cult.Events_Cult_Less.Add(eventdata);
        break;
      case 1:
        Quest_Cult.Events_Cult_Over.Add(eventdata);
        break;
    }
  }
  /// <summary>
  /// 0:���� 1:���� 2:��ü 3:���� 4:����
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
  /// �ܺ� �̺�Ʈ ��ȯ
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
        case FollowType.Event:  //�̺�Ʈ ������ ��� 
          List<string> _checktarget = new List<string>();
          if (_follow.FollowTargetSuccess == true)
          {
            switch (_follow.FollowTendency)
            {
              case 0: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_None; break;
              case 1: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_Rational; break;
              case 2: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_Physical; break;
              case 3: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_Mental; break;
              case 4: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_Material; break;
              case 5: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_All; break;
            }
          }
          else
          {
            switch (_follow.FollowTendency)
            {
              case 0: _checktarget = GameManager.Instance.MyGameData.FailEvent_None; break;
              case 1: _checktarget = GameManager.Instance.MyGameData.FailEvent_Rational; break;
              case 2: _checktarget = GameManager.Instance.MyGameData.FailEvent_Physical; break;
              case 3: _checktarget = GameManager.Instance.MyGameData.FailEvent_Mental; break;
              case 4: _checktarget = GameManager.Instance.MyGameData.FailEvent_Material; break;
              case 5: _checktarget = GameManager.Instance.MyGameData.FailEvent_All; break;
            }
          }
          if (_checktarget.Contains(_follow.FollowTarget)) _allevents.Add(_follow);
          break;
        case FollowType.EXP://���� ������ ��� ���� ������ ���� ID�� �´��� Ȯ��
          if (_follow.FollowTarget.Equals(GameManager.Instance.MyGameData.LongTermEXP.ID)) _allevents.Add(_follow);
          foreach (var _data in GameManager.Instance.MyGameData.ShortTermEXP)
            if (_follow.FollowTarget.Equals(_data.ID)) _allevents.Add(_follow);
          break;
        case FollowType.Skill://�׸� ������ ��� ���� �׸��� ������ ���� �̻����� Ȯ��
          int _targetlevel = 0;
          SkillType _type = SkillType.Conversation; ;
          switch (_follow.FollowTarget)
          {
            case "0"://��ȭ �׸�
              _type = SkillType.Conversation; break;
            case "1"://���� �׸�
              _type = SkillType.Force; break;
            case "2"://���� �׸�
              _type = SkillType.Wild; break;
            case "3"://�н� �׸�
              _type = SkillType.Intelligence; break;
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
    }//�ش� ����� ������ �Ϲ� �̺�Ʈ ����Ʈ

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
      if (Random.Range(0, ConstValues.EventPer_Envir + ConstValues.EventPer_NoEnvir) < ConstValues.EventPer_Envir)
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
      return _result[Random.Range(0, _result.Count)];
    }
  }
  /// <summary>
  /// �������� ��� �̺�Ʈ ��ȯ
  /// </summary>
  /// <param name="settletype"></param>
  /// <param name="placetype"></param>
  /// <param name="placelevel"></param>
  /// <param name="envir"></param>
  /// <returns></returns>
  public EventDataDefulat ReturnPlaceEvent(SettlementType settletype, SectorType sectortype, List<EnvironmentType> envirs)
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
        case FollowType.Event:  //�̺�Ʈ ������ ��� 
          List<string> _checktarget = new List<string>();
          if (_follow.FollowTargetSuccess == true)
          {
            switch (_follow.FollowTendency)
            {
              case 0: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_None; break;
              case 1: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_Rational; break;
              case 2: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_Physical; break;
              case 3: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_Mental; break;
              case 4: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_Material; break;
              case 5: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_All; break;
            }
          }
          else
          {
            switch (_follow.FollowTendency)
            {
              case 0: _checktarget = GameManager.Instance.MyGameData.FailEvent_None; break;
              case 1: _checktarget = GameManager.Instance.MyGameData.FailEvent_Rational; break;
              case 2: _checktarget = GameManager.Instance.MyGameData.FailEvent_Physical; break;
              case 3: _checktarget = GameManager.Instance.MyGameData.FailEvent_Mental; break;
              case 4: _checktarget = GameManager.Instance.MyGameData.FailEvent_Material; break;
              case 5: _checktarget = GameManager.Instance.MyGameData.FailEvent_All; break;
            }
          }
          if (_checktarget.Contains(_follow.FollowTarget)) _allevents.Add(_follow);
          break;
        case FollowType.EXP://���� ������ ��� ���� ������ ���� ID�� �´��� Ȯ��
          if (_follow.FollowTarget.Equals(GameManager.Instance.MyGameData.LongTermEXP.ID)) _allevents.Add(_follow);
          foreach (var _data in GameManager.Instance.MyGameData.ShortTermEXP)
            if (_follow.FollowTarget.Equals(_data.ID)) _allevents.Add(_follow);
          break;
        case FollowType.Skill://�׸� ������ ��� ���� �׸��� ������ ���� �̻����� Ȯ��
          int _targetlevel = 0;
          SkillType _type = SkillType.Conversation; ;
          switch (_follow.FollowTarget)
          {
            case "0"://��ȭ �׸�
              _type = SkillType.Conversation; break;
            case "1"://���� �׸�
              _type = SkillType.Force; break;
            case "2"://���� �׸�
              _type = SkillType.Wild; break;
            case "3"://�н� �׸�
              _type = SkillType.Intelligence; break;
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
    }//�ش� ����� ������ �Ϲ� �̺�Ʈ ����Ʈ

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
      if(Random.Range(0,ConstValues.EventPer_Envir+ConstValues.EventPer_NoEnvir)<ConstValues.EventPer_Envir)
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
      if (Random.Range(0, ConstValues.EventPer_Sector + ConstValues.EventPer_NoSector) < ConstValues.EventPer_Sector)
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
      return _result[Random.Range(0,_result.Count)];
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
    _max = Random.Range(0, _max);
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
  public Settlement Settlement=null; //������ Ÿ��
  public List<EnvironmentType> EnvirList = new List<EnvironmentType>();//���� ȯ�� Ÿ��
  
}
#region �̺�Ʈ ������ ���� �迭��
public enum FollowType { Event,EXP,Skill}
public enum SettlementType {Village,Town,City,Outer}
public enum EventAppearType { Outer, Village, Town, City, Settlement}
public enum SectorType {NULL, Residence, Temple,Marketplace, Library,Theater,Academy}
public enum EnvironmentType { NULL, River,Forest,Mountain,Sea,Beach,Land,RiverBeach, Highland }
public enum SelectionType { Single,Body, Head,Tendency,Experience }// (Vertical)Body : �� �̼� �� ��ü    (Horizontal)Head : �� ���� �� ����    
public enum PenaltyTarget { None,Status,EXP }
public enum RewardTarget { Experience,HP,Sanity,Gold,Skill,None}
public enum EventSequence { Progress,Clear}//Suggest: 3�� �����ϴ� �ܰ�  Progress: ������ ��ư ������ �ϴ� �ܰ�  Clear: ���� �����ؾ� �ϴ� �ܰ�
#endregion  
public class EventDataDefulat
{
  /// <summary>
  /// "Event_�̸�"
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
  public List<int> EnableSeasons = new List<int>();  //0���� ����� ��ȭ ����
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

  public SectorType Sector = SectorType.NULL;
  public EnvironmentType EnvironmentType = EnvironmentType.NULL;

  public SelectionType Selection_type;
  public SelectionData[] SelectionDatas;

  public FailureData[] FailureDatas;

  public SuccessData[] SuccessDatas;
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
  /// ���� ���� ���
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
    get { return WNCText.GetSeasonText(GameManager.Instance.GetTextData(ID + "_Selecting_Names").Split('@')[Index]); }
  }
/*  public string SubDescription
  {
    get { return WNCText.GetSeasonText(GameManager.Instance.GetTextData(ID + "_Selecting_Subdescriptions").Split('@')[Index]); }
  }
*/
  public SelectionTargetType ThisSelectionType = SelectionTargetType.Pay;

  public StatusType SelectionPayTarget = StatusType.HP;                       //Pay�϶� ���
  public List<SkillType> SelectionCheckSkill = new List<SkillType>();         //Check_Single,Check_Multy�϶� ���
  public RewardTarget SelectionSuccesReward= RewardTarget.None;
  public SkillType SelectionRewardSkillType = SkillType.Conversation;
}    

public class FailureData
{
  private EventDataDefulat MyEvent = null;
  public int Index = 0;
  public TendencyTypeEnum Tendencytype = TendencyTypeEnum.None;
  /// <summary>
  /// ���� ���� ���
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
  public StatusType Loss_target= StatusType.HP;
  public string ExpID;
}
public class GoldFailData : FailureData
{
  public GoldFailData() : base(null,TendencyTypeEnum.None, -1) { }
  public string Description = "";
  public Sprite Illust = null;
}
public enum SelectionTargetType { Pay, Check_Single,Check_Multy}//������ ���� ����
public enum StatusType { HP,Sanity,Gold}
public class SuccessData
{
  public TendencyTypeEnum Tendencytype = TendencyTypeEnum.None;
  private EventDataDefulat MyEvent = null;
  public int Index = 0;
  /// <summary>
  /// ���� ���� ���
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
  public RewardTarget Reward_Target;

  public SkillType Reward_SkillType;
  public string Reward_EXPID;
}

public class EventData:EventDataDefulat
{
}//�⺻ �̺�Ʈ
public class FollowEventData:EventDataDefulat
{
  public FollowType FollowType = 0;
  public string FollowTarget = "";
  public int FollowTargetLevel = 0;
  public bool FollowTargetSuccess = false;
  public int FollowTendency = 0;          //�̺�Ʈ�� ��� ��Ÿ,�̼�,��ü,����,���� ������ ����

  public FollowEndingData EndingData = null;
}//���� �̺�Ʈ
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
    public Sprite QuestIllust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(Type, OriginID + "_MainIllust"); } }
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
  public int Progress = 0;

  public TileData[] RitualPlaces = new TileData[3];

  #region ���ѷα� ����
  public Sprite Prologue_0_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, OriginID + "_Prologue_0_Illust"); } }
  public string Prologue_0_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_0_Description"); } }

  public Sprite Prologue_1_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, OriginID + "_Prologue_1_Illust"); } }
  public string Prologue_1_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_1_Description"); } }
  public string Prologue_1_Selection_0 { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_1_Selection_0"); } }
  public string Prologue_1_Selection_1 { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_1_Selection_1"); } }
 
  public Sprite Prologue_2_0_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, OriginID + "_Prologue_2_0_Illust"); } }
  public string Prologue_2_0_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_2_0_Description"); } }

  public Sprite Prologue_2_1_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, OriginID + "_Prologue_2_1_Illust"); } }
  public string Prologue_2_1_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_2_1_Description"); } }
 
  public Sprite Prologue_3_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, OriginID + "_Prologue_3_Illust"); } }
  public string Prologue_3_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_3_Description"); } }
  public string Prologue_3_Selection_0 { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_3_Selection_0"); } }
  public string Prologue_3_Selection_1 { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_3_Selection_1"); } }

  public Sprite Prologue_4_0_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, OriginID + "_Prologue_4_0_Illust"); } }
  public string Prologue_4_0_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_4_0_Description"); } }
  public Sprite Prologue_4_1_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, OriginID + "_Prologue_4_1_Illust"); } }
  public string Prologue_4_1_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_4_1_Description"); } }

  public Sprite Prologue_5_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, OriginID + "_Prologue_5_Illust"); } }
  public string Prologue_5_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_5_Description"); } }
  public Sprite Prologue_6_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, OriginID + "_Prologue_6_Illust"); } }
  public string Prologue_6_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_6_Description"); } }
  public Sprite Prologue_7_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, OriginID + "_Prologue_7_Illust"); } }
  public string Prologue_7_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_7_Description"); } }
  public Sprite Prologue_8_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, OriginID + "_Prologue_8_Illust"); } }
  public string Prologue_8_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_8_Description"); } }
  #endregion

  #region Ž��
  public Sprite Searching_0_Illust
  {
    get {
      string _seasontext = "";
      switch (GameManager.Instance.MyGameData.Turn)
      {
        case 0:_seasontext = "_spring";break;
        case 1:_seasontext = "_summer";break;
        case 2: _seasontext = "_autumn"; break;
        case 3: _seasontext = "_winter"; break;
      }
      return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, OriginID + "_Searching_0_Illust"+ _seasontext); }
  }
  public string Searching_0_Description
  {
    get { return GameManager.Instance.GetTextData(OriginID + "_Searching_0_Description"); }
  }
  public Sprite Searching_1_Illust
  {
    get {
      string _seasontext = "";
      switch (GameManager.Instance.MyGameData.Turn)
      {
        case 0: _seasontext = "_spring"; break;
        case 1: _seasontext = "_summer"; break;
        case 2: _seasontext = "_autumn"; break;
        case 3: _seasontext = "_winter"; break;
      }
          return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, OriginID + "_Searching_1_Illust"+_seasontext); }
  }
  public string Searching_1_Description
  {
    get { return GameManager.Instance.GetTextData(OriginID + "_Searching_1_Description"); }
  }
  public Sprite SearchingToProgress_Illust
  {
    get
    {
      return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, OriginID + "_SearchingToProgress");
    }
  }
  public string SearchingToProgress_Description
  {
    get { return GameManager.Instance.GetTextData(OriginID + "_SearchingToProgress_Description"); }
  }

#endregion
  public Sprite SearchingWarning_Illust
  {
    get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, OriginID + "_SearchingWarning"); }
  }
  public string SearchingWarning_Description
  {
    get { return GameManager.Instance.GetTextData(OriginID + "_SearchingWarning_Description"); }
  }

#region ����
public string Wanted_Description { get { return GameManager.Instance.GetTextData("Quest0_Wanted_Description"); } }
  public string Wanted_Description_Sabbat { get { return GameManager.Instance.GetTextData("Quest0_Wanted_Sabbat"); } }
  public string Wanted_Description_Ritual { get { return GameManager.Instance.GetTextData("Quest0_Wanted_Ritual"); } }

  public Sprite Wanted_Sabbat_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, "Quest0_Wanted_Sabbat_Illust"); } }
  public string Wanted_Sabbat_Description { get { return GameManager.Instance.GetTextData("Quest0_WantedResult_Sabbat_Description"); } }

  public Sprite Wanted_Ritual_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, "Quest0_Wanted_Ritual_Illust"); } }
  public string Wanted_Ritual_Description { get { return GameManager.Instance.GetTextData("Quest0_WantedResult_Ritual_Description"); } }
  #endregion

  #region ��ȸ �ػ�
  public Sprite Sabbat_0_Illust_Idle { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, "Quest0_Sabbat_0_Idle"); } }
  public Sprite Sabbat_0_Illust_Tendency { get { string _tendency = "";
      switch (GameManager.Instance.MyGameData.Tendency_Body.Level)
      {
        case -2:_tendency = "m2";break;
        case -1: _tendency = "m1"; break;
        case 1: _tendency = "p1"; break;
        case 2: _tendency = "p2"; break;
      }
      return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, "Quest0_Sabbat_0_Tendency_" + _tendency);
    } }
  public string Sabbat_0_Description_Idle { get { return GameManager.Instance.GetTextData("Quest0_Sabbat_0_Idle"); } }
  public string Sabbat_0_Description_Tendency
  {
    get
    {
      string _tendency = "";
      switch (GameManager.Instance.MyGameData.Tendency_Body.Level)
      {
        case -2: _tendency = "m2"; break;
        case -1: _tendency = "m1"; break;
        case 1: _tendency = "p1"; break;
        case 2: _tendency = "p2"; break;
      }
      return GameManager.Instance.GetTextData("Quest0_Sabbat_0_Tendency_" + _tendency);
    }
  }
  public Sprite Sabbat_1_Illust_Idle { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, "Quest1_Sabbat_1_Idle"); } }
  public Sprite Sabbat_1_Illust_Tendency
  {
    get
    {
      string _tendency = "";
      switch (GameManager.Instance.MyGameData.Tendency_Body.Level)
      {
        case -2: _tendency = "m2"; break;
        case -1: _tendency = "m1"; break;
        case 1: _tendency = "p1"; break;
        case 2: _tendency = "p2"; break;
      }
      return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Cult, "Quest1_Sabbat_1_Tendency_" + _tendency);
    }
  }
  public string Sabbat_1_Description_Idle { get { return GameManager.Instance.GetTextData("Quest1_Sabbat_1_Idle"); } }
  public string Sabbat_1_Description_Tendency
  {
    get
    {
      string _tendency = "";
      switch (GameManager.Instance.MyGameData.Tendency_Body.Level)
      {
        case -2: _tendency = "m2"; break;
        case -1: _tendency = "m1"; break;
        case 1: _tendency = "p1"; break;
        case 2: _tendency = "p2"; break;
      }
      return GameManager.Instance.GetTextData("Quest1_Sabbat_1_Tendency_" + _tendency);
    }
  }
  #endregion
  public List<QuestEventData_Wolf> Events_Cult_Less = new List<QuestEventData_Wolf>(); //QuestEventType 7
  public List<QuestEventData_Wolf> Events_Cult_Over = new List<QuestEventData_Wolf>();  //QuestEventType 8

//  public List<QuestEventData_Wolf> Events_Sabbat_0 = new List<QuestEventData_Wolf>();     //QuestEventType 9
 // public QuestEventData_Wolf Event_Sabbat_Hideout_0 = null;                          //QuestEventType 10
 // public List<QuestEventData_Wolf> Events_Sabbat_1 = new List<QuestEventData_Wolf>();     //QuestEventType 11
  //public QuestEventData_Wolf Event_Sabbat_Hideout_1 = null;                          //QuestEventType 12
 // public List<QuestEventData_Wolf> Events_Sabbat_2 = new List<QuestEventData_Wolf>();     //QuestEventType 13
//  public QuestEventData_Wolf Event_Sabbat_Hideout_2 = null;                          //QuestEventType 14
 // public List<QuestEventData_Wolf> Events_Sabbat_Final = new List<QuestEventData_Wolf>(); //QuestEventType 15
 // public QuestEventData_Wolf Event_Sabbat_Hideout_Final = null;                      //QuestEventType 16

//  public List<QuestEventData_Wolf> Events_Ritual_0 = new List<QuestEventData_Wolf>();     //QuestEventType 17
//  public QuestEventData_Wolf Event_Ritual_Encounter_0 = null;                        //QuestEventType 18
//  public List<QuestEventData_Wolf> Events_Ritual_1 = new List<QuestEventData_Wolf>();     //QuestEventType 19
//  public QuestEventData_Wolf Event_Ritual_Encounter_1 = null;                        //QuestEventType 20
 // public List<QuestEventData_Wolf> Events_Ritual_2 = new List<QuestEventData_Wolf>();     //QuestEventType 21
 // public QuestEventData_Wolf Event_Ritual_Encounter_2 = null;                        //QuestEventType 22
 // public List<QuestEventData_Wolf> Events_Ritual_Final = new List<QuestEventData_Wolf>(); //QuestEventType 23
//  public QuestEventData_Wolf Event_Ritual_Encounter_Final = null;                    //QuestEventType 24

  /// <summary>
  /// Phase�� ���� ��� ������ �̺�Ʈ���� ��ȯ(1,2,3,4)
  /// </summary>
  /// <returns></returns>
  public List<EventDataDefulat> GetAvailableEvents()
  {
    List<List<QuestEventData_Wolf>> _availablelists = new List<List<QuestEventData_Wolf>>();

    _availablelists.Add(Events_Cult_Less);
    if (GameManager.Instance.MyGameData.Quest_Cult_Progress > 50)
      _availablelists.Add(Events_Cult_Over);

    List<EventDataDefulat> _availableevents=new List<EventDataDefulat>();
    foreach(var list in _availablelists)
      foreach(var _event in list) _availableevents.Add(_event);

    return _availableevents;
  }
}//                                         ����Ʈ ������ �� ����
public class EventJsonData
{
  public string ID = "";              //ID
  public string PlaceInfo = "";          //0,1,2,3
  public string Season ;              //����,��,����,����,�ܿ�

  public string Selection_Type;           //0.���� 1.�̼�+��ü 2.����+���� 3.���� 4.���� 5.���
  public string Selection_Target;           //0.������ 1.���� 2.�׸� 3.���
  public string Selection_Info;             //0:���� ����  1:ü��,���ŷ�,��
                                            //2:��ȭ,����,����,����
                                            //3: 0.���� 1.����  2.�⸸  3.�� 4.���� 5.Ȱ�� 6.��ü 7.���� 8.���� 9.����

  public string Failure_Penalty;            //����,�ս�,����
  public string Failure_Penalty_info;       //(ü��,���ŷ�,��),���� ID

  public string Reward_Target;              //����,ü��,���ŷ�,��,���-�׸�,���-����,Ư��
  public string Reward_Info;                //���� :ID  ü��,���ŷ�,��:X  �׸�:��ȭ,����,����,�н�  �������:�� ����  Ư��:ID
}
public class FollowEventJsonData:EventJsonData
{

  public string FollowType = "";              //�̺�Ʈ,����,Ư��,�׸�,���
  public string FollowTarget = "";            //�ش� ID Ȥ�� 0,1,2,3 Ȥ�� 0~9
  public string FollowTargetSuccess = "";            //(�̺�Ʈ) ����/����
  public string FollowTendency = "";          //�̺�Ʈ�� ��� ��Ÿ,�̼�,��ü,����,���� ������ ����

  public string EndingName = "";            //������ ���� ���(""�� �ƴ� ���) ���� �̸�
}
public class QuestEventDataJson:EventJsonData
{
  public string QuestType = "";
  public string QuestEventType = "";
}

