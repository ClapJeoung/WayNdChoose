using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  private static GameManager instance;
  public static GameManager Instance { get { return instance; } }

  [HideInInspector] public GameData MyGameData = new GameData();            //게임 데이터(진행도,현재 진행 중 이벤트, 현재 맵 상태,퀘스트 등등)
  private const string GameDataName = "GameData.json";
  [HideInInspector] public MapData MyMapData = null;              //맵 데이터(맵 정보만)
  [HideInInspector] public MapSaveData MyMapSaveData = null;
  private const string MapDataName = "MapData.json";
  [HideInInspector] public ProgressData MyProgressData = new ProgressData();
  private const string ProgressDataName = "ProgressData.json";

  public ImageHolder ImageHolder = null;             //이벤트,경험,특성,정착지 일러스트 홀더

  [SerializeField] private TextAsset NormalEventData = null;  //이벤트 Json
  [SerializeField] private TextAsset FollowEventData = null;  //연계 이벤트 Json
  [SerializeField] private TextAsset QuestEventData = null;   //퀘스트 이벤트 Json
  [SerializeField] private TextAsset EXPData = null;    //경험 Json
  [SerializeField] private TextAsset TextData = null;
  public EventHolder EventHolder = new EventHolder();                               //이벤트 저장할 홀더
  public Dictionary<string, Experience> ExpDic = new Dictionary<string, Experience>();  //경험 딕셔너리
  public Dictionary<string, Experience> MadExpDic = new Dictionary<string, Experience>();
  public Dictionary<string, TextData> TextDic = new Dictionary<string, TextData>();   //각종 텍스트 딕셔터리
  public TextData NullText = null;
  public TextData GetTextData(string _id)
  {
    // Debug.Log($"{_id} ID를 가진 텍스트 데이터 {(TextDic.ContainsKey(_id)?"있음":"없음")}");
    if (!TextDic.ContainsKey(_id)) { Debug.Log($"{_id} 없음?"); return NullText; }
    return TextDic[_id];
  }
  public TextData GetTextData(SkillName _skill)
  {
    string _name = "";
    switch (_skill)
    {
      case SkillName.Speech: _name = "speech"; break;
      case SkillName.Threat: _name = "threat"; break;
      case SkillName.Deception: _name = "deception"; break;
      case SkillName.Logic: _name = "logic"; break;
      case SkillName.Martialarts: _name = "martialarts"; break;
      case SkillName.Bow: _name = "bow"; break;
      case SkillName.Somatology: _name = "somatology"; break;
      case SkillName.Survivable: _name = "survivable"; break;
      case SkillName.Biology: _name = "biology"; ; break;
      case SkillName.Knowledge: _name = "knowledge"; break;
    }
    return GetTextData(_name);
  }
  public TextData GetTextData(ThemeType _theme)
  {
    string _name = "";
    switch (_theme)
    {
      case ThemeType.Conversation: _name = "conversation"; break;
      case ThemeType.Force: _name = "force"; break;
      case ThemeType.Wild: _name = "wild"; break;
      case ThemeType.Intelligence: _name = "intelligence"; break;
    }
    return GetTextData(_name);
  }
  public TextData GetTextData(ThemeType theme,bool isup,bool isstrong)
  {
    string _name = "";
    switch (theme)
    {
      case ThemeType.Conversation: _name = "conversation"; break;
      case ThemeType.Force: _name = "force"; break;
      case ThemeType.Wild: _name = "wild"; break;
      case ThemeType.Intelligence: _name = "intelligence"; break;
    }
    _name += isstrong ? "double" : "";
    _name += isup ? "up" : "down";
    return GetTextData(_name);
  }
  public TextData GetTextData(EffectType _effect)
  {
    switch (_effect)
    {
      case EffectType.Conversation: return GetTextData("conversation");
      case EffectType.Force: return GetTextData("force");
      case EffectType.Wild: return GetTextData("wild");
      case EffectType.Intelligence: return GetTextData("intelligence");
      case EffectType.Speech: return GetTextData("speech");
      case EffectType.Threat: return GetTextData("threat");
      case EffectType.Deception: return GetTextData("deception");
      case EffectType.Logic: return GetTextData("logic");
      case EffectType.Martialarts: return GetTextData("martialarts");
      case EffectType.Bow: return GetTextData("bow");
      case EffectType.Somatology: return GetTextData("somatology");
      case EffectType.Survivable: return GetTextData("survivable");
      case EffectType.Biology: return GetTextData("biology");
      case EffectType.Knowledge: return GetTextData("knowledge");
      case EffectType.HPLoss: return GetTextData(StatusType.HP, false);
      case EffectType.HPGen: return GetTextData(StatusType.HP, true);
      case EffectType.SanityLoss: return GetTextData(StatusType.Sanity, false);
      case EffectType.SanityGen: return GetTextData(StatusType.Sanity, true);
      case EffectType.GoldLoss: return GetTextData(StatusType.Gold, false);
      case EffectType.GoldGen: return GetTextData(StatusType.Gold, true);
      default: return NullText;
    }
  }
  public TextData GetTextData(TendencyType tendency,int level)
  {
    string _name = tendency.Equals(TendencyType.Body) ? "tendency_body_" : "tendency_head_";
    string _level = "";
    switch (level)
    {
      case -2:_level = "m2";break;
      case -1:_level = "m1";break;
      case 0:_level = "0";break;
      case 1:_level = "p1";break;
      case 2:_level = "p2";break;
    }
    return GetTextData(_name + _level);
  }
  public TextData GetTextData(PlaceType _place)
  {
    switch (_place)
    {
      case PlaceType.Residence: return GetTextData("residence");
      case PlaceType.Marketplace: return GetTextData("marketplace");
      case PlaceType.Temple: return GetTextData("temple");
      case PlaceType.Library: return GetTextData("library");
      case PlaceType.Theater: return GetTextData("theater");
      case PlaceType.Academy: return GetTextData("academy");
    }
    return NullText;
  }
  public TextData GetPlaceEffectTextData(PlaceType place)
  {
    switch (place)
    {
      case PlaceType.Residence:
        return GetTextData("residenceeffect");
      case PlaceType.Marketplace:
        return GetTextData("marketplaceeffect");
      case PlaceType.Temple:
        return GetTextData("templeeffect");
      case PlaceType.Library:
        return GetTextData("libraryeffect");
      case PlaceType.Theater:
        return GetTextData("theatereffect");
      default:
        return GetTextData("academyeffect");
    }
  }
  public TextData GetTextData(System.Type _eventtype)
  {
    if (_eventtype == typeof(EventData)) return GetTextData("normaleventpredescription");
    else if (_eventtype == typeof(FollowEventData)) return GetTextData("followeventpredescription");
    else return GetTextData("questeventpredescription");
  }
  public TextData GetTextData(StatusType type)
  {
    switch (type)
    {
      case StatusType.HP:return GetTextData("hp");
      case StatusType.Sanity:return GetTextData("sanity");
      default:return GetTextData("gold");
    }
  }
  public TextData GetTextData(StatusType type,bool isincrease)
  {
    switch (type)
    {
      case StatusType.HP: if (isincrease) return GetTextData("hpincrease"); else return GetTextData("hpdecrease");
      case StatusType.Sanity: if (isincrease) return GetTextData("sanityincrease"); else return GetTextData("sanitydecrease");
      default: if (isincrease) return GetTextData("goldincrease"); else return GetTextData("golddecrease");
    }
  }
  /// <summary>
  /// 레벨은 1/2
  /// </summary>
  /// <param name="type"></param>
  /// <param name="isup"></param>
  /// <param name="level"></param>
  /// <returns></returns>
  public TextData GetTextData(StatusType type,bool isincrease,bool isup, int value)
  {
    string _targetname = "";
    switch (type)
    {
      case StatusType.HP:
        _targetname = "hp";break;
      case StatusType.Sanity:
        _targetname = "sanity";break;
      case StatusType.Gold:
        _targetname = "gold";break;
    }
    _targetname += isincrease ? "increase" : "decrease";
    _targetname += Mathf.Abs(value)>ConstValues.DoubleValue ? "" : "double";
    _targetname += isup ? "up" : "down";
    return GetTextData(_targetname);
  }
  public TextData GetTextData(StatusType type, bool isincrease, bool isup, bool isstrong)
  {
    string _targetname = "";
    switch (type)
    {
      case StatusType.HP:
        _targetname = "hp"; break;
      case StatusType.Sanity:
        _targetname = "sanity"; break;
      case StatusType.Gold:
        _targetname = "gold"; break;
    }
    _targetname += isincrease ? "increase" : "decrease";
    _targetname += isstrong ? "" : "double";
    _targetname += isup ? "up" : "down";
    return GetTextData(_targetname);
  }
    public void LoadData()
  {
    MyGameData = new GameData();
    //데이터가 있으면 데이터 불러오고, 데이터가 없다면 새로 만들기

    Dictionary<string, TextData> _temp = JsonConvert.DeserializeObject<Dictionary<string, TextData>>(TextData.text);
    foreach (var _data in _temp)
    {
      TextData _texttemp = _data.Value;
     if(_texttemp.Name.Contains("\\n")) _texttemp.Name=_texttemp.Name.Replace("\\n", "\n");
      if (_texttemp.Description.Contains("\\n")) _texttemp.Description = _texttemp.Description.Replace("\\n", "\n");
      if (_texttemp.SelectionDescription.Contains("\\n")) _texttemp.SelectionDescription = _texttemp.SelectionDescription.Replace("\\n", "\n");
      if (_texttemp.SelectionSubDescription.Contains("\\n")) _texttemp.SelectionSubDescription = _texttemp.SelectionSubDescription.Replace("\\n", "\n");
      if (_texttemp.FailDescription.Contains("\\n")) _texttemp.FailDescription = _texttemp.FailDescription.Replace("\\n", "\n");
      if (_texttemp.SuccessDescription.Contains("\\n")) _texttemp.SuccessDescription = _texttemp.SuccessDescription.Replace("\\n", "\n");
      TextDic.Add(_data.Value.ID, _data.Value);
    }

    Dictionary<string, EventJsonData> _eventjson = new Dictionary<string, EventJsonData>();
    _eventjson = JsonConvert.DeserializeObject<Dictionary<string, EventJsonData>>(NormalEventData.text);
    foreach (var _data in _eventjson) EventHolder.ConvertData_Normal(_data.Value);
    //이벤트 Json -> EventHolder

    Dictionary<string, FollowEventJsonData> _followeventjson = new Dictionary<string, FollowEventJsonData>();
    _followeventjson = JsonConvert.DeserializeObject<Dictionary<string, FollowEventJsonData>>(FollowEventData.text);
    foreach (var _data in _followeventjson) EventHolder.ConvertData_Follow(_data.Value);
    //연계 이벤트 Json -> EventHolder

    Dictionary<string, QuestEventDataJson> _questeventjson = new Dictionary<string, QuestEventDataJson>();
    _questeventjson = JsonConvert.DeserializeObject<Dictionary<string, QuestEventDataJson>>(QuestEventData.text);
    foreach (var _data in _questeventjson) EventHolder.ConvertData_Quest(_data.Value);
    //퀘스트 Json -> EventHolder

    Dictionary<string, ExperienceJsonData> _expjson = new Dictionary<string, ExperienceJsonData>();
    _expjson = JsonConvert.DeserializeObject<Dictionary<string, ExperienceJsonData>>(EXPData.text);
    foreach (var _data in _expjson)
    {
      Experience _exp = _data.Value.ReturnEXPClass();
      ExpDic.Add(_data.Value.ID, _exp);
      if(_exp.ExpType.Equals(ExpTypeEnum.Mad))MadExpDic.Add(_data.Value.ID, _exp);
    }
    //경험 Json -> EXPDic

    EventHolder.LoadAllEvents();

  }//각종 Json 가져와서 변환
  public void SaveData()
  {

  }//현재 데이터 저장
  public void SuccessCurrentEvent(TendencyType _tendencytype,int index)
  {
    EventHolder.RemoveEvent(MyGameData.CurrentEvent.OriginID);
    switch (_tendencytype)
    {
      case TendencyType.None:
        MyGameData.SuccessEvent_None.Add(MyGameData.CurrentEvent.ID); break;
      case TendencyType.Body:
        if(index.Equals(0))
          MyGameData.SuccessEvent_Rational.Add(MyGameData.CurrentEvent.ID); 
        else
          MyGameData.SuccessEvent_Physical.Add(MyGameData.CurrentEvent.ID); 
        break;
      case TendencyType.Head:
        if(index.Equals(0))
        MyGameData.SuccessEvent_Mental.Add(MyGameData.CurrentEvent.ID);
        else
        MyGameData.SuccessEvent_Material.Add(MyGameData.CurrentEvent.ID); 
        break;
    }
    MyGameData.SuccessEvent_All.Add(MyGameData.CurrentEvent.ID);
    MyGameData.CurrentEventSequence = EventSequence.Clear;
    if (MyGameData.CurrentSettlement != null)
    {
      MyGameData.Turn++;
      UIManager.Instance.UpdateTurnIcon();
    }
  }
  public void FailCurrentEvent(TendencyType _tendencytype, int index)
  {
    switch (_tendencytype)
    {
      case TendencyType.None:
        MyGameData.FailEvent_None.Add(MyGameData.CurrentEvent.ID); break;
      case TendencyType.Body:
        if (index.Equals(0))
          MyGameData.FailEvent_Rational.Add(MyGameData.CurrentEvent.ID);
        else
          MyGameData.FailEvent_Physical.Add(MyGameData.CurrentEvent.ID);
        break;
      case TendencyType.Head:
        if (index.Equals(0))
          MyGameData.FailEvent_Mental.Add(MyGameData.CurrentEvent.ID);
        else
          MyGameData.FailEvent_Material.Add(MyGameData.CurrentEvent.ID);
        break;
    }
    MyGameData.FailEvent_All.Add(MyGameData.CurrentEvent.ID);
    MyGameData.CurrentEventSequence = EventSequence.Clear;
    if (MyGameData.CurrentSettlement != null)
    {
      MyGameData.Turn++;
      UIManager.Instance.UpdateTurnIcon();
    }
  }
  public void AddBadExp(Experience badexp)
  {
    int _targetslot = 0;
    List<int> _emptylist = new List<int>();
    for (int i = 0; i < MyGameData.ShortTermEXP.Length; i++)
      if (MyGameData.ShortTermEXP[i] == null) _emptylist.Add(i);
    //단기 슬롯에서 빈 칸 가져오기
    if (_emptylist.Count > 0)
    {
      _targetslot=Random.Range(0,_emptylist.Count);
      return;
    } //단기 슬롯 중 빈 칸이 있다면 무작위로 악경험 삽입하고 종료

    for (int i = 0; i < MyGameData.LongTermEXP.Length; i++)
      if (MyGameData.LongTermEXP[i] == null) _emptylist.Add(i);
    if (_emptylist.Count > 0)
    {
      _targetslot = Random.Range(0, _emptylist.Count);
      return;
    } //단기 슬롯에 빈 칸이 없다면 장기 슬롯 중 무작위로 악경험 삽입하고 종료

    if (Random.Range(0, 100) < 75)
    {
      _targetslot = Random.Range(0, 4);
    } //장기,단기 둘 다 꽉 차있다면 75% 확률로 단기 경험 하나 대체
    else
    {
      _targetslot = Random.Range(0, 2);
    } //15% 확률로 장기 경험 하나 대체
  }
  public void AddShortExp(Experience _exp, int _index)
  {
    if (_exp.ExpType.Equals(ExpTypeEnum.Mad)) MyGameData.MadnessCount++;
    _exp.Duration = ConstValues.ShortTermStartTurn;
    MyGameData.ShortTermEXP[_index] = _exp;
    UIManager.Instance.UpdateExpShortTermIcon();
  }
  public void AddLongExp(Experience _exp, int _index)
  {
    if (_exp.ExpType.Equals(ExpTypeEnum.Mad)) MyGameData.MadnessCount++;
    _exp.Duration = ConstValues.LongTermStartTurn;
    MyGameData.LongTermEXP[_index] = _exp;
    MyGameData.CurrentSanity -= ConstValues.LongTermChangeCost;
    UIManager.Instance.UpdateSanityText();
    UIManager.Instance.UpdateExpLongTermIcon();
  }
  public void ShiftShortExp(Experience _exp, int _index)
  {
    _exp.Duration = ConstValues.ShortTermStartTurn;
    Experience _target = MyGameData.ShortTermEXP[_index];
    MyGameData.ShortTermEXP[_index] = _exp;
    UIManager.Instance.UpdateExpShortTermIcon();
  }
  public void ShiftLongExp(Experience _exp, int _index)
  {
    _exp.Duration = ConstValues.LongTermStartTurn;
    Experience _target = MyGameData.LongTermEXP[_index];
    MyGameData.LongTermEXP[_index] = _exp;
    MyGameData.CurrentSanity -= ConstValues.LongTermChangeCost;
    UIManager.Instance.UpdateExpLongTermIcon();
  }
  public void SetOuterEvent(EventDataDefulat _event)
  {
    if (_event.GetType().Equals(typeof(QuestEventData))) MyGameData.LastQuestCount = 0;
    MyGameData.CurrentEvent = _event;
    MyGameData.CurrentEventSequence = EventSequence.Progress;
    //현재 이벤트 데이터에 삽입
    MyGameData.RemoveEvent.Add(_event.ID);
    //추후 등장하지 않게
    UIManager.Instance.OpenDialogue();
    //다이어로그 열기
  }//야외 이동을 통해 이벤트를 받은 경우
  public void SetSettlementPlace()
  {
    
    UIManager.Instance.OpenSuggestUI();
    //제시 UI 열기
    SaveData();
  }//정착지의 장소 세팅
  public void SelectEvent(EventDataDefulat _targetevent)
  {
    if (_targetevent.GetType().Equals(typeof(QuestEventData))) MyGameData.LastQuestCount = 0;
    MyGameData.CurrentSanity -= MyGameData.SettleSanityLoss;
    UIManager.Instance.UpdateSanityText();
    Dictionary<Settlement,int> _temp=new Dictionary<Settlement,int>();
    foreach (var _data in MyGameData.AllSettleUnpleasant)
    {
      if (_data.Key.Equals(MyGameData.CurrentSettlement))
      {
        _temp.Add(_data.Key, MyGameData.AllSettleUnpleasant[_data.Key]+1);
      }
      else
      {
        if (MyGameData.AllSettleUnpleasant[_data.Key] == 0) continue;
        else _temp.Add(_data.Key,MyGameData.AllSettleUnpleasant[_data.Key]-1);
      }
    }//정착지에서 이벤트를 선택하면 현재 정착지 불쾌 ++, 
    foreach (var _data in _temp) MyGameData.AllSettleUnpleasant[_data.Key] = _data.Value;
    MyGameData.CurrentEvent = _targetevent;
    MyGameData.CurrentEventSequence = EventSequence.Progress;
    //현재 이벤트 데이터에 삽입
    MyGameData.RemoveEvent.Add(_targetevent.ID);
    //추후 등장하지 않게
    if (MyGameData.CurrentSuggestingEvents.Contains(_targetevent)) MyGameData.CurrentSuggestingEvents.Remove(_targetevent);
    //현재 제시 리스트에서 제거
    UIManager.Instance.OpenDialogue();
    SaveData();
  }//제시 패널에서 이벤트를 선택한 경우
  public void SetNewQuest(QuestHolder _quest)
  {
    MyGameData.CurrentQuest = _quest;
    UIManager.Instance.OpenQuestDialogue();
    SaveData();
  }//정착지 도착을 통해 퀘스트를 받은 경우
  public void AddTendencyCount(TendencyType _tendencytype,int index)
  {
    switch (_tendencytype)
    {
      case TendencyType.Body:
        MyGameData.Tendency_Body.AddCount(index.Equals(0) ? false : true);
        break;
      case TendencyType.Head:
        MyGameData.Tendency_Head.AddCount(index.Equals(0) ? false : true);
        break;
    }
  }
  private void Awake()
  {
        NullText = new TextData();
        NullText.Name = "NullName";
        NullText.Description = "NullDescription";
        NullText.SelectionDescription = "NullSelection@NullSelection";
        NullText.FailDescription = "NullFail";
        NullText.SuccessDescription = "NullSuccess";
    if(instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
      LoadData();
      OpenAllQuest();
      //  DebugAllEvents();
    }
    else Destroy(gameObject);

  }
  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Backspace))
    {
      CreateNewMap();

    }
    if (Input.GetKeyDown(KeyCode.F1)) MyGameData.CurrentSanity = 3;
  }
  public void DebugAllEvents()
  {
    string _str = "";
    foreach (var _data in EventHolder.AvailableNormalEvents)
    {
      _str += $"이벤트 ID : {_data.ID}\n이벤트 이름 : {_data.Name}\n설명 : {_data.Description}\n" +
          $"\n";
    }
    _str += "\n";
    foreach(var _data in EventHolder.AvailableFollowEvents)
    {
      _str += $"연계 이벤트 Id : {_data.ID}\n연계 대상 : {_data.FollowTarget}\n";
    }
    _str += "\n";
    foreach(var _data in EventHolder.AvailableQuests)
    {
      _str += $"퀘스트 {_data.Key} 시작 문구 : {_data.Value.StartDialogue}\n승 이벤트\n";
      foreach(var _rising in _data.Value.Eventlist_Rising)
      {
        _str += $"이벤트 ID : {_rising.ID}\n이벤트 이름 : {_rising.Name}\n설명 : {_rising.Description}\n" +
            $"\n";
      }
      foreach (var _climax in _data.Value.Eventlist_Climax)
      {
        _str += $"이벤트 ID : {_climax.ID}\n이벤트 이름 : {_climax.Name}\n설명 : {_climax.Description}\n" +
            $"\n";
      }
    }

    Debug.Log(_str);
  }
  public void OpenAllQuest()
  {
    foreach(var _data in EventHolder.AllQuests)
    {
      MyProgressData.TotalFoundQuest.Add(_data.Key);
    }
  }

  public void LoadGameScene()
  {
    StartCoroutine(loadscene("GameScene"));
  }
  private IEnumerator loadscene(string scenename)
  {
    if (MainUIManager.Instance != null) MyGameData.CurrentQuest = EventHolder.AllQuests[MainUIManager.Instance.SelectQuestID];
    AsyncOperation _oper = SceneManager.LoadSceneAsync(scenename);
    _oper.allowSceneActivation = true;

    yield return new WaitUntil(()=> _oper.isDone==true);
        yield return StartCoroutine(createnewmap());
    UIManager.Instance.UpdateAllUI();

    if (MyGameData.CurrentEvent == null)
    {
      if (MyGameData.CurrentSuggestingEvents.Count>0)
      {
        UIManager.Instance.OpenSuggestUI();
        //현재 진행 이벤트는 없는데 제시 이벤트가 남아있으면 이벤트 제시 패널에 장소, 일러스트, 정보를 채워넣고 제시 패널 열기
      }
      else
      {
        UIManager.Instance.OpenQuestDialogue();
        //현재 진행 이벤트도 없고, 제시 중인 이벤트도 없다는건 지금 막 퀘스트를 조우한 상태라는 뜻이니 퀘스트 패널 세팅하고 열기
      }
    }
    else
    {
      if (MyGameData.CurrentEventSequence.Equals(EventSequence.Progress))
      {
        UIManager.Instance.OpenDialogue();
        //이벤트 있을 때, 진행 단계일 경우 이름, 일러스트, 설명, 선택지 세팅하고 이벤트 패널 열기
      }
      else
      {
        string _id = MyGameData.CurrentEvent.ID;
        SuccessData _success = null;
        if (MyGameData.SuccessEvent_None.Contains(_id)) _success = MyGameData.CurrentEvent.SuccessDatas[0];
        else if(MyGameData.SuccessEvent_Rational.Contains(_id))_success = MyGameData.CurrentEvent.SuccessDatas[0];
        else if(MyGameData.SuccessEvent_Mental.Contains(_id)) _success = MyGameData.CurrentEvent.SuccessDatas[0];
        else if(MyGameData.SuccessEvent_Physical.Contains(_id)) _success = MyGameData.CurrentEvent.SuccessDatas[1];
        else _success=MyGameData.CurrentEvent.SuccessDatas[1];
        if (_success != null) { UIManager.Instance.OpenSuccessDialogue(_success); yield break; }

        FailureData _fail = null;
        if (MyGameData.FailEvent_None.Contains(_id)) _fail = MyGameData.CurrentEvent.FailureDatas[0];
        else if (MyGameData.FailEvent_Rational.Contains(_id)) _fail = MyGameData.CurrentEvent.FailureDatas[0];
        else if (MyGameData.FailEvent_Mental.Contains(_id)) _fail = MyGameData.CurrentEvent.FailureDatas[0];
        else if (MyGameData.FailEvent_Physical.Contains(_id)) _fail = MyGameData.CurrentEvent.FailureDatas[1];
        else _fail = MyGameData.CurrentEvent.FailureDatas[1];
        if (_fail != null) { UIManager.Instance.OpenFailDialogue(_fail); yield break; }
        

        //이벤트 있을 때, 완료 단계일 경우 완료 리스트에서 현재 이벤트 찾고 완료 결과에 따라 설명, 보상 세팅 열고 이벤트 패널 열기
      }
    }

    yield return null;
  }
  public void CreateNewMap()
  {
    StartCoroutine(createnewmap());
  }
  private IEnumerator createnewmap()
  {
    maptext _map = FindObjectOfType<maptext>().GetComponent<maptext>();

    _map.MakePerfectMap();

    yield return new WaitUntil(()=>MyMapSaveData != null);
    MyMapData = MyMapSaveData.ConvertToMapData();

    Settlement _startsettle = MyMapData.AllSettles[Random.Range(0, MyMapData.AllSettles.Count)];

    MyGameData.CurrentSettlement = _startsettle;
    MyGameData.CurrentPos = _startsettle.VectorPos;

    MyGameData.AvailableSettlement = MyMapData.GetCloseSettles(_startsettle, 3);

    _map.MakeTilemap(MyMapSaveData, MyMapData);

    MyGameData.CreateSettleUnpleasant(MyMapData.AllSettles);

    UIManager.Instance.UpdateMap_SetPlayerPos(_startsettle);
    MyGameData.Tendency_Body.Level = 2;
    MyGameData.Tendency_Head.Level = -1;
    yield return null;
  }
}
