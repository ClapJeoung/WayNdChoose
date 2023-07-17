using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EventHolder
{
  private const int PlacePer = 2, LevelPer = 3, EnvirPer = 2;
  public List<EventData> AvailableNormalEvents = new List<EventData>();
  public List<FollowEventData> AvailableFollowEvents = new List<FollowEventData>();
  public List<QuestHolder> AvailableQuests = new List<QuestHolder>();

  public List<EventData> AllNormalEvents = new List<EventData>();
  public List<FollowEventData> AllFollowEvents = new List<FollowEventData>();
  public List<QuestHolder> AllQuests = new List<QuestHolder>();
  public EventDataDefulat ReturnEventDataDefault(EventJsonData _data)
  {
    EventDataDefulat Data = new EventDataDefulat();

    Data.ID = _data.ID;

    string[] _seasons = _data.Season.Split('@');
    for (int i = 0; i < _seasons.Length; i++) _data.Season.ElementAtOrDefault(int.Parse(_seasons[i]));

    Data.SettlementType = (SettlementType)_data.Settlement;
    switch (_data.Place)
    {
      case 0: Data.PlaceType = PlaceType.Residence; break;
      case 1: Data.PlaceType = PlaceType.Marketplace; break;
      case 2: Data.PlaceType = PlaceType.Temple; break;
    }
    if (_data.Place > 2)
    {
      if (Data.SettlementType.Equals(SettlementType.City))
      {
        switch (_data.Place)
        {
          case 3: Data.PlaceType = PlaceType.Library; break;
        }
      }
      else
      {
        switch (_data.Place)
        {
          case 3: Data.PlaceType = PlaceType.Theater; break;
          case 4: Data.PlaceType = PlaceType.Academy; break;
        }
      }
    }

    Data.EnvironmentType = (EnvironmentType)_data.Environment;

    Data.Selection_type = (SelectionType)_data.Selection_Type;

    switch (Data.Selection_type)//����,�̼���ü,���Ź���,��Ÿ ��� �з����� ������,����,���� ������ �����
    {
      case SelectionType.Single://���� ������
        Data.SelectionDatas = new SelectionData[1];
        Data.SelectionDatas[0] = new SelectionData(Data ,-1);

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
        if (Data.SelectionDatas[0].ThisSelectionType.Equals(SelectionTargetType.Check_Single)||
          Data.SelectionDatas[0].ThisSelectionType.Equals(SelectionTargetType.Check_Multy))
        {
          Data.FailureDatas = new FailureData[1];
          Data.FailureDatas[0] = new FailureData(Data,-1);
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
        Data.SuccessDatas[0] = new SuccessData(Data,-1);
        Data.SuccessDatas[0].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target);
        switch (Data.SuccessDatas[0].Reward_Target)
        {
          case RewardTarget.Experience: Data.SuccessDatas[0].Reward_ID = _data.Reward_Info; break;
          case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;
          case RewardTarget.Skill: Data.SuccessDatas[0].Reward_Skill = (SkillType)int.Parse(_data.Reward_Info); break;
        }
        Data.SuccessDatas[0].SubReward_target = int.Parse(_data.SubReward);

        Data.SelectionDatas[0].SelectionSuccesRewards.Add(Data.SuccessDatas[0].Reward_Target);
        switch (Data.SuccessDatas[0].SubReward_target)
        {
          case 0: break;
          case 1: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity); break;
          case 2: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold); break;
          case 3:
            if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity);
            if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold);
            break;
        }

        break;

      case SelectionType.Body://�̼���ü
      case SelectionType.Head://���Ź���
        Data.SelectionDatas = new SelectionData[2];

        for(int i = 0; i < Data.SelectionDatas.Length; i++)
        {
          Data.SelectionDatas[i] = new SelectionData(Data, i);

          Data.SelectionDatas[i].ThisSelectionType = (SelectionTargetType)int.Parse(_data.Selection_Target);
          switch (Data.SelectionDatas[i].ThisSelectionType)
          {
            case SelectionTargetType.None: //������
              break;
            case SelectionTargetType.Pay: //����
              Data.SelectionDatas[i].SelectionPayTarget = (StatusType)int.Parse(_data.Selection_Info);
              break;
            case SelectionTargetType.Check_Single: //���(����)
              Data.SelectionDatas[i].SelectionCheckSkill.Add((SkillType)int.Parse(_data.Selection_Info));
              break;
            case SelectionTargetType.Check_Multy: //���(����)
              string[] _temp = _data.Selection_Info.Split(',');
              for (int j = 0; j < _temp.Length; j++) Data.SelectionDatas[i].SelectionCheckSkill.Add((SkillType)int.Parse(_temp[j]));
              break;
          }
          if (Data.SelectionDatas[i].ThisSelectionType.Equals(SelectionTargetType.Check_Single) ||
            Data.SelectionDatas[i].ThisSelectionType.Equals(SelectionTargetType.Check_Multy))
          {
            Data.FailureDatas = new FailureData[1];
            Data.FailureDatas[i] = new FailureData(Data, -1);
            Data.FailureDatas[i].Panelty_target = (PenaltyTarget)int.Parse(_data.Failure_Penalty);
            switch (Data.FailureDatas[i].Panelty_target)
            {
              case PenaltyTarget.None: break;
              case PenaltyTarget.Status: Data.FailureDatas[i].Loss_target = (StatusType)int.Parse(_data.Failure_Penalty_info); break;
              case PenaltyTarget.EXP: Data.FailureDatas[i].ExpID = _data.Failure_Penalty_info; break;
            }
          }
          else if (Data.SelectionDatas[i].ThisSelectionType.Equals(SelectionTargetType.Pay) && Data.SelectionDatas[i].SelectionPayTarget.Equals(StatusType.Gold))
          {
            Data.FailureDatas = new FailureData[1]; Data.FailureDatas[i] = GameManager.Instance.GoldFailData;
          }
          Data.SuccessDatas = new SuccessData[1];
          Data.SuccessDatas[i] = new SuccessData(Data, -1);
          Data.SuccessDatas[i].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target);
          switch (Data.SuccessDatas[i].Reward_Target)
          {
            case RewardTarget.Experience: Data.SuccessDatas[i].Reward_ID = _data.Reward_Info; break;
            case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;
            case RewardTarget.Skill: Data.SuccessDatas[i].Reward_Skill = (SkillType)int.Parse(_data.Reward_Info); break;
          }
          Data.SuccessDatas[i].SubReward_target = int.Parse(_data.SubReward);

          Data.SelectionDatas[i].SelectionSuccesRewards.Add(Data.SuccessDatas[i].Reward_Target);
          switch (Data.SuccessDatas[i].SubReward_target)
          {
            case 0: break;
            case 1: if (!Data.SelectionDatas[i].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[i].SelectionSuccesRewards.Add(RewardTarget.Sanity); break;
            case 2: if (!Data.SelectionDatas[i].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[i].SelectionSuccesRewards.Add(RewardTarget.Gold); break;
            case 3:
              if (!Data.SelectionDatas[i].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[i].SelectionSuccesRewards.Add(RewardTarget.Sanity);
              if (!Data.SelectionDatas[i].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[i].SelectionSuccesRewards.Add(RewardTarget.Gold);
              break;
          }
        }

        break;

      case SelectionType.Tendency:
      case SelectionType.Experience:
        Data.SelectionDatas = new SelectionData[1];
        Data.SelectionDatas[0] = new SelectionData(Data,-1);
        Data.SelectionDatas[0].ThisSelectionType =Data.Selection_type.Equals(SelectionType.Tendency)?SelectionTargetType.Tendency: SelectionTargetType.Exp;
      
        Data.SuccessDatas = new SuccessData[1];
        Data.SuccessDatas[0] = new SuccessData(Data,-1);
        Data.SuccessDatas[0].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target);
        switch (Data.SuccessDatas[0].Reward_Target)
        {
          case RewardTarget.Experience: Data.SuccessDatas[0].Reward_ID = _data.Reward_Info; break;
          case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;
          case RewardTarget.Skill: Data.SuccessDatas[0].Reward_Skill = (SkillType)int.Parse(_data.Reward_Info); break;
        }
        Data.SuccessDatas[0].SubReward_target = int.Parse(_data.SubReward);
        Data.SelectionDatas[0].SelectionSuccesRewards.Add(Data.SuccessDatas[0].Reward_Target);
        switch (Data.SuccessDatas[0].SubReward_target)
        {
          case 0: break;
          case 1: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity); break;
          case 2: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold); break;
          case 3:
            if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity);
            if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold);
            break;
        }
        break;
    }
    return Data;
  }
    public void ConvertData_Normal(EventJsonData _data)
  {
    EventData Data = (EventData)ReturnEventDataDefault(_data);
      AllNormalEvents.Add(Data);
  }

  public void ConvertData_Follow(FollowEventJsonData _data)
  {
    FollowEventData Data = (FollowEventData)ReturnEventDataDefault(_data);

    Data.FollowType = (FollowType)_data.FollowType; //���� ��� �̺�Ʈ,����,Ư��,�׸�,���
    Data.FollowTarget = _data.FollowTarget;         //���� ���- �̺�Ʈ,����,Ư���̸� Id   �׸��� 0,1,2,3  ����̸� 0~9
    if (Data.FollowType == FollowType.Event)
    {
      Data.FollowTargetSuccess = _data.FollowTargetSuccess == 0 ? true : false;//���� ����� �̺�Ʈ�� ��� ���� Ȥ�� ����
      Data.FollowTendency = _data.FollowTendency;                              //���� ����� �̺�Ʈ�� ��� ������ ����
    }
    else if (Data.FollowType.Equals(FollowType.Skill))
      Data.FollowTargetLevel = _data.FollowTargetSuccess;

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
    QuestEventData Data = (QuestEventData)ReturnEventDataDefault(_data);
    ����~;
  }//����Ʈ ������ ��ȹ ������ �߰��ؾ���

  public void SetAllEvents()
  {
    foreach (var _event in AllNormalEvents) AvailableNormalEvents.Add(_event);
    foreach(var _followevent in AllFollowEvents) AvailableFollowEvents.Add(_followevent);
  }//ó�� ���� �� ��� �̺�Ʈ�� ��� ���� �̺�Ʈ Ǯ�� �ֱ�
  public void LoadAllEvents()
  {
    bool _isenable = true;
    foreach (var _event in AllNormalEvents)
    {
      _isenable = true;
      foreach (var _removeoriginid in GameManager.Instance.MyGameData.RemoveEvent)
      {
        if (_event.ID.Equals(_removeoriginid))
        {
          Debug.Log(_event.ID);
          _isenable = false;
          break;
        }
      }
      if (_isenable) AvailableNormalEvents.Add(_event);
    }
    foreach (var _event in AllFollowEvents)
    {
      _isenable = true;
      foreach (var _removeoriginid in GameManager.Instance.MyGameData.RemoveEvent)
      {
        if (_event.ID.Equals(_removeoriginid))
        {
          _isenable = false;
          break;
        }
      }
      if (_isenable) AvailableFollowEvents.Add(_event);
    }
  }//Gamemanager.instance.GameData�� ������� �̹� Ŭ������ �̺�Ʈ ���� �� Ȱ��ȭ ����Ʈ�� �ֱ�
  public void RemoveEvent(string _originid)
  {

    List<EventData> _normals = new List<EventData>();
    List<FollowEventData> _follows = new List<FollowEventData>();

    foreach (var _data in AvailableNormalEvents)
      if (_data.ID.Equals(_originid))
        _normals.Add(_data);

    foreach (var _data in AvailableFollowEvents)
      if (_data.ID.Equals(_originid))
        _follows.Add(_data);

    foreach (var _deletenormal in _normals) AvailableNormalEvents.Remove(_deletenormal);
    foreach (var _deletfollow in _follows) AvailableFollowEvents.Remove(_deletfollow);
    GameManager.Instance.MyGameData.RemoveEvent.Add(_originid);
  }
  /// <summary>
  /// �ܺ� �̺�Ʈ ��ȯ
  /// </summary>
  /// <param name="envir"></param>
  /// <returns></returns>
  public EventDataDefulat ReturnOutsideEvent(List<EnvironmentType> envir)
  {
    List<EventDataDefulat> _temp = new List<EventDataDefulat>();
    List<EventDataDefulat> _followevents = new List<EventDataDefulat>();
    foreach (var _follow in AvailableFollowEvents)
    {
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
      if (_event.SettlementType.Equals(SettlementType.Outer))
      {
        if (_event.EnvironmentType == EnvironmentType.NULL || envir.Contains(_event.EnvironmentType))
        {
          if(_event.Season.Equals(4)||_event.Season.Equals(GameManager.Instance.MyGameData.Turn))
          _followevents.Add(_event);
        }
      }
    }//���� ������ �̺�Ʈ�� �� ȯ��, ���� ������ �͵� ����Ʈ
    _temp.Clear();
    List<EventDataDefulat> _normalevents = new List<EventDataDefulat>();
    foreach (var _event in AvailableNormalEvents)
    {
      if (_event.SettlementType.Equals(SettlementType.Outer))
      {
        if(_event.EnvironmentType != EnvironmentType.NULL|| envir.Contains(_event.EnvironmentType))
          if (_event.Season.Equals(4) || _event.Season.Equals(GameManager.Instance.MyGameData.Turn))
            _normalevents.Add(_event);
      }
    }//�Ϲ� �߿� �̺�Ʈ �� ȯ��, ���� ������ �� �� ����Ʈ

    if (_followevents.Count > 0 && _normalevents.Count > 0)
    {
      if (Random.Range(0, 100) < 80) return _followevents[Random.Range(0, _followevents.Count)];
      else return _normalevents[Random.Range(0, _normalevents.Count)];
    }//�� Ǯ �� �֤��� 80�� ����
    else if (_followevents.Count > 0 && _normalevents.Count.Equals(0)) return _followevents[Random.Range(0, _followevents.Count)];
    else return _normalevents[Random.Range(0, _normalevents.Count)];
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
        List<EventDataDefulat> _temp = new List<EventDataDefulat>();
        List<EventDataDefulat> _follows = new List<EventDataDefulat>();
        foreach (var _follow in AvailableFollowEvents)
        {
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
        foreach(var _event in _temp)
        {
            if (isenable(_event)) _follows.Add(_event);
        }//�ش� ����� ������ ���� �̺�Ʈ ����Ʈ

        List<EventDataDefulat> _normals = new List<EventDataDefulat>();
        foreach (var _event in AvailableNormalEvents)
        {
            if (isenable(_event)) _normals.Add(_event);
        }//�ش� ����� ������ �Ϲ� �̺�Ʈ ����Ʈ

        if (_follows.Count > 0 && _normals.Count > 0)
        {
            if (Random.Range(0, 100) < 80) return _follows[Random.Range(0, _follows.Count)];
            else return _normals[Random.Range(0, _normals.Count)];
        }
        else if(_follows.Count>0&&_normals.Count.Equals(0)) return _follows[Random.Range(0, _follows.Count)];
        else return _normals[Random.Range(0, _normals.Count)];

        bool isenable(EventDataDefulat eventdata)
        {
            if (!eventdata.SettlementType.Equals(settletype)) return false;     //�ش� �̺�Ʈ�� �������� ���� ������ X

            if (!eventdata.PlaceType.Equals(placetype)) return false;           //�ش� �̺�Ʈ�� ��Ұ� ���� ������ X

            if (eventdata.EnvironmentType != EnvironmentType.NULL)                              //ȯ�� �˻��� ��
                if (!envir.Contains(eventdata.EnvironmentType))return false;    //�ش� �̺�Ʈ�� ȯ���� ���� ������ X

      if (eventdata.Season != 4 && eventdata.Season != GameManager.Instance.MyGameData.Turn) return false;  //���� ������ �ƴѵ��� ������ ���� ������ X


        //������� ������ �� ��������Ƿ� O ��ȯ
        return true;
        }
    }
  /// <summary>
  /// �ش� �������� �ܺο��� ��� ������ ����Ʈ �̺�Ʈ �ϳ� ��ȯ
  /// </summary>
  /// <param name="tiledata"></param>
  /// <returns></returns>
  public EventDataDefulat ReturnQuestEvent(TargetTileEventData tiledata)
  {
    if (!GameManager.Instance.MyGameData.QuestAble) return null;

    QuestHolder _currentquest = GameManager.Instance.MyGameData.CurrentQuest;
    if (_currentquest == null) return null;
    switch (_currentquest.CurrentSequence)
    {
      case QuestSequence.Falling: //������ �ܰ迡���� ������ �̺�Ʈ �� �ϳ� 
        if (isenable(_currentquest.Event_Falling)) return _currentquest.Event_Falling;
        else return null;
      case QuestSequence.Climax:
        var _targetevents = _currentquest.Event_Climax;
        return _targetevents;
      default:
        List<QuestEventData> _temp = _currentquest.EventList_Rising_Available;
        List<QuestEventData> _avail = new List<QuestEventData>();
        foreach (var _event in _temp)
        {
          if (isenable(_event)) _avail.Add(_event);
        }
        if (_avail.Count.Equals(0)) return null;
        else return _avail[Random.Range(0, _avail.Count)];
    }

    bool isenable(EventDataDefulat eventdata)
    {
      if (eventdata.SettlementType.Equals(SettlementType.Outer))
      {
        if (!tiledata.SettlementType.Equals(SettlementType.Outer)) return false;
      }   //�߿� �̺�Ʈ�� ��� �߿ܸ� ����
      else
      {
        if (eventdata.SettlementType.Equals(SettlementType.None))
        {
          if (tiledata.SettlementType.Equals(SettlementType.Outer)) return false;
          //�߿� ���� �� ����
        }   //���� �������� ���
        else if (!eventdata.SettlementType.Equals(tiledata.SettlementType)) return false;
        //�� �� ������ �� �ƴ� �� X ��ȯ

      }   //������ �̺�Ʈ�� ���

      if (!tiledata.PlaceList.Contains(eventdata.PlaceType)) return false;           //�ش� �̺�Ʈ�� ��Ұ� ���� ������ X

      if (eventdata.EnvironmentType!=EnvironmentType.NULL)                              //ȯ�� �˻��� ��
        if (!tiledata.EnvironmentType.Contains(eventdata.EnvironmentType)) return false;    //�ش� �̺�Ʈ�� ȯ���� ���� ������ X

      if (eventdata.Season != 4 && eventdata.Season != GameManager.Instance.MyGameData.Turn) return false;  //���� ������ �ƴѵ��� ������ ���� ������ X

      //������� ������ �� ��������Ƿ� O ��ȯ
      return true;
    }

  }

  public bool IsFollowEventEnable(TargetTileEventData tiledata,PlaceType place)
  {
    List<EventDataDefulat> _follows = new List<EventDataDefulat>();
    List<EventDataDefulat> _temp=new List<EventDataDefulat>();
    foreach (var _follow in AvailableFollowEvents)
    {
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
          }//���� �̺�Ʈ ������ ��� ���� �̺�Ʈ ����Ʈ�� �˻� ���
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
          }//���� �κ�Ʈ ������ ��� ���� �̺�Ʈ ����Ʈ�� �˻� ���

          if (_checktarget.Contains(_follow.FollowTarget)) _temp.Add(_follow);//�ϴ� �˻� ������ �´ٸ� �ֱ�
          break;

        case FollowType.EXP://���� ������ ��� ���� ������ ���� ID�� �´��� Ȯ��
            if (_follow.FollowTarget.Equals(GameManager.Instance.MyGameData.LongTermEXP.ID)) _temp.Add(_follow);
          foreach (var _data in GameManager.Instance.MyGameData.ShortTermEXP)
            if (_follow.FollowTarget.Equals(_data.ID)) _temp.Add(_follow);
          break;

        case FollowType.Skill://��� ������ ��� ���� �׸��� ������ ���� �̻����� Ȯ��
          int _targetlevel = 0;
          switch (_follow.FollowTarget)
          {
            case "0"://��ȭ �׸�
              _targetlevel = GameManager.Instance.MyGameData.Skill_Conversation.Level; break;
            case "1"://���� �׸�
              _targetlevel = GameManager.Instance.MyGameData.Skill_Force.Level; break;
            case "2"://���� �׸�
              _targetlevel = GameManager.Instance.MyGameData.Skill_Wild.Level; break;
            case "3"://�н� �׸�
              _targetlevel = GameManager.Instance.MyGameData.Skill_Intelligence.Level; break;
          }
          if (_follow.FollowTargetLevel <= _targetlevel) _temp.Add(_follow);
          break;

      }
    }
    foreach (var _event in _temp)
    {
      if (isenable(_event)) _follows.Add(_event);
    }//�ش� ����� ������ ���� �̺�Ʈ ����Ʈ

    if (_follows.Count > 0) return true;
    else return false;

    bool isenable(EventDataDefulat eventdata)
    {
      if (!eventdata.SettlementType.Equals(tiledata.SettlementType)) return false;     //�ش� �̺�Ʈ�� �������� ���� ������ X

      if (!eventdata.PlaceType.Equals(place)) return false;           //�ش� �̺�Ʈ�� ��Ұ� ���� ������ X

      if (eventdata.EnvironmentType!=EnvironmentType.NULL)                              //ȯ�� �˻��� ��
        if (!tiledata.EnvironmentType.Contains(eventdata.EnvironmentType)) return false;    //�ش� �̺�Ʈ�� ȯ���� ���� ������ X


      //������� ������ �� ��������Ƿ� O ��ȯ
      return true;
    }
  }
}
public class TargetTileEventData
{
  public SettlementType SettlementType; //������ Ÿ��
  public List<PlaceType> PlaceList = new List<PlaceType>(); //(�������� ���) ��� Ÿ�԰� ��� ����
  public List<EnvironmentType> EnvironmentType = new List<EnvironmentType>();//���� ȯ�� Ÿ��
  public int Season;
}
#region �̺�Ʈ ������ ���� �迭��
public enum FollowType { Event,EXP,Skill}
public enum SettlementType { Town,City,Castle,Outer,None}
public enum PlaceType { Residence,Marketplace,Temple,Library,Theater,Academy,NULL}
public enum EnvironmentType { NULL, River,Forest,Highland,Mountain,Sea}
public enum SelectionType { Single,Body, Head,Tendency,Experience }// (Vertical)Body : �� �̼� �� ��ü    (Horizontal)Head : �� ���� �� ����    
public enum CheckTarget { None,Pay,Theme,Skill}
public enum PenaltyTarget { None,Status,EXP }
public enum RewardTarget { Experience,HP,Sanity,Gold,Skill,None}
public enum EventSequence { Progress,Clear}//Suggest: 3�� �����ϴ� �ܰ�  Progress: ������ ��ư ������ �ϴ� �ܰ�  Clear: ���� �����ؾ� �ϴ� �ܰ�
public enum QuestSequence { Start,Rising,Climax,Falling}
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
  public SettlementType SettlementType;
  public PlaceType PlaceType;
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
  public int SubReward_target;

}

public class EventData:EventDataDefulat
{}//�⺻ �̺�Ʈ
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
public class QuestEventData : EventDataDefulat
{
  public string QuestID = "";
  public QuestSequence TargetQuestSequence= QuestSequence.Start;
  public int ClimaxIndex = 0;
}
public class QuestHolder
{
    public string QuestID = "";     //����Ʈ ID
  public string QuestID_name
  {
    get
    {
      var _temp = QuestID.Split("_");
      return _temp[1];
    }
  }//quest_(�� �κ�)
  public string QuestName
  {
    get { return QuestTextDatas[0]; }
  }//����Ʈ�� �̸�
  public QuestSequence CurrentSequence=QuestSequence.Start; //���� ����Ʈ �ܰ�

  private List<string> questtextdatas = new List<string>();
  private List<string> QuestTextDatas
  {
    get
    {
      if (questtextdatas.Count.Equals(0))
      {
        questtextdatas.Add(GameManager.Instance.GetTextData(QuestID));
        questtextdatas.Add(GameManager.Instance.GetTextData(QuestID + "_rising"));
        for (int i = 0; i < ClimaxEventCount; i++)
          questtextdatas.Add(GameManager.Instance.GetTextData(QuestID + "_climax" + "_" + i.ToString()));
        questtextdatas.Add(GameManager.Instance.GetTextData(QuestID + "_falling"));
      }
      return questtextdatas;
    }
  }
  public string StartDialogue
  {
    get { return QuestTextDatas[0]; }
  }   //���� ���ο��� ����Ʈ ������ �� ������ �ؽ�Ʈ
  public string PreDescription
  {
    get { return QuestTextDatas[0]; }
  }  //���� ȭ�� �̸����� �ؽ�Ʈ
  public Sprite StartIllust = null; //���� �Ϸ���Ʈ

  public Sprite ExpSelectionIllust
  {
    get
    {
      return null;
    }
  }//���� ���� ���� �Ϸ���Ʈ
  public string ExpSelectionDescription
  {
    get
    {
      return GameManager.Instance.GetTextData(QuestID + "_expselect");
    }
  }//���� ���� ���� ����
  public Experience[] StartingExps
  {
    get
    {
      Experience[] _temp = new Experience[4];
      _temp[0] = GameManager.Instance.ExpDic["exp_" + QuestID_name + "_0"];
      _temp[1] = GameManager.Instance.ExpDic["exp_" + QuestID_name + "_1"];
      _temp[2] = GameManager.Instance.ExpDic["exp_" + QuestID_name + "_2"];
      _temp[3] = GameManager.Instance.ExpDic["exp_" + QuestID_name + "_3"];
      return _temp;
    }
  }//���� ���� 4��

  public Sprite StartingIllust
  {
    get { return null; }
  }//��� �Ϸ���Ʈ
  public string StartingDescription
  {
    get { return GameManager.Instance.GetTextData(QuestID+"_starting"); }
  }//��� ����
  public string StartingSelection
  {
    get { return GameManager.Instance.GetTextData(QuestID + "_starting"); }
  }//��� ������ ����
  public string RisingDescription
  {
    get { return QuestTextDatas[1]; }
  }
  public string ClimaxDescription
  {
    get { return QuestTextDatas[2 + FinishedClimaxCount]; }
  }
  public string FallingDescription
  {
    get { return QuestTextDatas[QuestTextDatas.Count - 1]; }
  }
  public string CurrentDescription
  {
    get
    {
      switch (CurrentSequence)
      {
        case QuestSequence.Rising:return RisingDescription;
          case QuestSequence.Climax:return ClimaxDescription;
        default:return FallingDescription;
      }
    }
  }

  public string StopSelectionName
  {
    get { return QuestTextDatas[QuestTextDatas.Count - 1].Split('@')[0]; }
  }
  public string StopSelectionSub
  {
    get { return QuestTextDatas[QuestTextDatas.Count - 1].Split('@')[0]; }
  }
  public string ContinueSelectionName
  {
    get
    {
     return QuestTextDatas[QuestTextDatas.Count - 1].Split('@')[1];
    }
  }
  public string ContinueSelectionSub {
    get
    {
      return QuestTextDatas[QuestTextDatas.Count - 1].Split('@')[1];
    }
  }
  public List<QuestEventData> Eventlist_Rising=new List<QuestEventData>();
  private List<string> eventlist_rising_origin = new List<string>();
  public List<string> Eventlist_Rising_Count
  {
    get
    {
      if (eventlist_rising_origin.Count.Equals(0))
      {
        foreach (var _origin in Eventlist_Rising)
          if (!eventlist_rising_origin.Contains(_origin.ID)) eventlist_rising_origin.Add(_origin.ID);
      }
      return eventlist_rising_origin;
    }
  }
    public List<string> Eventlist_Rising_clear = new List<string>();//�Ϸ��� �� �ܰ� �̺�Ʈ(���� ID)
  public List<QuestEventData> EventList_Rising_Available
  {
    get
    {
      List<QuestEventData> _temp = new List<QuestEventData>();
      foreach (var _event in Eventlist_Rising)
      {
        if (Eventlist_Rising_clear.Contains(_event.ID)) continue;
        _temp.Add(_event);
      }
      if (_temp.Count.Equals(0)) return null;
      else return _temp;
    }
  }
  public List<QuestEventData> Eventlist_Climax = new List<QuestEventData>();
  private List<string> eventlist_climax_origin = new List<string>();
  public List<string> EventList_Climax_Origin
  {
    get
    {
      if (eventlist_climax_origin.Count.Equals(0))
      {
        foreach (var _origin in Eventlist_Climax)
          if (!eventlist_climax_origin.Contains(_origin.ID)) eventlist_climax_origin.Add(_origin.ID);
      }
      return eventlist_climax_origin;
    }
  }//�� �ܰ� �̺�Ʈ ID(����)
  public QuestEventData Event_Falling = null;

  public int RisingEventCount
  {
    get
    {
      List<string> _originids = new List<string>();
      foreach (var _rising in Eventlist_Rising) if (!_originids.Contains(_rising.ID)) _originids.Add(_rising.ID);
      return _originids.Count;
    }
  }
  public int ClimaxEventCount
  {
    get
    {
      List<string> _originids = new List<string>();
      foreach (var _climax in Eventlist_Climax) if (!_originids.Contains(_climax.ID)) _originids.Add(_climax.ID);
      return _originids.Count;
    }
  }
  public int QuestClearCount
  {
    get { return SuccessRisingCount + SuccesClimaxCount; }
  } //������ �̺�Ʈ(rising,climax) ����
  public int FinishedRisingCount = 0;  //����+������ Rising ����(���� ID)
  public int FinishedClimaxCount = 0;  //����+������ Climax ����(���� ID)
  public int SuccessRisingCount = 0, SuccesClimaxCount = 0;//������ Rising,Climax ����(���� ID)
  public QuestEventData Event_Climax
  {
    get
    {
      if (!CurrentSequence.Equals(QuestSequence.Climax)) return null;

      QuestEventData _availableclimaxevent = null;
      foreach (var _questevent in Eventlist_Climax)
      {
        if (_questevent.ID.Equals(EventList_Climax_Origin[FinishedClimaxCount]))
        {
          if(_questevent.Season.Equals(0)|| _questevent.Season.Equals(GameManager.Instance.MyGameData.Turn + 1))
          {
            _availableclimaxevent = _questevent;
            break;
          }
        }
      }
      return _availableclimaxevent;//���� ID ����Ʈ�� EventList_Climax_Origin Ȱ���� ���� ������ ���� ID�� �ش��ϰ� ������ �´� �̺�Ʈ�� ��ȯ
    }
  }
  public Settlement NextQuestSettlement = null;
  public EnvironmentType NextQuestEnvir = EnvironmentType.NULL;
  public void AddClearEvent(QuestEventData _eventdata)
  {
    GameManager.Instance.MyGameData.LastQuestCount++;
    switch (_eventdata.TargetQuestSequence)
    {
      case QuestSequence.Start:
        CurrentSequence = QuestSequence.Rising;
        break;
      case QuestSequence.Rising:
        string _defaultid = _eventdata.ID;

          if (!Eventlist_Rising_clear.Contains(_defaultid)) Eventlist_Rising_clear.Add(_defaultid);
        //Ŭ������ �̺�Ʈ�� �⺻ ID�� �������� �ذ� Rising ����Ʈ�� �ִ´�

        SuccessRisingCount++;
        //�¸� ī��Ʈ�� ++
        FinishedRisingCount++;
        //�Ϸ� ī��Ʈ�� ++

        if (RisingEventCount - 2 < FinishedRisingCount)
        {
          if (Random.Range(0, 100) < 80) CurrentSequence = QuestSequence.Climax;
        }
        else if (RisingEventCount.Equals(FinishedRisingCount)) CurrentSequence = QuestSequence.Climax;
        //���� ���� �� ���� �ϸ� ���� Ȯ���� ������, ���� �� ä������ ��� ������
        break;
      case QuestSequence.Climax:
        NextQuestSettlement = null;
        NextQuestEnvir = EnvironmentType.NULL;
        //��ҿ� ������ ����Ʈ�� �ذ������Ƿ�(�߿� ����Ʈ�� �������)

        SuccesClimaxCount++;
        //�¸� ī��Ʈ�� ++
        FinishedClimaxCount++;
        //�Ϸ� ī��Ʈ�� ++
        if (ClimaxEventCount.Equals(FinishedClimaxCount)) CurrentSequence = QuestSequence.Falling;
        //Climax ���� �Ϸ������� ����������

        break;
      case QuestSequence.Falling:
        GameManager.Instance.MyGameData.CurrentEvent = null;
        break;
    }

  }
  public void AddFailEvent(QuestEventData _eventdata)
  {
    GameManager.Instance.MyGameData.LastQuestCount++;
    switch (_eventdata.TargetQuestSequence)
    {
      case QuestSequence.Start:
        CurrentSequence = QuestSequence.Rising;
        break;
      case QuestSequence.Rising:
        string _defaultid = _eventdata.ID;

          if (!Eventlist_Rising_clear.Contains(_defaultid)) Eventlist_Rising_clear.Add(_defaultid);
          //�Ϸ� ��Ͽ� �ֱ�

        FinishedRisingCount++;
        //������ �������Ƿ� �Ϸ� ī��Ʈ�� ++

        if (RisingEventCount - 2 < FinishedRisingCount)
        {
          if (Random.Range(0, 100) < 80) CurrentSequence = QuestSequence.Climax;
        }
        else if (RisingEventCount.Equals(FinishedRisingCount)) CurrentSequence = QuestSequence.Climax;
        break;
      case QuestSequence.Climax:
        NextQuestSettlement = null;
        NextQuestEnvir = EnvironmentType.NULL;
        //��ҿ� ������ ����Ʈ�� �ذ������Ƿ�(�߿� ����Ʈ�� �������)

        FinishedClimaxCount++;
        if (ClimaxEventCount.Equals(FinishedClimaxCount)) CurrentSequence = QuestSequence.Falling;
        break;
      case QuestSequence.Falling:
        GameManager.Instance.MyGameData.CurrentEvent = null;
        break;
    }
  }

}//                                         ����Ʈ ������ �� ����
public class EventJsonData
{
  public string ID = "";              //ID
  public int Settlement = 0;          //0,1,2,3
  public int Place = 0;               //0,1,2,3,4
    public int Environment = 0;  //����, ��,��,���,��,�ٴ�
  public string Season ;              //����,��,����,����,�ܿ�

  public int Selection_Type;           //0.���� 1.�̼�+��ü 2.����+���� 3.���� 4.���� 5.���
  public string Selection_Target;           //0.������ 1.���� 2.�׸� 3.���
  public string Selection_Info;             //0:���� ����  1:ü��,���ŷ�,��
                                            //2:��ȭ,����,����,����
                                            //3: 0.���� 1.����  2.�⸸  3.�� 4.���� 5.Ȱ�� 6.��ü 7.���� 8.���� 9.����

  public string Failure_Penalty;            //����,�ս�,����
  public string Failure_Penalty_info;       //(ü��,���ŷ�,��),���� ID

  public string Reward_Target;              //����,ü��,���ŷ�,��,���-�׸�,���-����,Ư��
  public string Reward_Info;                //���� :ID  ü��,���ŷ�,��:X  �׸�:��ȭ,����,����,�н�  �������:�� ����  Ư��:ID

  public string SubReward;                  //����,��,���ŷ�,��+���ŷ�
}
public class FollowEventJsonData:EventJsonData
{

  public int FollowType = 0;              //�̺�Ʈ,����,Ư��,�׸�,���
  public string FollowTarget = "";            //�ش� ID Ȥ�� 0,1,2,3 Ȥ�� 0~9
  public int FollowTargetSuccess = 0;            //(�̺�Ʈ) ����/����
  public int FollowTendency = 0;          //�̺�Ʈ�� ��� ��Ÿ,�̼�,��ü,����,���� ������ ����

  public string EndingName = "";            //������ ���� ���(""�� �ƴ� ���) ���� �̸�
}
public class QuestEventDataJson:EventJsonData
{
  public string QuestId = "";                 //����Ʈ ID
  public int Sequence = 0;                   //0:��  1:��   2:��   3:��
}

