using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class EventManager : MonoBehaviour
{
  [SerializeField] private TextAsset EventJsonData = null;
  private static EventManager instance;
  [HideInInspector] public EventHolder EventHolder=new EventHolder();
  public static EventManager Instance {get {return instance;}}
  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
   //   LoadEvent();
    }
    else Destroy(gameObject);
  }
  public void LoadEvent()
  {
    Dictionary<string,EventJsonData> jsonData = new Dictionary<string,EventJsonData>();
    jsonData = JsonConvert.DeserializeObject<Dictionary<string, EventJsonData>>(EventJsonData.text);
    foreach(var _data in jsonData)EventHolder.AddData(_data.Key, _data.Value);
  }
}
