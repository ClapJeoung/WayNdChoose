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

public class TestScript : MonoBehaviour,IPointerEnterHandler
{
  public void OnPointerEnter(PointerEventData _data)
  {
    Debug.Log("·¹ÈÄ~");
  }
  public void ClickEvent()
  {
    Debug.Log("Å×Ã­");
  }
}