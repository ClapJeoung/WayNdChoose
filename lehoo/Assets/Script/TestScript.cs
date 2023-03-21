using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using Newtonsoft.Json;
using Google.Apis.Auth;
using Google.Apis.Sheets;
using System.Data;
using System.IO;
using UnityEngine.EventSystems;

public class TestScript : MonoBehaviour
{
  [SerializeField] private TextAsset MyJson = null;
  private void Awake()
  {
    Dictionary<string,lehoojson> _lehoo= new Dictionary<string,lehoojson>();
    _lehoo = JsonConvert.DeserializeObject<Dictionary<string, lehoojson>>(MyJson.text);
    foreach (var _data in _lehoo)
      Debug.Log(_data.Value.qwer);
  }
}
public class lehoojson
{
  public string qwer;
}