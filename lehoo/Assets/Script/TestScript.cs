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

public class lehoo:MonoBehaviour
{
  public virtual void asdf()
  {
    Debug.Log("asdf");
  }
  public void techa()
  {
    asdf();
  }
}
public class TestScript : lehoo
{
  public override void asdf()
  {
    base.asdf();
    Debug.Log("asdf°­È­ÆÇ");
  }
  private void Awake()
  {
    techa();
  }
}
