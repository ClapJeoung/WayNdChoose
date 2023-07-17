using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;

public class GameManager : MonoBehaviour
{
  private static GameManager instance;
  public static GameManager Instance { get { return instance; } }

  [HideInInspector] public GameData MyGameData = null;            //���� ������(���൵,���� ���� �� �̺�Ʈ, ���� �� ����,����Ʈ ���)
  [HideInInspector] public GameJsonData MyGameJsonData = null;
  [HideInInspector] public const string GameDataName = "WNCGameData.json";
  [HideInInspector] public MapData MyMapData = null;              //�� ������(�� ������)
  [HideInInspector] public ProgressData MyProgressData = new ProgressData();

  public ImageHolder ImageHolder = null;             //�̺�Ʈ,����,Ư��,������ �Ϸ���Ʈ Ȧ��

  [SerializeField] private TextAsset NormalEventData = null;  //�̺�Ʈ Json
  [SerializeField] private TextAsset FollowEventData = null;  //���� �̺�Ʈ Json
  [SerializeField] private TextAsset QuestEventData = null;   //����Ʈ �̺�Ʈ Json
  [SerializeField] private TextAsset EXPData = null;    //���� Json
  [SerializeField] private TextAsset TextData = null;
  public EventHolder EventHolder = new EventHolder();                               //�̺�Ʈ ������ Ȧ��
  public Dictionary<string, Experience> ExpDic = new Dictionary<string, Experience>();  //���� ��ųʸ�
  public Dictionary<string, Experience> MadExpDic = new Dictionary<string, Experience>();
  public Dictionary<string,string> TextDic=new Dictionary<string, string>();
  public const string NullText = "no text exist";
  private GoldFailData goldfaildata = null;
  public GoldFailData GoldFailData
  {
    get
    {
      if (goldfaildata == null)
      {
        goldfaildata = new GoldFailData();
        goldfaildata.Description = GetTextData("GOLDFAIL_TEXT");
        goldfaildata.Panelty_target = PenaltyTarget.Status;
        goldfaildata.Loss_target = StatusType.Sanity;
        goldfaildata.Illust = ImageHolder.NoGoldIllust;

      }
      return goldfaildata;
    }
  }
  public string GetTextData(string _id)
  {
    // Debug.Log($"{_id} ID�� ���� �ؽ�Ʈ ������ {(TextDic.ContainsKey(_id)?"����":"����")}");
    if (!TextDic.ContainsKey(_id)) { Debug.Log($"{_id} ����?"); return NullText; }
    return TextDic[_id];
  }
  /// <summary>
  /// texttype : �̸�/����
  /// </summary>
  /// <param name="envir"></param>
  /// <param name="texttype"></param>
  /// <returns></returns>
  public string GetTextData(EnvironmentType envir,int texttype)
  {
    string _str = "";
    switch (envir)
    {
      case EnvironmentType.NULL:
        return NullText;
      case EnvironmentType.River:
        _str = "RIVER";
        break;
      case EnvironmentType.Forest:
        _str = "FOREST";
        break;
      case EnvironmentType.Highland:
        _str = "HIGHLAND";
        break;
      case EnvironmentType.Mountain:
        _str = "MOUNTAIN";
        break;
      default:
        _str = "SEA";
        break;
    }
    _str += "_";
    if (texttype.Equals(0)) _str += "NAME";
    else _str += "DESCRIPTION";
    return GetTextData(_str);
  }
  /// <summary>
  /// �̸�(������ ����),�̸�(������ ����),������,����,��������,����,ũ�� ����,����,ũ�� ����
  /// </summary>
  /// <param name="_theme"></param>
  /// <param name="texttype"></param>
  /// <returns></returns>
  public string GetTextData(SkillType skilltype,int texttype)
  {
    string _name = "";
    switch (skilltype)
    {
      case SkillType.Conversation: _name = "CONVERSATION"; break;
      case SkillType.Force: _name = "FORCE"; break;
      case SkillType.Wild: _name = "WILD"; break;
      case SkillType.Intelligence: _name = "INTELLIGENCE"; break;
    }
    _name += "_";
    switch (texttype)
    {
      case 0:
        _name += "NAME_NOICON";
        break;
      case 1:
        _name += "NAME_ICON";
        break;
      case 2:
        _name += "ICON";
        break;
      case 3:
        _name += "DESCRIPTION";
        break;
      case 4:
        _name += "SUBDESCRIPTION";
        break;
      case 5:
        _name += "UP_NORMAL";
        break;
      case 6:
        _name += "UP_HIGH";
        break;
      case 7:
        _name += "DOWN_NORMAL";
        break;
      case 8:
        _name += "DOWN_HIGH";
        break;
    }
    return GetTextData(_name);
  }
  public string GetTextData(SkillType skilltype, bool isup,bool isstrong,bool isicon)
  {
    string _name = "";
    switch (skilltype)
    {
      case SkillType.Conversation: _name = "CONVERSATION"; break;
      case SkillType.Force: _name = "FORCE"; break;
      case SkillType.Wild: _name = "WILD"; break;
      case SkillType.Intelligence: _name = "INTELLIGENCE"; break;
    }
    _name += isup ? "_UP" : "_DOWN";
    _name += isstrong ? "_HIGH" : "_NORMAL";
    _name += isicon ? "_ICON" : "";
    return GetTextData(_name);
  }
  public string GetTextData(EffectType _effect,bool isicon)
  {
    switch (_effect)
    {
      case EffectType.Conversation: return GetTextData(SkillType.Conversation,true,false,isicon);
      case EffectType.Force: return GetTextData(SkillType.Force , true, false, isicon);
      case EffectType.Wild: return GetTextData(SkillType.Wild, true, false, isicon);
      case EffectType.Intelligence: return GetTextData(SkillType.Intelligence, true, false, isicon);
      case EffectType.HPLoss: return GetTextData(StatusType.HP,false,isicon?2:1);
      case EffectType.HPGen: return GetTextData(StatusType.HP, true, isicon?2:1);
      case EffectType.SanityLoss: return GetTextData(StatusType.Sanity, false, isicon?2:1);
      case EffectType.SanityGen: return GetTextData(StatusType.Sanity, true, isicon?2:1);
      case EffectType.GoldLoss: return GetTextData(StatusType.Gold, false, isicon?2:1);
      case EffectType.GoldGen: return GetTextData(StatusType.Gold, true, isicon?2:1);
      default: return NullText;
    }
  }
  /// <summary>
  /// �̸� ���� �������� ������
  /// </summary>
  /// <param name="tendency"></param>
  /// <param name="level"></param>
  /// <returns></returns>
  public string GetTextData(TendencyType tendency,int level,int texttype)
  {
    string _name = tendency.Equals(TendencyType.Body) ? "TENDENCY_BODY" : "TENDENCY_HEAD";
    string _level = "";
    switch (level)
    {
      case -2:_level = "M2";break;
      case -1:_level = "M1";break;
      case 1:_level = "P1";break;
      case 2:_level = "P2";break;
    }
    string _type = "";
    switch (texttype)
    {
      case 0:_type = "NAME";break;
      case 1: _type = "DESCRIPTIION"; break;
      case 2: _type = "SUBDESCRIPTION"; break;
      case 3: _type = "ICON"; break;
    }
    return GetTextData(_name+"_" + _level+"_"+_type);
  }
  /// <summary>
  /// �̸� ���� ������ ȿ�� ȿ������
  /// </summary>
  /// <param name="_place"></param>
  /// <param name="texttype"></param>
  /// <returns></returns>
  public string GetTextData(PlaceType _place,int texttype)
  {
    string _str = "PLACE_";
    switch (_place)
    {
      case PlaceType.Residence: _str += "RESIDENCE";break;
      case PlaceType.Marketplace: _str += "MARKETPLACE"; break;
      case PlaceType.Temple: _str += "TEMPLE"; break;
      case PlaceType.Library: _str += "LIBRARY"; break;
      case PlaceType.Theater: _str += "THEATER"; break;
      case PlaceType.Academy: _str += "ACADEMY"; break;
    }
    _str += "_";
    switch (texttype)
    {
      case 0: _str += "NAME";break;
      case 1: _str += "DESCRIPTION";break;
      case 2: _str += "ICON";break;
      case 3: _str += "EFFECT_NAME";break;
      case 4: _str += "EFFECT_DESCRIPTION";break;
    }

    return GetTextData(_str);
  }
  /// <summary>
  /// �̸�(X) �̸�(O) ������ ���� ��������|ȸ��(X) ȸ��(O) ȸ��������|�Ҹ�(X) �Ҹ�(O) �Ҹ������|ȸ������(X) ȸ������(O) ȸ��������|�Ҹ�����(X) �Ҹ�����(O) �Ҹ������ 
  /// </summary>
  /// <param name="type"></param>
  /// <returns></returns>
  public string GetTextData(StatusType type,int texttype)
  {
    string _str = "";
    switch (type)
    {
      case StatusType.HP: _str = "HP";break;
      case StatusType.Sanity:_str = "SANITY";break;
      case StatusType.Gold: _str = "GOLD";break;
    }
    _str += "_";
    switch (texttype)
    {
      case 0:_str += "NAME_NOICON";break;
      case 1: _str += "NAME_ICON"; break;
      case 2: _str += "ICON"; break;
      case 3: _str += "DESCRIPTION"; break;
      case 4: _str += "SUBDESCRIPTION"; break;
      case 5: _str += "RESTORE_NAME_NOICON"; break;
      case 6: _str += "RESTORE_NAME_ICON"; break;
      case 7: _str += "RESTORE_ICON"; break;
      case 8: _str += "PAY_NAME_NOICON"; break;
      case 9: _str += "PAY_NAME_ICON"; break;
      case 10: _str += "PAY_ICON"; break;
      case 11: _str += "INCREASE_NAME_NOICON"; break;
      case 12: _str += "INCREASE_NAME_ICON"; break;
      case 13: _str += "INCREASE_ICON"; break;
      case 14: _str += "DECREASE_NAME_NOICON"; break;
      case 15: _str += "DECREASE_NAME_ICON"; break;
      case 16: _str += "DECREASE_ICON"; break;
    }
    return GetTextData(_str);
  }
  /// <summary>
  /// ������X ������O ������
  /// </summary>
  /// <param name="type"></param>
  /// <param name="isincrease"></param>
  /// <param name="icontype"></param>
  /// <returns></returns>
  public string GetTextData(StatusType type,bool isincrease,int icontype)
  {
    int _typecode = isincrease ? 11 : 14;
    _typecode += icontype;
    return GetTextData(type, _typecode);
  }
    public void LoadData()
  {
    if (File.Exists(Application.persistentDataPath+"/"+GameDataName ))
    {
      MyGameJsonData = JsonUtility.FromJson<GameJsonData>(File.ReadAllText(Application.persistentDataPath + "/" + GameDataName));
      MyGameData = MyGameJsonData.GetGameData();
      MyMapData = MyGameJsonData.GetMapData();
    }
    //����� �÷��̾� �����Ͱ� ������ ������ �ҷ�����

    Dictionary<string, string> _temp = JsonConvert.DeserializeObject<Dictionary<string, string>>(TextData.text);
    foreach (var _data in _temp)
    {
      string _texttemp = _data.Value;
      if (_texttemp.Contains("\\n")) _texttemp = _texttemp.Replace("\\n", "\n");
      if (TextDic.ContainsKey(_data.Key)) { Debug.Log($"{_data.Key} ��ħ! Ȯ�� �ʿ�!"); return; }
      TextDic.Add(_data.Key, _data.Value);
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
      if(_exp.ExpType.Equals(ExpTypeEnum.Mad))MadExpDic.Add(_data.Value.ID, _exp);
    }
    //���� Json -> EXPDic

   if(MyGameData!=null)EventHolder.LoadAllEvents();

  }//���� Json �����ͼ� ��ȯ
  public void SaveData()
  {

  }//���� ������ ����
  public void SuccessCurrentEvent(TendencyType _tendencytype,int index)
  {
   if(MyGameData.CurrentSettlement!=null)MyGameData.CurrentSettlement.SetAvailablePlaces();
    EventHolder.RemoveEvent(MyGameData.CurrentEvent.ID);
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
    if (MyGameData.CurrentSettlement != null) MyGameData.CurrentSettlement.SetAvailablePlaces();
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
    //�ܱ� ���Կ��� �� ĭ ��������
    if (_emptylist.Count > 0)
    {
      _targetslot=Random.Range(0,_emptylist.Count);
      MyGameData.ShortTermEXP[_targetslot] = badexp;
      UIManager.Instance.UpdateExpShortTermIcon();
      return;
    } //�ܱ� ���� �� �� ĭ�� �ִٸ� �������� �ǰ��� �����ϰ� ����

    if (MyGameData.LongTermEXP==null)
    {
      MyGameData.LongTermEXP = badexp;
      UIManager.Instance.UpdateExpLongTermIcon();
      return;
    } //�ܱ� ���Կ� �� ĭ�� ���ٸ� ��� ���� �� �������� �ǰ��� �����ϰ� ����

    if (Random.Range(0, 100) < 75)
    {
      _targetslot = Random.Range(0, 2);
      MyGameData.ShortTermEXP[_targetslot] = badexp;
      UIManager.Instance.UpdateExpShortTermIcon();
    } //���,�ܱ� �� �� �� ���ִٸ� 75% Ȯ���� �ܱ� ���� �ϳ� ��ü
    else
    {
      MyGameData.LongTermEXP = badexp;
      UIManager.Instance.UpdateExpLongTermIcon();
    } //15% Ȯ���� ��� ���� �ϳ� ��ü
  }
  public void AddShortExp(Experience _exp, int _index)
  {
    if (UIManager.Instance.MyQuestSuggent.IsActivePanel) UIManager.Instance.MyQuestSuggent.OpenStarting();
    //����Ʈ ���� �гο��� ���� �����ϴ� ����� ���� �ܰ�� �Ѿ��

    if (_exp.ExpType.Equals(ExpTypeEnum.Mad)) MyGameData.MadnessCount++;
    _exp.Duration = ConstValues.ShortTermStartTurn;
    MyGameData.ShortTermEXP[_index] = _exp;
    UIManager.Instance.UpdateExpShortTermIcon();
  }
  public void AddLongExp(Experience _exp)
  {
    if (UIManager.Instance.MyQuestSuggent.IsActivePanel) UIManager.Instance.MyQuestSuggent.OpenStarting();
    //����Ʈ ���� �гο��� ���� �����ϴ� ����� ���� �ܰ�� �Ѿ��

    if (_exp.ExpType.Equals(ExpTypeEnum.Mad)) MyGameData.MadnessCount++;
    _exp.Duration = ConstValues.LongTermStartTurn;
    MyGameData.LongTermEXP = _exp;
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
  public void ShiftLongExp(Experience _exp)
  {
    _exp.Duration = ConstValues.LongTermStartTurn;
    Experience _target = MyGameData.LongTermEXP;
    MyGameData.LongTermEXP= _exp;
    MyGameData.CurrentSanity -= ConstValues.LongTermChangeCost;
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
    //���̾�α� ����
  }//�߿� �̵��� ���� �̺�Ʈ�� ���� ���
  public void SetSettlementPlace()
  {
    
    UIManager.Instance.OpenSuggestUI();
    //���� UI ����
    SaveData();
  }//�������� ��� ����
  public void SelectEvent(EventDataDefulat _targetevent)
  {
    MyGameData.CurrentSanity -= MyGameData.SettleSanityLoss;
    UIManager.Instance.UpdateSanityText();
    Dictionary<Settlement,int> _temp=new Dictionary<Settlement,int>();
    MyGameData.AddDiscomfort(MyGameData.CurrentSettlement);
    MyGameData.CurrentEvent = _targetevent;
    MyGameData.CurrentEventSequence = EventSequence.Progress;
    //���� �̺�Ʈ �����Ϳ� ����
    MyGameData.RemoveEvent.Add(_targetevent.ID);
    //���� �������� �ʰ�
    UIManager.Instance.OpenDialogue();
    SaveData();
  }//���� �гο��� �̺�Ʈ�� ������ ���
  public void SelectQuestEvent(EventDataDefulat questevent)
  {
    MyGameData.LastQuestCount = 0;
    Dictionary<Settlement, int> _temp = new Dictionary<Settlement, int>();
    MyGameData.CurrentEvent = questevent;
    MyGameData.CurrentEventSequence = EventSequence.Progress;
    //���� �̺�Ʈ �����Ϳ� ����
    MyGameData.RemoveEvent.Add(questevent.ID);
    //���� �������� �ʰ�
    UIManager.Instance.OpenDialogue();
    SaveData();
  }
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
    if(instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
      LoadData();
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

  public void StartNewGame(QuestHolder newquest)
  {
    UIManager.Instance.AddUIQueue(startnewgame(newquest));
  }
  private IEnumerator startnewgame(QuestHolder newquest)
  {
    MyGameData = new GameData();//���ο� ���� ������ ����
    MyGameData.CurrentQuest= newquest;
    EventHolder.SetAllEvents();

    yield return StartCoroutine(createnewmap());//�� �� �����

    yield return StartCoroutine(UIManager.Instance.opengamescene());
    UIManager.Instance.UpdateAllUI();

    UIManager.Instance.OpenQuestDialogue();
  }
  /// <summary>
  /// ����� �����ͷ� ���� ����
  /// </summary>
  public void LoadGame()
  {
    UIManager.Instance.AddUIQueue(loadgame());
  }
  private IEnumerator loadgame()
  {
    //���� �����ʹ� �̹� �ҷ��� ������ ���

    UIManager.Instance.CreateMap();
    UIManager.Instance.UpdateMap_SetPlayerPos();
    yield return StartCoroutine(UIManager.Instance.opengamescene());
    UIManager.Instance.UpdateAllUI();

    if (MyGameData.CurrentEvent == null)
    {
      UIManager.Instance.OpenSuggestUI();
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

    yield return new WaitUntil(()=>MyGameJsonData != null);
    MyMapData = MyGameJsonData.GetMapData();

    Settlement _startsettle = MyMapData.AllSettles[Random.Range(0, MyMapData.AllSettles.Count)];

    MyGameData.CurrentSettlement = _startsettle;
    MyGameData.AvailableSettles = MyMapData.GetCloseSettles(_startsettle, 3);
    MyGameData.CurrentPos = _startsettle.VectorPos;

    _map.MakeTilemap(MyMapData);
    UIManager.Instance.UpdateMap_SetPlayerPos(_startsettle);
    yield return null;
  }
}
