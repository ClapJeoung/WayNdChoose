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
    Data.FollowType = (FollowType)_data.FollowType; //선행 대상 이벤트,경험,특성,테마,기술
    Data.FollowTarget = _data.FollowTarget;         //선행 대상- 이벤트,경험,특성이면 Id   테마면 0,1,2,3  기술이면 0~9
    if (Data.FollowType == FollowType.Event)
    {
      Data.FollowTargetSuccess = _data.FollowTargetSuccess == 0 ? true : false;//선행 대상이 이벤트일 경우 성공 혹은 실패
      Data.FollowTendency = _data.FollowTendency;                              //선행 대상이 이벤트일 경우 선택지 형식
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
        case 0: Data.SettlementType = SettlementType.None; break;//0: 아무 정착지
        case 1: Data.SettlementType = SettlementType.Town; break;//1: 마을
        case 2: Data.SettlementType = SettlementType.City; break;  //2: 도시
        case 3: Data.SettlementType = SettlementType.Castle; break; //3: 성채
        case 4: Data.SettlementType = SettlementType.Outer; break;//4: 외부
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
    }//딕셔너리에 퀘스트가 이미 만들어졌을 경우
    else
    {
      _quest = new QuestHolder();
    }//딕셔너리에 퀘스트가 없을 경우 새로 하나 만들기

    switch (_data.Sequence)
    {
      case 0://기
                _quest.QuestID = _data.QuestId;
        _quest.QuestName = _data.Name;
        _quest.StartDialogue = _data.Description;
        _quest.PreDescription = _data.PreDescription;
        _quest.StartIllustID = _data.IllustID;
        break;
      case 1://승
        _quest.Eventlist_Rising.Add(Data);
        break;
      case 2://전
        _quest.Eventlist_Climax.Add(Data);
        break;
      case 3://결
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
  }//Gamemanager.instance.GameData를 기반으로 이미 클리어한 이벤트 빼고 다 활성화 리스트에 넣기
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
      QuestHolder _currentquest = GameManager.Instance.MyGameData.CurrentQuest; //현재 활성화된 퀘스트
      switch (_currentquest.CurrentSequence)
      {
        case QuestSequence.Rising:  //'승' 단계 이벤트를 해야 할 때

          List<EventDataDefulat> _temptemp = new List<EventDataDefulat>();
          foreach (var _risingevent in _currentquest.Eventlist_Rising)
          {
            if (SimpleCheck(_risingevent))_temp.Add(_risingevent);    //간단히 검사해서 _temp에 추가
          }

          break;
        case QuestSequence.Climax:  //'전' 단계 이벤트를 해야 할 때

          EventDataDefulat _climaxevent = _currentquest.Eventlist_Climax[_currentquest.CurrentClimaxIndex];
          if (SimpleCheck(_climaxevent)) _temp.Add(_climaxevent);    //간단히 검사해서 _temp에 추가

          break;
        case QuestSequence.Falling: //마지막 이벤트를 해야 할 때

          EventDataDefulat _falling = _currentquest.Event_Falling;
          if (SimpleCheck(_falling)) _temp.Add(_falling);    //간단히 검사해서 _temp에 추가

          break;
      }

      foreach (var _event in _temp) _ResultEvents.Add(_event);
      _temp.Clear();

    }//현재 보유 중인 퀘스트가 있다면 채우기

    if (_tiledata.SettlementType.Equals(SettlementType.Outer) && _ResultEvents.Count.Equals(1)) return _ResultEvents;
    //야외 이벤트라면, 하나 채워지자마자 바로 반환
  //  Debug.Log($"가능한 퀘스트 이벤트 {_ResultEvents.Count}개");

    //가능한 연계 이벤트 모두 가져오기
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
        case FollowType.EXP://경험 연계일 경우 현재 보유한 경험 ID랑 맞는지 확인
          foreach(var _data in GameManager.Instance.MyGameData.LongTermEXP)
            if(_follow.FollowTarget.Equals(_data.ID))_temp.Add(_follow);
          foreach (var _data in GameManager.Instance.MyGameData.ShortTempEXP)
            if (_follow.FollowTarget.Equals(_data.ID)) _temp.Add(_follow);
          break;
        case FollowType.Trait://특성 연계일 경우 현재 보유한 특성 ID랑 맞는지 확인
          foreach (var _data in GameManager.Instance.MyGameData.Traits)
            if (_follow.FollowTarget.Equals(_data.ID)) _temp.Add(_follow);
          break;
        case FollowType.Theme://테마 연계일 경우 현재 테마의 레벨이 기준 이상인지 확인
          int _targetlevel = 0;
                    ThemeType _type = ThemeType.Conversation; ;
          switch (_follow.FollowTarget)
          {
            case "0"://대화 테마
                            _type = ThemeType.Conversation;  break;
            case "1"://무력 테마
 _type = ThemeType.Force;break;
            case "2"://생존 테마
                            _type = ThemeType.Nature;break;
            case "3"://학식 테마
                            _type = ThemeType.Intelligence;break;
          }
                    _targetlevel = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_type) + GameManager.Instance.MyGameData.GetEffectThemeCount_Trait(_type) +
                                GameManager.Instance.MyGameData.GetEffectThemeCount_Exp(_type) + GameManager.Instance.MyGameData.GetThemeLevelByTendency(_type);
                    if (_follow.FollowTargetLevel <= _targetlevel) _temp.Add(_follow);
          break;
        case FollowType.Skill://기술 연계일 경우 현재 기술의 레벨이 기준 이상인지 확인
          SkillName _skill = (SkillName)int.Parse(_follow.FollowTarget);
          if(_follow.FollowTargetLevel<= GameManager.Instance.MyGameData.Skills[_skill].Level)_temp.Add(_follow);
          break;
      }
    }
    foreach (var _event in _temp)
    {
      if (SimpleCheck(_event)) _followevents.Add(_event);
    }
  //  Debug.Log($"가능한 연계 이벤트 {_followevents.Count}개");
    if (_tiledata.SettlementType.Equals(SettlementType.Outer) && _followevents.Count > 0)
    {
      _ResultEvents.Add(_followevents[Random.Range(0, _followevents.Count)]);
      return _ResultEvents;
    }//야외 이벤트고 퀘스트 이벤트가 아무것도 없으면 이거 하나 채우고 바로 반환


    //가능한 일반 이벤트 모두 가져오기
    _temp.Clear();
    foreach(var _event in AvailableNormalEvents)
    {
      if(SimpleCheck(_event)) _normalevents.Add(_event);
    }
    string _str = $"가능한 일반 이벤트 {_normalevents.Count}개\n";
    foreach(var _event in _normalevents)
    {
      _str += $"{_event.Name}   ";
    }
   // Debug.Log(_str);
    if (_tiledata.SettlementType.Equals(SettlementType.Outer) && _followevents.Count.Equals(0) && _normalevents.Count > 0)
    {
      _ResultEvents.Add(_normalevents[Random.Range(0, _normalevents.Count)]);
      return _ResultEvents;
    }//야외 이벤트고 퀘스트,연계 둘 다 없으면 이거 하나 채우고 반환

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
    //일반,연계 이벤트를 다시 장소별로 분할
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
      if (_otherevents==null||_otherevents.Count.Equals(0)) Debug.Log("테챠아앗");
    }//퀘스트 이벤트가 하나 채워져 있을 경우
    else if (_ResultEvents.Count.Equals(0))
    {
      _otherevents = CheckForResult(2, 1);
      if (_otherevents==null||_otherevents.Count.Equals(0)) _otherevents = CheckForResult(1, 2);
      if (_otherevents==null||_otherevents.Count.Equals(0)) _otherevents = CheckForResult(0, 3);
      if (_otherevents==null||_otherevents.Count.Equals(0)) _otherevents = CheckForResult(3, 0);
      if (_otherevents==null||_otherevents.Count.Equals(0)) Debug.Log("데갸아앗");
    }//퀘스트 이벤트 없이 빈 칸만 3개일 경우

    foreach (var _event in _otherevents) _ResultEvents.Add(_event);
    return _ResultEvents;

    bool SimpleCheck(EventDataDefulat _data)
    {
      if (checkbysettle(_data) == true &&                              //정착지 종류가 맞고
       _tiledata.PlaceData.ContainsKey(_data.PlaceType) &&         //사용할 수 있는 장소고
       (_data.PlaceLevel.Equals(0)||_data.PlaceLevel.Equals(_tiledata.PlaceData[_data.PlaceType]))&&//이벤트의 장소 레벨이 0이거나 장소 레벨이 같고
       _tiledata.EnvironmentType.Contains(_data.EnvironmentType)) //현재 환경에 맞다면
        return true;  //true 반환
      return false;   //정착지 종류가 다르거나, 쓸 수 있는 장소가 아니거나, 장소 레벨이 아예 다르거나, 환경이 완전히 아니라면 false

      bool checkbysettle(EventDataDefulat _data)
      {
        if (_data.SettlementType.Equals(SettlementType.Outer))
          if (_tiledata.SettlementType.Equals(SettlementType.Outer)) return true;//야외 이벤트면 야외 장소만 통과

        if (_data.SettlementType.Equals(SettlementType.None))
        {
          if (_tiledata.SettlementType.Equals(SettlementType.Town) ||
              _tiledata.SettlementType.Equals(SettlementType.City) ||
              _tiledata.SettlementType.Equals(SettlementType.Castle)) return true;//전역 이벤트라면 마을,도시,성채를 통과
        }

        if (_data.SettlementType.Equals(_tiledata.SettlementType)) return true;//그 외는 해당하는 정착지만 받는다
        return false;
      }

    }
    List<EventDataDefulat> CheckSinglePlaceEvents(List<EventDataDefulat> _list)
    {
      if (_list.Count.Equals(0)) return _list;  //0개짜리는 검사 안함

      List<EventDataDefulat> _resultlist=new List<EventDataDefulat>();
      List<EventDataDefulat> _templist = new List<EventDataDefulat>();   //검사 결과를 넣는 임시 리스트

      bool _levelfailed = false;
      //_list에서 장소 레벨에 따라 거르는 단계
      List<EventDataDefulat> _firstlevels = new List<EventDataDefulat>(); //해당 장소의 레벨에 맞는 이벤트를 넣는 리스트
      List<EventDataDefulat> _secondlevels = new List<EventDataDefulat>();//장소 레벨이 0(전역)인 이벤트를 넣는 리스트
      foreach (var _event in _list)
      {
        if (_event.PlaceLevel.Equals(0)) _secondlevels.Add(_event);
        else if (_event.PlaceLevel.Equals(_tiledata.PlaceData[_event.PlaceType])) _firstlevels.Add(_event);
      }
      //   Debug.Log($"시작 데이터(전체) {_list.Count}개가 레벨에 따라 {_firstlevels.Count}/{_secondlevels.Count}로 분화");
      while (true)
      {
        bool _envirfailed = false;
        //_firstlevel에서 환경에 따라 거르는 단계
        List<EventDataDefulat> _firstenvir = new List<EventDataDefulat>();
        List<EventDataDefulat> _secondenvir = new List<EventDataDefulat>();
        foreach (var _event in _firstlevels)
        {
          if (_event.EnvironmentType.Equals(EnvironmentType.None)) _secondenvir.Add(_event);
          else if (_tiledata.EnvironmentType.Contains(_event.EnvironmentType)) _firstenvir.Add(_event);
        }
      //  Debug.Log($"시작 데이터(레벨) {_firstlevels.Count}개가 환경에 따라 {_firstenvir.Count}/{_secondenvir.Count}로 분화");
        while (true)
        {
          bool _seasonfailed = false;
          //_firstenvir에서 계절에 따라 거르는 단계
          List<EventDataDefulat> _firstseason=new List<EventDataDefulat>();
          List<EventDataDefulat> _secondseason=new List<EventDataDefulat>();
          foreach(var _event in _firstenvir)
          {
            if (_event.Season.Equals(0)) _secondseason.Add(_event);
            else if(_event.Season.Equals(_tiledata.Season)) _firstseason.Add(_event);
          }
       //   Debug.Log($"시작 데이터(환경) {_firstenvir.Count}개가 계절에 따라 {_firstseason.Count}/{_secondseason.Count}로 분화");
          while (true)
          {
            foreach(var _event in _firstseason)_resultlist.Add(_event);

            if (_resultlist.Count.Equals(0))
            {
              if (_seasonfailed) break;
              _seasonfailed = true;
    //          Debug.Log($"지역 계절 이벤트가 부족했으므로 {_firstseason.Count}개에 {_secondseason.Count}개를 더해서 다시");
              foreach (var _event in _secondseason)_firstseason.Add(_event);
            }//레벨 -> 환경 -> 계절(지역) 순서대로 걸러서 나온게 하나도 없다면 전역 계절로 한번 더
            else break;
          }//season이 주제인 루프

          if (_resultlist.Count.Equals(0))
          {
            if (_envirfailed) break;
            _envirfailed = true;
     //       Debug.Log($"지역 환경 이벤트가 부족했으므로 {_firstenvir.Count}개에 {_secondenvir.Count}개를 더해서 다시");
            foreach (var _event in _secondenvir) _firstenvir.Add(_event);
          }//레벨 -> 환경(지역) -> 계절(전역) 순서대로 걸러서 나온게 하나도 없다면 전역 환경으로 한번 더
          else break;
        }//envir이 주제인 루프

        if (_resultlist.Count.Equals(0))
        {
          if (_levelfailed) break;
          _levelfailed = true;
     //     Debug.Log($"지역 레벨 이벤트가 부족했으므로 {_firstlevels.Count}개에 {_secondlevels.Count}개를 더해서 다시");
          foreach (var _event in _secondlevels) _firstlevels.Add(_event);
        }//레벨(지역) -> 환경(전역) -> 계절(전역) 순서대로 걸러서 하나도 나온게 없다면 전역 레벨로 한번 더
        else break;
      }//level이 주제인 루프

      string _str = $"이 장소에서 나올 수 있는 이벤트 ";
      foreach(var _event in _list) { _str += $"{_event.ID} "; }
      _str+=$"중에서 최적의 이벤트 개수는 {_resultlist.Count}";
    //  Debug.Log(_str);
      return _resultlist;
    }//한 장소에 속하는 이벤트들을 장소 레벨과 환경, 계절로 걸러 List로 반환
    List<EventDataDefulat> CheckForResult(int _normalcount,int _followcount)
    {
      List<PlaceType> _normalplaces=new List<PlaceType>();  //사용 가능한 장소(일반)
      List<PlaceType> _followplaces=new List<PlaceType>();  //사용 가능한 장소(연계)'
      List<PlaceType> _tempplace=new List<PlaceType>();
      System.Random _rnd = new System.Random();
      foreach(var _place in _normalfirstplace) _tempplace.Add(_place);
      _normalplaces= _tempplace.OrderBy(a => _rnd.Next()).ToList();
      _tempplace.Clear();
      foreach (var _place in _followfirstplace) _tempplace.Add(_place);
      _followplaces = _tempplace.OrderBy(a => _rnd.Next()).ToList();
      //먼저 선순위에 있는 장소들로만 검사
      List<EventDataDefulat> _resultlist=new List<EventDataDefulat>();

      if (_normalplaces.Count < _normalcount || _followplaces.Count < _followcount) return null;
      //장소 개수가 요구하는 이벤트보다 적으면 보나마나 불가능한거니 아웃

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
      }//follow가 0이다 => 모든 이벤트를 normal에서만 뽑는가
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
      }//normal이 0이다 => 모든 이벤트를 follow에서만 뽑는다
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
                  }//각 장소별로 최적 이벤트들을 가져와 그 중 하나씩 result에 전달
                foreach(var _followplace in _followlist)
                {
                  if(_followplaceevents.ContainsKey(_followplace))
                  {
                    List<EventDataDefulat> _temp = CheckSinglePlaceEvents(_followplaceevents[_followplace]);
                    _resultlist.Add(_temp[Random.Range(0, _temp.Count)]);
                  }//각 장소별로 최적 이벤트들을 가져와 그 중 하나씩 result에 전달
                }
                break;
              }//하나도 겹친거 없이 성공했면 이 장소들을 바탕으로 이벤트 가져오기
            }
            if (_issuccess) break;
          }

          if (_resultlist.Count < _normalcount + _followcount)
          {
            if (_everfailed) return null;
            _everfailed = true;
            foreach (var _place in _normalsecondplace) _normalplaces.Add(_place);
            foreach (var _place in _followsecondplace) _followplaces.Add(_place);
          }//개수 충족 못 했을 경우 연계,일반 둘 다 이전 장소까지 포함해서 재검사, 또 실패하면 null 반환
          break;
        }
      }//그 외면 두개를 혼합해서 뽑는다

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
          }//myindex==size-1이면 맨 끝 요소니까 현재 위치부터 끝까지 반환
          else
          {
            List<List<int>> _temp = new List<List<int>>();  //반환할 리스트
            for (int i = _currentlistindex; i < _listcount - (_size - _currentsizeindex) + 1; i++)
            {
              List<List<int>> _temptemp = new List<List<int>>();//이번 반복에서 받아와 현재 i랑 융합할 리스트
              _temptemp = getmyuck(_listcount, i + 1, _size, _currentsizeindex + 1);

              foreach (var _list in _temptemp)
              {
                List<int> _lehoo = new List<int>();
                _lehoo.Add(i);
                foreach (var __list in _list) { _lehoo.Add(__list); }
                _temp.Add(_lehoo);
              }//받아온 리스트와 i를 붙임
            }
            return _temp;
          }//그 외라면 myindex 다음 요소를 받아와서 현재 listindex와 더해 반환
        }
      }//_size 크기 멱집합 산출(순서는 무작위)
    }//normallist랑 followlist를 활용해 normalcount,followcount 개수에 맞게 합쳐서 리스트 반환
  }
}
public class TargetTileEventData
{
  public SettlementType SettlementType; //정착지 타입
  public Dictionary<PlaceType, int> PlaceData = new Dictionary<PlaceType, int>(); //(정착지일 경우) 장소 타입과 장소 레벨
  public List<EnvironmentType> EnvironmentType = new List<EnvironmentType>();//주위 환경 타입
  public int Season;
}
#region 이벤트 정보에 쓰는 배열들
public enum FollowType { Event,EXP,Trait,Theme,Skill}
public enum SettlementType { Town,City,Castle,Outer,None}
public enum PlaceType { Residence,Marketplace,Temple,Library,Theater,Academy}
public enum EnvironmentType { None,River,Forest,Highland,Mountain,Sea }
public enum SelectionType { Single,Verticla, Horizontal,Tendency,Experience,Skill }//Horizontal : 좌 물질 우 정신     Vertical : 위 이성 아래 육체
public enum CheckTarget { None,Pay,Theme,Skill}
public enum PenaltyTarget { None,Status,EXP }
public enum RewardTarget { Experience,GoldAndExperience,Gold,HP,Sanity,Theme,Skill,Trait}
public enum EventSequence { Progress,Clear}//Suggest: 3개 제시하는 단계  Progress: 선택지 버튼 눌러야 하는 단계  Clear: 보상 수령해야 하는 단계
public enum QuestSequence { Start,Rising,Climax,Falling}
#endregion
public class EventDataDefulat
{
  public string ID = "";        //계절 별로 분화된 ID(ID_(계절))
  public string IllustID = "";  //안씀
  public Sprite Illust
  {
    get 
    { 
        return GameManager.Instance.ImageHolder.GetEventIllust(ID);
    }
  }
  public string Name = "";
  public string Description = "";
  public int Season = 0;  //0: 전역  1,2,3,4
  public SettlementType SettlementType;
  public PlaceType PlaceType;
  public int PlaceLevel = 0;  //0:전역 1,2,3
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
public enum SelectionTargetType { None, Pay, Check_Theme, Check_Skill, Exp, Skill, Tendency }//선택지 개별 내용
public enum PayTarget { HP,Sanity,Gold}
public class EventData:EventDataDefulat
{}//기본 이벤트
public class FollowEventData:EventDataDefulat
{
  public FollowType FollowType = 0;
  public string FollowTarget = "";
  public int FollowTargetLevel = 0;
  public bool FollowTargetSuccess = false;
  public int FollowTendency = 0;          //이벤트일 경우 기타,이성,육체,정신,물질 선택지 여부
}//연계 이벤트
public class QuestEventData : EventDataDefulat
{
  public string QuestID = "";
  public QuestSequence TargetQuestSequence= QuestSequence.Start;
  public int ClimaxIndex = 0;
}
public class QuestHolder
{
    public string QuestID = "";     //퀘스트 ID
  public string QuestName = "";     //퀘스트의 이름

  public QuestSequence CurrentSequence=QuestSequence.Start; //현재 퀘스트 단계
  public int CurrentClimaxIndex = 0;  //현재 퀘스트 단계가 Climax일 경우 그 이벤트의 진행 순서

  public string StartDialogue = "";   //게임 내부에서 퀘스트 조우할 시 나오는 텍스트
  public string PreDescription = "";  //시작 화면 미리보기 텍스트
  public string StartIllustID = "";   //대표 일러스트

  public List<QuestEventData> Eventlist_Rising=new List<QuestEventData>();
  public List<QuestEventData> Eventlist_Climax = new List<QuestEventData>();
  public QuestEventData Event_Falling = null;
}
public class EventJsonData
{
  public string ID = "";              //ID
  public int Settlement = 0;          //0,1,2,3
  public int Place = 0;               //0,1,2,3,4
    public int TileCondition = 0;       //0: 조건없음  1: 환경  2: 레벨
    public int TileCondition_info = 0;  //환경 : 숲,강,언덕,산,바다      레벨: 0,1,2
  public int Season = 0;              //전역,봄,여름,가을,겨울,사계절

  public int Selection_Type;           //0.단일 1.이성+육체 2.정신+물질 3.성향 4.경험 5.기술
  public string Selection_Target;           //0.무조건 1.지불 2.테마 3.기술
  public string Selection_Info;             //0:정보 없음  1:체력,정신력,돈
                                            //2:대화,무력,생존,정신
                                            //3: 0.설득 1.협박  2.기만  3.논리 4.격투 5.활술 6.인체 7.생존 8.생물 9.잡학

  public string Failure_Penalty;            //없음,손실,경험
  public string Failure_Penalty_info;       //(체력,정신력,돈),경험 ID

  public string Reward_Target;              //경험,체력,정신력,돈,기술-테마,기술-개별,특성
  public string Reward_Info;                //경험 :ID  체력,정신력,돈:X  테마:대화,무력,생존,학식  개별기술:위 참조  특성:ID

  public string SubReward;                  //없음,돈,정신력,돈+정신력
}
public class FollowEventJsonData
{
  public string ID = "";              //ID
  public int FollowType = 0;              //이벤트,경험,특성,테마,기술
  public string FollowTarget = "";            //해당 ID 혹은 0,1,2,3 혹은 0~9
  public int FollowTargetSuccess = 0;            //(이벤트) 성공/실패
  public int FollowTendency = 0;          //이벤트일 경우 기타,이성,육체,정신,물질 선택지 여부

  public int Season = 0;              //전역,봄,여름,가을,겨울
  public int Settlement = 0;          //0,1,2,3
  public int Place = 0;               //0,1,2,3,4

  public int Selection_Type;           //0.단일 1.이성+육체 2.정신+물질 3.성향 4.경험 5.기술
  public string Selection_Description = ""; //선택지 별 텍스트
  public string Selection_Target;           //0.무조건 1.지불 2.테마 3.기술
  public string Selection_Info;             //0:정보 없음  1:체력,정신력,돈
                                            //2:대화,무력,생존,정신
                                            //3: 0.설득 1.협박  2.기만  3.논리 4.격투 5.활술 6.인체 7.생존 8.생물 9.잡학
    public int TileCondition = 0;       //0: 조건없음  1: 환경  2: 레벨
    public int TileCondition_info = 0;  //환경 : 숲,강,언덕,산,바다      레벨: 0,1,2

  public string Failure_Penalty;            //없음,손실,경험
  public string Failure_Penalty_info;       //(체력,정신력,돈),경험 ID

  public string Reward_Target;              //경험,체력,정신력,돈,기술-테마,기술-개별,특성
  public string Reward_Info;                //경험 :ID  체력,정신력,돈:X  테마:대화,무력,생존,학식  개별기술:위 참조  특성:ID

  public string SubReward;                  //없음,돈,정신력,돈+정신력
}
public class QuestEventDataJson
{
  public string QuestId = "";                 //퀘스트 ID
  public string ID = "";
  public int Sequence = 0;                   //0:기  1:승   2:전   3:결

  public string IllustID = "";
  public string Name = "";              //이름
  public int Settlement = 0;          //0(아무 정착지),1,2,3,4(외부)
  public int Place = 0;               //0,1,2,3,4
  public int Environment_Type = 0;    //0:전역 1:숲 2:강 3:언덕 4:산 5:바다

  public int Selection_Type;           //0.단일 1.이성+육체 2.정신+물질 3.성향 4.경험 5.기술
  public string Selection_Target;           //0.무조건 1.지불 2.테마 3.기술
  public string Selection_Info;             //0:정보 없음  1:체력,정신력,돈
                                            //2:대화,무력,생존,정신
                                            //3: 0.설득 1.협박  2.기만  3.논리 4.격투 5.활술 6.인체 7.생존 8.생물 9.잡학

  public string Failure_Penalty;            //없음,손실,경험
  public string Failure_Penalty_info;       //(체력,정신력,돈),경험 ID

  public string Reward_Target;              //경험,체력,정신력,돈,기술-테마,기술-개별,특성
  public string Reward_Info;                //경험 :ID  체력,정신력,돈:X  테마:대화,무력,생존,학식  개별기술:위 참조  특성:ID

  public string SubReward;                  //없음,돈,정신력,돈+정신력
}

