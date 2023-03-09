using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using Newtonsoft.Json;

public class TestScript : MonoBehaviour
{
  public TextAsset asdf=null;

  private void Start()
  {
    string textdata = asdf.text;
   Dictionary<string,Dictionary<string,Dictionary<string,int>>> dic = new Dictionary<string,Dictionary<string,Dictionary<string,int>>>();
    dic=JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, int>>>>(textdata);
 
    foreach(var _sheet in dic)
    {
      Debug.Log($"Sheet : {_sheet.Key}");
      foreach(var _name in _sheet.Value)
      {
        Debug.Log($"Name : {_name.Key}");
        foreach(var _data in _name.Value)
        {
          Debug.Log($"data.key : {_data.Key}");
          Debug.Log($"data.value : {_data.Value}");
        }
      }
      Debug.Log("\n");
    }
  }

}
public class LehooTest
{
  public int b, c;
}
