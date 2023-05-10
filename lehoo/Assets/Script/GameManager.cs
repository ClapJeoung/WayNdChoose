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

  [HideInInspector] public GameData MyGameData = new GameData();            //���� ������(���൵,���� ���� �� �̺�Ʈ, ���� �� ����,����Ʈ ���)
  private const string GameDataName = "GameData.json";
  [HideInInspector] public MapData MyMapData = null;              //�� ������(�� ������)
  [HideInInspector] public MapSaveData MyMapSaveData = null;
  private const string MapDataName = "MapData.json";
  [HideInInspector] public ProgressData MyProgressData = new ProgressData();
  private const string ProgressDataName = "ProgressData.json";

  public ImageHolder ImageHolder = null;             //�̺�Ʈ,����,Ư��,������ �Ϸ���Ʈ Ȧ��

  [SerializeField] private TextAsset NormalEventData = null;  //�̺�Ʈ Json
  [SerializeField] private TextAsset FollowEventData = null;  //���� �̺�Ʈ Json
  [SerializeField] private TextAsset QuestEventData = null;   //����Ʈ �̺�Ʈ Json
  [SerializeField] private TextAsset EXPData = null;    //���� Json
  [SerializeField] private TextAsset TraitData = null;  //Ư�� Json
  [SerializeField] private TextAsset TextData = null;
  public EventHolder EventHolder = new EventHolder();                               //�̺�Ʈ ������ Ȧ��
  public Dictionary<string, Experience> ExpDic = new Dictionary<string, Experience>();  //���� ��ųʸ�
  public Dictionary<string, Trait> TraitsDic = new Dictionary<string, Trait>();         //Ư�� ��ųʸ�
  public Dictionary<string, TextData> TextDic = new Dictionary<string, TextData>();   //���� �ؽ�Ʈ ����͸�
  public TextData NullText = null;
  public TextData GetTextData(string _id)
  {
    // Debug.Log($"{_id} ID�� ���� �ؽ�Ʈ ������ {(TextDic.ContainsKey(_id)?"����":"����")}");
    if (!TextDic.ContainsKey(_id)) { Debug.Log($"{_id} ����?"); return NullText; }
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
      case EffectType.HPLoss: return GetTextData("hp");
      case EffectType.HPGen: return GetTextData("hp");
      case EffectType.SanityLoss: return GetTextData("sanity");
      case EffectType.SanityGen: return GetTextData("sanity");
      case EffectType.GoldLoss: return GetTextData("gold");
      case EffectType.GoldGen: return GetTextData("gold");
      default: return NullText;
    }
  }
  public TextData GetTextData(TendencyType _tendency)
  {
    switch (_tendency)
    {
      case TendencyType.Rational:
        return GetTextData("rational");
      case TendencyType.Physical:
        return GetTextData("physical");
      case TendencyType.Mental:
        return GetTextData("mental");
      case TendencyType.Material:
        return GetTextData("material");
      default: return NullText;
    }
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
  public TextData GetTextData(System.Type _eventtype)
  {
    if (_eventtype == typeof(EventData)) return GetTextData("normaleventpredescription");
    else if (_eventtype == typeof(FollowEventData)) return GetTextData("followeventpredescription");
    else return GetTextData("questeventpredescription");
  }
  public void LoadData()
  {
    MyGameData = new GameData();
    //�����Ͱ� ������ ������ �ҷ�����, �����Ͱ� ���ٸ� ���� �����

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
    //�̺�Ʈ Json -> EventHolder

    Dictionary<string, FollowEventJsonData> _followeventjson = new Dictionary<string, FollowEventJsonData>();
    _followeventjson = JsonConvert.DeserializeObject<Dictionary<string, FollowEventJsonData>>(FollowEventData.text);
    foreach (var _data in _followeventjson) EventHolder.ConvertData_Follow(_data.Value);
    //���� �̺�Ʈ Json -> EventHolder

    Dictionary<string, QuestEventDataJson> _questeventjson = new Dictionary<string, QuestEventDataJson>();
    _questeventjson = JsonConvert.DeserializeObject<Dictionary<string, QuestEventDataJson>>(QuestEventData.text);
    foreach (var _data in _questeventjson) EventHolder.ConvertData_Quest(_data.Value);
    //����Ʈ Json -> EventHolder

    Dictionary<string, ExperienceJsonData> _expjson = new Dictionary<string, ExperienceJsonData>();
    _expjson = JsonConvert.DeserializeObject<Dictionary<string, ExperienceJsonData>>(EXPData.text);
    foreach (var _data in _expjson)
    {
      Experience _exp = _data.Value.ReturnEXPClass();
      ExpDic.Add(_data.Value.ID, _exp);
    }
    //���� Json -> EXPDic

    Dictionary<string, TraitJsonData> _traitjson = new Dictionary<string, TraitJsonData>();
    _traitjson = JsonConvert.DeserializeObject<Dictionary<string, TraitJsonData>>(TraitData.text);
    foreach (var _data in _traitjson)
    {
      Trait _trait = _data.Value.ReturnTraitClass();
      TraitsDic.Add(_data.Value.ID, _trait);
    }
    //Ư�� Json -> TraitDic


    EventHolder.LoadAllEvents();

  }//���� Json �����ͼ� ��ȯ
  public void SaveData()
  {

  }//���� ������ ����
  public void SuccessCurrentEvent(TendencyType _tendencytype)
  {
    EventHolder.RemoveEvent(MyGameData.CurrentEvent.ID);
    switch (_tendencytype)
    {
      case TendencyType.None:
        MyGameData.SuccessEvent_None.Add(MyGameData.CurrentEvent.ID); break;
      case TendencyType.Rational:
        MyGameData.SuccessEvent_Rational.Add(MyGameData.CurrentEvent.ID); break;
      case TendencyType.Mental:
        MyGameData.SuccessEvent_Mental.Add(MyGameData.CurrentEvent.ID); break;
      case TendencyType.Physical:
        MyGameData.SuccessEvent_Physical.Add(MyGameData.CurrentEvent.ID); break;
      case TendencyType.Material:
        MyGameData.SuccessEvent_Material.Add(MyGameData.CurrentEvent.ID); break;
    }
    MyGameData.SuccessEvent_All.Add(MyGameData.CurrentEvent.ID);
    MyGameData.CurrentEventSequence = EventSequence.Clear;
    if (MyGameData.CurrentSettlement != null)
    {
      MyGameData.Turn++;
      UIManager.Instance.UpdateTurnIcon();
    }
  }
  public void FailCurrentEvent(TendencyType _tendencytype)
  {
    switch (_tendencytype)
    {
      case TendencyType.None:
        MyGameData.FailEvent_None.Add(MyGameData.CurrentEvent.ID); break;
      case TendencyType.Rational:
        MyGameData.FailEvent_Rational.Add(MyGameData.CurrentEvent.ID); break;
      case TendencyType.Mental:
        MyGameData.FailEvent_Mental.Add(MyGameData.CurrentEvent.ID); break;
      case TendencyType.Physical:
        MyGameData.FailEvent_Physical.Add(MyGameData.CurrentEvent.ID); break;
      case TendencyType.Material:
        MyGameData.FailEvent_Material.Add(MyGameData.CurrentEvent.ID); break;
    }
    MyGameData.FailEvent_All.Add(MyGameData.CurrentEvent.ID);
    MyGameData.CurrentEventSequence = EventSequence.Clear;
    if (MyGameData.CurrentSettlement != null)
    {
      MyGameData.Turn++;
      UIManager.Instance.UpdateTurnIcon();
    }
  }
  public void AddShortExp(Experience _exp, int _index)
  {
    _exp.Duration = ConstValues.ShortTermStartTurn;
    MyGameData.ShortTermEXP[_index] = _exp;
    UIManager.Instance.UpdateExpShortTermIcon();
  }
  public void AddLongExp(Experience _exp, int _index)
  {
    _exp.Duration = ConstValues.LongTermStartTurn;
    MyGameData.LongTermEXP[_index] = _exp;
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
    UIManager.Instance.UpdateExpLongTermIcon();
  }
  public void SetOuterEvent(EventDataDefulat _event)
  {
    if (_event.GetType().Equals(typeof(QuestEventData))) MyGameData.LastQuestCount = 0;
    MyGameData.CurrentEvent = _event;
    MyGameData.CurrentEventSequence = EventSequence.Progress;
    //���� �̺�Ʈ �����Ϳ� ����
    MyGameData.RemoveEvent.Add(_event.ID);
    //���� �������� �ʰ�
    UIManager.Instance.OpenDialogue();
    MyGameData.LastPlaceTypes.Add(_event.PlaceType);
    //���̾�α� ����
  }//�߿� �̵��� ���� �̺�Ʈ�� ���� ���
  public void SetSettleEventList(List<EventDataDefulat> _list)
  {
    MyGameData.CurrentSuggestingEvents = _list;
//    foreach (var _data in MyGameData.CurrentSuggestingEvents) Debug.Log(_data.Name);
    //���� ���� ��� �ֻ�ȭ
    UIManager.Instance.OpenSuggestUI();
    //���� UI ����
    SaveData();
  }//������ ������ ���� �̺�Ʈ ����Ʈ�� ���� ���
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
    }//���������� �̺�Ʈ�� �����ϸ� ���� ������ ���� ++, 
    foreach (var _data in _temp) MyGameData.AllSettleUnpleasant[_data.Key] = _data.Value;
    MyGameData.CurrentEvent = _targetevent;
    MyGameData.CurrentEventSequence = EventSequence.Progress;
    //���� �̺�Ʈ �����Ϳ� ����
    MyGameData.RemoveEvent.Add(_targetevent.ID);
    //���� �������� �ʰ�
    if (MyGameData.CurrentSuggestingEvents.Contains(_targetevent)) MyGameData.CurrentSuggestingEvents.Remove(_targetevent);
    //���� ���� ����Ʈ���� ����
    UIManager.Instance.OpenDialogue();
    MyGameData.LastPlaceTypes.Add(_targetevent.PlaceType);
    SaveData();
  }//���� �гο��� �̺�Ʈ�� ������ ���
  public void SetNewQuest(QuestHolder _quest)
  {
    MyGameData.CurrentQuest = _quest;
    UIManager.Instance.OpenQuestDialogue();
    SaveData();
  }//������ ������ ���� ����Ʈ�� ���� ���
  public void AddTendencyCount(TendencyType _tendencytype)
  {
    switch (_tendencytype)
    {
      case TendencyType.Rational:
        MyGameData.Tendency_RP.AddCount(_tendencytype);
        break;
      case TendencyType.Physical:
        MyGameData.Tendency_RP.AddCount(_tendencytype);
        break;
      case TendencyType.Mental:
        MyGameData.Tendency_MM.AddCount(_tendencytype);
        break;
      case TendencyType.Material:
        MyGameData.Tendency_MM.AddCount(_tendencytype);
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
      StartCoroutine(_eventtest());

    }
  }
  private IEnumerator _eventtest()
  {
    yield return new WaitUntil(() => (MyMapData != null));
    foreach (var _settle in MyMapData.AllSettles)
    {
      string _str = $"������ �̸� : {_settle.Name}  ������ ȯ��: ";
      if (_settle.IsForest) _str += "�� ";
      if (_settle.IsRiver) _str += "�� ";
      if (_settle.IsHighland) _str += "��� ";
      if (_settle.IsMountain) _str += "�� ";
      if (_settle.IsSea) _str += "�ٴ� ";
      _str += "\n";
      _str += $"���� ���� : {_settle.Wealth}  ���� ���� : {_settle.Wealth}  ��� ���� : {_settle.Faith}  ";
      switch (_settle.Type)
      {
        case SettlementType.Town:
          break;
        case SettlementType.City:
          _str += $"������ ���� : {_settle.Culture}";
          break;
        case SettlementType.Castle:
          _str += $"���� ���� : {_settle.Culture} ��ī���� ���� : {_settle.Science}";
          break;
      }
      _str += "\n\n";
      TargetTileEventData _tiledata = _settle.GetSettleTileEventData();
      List<EventDataDefulat> _results = EventHolder.ReturnEvent(_tiledata);
      foreach (var _event in _results)
      {
        _str += $"�̺�Ʈ �̸� : {_event.Name} �̺�Ʈ ���� : ";
        _str += _event.GetType().Equals(typeof(QuestEventData)) ? "����Ʈ " : _event.GetType().Equals(typeof(FollowEventData)) ? "���� " : "�Ϲ� ";
        _str += "\n";
        _str += $"���� ��� : {_event.PlaceType}  ���� ���� : {(_event.PlaceLevel == 0 ? "����" : _event.PlaceLevel == 1 ? "1" : _event.PlaceLevel == 2 ? 2 : 3)}";
        _str += "\n";
      }
  //    Debug.Log(_str);  
    }
    yield return null;
  }
  public void DebugAllEvents()
  {
    string _str = "";
    foreach (var _data in EventHolder.AvailableNormalEvents)
    {
      _str += $"�̺�Ʈ ID : {_data.ID}\n�̺�Ʈ �̸� : {_data.Name}\n���� : {_data.Description}\n" +
          $"\n";
    }
    _str += "\n";
    foreach(var _data in EventHolder.AvailableFollowEvents)
    {
      _str += $"���� �̺�Ʈ Id : {_data.ID}\n���� ��� : {_data.FollowTarget}\n";
    }
    _str += "\n";
    foreach(var _data in EventHolder.AvailableQuests)
    {
      _str += $"����Ʈ {_data.Key} ���� ���� : {_data.Value.StartDialogue}\n�� �̺�Ʈ\n";
      foreach(var _rising in _data.Value.Eventlist_Rising)
      {
        _str += $"�̺�Ʈ ID : {_rising.ID}\n�̺�Ʈ �̸� : {_rising.Name}\n���� : {_rising.Description}\n" +
            $"\n";
      }
      foreach (var _climax in _data.Value.Eventlist_Climax)
      {
        _str += $"�̺�Ʈ ID : {_climax.ID}\n�̺�Ʈ �̸� : {_climax.Name}\n���� : {_climax.Description}\n" +
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
        //���� ���� �̺�Ʈ�� ���µ� ���� �̺�Ʈ�� ���������� �̺�Ʈ ���� �гο� ���, �Ϸ���Ʈ, ������ ä���ְ� ���� �г� ����
      }
      else
      {
        UIManager.Instance.OpenQuestDialogue();
        //���� ���� �̺�Ʈ�� ����, ���� ���� �̺�Ʈ�� ���ٴ°� ���� �� ����Ʈ�� ������ ���¶�� ���̴� ����Ʈ �г� �����ϰ� ����
      }
    }
    else
    {
      if (MyGameData.CurrentEventSequence.Equals(EventSequence.Progress))
      {
        UIManager.Instance.OpenDialogue();
        //�̺�Ʈ ���� ��, ���� �ܰ��� ��� �̸�, �Ϸ���Ʈ, ����, ������ �����ϰ� �̺�Ʈ �г� ����
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
        

        //�̺�Ʈ ���� ��, �Ϸ� �ܰ��� ��� �Ϸ� ����Ʈ���� ���� �̺�Ʈ ã�� �Ϸ� ����� ���� ����, ���� ���� ���� �̺�Ʈ �г� ����
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
    foreach (Settlement _settle in MyGameData.AvailableSettlement) _settle.IsOpen = true;

    _map.MakeTilemap(MyMapSaveData, MyMapData);

    MyGameData.CreateSettleUnpleasant(MyMapData.AllSettles);

    UIManager.Instance.UpdateMap_SetPlayerPos(_startsettle);
    MyGameData.Tendency_MM.Level = 2;
    yield return null;
  }
}
