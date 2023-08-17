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

public class lehoo
{
}
public class techa : lehoo
{

}
public class TestScript:MonoBehaviour
{
  public lehoo asdf = null;
  private void Awake()
  {
    techa t = new techa();
    asdf = t;
    qwer(asdf);
  }
  public void qwer(lehoo _techa)
  {
    Debug.Log(_techa.GetType());
  }
}
