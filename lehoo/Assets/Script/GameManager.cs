using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;
using System.Linq;
using System;

public enum GameOverTypeEnum { HP,Sanity}
public class GameManager : MonoBehaviour
{
  [ContextMenu("이벤트 데이터 업데이트")]
  void EventDataUpdate()
  {
    StartCoroutine(eventdataupdate());
  }
  private IEnumerator eventdataupdate()
  {
    UnityWebRequest _sheet_event = UnityWebRequest.Get(SeetURL_Event);
    yield return _sheet_event.SendWebRequest();

    EventJsonDataList.Clear();
    string[] _eventrow = _sheet_event.downloadHandler.text.Split('\n');
    for (int i = 1; i < _eventrow.Length; i++)
    {
      string[] _data = _eventrow[i].Split('\t');
      if (_data[1] == "") continue;

      EventJsonData _json = new EventJsonData();
      _json.ID = _data[1];

      _json.EventInfo = _data[0];
      _json.PlaceInfo = _data[2];
      _json.Season= _data[3];

      _json.Selection_Type= _data[4];
      _json.Selection_Target= _data[5];
      _json.Selection_Info= _data[6];

      _json.Failure_Penalty= _data[7];
      _json.Failure_Penalty_info= _data[8];

      _json.Reward_Target= _data[9];
      _json.Reward_Info= _data[10];

      _json.EndingID = _data[11];
      EventJsonDataList.Add(_json);
    }
    print("이벤트 데이터 업데이트 완료");
  }
  [ContextMenu("경험 데이터 업데이트")]
  void ExpDataUpdate()
  {
    StartCoroutine(expdataupdate());
  }
  private IEnumerator expdataupdate()
  {
    UnityWebRequest _sheet_exp = UnityWebRequest.Get(SeetURL_Exp);
    yield return _sheet_exp.SendWebRequest();
    ExpJsonDataList.Clear();
    string[] _exprow = _sheet_exp.downloadHandler.text.Split('\n');
    for (int i = 1; i < _exprow.Length; i++)
    {
      string[] _data = _exprow[i].Split('\t');
      if (_data[0] == "") continue;
      ExperienceJsonData _json = new ExperienceJsonData();

      _json.ID = _data[0];
      _json.Type = _data[2];

      ExpJsonDataList.Add(_json);
    }
    print("경험 데이터 업데이트 완료");
  }
  [ContextMenu("텍스트 데이터 업데이트")]
  void TextDataUpdate()
  {
    StartCoroutine(textdataupdate());
  }
  private IEnumerator textdataupdate()
  {
    int _previewcount=TextDatas.Count;

    UnityWebRequest _sheet_text = UnityWebRequest.Get(SeetURL_Text);
    yield return _sheet_text.SendWebRequest();
    string[] _textrow = _sheet_text.downloadHandler.text.Split('\n');
    TextDatas.Clear();

    for (int i = 1; i < _textrow.Length; i++)
    {
      string[] _data = _textrow[i].Split("\t");
      if (_data.Length == 0 || _data[0] == "") continue;
      Textdata _textdata = new Textdata();
      _textdata.ID = _data[0];
      _textdata.kor = _data[1];
      TextDatas.Add(_textdata);
    }
    print($"텍스트 딕셔너리 업데이트 완료\n{_previewcount}개 -> {TextDatas.Count}개");
  }

  private static GameManager instance;
  public static GameManager Instance { get { return instance; } }
  public AudioManager AudioManager = null;

  [HideInInspector] public GameData MyGameData = null;            //게임 데이터(진행도,현재 진행 중 이벤트, 현재 맵 상태,퀘스트 등등)

  [HideInInspector] public GameJsonData GameJsonData = null;
  [HideInInspector] public const string GameDataName = "WNCGameData.json";
  [HideInInspector] public ProgressData MyProgressData = new ProgressData();

  public ImageHolder ImageHolder = null;             //이벤트,경험,특성,정착지 일러스트 홀더

  private const string SeetURL_Event = "https://docs.google.com/spreadsheets/d/1fbo8PVBwDS7RGBJwD-By54Hvk3Gtb-rXqh9HxIGtZn0/export?format=tsv&gid=0";
  private const string SeetURL_Exp = "https://docs.google.com/spreadsheets/d/1fbo8PVBwDS7RGBJwD-By54Hvk3Gtb-rXqh9HxIGtZn0/export?format=tsv&gid=634251844";
  private const string SeetURL_Text = "https://docs.google.com/spreadsheets/d/1fbo8PVBwDS7RGBJwD-By54Hvk3Gtb-rXqh9HxIGtZn0/export?format=tsv&gid=1628546529";
  public List<EventJsonData> EventJsonDataList= new List<EventJsonData>();
  public List<ExperienceJsonData> ExpJsonDataList= new List<ExperienceJsonData>();
  private IEnumerator loadspreadsheetdatas()
  {

    if (EventHolder.Quest_Cult == null) EventHolder.Quest_Cult = new QuestHolder_Cult("Cult", QuestType.Cult);

    foreach (var _data in EventJsonDataList)
    {
      string _eventinfo = _data.EventInfo.Split('@')[0];
      if (_eventinfo == null || _eventinfo == "0"||_eventinfo=="") EventHolder.ConvertData_Normal(_data);
      else if (_eventinfo == "1") EventHolder.ConvertData_Follow(_data);
      else EventHolder.ConvertData_Quest(_data);
    }

    foreach (var _data in ExpJsonDataList)
    {
      Experience _exp = _data.ReturnEXPClass();
      ExpDic.Add(_data.ID, _exp);
    }

    yield return null;
  }


  public EventHolder EventHolder = new EventHolder();                               //이벤트 저장할 홀더
  public Dictionary<string, Experience> ExpDic = new Dictionary<string, Experience>();  //경험 딕셔너리
  public List<Textdata> TextDatas = new List<Textdata>();
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
        goldfaildata.Penelty_target = PenaltyTarget.Status;
        goldfaildata.StatusType = StatusTypeEnum.Sanity;
        goldfaildata.Illust = ImageHolder.NoGoldIllust;

      }
      return goldfaildata;
    }
  }
  #region #데이터 관련#
  public string GetTextData(string _id)
  {
    foreach(var _data in TextDatas)
    {
      if (string.Compare(_id, _data.ID, true) == 0) return _data.Text;
    }
    
    Debug.Log($"{_id} 없음"); 
    return NullText;
  }
  /// <summary>
  /// texttype : 이름/설명
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
  /// 이름(X),이름(O),아이콘,설명,간략설명,+(X),++(X),-(X),--(X),+(O),++(O),-(O),--(O) 
  /// </summary>
  /// <param name="_theme"></param>
  /// <param name="texttype"></param>
  /// <returns></returns>
  public string GetTextData(SkillTypeEnum skilltype,int texttype)
  {
    string _name = "";
    switch (skilltype)
    {
      case SkillTypeEnum.Conversation: _name = "CONVERSATION"; break;
      case SkillTypeEnum.Force: _name = "FORCE"; break;
      case SkillTypeEnum.Wild: _name = "WILD"; break;
      case SkillTypeEnum.Intelligence: _name = "INTELLIGENCE"; break;
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
  public string GetTextData(SkillTypeEnum skilltype, bool isup,bool isstrong,bool isicon)
  {
    string _name = "";
    switch (skilltype)
    {
      case SkillTypeEnum.Conversation: _name = "CONVERSATION"; break;
      case SkillTypeEnum.Force: _name = "FORCE"; break;
      case SkillTypeEnum.Wild: _name = "WILD"; break;
      case SkillTypeEnum.Intelligence: _name = "INTELLIGENCE"; break;
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
      case EffectType.Conversation: return GetTextData(SkillTypeEnum.Conversation,true,false,isicon);
      case EffectType.Force: return GetTextData(SkillTypeEnum.Force , true, false, isicon);
      case EffectType.Wild: return GetTextData(SkillTypeEnum.Wild, true, false, isicon);
      case EffectType.Intelligence: return GetTextData(SkillTypeEnum.Intelligence, true, false, isicon);
      case EffectType.HPLoss: return GetTextData(StatusTypeEnum.HP,false,isicon?2:1);
      case EffectType.HPGen: return GetTextData(StatusTypeEnum.HP, true, isicon?2:1);
      case EffectType.SanityLoss: return GetTextData(StatusTypeEnum.Sanity, false, isicon?2:1);
      case EffectType.SanityGen: return GetTextData(StatusTypeEnum.Sanity, true, isicon?2:1);
      case EffectType.GoldLoss: return GetTextData(StatusTypeEnum.Gold, false, isicon?2:1);
      case EffectType.GoldGen: return GetTextData(StatusTypeEnum.Gold, true, isicon?2:1);
      default: return NullText;
    }
  }
  /// <summary>
  /// 이름 설명 간략설명 아이콘
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
      case 1: _type = "DESCRIPTION"; break;
      case 2: _type = "SUBDESCRIPTION"; break;
      case 3: _type = "ICON"; break;
    }
    return GetTextData(_name+"_" + _level+"_"+_type);
  }
  /// <summary>
  /// 이름 설명 아이콘 효과 효과설명
  /// </summary>
  /// <param name="_place"></param>
  /// <param name="texttype"></param>
  /// <returns></returns>
  public string GetTextData(SectorTypeEnum sector,int texttype)
  {
    string _str = "PLACE_";
    switch (sector)
    {
      case SectorTypeEnum.Residence: _str += "RESIDENCE";break;
      case SectorTypeEnum.Temple: _str += "TEMPLE"; break;
      case SectorTypeEnum.Marketplace: _str += "MARKETPLACE"; break;
      case SectorTypeEnum.Library: _str += "LIBRARY"; break;
      case SectorTypeEnum.Theater: _str += "THEATER"; break;
      case SectorTypeEnum.Academy: _str += "ACADEMY"; break;
    }
    _str += "_";
    switch (texttype)
    {
      case 0: _str += "NAME"; break;
      case 1: _str += "DESCRIPTION";break;
      case 2: _str += "ICON";break;
      case 3: _str += "EFFECT_DESCRIPTION"; break;
      case 4: _str += "EFFECT_DESCRIPTION";break;
    }

    return GetTextData(_str);
  }
  /// <summary>
  /// 이름(X) 이름(O) 아이콘 설명 간략설명|회복(X) 회복(O) 회복아이콘|소모(X) 소모(O) 소모아이콘|회복증가(X) 회복증가(O) 회복아이콘|소모증가(X) 소모증가(O) 소모아이콘 
  /// </summary>
  /// <param name="type"></param>
  /// <returns></returns>
  public string GetTextData(StatusTypeEnum type,int texttype)
  {
    string _str = "";
    switch (type)
    {
      case StatusTypeEnum.HP: _str = "HP";break;
      case StatusTypeEnum.Sanity:_str = "SANITY";break;
      case StatusTypeEnum.Gold: _str = "GOLD";break;
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
  /// 아이콘X 아이콘O 아이콘
  /// </summary>
  /// <param name="type"></param>
  /// <param name="isincrease"></param>
  /// <param name="icontype"></param>
  /// <returns></returns>
  public string GetTextData(StatusTypeEnum type,bool isincrease,int icontype)
  {
    int _typecode = isincrease ? 11 : 14;
    _typecode += icontype;
    return GetTextData(type, _typecode);
  }
    public void LoadData()
  {
    if (System.IO.File.Exists(Application.persistentDataPath+"/"+GameDataName ))
    {
      GameJsonData = JsonUtility.FromJson<GameJsonData>(System.IO.File.ReadAllText(Application.persistentDataPath + "/" + GameDataName));
      MyGameData = GameJsonData.GetGameData();
    }
    //저장된 플레이어 데이터가 있으면 데이터 불러오기

    UIManager.Instance.AddUIQueue((loadspreadsheetdatas()));
  }//각종 Json 가져와서 변환
  public void SaveData()
  {

  }//현재 데이터 저장
  #endregion

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

    switch (MyGameData.QuestType)
    {
      case QuestType.Cult:
        UIManager.Instance.CultUI.AddProgress(0);
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

    switch (MyGameData.QuestType)
    {
      case QuestType.Cult:
        UIManager.Instance.CultUI.AddProgress(1);

        break;
    }
  }
  public void AddExp_Long(Experience exp)
  {
    exp.Duration = ConstValues.LongTermStartTurn;
    MyGameData.LongExp = exp;

    UIManager.Instance.UpdateExpPael();
    UIManager.Instance.UpdateSkillLevel();
  }
  /// <summary>
  /// 0:A 1:B
  /// </summary>
  /// <param name="exp"></param>
  /// <param name="index"></param>
  public void AddExp_Short(Experience exp,bool index)
  {
    exp.Duration = ConstValues.ShortTermStartTurn;
    if(index==true)MyGameData.ShortExp_A=exp;
    else MyGameData.ShortExp_B=exp;

    UIManager.Instance.UpdateExpPael();
    UIManager.Instance.UpdateSkillLevel();
  }
  public void SetOuterEvent(EventData _event)
  {
    MyGameData.CurrentEvent = _event;
    MyGameData.CurrentEventSequence = EventSequence.Progress;
    //현재 이벤트 데이터에 삽입
    UIManager.Instance.OpenDialogue(false);
    //다이어로그 열기
  }//야외 이동을 통해 이벤트를 받은 경우
  public void SelectEvent(EventData _targetevent)
  {
    MyGameData.CurrentEvent = _targetevent;
    MyGameData.CurrentEventSequence = EventSequence.Progress;
    //현재 이벤트 데이터에 삽입
    UIManager.Instance.OpenDialogue(false);
    SaveData();
  }//정착지 패널에서 이벤트를 선택한 경우
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
      if(PlayerPrefs.GetInt("LanguageIndex", -1) == -1)
      {
        SystemLanguage _lang = Application.systemLanguage;
        PlayerPrefs.SetInt("LanguageIndex", (int)_lang);
      }
      LoadData();
      //  DebugAllEvents();
    }

  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Backspace))
    {
      MyGameData = new GameData();
      CreateNewMap();

    }
  }
  public void GameOver()
  {
    Sprite _illust = null;
    string _description = "";
    if(MyGameData.Madness_Conversation==false&&
      MyGameData.Madness_Force==false&&
      MyGameData.Madness_Wild==false&&
      MyGameData.Madness_Intelligence == false)
    {
      _illust = ImageHolder.GameOver_Idle;
      _description = GetTextData("GameOver_Normal");
    }
    else if(MyGameData.Madness_Conversation==true&&
      MyGameData.Madness_Force== true &&
      MyGameData.Madness_Wild== true &&
      MyGameData.Madness_Intelligence == true)
    {
      _illust = ImageHolder.GameOver_Madness;
      _description = GetTextData("GameOver_Mad");
    }
    else
    {
      List<int> _ableindexes = new List<int>();
      if (MyGameData.Madness_Conversation == true) _ableindexes.Add(0);
      if (MyGameData.Madness_Force == true) _ableindexes.Add(1);
      if (MyGameData.Madness_Wild == true) _ableindexes.Add(2);
      if (MyGameData.Madness_Intelligence == true) _ableindexes.Add(3);
      switch (_ableindexes[UnityEngine.Random.Range(0, _ableindexes.Count - 1)])
      {
        case 0:
          _illust = ImageHolder.GameOver_Conversation;
          _description = GetTextData("GameOver_Conversation");
          break;
        case 1:
          _illust = ImageHolder.GameOver_Force;
          _description = GetTextData("GameOver_GameOver_Mad");
          break;
        case 2:
          _illust = ImageHolder.GameOver_Wild;
          _description = GetTextData("GameOver_Wild");
          break;
        case 3:
          _illust = ImageHolder.GameOver_Intelligence;
          _description = GetTextData("GameOver_Intelligence");
          break;
      }
    }

    UIManager.Instance.OpenDead(_illust, _description);
  }
  public void SubEnding(EndingIllusts endingdata)
  {
    Tuple<List<Sprite>, List<string>, string, string> _temp =
      new Tuple<List<Sprite>, List<string>, string, string>(endingdata.Illusts, endingdata.Descriptions, endingdata.LastWord, "");

    UIManager.Instance.OpenEnding(_temp);
  }

  public void StartNewGame(QuestType newquest)
  {
    UIManager.Instance.AddUIQueue(startnewgame(newquest));
  }
  private IEnumerator startnewgame(QuestType newquest)
  {
    MyGameData = new GameData();//새로운 게임 데이터 생성
    MyGameData.QuestType= newquest;

    yield return StartCoroutine(createnewmap());//새 맵 만들기

    switch (MyGameData.QuestType)
    {
      case QuestType.Cult: UIManager.Instance.CultUI.OpenUI_Prologue((QuestHolder_Cult)MyGameData.CurrentQuestData); break;
    }

    UIManager.Instance.UpdateAllUI();

    yield return StartCoroutine(UIManager.Instance.opengamescene());

  }
  /// <summary>
  /// 저장된 데이터로 게임 시작
  /// </summary>
  public void LoadGame()
  {
    UIManager.Instance.AddUIQueue(loadgame());
  }
  private IEnumerator loadgame()
  {
    //게임 데이터는 이미 불러온 데이터 사용

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
    */
    yield return null;
  }
  public void CreateNewMap() => StartCoroutine(createnewmap());

  private IEnumerator createnewmap()
  {

    maptext _map = FindObjectOfType<maptext>().GetComponent<maptext>();

    _map.MakePerfectMap();

    yield return new WaitUntil(()=>MyGameData.MyMapData != null);

    List<TileData> _randomstartlands = MyGameData.MyMapData.GetEnvirTiles(new List<BottomEnvirType> { BottomEnvirType.Land }, new List<TopEnvirType> { TopEnvirType.Mountain }, 1);

    MyGameData.Coordinate = _randomstartlands[UnityEngine.Random.Range(0, _randomstartlands.Count)].Coordinate;

    _map.MakeTilemap();
    UIManager.Instance.UpdateMap_SetPlayerPos();
    yield return null;
  }
  public void EnterSettlement(Settlement targetsettlement)
  {
    MyGameData.CurrentSettlement=targetsettlement;

    switch (MyGameData.QuestType)
    {
      case QuestType.Cult:
        switch (MyGameData.Quest_Cult_Phase)
        {
          case 0:
            if (!MyGameData.Cult_SettlementTypes.Contains(targetsettlement.SettlementType))
            {
              UIManager.Instance.CultUI.AddProgress(2);
              MyGameData.Cult_SettlementTypes.Add(targetsettlement.SettlementType);
            }
            break;
          case 1:
            MyGameData.Cult_SabbatSector = targetsettlement.Sectors[UnityEngine.Random.Range(0, targetsettlement.Sectors.Count)];
            break;
          case 2:
            if (MyGameData.Cult_SabbatSector_CoolDown == 0)
              MyGameData.Cult_SabbatSector = targetsettlement.Sectors[UnityEngine.Random.Range(0, targetsettlement.Sectors.Count)];
            else MyGameData.Cult_SabbatSector = SectorTypeEnum.NULL;

            break;
        }
        break;
    }

    UIManager.Instance.MySettleUI.OpenUI(false);
  }
  public List<HexDir> GetLength(TileData start, TileData end)
  {

    HexGrid _current = new HexGrid(start.Coordinate);
    HexGrid _end=new HexGrid(end.Coordinate);

    HexGrid _distance = _end - _current;

    return _distance.GetDir;
  }
  public List<HexDir> GetLength(Vector2 start, Vector2 end)
  {
    return GetLength(MyGameData.MyMapData.Tile(start),
      MyGameData.MyMapData.Tile(end));
  }
  public List<HexDir> GetLength(Vector2Int start, Vector2Int end)
  {
    return GetLength(MyGameData.MyMapData.Tile(start),
      MyGameData.MyMapData.Tile(end));
  }

  [System.Serializable]
  public class Textdata
  {
    public string ID = "";
    public string kor = "";
      public string Text
    {
      get
      {
        int _languagecode = PlayerPrefs.GetInt("LanguageINdex");
        switch ((SystemLanguage)_languagecode)
        {
          case SystemLanguage.Afrikaans:
            break;
          case SystemLanguage.Arabic:
            break;
          case SystemLanguage.Basque:
            break;
          case SystemLanguage.Belarusian:
            break;
          case SystemLanguage.Bulgarian:
            break;
          case SystemLanguage.Catalan:
            break;
          case SystemLanguage.Chinese:
            break;
          case SystemLanguage.Czech:
            break;
          case SystemLanguage.Danish:
            break;
          case SystemLanguage.Dutch:
            break;
          case SystemLanguage.English:
            break;
          case SystemLanguage.Estonian:
            break;
          case SystemLanguage.Faroese:
            break;
          case SystemLanguage.Finnish:
            break;
          case SystemLanguage.French:
            break;
          case SystemLanguage.German:
            break;
          case SystemLanguage.Greek:
            break;
          case SystemLanguage.Hebrew:
            break;
          case SystemLanguage.Hungarian:
            break;
          case SystemLanguage.Icelandic:
            break;
          case SystemLanguage.Indonesian:
            break;
          case SystemLanguage.Italian:
            break;
          case SystemLanguage.Japanese:
            break;
          case SystemLanguage.Korean:
            return kor;
          case SystemLanguage.Latvian:
            break;
          case SystemLanguage.Lithuanian:
            break;
          case SystemLanguage.Norwegian:
            break;
          case SystemLanguage.Polish:
            break;
          case SystemLanguage.Portuguese:
            break;
          case SystemLanguage.Romanian:
            break;
          case SystemLanguage.Russian:
            break;
          case SystemLanguage.SerboCroatian:
            break;
          case SystemLanguage.Slovak:
            break;
          case SystemLanguage.Slovenian:
            break;
          case SystemLanguage.Spanish:
            break;
          case SystemLanguage.Swedish:
            break;
          case SystemLanguage.Thai:
            break;
          case SystemLanguage.Turkish:
            break;
          case SystemLanguage.Ukrainian:
            break;
          case SystemLanguage.Vietnamese:
            break;
          case SystemLanguage.ChineseSimplified:
            break;
          case SystemLanguage.ChineseTraditional:
            break;
          case SystemLanguage.Unknown:
            break;
        }
        return kor;
      }
    }
}
}
