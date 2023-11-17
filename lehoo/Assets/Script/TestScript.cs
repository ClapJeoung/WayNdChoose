using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class TestScript:MonoBehaviour
{
  public RectTransform TestRect = null;

  private void Update()
  {
    if (Input.GetKey(KeyCode.End))
    {
      Debug.Log(TestRect.name);
      Debug.Log(TestRect.GetComponent<CanvasRenderer>().cull);
    }
  }
}
