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
  [SerializeField] private TextAsset TraitData = null;  //특성 Json
  public EventHolder EventHolder = new EventHolder();                               //이벤트 저장할 홀더
  public Dictionary<string, Experience> ExpDic = new Dictionary<string, Experience>();  //경험 딕셔너리
  public Dictionary<string, Trait> TraitsDic = new Dictionary<string, Trait>();         //특성 딕셔너리

  public void LoadData()
  {
    Dictionary<string, EventJsonData> _eventjson = new Dictionary<string, EventJsonData>();
    _eventjson = JsonConvert.DeserializeObject<Dictionary<string, EventJsonData>>(NormalEventData.text);
    foreach (var _data in _eventjson) EventHolder.ConvertData_Normal(_data.Value);
    //이벤트 Json -> EventHolder

    Dictionary<string, FollowEventJsonData> _followeventjson = new Dictionary<string, FollowEventJsonData>();
    _followeventjson = JsonConvert.DeserializeObject<Dictionary<string, FollowEventJsonData>>(FollowEventData.text);
    foreach(var _data in _followeventjson) EventHolder.ConvertData_Follow(_data.Value);
    //연계 이벤트 Json -> EventHolder

    Dictionary<string,QuestEventDataJson> _questeventjson = new Dictionary<string, QuestEventDataJson>();
    _questeventjson = JsonConvert.DeserializeObject<Dictionary<string, QuestEventDataJson>>(QuestEventData.text);
    foreach( var _data in _questeventjson) EventHolder.ConvertData_Quest(_data.Value);
    //퀘스트 Json -> EventHolder

    Dictionary<string,ExperienceJsonData> _expjson = new Dictionary<string,ExperienceJsonData>();
    _expjson = JsonConvert.DeserializeObject<Dictionary<string, ExperienceJsonData>>(EXPData.text);
    foreach(var _data in _expjson) 
    { 
      Experience _exp = _data.Value.ReturnEXPClass();
      ExpDic.Add(_data.Key, _exp);
    }
    //경험 Json -> EXPDic

    Dictionary<string,TraitJsonData> _traitjson = new Dictionary<string,TraitJsonData>();
    _traitjson = JsonConvert.DeserializeObject<Dictionary<string,TraitJsonData>>(TraitData.text);
    foreach(var _data in _traitjson)
    {
      Trait _trait=_data.Value.ReturnTraitClass();
      TraitsDic.Add(_data.Key, _trait);
    }
    //특성 Json -> TraitDic
    //일단 데이터 불러오기는 나중에 만들것
    MyGameData = new GameData();

    EventHolder.LoadAllEvents();

  }//각종 Json 가져와서 변환

  private void Awake()
  {
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
      string _str = $"정착지 이름 : {_settle.Name}  정착지 환경: ";
      if (_settle.IsForest) _str += "숲 ";
      if (_settle.IsRiver) _str += "강 ";
      if (_settle.IsHighland) _str += "언덕 ";
      if (_settle.IsMountain) _str += "산 ";
      if (_settle.IsSea) _str += "바다 ";
      _str += "\n";
      _str += $"마을 레벨 : {_settle.Wealth}  시장 레벨 : {_settle.Wealth}  사원 레벨 : {_settle.Faith}  ";
      switch (_settle.Type)
      {
        case SettlementType.Town:
          break;
        case SettlementType.City:
          _str += $"도서관 레벨 : {_settle.Culture}";
          break;
        case SettlementType.Castle:
          _str += $"극장 레벨 : {_settle.Culture} 아카데미 레벨 : {_settle.Science}";
          break;
      }
      _str += "\n\n";
      TargetTileEventData _tiledata = _settle.GetSettleTileEventData();
      List<EventDataDefulat> _results = EventHolder.ReturnEvent(_tiledata);
      foreach (var _event in _results)
      {
        _str += $"이벤트 이름 : {_event.Name} 이벤트 종류 : ";
        _str += _event.GetType().Equals(typeof(QuestEventData)) ? "퀘스트 " : _event.GetType().Equals(typeof(FollowEventData)) ? "연계 " : "일반 ";
        _str += "\n";
        _str += $"등장 장소 : {_event.PlaceType}  등장 레벨 : {(_event.PlaceLevel == 0 ? "전역" : _event.PlaceLevel == 1 ? "1" : _event.PlaceLevel == 2 ? 2 : 3)}";
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
    AsyncOperation _oper = SceneManager.LoadSceneAsync(scenename);
    _oper.allowSceneActivation = true;

    yield return new WaitUntil(()=> _oper.isDone==true);
    CreateNewMap();
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
    MyGameData.CurrentPos = _startsettle.VectorPos();

    MyGameData.AvailableSettlement = MyMapData.GetCloseSettles(_startsettle, 3);
    foreach (Settlement _settle in MyGameData.AvailableSettlement) _settle.IsOpen = true;

    _map.MakeTilemap(MyMapSaveData, MyMapData);


    UIManager.Instance.UpdateMap_SetPlayerPos(_startsettle);
    yield return null;
  }
}
