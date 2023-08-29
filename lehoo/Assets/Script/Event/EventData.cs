using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager;

public class EventHolder
{

  public List<EventData> AllNormalEvents = new List<EventData>();
  public List<FollowEventData> AllFollowEvents = new List<FollowEventData>();
  public QuestHolder_Wolf Quest_Wolf = null;
  public Quest GetQuest(QuestType type)
  {
    switch (type)
    {
      case QuestType.Wolf:return Quest_Wolf;
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
      for (int i = 0; i < _seasons.Length; i++) _data.Season.ElementAtOrDefault(int.Parse(_seasons[i]));

      Data.AppearSpace = (EventAppearType)int.Parse(_data.Settlement);

      string[] _places = _data.Place.Split('@');
      foreach (string place in _places)
      {
        int _index = int.Parse(place);
        PlaceType _targetplace = PlaceType.NULL;
        if (_index < 3)
        {
          _targetplace = (PlaceType)_index;
        }
        else if (_index > 2)
        {
          if (Data.AppearSpace.Equals(EventAppearType.City))
          {
            switch (_index)
            {
              case 3: _targetplace = PlaceType.Library; break;
            }
          }
          else
          {
            switch (_index)
            {
              case 3: _targetplace = PlaceType.Theater; break;
              case 4: _targetplace = PlaceType.Academy; break;
            }
          }
        }
      }

    }

    if (_data.Environment != "")
    {
      Data.EnvironmentType = (EnvironmentType)int.Parse(_data.Environment);
    }

    if (_data.Selection_Type != "")
    {
      Data.Selection_type = (SelectionType)int.Parse(_data.Selection_Type);
      switch (Data.Selection_type)//����,�̼���ü,���Ź���,��Ÿ ��� �з����� ������,����,���� ������ �����
      {
        case SelectionType.Single://���� ������
          Data.SelectionDatas = new SelectionData[1];
          Data.SelectionDatas[0] = new SelectionData(Data, -1);

          if (_data.Selection_Target != "")
          {
            Data.SelectionDatas[0].ThisSelectionType = (SelectionTargetType)int.Parse(_data.Selection_Target);
            switch (Data.SelectionDatas[0].ThisSelectionType)
            {
              case SelectionTargetType.None: //������
                break;
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
              Data.FailureDatas[0] = new FailureData(Data, -1);
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
            Data.SuccessDatas[0] = new SuccessData(Data, -1);
            Data.SuccessDatas[0].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target);
            switch (Data.SuccessDatas[0].Reward_Target)
            {
              case RewardTarget.Experience: Data.SuccessDatas[0].Reward_ID = _data.Reward_Info; break;
              case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;
              case RewardTarget.Skill: Data.SuccessDatas[0].Reward_Skill = (SkillType)int.Parse(_data.Reward_Info); break;
            }

            Data.SelectionDatas[0].SelectionSuccesRewards.Add(Data.SuccessDatas[0].Reward_Target);

          }

          break;
        case SelectionType.Body://�̼���ü
        case SelectionType.Head://���Ź���
          Data.SelectionDatas = new SelectionData[2];
          Data.FailureDatas = new FailureData[2];
          Data.SuccessDatas = new SuccessData[2];

          if (_data.Selection_Target != "")
          {
            for (int i = 0; i < Data.SelectionDatas.Length; i++)
            {
              Data.SelectionDatas[i] = new SelectionData(Data, i);

              Data.SelectionDatas[i].ThisSelectionType = (SelectionTargetType)int.Parse(_data.Selection_Target.Split('@')[i]);
              switch (Data.SelectionDatas[i].ThisSelectionType)
              {
                case SelectionTargetType.None: //������
                  break;
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
                Data.FailureDatas[i] = new FailureData(Data, i);
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
              Data.SuccessDatas[i] = new SuccessData(Data, i);
              Data.SuccessDatas[i].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target.Split('@')[i]);
              switch (Data.SuccessDatas[i].Reward_Target)
              {
                case RewardTarget.Experience: Data.SuccessDatas[i].Reward_ID = _data.Reward_Info.Split('@')[i]; break;
                case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;
                case RewardTarget.Skill: Data.SuccessDatas[i].Reward_Skill = (SkillType)int.Parse(_data.Reward_Info.Split('@')[i]); break;
              }

              Data.SelectionDatas[i].SelectionSuccesRewards.Add(Data.SuccessDatas[i].Reward_Target);
            }
          }
          break;

        case SelectionType.Tendency:
        case SelectionType.Experience:
          Data.SelectionDatas = new SelectionData[1];
          Data.SelectionDatas[0] = new SelectionData(Data, -1);
          if (_data.Selection_Target != "")
          {
            Data.SelectionDatas[0].ThisSelectionType = Data.Selection_type.Equals(SelectionType.Tendency) ? SelectionTargetType.Tendency : SelectionTargetType.Exp;

            Data.SuccessDatas = new SuccessData[1];
            Data.SuccessDatas[0] = new SuccessData(Data, -1);
            Data.SuccessDatas[0].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target);
            switch (Data.SuccessDatas[0].Reward_Target)
            {
              case RewardTarget.Experience: Data.SuccessDatas[0].Reward_ID = _data.Reward_Info; break;
              case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;
              case RewardTarget.Skill: Data.SuccessDatas[0].Reward_Skill = (SkillType)int.Parse(_data.Reward_Info); break;
            }
            Data.SelectionDatas[0].SelectionSuccesRewards.Add(Data.SuccessDatas[0].Reward_Target);
          }
          break;
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
    if (Quest_Wolf == null) Quest_Wolf = new QuestHolder_Wolf();


    switch ((QuestType) int.Parse(_data.QuestType))
    {
      case QuestType.Wolf:
        ConvertData_Quest_wolf(_data);
        break;
    }
    
  }//����Ʈ ������ ��ȹ ������ �߰��ؾ���
  public void ConvertData_Quest_wolf(QuestEventDataJson jsondata)
  {
    QuestEventData_Wolf eventdata = ReturnEventDataDefault<QuestEventData_Wolf>(jsondata);
    eventdata.Type = QuestType.Wolf;
    switch (int.Parse(jsondata.QuestEventType))
    {
      case 0:
   //     Quest_Wolf.PrologueEvents_0= eventdata;
        break;
      case 1:
   //     Quest_Wolf.PrologueEvent_Tendency0= eventdata;
        break;
      case 2:
   //     Quest_Wolf.PrologueEvent_1= eventdata;
        break;
      case 3:
    //    Quest_Wolf.PrologueEvent_Tendency1 = eventdata;
        break;
      case 4:
    //    Quest_Wolf.PrologueEvent_Last = eventdata;
        break;
      case 5:
        Quest_Wolf.SearchingEvents.Add(eventdata);
        break;
      case 6:
        Quest_Wolf.Event_Wanted = eventdata;
        break;
      case 7:
        Quest_Wolf.Events_Public_Common.Add(eventdata);
        break;
      case 8:
        Quest_Wolf.Events_Public_Final.Add(eventdata);
        break;
      case 9:
        Quest_Wolf.Events_Cult_0.Add(eventdata);
        break;
      case 10:
        Quest_Wolf.Event_Cult_Hideout_0 = eventdata;
        break;
      case 11:
        Quest_Wolf.Events_Cult_1.Add(eventdata);
        break;
      case 12:
        Quest_Wolf.Event_Cult_Hideout_1 = eventdata;
        break;
      case 13:
        Quest_Wolf.Events_Cult_2.Add(eventdata);
        break;
      case 14:
        Quest_Wolf.Event_Cult_Hideout_2 = eventdata;
        break;
      case 15:
        Quest_Wolf.Events_Cult_Final.Add(eventdata);
        break;
      case 16:
        Quest_Wolf.Event_Cult_Hideout_Final= eventdata;
        break;
      case 17:
        Quest_Wolf.Events_Wolf_0.Add(eventdata);
        break;
      case 18:
        Quest_Wolf.Event_Wolf_Encounter_0 = eventdata;
        break;
      case 19:
        Quest_Wolf.Events_Wolf_1.Add(eventdata);
        break;
      case 20:
        Quest_Wolf.Event_Wolf_Encounter_1= eventdata;
        break;
      case 21:
        Quest_Wolf.Events_Wolf_2.Add(eventdata);
        break;
      case 22:
        Quest_Wolf.Event_Wolf_Encounter_2= eventdata;
        break;
      case 23:
        Quest_Wolf.Events_Wolf_Final.Add(eventdata);
        break;
      case 24:
        Quest_Wolf.Event_Wolf_Encounter_Final= eventdata;
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
    if (eventdata.GetType() == typeof(QuestEventData_Wolf))
    {
      GameManager.Instance.MyGameData.RemoveEvent.Add(eventdata.ID);
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
    GameManager.Instance.MyGameData.RemoveEvent.Add(eventdata.ID);
  }
  /// <summary>
  /// �ܺ� �̺�Ʈ ��ȯ
  /// </summary>
  /// <param name="envir"></param>
  /// <returns></returns>
  public EventDataDefulat ReturnOutsideEvent(List<EnvironmentType> envir)
  {
        List<EventDataDefulat> _follow_envir = new List<EventDataDefulat>();
        List<EventDataDefulat> _follow_noenvir = new List<EventDataDefulat>();
        List<EventDataDefulat> _normal_envir = new List<EventDataDefulat>();
        List<EventDataDefulat> _normal_noenvir = new List<EventDataDefulat>();

        List<EventDataDefulat> _temp = new List<EventDataDefulat>();
    foreach (var _follow in AllFollowEvents)
    {
      if (GameManager.Instance.MyGameData.RemoveEvent.Contains(_follow.ID)) continue;
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
          if (_checktarget.Contains(_follow.FollowTarget)) _temp.Add(_follow);
          break;
        case FollowType.EXP://���� ������ ��� ���� ������ ���� ID�� �´��� Ȯ��
            if (_follow.FollowTarget.Equals(GameManager.Instance.MyGameData.LongTermEXP.ID)) _temp.Add(_follow);
          foreach (var _data in GameManager.Instance.MyGameData.ShortTermEXP)
            if (_follow.FollowTarget.Equals(_data.ID)) _temp.Add(_follow);
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
          if (_follow.FollowTargetLevel <= _targetlevel) _temp.Add(_follow);
          break;
      }
    }
    foreach (var _event in _temp)
    {
      if (_event.AppearSpace.Equals(EventAppearType.Outer))
      {
                if (_event.EnvironmentType == EnvironmentType.NULL) _follow_noenvir.Add(_event);
                else if (envir.Contains(_event.EnvironmentType)) _follow_envir.Add(_event);
      }
    }//���� ������ �̺�Ʈ�� �� ȯ��, ���� ������ �͵� ����Ʈ
    _temp.Clear();
    foreach (var _event in AllNormalEvents)
    {
      if (GameManager.Instance.MyGameData.RemoveEvent.Contains(_event.ID)) continue;
            if (_event.AppearSpace.Equals(EventAppearType.Outer))
            {
                if (_event.EnvironmentType == EnvironmentType.NULL) _normal_noenvir.Add(_event);
                else if (envir.Contains(_event.EnvironmentType)) _normal_envir.Add(_event);
            }
        }//�Ϲ� �߿� �̺�Ʈ �� ȯ��, ���� ������ �� �� ����Ʈ

        List<int> _perlist = new List<int>();
        Dictionary<int, List<EventDataDefulat>> _listbyper = new Dictionary<int, List<EventDataDefulat>>();
        if (_follow_envir.Count > 0) 
        { 
            _perlist.Add(ConstValues.EventPer_Outer_Follow_Envir);
            _listbyper.Add(ConstValues.EventPer_Outer_Follow_Envir, _follow_envir);
        }
        if (_follow_noenvir.Count > 0)
        {
            _perlist.Add(ConstValues.EventPer_Outer_Follow_NoEnvir);
            _listbyper.Add(ConstValues.EventPer_Outer_Follow_NoEnvir, _follow_noenvir);
        }
        if (_normal_envir.Count > 0)
        {
            _perlist.Add(ConstValues.EventPer_Outer_Normal_Envir);
            _listbyper.Add(ConstValues.EventPer_Outer_Normal_Envir, _normal_envir);
        }
        if (_normal_noenvir.Count > 0)
        {
            _perlist.Add(ConstValues.EnvirPer_Outer_Normal_NoEnvir);
            _listbyper.Add(ConstValues.EnvirPer_Outer_Normal_NoEnvir, _normal_noenvir);
        }
        int _max = 0;
        foreach (var value in _perlist) _max += value;

        int _value = Random.Range(0, _max);
        int _sum = 0;
        List<EventDataDefulat> _targetlist = new List<EventDataDefulat>();
        for(int i = 0; i < _perlist.Count; i++)
        {
            _sum += _perlist[i];
            if (_value < _sum) { _targetlist = _listbyper[_perlist[i]];break; }
        }
        return _targetlist[Random.Range(0,_targetlist.Count)];
    }
    /// <summary>
    /// �������� ��� �̺�Ʈ ��ȯ
    /// </summary>
    /// <param name="settletype"></param>
    /// <param name="placetype"></param>
    /// <param name="placelevel"></param>
    /// <param name="envir"></param>
    /// <returns></returns>
    public EventDataDefulat ReturnPlaceEvent(SettlementType settletype,PlaceType placetype,List<EnvironmentType> envir)
    {
        List<EventDataDefulat> _follow_envir_place = new List<EventDataDefulat>();
        List<EventDataDefulat> _follow_envir_noplace = new List<EventDataDefulat>();
        List<EventDataDefulat> _follow_noenvir_place = new List<EventDataDefulat>();
        List<EventDataDefulat> _follow_noenvir_noplace = new List<EventDataDefulat>();
        List<EventDataDefulat> _normal_envir_place = new List<EventDataDefulat>();
        List<EventDataDefulat> _normal_envir_noplace = new List<EventDataDefulat>();
        List<EventDataDefulat> _normal_noenvir_place = new List<EventDataDefulat>();
        List<EventDataDefulat> _normal_noenvir_noplace = new List<EventDataDefulat>();

        List<EventDataDefulat> _temp = new List<EventDataDefulat>();
    foreach (var _follow in AllFollowEvents)
    {
      if (GameManager.Instance.MyGameData.RemoveEvent.Contains(_follow.ID)) continue;
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
          if (_checktarget.Contains(_follow.FollowTarget)) _temp.Add(_follow);
          break;
        case FollowType.EXP://���� ������ ��� ���� ������ ���� ID�� �´��� Ȯ��
          if (_follow.FollowTarget.Equals(GameManager.Instance.MyGameData.LongTermEXP.ID)) _temp.Add(_follow);
          foreach (var _data in GameManager.Instance.MyGameData.ShortTermEXP)
            if (_follow.FollowTarget.Equals(_data.ID)) _temp.Add(_follow);
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
          if (_follow.FollowTargetLevel <= _targetlevel) _temp.Add(_follow);
          break;
      }
    }
    foreach (var _event in _temp)
        {
            if (_event.EnvironmentType == EnvironmentType.NULL)
            {
                if (_event.Places.Contains(placetype)) _follow_noenvir_place.Add(_event);
                else _follow_noenvir_noplace.Add(_event);
            }
            else if (envir.Contains(_event.EnvironmentType))
            {
                if (_event.Places.Contains(placetype)) _follow_envir_place.Add(_event);
                else _follow_envir_noplace.Add(_event);
            }
        }//�ش� ����� ������ ���� �̺�Ʈ ����Ʈ

    foreach (var _event in AllNormalEvents)
    {
      if (GameManager.Instance.MyGameData.RemoveEvent.Contains(_event.ID)) continue;
      if (_event.RightSpace(settletype) == false) continue;

      if (_event.EnvironmentType == EnvironmentType.NULL)
      {
        if (_event.Places.Contains(placetype)) _normal_noenvir_place.Add(_event);
        else _normal_noenvir_noplace.Add(_event);
      }
      else if (envir.Contains(_event.EnvironmentType))
      {
        if (_event.Places.Contains(placetype)) _normal_envir_place.Add(_event);
        else _normal_envir_noplace.Add(_event);
      }
    }//�ش� ����� ������ �Ϲ� �̺�Ʈ ����Ʈ

    List<int> _perlist = new List<int>();
        Dictionary<int, List<EventDataDefulat>> _listbyper = new Dictionary<int, List<EventDataDefulat>>();

        if (_follow_envir_place.Count > 0)
        {
            _perlist.Add(ConstValues.EventPer_Settle_Follow_Envir_Place);
            _listbyper.Add(ConstValues.EventPer_Settle_Follow_Envir_Place, _follow_envir_place);
        }
        if (_follow_envir_noplace.Count > 0)
        {
            _perlist.Add(ConstValues.EventPer_Settle_Follow_Envir_NoPlace);
            _listbyper.Add(ConstValues.EventPer_Settle_Follow_Envir_NoPlace, _follow_envir_noplace);
        }
        if (_follow_noenvir_place.Count > 0)
        {
            _perlist.Add(ConstValues.EventPer_Settle_Follow_NoEnvir_Place);
            _listbyper.Add(ConstValues.EventPer_Settle_Follow_NoEnvir_Place, _follow_noenvir_place);
        }
        if (_follow_noenvir_noplace.Count > 0)
        {
            _perlist.Add(ConstValues.EventPer_Settle_Follow_NoEnvir_NoPlace);
            _listbyper.Add(ConstValues.EventPer_Settle_Follow_NoEnvir_NoPlace, _follow_noenvir_noplace);
        }
        if (_normal_envir_place.Count > 0)
        {
            _perlist.Add(ConstValues.EventPer_Settle_Normal_Envir_Place);
            _listbyper.Add(ConstValues.EventPer_Settle_Normal_Envir_Place, _normal_envir_place);
        }
        if (_normal_envir_noplace.Count > 0)
        {
            _perlist.Add(ConstValues.EventPer_Settle_Normal_Envir_NoPlace);
            _listbyper.Add(ConstValues.EventPer_Settle_Normal_Envir_NoPlace, _normal_envir_noplace);
        }
        if (_normal_noenvir_place.Count > 0)
        {
            _perlist.Add(ConstValues.EventPer_Settle_Normal_NoEnvir_Place);
            _listbyper.Add(ConstValues.EventPer_Settle_Normal_NoEnvir_Place, _normal_noenvir_place);
        }
        if (_normal_noenvir_noplace.Count > 0)
        {
            _perlist.Add(ConstValues.EventPer_Settle_Normal_NoEnvir_NoPlace);
            _listbyper.Add(ConstValues.EventPer_Settle_Normal_NoEnvir_NoPlace, _normal_noenvir_noplace);
        }

        int _max = 0;
        foreach (var value in _perlist) _max += value;

        int _value = Random.Range(0, _max);
        int _sum = 0;
        List<EventDataDefulat> _targetlist = new List<EventDataDefulat>();
        for (int i = 0; i < _perlist.Count; i++)
        {
            _sum += _perlist[i];
            if (_value < _sum) { _targetlist = _listbyper[_perlist[i]]; break; }
        }
        return _targetlist[Random.Range(0, _targetlist.Count)];

    }
    /// <summary>
    /// �ش� �������� �ܺο��� ��� ������ ����Ʈ �̺�Ʈ �ϳ� ��ȯ
    /// </summary>
    /// <param name="tiledata"></param>
    /// <returns></returns>
    public EventDataDefulat ReturnQuestEvent(TileInfoData tiledata)
  {

    Settlement _targetsettlement = tiledata.Settlement;

    return null;
  }

}
public class TileInfoData
{
  public LandscapeType LandScape = LandscapeType.Outer;
  public Settlement Settlement=null; //������ Ÿ��
  public List<EnvironmentType> EnvirList = new List<EnvironmentType>();//���� ȯ�� Ÿ��
  
}
#region �̺�Ʈ ������ ���� �迭��
public enum FollowType { Event,EXP,Skill}
public enum SettlementType {Town,City,Castle,Outer}
public enum EventAppearType { Outer,Town,City,Castle,Settlement}
public enum PlaceType {NULL, Residence,Marketplace,Temple,Library,Theater,Academy}
public enum EnvironmentType { NULL, River,Forest,Highland,Mountain,Sea,Beach,Land,RiverBeach}
public enum SelectionType { Single,Body, Head,Tendency,Experience }// (Vertical)Body : �� �̼� �� ��ü    (Horizontal)Head : �� ���� �� ����    
public enum CheckTarget { None,Pay,Theme,Skill}
public enum PenaltyTarget { None,Status,EXP }
public enum RewardTarget { Experience,HP,Sanity,Gold,Skill,None}
public enum EventSequence { Progress,Clear}//Suggest: 3�� �����ϴ� �ܰ�  Progress: ������ ��ư ������ �ϴ� �ܰ�  Clear: ���� �����ؾ� �ϴ� �ܰ�
#endregion  
public class EventDataDefulat
{
  public string ID = "";
  public Sprite Illust
  {
    get
    {
     return  GameManager.Instance.ImageHolder.GetEventStartIllust(ID, Season);
    }
  }
  public string Name
  {
    get
    {
      string _str = GameManager.Instance.GetTextData(ID + "_NAME");
      return WNCText.GetSeasonText(_str);
    }
  }
  public string Description
  {
    get
    {
      string _str = GameManager.Instance.GetTextData(ID + "_DESCRIPTION");
      return WNCText.GetSeasonText(_str);
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
  public EventAppearType AppearSpace;
    public bool RightSpace(EventAppearType currentspace)
    {
        switch (AppearSpace)
        {
            case EventAppearType.Outer:
                if (currentspace == EventAppearType.Outer) return true;
                break;
            case EventAppearType.Town:
                if (currentspace == EventAppearType.Settlement || currentspace == EventAppearType.Town) return true;
                break;
            case EventAppearType.City:
                if (currentspace == EventAppearType.Settlement || currentspace == EventAppearType.City) return true;
                break;
            case EventAppearType.Castle:
                if (currentspace == EventAppearType.Settlement || currentspace == EventAppearType.Castle) return true;
                break;
            case EventAppearType.Settlement:
                if (currentspace == EventAppearType.Outer) return false;
                return true;
        }
        return false;
    } 
    public bool RightSpace(SettlementType currentsettle)
    {
        switch (AppearSpace)
        {
            case EventAppearType.Outer:
                return false;
            case EventAppearType.Town:
                if (currentsettle == SettlementType.Town) return true;
                break;
            case EventAppearType.City:
                if (currentsettle == SettlementType.City) return true;
                break;
            case EventAppearType.Castle:
                if (currentsettle == SettlementType.Castle) return true;
                break;
            case EventAppearType.Settlement:
                if (currentsettle == SettlementType.Outer) return false;
                return true;
        }
        return false;
    }

    public List<PlaceType> Places=new List<PlaceType>();
  public EnvironmentType EnvironmentType = EnvironmentType.NULL;

  public SelectionType Selection_type;
  public SelectionData[] SelectionDatas;

  public FailureData[] FailureDatas;

  public SuccessData[] SuccessDatas;
}
public class SelectionData
{
  private EventDataDefulat MyEvent = null;
  public SelectionData(EventDataDefulat myevent,int index)
  {
    MyEvent = myevent;Index = index;
  }
  private string ID { get { return MyEvent.ID + SearchWord_SubDescriptoin + SearchIndex; } }
  public int Index = 0; //-1(����),0,1(����)
    private string SearchIndex
    {
      get
      {
        return Index == -1 ? "" : string.Format("_{0}", Index.ToString());
      }
    }
    public SelectionTargetType ThisSelectionType = SelectionTargetType.None;
    private const string SearchWord_Name = "_SELECTION";
    public string Name
  {
    get {
        return WNCText.GetSeasonText(ID + SearchWord_Name + SearchIndex); 
      }
  }
    private const string SearchWord_SubDescriptoin = "_SELECTIONDESCRIPTION";
  public string SubDescription
  {
    get { return WNCText.GetSeasonText(GameManager.Instance.GetTextData(ID)); }
  }
  public StatusType SelectionPayTarget = StatusType.HP;
  public List<SkillType> SelectionCheckSkill = new List<SkillType>();
  public List<RewardTarget> SelectionSuccesRewards=new List<RewardTarget>();
}    

public class FailureData
{
  private EventDataDefulat MyEvent = null;
  public FailureData(EventDataDefulat myevent, int index)
  {
    MyEvent = myevent; Index = index;
  }
  private string ID { get { return MyEvent.ID + SearchWord_Description + SearchIndex; } }
  public int Index = -1;   //-1(����),0,1(����)
    private string SearchIndex
    {
      get
      {
        return Index == -1 ? "" : string.Format("_{0}", Index.ToString());
      }
    }
    private const string SearchWord_Description = "_FAIL_DESCRIPTION";
    public string Description
  {
    get { return WNCText.GetSeasonText(GameManager.Instance.GetTextData(MyEvent.ID)); }
  }
  public PenaltyTarget Panelty_target;
  public StatusType Loss_target= StatusType.HP;
  public string ExpID;
  public Sprite Illust
  {
    get
    {
      return GameManager.Instance.ImageHolder.GetEventResultIllust(MyEvent.ID, MyEvent.Season,Index, false);
    }
  }
}
public class GoldFailData : FailureData
{
  public GoldFailData() : base(null, -1) { }
  new public string Description = "";
  new public Sprite Illust = null;
}
public enum SelectionTargetType { None, Pay, Check_Single,Check_Multy ,Tendency, Exp }//������ ���� ����
public enum StatusType { HP,Sanity,Gold}
public class SuccessData
{
  private EventDataDefulat MyEvent = null;
  public SuccessData(EventDataDefulat myevent, int index)
  {
    MyEvent = myevent; Index = index;
  }
  private string ID { get { return MyEvent.ID + SearchWord_Description + SearchIndex; } }
  public int Index = -1;   //-1(����),0,1(����)
  private string SearchIndex
  {
    get
    {
      return Index == -1 ? "" : string.Format("_{0}", Index.ToString());
    }
  }
  private const string SearchWord_Description = "_SUCCESS_DESCRIPTION";
  public string Description
  {
    get
    {
      return WNCText.GetSeasonText(GameManager.Instance.GetTextData(ID));
    }
  }
  public Sprite Illust
  {
    get
    {
      return GameManager.Instance.ImageHolder.GetEventResultIllust(MyEvent.ID, MyEvent.Season, Index, true);
    }
  }
  public RewardTarget Reward_Target;
  public SkillType Reward_Skill;
  public string Reward_ID;
  private int reward_value_origin = -1;
  public int Reward_Value_Origin
  {
    get 
    { if (reward_value_origin.Equals(-1))
        switch (Reward_Target)
        {
          case RewardTarget.HP: reward_value_origin = GameManager.Instance.MyGameData.RewardHPValue_origin; break;
          case RewardTarget.Sanity: reward_value_origin = GameManager.Instance.MyGameData.RewardSanityValue_origin; break;
          case RewardTarget.Gold: reward_value_origin = GameManager.Instance.MyGameData.RewardGoldValue_origin; break;
        }
      return reward_value_origin;
    }
    set { reward_value_origin = value; }
  }

  private int reward_value_modified = -1;
  public int Reward_Value_Modified
  {
    get
    {
      if (reward_value_modified.Equals(-1))
        switch (Reward_Target)
        {
          case RewardTarget.HP: reward_value_modified = GameManager.Instance.MyGameData.RewardHPValue_modified; break;
          case RewardTarget.Sanity: reward_value_modified = GameManager.Instance.MyGameData.RewardSanityValue_modified; break;
          case RewardTarget.Gold: reward_value_modified = GameManager.Instance.MyGameData.RewardGoldValue_modified; break;
        }
      return reward_value_modified;
    }
    set { reward_value_modified = value; }
  }

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
  public QuestEnum_Wolf EventType;
}
public enum QuestType { Wolf}
public enum QuestEnum_Wolf { None,Prologue,Starting, Public, Cult,Wolf}
public class Quest
{
  public const string OriginID="";
  public string QuestName { get { return GameManager.Instance.GetTextData(OriginID + "_Name_Text"); } }
  public string QuestDescription { get { return GameManager.Instance.GetTextData(OriginID + "_PreDescription_Text"); } }
    public Sprite QuestIllust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Wolf, OriginID + "MainIllust"); } }
  public QuestType Type = QuestType.Wolf;
}
public class QuestHolder_Wolf:Quest
{
  public int Progress = 0;
  new public const string OriginID = "Quest_Wolf";

  public TileData[] RitualPlaces = new TileData[3];

  #region ���ѷα� ����
  public Sprite Prologue_0_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Wolf, OriginID + "_Prologue_0_Illust"); } }
  public string Prologue_0_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_0_Description"); } }
  public string Prologue_0_Selection { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_0_Selection"); } }
  public Sprite Prologue_Tendency_0_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Wolf, OriginID + "_Prologue_Tendency_0_Illust"); } }
  public string Prologue_Tendency_0_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_Tendency_0_Description"); } }
  public string Prologue_Tendency_0_Selection_0 { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_Tendency_0_Selection_0"); } }
  public Sprite Prologue_Tendency_0_Selection_0_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Wolf,OriginID + "_Prologue_Tendency_0_Selection_0_Illust"); } }
  public string Prologue_Tendency_0_Selection_0_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_Tendency_0_Selection_0_Description"); } }
  public string Prologue_Tendency_0_Selection_0_Selection { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_Tendency_0_Selection_0_Selection"); } }
  public string Prologue_Tendency_0_Selection_1 { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_Tendency_0_Selection_1"); } }
  public Sprite Prologue_Tendency_0_Selection_1_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Wolf, OriginID + "_Prologue_Tendency_0_Selection_1_Illust"); } }
  public string Prologue_Tendency_0_Selection_1_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_Tendency_0_Selection_1_Description"); } }
  public string Prologue_Tendency_0_Selection_1_Selection { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_Tendency_0_Selection_1_Selection"); } }
  public Sprite Prologue_Tendency_1_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Wolf, OriginID + "_Prologue_Tendency_1_Illust"); } }
  public string Prologue_Tendency_1_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_Tendency_1_Description"); } }
  public string Prologue_Tendency_1_Selection_0 { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_Tendency_1_Selection_0"); } }
  public Sprite Prologue_Tendency_1_Selection_0_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Wolf, OriginID + "_Prologue_Tendency_1_Selection_0_Illust"); } }
  public string Prologue_Tendency_1_Selection_0_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_Tendency_1_Selection_0_Description"); } }
  public string Prologue_Tendency_1_Selection_0_Selection { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_Tendency_1_Selection_0_Selection"); } }
  public string Prologue_Tendency_1_Selection_1 { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_Tendency_1_Selection_1"); } }
  public Sprite Prologue_Tendency_1_Selection_1_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Wolf, OriginID + "_Prologue_Tendency_1_Selection_1_Illust"); } }
  public string Prologue_Tendency_1_Selection_1_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_Tendency_1_Selection_1_Description"); } }
  public string Prologue_Tendency_1_Selection_1_Selection { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_Tendency_1_Selection_1_Selection"); } }
  public Sprite Prologue_Tendency_Last_Illust { get { return GameManager.Instance.ImageHolder.GetQuestIllust(QuestType.Wolf, OriginID + "_Prologue_Last_Illust"); } }
  public string Prologue_Last_Description { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_Last_Description"); } }
  public string Prologue_Last_Selection { get { return GameManager.Instance.GetTextData(OriginID + "_Prologue_Last_Selection"); } }
  #endregion
  public List<QuestEventData_Wolf> SearchingEvents = new List<QuestEventData_Wolf>();       //QuestEventType 5 

  public QuestEventData_Wolf Event_Wanted = null;                                  //QuestEventType 6
  public List<QuestEventData_Wolf> Events_Public_Common = new List<QuestEventData_Wolf>(); //QuestEventType 7
  public List<QuestEventData_Wolf> Events_Public_Final = new List<QuestEventData_Wolf>();  //QuestEventType 8

  public List<QuestEventData_Wolf> Events_Cult_0 = new List<QuestEventData_Wolf>();     //QuestEventType 9
  public QuestEventData_Wolf Event_Cult_Hideout_0 = null;                          //QuestEventType 10
  public List<QuestEventData_Wolf> Events_Cult_1 = new List<QuestEventData_Wolf>();     //QuestEventType 11
  public QuestEventData_Wolf Event_Cult_Hideout_1 = null;                          //QuestEventType 12
  public List<QuestEventData_Wolf> Events_Cult_2 = new List<QuestEventData_Wolf>();     //QuestEventType 13
  public QuestEventData_Wolf Event_Cult_Hideout_2 = null;                          //QuestEventType 14
  public List<QuestEventData_Wolf> Events_Cult_Final = new List<QuestEventData_Wolf>(); //QuestEventType 15
  public QuestEventData_Wolf Event_Cult_Hideout_Final = null;                      //QuestEventType 16

  public List<QuestEventData_Wolf> Events_Wolf_0 = new List<QuestEventData_Wolf>();     //QuestEventType 17
  public QuestEventData_Wolf Event_Wolf_Encounter_0 = null;                        //QuestEventType 18
  public List<QuestEventData_Wolf> Events_Wolf_1 = new List<QuestEventData_Wolf>();     //QuestEventType 19
  public QuestEventData_Wolf Event_Wolf_Encounter_1 = null;                        //QuestEventType 20
  public List<QuestEventData_Wolf> Events_Wolf_2 = new List<QuestEventData_Wolf>();     //QuestEventType 21
  public QuestEventData_Wolf Event_Wolf_Encounter_2 = null;                        //QuestEventType 22
  public List<QuestEventData_Wolf> Events_Wolf_Final = new List<QuestEventData_Wolf>(); //QuestEventType 23
  public QuestEventData_Wolf Event_Wolf_Encounter_Final = null;                    //QuestEventType 24

}//                                         ����Ʈ ������ �� ����
public class EventJsonData
{
  public string ID = "";              //ID
  public string Settlement = "";          //0,1,2,3
  public string Place = "";               //0,1,2,3,4
    public string Environment = "";  //����, ��,��,���,��,�ٴ�
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

