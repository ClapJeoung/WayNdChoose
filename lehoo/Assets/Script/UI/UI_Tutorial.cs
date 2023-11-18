using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tutorial : MonoBehaviour
{
  public CanvasGroup Tutorial_Map = null;
  public TextMeshProUGUI Map_Hello = null;
  public TextMeshProUGUI Map_Progress = null;
  public TextMeshProUGUI Map_CultInfo = null;
  public TextMeshProUGUI Map_MoveCost = null;
  public TextMeshProUGUI Map_Settlement=null;
  public void OpenTutorial_Map()
  {
    var _text = GameManager.Instance.GetTextData("Tutorial_Map").Split('@');
    Map_Hello.text = _text[0];
    Map_Progress.text = _text[1];
    Map_CultInfo.text= _text[2];
    Map_MoveCost.text = string.Format(_text[3],ConstValues.Movecost_GoldValue*100);
    Map_Settlement.text = _text[4];
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
  public TextMeshProUGUI Settlement_Effect = null;
  public TextMeshProUGUI Settlement_Discomfort = null;
  public void OpenTutorial_Settlement()
  {
    var _text = GameManager.Instance.GetTextData("Tutorial_Settlement").Split('@');
    Settlement_Hello.text = _text[0];
    Settlement_Effect.text = string.Format(_text[1],ConstValues.RestSanityRestore);
    Settlement_Discomfort.text = _text[2];
    StartCoroutine(UIManager.Instance.ChangeAlpha(Tutorial_Settlement, 1.0f, 0.5f));
  }
  public void CloseTutorial_Settlement()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(Tutorial_Settlement, 0.0f, 0.3f));
    PlayerPrefs.SetInt("Tutorial_Settlement", 1);
  }
  [Space(10)]
  public CanvasGroup Tutorial_Event = null;
  public TextMeshProUGUI Event_Hello = null;
  public TextMeshProUGUI Event_Selections = null;
  public TextMeshProUGUI Event_NoSelection = null;
  public TextMeshProUGUI Event_Tendency = null;
  public TextMeshProUGUI Event_Cost = null;
  public void OpenTutorial_Event()
  {
    var _text = GameManager.Instance.GetTextData("Tutorial_Event").Split('@');
    Event_Hello.text = _text[0];
    Event_Selections.text = _text[1];
    Event_NoSelection.text = _text[2];
    Event_Tendency.text = _text[3];
    Event_Cost.text = _text[4];
    StartCoroutine(UIManager.Instance.ChangeAlpha(Tutorial_Event, 1.0f, 0.5f));
  }
  public void CloseTutorial_Event()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(Tutorial_Event, 0.0f, 0.3f));
    PlayerPrefs.SetInt("Tutorial_Event", 1);
  }

}
