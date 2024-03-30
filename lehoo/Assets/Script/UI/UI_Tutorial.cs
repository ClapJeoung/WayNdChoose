using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UI_Tutorial : MonoBehaviour
{
  #region Æ©Åä_ÁøÇàµµ
  [SerializeField] private CanvasGroup Tutorial_Cult = null;
  [SerializeField] private TextMeshProUGUI Cult_Hello = null;
  [SerializeField] private VideoPlayer Cult_Video_0 = null;
  [SerializeField] private TextMeshProUGUI Cult_Progress_0 = null;
  [SerializeField] private VideoPlayer Cult_Video_1 = null;
  [SerializeField] private TextMeshProUGUI Cult_Progress_1 = null;

  public void OpenTutorial_Cult()
  {
    var _text = GameManager.Instance.GetTextData("Tutorial_Cult").Split('@');
    Cult_Hello.text = _text[0];
    Cult_Progress_0.text = _text[1];
    Cult_Progress_1.text = _text[2];
    Cult_Video_0.Play();
    Cult_Video_1.Play();
    StartCoroutine(UIManager.Instance.ChangeAlpha(Tutorial_Cult, 1.0f, 0.5f));
  }
  public void CloseTutorial_Cult()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(Tutorial_Cult, 0.0f, 0.3f));
    Cult_Video_0.Stop();
    Cult_Video_1.Stop();
    PlayerPrefs.SetInt("Tutorial_Cult", 1);
  }
  #endregion
  #region Æ©Åä_Áöµµ
  [Space(10)]
  [SerializeField] private CanvasGroup Tutorial_Map = null;
  [SerializeField] private TextMeshProUGUI Map_Hello = null;
  [SerializeField] private CanvasGroup Map_Move_Group = null;
  [SerializeField] private Button Map_Move_Button = null;
  [SerializeField] private TextMeshProUGUI Map_Move_ButtonText = null;
  [SerializeField] private VideoPlayer Map_Move_Video = null;
  [SerializeField] private TextMeshProUGUI Map_Move_Text = null;
  [SerializeField] private CanvasGroup Map_Resource_Group = null;
  [SerializeField] private Button Map_Resource_Button = null;
  [SerializeField] private TextMeshProUGUI Map_Resource_ButtonText = null;
  [SerializeField] private VideoPlayer Map_Resource_Video_0 = null;
  [SerializeField] private VideoPlayer Map_Resource_Video_1 = null;
  [SerializeField] private TextMeshProUGUI Map_Resource_Text = null;
  public void OpenTutorial_Map()
  {
    var _text = GameManager.Instance.GetTextData("Tutorial_Map").Split('@');
    Map_Hello.text = _text[0];
    Map_Move_ButtonText.text = _text[1];
    Map_Move_Text.text = _text[2];
    Map_Resource_ButtonText.text = _text[3];
    Map_Resource_Text.text = _text[4];
    StartCoroutine(UIManager.Instance.ChangeAlpha(Tutorial_Map, 1.0f, 0.5f));
    OpenMove();
  }
  public void OpenMove()
  {
    Map_Resource_Group.alpha = 0.0f;
    Map_Resource_Video_0.Stop();
    Map_Resource_Video_1.Stop();
    Map_Resource_Button.interactable = true;
    Map_Move_Button.interactable = false;
    Map_Move_Group.alpha = 1.0f;
    Map_Move_Video.Play();
  }
  public void OpenResource()
  {
    Map_Resource_Group.alpha = 1.0f;
    Map_Resource_Video_0.Play();
    Map_Resource_Video_1.Play();
    Map_Resource_Button.interactable = false;
    Map_Move_Button.interactable = true;
    Map_Move_Group.alpha = 0.0f;
    Map_Move_Video.Stop();
  }
  public void CloseTutorial_Map()
  {
    Map_Move_Video.Stop();
    Map_Resource_Video_0.Stop();
    Map_Resource_Video_1.Stop();
    StartCoroutine(UIManager.Instance.ChangeAlpha(Tutorial_Map, 0.0f, 0.3f));
    PlayerPrefs.SetInt("Tutorial_Map", 1);
  }
  #endregion
  #region Æ©Åä_Á¤ÂøÁö
  [Space(10)]
  public CanvasGroup Tutorial_Settlement = null;
  public TextMeshProUGUI Settlement_Hello = null;
  public VideoPlayer Settlement_Video = null;
  public TextMeshProUGUI Settlement_Text = null;
  public void OpenTutorial_Settlement()
  {
    var _text = GameManager.Instance.GetTextData("Tutorial_Settlement").Split('@');
    Settlement_Hello.text = _text[0];
    Settlement_Text.text = _text[1];
    Settlement_Video.Play();
    StartCoroutine(UIManager.Instance.ChangeAlpha(Tutorial_Settlement, 1.0f, 0.5f));
  }
  public void CloseTutorial_Settlement()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(Tutorial_Settlement, 0.0f, 0.3f));
    Settlement_Video.Stop();
    PlayerPrefs.SetInt("Tutorial_Settlement", 1);
  }
  #endregion
  [Space(10)]
  [SerializeField] private CanvasGroup Tutorial_Event = null;
  [SerializeField] private TextMeshProUGUI Event_Hello = null;
  [SerializeField] private CanvasGroup Event_Selection_Group = null;
  [SerializeField] private Button Event_Selection_Button = null;
  [SerializeField] private TextMeshProUGUI Event_Selection_ButtonText = null;
  [SerializeField] private VideoPlayer Event_Selection_Video = null;
  [SerializeField] private TextMeshProUGUI Event_Selection_Text = null;
  [SerializeField] private CanvasGroup Event_Exp_Group = null;
  [SerializeField] private Button Event_Exp_Button = null;
  [SerializeField] private TextMeshProUGUI Event_Exp_ButtonText = null;
  [SerializeField] private VideoPlayer Event_Exp_Video = null;
  [SerializeField] private TextMeshProUGUI Event_Exp_Text = null;
  public void OpenTutorial_Event()
  {
    var _text = GameManager.Instance.GetTextData("Tutorial_Event").Split('@');
    Event_Hello.text = _text[0];
    Event_Selection_ButtonText.text = _text[1];
    Event_Selection_Text.text = _text[2];
    Event_Exp_ButtonText.text = _text[3];
    Event_Exp_Text.text = _text[4];
    OpenSelection();
    StartCoroutine(UIManager.Instance.ChangeAlpha(Tutorial_Event, 1.0f, 0.5f));
  }
  public void OpenSelection()
  {
    Event_Selection_Group.alpha = 1.0f;
    Event_Selection_Video.Play();
    Event_Selection_Button.interactable = false;
    Event_Exp_Group.alpha = 0.0f;
    Event_Exp_Video.Stop();
    Event_Exp_Button.interactable = true;
  }
  public void OpenExp()
  {
    Event_Selection_Group.alpha = 0.0f;
    Event_Selection_Video.Stop();
    Event_Selection_Button.interactable = true;
    Event_Exp_Group.alpha = 1.0f;
    Event_Exp_Video.Play();
    Event_Exp_Button.interactable = false;
  }
  public void CloseTutorial_Event()
  {
    Event_Selection_Video.Stop();
    Event_Exp_Video.Stop();
    StartCoroutine(UIManager.Instance.ChangeAlpha(Tutorial_Event, 0.0f, 0.3f));
    PlayerPrefs.SetInt("Tutorial_Event", 1);
  }

}
