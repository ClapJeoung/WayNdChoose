using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using System.Linq;

public class EventHolder
{
  private const int PlacePer = 2, LevelPer = 3, EnvirPer = 2;
  public List<EventData> AvailableNormalEvents = new List<EventData>();
  public List<FollowEventData> AvailableFollowEvents = new List<FollowEventData>();
  public Dictionary<string, QuestHolder> AvailableQuests = new Dictionary<string, QuestHolder>();

  public List<EventData> AllNormalEvents = new List<EventData>();
  public List<FollowEventData> AllFollowEvents = new List<FollowEventData>();
  public Dictionary<string, QuestHolder> AllQuests = new Dictionary<string, QuestHolder>();
  public void ConvertData_Normal(EventJsonData _data)
  {
    EventData Data = new EventData();
    Data.ID = _data.ID;
    Data.IllustID = _data.IllustID;
    Data.Name = _data.Name;
    Data.Description = _data.Description;
    Data.Season = _data.Season;
    Data.Selection_type = (SelectionType)_data.Selection_Type;
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
    Data.PlaceLevel = _data.Place_Level;

    Data.EnvironmentType = (EnvironmentType)_data.Environment_Type;

    Data.Selection_description = _data.Selection_Description.Split('@');

    string[] _temp = _data.Selection_Target.Split('@');
    Data.Selection_target = new CheckTarget[_temp.Length];
    for (int i = 0; i < _temp.Length; i++) Data.Selection_target[i] = (CheckTarget)int.Parse(_temp[i]);

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

      if (_data.Failure_Penalty_info != null)
        Data.Failure_penalty_info = _data.Failure_Penalty_info.Split('@');
    }

    Data.Success_description = _data.Success_Description.Split('@');

    _temp = _data.Reward_Target.Split('@');
    Data.Reward_Target = new RewardTarget[_temp.Length];
    for (int i = 0; i < _temp.Length; i++) Data.Reward_Target[i] = (RewardTarget)int.Parse(_temp[i]);

    if (_data.Reward_Info != null)
      Data.Reward_info = _data.Reward_Info.Split('@');

    _temp = _data.SubReward.Split('@');
    Data.SubReward_target = new int[_temp.Length];
    for (int i = 0; i < _temp.Length; i++) Data.SubReward_target[i] = int.Parse(_temp[i]);

    AllNormalEvents.Add(Data);
  }
  public void ConvertData_Follow(FollowEventJsonData _data)
  {
    FollowEventData Data = new FollowEventData();
    Data.ID = _data.ID;
    Data.IllustID = _data.IllustID;

    Data.Season = _data.Season;
    Data.FollowType = (FollowType)_data.FollowType; //���� ��� �̺�Ʈ,����,Ư��,�׸�,���
    Data.FollowTarget = _data.FollowTarget;         //���� ���- �̺�Ʈ,����,Ư���̸� Id   �׸��� 0,1,2,3  ����̸� 0~9
    if (Data.FollowType == FollowType.Event)
    {
      Data.FollowTargetSuccess = _data.FollowTargetSuccess == 0 ? true : false;//���� ����� �̺�Ʈ�� ��� ���� Ȥ�� ����
      Data.FollowTendency = _data.FollowTendency;                              //���� ����� �̺�Ʈ�� ��� ������ ����
    }
    else if (Data.FollowType.Equals(FollowType.Theme) || Data.FollowType.Equals(FollowType.Skill))
      Data.FollowTargetLevel =_data.FollowTargetSuccess;


    Data.Name = _data.Name;
    Data.Description = _data.Description;
    Data.Selection_type = (SelectionType)_data.Selection_Type;
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
    Data.PlaceLevel = _data.Place_Level;
    Data.EnvironmentType = (EnvironmentType)_data.Environment_Type;

    Data.Selection_description = _data.Selection_Description.Split('@');

    string[] _temp = _data.Selection_Target.Split('@');
    Data.Selection_target = new CheckTarget[_temp.Length];
    for (int i = 0; i < _temp.Length; i++) Data.Selection_target[i] = (CheckTarget)int.Parse(_temp[i]);

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

      if (_data.Failure_Penalty_info != null)
        Data.Failure_penalty_info = _data.Failure_Penalty_info.Split('@');
    }

    Data.Success_description = _data.Success_Description.Split('@');

    _temp = _data.Reward_Target.Split('@');
    Data.Reward_Target = new RewardTarget[_temp.Length];
    for (int i = 0; i < _temp.Length; i++) Data.Reward_Target[i] = (RewardTarget)int.Parse(_temp[i]);

    if (_data.Reward_Info != null)
      Data.Reward_info = _data.Reward_Info.Split('@');

    _temp = _data.SubReward.Split('@');
    Data.SubReward_target = new int[_temp.Length];
    for (int i = 0; i < _temp.Length; i++) Data.SubReward_target[i] = int.Parse(_temp[i]);

    AllFollowEvents.Add(Data);
  }
  public void ConvertData_Quest(QuestEventDataJson _data)
  {
    QuestEventData Data = new QuestEventData();
    if (_data.Sequence != 0)
    {
      Data.QuestID = _data.QuestId;
      Data.TargetQuestSequence = (QuestSequence)_data.Sequence;
      if (Data.TargetQuestSequence.Equals(QuestSequence.Climax))
      {
        Data.ClimaxIndex = AllQuests[Data.QuestID].Eventlist_Climax.Count + 1;
      }
      Data.ID = _data.ID;
      Data.IllustID = _data.IllustID;
      Data.Name = _data.Name;
      Data.Season = 0;
      Data.Description = _data.Description;
      Data.Selection_type = (SelectionType)_data.Selection_Type;
      switch (_data.Settlement)
      {
        case 0: Data.SettlementType = SettlementType.None; break;//0: �ƹ� ������
        case 1: Data.SettlementType = SettlementType.Town; break;//1: ����
        case 2: Data.SettlementType = SettlementType.City; break;  //2: ����
        case 3: Data.SettlementType = SettlementType.Castle; break; //3: ��ä
        case 4: Data.SettlementType = SettlementType.Outer; break;//4: �ܺ�
      }
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
      Data.EnvironmentType = (EnvironmentType)_data.Environment_Type;

      Data.Selection_description = _data.Selection_Description.Split('@');

      string[] _temp = _data.Selection_Target.Split('@');
      Data.Selection_target = new CheckTarget[_temp.Length];
      for (int i = 0; i < _temp.Length; i++) Data.Selection_target[i] = (CheckTarget)int.Parse(_temp[i]);

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

        if (_data.Failure_Penalty_info != null)
          Data.Failure_penalty_info = _data.Failure_Penalty_info.Split('@');
      }

      Data.Success_description = _data.Success_Description.Split('@');

      _temp = _data.Reward_Target.Split('@');
      Data.Reward_Target = new RewardTarget[_temp.Length];
      for (int i = 0; i < _temp.Length; i++) Data.Reward_Target[i] = (RewardTarget)int.Parse(_temp[i]);

      if (_data.Reward_Info != null)
        Data.Reward_info = _data.Reward_Info.Split('@');

      _temp = _data.SubReward.Split('@');
      Data.SubReward_target = new int[_temp.Length];
      for (int i = 0; i < _temp.Length; i++) Data.SubReward_target[i] = int.Parse(_temp[i]);

    }

    QuestHolder _quest = null;
    if (AllQuests.ContainsKey(_data.QuestId))
    {
      _quest = AllQuests[_data.QuestId];
    }//��ųʸ��� ����Ʈ�� �̹� ��������� ���
    else
    {
      _quest = new QuestHolder();
    }//��ųʸ��� ����Ʈ�� ���� ��� ���� �ϳ� �����

    switch (_data.Sequence)
    {
      case 0://��
                _quest.QuestID = _data.QuestId;
        _quest.QuestName = _data.Name;
        _quest.StartDialogue = _data.Description;
        _quest.PreDescription = _data.PreDescription;
        _quest.StartIllustID = _data.IllustID;
        break;
      case 1://��
        _quest.Eventlist_Rising.Add(Data);
        break;
      case 2://��
        _quest.Eventlist_Climax.Add(Data);
        break;
      case 3://��
        _quest.Event_Falling = Data;
        break;
    }
    if (!AllQuests.ContainsKey(_data.QuestId)) AllQuests.Add(_data.QuestId, _quest);
  }

  public void LoadAllEvents()
  {
    foreach (var _event in AllNormalEvents)
    {
      if (GameManager.Instance.MyGameData.RemoveEvent.Contains(_event.ID)) continue;
      AvailableNormalEvents.Add(_event);
    }
    foreach (var _event in AllFollowEvents)
    {
      if (GameManager.Instance.MyGameData.RemoveEvent.Contains(_event.ID)) continue;
      AvailableFollowEvents.Add(_event);
    }
    foreach (var _quest in AllQuests)
    {
      if (GameManager.Instance.MyGameData.ClearQuest.Contains(_quest.Key)) continue;
      if (GameManager.Instance.MyGameData.CurrentQuest == _quest.Value) continue;
      AvailableQuests.Add(_quest.Key, _quest.Value);
    }
  }//Gamemanager.instance.GameData�� ������� �̹� Ŭ������ �̺�Ʈ ���� �� Ȱ��ȭ ����Ʈ�� �ֱ�
  public void RemoveEvent(string _ID)
  {
    List<EventData> _eventdatas = new List<EventData>();
    foreach (var _data in AvailableNormalEvents)
    {
      if (_data.ID.Equals(_ID))
      {
        _eventdatas.Add(_data);
      }
    }
    foreach (var _deletedata in _eventdatas) AvailableNormalEvents.Remove(_deletedata);
  }
  public List<EventDataDefulat> ReturnEvent(TargetTileEventData _tiledata)
  {
    List<EventDataDefulat> _ResultEvents = new List<EventDataDefulat>();
    List<EventDataDefulat> _followevents=new List<EventDataDefulat>();
    List<EventDataDefulat> _normalevents=new List<EventDataDefulat>();
    List<EventDataDefulat> _temp = new List<EventDataDefulat>();
    if (GameManager.Instance.MyGameData.CurrentQuest != null)
    {
      QuestHolder _currentquest = GameManager.Instance.MyGameData.CurrentQuest; //���� Ȱ��ȭ�� ����Ʈ
      switch (_currentquest.CurrentSequence)
      {
        case QuestSequence.Rising:  //'��' �ܰ� �̺�Ʈ�� �ؾ� �� ��

          List<EventDataDefulat> _temptemp = new List<EventDataDefulat>();
          foreach (var _risingevent in _currentquest.Eventlist_Rising)
          {
            if (SimpleCheck(_risingevent))_temp.Add(_risingevent);    //������ �˻��ؼ� _temp�� �߰�
          }

          break;
        case QuestSequence.Climax:  //'��' �ܰ� �̺�Ʈ�� �ؾ� �� ��

          EventDataDefulat _climaxevent = _currentquest.Eventlist_Climax[_currentquest.CurrentClimaxIndex];
          if (SimpleCheck(_climaxevent)) _temp.Add(_climaxevent);    //������ �˻��ؼ� _temp�� �߰�

          break;
        case QuestSequence.Falling: //������ �̺�Ʈ�� �ؾ� �� ��

          EventDataDefulat _falling = _currentquest.Event_Falling;
          if (SimpleCheck(_falling)) _temp.Add(_falling);    //������ �˻��ؼ� _temp�� �߰�

          break;
      }

      foreach (var _event in _temp) _ResultEvents.Add(_event);
      _temp.Clear();

    }//���� ���� ���� ����Ʈ�� �ִٸ� ä���

    if (_tiledata.SettlementType.Equals(SettlementType.Outer) && _ResultEvents.Count.Equals(1)) return _ResultEvents;
    //�߿� �̺�Ʈ���, �ϳ� ä�����ڸ��� �ٷ� ��ȯ
  //  Debug.Log($"������ ����Ʈ �̺�Ʈ {_ResultEvents.Count}��");

    //������ ���� �̺�Ʈ ��� ��������
    _temp.Clear();
    foreach(var _follow in AvailableFollowEvents)
    {
      switch (_follow.FollowType)
      {
        case FollowType.Event:
          List<string> _checktarget=new List<string>();
          if (_follow.FollowTargetSuccess == true)
          {
            switch (_follow.FollowTendency)
            {
              case 0:_checktarget = GameManager.Instance.MyGameData.ClearEvent_None;break;
              case 1: _checktarget = GameManager.Instance.MyGameData.ClearEvent_Rational;break;
              case 2:_checktarget = GameManager.Instance.MyGameData.ClearEvent_Force;break;
              case 3:_checktarget=GameManager.Instance.MyGameData.ClearEvent_Mental;break;
              case 4: _checktarget = GameManager.Instance.MyGameData.ClearEvent_Material; break;
            }
          }
          else
          {
            switch (_follow.FollowTendency)
            {
              case 0: _checktarget = GameManager.Instance.MyGameData.FailEvent_None; break;
              case 1: _checktarget = GameManager.Instance.MyGameData.FailEvent_Rational; break;
              case 2: _checktarget = GameManager.Instance.MyGameData.FailEvent_Force; break;
              case 3: _checktarget = GameManager.Instance.MyGameData.FailEvent_Mental; break;
              case 4: _checktarget = GameManager.Instance.MyGameData.FailEvent_Material; break;
            }
          }
          if (_checktarget.Contains(_follow.FollowTarget)) _temp.Add(_follow);
          break;
        case FollowType.EXP://���� ������ ��� ���� ������ ���� ID�� �´��� Ȯ��
          foreach(var _data in GameManager.Instance.MyGameData.LongTermEXP)
            if(_follow.FollowTarget.Equals(_data.ID))_temp.Add(_follow);
          foreach (var _data in GameManager.Instance.MyGameData.ShortTempEXP)
            if (_follow.FollowTarget.Equals(_data.ID)) _temp.Add(_follow);
          break;
        case FollowType.Trait://Ư�� ������ ��� ���� ������ Ư�� ID�� �´��� Ȯ��
          foreach (var _data in GameManager.Instance.MyGameData.Traits)
            if (_follow.FollowTarget.Equals(_data.ID)) _temp.Add(_follow);
          break;
        case FollowType.Theme://�׸� ������ ��� ���� �׸��� ������ ���� �̻����� Ȯ��
          int _targetlevel = 0;
                    ThemeType _type = ThemeType.Conversation; ;
          switch (_follow.FollowTarget)
          {
            case "0"://��ȭ �׸�
                            _type = ThemeType.Conversation;  break;
            case "1"://���� �׸�
 _type = ThemeType.Force;break;
            case "2"://���� �׸�
                            _type = ThemeType.Nature;break;
            case "3"://�н� �׸�
                            _type = ThemeType.Intelligence;break;
          }
                    _targetlevel = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_type) + GameManager.Instance.MyGameData.GetEffectThemeCount_Trait(_type) +
                                GameManager.Instance.MyGameData.GetEffectThemeCount_Exp(_type) + GameManager.Instance.MyGameData.GetThemeLevelByTendency(_type);
                    if (_follow.FollowTargetLevel <= _targetlevel) _temp.Add(_follow);
          break;
        case FollowType.Skill://��� ������ ��� ���� ����� ������ ���� �̻����� Ȯ��
          SkillName _skill = (SkillName)int.Parse(_follow.FollowTarget);
          if(_follow.FollowTargetLevel<= GameManager.Instance.MyGameData.Skills[_skill].Level)_temp.Add(_follow);
          break;
      }
    }
    foreach (var _event in _temp)
    {
      if (SimpleCheck(_event)) _followevents.Add(_event);
    }
  //  Debug.Log($"������ ���� �̺�Ʈ {_followevents.Count}��");
    if (_tiledata.SettlementType.Equals(SettlementType.Outer) && _followevents.Count > 0)
    {
      _ResultEvents.Add(_followevents[Random.Range(0, _followevents.Count)]);
      return _ResultEvents;
    }//�߿� �̺�Ʈ�� ����Ʈ �̺�Ʈ�� �ƹ��͵� ������ �̰� �ϳ� ä��� �ٷ� ��ȯ


    //������ �Ϲ� �̺�Ʈ ��� ��������
    _temp.Clear();
    foreach(var _event in AvailableNormalEvents)
    {
      if(SimpleCheck(_event)) _normalevents.Add(_event);
    }
    string _str = $"������ �Ϲ� �̺�Ʈ {_normalevents.Count}��\n";
    foreach(var _event in _normalevents)
    {
      _str += $"{_event.Name}   ";
    }
   // Debug.Log(_str);
    if (_tiledata.SettlementType.Equals(SettlementType.Outer) && _followevents.Count.Equals(0) && _normalevents.Count > 0)
    {
      _ResultEvents.Add(_normalevents[Random.Range(0, _normalevents.Count)]);
      return _ResultEvents;
    }//�߿� �̺�Ʈ�� ����Ʈ,���� �� �� ������ �̰� �ϳ� ä��� ��ȯ

    List<PlaceType> _normalfirstplace=new List<PlaceType>();
    List<PlaceType> _normalsecondplace = new List<PlaceType>();
    List<PlaceType> _followfirstplace =new List<PlaceType>();
    List<PlaceType> _followsecondplace=new List<PlaceType>();
    foreach(var _event in _normalevents)
    {
      PlaceType _place = _event.PlaceType;
      if (GameManager.Instance.MyGameData.LastPlaceTypes.Contains(_place))
      {
        if(!_normalsecondplace.Contains(_place)) _normalsecondplace.Add(_place);
      }
      else
      {
        if(!_normalfirstplace.Contains(_place)) _normalfirstplace.Add(_place);
      }
    }
    if (_followevents == null) _followevents = new List<EventDataDefulat>();
    foreach(var _event in _followevents)
    {
      PlaceType _place = _event.PlaceType;
      if (GameManager.Instance.MyGameData.LastPlaceTypes.Contains(_place))
      {
        if (!_followsecondplace.Contains(_place)) _followsecondplace.Add(_place);
      }
      else
      {
        if (!_followfirstplace.Contains(_place)) _followfirstplace.Add(_place);
      }
    }
    //�Ϲ�,���� �̺�Ʈ�� �ٽ� ��Һ��� ����
    Dictionary<PlaceType,List<EventDataDefulat>> _normalplaceevents=new Dictionary<PlaceType,List<EventDataDefulat>>();
    Dictionary<PlaceType,List<EventDataDefulat>> _followplaceevents=new Dictionary<PlaceType,List<EventDataDefulat>>();
    foreach(var _event in _normalevents)
    {
      if(_normalplaceevents.ContainsKey(_event.PlaceType))_normalplaceevents[_event.PlaceType].Add(_event);
      else
      {
        List<EventDataDefulat> _newlist=new List<EventDataDefulat>();
        _newlist.Add(_event);
        _normalplaceevents.Add(_event.PlaceType, _newlist);
      }
    }
    foreach (var _event in _followevents)
    {
      if (_followplaceevents.ContainsKey(_event.PlaceType)) _followplaceevents[_event.PlaceType].Add(_event);
      else
      {
        List<EventDataDefulat> _newlist = new List<EventDataDefulat>();
        _newlist.Add(_event);
        _followplaceevents.Add(_event.PlaceType, _newlist);
      }
    }

    List<EventDataDefulat> _otherevents = new List<EventDataDefulat>();
    if (_ResultEvents.Count.Equals(1))
    {
      _otherevents = CheckForResult(1, 1);
      if (_otherevents==null||_otherevents.Count.Equals(0)) _otherevents = CheckForResult(0, 2);
      if (_otherevents==null||_otherevents.Count.Equals(0)) _otherevents = CheckForResult(2, 0);
      if (_otherevents==null||_otherevents.Count.Equals(0)) Debug.Log("��í�ƾ�");
    }//����Ʈ �̺�Ʈ�� �ϳ� ä���� ���� ���
    else if (_ResultEvents.Count.Equals(0))
    {
      _otherevents = CheckForResult(2, 1);
      if (_otherevents==null||_otherevents.Count.Equals(0)) _otherevents = CheckForResult(1, 2);
      if (_otherevents==null||_otherevents.Count.Equals(0)) _otherevents = CheckForResult(0, 3);
      if (_otherevents==null||_otherevents.Count.Equals(0)) _otherevents = CheckForResult(3, 0);
      if (_otherevents==null||_otherevents.Count.Equals(0)) Debug.Log("�����ƾ�");
    }//����Ʈ �̺�Ʈ ���� �� ĭ�� 3���� ���

    foreach (var _event in _otherevents) _ResultEvents.Add(_event);
    return _ResultEvents;

    bool SimpleCheck(EventDataDefulat _data)
    {
      if (checkbysettle(_data) == true &&                              //������ ������ �°�
       _tiledata.PlaceData.ContainsKey(_data.PlaceType) &&         //����� �� �ִ� ��Ұ�
       (_data.PlaceLevel.Equals(0)||_data.PlaceLevel.Equals(_tiledata.PlaceData[_data.PlaceType]))&&//�̺�Ʈ�� ��� ������ 0�̰ų� ��� ������ ����
       _tiledata.EnvironmentType.Contains(_data.EnvironmentType)) //���� ȯ�濡 �´ٸ�
        return true;  //true ��ȯ
      return false;   //������ ������ �ٸ��ų�, �� �� �ִ� ��Ұ� �ƴϰų�, ��� ������ �ƿ� �ٸ��ų�, ȯ���� ������ �ƴ϶�� false

      bool checkbysettle(EventDataDefulat _data)
      {
        if (_data.SettlementType.Equals(SettlementType.Outer))
          if (_tiledata.SettlementType.Equals(SettlementType.Outer)) return true;//�߿� �̺�Ʈ�� �߿� ��Ҹ� ���

        if (_data.SettlementType.Equals(SettlementType.None))
        {
          if (_tiledata.SettlementType.Equals(SettlementType.Town) ||
              _tiledata.SettlementType.Equals(SettlementType.City) ||
              _tiledata.SettlementType.Equals(SettlementType.Castle)) return true;//���� �̺�Ʈ��� ����,����,��ä�� ���
        }

        if (_data.SettlementType.Equals(_tiledata.SettlementType)) return true;//�� �ܴ� �ش��ϴ� �������� �޴´�
        return false;
      }

    }
    List<EventDataDefulat> CheckSinglePlaceEvents(List<EventDataDefulat> _list)
    {
      if (_list.Count.Equals(0)) return _list;  //0��¥���� �˻� ����

      List<EventDataDefulat> _resultlist=new List<EventDataDefulat>();
      List<EventDataDefulat> _templist = new List<EventDataDefulat>();   //�˻� ����� �ִ� �ӽ� ����Ʈ

      bool _levelfailed = false;
      //_list���� ��� ������ ���� �Ÿ��� �ܰ�
      List<EventDataDefulat> _firstlevels = new List<EventDataDefulat>(); //�ش� ����� ������ �´� �̺�Ʈ�� �ִ� ����Ʈ
      List<EventDataDefulat> _secondlevels = new List<EventDataDefulat>();//��� ������ 0(����)�� �̺�Ʈ�� �ִ� ����Ʈ
      foreach (var _event in _list)
      {
        if (_event.PlaceLevel.Equals(0)) _secondlevels.Add(_event);
        else if (_event.PlaceLevel.Equals(_tiledata.PlaceData[_event.PlaceType])) _firstlevels.Add(_event);
      }
      //   Debug.Log($"���� ������(��ü) {_list.Count}���� ������ ���� {_firstlevels.Count}/{_secondlevels.Count}�� ��ȭ");
      while (true)
      {
        bool _envirfailed = false;
        //_firstlevel���� ȯ�濡 ���� �Ÿ��� �ܰ�
        List<EventDataDefulat> _firstenvir = new List<EventDataDefulat>();
        List<EventDataDefulat> _secondenvir = new List<EventDataDefulat>();
        foreach (var _event in _firstlevels)
        {
          if (_event.EnvironmentType.Equals(EnvironmentType.None)) _secondenvir.Add(_event);
          else if (_tiledata.EnvironmentType.Contains(_event.EnvironmentType)) _firstenvir.Add(_event);
        }
      //  Debug.Log($"���� ������(����) {_firstlevels.Count}���� ȯ�濡 ���� {_firstenvir.Count}/{_secondenvir.Count}�� ��ȭ");
        while (true)
        {
          bool _seasonfailed = false;
          //_firstenvir���� ������ ���� �Ÿ��� �ܰ�
          List<EventDataDefulat> _firstseason=new List<EventDataDefulat>();
          List<EventDataDefulat> _secondseason=new List<EventDataDefulat>();
          foreach(var _event in _firstenvir)
          {
            if (_event.Season.Equals(0)) _secondseason.Add(_event);
            else if(_event.Season.Equals(_tiledata.Season)) _firstseason.Add(_event);
          }
       //   Debug.Log($"���� ������(ȯ��) {_firstenvir.Count}���� ������ ���� {_firstseason.Count}/{_secondseason.Count}�� ��ȭ");
          while (true)
          {
            foreach(var _event in _firstseason)_resultlist.Add(_event);

            if (_resultlist.Count.Equals(0))
            {
              if (_seasonfailed) break;
              _seasonfailed = true;
    //          Debug.Log($"���� ���� �̺�Ʈ�� ���������Ƿ� {_firstseason.Count}���� {_secondseason.Count}���� ���ؼ� �ٽ�");
              foreach (var _event in _secondseason)_firstseason.Add(_event);
            }//���� -> ȯ�� -> ����(����) ������� �ɷ��� ���°� �ϳ��� ���ٸ� ���� ������ �ѹ� ��
            else break;
          }//season�� ������ ����

          if (_resultlist.Count.Equals(0))
          {
            if (_envirfailed) break;
            _envirfailed = true;
     //       Debug.Log($"���� ȯ�� �̺�Ʈ�� ���������Ƿ� {_firstenvir.Count}���� {_secondenvir.Count}���� ���ؼ� �ٽ�");
            foreach (var _event in _secondenvir) _firstenvir.Add(_event);
          }//���� -> ȯ��(����) -> ����(����) ������� �ɷ��� ���°� �ϳ��� ���ٸ� ���� ȯ������ �ѹ� ��
          else break;
        }//envir�� ������ ����

        if (_resultlist.Count.Equals(0))
        {
          if (_levelfailed) break;
          _levelfailed = true;
     //     Debug.Log($"���� ���� �̺�Ʈ�� ���������Ƿ� {_firstlevels.Count}���� {_secondlevels.Count}���� ���ؼ� �ٽ�");
          foreach (var _event in _secondlevels) _firstlevels.Add(_event);
        }//����(����) -> ȯ��(����) -> ����(����) ������� �ɷ��� �ϳ��� ���°� ���ٸ� ���� ������ �ѹ� ��
        else break;
      }//level�� ������ ����

      string _str = $"�� ��ҿ��� ���� �� �ִ� �̺�Ʈ ";
      foreach(var _event in _list) { _str += $"{_event.ID} "; }
      _str+=$"�߿��� ������ �̺�Ʈ ������ {_resultlist.Count}";
    //  Debug.Log(_str);
      return _resultlist;
    }//�� ��ҿ� ���ϴ� �̺�Ʈ���� ��� ������ ȯ��, ������ �ɷ� List�� ��ȯ
    List<EventDataDefulat> CheckForResult(int _normalcount,int _followcount)
    {
      List<PlaceType> _normalplaces=new List<PlaceType>();  //��� ������ ���(�Ϲ�)
      List<PlaceType> _followplaces=new List<PlaceType>();  //��� ������ ���(����)'
      List<PlaceType> _tempplace=new List<PlaceType>();
      System.Random _rnd = new System.Random();
      foreach(var _place in _normalfirstplace) _tempplace.Add(_place);
      _normalplaces= _tempplace.OrderBy(a => _rnd.Next()).ToList();
      _tempplace.Clear();
      foreach (var _place in _followfirstplace) _tempplace.Add(_place);
      _followplaces = _tempplace.OrderBy(a => _rnd.Next()).ToList();
      //���� �������� �ִ� ��ҵ�θ� �˻�
      List<EventDataDefulat> _resultlist=new List<EventDataDefulat>();

      if (_normalplaces.Count < _normalcount || _followplaces.Count < _followcount) return null;
      //��� ������ �䱸�ϴ� �̺�Ʈ���� ������ �������� �Ұ����ѰŴ� �ƿ�

      bool _everfailed = false;
      if (_followcount.Equals(0))
      {
        foreach(var _place in _normalplaces)
        {
          if (_resultlist.Count.Equals(_normalcount)) break;
          if (_normalplaceevents.ContainsKey(_place))
          {
            List<EventDataDefulat> _temp=CheckSinglePlaceEvents(_normalplaceevents[_place]);
            _resultlist.Add(_temp[Random.Range(0,_temp.Count)]);
          }
        }

        if (_resultlist.Count < _normalcount)
        {
          if (_everfailed) return null;
          _everfailed = true;
          foreach (var _place in _normalsecondplace) _normalplaces.Add(_place);
        }
      }//follow�� 0�̴� => ��� �̺�Ʈ�� normal������ �̴°�
      else if (_normalcount.Equals(0))
      {
        foreach (var _place in _followplaces)
        {
          if (_resultlist.Count.Equals(_followcount)) break;
          if (_followplaceevents.ContainsKey(_place))
          {
            List<EventDataDefulat> _temp = CheckSinglePlaceEvents(_followplaceevents[_place]);
            _resultlist.Add(_temp[Random.Range(0, _temp.Count)]);
          }
        }

        if (_resultlist.Count < _followcount)
        {
          if (_everfailed) return null;
          _everfailed = true;
          foreach (var _place in _followsecondplace) _followplaces.Add(_place);
        }
      }//normal�� 0�̴� => ��� �̺�Ʈ�� follow������ �̴´�
      else
      {
        while (true)
        {
          _resultlist.Clear();
          List<List<PlaceType>> _normalmyuck = getplacetypebycount(_normalplaces, _normalcount);
          List<List<PlaceType>> _followmyuck = getplacetypebycount(_followplaces, _followcount);

          bool _issuccess = true;
          foreach (var _followlist in _followmyuck)
          {
            foreach(var _normallist in _normalmyuck)
            {
              _issuccess = true;
              foreach (var _normalplace in _normallist)
              {
                if (_followlist.Contains(_normalplace)) _issuccess = false;
              }

              if (_issuccess)
              {
                foreach(var _normalplace in _normallist)
                  if (_normalplaceevents.ContainsKey(_normalplace))
                  {
                    List<EventDataDefulat> _temp=CheckSinglePlaceEvents(_normalplaceevents[_normalplace]);
                    _resultlist.Add(_temp[Random.Range(0,_temp.Count)]);
                  }//�� ��Һ��� ���� �̺�Ʈ���� ������ �� �� �ϳ��� result�� ����
                foreach(var _followplace in _followlist)
                {
                  if(_followplaceevents.ContainsKey(_followplace))
                  {
                    List<EventDataDefulat> _temp = CheckSinglePlaceEvents(_followplaceevents[_followplace]);
                    _resultlist.Add(_temp[Random.Range(0, _temp.Count)]);
                  }//�� ��Һ��� ���� �̺�Ʈ���� ������ �� �� �ϳ��� result�� ����
                }
                break;
              }//�ϳ��� ��ģ�� ���� �����߸� �� ��ҵ��� �������� �̺�Ʈ ��������
            }
            if (_issuccess) break;
          }

          if (_resultlist.Count < _normalcount + _followcount)
          {
            if (_everfailed) return null;
            _everfailed = true;
            foreach (var _place in _normalsecondplace) _normalplaces.Add(_place);
            foreach (var _place in _followsecondplace) _followplaces.Add(_place);
          }//���� ���� �� ���� ��� ����,�Ϲ� �� �� ���� ��ұ��� �����ؼ� ��˻�, �� �����ϸ� null ��ȯ
          break;
        }
      }//�� �ܸ� �ΰ��� ȥ���ؼ� �̴´�

     // Debug.Log($"normalcount : {_normalcount} followcount : {_followcount} resultlist.count : {_resultlist.Count}");
      return _resultlist;
      List<List<PlaceType>> getplacetypebycount(List<PlaceType> _placelist,int _size)
      {
        List<List<int>> _temp = getmyuck(_placelist.Count, 0, _size, 0);
        System.Random _rnd = new System.Random();
        List<List<int>> _randomindex =_temp==null?new List<List<int>>(): _temp.OrderBy(a => _rnd.Next()).ToList();
        List<List<PlaceType>> _result=new List<List<PlaceType>>();
        foreach(var _list in _randomindex)
        {
          List<PlaceType> _place = new List<PlaceType>();
          foreach(var _index in _list)_place.Add(_placelist[_index]);
          _result.Add(_place);
        }
        return _result;

        List<List<int>> getmyuck(int _listcount, int _currentlistindex, int _size, int _currentsizeindex)
        {
          if (_listcount.Equals(0)) { return null; }
          if (_currentsizeindex.Equals(_size - 1))
          {
            List<List<int>> _temp = new List<List<int>>();
            for (int i = _currentlistindex; i < _listcount; i++)
            {
              List<int> __temp = new List<int>();
              __temp.Add(i);
              _temp.Add(__temp);
            }
            return _temp;
          }//myindex==size-1�̸� �� �� ��Ҵϱ� ���� ��ġ���� ������ ��ȯ
          else
          {
            List<List<int>> _temp = new List<List<int>>();  //��ȯ�� ����Ʈ
            for (int i = _currentlistindex; i < _listcount - (_size - _currentsizeindex) + 1; i++)
            {
              List<List<int>> _temptemp = new List<List<int>>();//�̹� �ݺ����� �޾ƿ� ���� i�� ������ ����Ʈ
              _temptemp = getmyuck(_listcount, i + 1, _size, _currentsizeindex + 1);

              foreach (var _list in _temptemp)
              {
                List<int> _lehoo = new List<int>();
                _lehoo.Add(i);
                foreach (var __list in _list) { _lehoo.Add(__list); }
                _temp.Add(_lehoo);
              }//�޾ƿ� ����Ʈ�� i�� ����
            }
            return _temp;
          }//�� �ܶ�� myindex ���� ��Ҹ� �޾ƿͼ� ���� listindex�� ���� ��ȯ
        }
      }//_size ũ�� ������ ����(������ ������)
    }//normallist�� followlist�� Ȱ���� normalcount,followcount ������ �°� ���ļ� ����Ʈ ��ȯ
  }
}
public class TargetTileEventData
{
  public SettlementType SettlementType; //������ Ÿ��
  public Dictionary<PlaceType, int> PlaceData = new Dictionary<PlaceType, int>(); //(�������� ���) ��� Ÿ�԰� ��� ����
  public List<EnvironmentType> EnvironmentType = new List<EnvironmentType>();//���� ȯ�� Ÿ��
  public int Season;
}
#region �̺�Ʈ ������ ���� �迭��
public enum FollowType { Event,EXP,Trait,Theme,Skill}
public enum SettlementType { Town,City,Castle,Outer,None}
public enum PlaceType { Residence,Marketplace,Temple,Library,Theater,Academy}
public enum EnvironmentType { None,River,Forest,Highland,Mountain,Sea }
public enum SelectionType { Single,Verticla, Horizontal,Tendency,Experience,Skill }//Horizontal : �� ���� �� ����     Vertical : �� �̼� �Ʒ� ��ü
public enum CheckTarget { None,Pay,Theme,Skill}
public enum PenaltyTarget { None,Status,EXP }
public enum RewardTarget { Experience,GoldAndExperience,Gold,HP,Sanity,Theme,Skill,Trait}
public enum EventSequence { Progress,Clear}//Suggest: 3�� �����ϴ� �ܰ�  Progress: ������ ��ư ������ �ϴ� �ܰ�  Clear: ���� �����ؾ� �ϴ� �ܰ�
public enum QuestSequence { Start,Rising,Climax,Falling}
#endregion
public class EventDataDefulat
{
  public string ID = "";        //���� ���� ��ȭ�� ID(ID_(����))
  public string IllustID = "";  //�Ⱦ�
  public Sprite Illust
  {
    get 
    { 
        return GameManager.Instance.ImageHolder.GetEventIllust(ID);
    }
  }
  public string Name = "";
  public string Description = "";
  public int Season = 0;  //0: ����  1,2,3,4
  public SettlementType SettlementType;
  public PlaceType PlaceType;
  public int PlaceLevel = 0;  //0:���� 1,2,3
  public EnvironmentType EnvironmentType = EnvironmentType.None;

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
public class SelectionData
{
    public SelectionTargetType ThisSelectionType = SelectionTargetType.None;
    public string SubDescription = "";
    public PayTarget SelectionPayTarget = PayTarget.HP;
    public ThemeType SelectionCheckTheme = ThemeType.Conversation;
    public SkillName SelectionCheckSkill = SkillName.Speech;
}
public enum SelectionTargetType { None, Pay, Check_Theme, Check_Skill, Exp, Skill, Tendency }//������ ���� ����
public enum PayTarget { HP,Sanity,Gold}
public class EventData:EventDataDefulat
{}//�⺻ �̺�Ʈ
public class FollowEventData:EventDataDefulat
{
  public FollowType FollowType = 0;
  public string FollowTarget = "";
  public int FollowTargetLevel = 0;
  public bool FollowTargetSuccess = false;
  public int FollowTendency = 0;          //�̺�Ʈ�� ��� ��Ÿ,�̼�,��ü,����,���� ������ ����
}//���� �̺�Ʈ
public class QuestEventData : EventDataDefulat
{
  public string QuestID = "";
  public QuestSequence TargetQuestSequence= QuestSequence.Start;
  public int ClimaxIndex = 0;
}
public class QuestHolder
{
    public string QuestID = "";     //����Ʈ ID
  public string QuestName = "";     //����Ʈ�� �̸�

  public QuestSequence CurrentSequence=QuestSequence.Start; //���� ����Ʈ �ܰ�
  public int CurrentClimaxIndex = 0;  //���� ����Ʈ �ܰ谡 Climax�� ��� �� �̺�Ʈ�� ���� ����

  public string StartDialogue = "";   //���� ���ο��� ����Ʈ ������ �� ������ �ؽ�Ʈ
  public string PreDescription = "";  //���� ȭ�� �̸����� �ؽ�Ʈ
  public string StartIllustID = "";   //��ǥ �Ϸ���Ʈ

  public List<QuestEventData> Eventlist_Rising=new List<QuestEventData>();
  public List<QuestEventData> Eventlist_Climax = new List<QuestEventData>();
  public QuestEventData Event_Falling = null;
}
public class EventJsonData
{
  public string ID = "";              //ID
  public int Settlement = 0;          //0,1,2,3
  public int Place = 0;               //0,1,2,3,4
    public int TileCondition = 0;       //0: ���Ǿ���  1: ȯ��  2: ����
    public int TileCondition_info = 0;  //ȯ�� : ��,��,���,��,�ٴ�      ����: 0,1,2
  public int Season = 0;              //����,��,����,����,�ܿ�,�����

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
public class FollowEventJsonData
{
  public string ID = "";              //ID
  public int FollowType = 0;              //�̺�Ʈ,����,Ư��,�׸�,���
  public string FollowTarget = "";            //�ش� ID Ȥ�� 0,1,2,3 Ȥ�� 0~9
  public int FollowTargetSuccess = 0;            //(�̺�Ʈ) ����/����
  public int FollowTendency = 0;          //�̺�Ʈ�� ��� ��Ÿ,�̼�,��ü,����,���� ������ ����

  public int Season = 0;              //����,��,����,����,�ܿ�
  public int Settlement = 0;          //0,1,2,3
  public int Place = 0;               //0,1,2,3,4

  public int Selection_Type;           //0.���� 1.�̼�+��ü 2.����+���� 3.���� 4.���� 5.���
  public string Selection_Description = ""; //������ �� �ؽ�Ʈ
  public string Selection_Target;           //0.������ 1.���� 2.�׸� 3.���
  public string Selection_Info;             //0:���� ����  1:ü��,���ŷ�,��
                                            //2:��ȭ,����,����,����
                                            //3: 0.���� 1.����  2.�⸸  3.�� 4.���� 5.Ȱ�� 6.��ü 7.���� 8.���� 9.����
    public int TileCondition = 0;       //0: ���Ǿ���  1: ȯ��  2: ����
    public int TileCondition_info = 0;  //ȯ�� : ��,��,���,��,�ٴ�      ����: 0,1,2

  public string Failure_Penalty;            //����,�ս�,����
  public string Failure_Penalty_info;       //(ü��,���ŷ�,��),���� ID

  public string Reward_Target;              //����,ü��,���ŷ�,��,���-�׸�,���-����,Ư��
  public string Reward_Info;                //���� :ID  ü��,���ŷ�,��:X  �׸�:��ȭ,����,����,�н�  �������:�� ����  Ư��:ID

  public string SubReward;                  //����,��,���ŷ�,��+���ŷ�
}
public class QuestEventDataJson
{
  public string QuestId = "";                 //����Ʈ ID
  public string ID = "";
  public int Sequence = 0;                   //0:��  1:��   2:��   3:��

  public string IllustID = "";
  public string Name = "";              //�̸�
  public int Settlement = 0;          //0(�ƹ� ������),1,2,3,4(�ܺ�)
  public int Place = 0;               //0,1,2,3,4
  public int Environment_Type = 0;    //0:���� 1:�� 2:�� 3:��� 4:�� 5:�ٴ�

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

