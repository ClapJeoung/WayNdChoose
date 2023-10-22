using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tutorial : MonoBehaviour
{
  public CanvasGroup Tutorial_Map = null;
  public TextMeshProUGUI Map_Hello = null;
  public TextMeshProUGUI Map_Progress = null;
  public TextMeshProUGUI Map_MoveCost = null;
  public TextMeshProUGUI Map_Settlement=null;
  public TextMeshProUGUI Map_Event = null;
  public void OpenTutorial_Map()
  {
    var _text = GameManager.Instance.GetTextData("Tutorial_Map").Split('@');
    Map_Hello.text = _text[0];
    Map_Progress.text = _text[1];
    Map_MoveCost.text = _text[2];
    Map_Settlement.text = _text[3];
    Map_Event.text = _text[4];
    StartCoroutine(UIManager.Instance.ChangeAlpha(Tutorial_Map, 1.0f, 0.5f));
  }
  public void CloseTutorial_Map()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(Tutorial_Map, 0.0f, 0.3f));
    PlayerPrefs.SetInt("Tutorial_Map", 1);
  }
  [Space(10)]
  public CanvasGroup Tutorial_Settlement = null;
  public TextMeshProUGUI Settlement_Hello = null;
  public TextMeshProUGUI Settlement_Rest = null;
  public TextMeshProUGUI Settlement_Effect = null;
  public TextMeshProUGUI Settlement_Discomfort = null;
  public TextMeshProUGUI Settlement_Event = null;
  public void OpenTutorial_Settlement()
  {
    var _text = GameManager.Instance.GetTextData("Tutorial_Settlement").Split('@');
    Settlement_Hello.text = _text[0];
    Settlement_Rest.text = _text[1];
    Settlement_Effect.text = _text[2];
    Settlement_Discomfort.text = _text[3];
    Settlement_Event.text = _text[4];
    StartCoroutine(UIManager.Instance.ChangeAlpha(Tutorial_Settlement, 1.0f, 0.5f));
  }
  public void CloseTutorial_Settlement()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(Tutorial_Settlement, 0.0f, 0.3f));
    PlayerPrefs.SetInt("Tutorial_Settlement", 1);
  }
}
