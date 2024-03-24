using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataSender : MonoBehaviour
{
  private const string URL = "";
    public void SendData(string eventid,bool isleft)
  {
  }
  private IEnumerator Post(string eventid,bool isleft)
  {
    WWWForm _form = new WWWForm();
    _form.AddField("", eventid);
    _form.AddField("", isleft ? 0 : 1);
    byte[] _rawData = _form.data;
    WWW _www = new WWW(URL, _rawData);
    yield return _www;
  }
}
