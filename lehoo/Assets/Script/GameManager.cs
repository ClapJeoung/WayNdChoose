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

  [HideInInspector] public GameData MyGameData = null;            //���� ������(���൵,���� ���� �� �̺�Ʈ, ���� �� ����,����Ʈ ���)
  private const string GameDataName = "GameData.json";
  [HideInInspector] public MapData MyMapData = null;              //�� ������(�� ������)
  private MapSaveData MyMapSaveData = null;
  private const string MapDataName = "MapData.json";

  [SerializeField] private ImageHolder ImageHolder = null;             //�̺�Ʈ,����,Ư��,������ �Ϸ���Ʈ Ȧ��

  [SerializeField] private TextAsset EventData = null;  //�̺�Ʈ Json
  [SerializeField] private TextAsset EXPData = null;    //���� Json
  [SerializeField] private TextAsset TraitData = null;  //Ư�� Json
  public EventHolder EventHolder = new EventHolder();                               //�̺�Ʈ ������ Ȧ��
  public Dictionary<string, Experience> ExpDic = new Dictionary<string, Experience>();  //���� ��ųʸ�
  public Dictionary<string, Trait> TraitsDic = new Dictionary<string, Trait>();         //Ư�� ��ųʸ�

  public void LoadData()
  {
    Dictionary<string, EventJsonData> _eventjson = new Dictionary<string, EventJsonData>();
    _eventjson = JsonConvert.DeserializeObject<Dictionary<string, EventJsonData>>(EventData.text);
    foreach (var _data in _eventjson) EventHolder.AddData_Normal(_data.Value);
    //�̺�Ʈ Json -> EventHolder

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


  }//���� Json �����ͼ� ��ȯ

  private void Awake()
  {
    if(instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
      LoadData();
            string _str = "";
            foreach(var _data in EventHolder.AvailableNormalEvents)
            {
                _str += $"�̺�Ʈ ID : {_data.ID}\n�̺�Ʈ �̸� : {_data.Name}\n�������� : {_data.PreDescription}\n���� : {_data.Description}\n" +
                    $"\n";
            }
            Debug.Log(_str);
    }
    else Destroy(gameObject);

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
