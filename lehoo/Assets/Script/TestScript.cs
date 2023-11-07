using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class TestScript:MonoBehaviour
{
  public Color Color_A = new Color();
  public Color Color_B = new Color();
  public Color Color_Result = new Color();
  public string Result_Text = "";

  public float A = 0;
  public float B = 1;

  private void lehu()
  {
    Color_Result = Color.Lerp(Color_A, Color_B, A / B);
    Result_Text = ColorUtility.ToHtmlStringRGB(Color.Lerp(Color_A, Color_B, A / B));
  }
}
