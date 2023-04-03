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
  private MapSaveData MyMapSaveData = new MapSaveData();
  private const string MapDataName = "MapData.json";

  [SerializeField] private ImageHolder ImageHolder = null;             //이벤트,경험,특성,정착지 일러스트 홀더

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
    //  DebugAllEvents();
    }
    else Destroy(gameObject);

  }
  public void DebugAllEvents()
  {
    string _str = "";
    foreach (var _data in EventHolder.AvailableNormalEvents)
    {
      _str += $"이벤트 ID : {_data.ID}\n이벤트 이름 : {_data.Name}\n간략설명 : {_data.PreDescription}\n설명 : {_data.Description}\n" +
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
        _str += $"이벤트 ID : {_rising.ID}\n이벤트 이름 : {_rising.Name}\n간략설명 : {_rising.PreDescription}\n설명 : {_rising.Description}\n" +
            $"\n";
      }
      foreach (var _climax in _data.Value.Eventlist_Climax)
      {
        _str += $"이벤트 ID : {_climax.ID}\n이벤트 이름 : {_climax.Name}\n간략설명 : {_climax.PreDescription}\n설명 : {_climax.Description}\n" +
            $"\n";
      }
    }

    Debug.Log(_str);
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
    maptext _map = FindObjectOfType<maptext>().GetComponent<maptext>();

    MyMapSaveData = _map.MakeMap();
    MyMapData = MyMapSaveData.ConvertToMapData();

    Settlement _startsettle = MyMapData.AllSettles[Random.Range(0, MyMapData.AllSettles.Count)];

    MyGameData.CurrentSettlement = _startsettle;
    MyGameData.CurrentPos = _startsettle.VectorPos();

    MyGameData.AvailableSettlement = MyMapData.GetCloseSettles(_startsettle, 3);
    foreach (Settlement _settle in MyGameData.AvailableSettlement) _settle.IsOpen = true;

    _map.MakeTilemap(MyMapSaveData,MyMapData);


    UIManager.Instance.UpdateMap_PlayerPos(_startsettle);
    UIManager.Instance.SetStartDialogue();
  }
}
