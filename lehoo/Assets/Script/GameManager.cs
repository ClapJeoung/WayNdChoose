using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;

public enum GameOverTypeEnum { HP,Sanity}
public class GameManager : MonoBehaviour
{
  private static GameManager instance;
  public static GameManager Instance { get { return instance; } }

  [HideInInspector] public GameData MyGameData = null;            //���� ������(���൵,���� ���� �� �̺�Ʈ, ���� �� ����,����Ʈ ���)
  [HideInInspector] public GameJsonData GameJsonData = null;
  [HideInInspector] public const string GameDataName = "WNCGameData.json";
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
  #region #������ ����#
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
  /// �̸�(X),�̸�(O),������,����,��������,+(X),++(X),-(X),--(X),+(O),++(O),-(O),--(O) 
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
      case 9:
        _name += "UP_NORMAL_ICON";
        break;
      case 10:
        _name += "UP_HIGH_ICON";
        break;
      case 11:
        _name += "DOWN_NORMAL_ICON";
        break;
      case 12:
        _name += "DOWN_HIGH_ICON";
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
  public string GetTextData(TendencyTypeEnum tendency,int level,int texttype)
  {
    string _name = tendency.Equals(TendencyTypeEnum.Body) ? "TENDENCY_BODY" : "TENDENCY_HEAD";
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
  public string GetTextData(SectorType sector,int texttype)
  {
    string _str = "PLACE_";
    switch (sector)
    {
      case SectorType.Residence: _str += "RESIDENCE";break;
      case SectorType.Temple: _str += "TEMPLE"; break;
      case SectorType.Marketplace: _str += "MARKETPLACE"; break;
      case SectorType.Library: _str += "LIBRARY"; break;
      case SectorType.Theater: _str += "THEATER"; break;
      case SectorType.Academy: _str += "ACADEMY"; break;
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
      GameJsonData = JsonUtility.FromJson<GameJsonData>(File.ReadAllText(Application.persistentDataPath + "/" + GameDataName));
      MyGameData = GameJsonData.GetGameData();
    }
    //����� �÷��̾� �����Ͱ� ������ ������ �ҷ�����

    Dictionary<string, TextData> _textdatas = JsonConvert.DeserializeObject<Dictionary<string, TextData>>(TextData.text);
    foreach (var _data in _textdatas)
    {
      string _texttemp = _data.Value.TEXT;
      if (_texttemp.Contains("\\n")) _texttemp = _texttemp.Replace("\\n", "\n");
      if (TextDic.ContainsKey(_data.Key)) { Debug.Log($"{_data.Key} ��ħ! Ȯ�� �ʿ�!"); continue; }
      if (TextDic.ContainsValue(_data.Value.TEXT)) { Debug.Log($"{_data.Value.TEXT} ��ħ! Ȯ�� �ʿ�!"); continue; }
      TextDic.Add(_data.Value.ID, _data.Value.TEXT);
    }

    if (NormalEventData != null)
    {
      Dictionary<string, EventJsonData> _eventjson = new Dictionary<string, EventJsonData>();
      _eventjson = JsonConvert.DeserializeObject<Dictionary<string, EventJsonData>>(NormalEventData.text);
      foreach (var _data in _eventjson) EventHolder.ConvertData_Normal(_data.Value);
      //�̺�Ʈ Json -> EventHolder
    }

    if (FollowEventData != null)
    {
      Dictionary<string, FollowEventJsonData> _followeventjson = new Dictionary<string, FollowEventJsonData>();
      _followeventjson = JsonConvert.DeserializeObject<Dictionary<string, FollowEventJsonData>>(FollowEventData.text);
      foreach (var _data in _followeventjson) EventHolder.ConvertData_Follow(_data.Value);
      //���� �̺�Ʈ Json -> EventHolder
    }

    if (EventHolder.Quest_Cult == null) EventHolder.Quest_Cult = new QuestHolder_Cult("Quest0", QuestType.Cult);
    if (QuestEventData != null)
    {
      Dictionary<string, QuestEventDataJson> _questeventjson = new Dictionary<string, QuestEventDataJson>();
      _questeventjson = JsonConvert.DeserializeObject<Dictionary<string, QuestEventDataJson>>(QuestEventData.text);
      foreach (var _data in _questeventjson) EventHolder.ConvertData_Quest(_data.Value);
      //����Ʈ Json -> EventHolder
    }


    if (EXPData != null)
    {
      Dictionary<string, ExperienceJsonData> _expjson = new Dictionary<string, ExperienceJsonData>();
      _expjson = JsonConvert.DeserializeObject<Dictionary<string, ExperienceJsonData>>(EXPData.text);
      foreach (var _data in _expjson)
      {
        Experience _exp = _data.Value.ReturnEXPClass();
        ExpDic.Add(_data.Value.ID, _exp);
        if (_exp.ExpType.Equals(ExpTypeEnum.Mad)) MadExpDic.Add(_data.Value.ID, _exp);
      }
      //���� Json -> EXPDic
    }

  }//���� Json �����ͼ� ��ȯ
  public void SaveData()
  {

  }//���� ������ ����
  #endregion

  public void RestInSector(SectorType sectortype,bool issanity)
  {
    if (issanity)
    {
      MyGameData.CurrentSanity -= MyGameData.SettleRestCost_Sanity;
    }
    else
    {
      MyGameData.Gold -= MyGameData.SettleRestCost_Gold;
    }
    switch (MyGameData.CurrentSettlement.SettlementType)
    {
      case SettlementType.Village:
        MyGameData.MovePoint += ConstValues.RestMovePoint_Village;
        break;
      case SettlementType.Town:
        MyGameData.MovePoint += ConstValues.RestMovePoint_Town;
        break;
      case SettlementType.City:
        MyGameData.MovePoint += ConstValues.RestMovePoint_City;
        break;
    }
    MyGameData.AddDiscomfort(MyGameData.CurrentSettlement);
    MyGameData.ApplySectorEffect(sectortype);

    EventManager.Instance.SetSettlementEvent(sectortype);

    switch (MyGameData.QuestType)
    {
      case QuestType.Cult:
        if (MyGameData.Quest_Cult_Sabbat_TokenedSectors[sectortype] == 0)
        {
          MyGameData.Quest_Cult_Progress += ConstValues.Quest_Cult_Sabbat_Progress_TokenSector;
          MyGameData.Quest_Cult_Sabbat_TokenedSectors[sectortype] = ConstValues.Quest_Wolf_TokenDuration;
        }
        else
        {
          MyGameData.Quest_Cult_Progress += ConstValues.Quest_Cult_Sabbat_Progress_NoTokenSector;
        }
        break;
    }
  }
  public void SuccessCurrentEvent(TendencyTypeEnum _tendencytype,int index)
  {
    int _tendencyindex = 0;
    switch (_tendencytype)
    {
      case TendencyTypeEnum.None:
        _tendencyindex = 0;
        break;
      case TendencyTypeEnum.Body:
        if (index.Equals(0)) _tendencyindex = 1;
        else _tendencyindex = 2;
        break;
      case TendencyTypeEnum.Head:
        if (index.Equals(0)) _tendencyindex = 3;
        else _tendencyindex = 4;
        break;
    }
    EventHolder.RemoveEvent(MyGameData.CurrentEvent,true,_tendencyindex);

    MyGameData.CurrentEventSequence = EventSequence.Clear;
    if (MyGameData.CurrentSettlement != null)
    {
      MyGameData.Turn++;
    }

    switch (MyGameData.QuestType)
    {
      case QuestType.Cult:
        if (MyGameData.CurrentEvent.GetType() == typeof(QuestEventData_Wolf))
        {
          if (MyGameData.Quest_Cult_Phase > 0)
          {
            switch (MyGameData.Quest_Cult_Type)
            {
              case 0:
                MyGameData.Quest_Cult_Progress += ConstValues.Quest_Cult_Ritual_Progress_EventClear;
                break;
              case 1:
                break;
            }
          }
          UIManager.Instance.QuestSidePanel_Cult.UpdateUI();
        }
        break;
    }
  }
  public void FailCurrentEvent(TendencyTypeEnum _tendencytype, int index)
  {
    int _tendencyindex = 0;
    switch (_tendencytype)
    {
      case TendencyTypeEnum.None:
        _tendencyindex = 0;
        break;
      case TendencyTypeEnum.Body:
        if (index.Equals(0)) _tendencyindex = 1;
        else _tendencyindex = 2;
        break;
      case TendencyTypeEnum.Head:
        if (index.Equals(0)) _tendencyindex = 3;
        else _tendencyindex = 4;
        break;
    }
    EventHolder.RemoveEvent(MyGameData.CurrentEvent, false, _tendencyindex);

    MyGameData.CurrentEventSequence = EventSequence.Clear;
    if (MyGameData.CurrentSettlement != null)
    {
      MyGameData.Turn++;
    }
    switch (MyGameData.QuestType)
    {
      case QuestType.Cult:
        if (MyGameData.CurrentEvent.GetType() == typeof(QuestEventData_Wolf))
        {
          if (MyGameData.Quest_Cult_Phase > 0)
          {
            switch (MyGameData.Quest_Cult_Type)
            {
              case 0:
                MyGameData.Quest_Cult_Progress += ConstValues.Quest_Cult_Ritual_Progress_EventFail;
                break;
              case 1:
                break;
            }
          }
          UIManager.Instance.QuestSidePanel_Cult.UpdateUI();
        }
        break;
    }
  }
  public void AddExp_Long(Experience exp)
  {
    if (exp.ExpType == ExpTypeEnum.Mad)
    {
      MyGameData.MaxSanity -= ConstValues.SanityLoseByMadnessExp;
      MyGameData.CurrentSanity = MyGameData.MaxSanity;
      UIManager.Instance.MyMadPanel.CloseUI();
    }
    MyGameData.LongTermEXP = exp;
    UIManager.Instance.UpdateExpLongTermIcon();
  }
  public void AddExp_Short(Experience exp,int index)
  {
    if (exp.ExpType == ExpTypeEnum.Mad)
    {
      MyGameData.MaxSanity -= ConstValues.SanityLoseByMadnessExp;
      MyGameData.CurrentSanity = MyGameData.MaxSanity;
      UIManager.Instance.MyMadPanel.CloseUI();
    }
    MyGameData.ShortTermEXP[index] = exp;
    UIManager.Instance.UpdateExpShortTermIcon();
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
    MyGameData.CurrentEvent = _event;
    MyGameData.CurrentEventSequence = EventSequence.Progress;
    //���� �̺�Ʈ �����Ϳ� ����
    UIManager.Instance.OpenDialogue();
    //���̾�α� ����
  }//�߿� �̵��� ���� �̺�Ʈ�� ���� ���
  public void SelectEvent(EventDataDefulat _targetevent)
  {
    MyGameData.CurrentEvent = _targetevent;
    MyGameData.CurrentEventSequence = EventSequence.Progress;
    //���� �̺�Ʈ �����Ϳ� ����
    UIManager.Instance.OpenDialogue();
    SaveData();
  }//���� �гο��� �̺�Ʈ�� ������ ���
  public void SelectQuestEvent(EventDataDefulat questevent)
  {
    Dictionary<Settlement, int> _temp = new Dictionary<Settlement, int>();
    MyGameData.CurrentEvent = questevent;
    MyGameData.CurrentEventSequence = EventSequence.Progress;
    //���� �̺�Ʈ �����Ϳ� ����
    UIManager.Instance.OpenDialogue();
    SaveData();
  }
  public void AddTendencyCount(TendencyTypeEnum _tendencytype,int index)
  {
    switch (_tendencytype)
    {
      case TendencyTypeEnum.Body:
        MyGameData.Tendency_Body.AddCount(index.Equals(0) ? true : false);
        break;
      case TendencyTypeEnum.Head:
        MyGameData.Tendency_Head.AddCount(index.Equals(0) ? true : false);
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
      MyGameData = new GameData();
      CreateNewMap();

    }
  }
  public void GameOver(GameOverTypeEnum gameovertype)
  {
    UIManager.Instance.GameOver(gameovertype);
  }
  public void StartNewGame(QuestType newquest)
  {
    UIManager.Instance.AddUIQueue(startnewgame(newquest));
  }
  private IEnumerator startnewgame(QuestType newquest)
  {
    MyGameData = new GameData();//���ο� ���� ������ ����
    MyGameData.QuestType= newquest;

    yield return StartCoroutine(createnewmap());//�� �� �����

    Settlement _randomsettle=MyGameData.MyMapData.AllSettles[Random.Range(0,MyGameData.MyMapData.AllSettles.Count)];
    MyGameData.CurrentSettlement= _randomsettle;
    MyGameData.Coordinate=_randomsettle.Tiles[Random.Range(0,_randomsettle.Tiles.Count)].Coordinate;
    UIManager.Instance.UpdateAllUI();

    yield return StartCoroutine(UIManager.Instance.opengamescene());
    UIManager.Instance.UpdateMap_SetPlayerPos(MyGameData.Coordinate);
    switch (MyGameData.QuestType)
    {
      case QuestType.Cult: UIManager.Instance.QuestUI_Cult.OpenUI_Prologue((QuestHolder_Cult)MyGameData.CurrentQuestData); break;
    }
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

    /*
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
    */
    yield return null;
  }
  public void CreateNewMap() => StartCoroutine(createnewmap());

  private IEnumerator createnewmap()
  {
    maptext _map = FindObjectOfType<maptext>().GetComponent<maptext>();

    _map.MakePerfectMap();

    yield return new WaitUntil(()=>MyGameData.MyMapData != null);

    Settlement _startsettle = MyGameData.MyMapData.AllSettles[Random.Range(0, MyGameData.MyMapData.AllSettles.Count)];

    MyGameData.CurrentSettlement = _startsettle;
    MyGameData.Coordinate = _startsettle.Position;

    _map.MakeTilemap();
    UIManager.Instance.UpdateMap_SetPlayerPos(_startsettle.Tiles[Random.Range(0,_startsettle.Tiles.Count)].Coordinate);
    yield return null;
  }
  public void EnterSettlement(Settlement targetsettlement)
  {
    MyGameData.CurrentSettlement=targetsettlement;

    switch (MyGameData.QuestType)
    {
      case QuestType.Cult:
        if (MyGameData.Quest_Cult_Phase == 0)
        {
          EventManager.Instance.SetQuestEvent_Wolf_Searching();
        }//Ž�� �ܰ�
        else if (MyGameData.Quest_Cult_Phase == 1)
        {
          switch (MyGameData.Quest_Cult_Type)
          {
            case 0:
              break;

            case 1:
              break;
          }
        }
        break;
    }

  }
}
public class TextData
{
 public string ID = "", TEXT = "", ETC = "";
}
