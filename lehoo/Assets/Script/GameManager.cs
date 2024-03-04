using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;
using System.Linq;
using System;
using Steamworks;
using System.Text;

public enum GameOverTypeEnum { HP,Sanity}
public class GameManager : MonoBehaviour
{
  [ContextMenu("텍스트 체크")]
  public void TextCheck()
  {
    List<string> _errortexts= new List<string>();
    foreach(var _event in EventHolder.AllEvent)
    {
      if(_event.Name==NullText)_errortexts.Add(_event.ID+".Name");
      foreach(var _string in _event.BeginningDescriptions)
      if (_string == NullText) _errortexts.Add(_event.ID + ".BeginningDescriptioins");
      for(int i=0;i<_event.SelectionDatas.Length;i++) 
      {
        if (_event.SelectionDatas[i].Name == NullText) _errortexts.Add(_event.ID + $"Selection[{i}].Name");
        foreach (var _string in _event.SelectionDatas[i].SuccessData.Descriptions)
          if (_string == NullText) _errortexts.Add(_event.ID + $".{i}SuccessDescription");
        if(_event.SelectionDatas[i].FailData!=null)
          foreach (var _string in _event.SelectionDatas[i].FailData.Descriptions)
            if (_string == NullText) _errortexts.Add(_event.ID + $".{i}FailDescription");
      }
    }
    foreach (var _dat in _errortexts) Debug.Log(_dat);
  }
  [ContextMenu("데이터 업데이트")]
  public void UpdateSpreadsheetData()
  {
    StartCoroutine(updatesheet());
  }
  private IEnumerator updatesheet()
  {
    yield return StartCoroutine(textdataupdate());
    yield return StartCoroutine(expdataupdate());
    yield return StartCoroutine(eventdataupdate());
  }
  private void EventDataUpdate()
  {
    StartCoroutine(updatedatas());
  }
  public bool IsPlaying = false;
  private IEnumerator updatedatas()
  {
#if UNITY_EDITOR
    yield return StartCoroutine(updatesheet());
#endif
    yield return StartCoroutine((ConvertSheetDatas()));

    if (System.IO.File.Exists(Application.persistentDataPath + "/" + GameDataName))
    {
      GameSaveData = JsonUtility.FromJson<GameJsonData>(System.IO.File.ReadAllText(Application.persistentDataPath + "/" + GameDataName));
      try
      {
        MyGameData = new GameData(GameSaveData);
      }
      catch (Exception e)
      {
        Debug.Log(e);
        if (System.IO.File.Exists(Application.persistentDataPath + "/" + GameDataName)) System.IO.File.Delete(Application.persistentDataPath + "/" + GameDataName);
      }
    }
    //저장된 플레이어 데이터가 있으면 데이터 불러오기

 //   Debug.Log(GetTextData("ProgressInfo"));
    UIManager.Instance.MainUi.SetupMain();
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

      _json.Selection_Type= _data[3];
      _json.Selection_Target= _data[4];
      _json.Selection_Info= _data[5];

      _json.Failure_Penalty= _data[6];
      _json.Failure_Penalty_info= _data[7];

      _json.Reward_Target= _data[8];
      _json.Reward_Info= _data[9];

      _json.EndingID = _data[10];
      EventJsonDataList.Add(_json);
    }
    print("이벤트 데이터 업데이트 완료");

    yield return null;
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
      _json.Type = _data[1];

      ExpJsonDataList.Add(_json);
    }
    print("경험 데이터 업데이트 완료");

    yield return null;
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
      _textdata.kor = _data[1].Contains("\r")?_data[1].Replace("\r",""):_data[1];
      _textdata.en = _data[2].Contains("\r") ? _data[2].Replace("\r", "") : _data[2];
      TextDatas.Add(_textdata);
    }
    print($"텍스트 딕셔너리 업데이트 완료\n{_previewcount}개 -> {TextDatas.Count}개");

    yield return null;
  }

  private static GameManager instance;
  public static GameManager Instance { get { return instance; } }

  [HideInInspector] public GameData MyGameData = null;            //게임 데이터(진행도,현재 진행 중 이벤트, 현재 맵 상태,퀘스트 등등)

  public GameJsonData GameSaveData = null;
  public const string GameDataName = "WNCGameData.json";
  [HideInInspector] public ProgressData MyProgressData = new ProgressData();

  public ImageHolder ImageHolder = null;             //이벤트,경험,특성,정착지 일러스트 홀더

  private const string SeetURL_Event = "https://docs.google.com/spreadsheets/d/1fbo8PVBwDS7RGBJwD-By54Hvk3Gtb-rXqh9HxIGtZn0/export?format=tsv&gid=0";
  private const string SeetURL_Exp = "https://docs.google.com/spreadsheets/d/1fbo8PVBwDS7RGBJwD-By54Hvk3Gtb-rXqh9HxIGtZn0/export?format=tsv&gid=634251844";
  private const string SeetURL_Text = "https://docs.google.com/spreadsheets/d/1fbo8PVBwDS7RGBJwD-By54Hvk3Gtb-rXqh9HxIGtZn0/export?format=tsv&gid=1628546529";
  public List<EventJsonData> EventJsonDataList= new List<EventJsonData>();
  public List<ExperienceJsonData> ExpJsonDataList= new List<ExperienceJsonData>();
  private IEnumerator ConvertSheetDatas()
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
      if (string.Compare(_id, _data.ID, true) == 0)
        return _data.Text;
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
      case EffectType.SanityLoss: return GetTextData(StatusTypeEnum.Sanity, false, isicon?2:1);
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
     // case SectorTypeEnum.Theater: _str += "THEATER"; break;
    //  case SectorTypeEnum.Academy: _str += "ACADEMY"; break;
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
  public string GetTextData(SettlementType type)
  {
    switch (type)
    {
      case SettlementType.Village:return GetTextData("Village");
      case SettlementType.Town: return GetTextData("Town");
      case SettlementType.City: return GetTextData("City");
    }

    return NullText;
  }

  public void SaveData()
  {
    GameJsonData _newjsondata=new GameJsonData(MyGameData);
    string _json = JsonUtility.ToJson(_newjsondata);
    System.IO.File.WriteAllText(Application.persistentDataPath + "/" + GameDataName, _json);

    //여러 파일들을 한번에 저장할 때 EndFileWriteBatch로 감싸주도록 한다.
    //Steamworks.SteamRemoteStorage.BeginFileWriteBatch();

    //총 할당량과 현재 남은 용량을 조회할 수 있다.
    //Steamworks.SteamRemoteStorage.GetQuota(out totalBytes, out availableBytes);

   // var _byte = Encoding.UTF8.GetBytes(_json);
    //실제 클라우드에 저장을 위한 함수이지만 동기화를 위해 별도의 장소에 저장을 해둔다.  실제로 네트워크를 통한 동기화는 이 시점에서는 진행하지 않는다.  비동기나 동기 여부에 따라 FileWriteAsync 또는 FileWrite를 사용한다.  fileStr은 경로가 아니라 저장하는 파일의 이름과 확장자만을 포함해야 한다. (ex. savedata.dat)
   // Steamworks.SteamRemoteStorage.FileWriteAsync(GameDataName, _byte, (uint)_byte.Length);

    //BeginFileWriteBatch()를 사용했다면 이 함수로 마무리한다.
    //Steamworks.SteamRemoteStorage.EndFileWriteBatch();
  }//현재 데이터 저장
  #endregion

  public void SuccessCurrentEvent(TendencyTypeEnum _tendencytype,bool dir)
  {
    int _tendencyindex = 0;
    switch (_tendencytype)
    {
      case TendencyTypeEnum.None:
        _tendencyindex = 0;
        break;
      case TendencyTypeEnum.Body:
        if (dir) _tendencyindex = 1;
        else _tendencyindex = 2;
        break;
      case TendencyTypeEnum.Head:
        if (dir) _tendencyindex = 3;
        else _tendencyindex = 4;
        break;
    }
    EventHolder.RemoveEvent(MyGameData.CurrentEvent,true,_tendencyindex);

    MyGameData.CurrentEventSequence = EventSequence.Clear;

    switch (MyGameData.QuestType)
    {
      case QuestType.Cult:
        UIManager.Instance.CultUI.AddProgress(0, null);
        break;
    }
  }
  public void FailCurrentEvent(TendencyTypeEnum _tendencytype, bool dir)
  {
    int _tendencyindex = 0;
    switch (_tendencytype)
    {
      case TendencyTypeEnum.None:
        _tendencyindex = 0;
        break;
      case TendencyTypeEnum.Body:
        if (dir) _tendencyindex = 1;
        else _tendencyindex = 2;
        break;
      case TendencyTypeEnum.Head:
        if (dir) _tendencyindex = 3;
        else _tendencyindex = 4;
        break;
    }
    EventHolder.RemoveEvent(MyGameData.CurrentEvent, false, _tendencyindex);

    MyGameData.CurrentEventSequence = EventSequence.Clear;

    switch (MyGameData.QuestType)
    {
      case QuestType.Cult:
        UIManager.Instance.CultUI.AddProgress(1, null);

        break;
    }
  }
  public void AddExp_Long(Experience exp,bool sanityloss)
  {
    UIManager.Instance.SetInfoPanel(string.Format(GetTextData("GainExp"), exp.Name));
    if (sanityloss) MyGameData.Sanity -= (int)(ConstValues.LongTermChangeCost * MyGameData.GetSanityLossModify(true, 0));

    exp.Duration = ConstValues.EXPMaxTurn_long_idle;
    MyGameData.LongExp = exp;

    if (MyGameData.Madness_Intelligence)
    {
      if (MyGameData.ShortExp_A != null)
      {
        MyGameData.ShortExp_A.Duration -= ConstValues.MadnessEffect_Intelligence;
        UIManager.Instance.UpdateExpMad(1);
      }
      if (MyGameData.ShortExp_B != null)
      {
        MyGameData.ShortExp_B.Duration -= ConstValues.MadnessEffect_Intelligence;
        UIManager.Instance.UpdateExpMad(2);
      }

      UIManager.Instance.HighlightManager.Highlight_Madness(SkillTypeEnum.Intelligence);
      UIManager.Instance.AudioManager.PlaySFX(34, "madness");
    }

    UIManager.Instance.UpdateExpPanel();
    UIManager.Instance.UpdateSkillLevel();
  }
  /// <summary>
  /// 0:A 1:B
  /// </summary>
  /// <param name="exp"></param>
  /// <param name="index"></param>
  public void AddExp_Short(Experience exp,bool index)
  {
    UIManager.Instance.SetInfoPanel(string.Format(GetTextData("GainExp"), exp.Name));
    exp.Duration = ConstValues.EXPMaxTurn_short_idle;
    if (index == true)
    {
      MyGameData.ShortExp_A = exp;

      if (MyGameData.Madness_Intelligence)
      {
        if (MyGameData.LongExp != null)
        {
          MyGameData.LongExp.Duration -= ConstValues.MadnessEffect_Intelligence;
          UIManager.Instance.UpdateExpMad(0);
        }
        if (MyGameData.ShortExp_B != null)
        {
          MyGameData.ShortExp_B.Duration -= ConstValues.MadnessEffect_Intelligence;
          UIManager.Instance.UpdateExpMad(2);
        }

        UIManager.Instance.HighlightManager.Highlight_Madness(SkillTypeEnum.Intelligence);
        UIManager.Instance.AudioManager.PlaySFX(34, "madness");
      }
    }
    else
    {
      MyGameData.ShortExp_B = exp;

      if (MyGameData.Madness_Intelligence)
      {
        if (MyGameData.LongExp != null)
        {
          MyGameData.LongExp.Duration -= ConstValues.MadnessEffect_Intelligence;
          UIManager.Instance.UpdateExpMad(0);
        }
        if (MyGameData.ShortExp_A != null)
        {
          MyGameData.ShortExp_A.Duration -= ConstValues.MadnessEffect_Intelligence;
          UIManager.Instance.UpdateExpMad(1);
        }

        UIManager.Instance.HighlightManager.Highlight_Madness(SkillTypeEnum.Intelligence);
        UIManager.Instance.AudioManager.PlaySFX(34, "madness");
      }
    }

    UIManager.Instance.UpdateExpPanel();
    UIManager.Instance.UpdateSkillLevel();
  }
  public void SetEvent(EventData eventdata)
  {
    MyGameData.CurrentEvent = eventdata;
    MyGameData.CurrentEventSequence = EventSequence.Progress;
    if (MyGameData.CurrentEventLine == "" && eventdata.EventLine != "")
    {
      MyGameData.CurrentEventLine = eventdata.EventLine;
    }
    else if (MyGameData.CurrentEventLine !=""&& MyGameData.CurrentEventLine == eventdata.EventLine && eventdata.EndingID != "")
      MyGameData.CurrentEventLine = "";

    UIManager.Instance.OpenDialogue_Event(false);

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
    if (instance == null)
    {
      instance = this;
      if (PlayerPrefs.GetInt("LanguageIndex", -1) == -1)
      {
        SystemLanguage _lang = Application.systemLanguage;
        PlayerPrefs.SetInt("LanguageIndex", (int)_lang);
      }
      EventDataUpdate();
      DontDestroyOnLoad(gameObject);
      //  DebugAllEvents();
    }
    else { Destroy(this); }

  }
  private void OnApplicationQuit()
  {
   // 기존 스팀웍스 끄기 기능
  }
  private void Start()
  {
    UIManager.Instance.AudioManager.PlayBGM();
  }

  private void Update()
  {
#if UNITY_EDITOR
    if (Input.GetKeyDown(KeyCode.Backspace))
    {
         MyGameData = new GameData(QuestType.Cult);
      FindObjectOfType<maptext>().GetComponent<maptext>().CreateMap();
   //   CreatMapForDebug();
    //  Debug.Log(MyGameData.CurrentEvent != null);
    }

    /*
    if (Input.GetKeyDown(KeyCode.Space) && SteamManager.Initialized)
    {
      Debug.Log("레후");

      SteamUserStats.ClearAchievement("Achievmenttest0");

      SteamUserStats.StoreStats();
    }
    */

    if (Input.GetKeyDown(KeyCode.KeypadEnter))
      UIManager.Instance.SetInfoPanel("레후");
#endif
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
          _description = GetTextData("GameOver_Force");
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
    DeleteSaveData();
    UIManager.Instance.OpenDead(_illust, _description);
  }
  public void DeleteSaveData()
  {
    if(System.IO.File.Exists(Application.persistentDataPath + "/" + GameDataName)) System.IO.File.Delete(Application.persistentDataPath + "/" + GameDataName);
  }
  public void SubEnding(EndingDatas endingdata)
  {
    UIManager.Instance.OpenEnding(endingdata);
  }

  public void StartNewGame(QuestType newquest)
  {
    UIManager.Instance.AddUIQueue(startnewgame(newquest));
  }
  private IEnumerator startnewgame(QuestType newquest)
  {
    MyGameData = new GameData(newquest);//새로운 게임 데이터 생성

    yield return StartCoroutine(createnewmap(true));//새 맵 만들기

    switch (MyGameData.QuestType)
    {
      case QuestType.Cult: UIManager.Instance.CultUI.OpenUI_Prologue((QuestHolder_Cult)MyGameData.CurrentQuestData); break;
    }

    MyGameData.MyMapData.SetEventTiles();
    UIManager.Instance.UpdateAllUI();

    yield return StartCoroutine(UIManager.Instance.opengamescene());

    IsPlaying = true;
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
    //    Debug.Log(JsonUtility.ToJson(GameSaveData));
    //    Debug.Log(JsonUtility.ToJson(new GameJsonData(new GameData(new GameJsonData(MyGameData)))));
    MyGameData = new GameData(GameSaveData);

    yield return new WaitUntil(()=>MyGameData!=null);

    yield return StartCoroutine(UIManager.Instance.MapUI.MapCreater.MakeTilemap());
    UIManager.Instance.UpdateMap_SetPlayerPos();
    UIManager.Instance.UpdateAllUI();

    yield return StartCoroutine(UIManager.Instance.opengamescene());

    string _eventid = MyGameData.CurrentEvent==null?null: MyGameData.CurrentEvent.ID;

    if (_eventid != null)
    {
      if (MyGameData.CurrentEventSequence == EventSequence.Progress)
      {
        UIManager.Instance.AddUIQueue(UIManager.Instance.DialogueUI.OpenEventUI(false));
      }
      else
      {
        bool _issuccess = false;
        bool _isleft = false;
        if (MyGameData.SuccessEvent_Rational.Contains(_eventid) || MyGameData.SuccessEvent_Mental.Contains(_eventid) || MyGameData.SuccessEvent_None.Contains(_eventid))
        {
          _issuccess = true;
          _isleft = true;
        }
        else if (MyGameData.SuccessEvent_Physical.Contains(_eventid) || MyGameData.SuccessEvent_Material.Contains(_eventid))
        {
          _issuccess = true;
          _isleft = false;
        }
        else if (MyGameData.FailEvent_Rational.Contains(_eventid) || MyGameData.FailEvent_Mental.Contains(_eventid) || MyGameData.FailEvent_None.Contains(_eventid))
        {
          _issuccess = false;
          _isleft = true;
        }
        else if (MyGameData.FailEvent_Physical.Contains(_eventid) || MyGameData.FailEvent_Material.Contains(_eventid))
        {
          _issuccess = false;
          _isleft = false;
        }
        UIManager.Instance.AddUIQueue(UIManager.Instance.DialogueUI.OpenEventUI(_issuccess, _isleft, false));
      }
    }
    else if(MyGameData.CurrentTile.TileSettle!=null)
    {
      UIManager.Instance.AddUIQueue(UIManager.Instance.DialogueUI.openui_settlement(true));
    }
    else
    {
      UIManager.Instance.MapUI.OpenUI(false);
    }

    IsPlaying = true;

    if (MyGameData.Sanity <= 0) UIManager.Instance.GetMad();

    yield return null;
  }
  public void CreatMapForDebug() => StartCoroutine(createnewmap(false));

  private IEnumerator createnewmap(bool realperfect)
  {
    maptext _map = FindObjectOfType<maptext>().GetComponent<maptext>();

    yield return StartCoroutine(_map.makeperfectmap(realperfect));

    yield return new WaitUntil(()=>MyGameData.MyMapData != null);

    List<TileData> _randomstartlands = new List<TileData>();
    TileData _villagetile = MyGameData.MyMapData.Villages[UnityEngine.Random.Range(0, MyGameData.MyMapData.Villages.Count)].Tile;
    _randomstartlands = MyGameData.MyMapData.GetAroundTile(_villagetile, 3);
    List<TileData> _disabletiles=new List<TileData>();
    foreach (var _tile in _randomstartlands)
    {
      if (!_tile.Interactable) { _disabletiles.Add(_tile); }
      if (_tile.TileSettle != null) { _disabletiles.Add(_tile); }
    }
    foreach(var _deletetile in _disabletiles)_randomstartlands.Remove(_deletetile);
    foreach (var _aroundtiles in MyGameData.MyMapData.GetAroundTile(_villagetile, 2)) _randomstartlands.Remove(_aroundtiles);

    MyGameData.Coordinate = _randomstartlands[UnityEngine.Random.Range(0, _randomstartlands.Count)].Coordinate;

    foreach (var _tile in MyGameData.MyMapData.GetAroundTile(MyGameData.CurrentTile, MyGameData.ViewRange))
      _tile.Fogstate = 2;

    yield return StartCoroutine(_map.MakeTilemap());
    UIManager.Instance.UpdateMap_SetPlayerPos();
    yield return null;
  }
  public void EnterSettlement(Settlement targetsettlement)
  {
    if (MyGameData.Supply < 0) MyGameData.Supply = 0;
    MyGameData.CurrentEvent = null;
    MyGameData.CurrentSettlement=targetsettlement;

    switch (MyGameData.QuestType)
    {
      case QuestType.Cult:
        switch (MyGameData.Quest_Cult_Phase)
        {
          case 0:
            if(targetsettlement.SettlementType==SettlementType.Village) UIManager.Instance.CultUI.AddProgress(2, null);
            break;
          case 1:
            if (targetsettlement.SettlementType == SettlementType.Town) UIManager.Instance.CultUI.AddProgress(2, null);
            break;
          case 2:
            if (targetsettlement.SettlementType == SettlementType.City) UIManager.Instance.CultUI.AddProgress(2, null);
            break;
        }
        break;
    }
    UIManager.Instance.AddUIQueue(UIManager.Instance.DialogueUI.openui_settlement(false));
  }

  [System.Serializable]
  public class Textdata
  {
    public string ID = "";
    public string kor = "";
    public string en = "";
      public string Text
    {
      get
      {
        int _languagecode = PlayerPrefs.GetInt("LanguageIndex");
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
            return en;
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
