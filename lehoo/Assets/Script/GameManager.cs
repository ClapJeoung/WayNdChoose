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
  private MapSaveData MyMapSaveData = new MapSaveData();
  private const string MapDataName = "MapData.json";

  [SerializeField] private ImageHolder ImageHolder = null;             //�̺�Ʈ,����,Ư��,������ �Ϸ���Ʈ Ȧ��

  [SerializeField] private TextAsset NormalEventData = null;  //�̺�Ʈ Json
  [SerializeField] private TextAsset FollowEventData = null;  //���� �̺�Ʈ Json
  [SerializeField] private TextAsset QuestEventData = null;   //����Ʈ �̺�Ʈ Json
  [SerializeField] private TextAsset EXPData = null;    //���� Json
  [SerializeField] private TextAsset TraitData = null;  //Ư�� Json
  public EventHolder EventHolder = new EventHolder();                               //�̺�Ʈ ������ Ȧ��
  public Dictionary<string, Experience> ExpDic = new Dictionary<string, Experience>();  //���� ��ųʸ�
  public Dictionary<string, Trait> TraitsDic = new Dictionary<string, Trait>();         //Ư�� ��ųʸ�

  public void LoadData()
  {
    Dictionary<string, EventJsonData> _eventjson = new Dictionary<string, EventJsonData>();
    _eventjson = JsonConvert.DeserializeObject<Dictionary<string, EventJsonData>>(NormalEventData.text);
    foreach (var _data in _eventjson) EventHolder.ConvertData_Normal(_data.Value);
    //�̺�Ʈ Json -> EventHolder

    Dictionary<string, FollowEventJsonData> _followeventjson = new Dictionary<string, FollowEventJsonData>();
    _followeventjson = JsonConvert.DeserializeObject<Dictionary<string, FollowEventJsonData>>(FollowEventData.text);
    foreach(var _data in _followeventjson) EventHolder.ConvertData_Follow(_data.Value);
    //���� �̺�Ʈ Json -> EventHolder

    Dictionary<string,QuestEventDataJson> _questeventjson = new Dictionary<string, QuestEventDataJson>();
    _questeventjson = JsonConvert.DeserializeObject<Dictionary<string, QuestEventDataJson>>(QuestEventData.text);
    foreach( var _data in _questeventjson) EventHolder.ConvertData_Quest(_data.Value);
    //����Ʈ Json -> EventHolder

    Dictionary<string,ExperienceJsonData> _expjson = new Dictionary<string,ExperienceJsonData>();
    _expjson = JsonConvert.DeserializeObject<Dictionary<string, ExperienceJsonData>>(EXPData.text);
    foreach(var _data in _expjson) 
    { 
      Experience _exp = _data.Value.ReturnEXPClass();
      ExpDic.Add(_data.Key, _exp);
    }
    //���� Json -> EXPDic

    Dictionary<string,TraitJsonData> _traitjson = new Dictionary<string,TraitJsonData>();
    _traitjson = JsonConvert.DeserializeObject<Dictionary<string,TraitJsonData>>(TraitData.text);
    foreach(var _data in _traitjson)
    {
      Trait _trait=_data.Value.ReturnTraitClass();
      TraitsDic.Add(_data.Key, _trait);
    }
    //Ư�� Json -> TraitDic
    //�ϴ� ������ �ҷ������ ���߿� �����
    MyGameData = new GameData();

    EventHolder.LoadAllEvents();

  }//���� Json �����ͼ� ��ȯ

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
      _str += $"�̺�Ʈ ID : {_data.ID}\n�̺�Ʈ �̸� : {_data.Name}\n�������� : {_data.PreDescription}\n���� : {_data.Description}\n" +
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
        _str += $"�̺�Ʈ ID : {_rising.ID}\n�̺�Ʈ �̸� : {_rising.Name}\n�������� : {_rising.PreDescription}\n���� : {_rising.Description}\n" +
            $"\n";
      }
      foreach (var _climax in _data.Value.Eventlist_Climax)
      {
        _str += $"�̺�Ʈ ID : {_climax.ID}\n�̺�Ʈ �̸� : {_climax.Name}\n�������� : {_climax.PreDescription}\n���� : {_climax.Description}\n" +
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
